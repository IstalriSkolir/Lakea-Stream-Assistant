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
            captured = new LakeaCaptured(this, config.Settings.Captures);
            lakea = new LakeaFunctions(config.Events, passArgs, commands, this);
            obs = new OBSFunctions(config.Events, passArgs);
            twitch = new TwitchFunctions(config.Events, passArgs);
            outputs = new EventOutputs(this, config.Settings, captured);
        }

        //Called on a new event, checks event type before calling relevent function
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
                        item = lakea.NewSupportingApplicationEvent((EventItem)eve);
                        break;
                    case EventType.Lakea_Timer_Start:
                        lakea.NewTimerStart();
                        item = null;
                        break;
                    case EventType.Lakea_Callback:
                        item = lakea.NewCallback((LakeaCallback)eve);
                        break;
                    case EventType.Lakea_Command:
                        item = lakea.NewCommand((LakeaCommand)eve);
                        break;
                    case EventType.Lakea_Exit:
                        item = lakea.NewExit((EventItem)eve);
                        break;
                    case EventType.Lakea_Released:
                        item = (EventItem)eve;
                        break;
                    case EventType.Lakea_Start_Up:
                        item = lakea.NewStartup((EventItem)eve);
                        break;
                    case EventType.Lakea_Timer_Fired:
                        item = lakea.NewTimer((LakeaTimer)eve);
                        break;
                    case EventType.OBS_Scene_Changed:
                        item = obs.NewChangedScene((OBSSceneChange)eve);
                        break;
                    case EventType.OBS_Source_Active_Status:
                        item = obs.NewSourceActiveStatus((OBSSourceActive)eve);
                        break;
                    case EventType.Twitch_Bits:
                        item = twitch.NewBits((TwitchBits)eve);
                        break;
                    case EventType.Twitch_Command:
                        item = twitch.NewCommand((TwitchCommand)eve);
                        break;
                    case EventType.Twitch_Follow:
                        item = twitch.NewFollow((TwitchFollow)eve);
                        break;
                    case EventType.Twitch_Raid:
                        item = twitch.NewRaid((TwitchRaid)eve);
                        break;
                    case EventType.Twitch_Subscription:
                        item = twitch.newSubscription((TwitchPubSubSubscription)eve);
                        break;
                    case EventType.Twitch_Redeem:
                        item = twitch.NewRedeem((TwitchRedeem)eve);
                        break;
                }
                if(item != null)
                {
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
        private void processEvent(EventItem item)
        {
            try
            {
                if (item != null)
                {
                    if (captured.IsCaught && (item.EventGoal != EventGoal.Null && item.EventGoal != EventGoal.Lakea_Freed))
                    {
                        outputs.LakeaSendRetort();
                        return;
                    }
                    switch (item.EventGoal)
                    {
                        case EventGoal.Null:
                            outputs.NullEvent("Null Event -> " + item.Name);
                            break;
                        case EventGoal.Battle_Simulator_Character_Sheet:
                            outputs.GetCharacterSheet(item.Args, item.Callback);
                            break;
                        case EventGoal.Battle_Simulator_Encounter:
                            outputs.Battle(item.Args, item.Callback);
                            break;
                        case EventGoal.Battle_Simulator_Nonencounter:
                            outputs.OtherBattleSimEvent(item.Args, item.Callback);
                            break;
                        case EventGoal.Lakea_Caught:
                            outputs.CaptureLakea(item.Duration);
                            break;
                        case EventGoal.Lakea_Freed:
                            outputs.LakeaFreed(item.Args, item.Callback);
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