namespace Lakea_Stream_Assistant.Enums
{
    public enum EventSource
    {
        Base_Camp,
        Twitch
    }

    public enum TwitchEventType
    {
        Bits,
        Follow,
        Redeem
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
        Disable_OBS_Source
    }
}
