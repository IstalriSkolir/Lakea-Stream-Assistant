using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;

namespace Lakea_Stream_Assistant.EventProcessing.Commands
{
    public class FollowageCommand : CommandBase
    {
        public override EventItem Run(IncomingEvent eve)
        {
            string displayName = eve.Args["DisplayName"];
            Terminal.Output($"Lakea: Followage Command -> {displayName}");
            Logs.Instance.NewLog(LogLevel.Info, $"Followage Command -> {displayName}");
            DateTime followDate = DateTime.Parse(Twitch.GetChannelFollowers(eve.Args["AccountID"], 1).Result.Data[0].FollowedAt);
            var totalDays = (DateTime.UtcNow - followDate).TotalDays;
            Dictionary<string, Double> time = new Dictionary<string, double>()
            {
                { "years", Math.Truncate(totalDays / 365) },
                { "months", Math.Truncate((totalDays % 365) / 30) },
                { "days", Math.Truncate((totalDays % 365) % 30) }
            };
            if (time["years"] == 0) time.Remove("years");
            if (time["months"] == 0) time.Remove("months");
            if (time["days"] == 0) time.Remove("days");
            string followed = string.Empty;
            if (time.Count == 3)
                followed = $"{time["years"]} years, {time["months"]} months and {time["days"]} days";
            else if (time.Count == 2)
                followed = $"{time.First().Value} {time.First().Key}, {time.Last().Value} {time.Last().Key}";
            else
                followed = $"{time.First().Value} {time.First().Key}";
            Dictionary<string, string> args = new Dictionary<string, string>()
            {
                { "Message", $"{displayName} has been following for {followed}!" }
            };
            return new EventItem(eve.Source, EventType.Lakea_Command, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message, "Followage Command", "Lakea_Followage_Command", args: args);
        }
    }
}
