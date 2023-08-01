using Lakea_Stream_Assistant.Enums;

namespace Lakea_Stream_Assistant.Models.Events
{
    public abstract class TwitchEvent : Event
    {
        private protected TwitchEventType eventType;

        public abstract TwitchEventType EventType { get; }
    }
}
