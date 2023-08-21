using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events.EventAbstracts;
using Lakea_Stream_Assistant.Models.Events.EventItems;
using TwitchLib.Client.Events;

namespace Lakea_Stream_Assistant.Models.Events
{
    public class LakeaCommand : Event
    {
        private List<string> args;
        private string argsAsString;
        private string command;
        private char identifier;

        public LakeaCommand(EventSource source, EventType type, OnChatCommandReceivedArgs args)
        {
            this.source = source;
            this.type = type;
            this.argsAsString = args.Command.ArgumentsAsString;
            this.command = args.Command.CommandText;
            this.identifier = args.Command.CommandIdentifier;
            this.args = new List<string>();
            foreach(string arg in args.Command.ArgumentsAsList)
            {
                this.args.Add(arg);
            }
        }

        public override EventSource Source { get { return source; } }
        public override EventType Type { get { return type; } }
        public List<string> Args { get { return args; } }
        public string ArgsAsString { get { return argsAsString; } }
        public string Command { get { return command; } }
        public char Identifier { get { return identifier; } }

        public override Dictionary<string, string> GetArgs()
        {
            return null;
        }
    }
}
