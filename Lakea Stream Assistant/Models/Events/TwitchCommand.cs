using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events.EventAbstracts;
using TwitchLib.Client.Events;
using TwitchLib.PubSub.Events;

namespace Lakea_Stream_Assistant.Models.Events
{
    public class TwitchCommand : Event
    {
        private OnChatCommandReceivedArgs args;

        public TwitchCommand(EventSource source, EventType type, OnChatCommandReceivedArgs args)
        {
            this.source = source;
            this.type = type;
            this.args = args;
        }

        public override EventSource Source { get { return source; } }
        public override EventType Type { get { return type; } }
        public OnChatCommandReceivedArgs Args { get { return args; } }
    }
}
