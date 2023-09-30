using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.EventProcessing.Commands;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventAbstracts;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;

namespace Lakea_Stream_Assistant.EventProcessing.Processing
{
    //Receives new events and calls the relevant functions for the event type
    public class EventInput
    {
        private LakeaFunctions lakea;
        private OBSFunctions obs;
        private TwitchFunctions twitch;
        private EventOutputs outputs;
        private EventProcesser processer;
        private EventPassArguments passArgs;

        public EventInput(ConfigEvent[] events, DefaultCommands commands)
        {
            this.outputs = new EventOutputs(this);
            this.passArgs = new EventPassArguments();
            this.processer = new EventProcesser(outputs);
            this.twitch = new TwitchFunctions(events, processer, passArgs);
            this.obs = new OBSFunctions(events, processer, passArgs);
            this.lakea = new LakeaFunctions(events, processer, passArgs, commands, this);
        }

        //Called on a new event, checks event type before calling relevent function
        public void NewEvent(Event eve)
        {
            try
            {
                EventStats.NewEvent(eve);
                switch (eve.Type)
                {
                    case EventType.Battle_Simulator_Encounter:
                    case EventType.Battle_Simulator_Nonencounter:
                        lakea.NewSupportingApplicationEvent((EventItem)eve);
                        break;
                    case EventType.Lakea_Timer_Start:
                        lakea.NewTimerStart();
                        break;
                    case EventType.Lakea_Callback:
                        lakea.NewCallback((LakeaCallback)eve);
                        break;
                    case EventType.Lakea_Command:
                        lakea.NewCommand((LakeaCommand)eve);
                        break;
                    case EventType.Lakea_Timer_Fired:
                        lakea.NewTimer((LakeaTimer)eve);
                        break;
                    case EventType.OBS_Scene_Changed:
                        obs.NewChangedScene((OBSSceneChange)eve);
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
                         twitch.newSubscription((TwitchPubSubSubscription)eve);
                        break;
                    case EventType.Twitch_Redeem:
                        twitch.NewRedeem((TwitchRedeem)eve);
                        break;
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: New Event Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }
    }
}
