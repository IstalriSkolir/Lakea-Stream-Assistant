using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Models.Tokens;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;

namespace Lakea_Stream_Assistant.EventProcessing.Commands
{
    public class ExitCommand : CommandBase
    {
        private KeepAliveToken token;

        public ExitCommand(KeepAliveToken token)
        {
            this.token = token;
        }
        public override EventItem Run(IncomingEvent eve)
        {
            string command = eve.Args["CommandText"];
            string displayName = eve.Args["DisplayName"];
            string channel = eve.Args["Channel"];
            bool isBroadcaster = bool.Parse(eve.Args["IsBroadcaster"]);
            Terminal.Output($"Lakea: Exit Command -> {command}");
            Logs.Instance.NewLog(LogLevel.Info, $"Exit Command -> {command}");
            if (isBroadcaster)
            {
                token.Kill();
                return null;
            }
            else
            {
                Terminal.Output($"Lakea: Exit Command -> Access Denied, {displayName}");
                Logs.Instance.NewLog(LogLevel.Warning, $"Exit Command -> Access Denied, {displayName}");
                Dictionary<string, string> args = new Dictionary<string, string>
                {
                    { "Message", $"Sorry {displayName}, only {channel} can use that command!" }
                };
                return new EventItem(eve.Source, EventType.Lakea_Command, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message, "Exit Command", "Lakea_Exit_Command", args: args);
            }
        }
    }
}
