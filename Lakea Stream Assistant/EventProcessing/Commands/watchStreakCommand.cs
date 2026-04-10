using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;

namespace Lakea_Stream_Assistant.EventProcessing.Commands
{
    public class watchStreakCommand : CommandBase
    {
        public override EventItem Run(IncomingEvent eve)
        {
            string displayName = eve.Args["DisplayName"];
            Terminal.Output($"Lakea: Watch Streak Command -> {displayName}");
            Logs.Instance.NewLog(LogLevel.Info, $"Watch Streak Command -> {displayName}");
            WatchStreakDataStreak userStreaks = Twitch.GetUserWatchStreak(eve.Args["AccountID"]);
            Dictionary<string, string> args = new Dictionary<string, string>()
            {
                { "Message", $"{displayName} has a watch streak of {userStreaks.CurrentStreak}! Thank you for regularly tuning in!" }
            };
            return new EventItem(eve.Source, EventType.Lakea_Command, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message, "Watch Streak Command", "Lakea_Watch_Streak_Command", args: args);
        }
    }
}
