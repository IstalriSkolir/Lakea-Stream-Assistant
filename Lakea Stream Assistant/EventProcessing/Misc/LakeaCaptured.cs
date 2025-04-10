using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.EventProcessing.Battle_Simulator;
using Lakea_Stream_Assistant.EventProcessing.Processing;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;

namespace Lakea_Stream_Assistant.EventProcessing.Misc
{
    public class LakeaCaptured
    {
        private EventInput input;
        private LakeaFunctions lakea;
        private BattleFileParser battleFileParser;
        private Random random;
        private string[] retortEvents;
        private bool isCaught;

        private const int tick = 60000;
        private int lakeaDexMod;
        private int characterDexMod;
        private float baseProgress;
        private int minRoll;
        private int maxRoll;
        private float progress;
        private float escapeDC;

        public LakeaCaptured(EventInput eventInput, LakeaFunctions lakeaFunctions, ConfigSettings settings)
        {
            input = eventInput;
            lakea = lakeaFunctions;
            retortEvents = settings.CapturedEventRetorts.EventType;
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
            escapeDC = (float)characterDexMod * float.Parse(args["DifficultyModifier"]);
            Terminal.Output("Lakea: Captured -> True");
            Logs.Instance.NewLog(LogLevel.Info, "Lakea Captured -> True");
            isCaught = true;
            Task.Delay(tick).ContinueWith(t => timedEscapeAttempt());
            //Task.Delay(captureDuration * 1000).ContinueWith(t => timerRelease());
        }

        public EventItem CheckIfCaptured(EventItem item)
        {
            if (!isCaught || (item.EventGoal == EventGoal.Null || item.EventGoal == EventGoal.Lakea_Released) || item.Type == EventType.Lakea_Callback || item.Type == EventType.Lakea_Struggle)
            {
                return item;
            }
            else
            {
                if (retortEvents.Contains(item.Type.ToString()))
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
                    { "EscapeDC", escapeDC.ToString() }
                };
                input.NewEvent(new IncomingEvent(EventSource.Lakea, EventType.Lakea_Struggle, args));
            }
            else
            {
                release();
            }
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
            input.NewEvent(eve);
        }
    }
}
