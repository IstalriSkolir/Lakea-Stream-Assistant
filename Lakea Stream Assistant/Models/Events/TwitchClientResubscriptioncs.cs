using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events.EventAbstracts;
using TwitchLib.Client.Events;

namespace Lakea_Stream_Assistant.Models.Events
{
    public class TwitchClientResubscriptioncs : Event
    {
        private OnReSubscriberArgs args;

        public TwitchClientResubscriptioncs(EventSource source, EventType type, OnReSubscriberArgs args)
        {
            this.source = source;
            this.type = type;
            this.args = args;
        }

        public OnReSubscriberArgs Args { get { return args; } }

        public override Dictionary<string, string> GetArgs()
        {
            Dictionary<string, string> redeemArgs = new Dictionary<string, string>
            {
                { "DisplayName", args.ReSubscriber.DisplayName },
                { "IsModerator", args.ReSubscriber.IsModerator.ToString() },
                { "IsSubscriber", args.ReSubscriber.IsSubscriber.ToString() },
                { "IsPartner", args.ReSubscriber.IsPartner.ToString() },
                { "IsTurbo", args.ReSubscriber.IsTurbo.ToString() },
                { "SubscriptionPlan", args.ReSubscriber.SubscriptionPlanName },
                { "AccountID", args.ReSubscriber.UserId },
                { "SubscriptionMessage", args.ReSubscriber.ResubMessage },
                { "Months", args.ReSubscriber.Months.ToString() }
            };
            return redeemArgs;
        }
    }
}
