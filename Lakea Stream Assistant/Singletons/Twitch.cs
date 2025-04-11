using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.EventProcessing.Processing;
using Lakea_Stream_Assistant.EventProcessing.Commands;
using Lakea_Stream_Assistant.EventProcessing.Misc;
using Lakea_Stream_Assistant.Static;
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
using TwitchLib.Api.Helix.Models.Moderation.BanUser;
using TwitchLib.Api.Helix.Models.Games;
using TwitchLib.EventSub.Websockets.Core.EventArgs;
using TwitchLib.EventSub.Websockets.Core.EventArgs.Channel;
using TwitchLib.EventSub.Websockets.Extensions;
using TwitchLib.EventSub.Websockets;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using TwitchLib.Api.Core.Enums;
using System;

namespace Lakea_Stream_Assistant.Singletons
{
    //Sealed class for Twitch Integration
    public sealed class Twitch
    {
        private static StandardiseInput standardiseInput;
        private static HashChecker hashChecker;
        private static DefaultCommands lakeaCommands;
        private static ScamMessageDetector scamMessageDetector;
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
        private static bool eventSubConnected = false;

        public static bool IsEventSubConnected { get { return eventSubConnected; } set { eventSubConnected = value; } }
        public static string ChannelID { get { return channelID; } }
        public static string ClientID { get { return clientID; } }
        public static string ChannelAuthKey { get { return channelAuthKey; } }

        #region Initiliase

        //Initiliases the Singleton by connecting to Twitch with the settings in the config object
        public static async void Initialise(Config config, DefaultCommands commands)
        {
            try
            {
                standardiseInput = new StandardiseInput();
                hashChecker = new HashChecker();
                scamMessageDetector = new ScamMessageDetector(config.Settings.ScamMessageDetection);
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
                initialiseAPI();
                initialiseEventSub();
            }
            catch (Exception ex)
            {
                Terminal.Output("Fatal Error: Failed to Connect to Twitch -> " + ex.Message);
                Terminal.Output("Terminating Lakea...");
                Logs.Instance.NewLog(Enums.LogLevel.Fatal, ex);
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
                Logs.Instance.NewLog(Enums.LogLevel.Info, "Connecting to Twitch Client...");
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
                Logs.Instance.NewLog(Enums.LogLevel.Error, ex);
            }
        }

        // Initialise Twitch's EventSub connection
        private static void initialiseEventSub()
        {
            initialiseEventSubService().Build().Run();
        }

