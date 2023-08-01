using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Twitch;

namespace Lakea_Stream_Assistant.Models.Events
{
    public class HandleEvents
    {
        private TwitchFunctions twitch;

        public HandleEvents(ConfigEvent[] events)
        {
            twitch = new TwitchFunctions(events);
        }

        public void NewEvent(Event eve)
        {
            switch (eve.Source)
            {
                case EventSource.Base_Camp:
                    baseCampEvent(eve);
                    break;
                case EventSource.Twitch:
                    twitchEvent((TwitchEvent)eve);
                    break;
            }
        }

        private void baseCampEvent(Event eve)
        {

        }

        private void twitchEvent(TwitchEvent eve)
        {
            switch (eve.EventType)
            {
                case TwitchEventType.Bits:
                    break;
                case TwitchEventType.Follow:
                    break;
                case TwitchEventType.Redeem:
                    twitch.NewRedeem((TwitchRedeem)eve);
                    break;
            }
        }
    }
}
