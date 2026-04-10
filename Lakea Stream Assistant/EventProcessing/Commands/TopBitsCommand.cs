using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;
using TwitchLib.Api.Helix.Models.Bits;

namespace Lakea_Stream_Assistant.EventProcessing.Commands
{
    public class TopBitsCommand : CommandBase
    {
        public override EventItem Run(IncomingEvent eve)
        {
            string displayName = eve.Args["DisplayName"];
            Terminal.Output($"Lakea: Top Bits Command -> {displayName}");
            Logs.Instance.NewLog(LogLevel.Info, $"Top Bits Command -> {displayName}");
            Dictionary<string, string> args = new Dictionary<string, string>();
            GetBitsLeaderboardResponse response = Twitch.GetBitsLeaderBoard(10).Result;
            if (response != null && response.Listings.Length > 0)
            {
                string message = "Top Cheerers! ";
                for (int index = 0; index < response.Listings.Length; index++)
                {
                    message += $"{(index + 1)}. {response.Listings[index].UserName} -> {response.Listings[index].Score} bits, ";
                }
                args.Add("Message", message);
            }
            else
            {
                args.Add("Message", "Error getting top cheerers!");
            }
            return new EventItem(eve.Source, EventType.Lakea_Command, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message, "Top Bits Command", "Lakea_Top_Bits_Command", args: args);
        }
    }
}
