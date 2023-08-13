using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Configuration;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Singletons;

namespace Lakea_Stream_Assistant.EventProcessing
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
            callbacks = new Dictionary<string, EventItem>();
            timers = new Dictionary<string, EventItem>();
            EnumConverter enums = new EnumConverter();
            foreach (ConfigEvent eve in events)
            {
                try
                {
                    EventSource source = enums.ConvertEventSourceString(eve.EventDetails.Source);
                    if (source == EventSource.Lakea)
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
                                Logs.Instance.NewLog(LogLevel.Warning, new Exception("Lakea: Invalid 'EventType' in 'LakeaFunctions' Constructor -> " + type));
                                break;
                        }
                    }
                }
                catch(Exception ex)
                {
                    Console.Error.WriteLine("Lakea: Error Loading Event -> " + eve.EventDetails.Name);
                    Logs.Instance.NewLog(LogLevel.Error, ex);
                }
            }
        }

        //When a callback event is triggered, checks dictionary for event before triggering the events effect
        public void NewCallback(LakeaCallback eve)
        {
            try
            {
                if (callbacks.ContainsKey(eve.Callback.ID))
                {
                    Console.WriteLine("Lakea: Callback -> " + callbacks[eve.Callback.ID].Name);
                    if (callbacks[eve.Callback.ID].UsePreviousArguments)
                    {
                        IDictionary<string, string> args = eve.GetCallbackArguments(callbacks[eve.Callback.ID]);
                        EventItem item = new EventItem(callbacks[eve.Callback.ID], args);
                        processer.ProcessEvent(item);
                    }
                    else
                    {
                        processer.ProcessEvent(callbacks[eve.Callback.ID]);
                    }
                }
                else
                {
                    Console.WriteLine("Lakea: Unrecognised Callback ID -> " + eve.Callback.ID);
                    Logs.Instance.NewLog(LogLevel.Warning, "Unrecognised Callback ID -> " + eve.Callback.ID);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lakea: Callback Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        //Starts internal 1 second ticking timer if there are any timer events
        public void NewTimerStart()
        {
            if (timers.Count > 0)
            {
                if (timers.Count == 1)
                {
                    Console.WriteLine("Lakea: " + timers.Count + " Timer Event Found, Initiliasing Timer...");
                    Logs.Instance.NewLog(LogLevel.Info, timers.Count + " Timer Event Found, Initiliasing Timer...");
                }
                else
                {
                    Console.WriteLine("Lakea: " + timers.Count + " Timer Events Found, Initiliasing Timer...");
                    Logs.Instance.NewLog(LogLevel.Info, timers.Count + " Timer Events Found, Initiliasing Timer...");
                }
                Task.Delay(1000).ContinueWith(t => NewTimerTick());
            }
            else
            {
                Console.WriteLine("Lakea: No Timer Events, Timer Not Initiliased");
                Logs.Instance.NewLog(LogLevel.Info, "No Timer Events, Timer Not Initiliased");
            }
        }

        //Internal timer ticks every second
        public void NewTimerTick()
        {
            foreach (var eve in timers)
            {
                try
                {
                    if (eve.Value.Args["First_Fire"] == "false")
                    {
                        int timeLeft = int.Parse(eve.Value.Args["Timer_Value"]);
                        timeLeft--;
                        if (timeLeft <= 0)
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
                        int timeleft = int.Parse(eve.Value.Args["Timer_Value"]);
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
                catch (Exception ex)
                {
                    Logs.Instance.NewLog(LogLevel.Error, ex);
                }
            }
            Task.Delay(1000).ContinueWith(t => NewTimerTick());
        }
    }
}
