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
        private LakeaFunctions lakea;
        private string[] retortEvents;
        private bool isCaught;

        public LakeaCaptured(EventInput eventInput, LakeaFunctions lakeaFunctions, SettingsCapturedEventRetort retorts)
        {
            input = eventInput;
            lakea = lakeaFunctions;
            retortEvents = retorts.EventType;
            isCaught = false;
        }

        public void LakeaCaught(EventOutputs outputs, int captureDuration)
        {
            Terminal.Output("Lakea: Captured -> True");
            Logs.Instance.NewLog(LogLevel.Info, "Lakea Captured -> True");
            isCaught = true;
            Task.Delay(captureDuration * 1000).ContinueWith(t => timerRelease());
        }

        public EventItem CheckIfCaptured(EventItem item)
        {
            if (!isCaught || (item.EventGoal == EventGoal.Null || item.EventGoal == EventGoal.Lakea_Released) || item.Type == EventType.Lakea_Callback)
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

        private void timerRelease()
        {
            Terminal.Output("Lakea: Captured -> False");
            Logs.Instance.NewLog(LogLevel.Info, "Lakea Captured -> False");
            isCaught = false;
            input.NewEvent(new EventItem(EventSource.Lakea, EventType.Lakea_Released, EventTarget.Lakea, EventGoal.Lakea_Released, "Lakea Released", "Lakea_Released"));
        }
    }
}
