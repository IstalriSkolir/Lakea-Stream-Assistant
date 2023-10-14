using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events.EventAbstracts;
using OBSWebsocketDotNet.Types.Events;

namespace Lakea_Stream_Assistant.Models.Events
{
    public class OBSSourceActive : Event
    {
        private SceneItemEnableStateChangedEventArgs args;
        private string sourceName;

        public OBSSourceActive(EventSource source, EventType type, SceneItemEnableStateChangedEventArgs args, string sourceName)
        {
            this.source = source;
            this.type = type;
            this.args = args;
            this.sourceName = sourceName;
        }

        public SceneItemEnableStateChangedEventArgs Args { get { return args; } }
        public string SourceName {  get { return sourceName; } }

        public override Dictionary<string, string> GetArgs()
        {
            Dictionary<string, string> sourceArgs = new Dictionary<string, string>
            {
                { "SourceID", args.SceneItemId.ToString() },
                { "SourceName", sourceName },
                { "SceneName", args.SceneName },
                { "Enabled", args.SceneItemEnabled.ToString() }
            };
            return sourceArgs;
        }
    }
}
