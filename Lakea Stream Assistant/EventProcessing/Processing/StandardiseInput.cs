using OBSWebsocketDotNet.Types.Events;
using TwitchLib.Client.Events;
using TwitchLib.EventSub.Websockets.Core.EventArgs.Channel;
using TwitchLib.PubSub.Events;

namespace Lakea_Stream_Assistant.EventProcessing.Processing
{
    // Class with functions to standardise incoming event data into Dictionary<string, string> structures
    public class StandardiseInput
    {
        #region OBS Events

        public Dictionary<string, string> ConvertOBSSceneChangeData(ProgramSceneChangedEventArgs args)
        {
            Dictionary<string, string> sceneArgs = new Dictionary<string, string>
            {
                { "SceneName", args.SceneName }
            };
            return sceneArgs;
        }

        public Dictionary<string, string> ConvertOBSSourceActiveData(SceneItemEnableStateChangedEventArgs args, string sourceName)
        {
            Dictionary<string, string> sourceArgs = new Dictionary<string, string>
            {
                { "SourceID", args.SceneItemId.ToString() },
                { "SourceName", sourceName },
                { "SceneName", args.SceneName },
                { "Enabled", args.SceneItemEnabled.ToString() }
            };
            return sourceArgs;
        }

        #endregion

        #region Twitch Events

        // Convert Twitch bits data to Dictionary
        public Dictionary<string, string> ConvertTwitchBitsData(ChannelCheerArgs args)
        {
            Dictionary<string, string> bitsArgs = new Dictionary<string, string>
            {
                { "Bits", args.Notification.Payload.Event.Bits.ToString() },
                { "IsAnonymous", args.Notification.Payload.Event.IsAnonymous.ToString() },
                { "DisplayName", args.Notification.Payload.Event.UserName },
                { "ChannelName", args.Notification.Payload.Event.BroadcasterUserName },
                { "ChatMessage", args.Notification.Payload.Event.Message },
                { "AccountID", args.Notification.Payload.Event.UserId }
            };
            return bitsArgs;
        }

        // Convert Twitch command data to Dictionary
        public Dictionary<string, string> ConvertTwitchCommandData(OnChatCommandReceivedArgs args)
        {
            Dictionary<string, string> commandArgs = new Dictionary<string, string>
            {
                { "CommandIdentifier", args.Command.CommandIdentifier.ToString() },
                { "CommandText", args.Command.CommandText },
                { "IsBroadcaster", args.Command.ChatMessage.IsBroadcaster.ToString() },
                { "IsModerator", args.Command.ChatMessage.IsModerator.ToString() },
                { "IsSubscriber", args.Command.ChatMessage.IsSubscriber.ToString() },
                { "IsPartner", args.Command.ChatMessage.IsPartner.ToString() },
                { "IsVip", args.Command.ChatMessage.IsVip.ToString() },
                { "DisplayName", args.Command.ChatMessage.DisplayName },
                { "ChatMessage", args.Command.ChatMessage.Message },
                { "AccountID", args.Command.ChatMessage.UserId },
                { "ArgumentsAsString", args.Command.ArgumentsAsString },
                { "Channel", args.Command.ChatMessage.Channel }
            };
            for (int i = 0; i < args.Command.ArgumentsAsList.Count; i++)
            {
                commandArgs.Add("CommandArg" + (i + 1), args.Command.ArgumentsAsList[i]);
            }
            return commandArgs;
        }

        // Convert Twitch continued gifted subscription data to Dictionary
        public Dictionary<string, string> ConvertTwitchContinuedGiftedSubscriptionData(OnContinuedGiftedSubscriptionArgs args)
        {
            Dictionary<string, string> redeemArgs = new Dictionary<string, string>
            {
                { "DisplayName", args.ContinuedGiftedSubscription.DisplayName },
                { "IsModerator", args.ContinuedGiftedSubscription.IsModerator.ToString() },
                { "IsSubscriber", args.ContinuedGiftedSubscription.IsSubscriber.ToString() },
                { "ContinuedGiftedSubscription", args.ContinuedGiftedSubscription.ToString() },
                { "AccountID", args.ContinuedGiftedSubscription.UserId }
            };
            return redeemArgs;
        }

        // Convert Twitch follow data to Dictionary
        public Dictionary<string, string> ConvertTwitchFollowData(ChannelFollowArgs args)
        {
            Dictionary<string, string> followArgs = new Dictionary<string, string>
            {
                { "DisplayName", args.Notification.Payload.Event.UserName },
                { "AccountID", args.Notification.Payload.Event.UserId },
                { "ChannelID", args.Notification.Payload.Event.BroadcasterUserId }
            };
            return followArgs;
        }

