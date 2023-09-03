using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Configuration;
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
        private readonly Dictionary<string, Func<LakeaCommand, EventItem>> commandFunctions;
        private readonly Dictionary<string, CommandConfiguration> commandConfigs;
        KeepAliveToken keepAliveToken;

        public DefaultCommands(SettingsCommands commands, KeepAliveToken keepAliveToken)
        {
            this.quotes = new QuoteCommand();
            this.keepAliveToken = keepAliveToken;
            this.commandFunctions = new Dictionary<string, Func<LakeaCommand, EventItem>>
            {
                { "exit", exitCommand },
                { "quote", quoteCommand },
                { "quotecount", quoteCommand },
                { "addquote", quoteCommand },
                { "quoteadd", quoteCommand },
                { "quotefest", quoteCommand },
                { "status", statusCommand }
            };
            this.commandConfigs = new Dictionary<string, CommandConfiguration>
            {
                { "exit", new CommandConfiguration("Exit", commands.Exit.Enabled, commands.Exit.ModOnly) },
                { "quote", new CommandConfiguration("Quote", commands.Quotes.Enabled, commands.Quotes.ModOnly) },
                { "quotecount", new CommandConfiguration("QuoteCount", commands.Quotes.Enabled, commands.Quotes.ModOnly) },
                { "addquote", new CommandConfiguration("AddQuote", commands.Quotes.Enabled, commands.Quotes.ModOnly) },
                { "quoteadd", new CommandConfiguration("AddQuote", commands.Quotes.Enabled, commands.Quotes.ModOnly) },
                { "quotefest", new CommandConfiguration("QuoteFest", commands.Quotes.Enabled, commands.Quotes.ModOnly) },
                { "status", new CommandConfiguration("Status", commands.Status.Enabled, commands.Status.ModOnly) }
            };
        }

        public bool CheckIfCommandIsLakeaCommand(string command)
        {
            if (commandFunctions.ContainsKey(command)) return true;
            else return false;
        }

        public EventItem NewLakeaCommand(LakeaCommand eve)
        {
            try
            {
                string command = eve.Args.Command.CommandText.ToLower();
                if (commandConfigs[command].IsEnabled)
                {
                    if (commandConfigs[command].ModOnly)
                    {
                        if (eve.Args.Command.ChatMessage.IsModerator || eve.Args.Command.ChatMessage.IsBroadcaster)
                        {
                            return commandFunctions[command].Invoke(eve);
                        }
                        else
                        {
                            Terminal.Output("Lakea: " + eve.Args.Command.CommandText + " Command -> Access Denied, " + eve.Args.Command.ChatMessage.DisplayName);
                            Logs.Instance.NewLog(LogLevel.Warning, eve.Args.Command.CommandText + " Command -> Access Denied, " + eve.Args.Command.ChatMessage.DisplayName);
                            Dictionary<string, string> args = new Dictionary<string, string>
                            {
                                { "Message", "Sorry @" + eve.Args.Command.ChatMessage.DisplayName + ", only moderators can use that command!" }
                            };
                            return new EventItem(eve.Source, EventType.Lakea_Command, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message, eve.Args.Command.CommandText + " Command", "Lakea_Command_Access_Denied", args: args);
                        }
                    }
                    else
                    {
                        return commandFunctions[command].Invoke(eve);
                    }
                }
                else
                {
                    Terminal.Output("Lakea: Default Command " + eve.Args.Command.CommandIdentifier + eve.Args.Command.CommandText + " is Disabled");
                    Logs.Instance.NewLog(LogLevel.Info, "Default Command " + eve.Args.Command.CommandIdentifier + eve.Args.Command.CommandText + " is Disabled");
                    return new EventItem(eve.Source, EventType.Lakea_Command, EventTarget.Null, EventGoal.Null, eve.Args.Command.CommandIdentifier + eve.Args.Command.CommandText);
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
            Terminal.Output("Lakea: Quote Command -> " + eve.Args.Command.CommandText);
            Logs.Instance.NewLog(LogLevel.Info, "Quote Command -> " + eve.Args.Command.CommandText);
            Dictionary<string, string> args = quotes.NewQuote(eve);
            if ("quotefest".Equals(eve.Args.Command.CommandText))
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
            Terminal.Output("Lakea: Exit Command -> " + eve.Args.Command.CommandText);
            Logs.Instance.NewLog(LogLevel.Info, "Exit Command -> " + eve.Args.Command.CommandText);
            if (eve.Args.Command.ChatMessage.IsBroadcaster)
            {
                keepAliveToken.Kill();
                return null;
            }
            else
            {
                Terminal.Output("Lakea: Exit Command -> Access Denied, " + eve.Args.Command.ChatMessage.DisplayName);
                Logs.Instance.NewLog(LogLevel.Warning, "Exit Command -> Access Denied, " + eve.Args.Command.ChatMessage.DisplayName);
                Dictionary<string, string> args = new Dictionary<string, string>
                {
                    { "Message", "Sorry @" + eve.Args.Command.ChatMessage.DisplayName + ", only @" + eve.Args.Command.ChatMessage.Channel + " can use that command!" }
                };
                return new EventItem(eve.Source, EventType.Lakea_Command, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message, "Exit Command", "Lakea_Exit_Command", args: args);
            }
        }
    }
}
