
public class TwitchUser
{
    public int TwitchID { get; set; }
    public string TwitchUsername { get; set; }
    public Channelpoints ChannelPoints { get; set; }
    public Watchstreak WatchStreak { get; set; }
}

public class Channelpoints
{
    public int TotalSpent { get; set; }
    public Redeem[] Redeems { get; set; }
}

public class Redeem
{
    public string RedeemID { get; set; }
    public string RedeemName { get; set; }
    public int RedemptionCount { get; set; }
    public int RedemptionTotalSpent { get; set; }
}

public class Watchstreak
{
}
