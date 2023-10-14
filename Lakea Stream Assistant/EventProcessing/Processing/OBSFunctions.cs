using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventItems;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;

namespace Lakea_Stream_Assistant.EventProcessing.Processing
{
    //Functions for handling OBS Events
    public class OBSFunctions
    {
        private EventProcesser processer;
        private EventPassArguments passArgs;
        private Dictionary<string, EventItem> sceneChanges;
        private Dictionary<string, EventItem> sourceActiveStatus;

        //Contructor stores list of events to check against when it receives a new event
        public OBSFunctions(ConfigEvent[] events, EventProcesser processor, EventPassArguments passArgs)
        {
            this.processer = processor;
            this.passArgs = passArgs;
            sceneChanges = new Dictionary<string, EventItem>();
            sourceActiveStatus = new Dictionary<string, EventItem>();
            EnumConverter enums = new EnumConverter();
            foreach(ConfigEvent eve in events)
            {
                try
                {
                    EventSource source = enums.ConvertEventSourceString(eve.EventDetails.Source);
                    if(source == EventSource.OBS)
                    {
                        EventType type = enums.ConvertEventTypeString(eve.EventDetails.Type);
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

        //When a scene changes, checks the scenes dictionary for event before triggering events effect
        public void NewChangedScene(OBSSceneChange eve)
        {
            try
            {
                if(sceneChanges.ContainsKey(eve.Args.SceneName))
                {
                    EventItem item = passArgs.GetEventArgs(sceneChanges[eve.Args.SceneName], eve);
                    if (item != null)
                    {
                        processer.ProcessEvent(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: OBS Changed Scene Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        //When a source active status changes, checks the source active status dictionary for event before triggering events effect
        public void NewSourceActiveStatus(OBSSourceActive eve)
        {
            try
            {
                if (sourceActiveStatus.ContainsKey(eve.SourceName))
                {
                    if (sourceActiveStatus[eve.SourceName].Args.ContainsKey("Active"))
                    {
                        bool target = Convert.ToBoolean(sourceActiveStatus[eve.SourceName].Args["Active"]);
                        if(target == eve.Args.SceneItemEnabled)
                        {
                            EventItem item = passArgs.GetEventArgs(sourceActiveStatus[eve.SourceName], eve);
                            if (item != null)
                            {
                                processer.ProcessEvent(item);
                            }
                        }
                    }
                    else
                    {
                        EventItem item = passArgs.GetEventArgs(sourceActiveStatus[eve.SourceName], eve);
                        if (item != null)
                        {
                            processer.ProcessEvent(item);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Terminal.Output("Lakea: OBS Source Active Status Changed Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }
    }
}
