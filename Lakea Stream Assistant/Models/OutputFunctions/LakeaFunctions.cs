using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Configuration;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;
namespace Lakea_Stream_Assistant.Models.OutputFunctions
{
    // Functions for handling Lakea Events
    public class LakeaFunctions
    {
        private EventProcesser processer;
        private IDictionary<string, EventItem> callbacks;

        //Contructor stores list of events to check against when it receives a new event
        public LakeaFunctions(ConfigEvent[] events, EventProcesser processer) 
        {
            this.processer = processer;
            this.callbacks = new Dictionary<string, EventItem>();
            EnumConverter enums = new EnumConverter();
            foreach (ConfigEvent eve in  events)
            {
                EventSource source = enums.ConvertEventSourceString(eve.EventDetails.Source);
                if (source == Enums.EventSource.Lakea)
                {
                    EventType type = enums.ConvertEventTypeString(eve.EventDetails.Type);
                    switch (type)
                    {
                        case EventType.Lakea_Callback:
                            callbacks.Add(eve.EventDetails.ID, new EventItem(eve));
                            break;
                        default:
                            Console.WriteLine("Lakea: Invalid 'EventType' in 'LakeaFunctions' Constructor -> " + type);
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
