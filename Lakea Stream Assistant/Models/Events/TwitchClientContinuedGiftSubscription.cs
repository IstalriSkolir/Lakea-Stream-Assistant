using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events.EventAbstracts;
using TwitchLib.Client.Events;

namespace Lakea_Stream_Assistant.Models.Events
{
    public class TwitchClientContinuedGiftSubscription : Event
    {
        private OnContinuedGiftedSubscriptionArgs args;

        public TwitchClientContinuedGiftSubscription(EventSource source, EventType type, OnContinuedGiftedSubscriptionArgs args) 
        {
            this.source = source;
            this.type = type;
            this.args = args;
        }

        public OnContinuedGiftedSubscriptionArgs Args { get { return args; } }

        public override Dictionary<string, string> GetArgs()
        {
            Dictionary<string, string> redeemArgs = new Dictionary<string, string>
            {
                { "DisplayName", args.ContinuedGiftedSubscription.DisplayName },
                { "IsModerator", args.ContinuedGiftedSubscription.IsModerator.ToString() },
                { "IsSubscriber", args.ContinuedGiftedSubscription.IsSubscriber.ToString() },
                { "AccountID", args.ContinuedGiftedSubscription.UserId }
            };
            return redeemArgs;
        }
    }
}
