using Lakea_Stream_Assistant.Models.Events.EventAbstracts;
using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Models.Events.EventItems;

namespace Lakea_Stream_Assistant.Models.Events
{
    public class LakeaCallback : Event
    {
        private Dictionary<string, string> args;
        private Callbacks callback;

        public LakeaCallback(EventSource source, EventType type, Callbacks callback, Dictionary<string, string> args) 
        {
            this.source = source;
            this.type = type;
            this.callback = callback;
            this.args = args;
        }
        public override EventSource Source { get { return source; } }
        public override EventType Type { get { return type; } }
        public Callbacks Callback { get { return callback; } }
        public Dictionary<string, string> Args { get { return args; } }

        public override Dictionary<string, string> GetArgs()
        {
            return args;
        }

        public Dictionary<string, string> GetCallbackArguments(EventItem item)
        {
            Dictionary<string, string> newArgs = new Dictionary<string, string>();
            switch (item.EventGoal)
            {
                case EventGoal.Twitch_Send_Chat_Message:
                    newArgs = callbackArgsForTwitchChatMessage(item);
                    break;
            }
            return newArgs;
        }

        private Dictionary<string, string> callbackArgsForTwitchChatMessage(EventItem item)
        {
            Dictionary<string, string> newArgs = new Dictionary<string, string>();
            string key = "Message" + args["SourceNumber"];
            newArgs.Add("Message", item.Args[key]);
            return newArgs;
        }
    }
}
