using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventItems;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;

namespace Lakea_Stream_Assistant.EventProcessing.Processing
{
    public class OBSFunctions
    {
        private EventProcesser processer;
        private EventPassArguments passArgs;
        private Dictionary<string, EventItem> sceneChanges;

        public OBSFunctions(ConfigEvent[] events, EventProcesser processor, EventPassArguments passArgs)
        {
            this.processer = processor;
            this.passArgs = passArgs;
            sceneChanges = new Dictionary<string, EventItem>();
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
    }
}