        private static IHostBuilder initialiseEventSubService() =>
            Host.CreateDefaultBuilder()
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
            })
            .ConfigureServices((hostContext, services) =>
            {
                //services.AddLogging();
                services.AddTwitchLibEventSubWebsockets();
                services.AddHostedService<EventSubService>();
            });

        // Initiliase Twitch's API connection
        private static void initialiseAPI()
        {
            Terminal.Output("Twitch: API Connecting...");
            Logs.Instance.NewLog(Enums.LogLevel.Info, "Connecting to Twitch API...");
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
                if (client != null) return client.IsConnected;
                else return false;
            }
        }

        // Called when the client successfully connects to Twitch
        private static void onClientConnected(object sender, OnConnectedArgs e)
        {
            Terminal.Output("Twitch: Client Connected");
            Logs.Instance.NewLog(Enums.LogLevel.Info, "Connected to Twitch Client...");
        }

        // Called when the client disconnects from Twitch
        private static void onClientDisconnected(object sender, OnDisconnectedEventArgs e)
        {
            Terminal.Output("Twitch: Client Disconnected, Attempting to Reconnect...");
            Logs.Instance.NewLog(Enums.LogLevel.Info, "Disconnected from Twitch Client: " + e);
            initiliaseClient();
        }

        // Called on a message event, calls on the ScamMessageDetector to check for a scam message
        private static void onChatMessage(object sender, OnMessageReceivedArgs e)
        {
            Terminal.Output("Twitch: Message -> " + e.ChatMessage.DisplayName + ", " + e.ChatMessage.Message);
            Logs.Instance.NewLog(Enums.LogLevel.Info, "Twitch Message -> " + e.ChatMessage.DisplayName + ", " + e.ChatMessage.Message);
            scamMessageDetector.CheckChatMessage(e);
        }

        // Called on a command event, checks if command is custom or not before passing the event info to the eventHandler
        private static void onChatCommand(object sender, OnChatCommandReceivedArgs e)
        {
            string propToHash = e.Command.ChatMessage.UserId + e.Command.CommandText + e.Command.ChatMessage.TmiSentTs;
            if (hashChecker.CheckPayloadIsntDuplicate("Twitch Command", propToHash))
            {
                Dictionary<string, string> data = standardiseInput.ConvertTwitchCommandData(e);
                IncomingEvent eve;
                if (lakeaCommands.CheckIfCommandIsLakeaCommand(e.Command.CommandText))
                {
                    Terminal.Output("Twitch: Default Command -> " + e.Command.CommandIdentifier + e.Command.CommandText);
                    Logs.Instance.NewLog(Enums.LogLevel.Info, "Default Command -> " + e.Command.CommandIdentifier + e.Command.CommandText);
                    eve = new IncomingEvent(EventSource.Twitch, EventType.Lakea_Command, data);
                }
                else
                {
                    Terminal.Output("Twitch: Command -> " + e.Command.CommandIdentifier + e.Command.CommandText);
                    Logs.Instance.NewLog(Enums.LogLevel.Info, "Custom Command -> " + e.Command.CommandIdentifier + e.Command.CommandText);
                    eve = new IncomingEvent(EventSource.Twitch, EventType.Twitch_Command, data);
                }
                StreamAssistant.EventHandler.NewEvent(eve);
            }
        }

        // Called on a command event, passes event info to the eventHandler
        private static void onRaid(object sender, OnRaidNotificationArgs e)
        {
            string propToHash = e.RaidNotification.UserId + e.RaidNotification.RoomId + e.RaidNotification.TmiSentTs;
            if (hashChecker.CheckPayloadIsntDuplicate("Twitch Raid", propToHash))
            {
                Terminal.Output("Twitch: Raid -> " + e.RaidNotification.DisplayName);
                Logs.Instance.NewLog(Enums.LogLevel.Info, "Twitch Raid -> " + e.RaidNotification.DisplayName);
                Dictionary<string, string> data = standardiseInput.ConvertTwitchRaidData(e);
                IncomingEvent eve = new IncomingEvent(EventSource.Twitch, EventType.Twitch_Raid, data);
                StreamAssistant.EventHandler.NewEvent(eve);
            }
        }

        // Called on a subscription event, passes event info to the eventHandler
        private static void onSubscription(object sender, OnNewSubscriberArgs e)
        {
            string propToHash = e.Subscriber.UserId + e.Subscriber.SubscriptionPlanName + e.Subscriber.TmiSentTs;
            if (hashChecker.CheckPayloadIsntDuplicate("Twitch Subscriber", propToHash))
            {
                Terminal.Output("Twitch: Subscription -> " + e.Subscriber.DisplayName + ", " + e.Subscriber.SubscriptionPlanName);
                Logs.Instance.NewLog(Enums.LogLevel.Info, "Twitch Subscription -> " + e.Subscriber.DisplayName + ", " + e.Subscriber.SubscriptionPlanName);
                Dictionary<string, string> data = standardiseInput.ConvertTwitchSubscriptionData(e);
                IncomingEvent eve = new IncomingEvent(EventSource.Twitch, EventType.Twitch_Subscription, data);
                StreamAssistant.EventHandler.NewEvent(eve);
            }
        }

        // Called on a resubscription event, passes event info to the eventHandler
        private static void onResubscription(object sender, OnReSubscriberArgs e)
        {
            string propToHash = e.ReSubscriber.UserId + e.ReSubscriber.SubscriptionPlanName + e.ReSubscriber.TmiSentTs;
            if (hashChecker.CheckPayloadIsntDuplicate("Twitch Resubscriber", propToHash))
            {
                Terminal.Output("Twitch: Resubscription -> " + e.ReSubscriber.DisplayName + ", " + e.ReSubscriber.SubscriptionPlanName);
                Logs.Instance.NewLog(Enums.LogLevel.Info, "Twitch Resubscription -> " + e.ReSubscriber.DisplayName + ", " + e.ReSubscriber.SubscriptionPlanName);
                Dictionary<string, string> data = standardiseInput.ConvertTwitchResubscriptionData(e);
                IncomingEvent eve = new IncomingEvent(EventSource.Twitch, EventType.Twitch_Resubscription, data);
                StreamAssistant.EventHandler.NewEvent(eve);
            }
        }

        // Called on a prime paid subscription event, passes event info to the eventHandler
        private static void onPrimePaidSubscription(object sender, OnPrimePaidSubscriberArgs e)
        {
            string propToHash = e.PrimePaidSubscriber.UserId + e.PrimePaidSubscriber.SubscriptionPlanName + e.PrimePaidSubscriber.TmiSentTs;
            if (hashChecker.CheckPayloadIsntDuplicate("Twitch Prime Paid Subscription", propToHash))
            {
                Terminal.Output("Twitch: Prime Paid Subscription -> " + e.PrimePaidSubscriber.DisplayName + ", " + e.PrimePaidSubscriber.SubscriptionPlanName);
                Logs.Instance.NewLog(Enums.LogLevel.Info, "Twitch Prime Paid Subscription -> " + e.PrimePaidSubscriber.DisplayName + ", " + e.PrimePaidSubscriber.SubscriptionPlanName);
                Dictionary<string, string> data = standardiseInput.ConvertTwitchPrimePaidSubscriptionData(e);
                IncomingEvent eve = new IncomingEvent(EventSource.Twitch, EventType.Twitch_Prime_Paid_Subscription, data);
                StreamAssistant.EventHandler.NewEvent(eve);
            }
        }

        // Called on a gifted subscription event, passes event info to the eventHandler
        private static void onGiftedSubscription(object sender, OnGiftedSubscriptionArgs e)
        {
            string propToHash = e.GiftedSubscription.UserId + e.GiftedSubscription.MsgParamRecipientId + e.GiftedSubscription.TmiSentTs;
            if (hashChecker.CheckPayloadIsntDuplicate("Twitch Gifted Subscription", propToHash))
            {
                Terminal.Output("Twitch: Gifted Subscription -> " + e.GiftedSubscription.DisplayName + ", " + e.GiftedSubscription.MsgParamSubPlanName);
                Logs.Instance.NewLog(Enums.LogLevel.Info, "Twitch Gifted Subscription -> " + e.GiftedSubscription.DisplayName + ", " + e.GiftedSubscription.MsgParamSubPlanName);
                Dictionary<string, string> data = standardiseInput.ConvertTwitchGiftedSubscriptionData(e);
                IncomingEvent eve = new IncomingEvent(EventSource.Twitch, EventType.Twitch_Gifted_Subscription, data);
                StreamAssistant.EventHandler.NewEvent(eve);
            }
        }

        // Called on a continued gift subscription event, passes event info to the event handler
        private static void onContinuedGiftedSubscription(object sender, OnContinuedGiftedSubscriptionArgs e)
        {
            string propToHash = e.ContinuedGiftedSubscription.UserId + e.ContinuedGiftedSubscription.RoomId + e.ContinuedGiftedSubscription.TmiSentTs;
            if (hashChecker.CheckPayloadIsntDuplicate("Twitch Continued Gifted Subscription", propToHash))
            {
                Terminal.Output("Twitch: Continued Gifted Subscription -> " + e.ContinuedGiftedSubscription.DisplayName);
                Logs.Instance.NewLog(Enums.LogLevel.Info, "Twitch Continued Gifted Subscription -> " + e.ContinuedGiftedSubscription.DisplayName);
                Dictionary<string, string> data = standardiseInput.ConvertTwitchContinuedGiftedSubscriptionData(e);
                IncomingEvent eve = new IncomingEvent(EventSource.Twitch, EventType.Twitch_Continued_Gifted_Subscription, data);
                StreamAssistant.EventHandler.NewEvent(eve);
            }
        }

        // Write a message to Twitch chat
        public static void WriteToChat(string message)
        {
            try
            {
                Terminal.Output("Twitch: Sending Message -> '" + message + "'");
                Logs.Instance.NewLog(Enums.LogLevel.Info, "Twitch Send Chat Message -> " + message);
                client.SendMessage(client.JoinedChannels[0], $"" + message);
            }
            catch (Exception ex)
            {
                Terminal.Output("Twitch: Error Sending Chat Message -> " + ex.Message);
                Logs.Instance.NewLog(Enums.LogLevel.Error, ex);
            }
        }

        // Reply to a Twitch chat message
        public static void ReplyToChatMessage(string messageID, string reply)
        {
            try
            {
                Terminal.Output("Twitch: Replying To Message -> '" + reply + "'");
                Logs.Instance.NewLog(Enums.LogLevel.Info, "Twitch Send Chat Message Reply -> " + reply);
                client.SendReply(client.JoinedChannels[0], messageID, $"" + reply);
            }
            catch (Exception ex)
            {
                Terminal.Output("Twitch: Error Replying to Chat Message -> " + ex.Message);
                Logs.Instance.NewLog(Enums.LogLevel.Error, ex);
            }
        }

        // Write a whisper message to a Twitch user
        //Currently not working, Todo issue #70
        public static void WriteWhisperToUser(string message, string user)
        {
            Terminal.Output("Twitch: Sending Whisper -> '" + user + "' - '" + message + "'");
            Logs.Instance.NewLog(Enums.LogLevel.Info, "Twitch Send Whisper Message -> '" + user + "' - '" + message + "'");
            //client.SendWhisper(user, message, true);//https://wiki.streamer.bot/en/Sub-Actions/Code/CSharp/Available-Methods/Twitch#whisper
        }

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
                Logs.Instance.NewLog(Enums.LogLevel.Error, ex);
            }
            return null;
        }

        // Update a channel redeem
        public async static Task<UpdateCustomRewardResponse> UpdateChannelRedeem(string redeemID, UpdateCustomRewardRequest requestData)
        {
            try
            {
                UpdateCustomRewardResponse response = await api.Helix.ChannelPoints.UpdateCustomRewardAsync(channelID, redeemID, requestData, channelAuthKey);
                //UpdateCustomRewardResponse response = await api.Helix.ChannelPoints.UpdateCustomRewardAsync(clientID, redeemID, requestData, channelAuthKey);
                return response;
            }
            catch (Exception ex)
            {
                Terminal.Output("Twitch: Error Updating Channel Redeem -> " + ex.Message);
                Logs.Instance.NewLog(Enums.LogLevel.Error, ex);
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
                Logs.Instance.NewLog(Enums.LogLevel.Error, ex);
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
                Logs.Instance.NewLog(Enums.LogLevel.Error, ex);
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
                Logs.Instance.NewLog(Enums.LogLevel.Error, ex);
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
                Logs.Instance.NewLog(Enums.LogLevel.Error, ex);
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
                Logs.Instance.NewLog(Enums.LogLevel.Info, "Twitch Fetch User Subscription...");
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
                Logs.Instance.NewLog(Enums.LogLevel.Error, ex);
            }
            return TwitchSubTier.None;
        }

        // Delete a message from Twitch chat
        public static async void DeleteChatMessage(string messageID)
        {
            try
            {
                Terminal.Output("Twitch: Deleting Chat Message...");
                Logs.Instance.NewLog(Enums.LogLevel.Info, "Twitch Deleting Chat Message...");
                await api.Helix.Moderation.DeleteChatMessagesAsync(channelID, channelID, messageID);
            }
            catch (Exception ex)
            {
                Terminal.Output("Twitch: Failed to Delete Chat Message -> " + ex.Message);
                Logs.Instance.NewLog(Enums.LogLevel.Error, ex);
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
                Logs.Instance.NewLog(Enums.LogLevel.Info, "Twitch Banning User from Chat...");
                await api.Helix.Moderation.BanUserAsync(channelID, channelID, request, channelAuthKey);
            }
            catch (Exception ex)
            {
                Terminal.Output("Twitch: Failed to Ban User from Chat -> " + ex.Message);
                Logs.Instance.NewLog(Enums.LogLevel.Error, ex);
            }
        }

        #endregion
    }

    #region Twitch EventSub

    // Websocket service for Twitch's EventSub
    public class EventSubService : IHostedService
    {
        private readonly EventSubWebsocketClient client;
        private TwitchAPI api;
        private HashChecker hashChecker;
        private StandardiseInput standardiseInput;

        #region Initialise Service

        // Constructor gets Twitch API details as the API is required to request subscriptions for EventSub
        public EventSubService(EventSubWebsocketClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(EventSubWebsocketClient));
            client.WebsocketConnected += OnWebsocketConnected;
            client.WebsocketDisconnected += OnWebsocketDisconnected;
            client.WebsocketReconnected += OnWebsocketReconnected;
            client.ErrorOccurred += OnErrorOccurred;
            client.ChannelFollow += OnFollow;
            client.ChannelPointsCustomRewardRedemptionAdd += OnChannelRedeem;
            client.ChannelCheer += OnBits;
            api = new TwitchAPI();
            api.Settings.ClientId = Twitch.ClientID;
            api.Settings.AccessToken = Twitch.ChannelAuthKey;
            hashChecker = new HashChecker();
            standardiseInput = new StandardiseInput();
        }

        // Start the websocket service
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Terminal.Output("Twitch: EventSub Connecting...");
            Logs.Instance.NewLog(Enums.LogLevel.Info, "EventSub Connecting...");
            await client.ConnectAsync();
        }

        // Stop the websocket service
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Terminal.Output("Twitch: EventSub Disconnecting...");
            Logs.Instance.NewLog(Enums.LogLevel.Info, "EventSub Disconnecting...");
            await client.DisconnectAsync();
        }

        // Once the websocket is connected, send the requests for the required subscriptions using the Twitch API
        private async Task OnWebsocketConnected(object sender, WebsocketConnectedArgs e)
        {
            Twitch.IsEventSubConnected = true;
            Terminal.Output("Twitch: EventSub Connected, Session ID: " + client.SessionId + ", Sending Subscriptions...");
            Logs.Instance.NewLog(Enums.LogLevel.Info, "EventSub Connected, Session ID: " + client.SessionId + ", Sending Subscriptions...");
            if (!e.IsRequestedReconnect)
            {
                try
                {
                    var conditions = new Dictionary<string, string>()
                    {
                        { "broadcaster_user_id", Twitch.ChannelID }
                    };
                    await api.Helix.EventSub.CreateEventSubSubscriptionAsync(
                        "channel.channel_points_custom_reward_redemption.add",
                        "1",
                        conditions,
                        EventSubTransportMethod.Websocket,
                        client.SessionId
                    );
                    await api.Helix.EventSub.CreateEventSubSubscriptionAsync(
                        "channel.cheer",
                        "1",
                        conditions,
                        EventSubTransportMethod.Websocket,
                        client.SessionId
                    );
                    conditions.Add("moderator_user_id", Twitch.ChannelID);
                    var response = await api.Helix.EventSub.CreateEventSubSubscriptionAsync(
                        "channel.follow",
                        "2",
                        conditions,
                        EventSubTransportMethod.Websocket,
                        client.SessionId
                    );
                    Terminal.Output("Twitch: EventSub Subscriptions Sent");
                    Logs.Instance.NewLog(Enums.LogLevel.Info, "EventSub Subscriptions Sent");
                }
                catch (Exception ex)
                {
                    Terminal.Output("Twitch: Error Sending EventSub Subscriptions");
                    Logs.Instance.NewLog(Enums.LogLevel.Info, ex.Message);
                }
            }
        }

        // On websocket disconnect
        private async Task OnWebsocketDisconnected(object sender, EventArgs e)
        {
            Twitch.IsEventSubConnected = false;
            Terminal.Output("Twitch: EventSub Disconnected, Session ID: " + client.SessionId + ", Attempting to Reconnect...");
            Logs.Instance.NewLog(Enums.LogLevel.Info, "EventSub Disconnected, Session ID: " + client.SessionId + ",Attempting to Reconnect...");

            // Don't do this in production. You should implement a better reconnect strategy with exponential backoff
            while (!await client.ReconnectAsync())
            {
                Terminal.Output("Twitch: EventSub Failed to Reconnect");
                Logs.Instance.NewLog(Enums.LogLevel.Error, "EventSub Failed to Reconnect");

                await Task.Delay(1000);
            }
        }

        // On websocker reconnect
        private async Task OnWebsocketReconnected(object sender, EventArgs e)
        {
            Terminal.Output("Twitch: EventSub Reconnected, Session ID: " + client.SessionId);
            Logs.Instance.NewLog(Enums.LogLevel.Info, "EventSub Reconnected, Session ID: " + client.SessionId);
        }

        // On error occuring with the websocket
        private async Task OnErrorOccurred(object sender, ErrorOccuredArgs e)
        {
            Terminal.Output("Twitch: EventSub Error, Session ID: " + client.SessionId + ", " + e.Message);
            Logs.Instance.NewLog(Enums.LogLevel.Error, "EventSub Session ID: " + client.SessionId + ", " + e.Message);
        }

        #endregion

        // Called on a follow event, passes event info to the EventHandler
        private async Task OnFollow(object sender, ChannelFollowArgs e)
        {
            string propToHash = e.Notification.Payload.Event.UserId + e.Notification.Payload.Event.BroadcasterUserId;
            if (hashChecker.CheckPayloadIsntDuplicate("Twitch Follow", propToHash))
            {
                Terminal.Output("Twitch: Follow -> " + e.Notification.Payload.Event.UserName);
                Logs.Instance.NewLog(Enums.LogLevel.Info, "Twitch Follow -> " + e.Notification.Payload.Event.UserName);
                Dictionary<string, string> data = standardiseInput.ConvertTwitchFollowData(e);
                IncomingEvent eve = new IncomingEvent(EventSource.Twitch, EventType.Twitch_Follow, data);
                StreamAssistant.EventHandler.NewEvent(eve);
            }
        }

        // Called on a channel redeem event, passes event info to the EventHandler
        private async Task OnChannelRedeem(object sender, ChannelPointsCustomRewardRedemptionArgs e)
        {
            string propToHash = e.Notification.Payload.Event.Id + e.Notification.Payload.Event.UserId + e.Notification.Payload.Event.RedeemedAt;
            if(hashChecker.CheckPayloadIsntDuplicate("Channel Redeem", propToHash))
            {
                Terminal.Output("Twitch: Redeem -> " + e.Notification.Payload.Event.Reward.Title);
                Logs.Instance.NewLog(Enums.LogLevel.Info, "Twitch Channel Redeem -> " + e.Notification.Payload.Event.Reward.Title);
                Dictionary<string, string> data = standardiseInput.ConvertTwitchRedeemData(e);
                IncomingEvent eve = new IncomingEvent(EventSource.Twitch, EventType.Twitch_Redeem, data);
                StreamAssistant.EventHandler.NewEvent(eve);
            }
        }

        // Called on a cheer event, passes event info to the EventHandler
        private async Task OnBits(object sender, ChannelCheerArgs e)
        {
            string propToHash = e.Notification.Payload.Event.UserId + e.Notification.Payload.Event.Bits + e.Notification.Metadata.MessageTimestamp;
            if (hashChecker.CheckPayloadIsntDuplicate("Twitch Bits", propToHash))
            {
                Terminal.Output("Twitch: Bits -> User: " + e.Notification.Payload.Event.UserName + ", Bits: " + e.Notification.Payload.Event.Bits);
                Logs.Instance.NewLog(Enums.LogLevel.Info, "Twitch Bits -> User: " + e.Notification.Payload.Event.UserName + ", Bits: " + e.Notification.Payload.Event.Bits);
                Dictionary<string, string> data = standardiseInput.ConvertTwitchBitsData(e);
                IncomingEvent eve = new IncomingEvent(EventSource.Twitch, EventType.Twitch_Bits, data);
                StreamAssistant.EventHandler.NewEvent(eve);
            }
        }
    }

    #endregion
}
