using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events.EventAbstracts;
using Lakea_Stream_Assistant.Models.OutputFunctions;
using Lakea_Stream_Assistant.Models.Twitch;

namespace Lakea_Stream_Assistant.Models.Events
{
    //Receives new events and calls the relevant functions for the event type
    public class HandleEvents
    {
        private LakeaFunctions lakea;
        private TwitchFunctions twitch;
        private EventOutputs outputs;

        public HandleEvents(ConfigEvent[] events)
        {
            outputs = new EventOutputs(this);
            twitch = new TwitchFunctions(events, outputs);
            lakea = new LakeaFunctions(events, outputs);
        }

        //Called on a new event, sorts by event source
        public void NewEvent(Event eve)
        {
            switch (eve.Source)
            {
                case EventSource.Base_Camp:
                    baseCampEvent(eve);
                    break;
                case EventSource.Lakea:
                    LakeaEvent((LakeaEvent)eve);
                    break;
                case EventSource.Twitch:
                    twitchEvent((TwitchEvent)eve);
                    break;
            }
        }

        //Called on a Base Camp event, sorts by event type
        private void baseCampEvent(Event eve)
        {

        }

        //Called on a Lakea event, sorts by event type
        private void LakeaEvent(LakeaEvent eve)
        {
            switch (eve.EventType)
            {
                case LakeaEventType.Callback:
                    lakea.NewCallback((LakeaCallback)eve);
                    break;
            }
        }

        //Called on a Twitch event, sorts by event type
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
