using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;
using TwitchLib.Api.Helix.Models.Channels.GetChannelInformation;
using TwitchLib.Api.Helix.Models.Channels.ModifyChannelInformation;

namespace Lakea_Stream_Assistant.EventProcessing.Commands
{
    public class TitleCommand : CommandBase
    {
        public override EventItem Run(IncomingEvent eve)
        {
            string argumentsAsString = eve.Args["ArgumentsAsString"];
            Terminal.Output("Lakea: Update Stream Title Command -> Updating Stream Title");
            Logs.Instance.NewLog(LogLevel.Info, "Update Stream Title Command -> Updating Stream Title");
            Dictionary<string, string> args = new Dictionary<string, string>();
            if (eve.Args.ContainsKey("CommandArg1"))
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
    }
}
