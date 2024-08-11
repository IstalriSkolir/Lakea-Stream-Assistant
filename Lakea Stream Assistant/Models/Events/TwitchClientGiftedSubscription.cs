using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events.EventAbstracts;
using TwitchLib.Client.Events;

namespace Lakea_Stream_Assistant.Models.Events
{
    public class TwitchClientGiftedSubscription : Event
    {
        private OnGiftedSubscriptionArgs args;

        public TwitchClientGiftedSubscription(EventSource source, EventType type, OnGiftedSubscriptionArgs args)
        {
            this.source = source;
            this.type = type;
            this.args = args;
        }

        public OnGiftedSubscriptionArgs Args { get { return args; } }

        public override Dictionary<string, string> GetArgs()
        {
            Dictionary<string, string> redeemArgs = new Dictionary<string, string>
            {
                { "DisplayName", args.GiftedSubscription.DisplayName },
                { "IsModerator", args.GiftedSubscription.IsModerator.ToString() },
                { "IsSubscriber", args.GiftedSubscription.IsSubscriber.ToString() },
                { "IsAnonymous", args.GiftedSubscription.IsAnonymous.ToString() },
                { "IsTurbo", args.GiftedSubscription.IsTurbo.ToString() },
                { "SubscriptionPlan", args.GiftedSubscription.MsgParamSubPlanName },
                { "AccountID", args.GiftedSubscription.UserId },
                { "RecipientAccountID", args.GiftedSubscription.MsgParamRecipientId },
                { "RecipientDisplayName", args.GiftedSubscription.MsgParamRecipientDisplayName }
            };
            return redeemArgs;
        }
    }
}
