using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.EventProcessing.Misc;
using Lakea_Stream_Assistant.Models.Configuration;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Models.Tokens;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;

namespace Lakea_Stream_Assistant.EventProcessing.Commands
{
    // This class handles default commands for Lakea
    public class DefaultCommands
    {
        private readonly Dictionary<string, CommandBase> commandFunctions;
        private readonly Dictionary<string, CommandConfiguration> commandConfigs;
        KeepAliveToken keepAliveToken;

        // Constructor takes object references, sets predefined dictionaries and command active/modonly status
        public DefaultCommands(ConfigSettings settings, ExternalProcesses externalProcesses, KeepAliveToken keepAliveToken)
        {
            QuoteCommand quotes = new QuoteCommand(settings.ResourcePath, settings.Commands.Quotes.QuoteCooldown);
            TotalBitsCommand totalBits = new TotalBitsCommand();
            TopBitsCommand topBits = new TopBitsCommand();
            watchStreakCommand streak = new watchStreakCommand();
            this.keepAliveToken = keepAliveToken;
            this.commandFunctions = new Dictionary<string, CommandBase>
            {
                { "announcement", new AnnoucementCommand() },
                { "boop", new BoopCommand() },
                { "category", new CategoryCommand() },
                { "clip", new ClipCommand() },
                { "exit", new ExitCommand(keepAliveToken) },
                { "followage", new FollowageCommand() },
                { "process", new ProcessCommand(externalProcesses) },
                { "quote", quotes },
                { "quotecount", quotes },
                { "addquote", quotes },
                { "quoteadd", quotes },
                { "quotefest", quotes },
                { "so", new ShoutOutCommand() },              
                { "title", new TitleCommand() },
                { "totalbits", totalBits },
                { "totalcheers",totalBits },
                { "topbits", topBits },
                { "topcheers", topBits },
                { "streak", streak },
                { "watchstreak", streak }             
            };
            this.commandConfigs = new Dictionary<string, CommandConfiguration>
            {
                { "announcement", new CommandConfiguration("Announcement", settings.Commands.Announcement.Enabled, settings.Commands.Announcement.ModOnly) },
                { "boop", new CommandConfiguration("Boop", settings.Commands.Boop.Enabled, settings.Commands.Boop.ModOnly) },
                { "category", new CommandConfiguration("Category", settings.Commands.Category.Enabled, settings.Commands.Category.ModOnly) },
                { "clip", new CommandConfiguration("Clip", settings.Commands.Clips.Enabled, settings.Commands.Clips.ModOnly) },
                { "exit", new CommandConfiguration("Exit", settings.Commands.Exit.Enabled, settings.Commands.Exit.ModOnly) },
                { "followage", new CommandConfiguration("Followage", settings.Commands.Followage.Enabled, settings.Commands.Followage.ModOnly) },
                { "process", new CommandConfiguration("Process", settings.Commands.Process.Enabled, settings.Commands.Process.ModOnly) },
                { "quote", new CommandConfiguration("Quote", settings.Commands.Quotes.Enabled, settings.Commands.Quotes.ModOnly) },
                { "quotecount", new CommandConfiguration("QuoteCount", settings.Commands.Quotes.Enabled, settings.Commands.Quotes.ModOnly) },
                { "addquote", new CommandConfiguration("AddQuote", settings.Commands.Quotes.Enabled, settings.Commands.Quotes.ModOnly) },
                { "quoteadd", new CommandConfiguration("AddQuote", settings.Commands.Quotes.Enabled, settings.Commands.Quotes.ModOnly) },
                { "quotefest", new CommandConfiguration("QuoteFest", settings.Commands.Quotes.Enabled, settings.Commands.Quotes.ModOnly) },
                { "so", new CommandConfiguration("Shout Out", settings.Commands.ShoutOut.Enabled, settings.Commands.ShoutOut.ModOnly) },
                { "title", new CommandConfiguration("Title", settings.Commands.Title.Enabled, settings.Commands.Title.ModOnly) },
                { "totalbits", new CommandConfiguration("Total Bits", settings.Commands.TotalBits.Enabled, settings.Commands.TotalBits.ModOnly) },
                { "totalcheers", new CommandConfiguration("Total Bits", settings.Commands.TotalBits.Enabled, settings.Commands.TotalBits.ModOnly) },
                { "topbits", new CommandConfiguration("Top Bits", settings.Commands.TotalBits.Enabled, settings.Commands.TopBits.ModOnly) },
                { "topcheers", new CommandConfiguration("Top Bits", settings.Commands.TotalBits.Enabled, settings.Commands.TopBits.ModOnly) },
                { "streak", new CommandConfiguration("Watch Streaks", settings.Commands.WatchStreak.Enabled, settings.Commands.WatchStreak.ModOnly) },
                { "watchstreak", new CommandConfiguration("Watch Streaks", settings.Commands.WatchStreak.Enabled, settings.Commands.WatchStreak.ModOnly) },
            };
        }

        // Checks if a command is a default command, if not it is a custom command
        public bool CheckIfCommandIsLakeaCommand(string command)
        {
            if (commandFunctions.ContainsKey(command.ToLower())) return true;
            else return false;
        }

        // Called when a Lakea command is received, checks if the command is enabled and modonly before call relevant function from commandFunctions dictionary
        public EventItem NewLakeaCommand(IncomingEvent eve)
        {
            try
            {
                string commandIdentifier = eve.Args["CommandIdentifier"];
                string commandLower = eve.Args["CommandText"].ToLower();
                string command = eve.Args["CommandText"];
                string displayName = eve.Args["DisplayName"];
                bool isBroadcaster = bool.Parse(eve.Args["IsBroadcaster"]);
                bool isModerator = bool.Parse(eve.Args["IsModerator"]);
                if (commandConfigs[commandLower].IsEnabled)
                {
                    if (commandConfigs[commandLower].ModOnly)
                    {
                        if (isModerator || isBroadcaster)
                        {
                            return commandFunctions[commandLower].Run(eve);
                        }
                        else
                        {
                            Terminal.Output($"Lakea: {command} Command -> Access Denied, {displayName}");
                            Logs.Instance.NewLog(LogLevel.Warning, $"{command} Command -> Access Denied, {displayName}");
                            Dictionary<string, string> args = new Dictionary<string, string>
                            {
                                { "Message", $"Sorry {displayName}, only moderators can use that command!" }
                            };
                            return new EventItem(eve.Source, EventType.Lakea_Command, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message, $"{command} Command", "Lakea_Command_Access_Denied", args: args);
                        }
                    }
                    else
                    {
                        return commandFunctions[commandLower].Run(eve);
                    }
                }
                else
                {
                    Terminal.Output($"Lakea: Default Command {commandIdentifier}{command} is Disabled");
                    Logs.Instance.NewLog(LogLevel.Info, $"Default Command {commandIdentifier}{command} is Disabled");
                    return new EventItem(eve.Source, EventType.Lakea_Command, EventTarget.Null, EventGoal.Null, commandIdentifier + command);
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Default Command Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }
    }
}
