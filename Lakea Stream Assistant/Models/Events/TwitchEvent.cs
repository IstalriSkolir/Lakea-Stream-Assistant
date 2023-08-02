using Lakea_Stream_Assistant.Enums;

namespace Lakea_Stream_Assistant.Models.Events
{
    //Inherits from Event class, all specific Twitch events inherit from this class
    public abstract class TwitchEvent : Event
    {
        private protected TwitchEventType eventType;

        public abstract TwitchEventType EventType { get; }
    }
}
