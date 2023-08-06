using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events.EventAbstracts;
using TwitchLib.PubSub.Events;

namespace Lakea_Stream_Assistant.Models.Events
{
    //Inherits from TwitchEvent, is the class for storing Channel redeem event data
    public class TwitchRedeem : Event
    {
        private OnChannelPointsRewardRedeemedArgs args;

        public TwitchRedeem(EventSource source, EventType type, OnChannelPointsRewardRedeemedArgs args)
        {
            this.source = source;
            this.type = type;
            this.args = args;
        }

        public override EventSource Source { get { return source; } }
        public override EventType Type { get { return type; } }
        public OnChannelPointsRewardRedeemedArgs Args { get { return args; } }
    }
}
