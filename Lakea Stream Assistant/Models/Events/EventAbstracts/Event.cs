using Lakea_Stream_Assistant.Enums;

namespace Lakea_Stream_Assistant.Models.Events.EventAbstracts
{
    //Top level class for event objects, all event objects inherit from this abstract class
    public abstract class Event
    {
        private protected EventSource source;
        private protected EventType type;

        public EventSource Source { get { return source; } }
        public EventType Type { get { return type; } }

        public abstract Dictionary<string, string> GetArgs();
    }
}
