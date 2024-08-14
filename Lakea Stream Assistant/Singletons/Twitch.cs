using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.EventProcessing.Processing;
using Lakea_Stream_Assistant.EventProcessing.Commands;
using Lakea_Stream_Assistant.Static;
using TwitchLib.PubSub;
using TwitchLib.PubSub.Events;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using TwitchLib.Client.Events;
using TwitchLib.Communication.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Events;
using TwitchLib.Api;
using TwitchLib.Api.Helix.Models.Subscriptions;
using TwitchLib.Api.Helix.Models.ChannelPoints.CreateCustomReward;
using TwitchLib.Api.Helix.Models.ChannelPoints.UpdateCustomReward;
using TwitchLib.Api.Helix.Models.Channels.ModifyChannelInformation;
using TwitchLib.Api.Helix.Models.Channels.GetChannelInformation;
using TwitchLib.Api.Helix.Models.Games;
using Lakea_Stream_Assistant.EventProcessing.Misc;
using TwitchLib.Client.Extensions;
using TwitchLib.Api.Helix.Models.Moderation.BanUser;

namespace Lakea_Stream_Assistant.Singletons
{
    //Sealed class for Twitch Integration
    public sealed class Twitch
    {
        private static DefaultCommands lakeaCommands;
        private static EventInput eventHandler;
        private static ScamMessageDetector scamMessageDetector;
        private static TwitchPubSub pubSub;
        private static TwitchClient client;
        private static TwitchAPI api;
        private static string channelUsername;
        private static string channelID;
        private static string channelAuthKey;
        private static string clientID;
        private static string botUsername;
        private static string botAuthKey;
        private static string botID;
        private static string botChannelToJoin;
        private static char commandIdentifier;
        private static bool pubSubConnected = false;

        public static bool IsPubSubConnected { get { return pubSubConnected; } }

        #region Initiliase

        //Initiliases the Singleton by connecting to Twitch with the settings in the config object
        public static async void Initialise(Config config, EventInput newEventsObj, DefaultCommands commands)
        {
            try
            {
                scamMessageDetector = new ScamMessageDetector(config.Settings.ScamMessageDetection);
                eventHandler = newEventsObj;
                lakeaCommands = commands;
                channelUsername = config.Twitch.StreamingChannel.UserName;
                channelID = config.Twitch.StreamingChannel.ID.ToString();
                channelAuthKey = config.Twitch.StreamingChannel.AuthKey;
                clientID = config.Twitch.StreamingChannel.ClientID;
                botUsername = config.Twitch.BotChannel.UserName;
                botAuthKey = config.Twitch.BotChannel.UserToken;
                botID = config.Twitch.BotChannel.UserID;
                botChannelToJoin = config.Twitch.BotChannel.ChannelConnection;
                commandIdentifier = config.Twitch.CommandIdentifier.ToCharArray()[0];
                initiliaseClient();
                initiliasePubSub();
                initialiseAPI();
            }
            catch (Exception ex)
            {
                Terminal.Output("Fatal Error: Failed to Connect to Twitch -> " + ex.Message);
                Terminal.Output("Terminating Lakea...");
                Logs.Instance.NewLog(LogLevel.Fatal, ex);
                Thread.Sleep(5000);
                Environment.Exit(1);
            }
        }

