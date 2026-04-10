using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;

namespace Lakea_Stream_Assistant.EventProcessing.Commands
{
    public class BoopCommand : CommandBase
    {
        private List<string> replies = new List<string>()
        {
            "Excuse me, do I look like I appreciate being booped?"
        };

        public override EventItem Run(IncomingEvent eve)
        {
            Terminal.Output("Lakea: Status Command -> Active");
            Logs.Instance.NewLog(LogLevel.Info, "Status Command -> Active");
            int index = Dice.Roll(replies.Count);
            string message = replies[index];
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "Message", message }
            };
            return new EventItem(eve.Source, EventType.Lakea_Command, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message, "Status Command", "Lakea_Status_Command", args: args);
        }
    }
}
