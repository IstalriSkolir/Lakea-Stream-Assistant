using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventItems;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Models.OutputFunctions;

namespace Lakea_Stream_Assistant.Models.Twitch
{
    // Functions for handling Twitch Events
    public class TwitchFunctions
    {
        private EventOutputs outputs;
        private IDictionary<string, TwitchEventItem> redeems = new Dictionary<string, TwitchEventItem>();

        //Contructor stores list of Twitch events to check against when it receives a new event
        public TwitchFunctions(ConfigEvent[] events, EventOutputs outputs)
        {
            this.outputs = outputs;
            foreach (ConfigEvent eve in events)
            {
                if ("twitch".Equals(eve.EventDetails.Source.ToLower()))
                {
                    switch (eve.EventDetails.Type.ToLower())
                    {
                        case "redeem":
                            redeems.Add(eve.EventDetails.ID, new TwitchEventItem(eve));
                            break;
                        default:
                            Console.WriteLine("Error Parsing Event '" + eve.EventDetails.Name + "' -> Unrecognised Event Type: " + eve.EventDetails.Type);
                            break;
                    }
                }
            }
        }

        //When a channel redeem event is triggered, checks dictionary for event before triggering the events effect
        public void NewRedeem(TwitchRedeem eve)
        {
            try
            {
                if (redeems.ContainsKey(eve.Args.RewardRedeemed.Redemption.Reward.Id))
                {
                    processTwitchEvent(redeems[eve.Args.RewardRedeemed.Redemption.Reward.Id]);
                }
                else
                {
                    Console.WriteLine("Lakea: Unrecognised Channel Redeem -> " + eve.Args.RewardRedeemed.Redemption.Reward.Title + " - " + eve.Args.RewardRedeemed.Redemption.Reward.Id);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Lakea: Twitch Redeem Error -> " + e.Message);
            }
        }

        //Checks events target app/platform
        private void processTwitchEvent(TwitchEventItem item)
        {
            switch (item.EventTarget)
            {
                case EventTarget.Base_Camp:
                    break;
                case EventTarget.OBS:
                    eventTargetOBS(item);
                    break;
                case EventTarget.Twitch:
                    eventTargetTwitch(item);
                    break;
            }
        }

        //Checks events OBS target and calls relevent function in 'outputs' object
        private void eventTargetOBS(TwitchEventItem item)
        {
            switch (item.EventGoal)
            {
                case EventGoal.Disable_OBS_Source:
                    outputs.SetActiveOBSSource(item.Args[0], item.Duration, false, item.Callback);
                    break;
                case EventGoal.Enable_OBS_Source:
                    outputs.SetActiveOBSSource(item.Args[0], item.Duration, true, item.Callback);
                    break;
                case EventGoal.Enable_Random_OBS_Source:
                    outputs.SetRandomActiveOBSSource(item.Args, item.Duration, true, item.Callback);
                    break;
                case EventGoal.Disable_Random_OBS_Source:
                    outputs.SetRandomActiveOBSSource(item.Args, item.Duration, false, item.Callback);
                    break;
                case EventGoal.Change_OBS_Scene:
                    outputs.ChangeOBSScene(item.Args[0], item.Callback);
                    break;
            }
        }

        //Checks events Twitch target and calls relevant function in 'outputs' object
        private void eventTargetTwitch(TwitchEventItem item)
        {
            switch (item.EventGoal)
            {
                case EventGoal.Send_Twitch_Chat_Message:
                    outputs.SendTwitchChatMessage(item.Args[0], item.Callback);
                    break;
            }
        }
    }
}
