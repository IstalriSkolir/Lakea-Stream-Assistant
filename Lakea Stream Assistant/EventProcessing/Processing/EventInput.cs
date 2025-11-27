using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.EventProcessing.Commands;
using Lakea_Stream_Assistant.EventProcessing.Misc;
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
        private EventPassArguments passArgs;
        private LakeaCaptured captured;

        public EventInput(Config config, DefaultCommands commands)
        {
            passArgs = new EventPassArguments();
            lakea = new LakeaFunctions(config.Events, passArgs, commands, this);
            obs = new OBSFunctions(config.Events, passArgs);
            twitch = new TwitchFunctions(config.Events, passArgs, commands);
            captured = new LakeaCaptured(lakea, config.Settings);
            outputs = new EventOutputs(config.Settings, captured);
        }

        #region Update Events

        // Update event dictionaries in the different platform objects
        public void UpdateEventDictionaries(string key, EventItem item, bool remove)
        {
            switch (item.Source)
            {
                case EventSource.Lakea:
                    lakea.UpdateDictionary(key, item, remove);
                    break;
                case EventSource.OBS:
                    obs.UpdateDictionary(key, item, remove);
                    break;
                case EventSource.Twitch:
                    twitch.UpdateDictionary(key, item, remove);
                    break;
                default:
                    Terminal.Output("Lakea: Unsupported Source Dictionary Update -> " + item.Source);
                    Logs.Instance.NewLog(LogLevel.Warning, "Unsupported Source Dictionary Update -> " + item.Source);
                    break;
            }
        }

        #endregion

        // Called on a new event, checks event type before calling relevent function
        public void NewEvent(Event eve)
        {
            try
            {
                EventStats.NewEvent(eve);
                EventItem item = new EventItem();
                switch (eve.Type)
                {
                    case EventType.Battle_Simulator_Encounter:
                    case EventType.Battle_Simulator_Nonencounter:
                        item = lakea.NewSupportingApplicationEvent((IncomingEvent)eve);
                        break;
                    case EventType.Lakea_Timer_Start:
                        lakea.NewTimerStart();
                        item = null;
                        break;
                    case EventType.Lakea_Callback:
                        item = lakea.NewCallback((IncomingEvent)eve);
                        break;
                    case EventType.Lakea_Command:
                        item = lakea.NewCommand((IncomingEvent)eve);
                        break;
                    case EventType.Lakea_Exit:
                        item = lakea.NewExit((IncomingEvent)eve);
                        break;
                    case EventType.Lakea_Struggle:
                        item = lakea.LakeaStruggle((IncomingEvent)eve);
                        break;
                    case EventType.Lakea_Released:
                        item = lakea.LakeaReleased((IncomingEvent)eve);
                        break;
                    case EventType.Lakea_Start_Up:
                        item = lakea.NewStartup((IncomingEvent)eve);
                        break;
                    case EventType.Lakea_Timer_Fired:
                        item = lakea.NewTimer((IncomingEvent)eve);
                        break;
                    case EventType.Lakea_Web_Socket:
                        item = lakea.NewWebSocketEvent((EventItem)eve);
                        break;
                    case EventType.OBS_Scene_Changed:
                        item = obs.NewChangedScene((IncomingEvent)eve);
                        break;
                    case EventType.OBS_Source_Active_Status:
                        item = obs.NewSourceActiveStatus((IncomingEvent)eve);
                        break;
                    case EventType.Twitch_Bits:
                        item = twitch.NewBits((IncomingEvent)eve);
                        break;
                    case EventType.Twitch_Command:
                        item = twitch.NewCommand((IncomingEvent)eve);
                        break;
                    case EventType.Twitch_Follow:
                        item = twitch.NewFollow((IncomingEvent)eve);
                        break;
                    case EventType.Twitch_Raid:
                        item = twitch.NewRaid((IncomingEvent)eve);
                        break;
                    case EventType.Twitch_Watch_Streak:
                        item = twitch.NewWatchStreak((IncomingEvent)eve);
                        break;
                    case EventType.Twitch_Subscription:
                        item = twitch.NewSubscription((IncomingEvent)eve);
                        break;
                    case EventType.Twitch_Resubscription:
                        item = twitch.NewResubscription((IncomingEvent)eve);
                        break;
                    case EventType.Twitch_Prime_Paid_Subscription:
                        item = twitch.NewPrimePaidSubscription((IncomingEvent)eve);
                        break;
                    case EventType.Twitch_Gifted_Subscription:
                        item = twitch.NewGiftedSubscription((IncomingEvent)eve);
                        break;
                    case EventType.Twitch_Continued_Gifted_Subscription:
                        item = twitch.NewGiftedSubscriptionContinued((IncomingEvent)eve);
                        break;
                    case EventType.Twitch_Redeem:
                        item = twitch.NewRedeem((IncomingEvent)eve);
                        break;
                    default:
                        Terminal.Output("EventHandler: Unrecognised Event Type -> " + eve.Type);
                        Logs.Instance.NewLog(LogLevel.Warning, "EventHandler Unrecognised Event Type");
                        break;
                }
                if(item != null)
                {
                    item = captured.CheckIfCaptured(item);
                    processEvent(item);
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: New Event Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        // Receives 'EventItem' object and calls the corresponding 'EventOutputs' function with the relevant arguments
        private void processEvent(EventItem item)//, EventType type)
        {
            try
            {
                if (item != null)
                {
                    switch (item.EventGoal)
                    {
                        case EventGoal.Null:
                            outputs.NullEvent("Null Event -> " + item.Name);
                            break;
                        case EventGoal.Battle_Simulator_Character_Sheet:
                            outputs.GetCharacterSheet(item.Args, item.Callback);
                            break;
                        case EventGoal.Battle_Simulator_Character_Statistics:
                            outputs.GetCharacterStatistics(item.Args, item.Callback);
                            break;
                        case EventGoal.Battle_Simulator_Encounter:
                            outputs.Battle(item.Args, item.Callback);
                            break;
                        case EventGoal.Battle_Simulator_Nonencounter:
                            outputs.OtherBattleSimEvent(item.Args, item.Callback);
                            break;
                        case EventGoal.Lakea_Caught:
                            outputs.CaptureLakea(item.Args, item.Callback);
                            break;
                        case EventGoal.OBS_Disable_Source:
                            outputs.SetActiveOBSSource(item.Args, item.Duration, false, item.Callback);
                            break;
                        case EventGoal.OBS_Enable_Source:
                            outputs.SetActiveOBSSource(item.Args, item.Duration, true, item.Callback);
                            break;
                        case EventGoal.OBS_Enable_Random_Source:
                            outputs.SetRandomActiveOBSSource(item.Args, item.Duration, true, item.Callback);
                            break;
                        case EventGoal.OBS_Disable_Random_Source:
                            outputs.SetRandomActiveOBSSource(item.Args, item.Duration, false, item.Callback);
                            break;
                        case EventGoal.OBS_Loop_Sources:
                            outputs.LoopOBSSources(item.Args, item.Callback);
                            break;
                        case EventGoal.OBS_Change_Scene:
                            outputs.ChangeOBSScene(item.Args, item.Callback);
                            break;
                        case EventGoal.OBS_Change_Volume:
                            outputs.ChangeOBSSourceVolume(item.Args, item.Callback);
                            break;
                        case EventGoal.Python_Run_Script:
                            outputs.RunPythonScript(item.Args, item.Callback);
                            break;
                        case EventGoal.Twitch_Send_Chat_Message:
                            outputs.SendTwitchChatMessage(item.Args, item.Callback);
                            break;
                        case EventGoal.Twitch_Send_Chat_Message_List:
                            outputs.SendTwitchChatMessageList(item.Args, item.Callback);
                            break;
                        case EventGoal.Twitch_Send_Random_Chat_Message:
                            outputs.SendTwitchRandomChatMessage(item.Args, item.Callback);
                            break;
                        case EventGoal.Twitch_Send_Whisper_Message:
                            outputs.SendTwitchWhisperMessage(item.Args, item.Callback);
                            break;
                        case EventGoal.Twitch_Send_Announcement:
                            outputs.SendTwitchAnnouncement(item.Args, item.Callback);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Process Event Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }
    }
}