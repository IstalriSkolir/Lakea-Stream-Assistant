using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Models.Misc;

namespace Lakea_Stream_Assistant.EventProcessing.Commands
{
    public class TotalPointsCommand : CommandBase
    {
        private string userDirectory;
        
        public TotalPointsCommand(string resourcePath)
        {
            userDirectory = $"{resourcePath}\\TwitchUsers\\";
        }

        public override EventItem Run(IncomingEvent eve)
        {
            JSONIO json = new JSONIO();
            TwitchUser user = json.ReadJSONFile<TwitchUser>($"{userDirectory}{eve.Args["AccountID"]}.json");
            string message = string.Empty;
            if (user != null) message = $"{eve.Args["DisplayName"]} has spent a total of {user.ChannelPoints.TotalSpent} arrows!";
            else message = $"You haven't spent any channel points yet {eve.Args["DisplayName"]}!";
            Dictionary<string, string> args = new Dictionary<string, string>()
            {
                { "Message", message }
            };
            return new EventItem(eve.Source, EventType.Lakea_Command, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message, "Total Points Command", "Lakea_Total_Points_Command", args: args);
        }
    }
}
