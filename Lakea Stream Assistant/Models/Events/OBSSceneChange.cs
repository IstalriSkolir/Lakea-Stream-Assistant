using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events.EventAbstracts;
using OBSWebsocketDotNet.Types.Events;

namespace Lakea_Stream_Assistant.Models.Events
{
    public class OBSSceneChange : Event
    {
        private ProgramSceneChangedEventArgs args;

        public OBSSceneChange(EventSource source, EventType type, ProgramSceneChangedEventArgs args)
        {
            this.source = source;
            this.type = type;
            this.args = args;
        }

        public ProgramSceneChangedEventArgs Args { get { return args; } }

        public override Dictionary<string, string> GetArgs()
        {
            Dictionary<string, string> sceneArgs = new Dictionary<string, string>
            {
                { "SceneName", args.SceneName }
            };
            return sceneArgs;
        }
    }
}
