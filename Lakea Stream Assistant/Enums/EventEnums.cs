namespace Lakea_Stream_Assistant.Enums
{
    public enum EventSource
    {
        Base_Camp,
        Lakea,
        Twitch
    }

    public enum TwitchEventType
    {
        Bits,
        Follow,
        Redeem
    }

    public enum LakeaEventType
    {
        Callback
    }

    public enum EventTarget
    {
        Base_Camp,
        Twitch,
        OBS
    }

    public enum EventGoal
    {
        Enable_OBS_Source,
        Disable_OBS_Source,
        Enable_Random_OBS_Source,
        Disable_Random_OBS_Source,
        Change_OBS_Scene,
        Send_Twitch_Chat_Message
    }
}
