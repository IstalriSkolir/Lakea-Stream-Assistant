using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events.EventAbstracts;
using TwitchLib.Client.Events;
using TwitchLib.PubSub.Events;

namespace Lakea_Stream_Assistant.Models.Events
{
    public class TwitchSubscription : Event
    {
        private OnNewSubscriberArgs args;

        public TwitchSubscription(EventSource source, EventType type, OnNewSubscriberArgs args)
        {
            this.source = source;
            this.type = type;
            this.args = args;
        }

        public override EventSource Source { get { return source; } }
        public override EventType Type { get { return type; } }
        public OnNewSubscriberArgs Args { get { return args; } }

        public override Dictionary<string, string> GetArgs()
        {
            Dictionary<string, string> redeemArgs = new Dictionary<string, string>
            {
                { "DisplayName", args.Subscriber.DisplayName },
                { "IsModerator", args.Subscriber.IsModerator.ToString() },
                { "IsSubscriber", args.Subscriber.IsSubscriber.ToString() },
                { "IsPartner", args.Subscriber.IsPartner.ToString() },
                { "SubscriptionPlan", args.Subscriber.SubscriptionPlanName }
            };
            return redeemArgs;
        }
    }
}
