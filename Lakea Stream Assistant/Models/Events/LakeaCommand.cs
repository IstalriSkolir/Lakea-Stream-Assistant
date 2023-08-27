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

        public override EventSource Source { get { return source; } }
        public override EventType Type { get { return type; } }
        public List<string> Args { get { return args.Command.ArgumentsAsList; } }
        public string ArgsAsString { get { return args.Command.ArgumentsAsString; } }
        public string Command { get { return args.Command.CommandText; } }
        public char Identifier { get { return args.Command.CommandIdentifier; } }
        public string DisplayName { get { return args.Command.ChatMessage.DisplayName; } }

        public override Dictionary<string, string> GetArgs()
        {
            return null;
        }
    }
}
