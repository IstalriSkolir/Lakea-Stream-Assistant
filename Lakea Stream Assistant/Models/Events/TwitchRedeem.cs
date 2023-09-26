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

        public OnChannelPointsRewardRedeemedArgs Args { get { return args; } }

        public override Dictionary<string, string> GetArgs()
        {
            Dictionary<string, string> redeemArgs = new Dictionary<string, string>
            {
                { "DisplayName", args.RewardRedeemed.Redemption.User.DisplayName },
                { "AccountID", args.RewardRedeemed.Redemption.User.Id },
                { "RedeemTitle", args.RewardRedeemed.Redemption.Reward.Title },
                { "RedeemCost", args.RewardRedeemed.Redemption.Reward.Cost.ToString() }
            };
            return redeemArgs;
        }
    }
}
