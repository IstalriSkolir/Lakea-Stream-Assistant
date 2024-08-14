namespace Lakea_Stream_Assistant.Enums
{
    public enum LogLevel : byte
    {
        Info,
        Warning,
        Error,
        Fatal
    }

    public enum TwitchSubTier : byte
    {
        None,
        Tier_1,
        Tier_2,
        Tier_3
    }

    public enum ScamActionMode : byte
    {
        Nothing,
        SendChatMessage,
        DeleteMessage,
        BanUser
    }
}
