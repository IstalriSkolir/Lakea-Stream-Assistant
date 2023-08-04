using Lakea_Stream_Assistant.Enums;

namespace Lakea_Stream_Assistant.Models.Events.EventAbstracts
{
    //Inherits from Event class, all specific Lakea events inherit from this class
    public abstract class LakeaEvent : Event
    {
        private protected LakeaEventType eventType;

        public abstract LakeaEventType EventType { get; }
    }
}
