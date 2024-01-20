using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.EventProcessing.Processing;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;

namespace Lakea_Stream_Assistant.EventProcessing.Misc
{
    public class LakeaCaptured
    {
        private EventInput input;
        private Dictionary<string, string> capturedLines;
        private Dictionary<string, string> retortLines;
        private Dictionary<string, string> releasedLines; 
        private bool isCaught;

        public LakeaCaptured(EventInput eventInput, SettingsCaptures captures)
        {
            input = eventInput;
            isCaught = false;
            capturedLines = new Dictionary<string, string>();
            retortLines = new Dictionary<string, string>();
            releasedLines = new Dictionary<string, string>();
            for(int x = 0; x < captures.CapturedLines.Count(); x++)
            {
                capturedLines.Add("Message" + (x + 1), captures.CapturedLines[x]);
            }
            for(int x = 0; x < captures.RetortLines.Count(); x++)
            {
                retortLines.Add("Message" + (x + 1), captures.RetortLines[x]);
            }
            for(int x = 0; x < captures.ReleasedLines.Count(); x++)
            {
                releasedLines.Add("Message" + (x + 1), captures.ReleasedLines[x]);
            }
        }

        public bool IsCaught { get { return isCaught; } }

        public void LakeaCaught(EventOutputs outputs, int captureDuration)
        {
            Terminal.Output("Lakea: Captured -> True");
            Logs.Instance.NewLog(LogLevel.Info, "Lakea Captured -> True");
            isCaught = true;
            if(capturedLines.Count > 0)
            {
                outputs.SendTwitchRandomChatMessage(capturedLines, null);
            }
            Task.Delay(captureDuration * 1000).ContinueWith(t => timerRelease());
        }

        public void LakeaReleased(EventOutputs outputs)
        {
            Terminal.Output("Lakea: Captured -> False");
            Logs.Instance.NewLog(LogLevel.Info, "Lakea Captured -> False");
            isCaught = false;
            if(releasedLines.Count > 0)
            {
                outputs.SendTwitchRandomChatMessage(releasedLines, null);
            }
        }

        public void Retort(EventOutputs outputs)
        {
            Terminal.Output("Lakea: Captured -> Sending Retort");
            Logs.Instance.NewLog(LogLevel.Info, "Lakea Captured -> Sending Retort");
            if (retortLines.Count > 0)
            {
                outputs.SendTwitchRandomChatMessage(retortLines, null);
            }
        }

        private void timerRelease()
        {
            Terminal.Output("Lakea: Release -> Timer Fired");
            Logs.Instance.NewLog(LogLevel.Info, "Lakea Release -> Timer Fired");
            input.NewEvent(new EventItem(EventSource.Lakea, EventType.Lakea_Released, EventTarget.Lakea, EventGoal.Lakea_Freed, "Lakea Freed"));
        }
    }
}
