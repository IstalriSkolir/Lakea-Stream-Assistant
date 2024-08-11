using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events.EventAbstracts;
using TwitchLib.Client.Events;

namespace Lakea_Stream_Assistant.Models.Events
{
    public class TwitchClientPrimePaidSubscription : Event
    {
        private OnPrimePaidSubscriberArgs args;

        public TwitchClientPrimePaidSubscription(EventSource source, EventType type, OnPrimePaidSubscriberArgs args)
        {
            this.source = source;
            this.type = type;
            this.args = args;
        }

        public OnPrimePaidSubscriberArgs Args { get { return args; } }

        public override Dictionary<string, string> GetArgs()
        {
            Dictionary<string, string> redeemArgs = new Dictionary<string, string>
            {
                { "DisplayName", args.PrimePaidSubscriber.DisplayName },
                { "IsModerator", args.PrimePaidSubscriber.IsModerator.ToString() },
                { "IsSubscriber", args.PrimePaidSubscriber.IsSubscriber.ToString() },
                { "IsPartner", args.PrimePaidSubscriber.IsPartner.ToString() },
                { "IsTurbo", args.PrimePaidSubscriber.IsTurbo.ToString() },
                { "SubscriptionPlan", args.PrimePaidSubscriber.SubscriptionPlanName },
                { "AccountID", args.PrimePaidSubscriber.UserId },
                { "SubscriptionMessage", args.PrimePaidSubscriber.ResubMessage }
            };
            return redeemArgs;
        }
    }
}
