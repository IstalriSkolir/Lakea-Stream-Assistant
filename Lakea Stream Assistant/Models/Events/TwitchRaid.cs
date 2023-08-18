using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events.EventAbstracts;
using TwitchLib.Client.Events;
using TwitchLib.PubSub.Events;

namespace Lakea_Stream_Assistant.Models.Events
{
    public class TwitchRaid : Event
    {
        private OnRaidNotificationArgs args;

        public TwitchRaid(EventSource source, EventType type, OnRaidNotificationArgs args) 
        {
            this.source = source;
            this.type = type;
            this.args = args;
        }

        public override EventSource Source { get { return source; } }
        public override EventType Type { get { return type; } }
        public OnRaidNotificationArgs Args { get { return args; } }

        public override Dictionary<string,string> GetArgs()
        {
            Dictionary<string, string> raidArgs = new Dictionary<string, string>
            {
                { "DisplayName", args.RaidNotification.DisplayName },
                { "RaiderCount", args.RaidNotification.MsgParamViewerCount }
            };
            return raidArgs;
        }
    }
}
