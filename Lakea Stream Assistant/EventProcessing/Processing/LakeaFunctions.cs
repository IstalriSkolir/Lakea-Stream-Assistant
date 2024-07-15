using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.EventProcessing.Commands;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;
using System;

namespace Lakea_Stream_Assistant.EventProcessing.Processing
{
    // Functions for handling Lakea Events
    public class LakeaFunctions
    {
        private EventInput input;
        private EventPassArguments passArgs;
        private DefaultCommands commands;
        private Dictionary<EventType, Dictionary<string, EventItem>> events;
        private Dictionary<string, EventItem> callbacks;
        private Dictionary<string, EventItem> timers;
        private Dictionary<string, EventItem> applications;
        private Dictionary<string, EventItem> lakeaReleased;
        private Dictionary<string, EventItem> lakeaRetort;
        private Dictionary<string, EventItem> webSocketEvents;
        private List<EventItem> startupEvents;
        private List<EventItem> shutdownEvents;

        //Contructor stores list of events to check against when it receives a new event
        public LakeaFunctions(ConfigEvent[] newEvents, EventPassArguments passArgs, DefaultCommands commands, EventInput input)
        {
            this.input = input;
            this.passArgs = passArgs;
            this.commands = commands;
            callbacks = new Dictionary<string, EventItem>();
            timers = new Dictionary<string, EventItem>();
            applications = new Dictionary<string, EventItem>();
            lakeaReleased = new Dictionary<string, EventItem>();
            lakeaRetort = new Dictionary<string, EventItem>();
            webSocketEvents = new Dictionary<string, EventItem>();
            events = new Dictionary<EventType, Dictionary<string, EventItem>>();
            events.Add(EventType.Lakea_Callback, callbacks);
            events.Add(EventType.Lakea_Released, lakeaReleased);
            events.Add(EventType.Lakea_Retort, lakeaRetort);
            events.Add(EventType.Lakea_Web_Socket, webSocketEvents);
            startupEvents = new List<EventItem>();
            shutdownEvents = new List<EventItem>();
            EnumConverter enums = new EnumConverter();
            foreach (ConfigEvent eve in newEvents)
            {
                try
                {
                    EventSource source = enums.ConvertEventSourceString(eve.EventDetails.Source);
                    if (source == EventSource.Lakea || source == EventSource.Battle_Simulator)
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
                            case EventType.Battle_Simulator_Encounter:
                            case EventType.Battle_Simulator_Nonencounter:
                                applications.Add(eve.EventDetails.ID, new EventItem(eve));
                                break;
                            case EventType.Lakea_Released:
                                lakeaReleased.Add(eve.EventDetails.ID, new EventItem(eve));
                                break;
                            case EventType.Lakea_Retort:
                                lakeaRetort.Add(eve.EventDetails.ID, new EventItem(eve));
                                break;
                            case EventType.Lakea_Start_Up:
                                startupEvents.Add(new EventItem(eve));
                                break;
                            case EventType.Lakea_Exit:
                                shutdownEvents.Add(new EventItem(eve));
                                break;
                            case EventType.Lakea_Web_Socket:
                                webSocketEvents.Add(eve.EventDetails.ID, new EventItem(eve));
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

        // Update Lakea's events during runtime
        public void UpdateDictionary(string id, EventItem item, bool remove)
        {
            try
            {
                Dictionary<string, EventItem> toUpdate = events[item.Type];
                if (remove)
                {
                    if (toUpdate.ContainsKey(id))
                    {
                        Terminal.Output("Lakea: Removing Lakea Event -> " + toUpdate[id].Name);
                        Logs.Instance.NewLog(LogLevel.Info, "Removing Lakea Event -> " + toUpdate[id].Name);
                        toUpdate.Remove(id);
                    }
                    else
                    {
                        Terminal.Output("Lakea: No Lakea Event Found -> " + id);
                        Logs.Instance.NewLog(LogLevel.Warning, "No Lakea Event Found -> " + id);
                    }
                }
                else
                {
                    toUpdate.Add(id, item);
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Error Updating Lakea Events -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex.Message);
            }
        }

        //When Lakea finishes setting up, run all start up events in config
        public EventItem NewStartup(EventItem eve)
        {
            try
            {
                if(startupEvents.Count > 0)
                {
                    EventItem item = startupEvents[0];
                    startupEvents.RemoveAt(0);
                    item = passArgs.GetEventArgs(item, eve);
                    Task.Run(() => input.NewEvent(new EventItem(EventSource.Lakea, EventType.Lakea_Start_Up, EventTarget.Null, EventGoal.Null, "Lakea Start Up")));
                    if (item != null)
                    {
                        if (item.Duration == 0)
                        {
                            return item;
                        }
                        else
                        {
                            //Bad practice, could have multiple hanging threads, will need refactoring at later date
                            Thread.Sleep(TimeSpan.FromSeconds(item.Duration));
                            return item;
                        }
                    }                  
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Startup Event Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }

        //When Lakea shuts down, run all exit events in config
        public EventItem NewExit(EventItem eve)
        {
            try
            {
                if(shutdownEvents.Count > 0)
                {
                    EventItem item = shutdownEvents[shutdownEvents.Count - 1];
                    shutdownEvents.RemoveAt(shutdownEvents.Count - 1);
                    item = passArgs.GetEventArgs(item, eve);
                    input.NewEvent(new EventItem(EventSource.Lakea, EventType.Lakea_Exit, EventTarget.Null, EventGoal.Null, "Lakea Exit"));
                    if(item != null)
                    {
                        return item;
                    }
                }
            }
            catch(Exception ex)
            {
                Terminal.Output("Lakea: Exit Event Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }

        //When a callback event is triggered, checks dictionary for event before triggering the events effect
        public EventItem NewCallback(LakeaCallback eve)
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
                        return item;
                    }
                    else
                    {
                        EventItem item = callbacks[eve.Callback.ID];
                        item = passArgs.GetEventArgs(item, eve);
                        return item;
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
            return null;
        }

        //When a command event that is a inbuilt Lake command, call the commands object to call the relevant 'EventItem' for it
        public EventItem NewCommand(LakeaCommand eve)
        {
            try
            {
                EventItem item = commands.NewLakeaCommand(eve);
                if (item != null)
                {
                    return item;
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Default Command Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }

        //When Lakea is freed, check for events for it
        public EventItem LakeaReleased(EventItem eve)
        {
            try
            {
                if (lakeaReleased.ContainsKey(eve.ID))
                {
                    EventItem item = passArgs.GetEventArgs(lakeaReleased[eve.ID], eve);
                    if(item != null)
                    {
                        return item;
                    }
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Lakea Released Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }

        //When a event is fired and Lakea is caught, process the Lakea retort event
        public EventItem LakeaRetort()
        {
            try
            {
                if(lakeaRetort.ContainsKey("Lakea_Retort"))
                {
                    //return passArgs.GetEventArgs(lakeaRetort, eve);
                    return lakeaRetort["Lakea_Retort"];
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Lakea Retort Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }

        //When a event comes from one of the supporting apps, pass it through to the EventProcessor
        public EventItem NewSupportingApplicationEvent(EventItem eve)
        {
            try
            {
                if (applications.ContainsKey(eve.ID))
                {
                    EventItem item = passArgs.GetEventArgs(applications[eve.ID], eve);
                    if (item != null)
                    {
                        return item;
                    }
                }
                else if(eve.EventGoal != EventGoal.Null)
                {
                    return eve;
                }
            }
            catch(Exception ex)
            {
                Terminal.Output("Lakea: Supporting Application Event Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }

        //When a Web Socket event is fired
        public EventItem NewWebSocketEvent(EventItem eve)
        {
            try
            {
                if (webSocketEvents.ContainsKey(eve.ID))
                {
                    return webSocketEvents[eve.ID];
                }
            }
            catch(Exception ex)
            {
                Terminal.Output("Lakea: Web Socket Event Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }

        //When a timer event is fired, process the event item it carries
        public EventItem NewTimer(LakeaTimer eve)
        {
            try
            {
                return eve.EventItem;
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Timer Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
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
