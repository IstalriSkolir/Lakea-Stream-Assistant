using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using TwitchLib.PubSub;
using TwitchLib.PubSub.Events;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Client.Events;
using Lakea_Stream_Assistant.EventProcessing;

namespace Lakea_Stream_Assistant.Singletons
{
    public sealed class Twitch
    {
        //public static bool Initiliased = false;
        public static Tuple<int, int> ServicesConnected = Tuple.Create(0, 2);
        private static EventInput eventHandler;
        private static TwitchPubSub pubSub;
        private static TwitchClient client;
        private static string channelUsername;
        private static string channelID;
        private static string channelAuthKey;
        private static string botUsername;
        private static string botAuthKey;
        private static string botChannelToJoin;
        private static char commandIdentifier;

        #region Initiliase

        //Initiliases the Singleton by connecting to Twitch with the settings in the config object
        public static void Init(Config config, EventInput newEventsObj)
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
                commandIdentifier = config.Twitch.CommandIdentifier.ToCharArray()[0];
                initiliaseClient();
                initiliasePubSub();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Twitch: Failed to Connect -> " + ex.Message);
                Console.ReadLine();
                Environment.Exit(1);
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
                client.AddChatCommandIdentifier(commandIdentifier);
                client.OnConnected += onClientConnected;
                client.OnChatCommandReceived += onChatCommand;
                client.Connect();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Twitch: Client Failed to Connect -> " + ex.Message);
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
                pubSub.OnFollow += onChannelFollow;
                pubSub.OnBitsReceivedV2 += onChannelBitsV2;
                pubSub.OnChannelPointsRewardRedeemed += onChannelPointsRedeemed;
                pubSub.ListenToFollows(channelID);
                pubSub.ListenToBitsEventsV2(channelID);
                pubSub.ListenToChannelPoints(channelID);
                pubSub.Connect();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Twitch: PubSub Failed to Connect -> " + ex.Message);
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

        private static void onChatCommand(object sender, OnChatCommandReceivedArgs e)
        {
            Console.WriteLine("Twitch: Command -> " + e.Command.CommandIdentifier + e.Command.CommandText);
            eventHandler.NewEvent(new TwitchCommand(EventSource.Twitch, EventType.Twitch_Command, e));
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

        //Called on a follow event, passes event info to the eventHandler
        private static void onChannelFollow(object sender, OnFollowArgs e)
        {
            Console.WriteLine("Twitch: Follow -> " + e.DisplayName);
            eventHandler.NewEvent(new TwitchFollow(EventSource.Twitch, EventType.Twitch_Follow, e));
        }

        //Called on a bits event, passes event info to the eventHandler
        private static void onChannelBitsV2(object sender, OnBitsReceivedV2Args e)
        {
            Console.WriteLine("Twitch: Bits -> " + e.BitsUsed);
            eventHandler.NewEvent(new TwitchBits(EventSource.Twitch, EventType.Twitch_Bits, e));
        }

        //Called on a channel redeem event, passes event info to the eventHandler
        private static void onChannelPointsRedeemed(object sender, OnChannelPointsRewardRedeemedArgs e)
        {
            Console.WriteLine("Twitch: Redeem -> " + e.RewardRedeemed.Redemption.Reward.Title);
            eventHandler.NewEvent(new TwitchRedeem(EventSource.Twitch, EventType.Twitch_Redeem, e));
        }

        #endregion
    }
}
