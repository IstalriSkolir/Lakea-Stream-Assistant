namespace Lakea_Stream_Assistant.Enums
{
    public enum EventSource : byte
    {
        Base_Camp,
        Battle_Simulator,
        OBS,
        Lakea,
        Twitch
    }

    public enum EventType : ushort
    {
        Battle_Simulator_Encounter,
        Battle_Simulator_Nonencounter,
        Lakea_Callback,
        Lakea_Command,
        Lakea_Timer_Start,
        Lakea_Timer_Fired,
        OBS_Scene_Changed,
        Twitch_Bits,
        Twitch_Command,
        Twitch_Follow,
        Twitch_Raid,
        Twitch_Redeem,
        Twitch_Subscription
    }

    public enum EventTarget : byte
    {
        Null,
        Base_Camp,
        Battle_Simulator,
        Twitch,
        OBS
    }

    public enum EventGoal : ushort
    {
        Null,
        Battle_Simulator_Character_Sheet,
        Battle_Simulator_Nonencounter,
        Battle_Simulator_Encounter,
        OBS_Enable_Source,
        OBS_Disable_Source,
        OBS_Enable_Random_Source,
        OBS_Disable_Random_Source,
        OBS_Loop_Sources,
        OBS_Change_Scene,
        Twitch_Send_Chat_Message,
        Twitch_Send_Chat_Message_List,
        Twitch_Send_Whisper_Message
    }
}
