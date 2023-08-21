using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Singletons;

namespace Lakea_Stream_Assistant.EventProcessing.Commands
{
    public class DefaultCommands
    {
        private QuoteCommand quotes;
        private readonly Dictionary<string, Func<LakeaCommand, EventItem>> commands;
        private readonly Dictionary<string, bool> active;

        public DefaultCommands(SettingsCommands commands)
        {
            this.quotes = new QuoteCommand();
            this.commands = new Dictionary<string, Func<LakeaCommand, EventItem>>
            {
                { "status", statusCommand },
                { "quote", quoteCommand },
                { "quotecount", quoteCommand },
                { "addquote", quoteCommand },
                { "quoteadd", quoteCommand },
                { "quotefest", quoteCommand }
            };
            this.active = new Dictionary<string, bool>
            {
                { "status", commands.Status },
                { "quote", commands.Quotes },
                { "quotecount", commands.Quotes },
                { "addquote", commands.Quotes },
                { "quoteadd", commands.Quotes },
                { "quotefest", commands.Quotes }
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
                    Console.WriteLine("Lakea: Default Command " + eve.Identifier + eve.Command + " is Disabled");
                    Logs.Instance.NewLog(LogLevel.Info, "Default Command " + eve.Identifier + eve.Command + " is Disabled");
                    return new EventItem(eve.Source, EventType.Lakea_Command, EventTarget.Null, EventGoal.Null, eve.Identifier + eve.Command);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lakea: Default Command Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }

        private EventItem statusCommand(LakeaCommand eve)
        {
            Console.WriteLine("Lakea: Status Command -> Active");
            Logs.Instance.NewLog(LogLevel.Info, "Status Command -> Active");
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "Message", "I'm still active, all is well Cuppa" }
            };
            return new EventItem(eve.Source, EventType.Lakea_Command, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message, "Status Command", "Lakea_Status_Command", args: args);
        }

        private EventItem quoteCommand(LakeaCommand eve)
        {
            Console.WriteLine("Lakea: Quote Command -> " + eve.Command);
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
    }
}
