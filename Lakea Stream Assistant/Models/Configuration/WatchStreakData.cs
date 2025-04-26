
// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class WatchStreakData
{

    private string lastStreamDateField;

    private WatchStreakDataStreak[] streaksField;

    /// <remarks/>
    public string LastStreamDate
    {
        get
        {
            return this.lastStreamDateField;
        }
        set
        {
            this.lastStreamDateField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("Streak", IsNullable = false)]
    public WatchStreakDataStreak[] Streaks
    {
        get
        {
            return this.streaksField;
        }
        set
        {
            this.streaksField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class WatchStreakDataStreak
{

    private string twitchIDField;

    private int currentStreakField;

    private string lastSeenStringField;

    private DateOnly lastSeenField;

    /// <remarks/>
    public string TwitchID
    {
        get
        {
            return this.twitchIDField;
        }
        set
        {
            this.twitchIDField = value;
        }
    }

    /// <remarks/>
    public int CurrentStreak
    {
        get
        {
            return this.currentStreakField;
        }
        set
        {
            this.currentStreakField = value;
        }
    }

    /// <remarks/>
    public string LastSeenString
    {
        get
        {
            return this.lastSeenStringField;
        }
        set
        {
            this.lastSeenStringField = value;
        }
    }

    /// <remarks/>
    public DateOnly LastSeen
    {
        get
        {
            return this.lastSeenField;
        }
        set
        {
            this.lastSeenField = value;
        }
    }
}

