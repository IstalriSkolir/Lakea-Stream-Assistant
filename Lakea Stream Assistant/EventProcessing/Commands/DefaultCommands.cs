using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.EventProcessing.Misc;
using Lakea_Stream_Assistant.Models.Configuration;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Models.Tokens;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;
using TwitchLib.Api.Helix.Models.Bits;
using TwitchLib.Api.Helix.Models.Channels.GetChannelInformation;
using TwitchLib.Api.Helix.Models.Channels.ModifyChannelInformation;
using TwitchLib.Api.Helix.Models.Games;

namespace Lakea_Stream_Assistant.EventProcessing.Commands
{
    // This class handles default commands for Lakea
    public class DefaultCommands
    {
        private ProcessCommand process;
        private QuoteCommand quotes;
        private readonly Dictionary<string, Func<IncomingEvent, EventItem>> commandFunctions;
        private readonly Dictionary<string, CommandConfiguration> commandConfigs;
        KeepAliveToken keepAliveToken;

        // Constructor takes object references, sets predefined dictionaries and command active/modonly status
        public DefaultCommands(ConfigSettings settings, ExternalProcesses externalProcesses, KeepAliveToken keepAliveToken)
        {
            this.process = new ProcessCommand(externalProcesses);
            this.quotes = new QuoteCommand(settings.ResourcePath, settings.Commands.Quotes.QuoteCooldown);
            this.keepAliveToken = keepAliveToken;
            this.commandFunctions = new Dictionary<string, Func<IncomingEvent, EventItem>>
            {
                { "announcement", annoucement },
                { "category", categoryCommand },
                { "clip", clip },
                { "exit", exitCommand },
                { "followage", followage },
                { "process", processCommand },
                { "quote", quoteCommand },
                { "quotecount", quoteCommand },
                { "addquote", quoteCommand },
                { "quoteadd", quoteCommand },
                { "quotefest", quoteCommand },
                { "resetterminal", resetTerminalCommand },
                { "so", shoutOutCommand },
                { "status", statusCommand },
                { "title", titleCommand },
                { "totalbits", totalBitsCommand },
                { "totalcheers", totalBitsCommand },
                { "topbits", topBitsCommand },
                { "topcheers", topBitsCommand },
                { "streak", watchStreakCommand },
                { "watchstreak", watchStreakCommand }             
            };
            this.commandConfigs = new Dictionary<string, CommandConfiguration>
            {
                { "announcement", new CommandConfiguration("Announcement", settings.Commands.Announcement.Enabled, settings.Commands.Announcement.ModOnly) },
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
                { "resetterminal", new CommandConfiguration("Reset Terminal", settings.Commands.ResetTerminal.Enabled, settings.Commands.ResetTerminal.ModOnly) },
                { "so", new CommandConfiguration("Shout Out", settings.Commands.ShoutOut.Enabled, settings.Commands.ShoutOut.ModOnly) },
                { "status", new CommandConfiguration("Status", settings.Commands.Status.Enabled, settings.Commands.Status.ModOnly) },
                { "title", new CommandConfiguration("Title", settings.Commands.Title.Enabled, settings.Commands.Status.ModOnly) },
                { "totalbits", new CommandConfiguration("Total Bits", settings.Commands.TotalBits.Enabled, settings.Commands.Status.ModOnly) },
                { "totalcheers", new CommandConfiguration("Total Bits", settings.Commands.TotalBits.Enabled, settings.Commands.Status.ModOnly) },
                { "topbits", new CommandConfiguration("Top Bits", settings.Commands.TotalBits.Enabled, settings.Commands.Status.ModOnly) },
                { "topcheers", new CommandConfiguration("Top Bits", settings.Commands.TotalBits.Enabled, settings.Commands.Status.ModOnly) },
                { "streak", new CommandConfiguration("Watch Streaks", settings.Commands.WatchStreak.Enabled, settings.Commands.Status.ModOnly) },
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
                            return commandFunctions[commandLower].Invoke(eve);
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
                        return commandFunctions[commandLower].Invoke(eve);
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

        // Calls on the Twitch API to update the stream category
        private EventItem categoryCommand(IncomingEvent eve)
        {
            string argumentsAsString = eve.Args["ArgumentsAsString"];
            Terminal.Output("Lakea: Update Stream Category Command -> Updating Stream Category");
            Logs.Instance.NewLog(LogLevel.Info, "Update Stream Category Command -> Updating Stream Category");
            Dictionary<string, string> args = new Dictionary<string, string>();
            if(eve.Args.ContainsKey("CommandArg1"))
            //if (eve.Args.Command.ArgumentsAsList.Count > 0)
            {
                GetGamesResponse response = Twitch.GetCategoryInformation(new List<string>() { argumentsAsString }).Result;
                if(response.Data.Count() > 0)
                {
                    args.Add("Message", "On it, give me a moment!");
                    ModifyChannelInformationRequest request = new ModifyChannelInformationRequest();
                    request.GameId = response.Data[0].Id;
                    Twitch.UpdateChannelInformation(request);
                    return new EventItem(eve.Source, EventType.Lakea_Command, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message, "Update Stream Category Command", "Lakea_Update_Stream_Category", args: args);
                }
                else
                {
                    args.Add("Message", $"Twitch doesn't seem to have any categories of the name '{argumentsAsString}', are you sure that's the right one?");
                }
            }
            else
            {
                GetChannelInformationResponse response = Twitch.GetChannelInformation().Result;
                args.Add("Message", $"The current stream title is '{response.Data[0].GameName}', don't know why you didn't just look at it though!");
            }
            return new EventItem(eve.Source, EventType.Lakea_Command, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message, "Update Stream Category Command", "Lakea_Update_Stream_Category", args: args);
        }

        // Checks if it was the broadcaster that called the command, then kills the keep alive token so the main thread can end the application
        private EventItem exitCommand(IncomingEvent eve)
        {
            string command = eve.Args["CommandText"];
            string displayName = eve.Args["DisplayName"];
            string channel = eve.Args["Channel"];
            bool isBroadcaster = bool.Parse(eve.Args["IsBroadcaster"]);
            Terminal.Output($"Lakea: Exit Command -> {command}");
            Logs.Instance.NewLog(LogLevel.Info, $"Exit Command -> {command}");
            if (isBroadcaster)
            {
                keepAliveToken.Kill();
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

        // Calls the Processes object to process the command before returning the output in a new EventItem object
        private EventItem processCommand(IncomingEvent eve)
        {
            string argumentsAsString = eve.Args["ArgumentsAsString"];
            Terminal.Output($"Lakea: Process Command -> {argumentsAsString}");
            Logs.Instance.NewLog(LogLevel.Info, $"Process Command -> {argumentsAsString}");
            Dictionary<string, string> args = process.NewProcessCommand(eve);
            return new EventItem(eve.Source, EventType.Lakea_Command, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message, "Process Command", "Lakea_Process_Command", args: args);
        }

        // Calls on the Quotes object to process the quote command before returning the output in a new EventItem object
        private EventItem quoteCommand(IncomingEvent eve)
        {
            string command = eve.Args["CommandText"];
            Terminal.Output($"Lakea: Quote Command -> {command}");
            Logs.Instance.NewLog(LogLevel.Info, $"Quote Command -> {command}");
            Dictionary<string, string> args = quotes.NewQuoteCommand(eve);
            if (args != null)
            {
                if ("quotefest".Equals(command))
                {
                    return new EventItem(eve.Source, EventType.Lakea_Command, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message_List, "Quote Command", "Lakea_Quote_Command", args: args);
                }
                else
                {
                    return new EventItem(eve.Source, EventType.Lakea_Command, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message, "Quote Command", "Lakea_Quote_Command", args: args);
                }
            }
            return null;
        }

        // Resets the terminal with a full refresh on a new thread
        private EventItem resetTerminalCommand(IncomingEvent eve)
        {
            Terminal.Output("Lakea: Reset Terminal Command -> Resetting Terminal");
            Logs.Instance.NewLog(LogLevel.Info, "Reset Terminal Command -> Resetting Terminal");
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "Message", "On it, give me a moment!" }
            };
            Terminal.ResetTerminal();
            return new EventItem(eve.Source, EventType.Lakea_Command, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message, "Reset Terminal Command", "Lakea_Reset_Terminal_Command", args: args);
        }

        // Returns a new EvenItem object that sends a shoutout message for the entered username to the Twitch chat
        private EventItem shoutOutCommand(IncomingEvent eve)
        {
            string argumentsAsString = eve.Args["ArgumentsAsString"];

            Dictionary<string, string> args = new Dictionary<string, string>();
            Terminal.Output($"Lakea: Shout Out Command -> {argumentsAsString}");
            if(eve.Args.ContainsKey("CommandArg1"))
            {
                string commandArg1 = eve.Args["CommandArg1"];
                args.Add("Message", $"Hey guys, go give {commandArg1} some love and support! You can find them at https://www.twitch.tv/{commandArg1}");
            }
            else
            {
                Terminal.Output("Lakea: Shout Out Command -> No User Name given to Shout Out");
                Logs.Instance.NewLog(LogLevel.Warning, "Shout Out Command -> No User Given");
                args.Add("Message", "You didn't tell me who to shout out @{DisplayName}! Who am I shouting out?");
            }
            return new EventItem(eve.Source, EventType.Lakea_Command, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message, "Shout Out Command", "Lakea_Shout_Out_Command", args: args);
        }

        // Returns a new EventItem that sends a message to the Twitchchat to confirm Lakea is still active
        private EventItem statusCommand(IncomingEvent eve)
        {
            Terminal.Output("Lakea: Status Command -> Active");
            Logs.Instance.NewLog(LogLevel.Info, "Status Command -> Active");
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "Message", "I'm still active, all is well Cuppa" }
            };
            return new EventItem(eve.Source, EventType.Lakea_Command, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message, "Status Command", "Lakea_Status_Command", args: args);
        }

