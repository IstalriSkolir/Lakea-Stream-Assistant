using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.EventProcessing.Commands;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventAbstracts;

namespace Lakea_Stream_Assistant.EventProcessing.Processing
{
    //Receives new events and calls the relevant functions for the event type
    public class EventInput
    {
        private LakeaFunctions lakea;
        private TwitchFunctions twitch;
        private EventOutputs outputs;
        private EventProcesser processer;
        private EventPassArguments passArgs;

        public EventInput(ConfigEvent[] events, InternalCommands commands)
        {
            outputs = new EventOutputs(this);
            passArgs = new EventPassArguments();
            processer = new EventProcesser(outputs);
            twitch = new TwitchFunctions(events, processer, passArgs);
            lakea = new LakeaFunctions(events, processer, passArgs, commands);
        }

        //Called on a new event, checks event type before callin relevent function
        public void NewEvent(Event eve)
        {
            switch (eve.Type)
            {
                case EventType.Lakea_Timer:
                    lakea.NewTimerStart();
                    break;
                case EventType.Lakea_Callback:
                    lakea.NewCallback((LakeaCallback)eve);
                    break;
                case EventType.Lakea_Command:
                    lakea.NewCommand((LakeaCommand)eve);
                    break;
                case EventType.Twitch_Bits:
                    twitch.NewBits((TwitchBits)eve);
                    break;
                case EventType.Twitch_Command:
                    twitch.NewCommand((TwitchCommand)eve);
                    break;
                case EventType.Twitch_Follow:
                    twitch.NewFollow((TwitchFollow)eve);
                    break;
                case EventType.Twitch_Raid:
                    twitch.NewRaid((TwitchRaid)eve);
                    break;
                case EventType.Twitch_Subscription:
                    twitch.newSubscription((TwitchSubscription)eve);
                    break;
                case EventType.Twitch_Redeem:
                    twitch.NewRedeem((TwitchRedeem)eve);
                    break;
            }
        }
    }
}
