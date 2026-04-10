using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;

namespace Lakea_Stream_Assistant.EventProcessing.Commands
{
    public abstract class CommandBase
    {
        public abstract EventItem Run(IncomingEvent eve);
    }
}
