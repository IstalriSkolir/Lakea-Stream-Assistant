using Lakea_Stream_Assistant.EventProcessing.Battle_Simulator;
using Lakea_Stream_Assistant.EventProcessing.Processing;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;
using Lakea_Stream_Assistant.Utilities;
using TwitchLib.Api.Helix.Models.ChannelPoints.CreateCustomReward;
using TwitchLib.Api.Helix.Models.ChannelPoints.UpdateCustomReward;

namespace Lakea_Stream_Assistant.EventProcessing.Misc
{
    public class LakeaCaptured
    {
        private LakeaFunctions lakea;
        private BattleFileParser battleFileParser;
        private Random random;
        private RedeemsRetortRedeem retortRedeem;
        private EventType[] retortEvents;
        private EventType[] bypassEvents;
        private string[] redeemIDs;
        private string retortRedeemID;
        private bool isCaught;

        private const int tick = 60000;
        private const float escapeDC = 100;
        private const float minimumProgress = 3.35f;
        private int lakeaDexMod;
        private int characterDexMod;
        private float baseProgress;
        private int minRoll;
        private int maxRoll;
        private float progress;
        private string captorDisplayName;
        private string captorAccountID;

        public LakeaCaptured(LakeaFunctions lakeaFunctions, ConfigSettings settings)
        {
            lakea = lakeaFunctions;
            retortRedeem = settings.Captured.Redeems.RetortRedeem;
            redeemIDs = settings.Captured.Redeems.Disable;
            retortEvents = new EventType[settings.Captured.EventRetorts.Length];
            for (int index = 0; index < retortEvents.Length; index++)
                retortEvents[index] = settings.Captured.EventRetorts[index].ToEnum<EventType>(); 
            EventType[] defaultEventBypasses = { EventType.Lakea_Callback, EventType.Lakea_Struggle };
            EventType[] configBypassEvents = new EventType[settings.Captured.BypassEvents.Length];
            for (int index = 0; index < settings.Captured.BypassEvents.Length; index++)
                configBypassEvents[index] = settings.Captured.BypassEvents[index].ToEnum<EventType>();
            bypassEvents = defaultEventBypasses.Concat(configBypassEvents).ToArray();
            battleFileParser = new BattleFileParser(settings.ResourcePath);
            random = new Random();
            isCaught = false;
        }

        public void LakeaCaught(EventOutputs outputs, Dictionary<string, string> args)
        {
            Dictionary<string, string> characterData = battleFileParser.GetCharacterData(args["AccountID"], args["DisplayName"]);
            characterDexMod = int.Parse(characterData["DEX"]) / 3;
            lakeaDexMod = int.Parse(args["LakeaDexterity"]) / 3;
            baseProgress = float.Parse(args["ProgressBase"]);
            minRoll = int.Parse(args["MinimumRoll"]);
            maxRoll = int.Parse(args["MaximumRoll"]);
            Terminal.Output("Lakea: Captured -> True");
            Logs.Instance.NewLog(LogLevel.Info, "Lakea Captured -> True");
            isCaught = true;
            Task.Delay(tick).ContinueWith(t => timedEscapeAttempt());
            Task.Run(() => { updateRedeems(false); });
            Task.Run(() => { createRetortRedeem(); });
        }

        public EventItem CheckIfCaptured(EventItem item)
        {
            if (!isCaught || (item.EventGoal == EventGoal.Null || item.EventGoal == EventGoal.Lakea_Released) || bypassEvents.Contains(item.Type) )//item.Type == EventType.Lakea_Callback || item.Type == EventType.Lakea_Struggle)
            {
                return item;
            }
            else
            {
                if (retortEvents.Contains(item.Type))
                {
                    Terminal.Output("Lakea: Captured -> Sending Retort");
                    Logs.Instance.NewLog(LogLevel.Info, "Lakea Captured -> Sending Retort");
                    return lakea.LakeaRetort();
                }
                else
                {
                    return null;
                }
            }
        }

        private void timedEscapeAttempt()
        {
            float additional = (float)random.Next(minRoll, maxRoll) * (baseProgress - ((float)characterDexMod / (float)lakeaDexMod));
            if(additional < minimumProgress)
                additional = minimumProgress;
            progress += additional;
            if(progress < escapeDC)
            {
                Terminal.Output("Lakea: Escape Progress -> Progress: " + progress + ", EscapeDC: " + escapeDC);
                Logs.Instance.NewLog(LogLevel.Info, "Lakea Escape Progress -> Progress: " + progress + ", EscapeDC: " + escapeDC);
                Task.Delay(tick).ContinueWith(t => timedEscapeAttempt());
                Dictionary<string, string> args = new Dictionary<string, string>()
                {
                    { "EventID", "Lakea_Struggle" },
                    { "EscapeProgress", progress.ToString() },
                    { "EscapeDC", escapeDC.ToString() },
                    { "CaptorDisplayName", captorDisplayName },
                    { "CaptorAccountID", captorAccountID }
                };
                Task.Run(() =>
                {
                    StreamAssistant.EventHandler.NewEvent(new IncomingEvent(EventSource.Lakea, EventType.Lakea_Struggle, args));
                });
            }
            else
            {
                release();
            }
        }

        private async void updateRedeems(bool enabled)
        {
            foreach(string id in redeemIDs)
            {
                UpdateCustomRewardRequest request = new UpdateCustomRewardRequest();
                request.IsEnabled = enabled;
                await Twitch.UpdateChannelRedeem(id, request);
            }
        }

        private async void createRetortRedeem()
        {
            CreateCustomRewardsRequest request = new CreateCustomRewardsRequest();
            request.Title = retortRedeem.Title;
            request.Prompt = retortRedeem.Description;
            request.Cost = retortRedeem.Cost;
            request.IsEnabled = true;
            request.IsUserInputRequired = false;
            request.ShouldRedemptionsSkipRequestQueue = true;
            CreateCustomRewardsResponse response = await Twitch.CreateChannelRedeem(request);
            retortRedeemID = response.Data[0].Id;
            EventItem item = new EventItem(EventSource.Twitch, EventType.Twitch_Redeem, EventTarget.Lakea, EventGoal.Lakea_Retort, "Lakea Retort", retortRedeemID);
            StreamAssistant.EventHandler.UpdateEventDictionaries(retortRedeemID, item);
        }

        private async void removeRetortRedeem()
        {
            Twitch.DeleteChannelRedeem(retortRedeemID);
            StreamAssistant.EventHandler.UpdateEventDictionaries(retortRedeemID, EventSource.Twitch, EventType.Twitch_Redeem);
        }

        private void release()
        {
            progress = 0;
            Terminal.Output("Lakea: Captured -> False");
            Logs.Instance.NewLog(LogLevel.Info, "Lakea Captured -> False");
            isCaught = false;
            Dictionary<string, string> data = new Dictionary<string, string>()
            {
                { "EventID", "Lakea_Released" }
            };
            IncomingEvent eve = new IncomingEvent(EventSource.Lakea, EventType.Lakea_Released, data);
            Task.Run(() => { updateRedeems(true); });
            Task.Run(() => { removeRetortRedeem(); });
            Task.Run(() => {
                StreamAssistant.EventHandler.NewEvent(eve);
            });
        }
    }
}
