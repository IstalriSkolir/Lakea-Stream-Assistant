using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events.EventAbstracts;
using Lakea_Stream_Assistant.Models.Events.EventItems;
using TwitchLib.Client.Events;

namespace Lakea_Stream_Assistant.Models.Events
{
    public class LakeaCommand : Event
    {
        private OnChatCommandReceivedArgs args;

        public LakeaCommand(EventSource source, EventType type, OnChatCommandReceivedArgs args)
        {
            this.source = source;
            this.type = type;
            this.args = args;
        }

        public OnChatCommandReceivedArgs Args { get { return args; } }

        public override Dictionary<string, string> GetArgs()
        {
            return null;
        }
    }
}
