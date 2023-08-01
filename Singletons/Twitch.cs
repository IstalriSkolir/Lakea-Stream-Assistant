using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using TwitchLib.PubSub;
using TwitchLib.PubSub.Events;

namespace Lakea_Stream_Assistant.Singletons
{
    public sealed class Twitch
    {
        public static bool Initiliased = false;
        private static HandleEvents eventHandler;
        private static TwitchPubSub client;
        private static string channelUsername;
        private static string channelID;
        private static string channelAuthKey;

        public static void Init(Config config, HandleEvents newEventsObj)
        {
            try
            {
                Console.WriteLine("Twitch: Connecting...");
                eventHandler = newEventsObj;
                channelUsername = config.Twitch.StreamingChannel.Username;
                channelID = config.Twitch.StreamingChannel.ID.ToString();
                channelAuthKey = config.Twitch.StreamingChannel.AuthKey;
                client = new TwitchPubSub();
                client.OnPubSubServiceConnected += onPubSubServiceConnected;
                client.OnListenResponse += onListenResponse;
                client.OnChannelPointsRewardRedeemed += onChannelPointsRedeemed;
                client.ListenToChannelPoints(channelID);

                client.Connect();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Twitch: Failed to Connect -> " + ex.Message);
                Console.ReadLine();
                Environment.Exit(1);
            }
        }

        private static void onPubSubServiceConnected(object sender, EventArgs e)
        {
            try
            {
                Console.WriteLine("Twitch: Sending Auth Key...");
                client.SendTopics(channelAuthKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Twitch: Failed to send Auth Key -> " + ex.Message);
                Console.ReadLine();
                Environment.Exit(1);
            }
        }

        private static void onListenResponse(object sender, OnListenResponseArgs e)
        {
            if (!e.Successful)
            {
                Console.WriteLine("Failed to connect to Twitch: " + e.Response.Error);
                Console.ReadLine();
                Environment.Exit(1);
            }
            else
            {
                Console.WriteLine("Twitch: Connected");
                Initiliased = true;
            }
        }

        private static void onChannelPointsRedeemed(object sender, OnChannelPointsRewardRedeemedArgs e)
        {
            Console.WriteLine("Twitch: Redeem -> " + e.RewardRedeemed.Redemption.Reward.Title);
            eventHandler.NewEvent(new TwitchRedeem(EventSource.Twitch, TwitchEventType.Redeem, e));
        }
    }
}
