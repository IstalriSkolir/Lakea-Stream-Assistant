using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;

namespace Lakea_Stream_Assistant.EventProcessing.Commands
{
    public class ClipCommand : CommandBase
    {
        public override EventItem Run(IncomingEvent eve)
        {
            string displayName = eve.Args["DisplayName"];
            Terminal.Output($"Lakea: Clip Command -> {displayName}");
            Twitch.CreateStreamClip();



            return new EventItem();
        }
    }
}
