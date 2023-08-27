using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events.EventAbstracts;
using Lakea_Stream_Assistant.Models.Events.EventLists;

namespace Lakea_Stream_Assistant.Models.Events
{
    public class LakeaTimer : Event
    {
        private EventItem timerItem;

        public LakeaTimer(EventSource source, EventType type)
        {
            this.source = source;
            this.type = type;
        }

        public LakeaTimer(EventSource source, EventType type, EventItem timerItem)
        {
            this.source = source;
            this.type = type;
            this.timerItem = timerItem;
        }

        public EventItem EventItem { get { return timerItem; } }

        public override Dictionary<string, string> GetArgs()
        {
            return new Dictionary<string, string>();    
        }
    }
}
