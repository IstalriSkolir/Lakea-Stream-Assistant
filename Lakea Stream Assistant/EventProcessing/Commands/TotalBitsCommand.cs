using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;
using TwitchLib.Api.Helix.Models.Bits;

namespace Lakea_Stream_Assistant.EventProcessing.Commands
{
    public class TotalBitsCommand : CommandBase
    {
        public override EventItem Run(IncomingEvent eve)
        {
            string displayName = eve.Args["DisplayName"];
            Terminal.Output($"Lakea: Total Bits Command -> {displayName}");
            Logs.Instance.NewLog(LogLevel.Info, $"Total Bits Command -> {displayName}");
            Dictionary<string, string> args = new Dictionary<string, string>();
            GetBitsLeaderboardResponse response = Twitch.GetBitsLeaderBoard(1, eve.Args["AccountID"]).Result;
            if (response != null && response.Listings.Length > 0)
            {
                int userBits = response.Listings[0].Score;
                int userRank = response.Listings[0].Rank;
                args.Add("Message", $"{displayName} has cheered a total of {userBits} bits and is rank {userRank} on the leaderboard! Thank you for supporting Materies materi33Lakeaheart");
            }
            else
            {
                args.Add("Message", displayName + " hasn't cheered any bits yet!");
            }
            return new EventItem(eve.Source, EventType.Lakea_Command, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message, "Total Bits Command", "Lakea_Total_Bits_Command", args: args);
        }
    }
}