        // Calls on the Twitch API to update the stream title
        private EventItem titleCommand(IncomingEvent eve)
        {
            string argumentsAsString = eve.Args["ArgumentsAsString"];
            Terminal.Output("Lakea: Update Stream Title Command -> Updating Stream Title");
            Logs.Instance.NewLog(LogLevel.Info, "Update Stream Title Command -> Updating Stream Title");
            Dictionary<string, string> args = new Dictionary<string, string>();
            if(eve.Args.ContainsKey("CommandArg1"))
            {  
                args.Add("Message", "On it, give me a moment!");
                ModifyChannelInformationRequest request = new ModifyChannelInformationRequest();
                request.Title = argumentsAsString;
                Twitch.UpdateChannelInformation(request);
                return new EventItem(eve.Source, EventType.Lakea_Command, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message, "Update Stream Title Command", "Lakea_Update_Stream_Title", args: args);
            }
            else
            {
                GetChannelInformationResponse response = Twitch.GetChannelInformation().Result;
                args.Add("Message", $"The current stream title is '{response.Data[0].Title}', don't know why you didn't just look at it though!");
            }
            return new EventItem(eve.Source, EventType.Lakea_Command, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message, "Update Stream Title Command", "Lakea_Update_Stream_Title", args: args);
        }

        // Returns a new EventItem that sends a message to Twitch chat with the users total bits cheered
        private EventItem totalBitsCommand(IncomingEvent eve)
        {
            string displayName = eve.Args["DisplayName"];
            Terminal.Output($"Lakea: Total Bits Command -> {displayName}");
            Logs.Instance.NewLog(LogLevel.Info, $"Total Bits Command -> {displayName}");
            Dictionary<string, string> args = new Dictionary<string, string>();
            GetBitsLeaderboardResponse response = Twitch.GetBitsLeaderBoard(1, eve.Args["AccountID"]).Result;
            if (response != null && response.Listings.Length > 0)
            {
                int userBits = response.Listings[0].Score;
                int userRank = response.Listings[0].Rank;
                args.Add("Message", $"{displayName} has cheered a total of {userBits} bits and is rank {userRank} on the leaderboard! Thank you for supporting Materies materi33Lakeaheart");
            }
            else
            {
                args.Add("Message", displayName + " hasn't cheered any bits yet!");
            }
            return new EventItem(eve.Source, EventType.Lakea_Command, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message, "Total Bits Command", "Lakea_Total_Bits_Command", args: args);
        }

