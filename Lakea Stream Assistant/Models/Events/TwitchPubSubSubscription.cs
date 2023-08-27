using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events.EventAbstracts;
using TwitchLib.PubSub.Events;

namespace Lakea_Stream_Assistant.Models.Events
{
    public class TwitchPubSubSubscription : Event
    {
        private OnChannelSubscriptionArgs args;

        public TwitchPubSubSubscription(EventSource source, EventType type, OnChannelSubscriptionArgs args)
        {
            this.source = source;
            this.type = type;
            this.args = args;
        }

        public OnChannelSubscriptionArgs Args { get { return args; } }

        public override Dictionary<string, string> GetArgs()
        {
            Dictionary<string, string> subscriptionArgs = new Dictionary<string, string>
            {
                { "DisplayName", args.Subscription.DisplayName },
                { "SubscriptionPlan", args.Subscription.SubscriptionPlan.ToString() },
                { "IsGift", args.Subscription.IsGift.ToString() },
                { "RecipientName", args.Subscription.RecipientDisplayName },
                { "Months", args.Subscription.Months.ToString() },
                { "StreakMonths", args.Subscription.StreakMonths.ToString() },
                { "CumulativeMonths", args.Subscription.CumulativeMonths.ToString() },
                { "SubMessage", args.Subscription.SubMessage.Message }
            };
            return subscriptionArgs;
        }
    }
}
