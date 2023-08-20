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
        Lakea_Callback,
        Lakea_Command,
        Lakea_Timer,
        Twitch_Bits,
        Twitch_Command,
        Twitch_Follow,
        Twitch_Raid,
        Twitch_Redeem,
        Twitch_Subscription
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
        Twitch_Send_Chat_Message,
        Twitch_Send_Whisper_Message
    }
}
