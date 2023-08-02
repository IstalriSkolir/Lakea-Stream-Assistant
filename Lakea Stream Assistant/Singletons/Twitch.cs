using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using TwitchLib.PubSub;
using TwitchLib.PubSub.Events;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Client.Events;

namespace Lakea_Stream_Assistant.Singletons
{
    public sealed class Twitch
    {
        //public static bool Initiliased = false;
        public static Tuple<int, int> ServicesConnected = Tuple.Create(0, 2);
        private static HandleEvents eventHandler;
        private static TwitchPubSub pubSub;
        private static TwitchClient client;
        private static string channelUsername;
        private static string channelID;
        private static string channelAuthKey;
        private static string botUsername;
        private static string botAuthKey;
        private static string botChannelToJoin;

        #region Initiliase

        //Initiliases the Singleton by connecting to Twitch with the settings in the config object
        public static void Init(Config config, HandleEvents newEventsObj)
        {
            try
            {
                eventHandler = newEventsObj;
                channelUsername = config.Twitch.StreamingChannel.UserName;
                channelID = config.Twitch.StreamingChannel.ID.ToString();
                channelAuthKey = config.Twitch.StreamingChannel.AuthKey;
                botUsername = config.Twitch.BotChannel.UserName;
                botAuthKey = config.Twitch.BotChannel.UserToken;
                botChannelToJoin = config.Twitch.BotChannel.ChannelConnection;
                initiliasePubSub();
                initiliaseClient();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Twitch: Failed to Connect -> " + ex.Message);
                Console.ReadLine();
                Environment.Exit(1);
            }
        }

        //Initiliase Twitch's PubSub connection
        private static void initiliasePubSub()
        {
            try
            {
                Console.WriteLine("Twitch: PubSub Connecting...");
                pubSub = new TwitchPubSub();
                pubSub.OnPubSubServiceConnected += onPubSubServiceConnected;
                pubSub.OnListenResponse += onPubSubListenResponse;
                pubSub.OnChannelPointsRewardRedeemed += onChannelPointsRedeemed;
                pubSub.ListenToChannelPoints(channelID);
                pubSub.Connect();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Twitch: PubSub Failed to Connect -> " + ex.Message);
            }
        }

        //Initiliase Twitch's Client connection
        private static void initiliaseClient()
        {
            try
            {
                Console.WriteLine("Twitch: Client Connecting...");
                ConnectionCredentials crednetials = new ConnectionCredentials(botUsername, botAuthKey);
                var clientOptions = new ClientOptions
                {
                    MessagesAllowedInPeriod = 750,
                    ThrottlingPeriod = TimeSpan.FromSeconds(30)
                };
                WebSocketClient customClient = new WebSocketClient(clientOptions);
                client = new TwitchClient(customClient);
                client.Initialize(crednetials, botChannelToJoin);
                client.OnConnected += onClientConnected;
                client.Connect();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Twitch: Client Failed to Connect -> " + ex.Message);
            }
        }

        #endregion

        #region Twitch Client

        //Called when the client successfully connects to Twitch
        private static void onClientConnected(object sender, OnConnectedArgs e)
        {
            Console.WriteLine("Twitch: Client Connected");
            ServicesConnected = Tuple.Create(ServicesConnected.Item1 + 1, ServicesConnected.Item2);
        }

        //Write a message to Twitch chat
        public static void WriteToChat(string message)
        {
            Console.WriteLine("Twitch: Sending Message -> '" + message + "'");
            client.SendMessage(client.JoinedChannels[0], $"" + message);
        }

        #endregion

        #region Twitch Pubsub

        //Once connected, send auth key to verify connection
        private static void onPubSubServiceConnected(object sender, EventArgs e)
        {
            try
            {
                Console.WriteLine("Twitch: Sending PubSub Auth Key...");
                pubSub.SendTopics(channelAuthKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Twitch: Failed to send PubSub Auth Key -> " + ex.Message);
                Console.ReadLine();
                Environment.Exit(1);
            }
        }

        //Listen for if connection and auth key were succesful
        private static void onPubSubListenResponse(object sender, OnListenResponseArgs e)
        {
            if (!e.Successful)
            {
                Console.WriteLine("Failed to connect to Twitch PubSub: " + e.Response.Error);
                Console.ReadLine();
                Environment.Exit(1);
            }
            else
            {
                Console.WriteLine("Twitch: PubSub Connected");
                //Initiliased = true;
                ServicesConnected = Tuple.Create(ServicesConnected.Item1 + 1, ServicesConnected.Item2);
            }
        }

        //Called on a channel redeem event, passes event info the eventHandler
        private static void onChannelPointsRedeemed(object sender, OnChannelPointsRewardRedeemedArgs e)
        {
            Console.WriteLine("Twitch: Redeem -> " + e.RewardRedeemed.Redemption.Reward.Title);
            eventHandler.NewEvent(new TwitchRedeem(EventSource.Twitch, TwitchEventType.Redeem, e));
        }

        #endregion
    }
}
