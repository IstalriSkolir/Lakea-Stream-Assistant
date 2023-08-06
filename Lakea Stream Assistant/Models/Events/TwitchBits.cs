using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events.EventAbstracts;
using TwitchLib.PubSub.Events;

namespace Lakea_Stream_Assistant.Models.Events
{
    public class TwitchBits : Event
    {
        private OnBitsReceivedV2Args args;

        public TwitchBits(EventSource source, EventType type, OnBitsReceivedV2Args args) 
        {
            this.source = source;
            this.type = type;
            this.args = args;
        }

        public override EventSource Source { get { return source; } }
        public override EventType Type { get { return type; } }
        public OnBitsReceivedV2Args Args { get { return args; } }
    }
}
