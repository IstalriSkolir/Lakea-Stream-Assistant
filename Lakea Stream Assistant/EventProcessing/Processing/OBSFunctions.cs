using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventItems;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;
using Lakea_Stream_Assistant.Utilities;

namespace Lakea_Stream_Assistant.EventProcessing.Processing
{
    //Functions for handling OBS Events
    public class OBSFunctions
    {
        private EventPassArguments passArgs;
        private Dictionary<EventType, Dictionary<string, EventItem>> events;
        private Dictionary<string, EventItem> sceneChanges;
        private Dictionary<string, EventItem> sourceActiveStatus;

        //Contructor stores list of events to check against when it receives a new event
        public OBSFunctions(ConfigEvent[] newEvents, EventPassArguments passArgs)
        {
            this.passArgs = passArgs;
            sceneChanges = new Dictionary<string, EventItem>();
            sourceActiveStatus = new Dictionary<string, EventItem>();
            events = new Dictionary<EventType, Dictionary<string, EventItem>>();
            events.Add(EventType.OBS_Scene_Changed, sceneChanges);
            events.Add(EventType.OBS_Source_Active_Status, sourceActiveStatus);
            foreach(ConfigEvent eve in newEvents)
            {
                try
                {
                    EventSource source = eve.EventDetails.Source.ToEnum<EventSource>();
                    if (source == EventSource.OBS)
                    {
                        EventType type = eve.EventDetails.Type.ToEnum<EventType>();
                        switch(type)
                        {
                            case EventType.OBS_Scene_Changed:
                                sceneChanges.Add(eve.EventDetails.ID, new EventItem(eve));
                                break;
                            case EventType.OBS_Source_Active_Status:
                                sourceActiveStatus.Add(eve.EventDetails.ID, new EventItem(eve));
                                break;
                            default:
                                Console.WriteLine("Lakea: Invalid 'EventType' in 'OBSFunctions' Constructor -> " + type);
                                Logs.Instance.NewLog(LogLevel.Warning, new Exception("Lakea: Invalid 'EventType' in 'OBSFunctions' Constructor -> " + type));
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
        }

        // Update the OBS events during runtime
        public void UpdateDictionary(string id, EventItem item, bool remove)
        {
            try
            {
                Dictionary<string, EventItem> toUpdate = events[item.Type];
                if (remove)
                {
                    if (toUpdate.ContainsKey(id))
                    {
                        Terminal.Output("Lakea: Removing OBS Event -> " + toUpdate[id].Name);
                        Logs.Instance.NewLog(LogLevel.Info, "Removing OBS Event -> " + toUpdate[id].Name);
                        toUpdate.Remove(id);
                    }
                    else
                    {
                        Terminal.Output("Lakea: No OBS Event Found -> " + id);
                        Logs.Instance.NewLog(LogLevel.Warning, "No OBS Event Found -> " + id);
                    }
                }
                else
                {
                    toUpdate.Add(id, item);
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Error Updating OBS Events -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex.Message);
            }
        }

        //When a scene changes, checks the scenes dictionary for event before triggering events effect
        public EventItem NewChangedScene(IncomingEvent eve)
        {
            try
            {
                string sceneName = eve.Args["SceneName"];
                if(sceneChanges.ContainsKey(sceneName))
                {
                    EventItem item = passArgs.GetEventArgs(sceneChanges[sceneName], eve);
                    if (item != null)
                    {
                        return item;
                    }
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: OBS Changed Scene Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }

        //When a source active status changes, checks the source active status dictionary for event before triggering events effect
        public EventItem NewSourceActiveStatus(IncomingEvent eve)
        {
            try
            {
                string sourceName = eve.Args["SourceName"];
                bool enabled = bool.Parse(eve.Args["Enabled"]);
                if (sourceActiveStatus.ContainsKey(sourceName))
                {
                    if (sourceActiveStatus[sourceName].Args.ContainsKey("Active"))
                    {
                        bool target = Convert.ToBoolean(sourceActiveStatus[sourceName].Args["Active"]);
                        if(target == enabled)
                        {
                            EventItem item = passArgs.GetEventArgs(sourceActiveStatus[sourceName], eve);
                            if (item != null)
                            {
                                return item;
                            }
                        }
                    }
                    else
                    {
                        EventItem item = passArgs.GetEventArgs(sourceActiveStatus[sourceName], eve);
                        if (item != null)
                        {
                            return item;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Terminal.Output("Lakea: OBS Source Active Status Changed Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }
    }
}
