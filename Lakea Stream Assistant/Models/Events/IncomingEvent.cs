using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events.EventAbstracts;

namespace Lakea_Stream_Assistant.Models.Events
{
    public class IncomingEvent : Event
    {
        private Dictionary<string, string> args;

        public Dictionary<string, string> Args { get { return args; } }

        public IncomingEvent(EventSource source, EventType type, Dictionary<string, string> args)
        {
            this.source = source;
            this.type = type;
            this.args = args;
        }
    }
}
