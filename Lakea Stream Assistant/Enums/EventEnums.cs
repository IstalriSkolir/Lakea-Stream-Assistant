namespace Lakea_Stream_Assistant.Enums
{
    public enum EventSource
    {
        Base_Camp,
        Lakea,
        Twitch
    }

    public enum EventType
    {
        Twitch_Bits,
        Twitch_Command,
        Twitch_Follow,
        Twitch_Redeem,
        Lakea_Callback,
        Lakea_Timer
    }

    public enum EventTarget
    {
        Null,
        Base_Camp,
        Twitch,
        OBS
    }

    public enum EventGoal
    {
        Null,
        OBS_Enable_Source,
        OBS_Disable_Source,
        OBS_Enable_Random_Source,
        OBS_Disable_Random_Source,
        OBS_Change_Scene,
        Twitch_Send_Chat_Message
    }
}
