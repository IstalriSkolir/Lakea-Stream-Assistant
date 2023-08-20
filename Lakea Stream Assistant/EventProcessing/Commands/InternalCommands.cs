using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Singletons;

namespace Lakea_Stream_Assistant.EventProcessing.Commands
{
    public class InternalCommands
    {
        private readonly Dictionary<string, Func<LakeaCommand, EventItem>> commands;
        private readonly Dictionary<string, bool> active;

        public InternalCommands(SettingsCommands commands)
        {
            this.commands = new Dictionary<string, Func<LakeaCommand, EventItem>>
            {
                { "status", statusCommands }
            };
            this.active = new Dictionary<string, bool>
            {
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

        private EventItem statusCommands(LakeaCommand eve)
        {
            Dictionary<string, string> args = new Dictionary<string, string>
            {
                { "Message", "I'm still active, all is well Cuppa" }
            };
            return new EventItem(eve.Source, EventType.Lakea_Command, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message, "Status Command", "Lakea_Status_Command", args: args);
        }
    }
}
