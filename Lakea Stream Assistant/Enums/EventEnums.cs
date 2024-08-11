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
        Lakea_Exit,
        Lakea_Released,
        Lakea_Retort,
        Lakea_Start_Up,
        Lakea_Timer_Start,
        Lakea_Timer_Fired,
        Lakea_Web_Socket,
        OBS_Scene_Changed,
        OBS_Source_Active_Status,
        Twitch_Bits,
        Twitch_Command,
        Twitch_Follow,
        Twitch_Raid,
        Twitch_Redeem,
        Twitch_Subscription,
        Twitch_Resubscription,
        Twitch_Prime_Paid_Subscription,
        Twitch_Gifted_Subscription,
        Twitch_Continued_Gifted_Subscription
    }

    public enum EventTarget : byte
    {
        Null,
        Base_Camp,
        Battle_Simulator,
        Lakea,
        Python,
        Twitch,
        OBS
    }

    public enum EventGoal : ushort
    {
        Null,
        Battle_Simulator_Character_Sheet,
        Battle_Simulator_Character_Statistics,
        Battle_Simulator_Nonencounter,
        Battle_Simulator_Encounter,
        Lakea_Caught,
        Lakea_Released,
        OBS_Enable_Source,
        OBS_Disable_Source,
        OBS_Enable_Random_Source,
        OBS_Disable_Random_Source,
        OBS_Loop_Sources,
        OBS_Change_Scene,
        Python_Run_Script,
        Twitch_Send_Chat_Message,
        Twitch_Send_Chat_Message_List,
        Twitch_Send_Random_Chat_Message,
        Twitch_Send_Whisper_Message
    }
}
