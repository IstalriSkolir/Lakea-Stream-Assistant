using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using TwitchLib.PubSub.Events;

namespace Lakea_Stream_Assistant.Models.Events
{
    public class TwitchRedeem : TwitchEvent
    {
        private OnChannelPointsRewardRedeemedArgs args;

        public TwitchRedeem(EventSource newSource, TwitchEventType newTwitchEvent, OnChannelPointsRewardRedeemedArgs newArgs)
        {
            this.source = newSource;
            this.eventType = newTwitchEvent;
            this.args = newArgs;
        }

        public override EventSource Source { get { return source; } }
        public override TwitchEventType EventType { get { return eventType; } }
        public OnChannelPointsRewardRedeemedArgs Args { get { return args; } }
    }
}
