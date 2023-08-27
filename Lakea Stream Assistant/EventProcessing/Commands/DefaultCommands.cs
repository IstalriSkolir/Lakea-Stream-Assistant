using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Models.Tokens;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;

namespace Lakea_Stream_Assistant.EventProcessing.Commands
{
    public class DefaultCommands
    {
        private QuoteCommand quotes;
        private readonly Dictionary<string, Func<LakeaCommand, EventItem>> commands;
        private readonly Dictionary<string, bool> active;
        KeepAliveToken keepAliveToken;
        private string broadcaster;

        public DefaultCommands(SettingsCommands commands, KeepAliveToken keepAliveToken, string broadcaster)
        {
            this.quotes = new QuoteCommand();
            this.broadcaster = broadcaster;
            this.keepAliveToken = keepAliveToken;
            this.commands = new Dictionary<string, Func<LakeaCommand, EventItem>>
            {
                { "exit", exitCommand },
                { "quote", quoteCommand },
                { "quotecount", quoteCommand },
                { "addquote", quoteCommand },
                { "quoteadd", quoteCommand },
                { "quotefest", quoteCommand },
                { "status", statusCommand }
            };
            this.active = new Dictionary<string, bool>
            {
                { "exit", commands.Exit },
                { "quote", commands.Quotes },
                { "quotecount", commands.Quotes },
                { "addquote", commands.Quotes },
                { "quoteadd", commands.Quotes },
                { "quotefest", commands.Quotes },
                { "status", commands.Status }
            };
        }

        public bool CheckIfCommandIsLakeaCommand(string command)
        {
            if (commands.ContainsKey(command)) return true;
            else return false;
        }

        public EventItem NewLakeaCommand(LakeaCommand eve)
        {
            try
            {
                string command = eve.Command.ToLower();
                if (active[command])
                {
                    return commands[command].Invoke(eve);
                }
                else
                {
                    Terminal.Output("Lakea: Default Command " + eve.Identifier + eve.Command + " is Disabled");
                    Logs.Instance.NewLog(LogLevel.Info, "Default Command " + eve.Identifier + eve.Command + " is Disabled");
                    return new EventItem(eve.Source, EventType.Lakea_Command, EventTarget.Null, EventGoal.Null, eve.Identifier + eve.Command);
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Default Command Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }

        private EventItem statusCommand(LakeaCommand eve)
        {
            Terminal.Output("Lakea: Status Command -> Active");
            Logs.Instance.NewLog(LogLevel.Info, "Status Command -> Active");
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "Message", "I'm still active, all is well Cuppa" }
            };
            return new EventItem(eve.Source, EventType.Lakea_Command, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message, "Status Command", "Lakea_Status_Command", args: args);
        }

        private EventItem quoteCommand(LakeaCommand eve)
        {
            Terminal.Output("Lakea: Quote Command -> " + eve.Command);
            Logs.Instance.NewLog(LogLevel.Info, "Quote Command -> " + eve.Command);
            Dictionary<string, string> args = quotes.NewQuote(eve);
            if ("quotefest".Equals(eve.Command))
            {
                return new EventItem(eve.Source, EventType.Lakea_Command, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message_List, "Quote Command", "Lakea_Quote_Command", args: args);
            }
            else
            {
                return new EventItem(eve.Source, EventType.Lakea_Command, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message, "Quote Command", "Lakea_Quote_Command", args: args);
            }
        }

        private EventItem exitCommand(LakeaCommand eve)
        {
            Terminal.Output("Lakea: Exit Command -> " + eve.Command);
            Logs.Instance.NewLog(LogLevel.Info, "Exit Command -> " + eve.Command);
            if (broadcaster.Equals(eve.DisplayName))
            {
                keepAliveToken.Kill();
                return null;
            }
            else
            {
                Terminal.Output("Lakea: Exit Command -> Access Denied, " + eve.DisplayName);
                Logs.Instance.NewLog(LogLevel.Warning, "Exit Command -> Access Denied, " + eve.DisplayName);
                Dictionary<string, string> args = new Dictionary<string, string>
                {
                    { "Message", "Sorry @" + eve.DisplayName + ", only @" + broadcaster + " can use that command!" }
                };
                return new EventItem(eve.Source, EventType.Lakea_Command, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message, "Exit Command", "Lakea_Exit_Command", args: args);
            }
        }
    }
}
