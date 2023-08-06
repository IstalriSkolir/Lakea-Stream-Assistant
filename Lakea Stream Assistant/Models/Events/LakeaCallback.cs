using Lakea_Stream_Assistant.Models.Events.EventAbstracts;
using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events.EventLists;

namespace Lakea_Stream_Assistant.Models.Events
{
    public class LakeaCallback : Event
    {
        private IDictionary<string, string> args;
        private string callbackID;

        public LakeaCallback(EventSource source, EventType type, string callbackID, IDictionary<string, string> args) 
        {
            this.source = source;
            this.type = type;
            this.callbackID = callbackID;
            this.args = args;
        }
        public override EventSource Source { get { return source; } }
        public override EventType Type { get { return type; } }
        public string CallbackID { get { return callbackID; } }
        public IDictionary<string, string> Args { get { return args; } }

        public IDictionary<string, string> GetCallbackArguments(EventItem item)
        {

            //foreach (var arg in args)
            //{
            //    if (item.Args.ContainsKey(arg.Key))
            //    {
            //        item.Args.Remove(arg.Key);
            //    }
            //    item.Args.Add(arg.Key, arg.Value);
            //}
            //return item.Args;
            IDictionary<string, string> newArgs = new Dictionary<string, string>();
            switch (item.EventGoal)
            {
                case EventGoal.Twitch_Send_Chat_Message:
                    newArgs = callbackArgsForTwitchChatMessage(item);
                    break;
            }
            return newArgs;
        }

        private IDictionary<string, string> callbackArgsForTwitchChatMessage(EventItem item)
        {
            IDictionary<string, string> newArgs = new Dictionary<string, string>();
            string key = "Message" + args["sourceNumber"];
            newArgs.Add("Message", item.Args[key]);
            return newArgs;
        }
    }
}
