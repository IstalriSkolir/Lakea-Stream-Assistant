using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventAbstracts;
using Lakea_Stream_Assistant.Models.Twitch;

namespace Lakea_Stream_Assistant.Models.OutputFunctions
{
    //Receives new events and calls the relevant functions for the event type
    public class EventInput
    {
        private LakeaFunctions lakea;
        private TwitchFunctions twitch;
        private EventOutputs outputs;
        private EventProcesser processer;

        public EventInput(ConfigEvent[] events)
        {
            outputs = new EventOutputs(this);
            processer = new EventProcesser(outputs);
            twitch = new TwitchFunctions(events, processer);
            lakea = new LakeaFunctions(events, processer);
        }

        //Called on a new event, checks event type before callin relevent function
        public void NewEvent(Event eve)
        {
            switch (eve.Type)
            {
                case EventType.Lakea_Callback:
                    lakea.NewCallback((LakeaCallback)eve);
                    break;
                case EventType.Twitch_Bits:
                    twitch.NewBits((TwitchBits)eve);
                    break;
                case EventType.Twitch_Follow:
                    twitch.NewFollow((TwitchFollow)eve);
                    break;
                case EventType.Twitch_Redeem:
                    twitch.NewRedeem((TwitchRedeem)eve);
                    break;
            }
        }
    }
}
