using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;

namespace Lakea_Stream_Assistant.EventProcessing.Commands
{
    public class ShoutOutCommand : CommandBase
    {
        public override EventItem Run(IncomingEvent eve)
        {
            string argumentsAsString = eve.Args["ArgumentsAsString"];
            Dictionary<string, string> args = new Dictionary<string, string>();
            Terminal.Output($"Lakea: Shout Out Command -> {argumentsAsString}");
            if (eve.Args.ContainsKey("CommandArg1"))
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
    }
}