        // Convert Twitch gifted subscription data to Dictionary
        public Dictionary<string, string> ConvertTwitchGiftedSubscriptionData(OnGiftedSubscriptionArgs args)
        {
            Dictionary<string, string> redeemArgs = new Dictionary<string, string>
            {
                { "DisplayName", args.GiftedSubscription.DisplayName },
                { "IsModerator", args.GiftedSubscription.IsModerator.ToString() },
                { "IsSubscriber", args.GiftedSubscription.IsSubscriber.ToString() },
                { "IsAnonymous", args.GiftedSubscription.IsAnonymous.ToString() },
                { "IsTurbo", args.GiftedSubscription.IsTurbo.ToString() },
                { "SubscriptionPlan", args.GiftedSubscription.MsgParamSubPlan.ToString() },
                { "SubscriptionPlanName", args.GiftedSubscription.MsgParamSubPlanName },
                { "AccountID", args.GiftedSubscription.UserId },
                { "RecipientAccountID", args.GiftedSubscription.MsgParamRecipientId },
                { "RecipientDisplayName", args.GiftedSubscription.MsgParamRecipientDisplayName }
            };
            return redeemArgs;
        }

        // Convert Twitch raid data to Dictionary
        public Dictionary<string, string> ConvertTwitchRaidData(OnRaidNotificationArgs args)
        {
            Dictionary<string, string> raidArgs = new Dictionary<string, string>
            {
                { "DisplayName", args.RaidNotification.DisplayName },
                { "RaiderCount", args.RaidNotification.MsgParamViewerCount }
            };
            return raidArgs;
        }

        // Convert Twitch prime paid subscription data to Dictionary
        public Dictionary<string, string> ConvertTwitchPrimePaidSubscriptionData(OnPrimePaidSubscriberArgs args)
        {
            Dictionary<string, string> redeemArgs = new Dictionary<string, string>
            {
                { "DisplayName", args.PrimePaidSubscriber.DisplayName },
                { "IsModerator", args.PrimePaidSubscriber.IsModerator.ToString() },
                { "IsSubscriber", args.PrimePaidSubscriber.IsSubscriber.ToString() },
                { "IsPartner", args.PrimePaidSubscriber.IsPartner.ToString() },
                { "IsTurbo", args.PrimePaidSubscriber.IsTurbo.ToString() },
                { "SubscriptionPlan", args.PrimePaidSubscriber.SubscriptionPlan.ToString() },
                { "SubscriptionPlanName", args.PrimePaidSubscriber.SubscriptionPlanName },
                { "AccountID", args.PrimePaidSubscriber.UserId },
                { "SubscriptionMessage", args.PrimePaidSubscriber.ResubMessage }
            };
            return redeemArgs;
        }

        // Convert Twitch redeem data to Dictionary
        public Dictionary<string, string> ConvertTwitchRedeemData(ChannelPointsCustomRewardRedemptionArgs args)
        {
            Dictionary<string, string> argsDict = new Dictionary<string, string>
            {
                { "DisplayName", args.Notification.Payload.Event.UserName },
                { "AccountID", args.Notification.Payload.Event.UserId },
                { "RedeemTitle", args.Notification.Payload.Event.Reward.Title },
                { "RedeemCost", args.Notification.Payload.Event.Reward.Cost.ToString() },
                { "RedeemID", args.Notification.Payload.Event.Reward.Id }
            };
            return argsDict;
        }

        // Convert Twitch resubscription data to Dictionary
        public Dictionary<string, string> ConvertTwitchResubscriptionData(OnReSubscriberArgs args)
        {
            Dictionary<string, string> redeemArgs = new Dictionary<string, string>
            {
                { "DisplayName", args.ReSubscriber.DisplayName },
                { "IsModerator", args.ReSubscriber.IsModerator.ToString() },
                { "IsSubscriber", args.ReSubscriber.IsSubscriber.ToString() },
                { "IsPartner", args.ReSubscriber.IsPartner.ToString() },
                { "IsTurbo", args.ReSubscriber.IsTurbo.ToString() },
                { "SubscriptionPlan", args.ReSubscriber.SubscriptionPlan.ToString() },
                { "SubscriptionPlanName", args.ReSubscriber.SubscriptionPlanName },
                { "AccountID", args.ReSubscriber.UserId },
                { "SubscriptionMessage", args.ReSubscriber.ResubMessage },
                { "Months", args.ReSubscriber.Months.ToString() }
            };
            return redeemArgs;
        }

        // Convert Twitch subscription data to Dictionary
        public Dictionary<string, string> ConvertTwitchSubscriptionData(OnNewSubscriberArgs args)
        {
            Dictionary<string, string> redeemArgs = new Dictionary<string, string>
            {
                { "DisplayName", args.Subscriber.DisplayName },
                { "IsModerator", args.Subscriber.IsModerator.ToString() },
                { "IsSubscriber", args.Subscriber.IsSubscriber.ToString() },
                { "IsPartner", args.Subscriber.IsPartner.ToString() },
                { "IsTurbo", args.Subscriber.IsTurbo.ToString() },
                { "SubscriptionPlan", args.Subscriber.SubscriptionPlan.ToString() },
                { "SubscriptionPlanName", args.Subscriber.SubscriptionPlanName },
                { "AccountID", args.Subscriber.UserId },
                { "SubscriptionMessage", args.Subscriber.ResubMessage }
            };
            return redeemArgs;
        }

        #endregion
    }
}
