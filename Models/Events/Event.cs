using Lakea_Stream_Assistant.Enums;

namespace Lakea_Stream_Assistant.Models.Events
{
    public abstract class Event
    {
        private protected EventSource source;

        public abstract EventSource Source { get; }
    }
}
