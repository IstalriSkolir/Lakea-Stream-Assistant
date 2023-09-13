using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.EventProcessing.Commands;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;

namespace Lakea_Stream_Assistant.EventProcessing.Processing
{
    // Functions for handling Lakea Events
    public class LakeaFunctions
    {
        private EventInput input;
        private EventProcesser processer;
        private EventPassArguments passArgs;
        private DefaultCommands commands;
        private Dictionary<string, EventItem> callbacks;
        private Dictionary<string, EventItem> timers;

        //Contructor stores list of events to check against when it receives a new event
        public LakeaFunctions(ConfigEvent[] events, EventProcesser processer, EventPassArguments passArgs, DefaultCommands commands, EventInput input)
        {
            this.input = input;
            this.processer = processer;
            this.passArgs = passArgs;
            this.commands = commands;
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
                            case EventType.Lakea_Timer_Start:
                                timers.Add(eve.EventDetails.ID, new EventItem(eve));
                                break;
                            default:
                                Console.WriteLine("Lakea: Invalid 'EventType' in 'LakeaFunctions' Constructor -> " + type);
                                Logs.Instance.NewLog(LogLevel.Warning, new Exception("Lakea: Invalid 'EventType' in 'LakeaFunctions' Constructor -> " + type));
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Terminal.Output("Lakea: Error Loading Event -> " + eve.EventDetails.Name);
                    Logs.Instance.NewLog(LogLevel.Error, ex);
                }
            }

            this.input = input;
        }

        //When a callback event is triggered, checks dictionary for event before triggering the events effect
        public void NewCallback(LakeaCallback eve)
        {
            try
            {
                if (callbacks.ContainsKey(eve.Callback.ID))
                {
                    Terminal.Output("Lakea: Callback -> " + callbacks[eve.Callback.ID].Name);
                    if (callbacks[eve.Callback.ID].UsePreviousArguments)
                    {
                        Dictionary<string, string> args = eve.GetCallbackArguments(callbacks[eve.Callback.ID]);
                        Dictionary<string, string> currentArgs = new Dictionary<string, string>();
                        foreach (var arg in callbacks[eve.Callback.ID].Args)
                        {
                            currentArgs.Add(arg.Key, arg.Value);
                        }
                        foreach (var arg in args)
                        {
                            currentArgs.Add(arg.Key, arg.Value);
                        }
                        EventItem item = new EventItem(callbacks[eve.Callback.ID], currentArgs);
                        item = passArgs.GetEventArgs(item, eve);
                        processer.ProcessEvent(item);
                    }
                    else
                    {
                        EventItem item = callbacks[eve.Callback.ID];
                        item = passArgs.GetEventArgs(item, eve);
                        processer.ProcessEvent(item);
                    }
                }
                else
                {
                    Terminal.Output("Lakea: Unrecognised Callback ID -> " + eve.Callback.ID);
                    Logs.Instance.NewLog(LogLevel.Warning, "Unrecognised Callback ID -> " + eve.Callback.ID);
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Callback Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        //When a command event that is a inbuilt Lake command, call the commands object to call the relevant 'EventItem' for it
        public void NewCommand(LakeaCommand eve)
        {
            try
            {
                EventItem item = commands.NewLakeaCommand(eve);
                if (item != null)
                {
                    processer.ProcessEvent(item);
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Default Command Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        //When a timer event is fired, process the event item it carries
        public void NewTimer(LakeaTimer eve)
        {
            try
            {
                processer.ProcessEvent(eve.EventItem);
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Timer Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        //Start the timers for timed events
        public void NewTimerStart()
        {
            if(timers.Count > 0)
            {
                if (timers.Count == 1)
                {
                    Terminal.Output("Lakea: " + timers.Count + " Timer Event Found, Initiliasing Timer...");
                    Logs.Instance.NewLog(LogLevel.Info, timers.Count + " Timer Event Found, Initiliasing Timer...");
                }
                else
                {
                    Terminal.Output("Lakea: " + timers.Count + " Timer Events Found, Initiliasing Timer...");
                    Logs.Instance.NewLog(LogLevel.Info, timers.Count + " Timer Events Found, Initiliasing Timer...");
                }

                List<Task> tasks = new List<Task>();
                foreach (var timer in timers)
                {
                    try
                    {
                        tasks.Add(Task.Delay(Int32.Parse(timer.Value.Args["Start_Delay"]) * 1000).ContinueWith(t => { timerTick(timer.Key); }));
                    }
                    catch (Exception ex)
                    {
                        Terminal.Output("Lakea: Timer Initilasation Error -> " + timer.Value.Name);
                        Logs.Instance.NewLog(LogLevel.Error, ex);
                    }
                }
            }
            else
            {
                Terminal.Output("Lakea: No Timer Events, Timer Not Initiliased");
                Logs.Instance.NewLog(LogLevel.Info, "No Timer Events, Timer Not Initiliased");
            }
        }

        //When a timer fires, check dictionary for the relevant event and process it
        private void timerTick(string timerID)
        {
            try
            {
                if (timers.ContainsKey(timerID))
                {
                    input.NewEvent(new LakeaTimer(EventSource.Lakea, EventType.Lakea_Timer_Fired, timers[timerID]));
                    Task.Delay(Int32.Parse(timers[timerID].Args["Timer_Delay"]) * 1000).ContinueWith(t => { timerTick(timerID); });
                }
                else
                {
                    Terminal.Output("Lakea: Unrecognised Timer ID -> " + timerID);
                    Logs.Instance.NewLog(LogLevel.Warning, "Unrecognised Timer ID -> " + timerID);
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Timer Error -> " + timerID);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }
    }
}