        private EventItem topBitsCommand(IncomingEvent eve)
        {
            string displayName = eve.Args["DisplayName"];
            Terminal.Output($"Lakea: Top Bits Command -> {displayName}");
            Logs.Instance.NewLog(LogLevel.Info, $"Top Bits Command -> {displayName}");
            Dictionary<string, string> args = new Dictionary<string, string>();
            GetBitsLeaderboardResponse response = Twitch.GetBitsLeaderBoard(10).Result;
            if (response != null && response.Listings.Length > 0)
            {
                string message = "Top Cheerers! ";
                for(int index = 0; index < response.Listings.Length; index++)
                {
                    message += $"{(index + 1)}. {response.Listings[index].UserName} -> {response.Listings[index].Score} bits, ";
                }
                args.Add("Message", message);
            }
            else
            {
                args.Add("Message", "Error getting top cheerers!");
            }
            return new EventItem(eve.Source, EventType.Lakea_Command, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message, "Top Bits Command", "Lakea_Top_Bits_Command", args: args);
        }

        private EventItem watchStreakCommand(IncomingEvent eve)
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

        private EventItem followage(IncomingEvent eve)
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

        private EventItem annoucement(IncomingEvent eve)
        {
            string displayName = eve.Args["DisplayName"];
            Terminal.Output($"Lakea: Announcement Command -> {displayName}");
            Logs.Instance.NewLog(LogLevel.Info, $"Announcement Command -> {displayName}");
            Twitch.SendChatAnnouncement(eve.Args["ArgumentsAsString"]).RunSynchronously();
            return new EventItem(eve.Source, EventType.Twitch_Command, EventTarget.Null, EventGoal.Null, "Announcement Command", "Twitch_Announcement_Command", args:null);
        }

        private EventItem clip(IncomingEvent eve)
        {
            string displayName = eve.Args["DisplayName"];
            Terminal.Output($"Lakea: Clip Command -> {displayName}");
            Logs.Instance.NewLog(LogLevel.Info, $"Clip Command -> {displayName}");
            Twitch.CreateStreamClip();



            return new EventItem();
        }
    }
}