        // Initiliase Twitch's Client connection
        private static void initiliaseClient()
        {
            try
            {
                Terminal.Output("Twitch: Client Connecting...");
                Logs.Instance.NewLog(LogLevel.Info, "Connecting to Twitch Client...");
                ConnectionCredentials credentials = new ConnectionCredentials(botUsername, botAuthKey);
                var clientOptions = new ClientOptions
                {
                    MessagesAllowedInPeriod = 750,
                    ThrottlingPeriod = TimeSpan.FromSeconds(30)
                };
                WebSocketClient customClient = new WebSocketClient(clientOptions);
                client = new TwitchClient(customClient);
                client.Initialize(credentials, botChannelToJoin);
                client.AddChatCommandIdentifier(commandIdentifier);
                client.OnConnected += onClientConnected;
                client.OnDisconnected += onClientDisconnected;
                client.OnChatCommandReceived += onChatCommand;
                client.OnRaidNotification += onRaid;
                client.OnNewSubscriber += onSubscription;
                client.OnReSubscriber += onResubscription;
                client.OnPrimePaidSubscriber += onPrimePaidSubscription;
                client.OnGiftedSubscription += onGiftedSubscription;
                client.OnContinuedGiftedSubscription += onContinuedGiftedSubscription;
                client.OnMessageReceived += onChatMessage;
                client.Connect();
            }
            catch (Exception ex)
            {
                Terminal.Output("Twitch: Client Failed to Connect -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        // Initiliase Twitch's PubSub connection
        private static void initiliasePubSub()
        {
            try
            {
                Terminal.Output("Twitch: PubSub Connecting...");
                Logs.Instance.NewLog(LogLevel.Info, "Connecting to Twitch Pub Sub...");
                pubSub = new TwitchPubSub();
                pubSub.OnPubSubServiceConnected += onPubSubServiceConnected;
                pubSub.OnPubSubServiceClosed += onPubSubServiceDisconnected;
                pubSub.OnListenResponse += onPubSubListenResponse;
                pubSub.OnFollow += onChannelFollow;
                pubSub.OnBitsReceivedV2 += onChannelBitsV2;
                pubSub.OnChannelPointsRewardRedeemed += onChannelPointsRedeemed;
                //pubSub.OnChannelSubscription += onChannelSubscription;
                pubSub.ListenToFollows(channelID);
                pubSub.ListenToBitsEventsV2(channelID);
                pubSub.ListenToChannelPoints(channelID);
                pubSub.ListenToSubscriptions(channelID);
                pubSub.Connect();
            }
            catch (Exception ex)
            {
                Terminal.Output("Twitch: PubSub Failed to Connect -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        // Initiliase Twitch's API connection
        private static void initialiseAPI()
        {
            Terminal.Output("Twitch: API Connecting...");
            Logs.Instance.NewLog(LogLevel.Info, "Connecting to Twitch API...");
            api = new TwitchAPI();
            api.Settings.ClientId = clientID;
            api.Settings.AccessToken = channelAuthKey;
            //Subscription[] subs = GetSubscriberList().Result;
            //Subscription sub = CheckUserSubscription("106861102").Result;
            //CheckUserSubscription("106861102");//.Result;
            //TwitchSub sub = GetUserSubscriptionTier("756882056").Result;
        }

        #endregion

        #region Twitch Client

        public static bool IsClientConnected
        {
            get
            {
                if(client != null) return client.IsConnected;
                else return false;
            }
        }

        // Called when the client successfully connects to Twitch
        private static void onClientConnected(object sender, OnConnectedArgs e)
        {
            Terminal.Output("Twitch: Client Connected");
            Logs.Instance.NewLog(LogLevel.Info, "Connected to Twitch Client...");
        }

        // Called when the client disconnects from Twitch
        private static void onClientDisconnected(object sender, OnDisconnectedEventArgs e)
        {
            Terminal.Output("Twitch: Client Disconnected, Attempting to Reconnect...");
            Logs.Instance.NewLog(LogLevel.Info, "Disconnected from Twitch Client: " + e);
            initiliaseClient();
        }

        // Called on a message event, calls on the ScamMessageDetector to check for a scam message
        private static void onChatMessage(object sender, OnMessageReceivedArgs e)
        {
            Terminal.Output("Twitch: Message -> " + e.ChatMessage.DisplayName + ", " + e.ChatMessage.Message);
            Logs.Instance.NewLog(LogLevel.Info, "Twitch Message -> " + e.ChatMessage.DisplayName + ", " + e.ChatMessage.Message);
            scamMessageDetector.CheckChatMessage(e);
        }

        // Called on a command event, checks if command is custom or not before passing the event info to the eventHandler
        private static void onChatCommand(object sender, OnChatCommandReceivedArgs e)
        {
            if (lakeaCommands.CheckIfCommandIsLakeaCommand(e.Command.CommandText))
            {
                Terminal.Output("Twitch: Default Command -> " + e.Command.CommandIdentifier + e.Command.CommandText);
                Logs.Instance.NewLog(LogLevel.Info, "Default Command -> " + e.Command.CommandIdentifier + e.Command.CommandText);
                eventHandler.NewEvent(new LakeaCommand(EventSource.Twitch, EventType.Lakea_Command, e));
            }
            else
            {
                Terminal.Output("Twitch: Command -> " + e.Command.CommandIdentifier + e.Command.CommandText);
                Logs.Instance.NewLog(LogLevel.Info, "Custom Command -> " + e.Command.CommandIdentifier + e.Command.CommandText);
                eventHandler.NewEvent(new TwitchCommand(EventSource.Twitch, EventType.Twitch_Command, e));
            }
        }

        // Called on a command event, passes event info to the eventHandler
        private static void onRaid(object sender, OnRaidNotificationArgs e)
        {
            Terminal.Output("Twitch: Raid -> " + e.RaidNotification.DisplayName);
            Logs.Instance.NewLog(LogLevel.Info, "Twitch Raid -> " + e.RaidNotification.DisplayName);
            eventHandler.NewEvent(new TwitchRaid(EventSource.Twitch, EventType.Twitch_Raid, e));
        }

        // Called on a subscription event, passes event info to the eventHandler
        private static void onSubscription(object sender, OnNewSubscriberArgs e)
        {
            Terminal.Output("Twitch: Subscription -> " + e.Subscriber.DisplayName + ", " + e.Subscriber.SubscriptionPlanName);
            Logs.Instance.NewLog(LogLevel.Info, "Twitch Subscription -> " + e.Subscriber.DisplayName + ", " + e.Subscriber.SubscriptionPlanName);
            eventHandler.NewEvent(new TwitchClientSubscription(EventSource.Twitch, EventType.Twitch_Subscription, e));
        }

        // Called on a resubscription event, passes event info to the eventHandler
        private static void onResubscription(object sender, OnReSubscriberArgs e)
        {
            Terminal.Output("Twitch: Resubscription -> " + e.ReSubscriber.DisplayName + ", " + e.ReSubscriber.SubscriptionPlanName);
            Logs.Instance.NewLog(LogLevel.Info, "Twitch Resubscription -> " + e.ReSubscriber.DisplayName + ", " + e.ReSubscriber.SubscriptionPlanName);
            eventHandler.NewEvent(new TwitchClientResubscriptioncs(EventSource.Twitch, EventType.Twitch_Resubscription, e));
        }

        // Called on a prime paid subscription event, passes event info to the eventHandler
        private static void onPrimePaidSubscription(object sender, OnPrimePaidSubscriberArgs e)
        {
            Terminal.Output("Twitch: Prime Paid Subscription -> " + e.PrimePaidSubscriber.DisplayName + ", " + e.PrimePaidSubscriber.SubscriptionPlanName);
            Logs.Instance.NewLog(LogLevel.Info, "Twitch Prime Paid Subscription -> " + e.PrimePaidSubscriber.DisplayName + ", " + e.PrimePaidSubscriber.SubscriptionPlanName);
            eventHandler.NewEvent(new TwitchClientPrimePaidSubscription(EventSource.Twitch, EventType.Twitch_Prime_Paid_Subscription, e));
        }

        // Called on a gifted subscription event, passes event info to the eventHandler
        private static void onGiftedSubscription(object sender, OnGiftedSubscriptionArgs e)
        {
            Terminal.Output("Twitch: Gifted Subscription -> " + e.GiftedSubscription.DisplayName + ", " + e.GiftedSubscription.MsgParamSubPlanName);
            Logs.Instance.NewLog(LogLevel.Info, "Twitch Gifted Subscription -> " + e.GiftedSubscription.DisplayName + ", " + e.GiftedSubscription.MsgParamSubPlanName);
            eventHandler.NewEvent(new TwitchClientGiftedSubscription(EventSource.Twitch, EventType.Twitch_Gifted_Subscription, e));
        }

        // Called on a continued gift subscription event, passes event info to the event handler
        private static void onContinuedGiftedSubscription(object sender, OnContinuedGiftedSubscriptionArgs e)
        {
            Terminal.Output("Twitch: Continued Gifted Subscription -> " + e.ContinuedGiftedSubscription.DisplayName);
            Logs.Instance.NewLog(LogLevel.Info, "Twitch Continued Gifted Subscription -> " + e.ContinuedGiftedSubscription.DisplayName);
            eventHandler.NewEvent(new TwitchClientContinuedGiftSubscription(EventSource.Twitch, EventType.Twitch_Continued_Gifted_Subscription, e));
        }

        // Write a message to Twitch chat
        public static void WriteToChat(string message)
        {
            try
            {
                Terminal.Output("Twitch: Sending Message -> '" + message + "'");
                Logs.Instance.NewLog(LogLevel.Info, "Twitch Send Chat Message -> " + message);
                client.SendMessage(client.JoinedChannels[0], $"" + message);
            }
            catch(Exception ex)
            {
                Terminal.Output("Twitch: Error Sending Chat Message -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        // Reply to a Twitch chat message
        public static void ReplyToChatMessage(string messageID, string reply)
        {
            try
            {
                Terminal.Output("Twitch: Replying To Message -> '" + reply + "'");
                Logs.Instance.NewLog(LogLevel.Info, "Twitch Send Chat Message Reply -> " + reply);
                client.SendReply(client.JoinedChannels[0], messageID, $"" + reply);
            }
            catch (Exception ex)
            {
                Terminal.Output("Twitch: Error Replying to Chat Message -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        // Write a whisper message to a Twitch user
        //Currently not working, Todo issue #70
        public static void WriteWhisperToUser(string message, string user)
        {
            Terminal.Output("Twitch: Sending Whisper -> '" + user + "' - '" + message + "'");
            Logs.Instance.NewLog(LogLevel.Info, "Twitch Send Whisper Message -> '" + user + "' - '" + message + "'");
            client.SendWhisper(user, message, true);//https://wiki.streamer.bot/en/Sub-Actions/Code/CSharp/Available-Methods/Twitch#whisper
        }

        #endregion

        #region Twitch Pubsub

        // When disconnected from the PubSub service
        private static void onPubSubServiceDisconnected(object sender, EventArgs e)
        {
            pubSubConnected = false;
            Terminal.Output("Twitch: PubSub Disconnected, Attempting to Reconnect...");
            Logs.Instance.NewLog(LogLevel.Info, "PubSub Disconnected");
            initiliasePubSub();
        }

        // Once connected, send auth key to verify connection
        private static void onPubSubServiceConnected(object sender, EventArgs e)
        {
            try
            {
                Terminal.Output("Twitch: PubSub Connected, Sending Auth Key...");
                Logs.Instance.NewLog(LogLevel.Info, "Sending Twitch PubSub Auth Key...");
                pubSub.SendTopics(channelAuthKey);
            }
            catch (Exception ex)
            {
                Terminal.Output("Twitch: Failed to send PubSub Auth Key -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        //Listen for if connection and auth key were succesful
        private static void onPubSubListenResponse(object sender, OnListenResponseArgs e)
        {
            if (e.Successful)
            {
                pubSubConnected = true;
                Terminal.Output("Twitch: Auth Key Accepted");
                Logs.Instance.NewLog(LogLevel.Info, "PubSub Auth Key Accepted");
            }
            else
            {
                Terminal.Output("Auth Key Rejected: " + e.Response.Error);
                Logs.Instance.NewLog(LogLevel.Error, e.Response.Error);
            }
        }

        //Called on a follow event, passes event info to the eventHandler
        private static void onChannelFollow(object sender, OnFollowArgs e)
        {
            Terminal.Output("Twitch: Follow -> " + e.DisplayName);
            Logs.Instance.NewLog(LogLevel.Info, "Twitch Follow -> " + e.DisplayName);
            eventHandler.NewEvent(new TwitchFollow(EventSource.Twitch, EventType.Twitch_Follow, e));
        }

        //Called on a bits event, passes event info to the eventHandler
        private static void onChannelBitsV2(object sender, OnBitsReceivedV2Args e)
        {
            Terminal.Output("Twitch: Bits -> " + e.BitsUsed);
            Logs.Instance.NewLog(LogLevel.Info, "Twitch Bits -> " + e.BitsUsed);
            eventHandler.NewEvent(new TwitchBits(EventSource.Twitch, EventType.Twitch_Bits, e));
        }

        //Called on a channel redeem event, passes event info to the eventHandler
        private static void onChannelPointsRedeemed(object sender, OnChannelPointsRewardRedeemedArgs e)
        {
            Terminal.Output("Twitch: Redeem -> " + e.RewardRedeemed.Redemption.Reward.Title);
            Logs.Instance.NewLog(LogLevel.Info, "Twitch Channel Redeem -> " + e.RewardRedeemed.Redemption.Reward.Title);
            eventHandler.NewEvent(new TwitchRedeem(EventSource.Twitch, EventType.Twitch_Redeem, e));
        }


        // Commented out to test Twitch Client Subscriptions
        // Called on a channel subscription, passes event info to the eventHandler
        //private static void onChannelSubscription(object sender, OnChannelSubscriptionArgs e)
        //{
        //    Terminal.Output("Twitch: Subscription -> " + e.Subscription.DisplayName);
        //    Logs.Instance.NewLog(LogLevel.Info, "Twitch Subscription -> " + e.Subscription);
        //    eventHandler.NewEvent(new TwitchPubSubSubscription(EventSource.Twitch, EventType.Twitch_Subscription, e));
        //}

        #endregion

        #region Twitch API

        // Create a new Channel Redeem
        public async static Task<CreateCustomRewardsResponse> CreateChannelRedeem(CreateCustomRewardsRequest requestData)
        {
            try
            {
                CreateCustomRewardsResponse response = await api.Helix.ChannelPoints.CreateCustomRewardsAsync(channelID, requestData, channelAuthKey);
                return response;
            }
            catch (Exception ex)
            {
                Terminal.Output("Twitch: Error Creating Channel Redeem -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }

        // Update a channel redeem
        public async static Task<UpdateCustomRewardResponse> UpdateChannelRedeem(string redeemID, UpdateCustomRewardRequest requestData)
        {
            try
            {
                UpdateCustomRewardResponse response = await api.Helix.ChannelPoints.UpdateCustomRewardAsync(channelID, redeemID, requestData, channelAuthKey);
                return response;
            }
            catch (Exception ex)
            {
                Terminal.Output("Twitch: Error Updating Channel Redeem -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }

        // Delete a channel redeem
        public async static void DeleteChannelRedeem(string redeemID)
        {
            try
            {
                await api.Helix.ChannelPoints.DeleteCustomRewardAsync(channelID, redeemID, channelAuthKey);
            }
            catch (Exception ex)
            {
                Terminal.Output("Twitch: Error Deleting Channel Redeem -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        // Get stream information such as stream title or game category
        public async static Task<GetChannelInformationResponse> GetChannelInformation()
        {
            try
            {
                GetChannelInformationResponse response = await api.Helix.Channels.GetChannelInformationAsync(channelID, channelAuthKey);
                return response;
            }
            catch (Exception ex)
            {
                Terminal.Output("Twitch: Error Getting Stream Information -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }

        // Update stream information such as stream title or game category
        public async static void UpdateChannelInformation(ModifyChannelInformationRequest request)
        {
            try
            {
                await api.Helix.Channels.ModifyChannelInformationAsync(channelID, request, channelAuthKey);
            }
            catch (Exception ex)
            {
                Terminal.Output("Twitch: Error Updating Stream Title -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        // Get stream category data from Twitch
        public async static Task<GetGamesResponse> GetCategoryInformation(List<string> games)
        {
            try
            {
                GetGamesResponse response = await api.Helix.Games.GetGamesAsync(gameNames: games, accessToken: channelAuthKey);
                return response;
            }
            catch (Exception ex)
            {
                Terminal.Output("Twitch: Error Getting Category Information -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }

        //Get list of channel subscribers
        //public async static Task<Subscription[]> GetSubscriberList(int size = 20)
        //{
        //    try
        //    {
        //        Terminal.Output("Twitch: Fetching Subscriber List...");
        //        Logs.Instance.NewLog(LogLevel.Info, "Twitch Fetch Subscriber List...");
        //        var allSubscriptions = await api.Helix.Subscriptions.GetBroadcasterSubscriptionsAsync(channelID, size, null, channelAuthKey);
        //        return allSubscriptions.Data;
        //    }
        //    catch (Exception ex)
        //    {
        //        Terminal.Output("Twitch: Failed to Fetch Subscriber List -> " + ex.Message);
        //        Logs.Instance.NewLog(LogLevel.Error, ex);
        //    }
        //    return new Subscription[0];
        //}

        //Get user subscription state
        //public async static Task<Subscription> CheckUserSubscription(string userID)
        //public async static void CheckUserSubscription(string userID)
        //{
        //    try
        //    {
        //        Terminal.Output("Twitch: Checking User Subscription...");
        //        Logs.Instance.NewLog(LogLevel.Info, "Twitch Checking User Subscription...");
        //        var subscription = await api.Helix.Subscriptions.CheckUserSubscriptionAsync(channelID, userID, channelAuthKey);
        //        //var subscription = await api.Helix.Subscriptions.CheckUserSubscriptionAsync(channelID, userID);
        //        int i = 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        Terminal.Output("Twitch: Failed to Check User Subscription -> " + ex.Message);
        //        Logs.Instance.NewLog(LogLevel.Error, ex);
        //    }
        //    //return null;
        //}

        public async static Task<TwitchSubTier> GetUserSubscriptionTier(string userID)
        {
            try
            {
                Terminal.Output("Twitch: Fetching User Subscription...");
                Logs.Instance.NewLog(LogLevel.Info, "Twitch Fetch User Subscription...");
                var allSubscriptions = await api.Helix.Subscriptions.GetBroadcasterSubscriptionsAsync(channelID, 100, null, channelAuthKey);
                string tierString = "NONE";
                foreach (Subscription sub in allSubscriptions.Data)
                {
                    if (userID.Equals(sub.UserId))
                    {
                        tierString = sub.Tier;
                        break;
                    }
                }
                switch (tierString)
                {
                    case "1000":
                        return TwitchSubTier.Tier_1;
                    case "2000":
                        return TwitchSubTier.Tier_2;
                    case "3000":
                        return TwitchSubTier.Tier_3;
                    default:
                        return TwitchSubTier.None;
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Twitch: Failed to Fetch User Subscription -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return TwitchSubTier.None;
        }

        // Delete a message from Twitch chat
        public static async void DeleteChatMessage(string messageID)
        {
            try
            {
                Terminal.Output("Twitch: Deleting Chat Message...");
                Logs.Instance.NewLog(LogLevel.Info, "Twitch Deleting Chat Message...");
                await api.Helix.Moderation.DeleteChatMessagesAsync(channelID, channelID, messageID);
            }
            catch(Exception ex)
            {
                Terminal.Output("Twitch: Failed to Delete Chat Message -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        // Ban a user from Twitch chat for a specified reason
        public static async void BanChatUser(string accountID, string reason)
        {
            try
            {
                BanUserRequest request = new BanUserRequest();
                request.UserId = accountID;
                request.Reason = reason;
                Terminal.Output("Twitch: Banning User from Chat...");
                Logs.Instance.NewLog(LogLevel.Info, "Twitch Banning User from Chat...");
                await api.Helix.Moderation.BanUserAsync(channelID, channelID, request, channelAuthKey);
            }
            catch (Exception ex)
            {
                Terminal.Output("Twitch: Failed to Ban User from Chat -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        #endregion
    }
}
