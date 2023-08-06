using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events.EventLists;

namespace Lakea_Stream_Assistant.Models.OutputFunctions
{
    //handles stored 'EventItem' and calls the corresponding 'EventOutputs' function
    public class EventProcesser
    {
        private EventOutputs outputs;

        public EventProcesser(EventOutputs outputs) 
        {
            this.outputs = outputs;
        }

        // Receives 'EventItem' object and calls the corresponding 'EventOutputs' function with the relevant arguments
        public void ProcessEvent(EventItem item)
        {
            switch (item.EventGoal)
            {
                case EventGoal.Null:
                    outputs.NullEvent("Null Event -> " + item.Name);
                    break;
                case EventGoal.OBS_Disable_Source:
                    outputs.SetActiveOBSSource(item.Args[0], item.Duration, false, item.Callback);
                    break;
                case EventGoal.OBS_Enable_Source:
                    outputs.SetActiveOBSSource(item.Args[0], item.Duration, true, item.Callback);
                    break;
                case EventGoal.OBS_Enable_Random_Source:
                    outputs.SetRandomActiveOBSSource(item.Args, item.Duration, true, item.Callback);
                    break;
                case EventGoal.OBS_Disable_Random_Source:
                    outputs.SetRandomActiveOBSSource(item.Args, item.Duration, false, item.Callback);
                    break;
                case EventGoal.OBS_Change_Scene:
                    outputs.ChangeOBSScene(item.Args[0], item.Callback);
                    break;
                case EventGoal.Twitch_Send_Chat_Message:
                    outputs.SendTwitchChatMessage(item.Args[0], item.Callback);
                    break;
            }
        }
    }
}
