using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;

namespace Lakea_Stream_Assistant.Models.OutputFunctions
{
    // Functions for handling Lakea Events
    public class LakeaFunctions
    {
        private EventProcesser processer;
        private IDictionary<string, EventItem> callbacks = new Dictionary<string, EventItem>();

        //Contructor stores list of Lakea events to check against when it receives a new event
        public LakeaFunctions(ConfigEvent[] events, EventProcesser processer) 
        {
            this.processer = processer;
            foreach (ConfigEvent eve in  events)
            {
                if ("lakea".Equals(eve.EventDetails.Source.ToLower()))
                {
                    switch (eve.EventDetails.Type.ToLower())
                    {
                        case "lakea_callback":
                            callbacks.Add(eve.EventDetails.ID, new EventItem(eve));
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
                        EventItem item = new EventItem(callbacks[eve.CallbackID], args);
                        processer.ProcessEvent(item);
                    }
                    else
                    {
                        processer.ProcessEvent(callbacks[eve.CallbackID]);
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
    }
}
