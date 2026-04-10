using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;

namespace Lakea_Stream_Assistant.EventProcessing.Commands
{
    public class AnnoucementCommand : CommandBase
    {
        public override EventItem Run(IncomingEvent eve)
        {
            string displayName = eve.Args["DisplayName"];
            Terminal.Output($"Lakea: Announcement Command -> {displayName}");
            Logs.Instance.NewLog(LogLevel.Info, $"Announcement Command -> {displayName}");
            Twitch.SendChatAnnouncement(eve.Args["ArgumentsAsString"]).RunSynchronously();
            return new EventItem(eve.Source, EventType.Twitch_Command, EventTarget.Null, EventGoal.Null, "Announcement Command", "Twitch_Announcement_Command", args: null);
        }
    }
}
