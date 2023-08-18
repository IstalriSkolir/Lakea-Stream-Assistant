using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events.EventAbstracts;

namespace Lakea_Stream_Assistant.Models.Events
{
    public class LakeaTimer : Event
    {
        public LakeaTimer(EventSource source, EventType type)
        {
            this.source = source;
            this.type = type;
        }

        public override EventSource Source { get { return source; } }
        public override EventType Type { get { return type; } }

        public override Dictionary<string, string> GetArgs()
        {
            return new Dictionary<string, string>();    
        }
    }
}
