using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Configuration;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using System;
using TwitchLib.Api.Helix.Models.Soundtrack;

namespace Lakea_Stream_Assistant.Models.OutputFunctions
{
    // Functions for handling Lakea Events
    public class LakeaFunctions
    {
        private EventProcesser processer;
        private IDictionary<string, EventItem> callbacks;
        private IDictionary<string, EventItem> timers;

        //Contructor stores list of events to check against when it receives a new event
        public LakeaFunctions(ConfigEvent[] events, EventProcesser processer) 
        {
            this.processer = processer;
            this.callbacks = new Dictionary<string, EventItem>();
            this.timers = new Dictionary<string, EventItem>();
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
                        case EventType.Lakea_Timer:
                            timers.Add(eve.EventDetails.ID, new EventItem(eve));
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
                    Console.WriteLine("Lakea: Callback -> " + callbacks[eve.CallbackID].Name);
                    if (callbacks[eve.CallbackID].UsePreviousArguments)
                    {
                        IDictionary<string, string> args = eve.GetCallbackArguments(callbacks[eve.CallbackID]);
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

        //Starts internal 1 second ticking timer if there are any timer events
        public void NewTimerStart()
        {
            if(timers.Count > 0)
            {
                if(timers.Count == 1)
                {
                    Console.WriteLine("Lakea: " + timers.Count + " Timer event Found, Initiliasing Timer...");
                }
                else
                {
                    Console.WriteLine("Lakea: " + timers.Count + " Timer events Found, Initiliasing Timer...");
                }
                Task.Delay(1000).ContinueWith(t => NewTimerTick());
            }
            else
            {
                Console.WriteLine("Lakea: No Timer Events, Timer Not Initiliased");
            }
        }

        //Internal timer ticks every second
        public void NewTimerTick()
        {
            foreach(var eve in timers)
            {
                if (eve.Value.Args["First_Fire"] == "false")
                {
                    int timeLeft = Int32.Parse(eve.Value.Args["Timer_Value"]);
                    timeLeft--;
                    if(timeLeft <= 0)
                    {
                        Console.WriteLine("Lakea: Timer -> " + timers[eve.Value.ID].Name);
                        processer.ProcessEvent(timers[eve.Value.ID]);
                        eve.Value.Args["First_Fire"] = "true";
                    }
                    else
                    {
                        eve.Value.Args["Timer_Value"] = timeLeft.ToString();
                    }
                }
                else
                {
                    int timeleft = Int32.Parse(eve.Value.Args["Timer_Value"]);
                    timeleft--;
                    if (timeleft <= 0)
                    {
                        Console.WriteLine("Lakea: Timer -> " + timers[eve.Value.ID].Name);
                        processer.ProcessEvent(timers[eve.Value.ID]);
                        eve.Value.Args["Timer_Value"] = eve.Value.Args["Timer_Delay"];
                    }
                    else
                    {
                        eve.Value.Args["Timer_Value"] = timeleft.ToString();
                    }
                }
            }
            Task.Delay(1000).ContinueWith(t => NewTimerTick());
        }
    }
}
