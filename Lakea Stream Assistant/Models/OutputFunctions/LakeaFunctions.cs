using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventItems;

namespace Lakea_Stream_Assistant.Models.OutputFunctions
{
    // Functions for handling Lakea Events
    public class LakeaFunctions
    {
        private EventOutputs outputs;
        private IDictionary<string, LakeaEventItem> callbacks = new Dictionary<string, LakeaEventItem>();

        //Contructor stores list of Lakea events to check against when it receives a new event
        public LakeaFunctions(ConfigEvent[] events, EventOutputs outputs) 
        {
            this.outputs = outputs;
            foreach (ConfigEvent eve in  events)
            {
                if ("lakea".Equals(eve.EventDetails.Source.ToLower()))
                {
                    switch (eve.EventDetails.Type.ToLower())
                    {
                        case "callback":
                            callbacks.Add(eve.EventDetails.ID, new LakeaEventItem(eve));
                            break;
                        default:
                            Console.WriteLine("Error Parsing Event '" + eve.EventDetails.Name + "' -> Unrecognised Event Type: " + eve.EventDetails.Type);
                            break;
                    }
                }
            }
        }

        //When a callback event is triggered, checks dictionary for event before triggering the events effect
        public void NewCallback(LakeaCallback eve)
        {
            try
            {
                if (callbacks.ContainsKey(eve.CallbackID))
                {
                    if (callbacks[eve.CallbackID].UsePreviousArguments)
                    {
                        string[] args = eve.GetCallbackArguments(callbacks[eve.CallbackID]);
                        LakeaEventItem item = new LakeaEventItem(callbacks[eve.CallbackID], args);
                        processLakeaEvent(item);
                    }
                    else
                    {
                        processLakeaEvent(callbacks[eve.CallbackID]);
                    }
                }
                else
                {
                    Console.WriteLine("Lakea: Unrecognised Callback ID -> " + eve.CallbackID);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Lakea: Callback Error -> " + e.Message);
            }
        }

        //Checks events target app/platform
        private void processLakeaEvent(LakeaEventItem item)
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
        private void eventTargetOBS(LakeaEventItem item)
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
        private void eventTargetTwitch(LakeaEventItem item)
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
