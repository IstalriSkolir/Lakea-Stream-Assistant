using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;
using TwitchLib.Api.Helix.Models.Channels.GetChannelInformation;
using TwitchLib.Api.Helix.Models.Channels.ModifyChannelInformation;
using TwitchLib.Api.Helix.Models.Games;

namespace Lakea_Stream_Assistant.EventProcessing.Commands
{
    public class CategoryCommand : CommandBase
    {
        public override EventItem Run(IncomingEvent eve)
        {
            string argumentsAsString = eve.Args["ArgumentsAsString"];
            Terminal.Output("Lakea: Update Stream Category Command -> Updating Stream Category");
            Logs.Instance.NewLog(LogLevel.Info, "Update Stream Category Command -> Updating Stream Category");
            Dictionary<string, string> args = new Dictionary<string, string>();
            if (eve.Args.ContainsKey("CommandArg1"))
            {
                GetGamesResponse response = Twitch.GetCategoryInformation(new List<string>() { argumentsAsString }).Result;
                if (response.Data.Count() > 0)
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
    }
}
