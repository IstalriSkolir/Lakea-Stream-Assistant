using Lakea_Stream_Assistant.Models.Events.EventAbstracts;
using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events.EventItems;

namespace Lakea_Stream_Assistant.Models.Events
{
    public class LakeaCallback : LakeaEvent
    {
        private IDictionary<string, string> args;
        private string callbackID;

        public LakeaCallback(EventSource source, LakeaEventType type, string callbackID, IDictionary<string, string> args) 
        {
            this.source = source;
            this.eventType = type;
            this.callbackID = callbackID;
            this.args = args;
        }
        public override EventSource Source { get { return source; } }
        public override LakeaEventType EventType { get { return eventType; } }
        public string CallbackID { get { return callbackID; } }
        public IDictionary<string, string> Args { get { return args; } }

        public string[] GetCallbackArguments(LakeaEventItem item)
        {
            string[] newArgs = new string[0];
            switch (item.EventGoal)
            {
                case EventGoal.Send_Twitch_Chat_Message:
                    newArgs = callbackArgsForTwitchChatMessage(item);
                    break;
            }
            return newArgs;
        }

        private string[] callbackArgsForTwitchChatMessage(LakeaEventItem item)
        {
            int index = Int32.Parse(args["sourceNumber"]);
            return new string[] { item.Args[index] };
        }
    }
}
