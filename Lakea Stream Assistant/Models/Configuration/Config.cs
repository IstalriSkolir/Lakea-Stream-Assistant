/*

Code below has been generated from Config.xml, it should not be manually editted. Any changes is data structure should be done by editting the
xml file and regenerating this structure from the editted xml. To regenerate structure, select all the code below and delete it. Then select all
in the xml file before returning here and selecting Edit -> Paste Special -> Paste XML As Classes

*/
// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class Config
{

    private ConfigOBS oBSField;

    private ConfigTwitch twitchField;

    private ConfigEvent[] eventsField;

    /// <remarks/>
    public ConfigOBS OBS
    {
        get
        {
            return this.oBSField;
        }
        set
        {
            this.oBSField = value;
        }
    }

    /// <remarks/>
    public ConfigTwitch Twitch
    {
        get
        {
            return this.twitchField;
        }
        set
        {
            this.twitchField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("Event", IsNullable = false)]
    public ConfigEvent[] Events
    {
        get
        {
            return this.eventsField;
        }
        set
        {
            this.eventsField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class ConfigOBS
{

    private string ipField;

    private ushort portField;

    private string passwordField;

    /// <remarks/>
    public string IP
    {
        get
        {
            return this.ipField;
        }
        set
        {
            this.ipField = value;
        }
    }

    /// <remarks/>
    public ushort Port
    {
        get
        {
            return this.portField;
        }
        set
        {
            this.portField = value;
        }
    }

    /// <remarks/>
    public string Password
    {
        get
        {
            return this.passwordField;
        }
        set
        {
            this.passwordField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class ConfigTwitch
{

    private ConfigTwitchStreamingChannel streamingChannelField;

    private object botChannelField;

    /// <remarks/>
    public ConfigTwitchStreamingChannel StreamingChannel
    {
        get
        {
            return this.streamingChannelField;
        }
        set
        {
            this.streamingChannelField = value;
        }
    }

    /// <remarks/>
    public object BotChannel
    {
        get
        {
            return this.botChannelField;
        }
        set
        {
            this.botChannelField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class ConfigTwitchStreamingChannel
{

    private string usernameField;

    private uint idField;

    private string authKeyField;

    /// <remarks/>
    public string Username
    {
        get
        {
            return this.usernameField;
        }
        set
        {
            this.usernameField = value;
        }
    }

    /// <remarks/>
    public uint ID
    {
        get
        {
            return this.idField;
        }
        set
        {
            this.idField = value;
        }
    }

    /// <remarks/>
    public string AuthKey
    {
        get
        {
            return this.authKeyField;
        }
        set
        {
            this.authKeyField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class ConfigEvent
{

    private ConfigEventEventDetails eventDetailsField;

    private ConfigEventEventTarget eventTargetField;

    /// <remarks/>
    public ConfigEventEventDetails EventDetails
    {
        get
        {
            return this.eventDetailsField;
        }
        set
        {
            this.eventDetailsField = value;
        }
    }

    /// <remarks/>
    public ConfigEventEventTarget EventTarget
    {
        get
        {
            return this.eventTargetField;
        }
        set
        {
            this.eventTargetField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class ConfigEventEventDetails
{

    private string nameField;

    private string sourceField;

    private string typeField;

    private string idField;

    /// <remarks/>
    public string Name
    {
        get
        {
            return this.nameField;
        }
        set
        {
            this.nameField = value;
        }
    }

    /// <remarks/>
    public string Source
    {
        get
        {
            return this.sourceField;
        }
        set
        {
            this.sourceField = value;
        }
    }

    /// <remarks/>
    public string Type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }

    /// <remarks/>
    public string ID
    {
        get
        {
            return this.idField;
        }
        set
        {
            this.idField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class ConfigEventEventTarget
{

    private string targetField;

    private string goalField;

    private string objectField;

    private ushort durationField;

    private bool durationFieldSpecified;

    /// <remarks/>
    public string Target
    {
        get
        {
            return this.targetField;
        }
        set
        {
            this.targetField = value;
        }
    }

    /// <remarks/>
    public string Goal
    {
        get
        {
            return this.goalField;
        }
        set
        {
            this.goalField = value;
        }
    }

    /// <remarks/>
    public string Object
    {
        get
        {
            return this.objectField;
        }
        set
        {
            this.objectField = value;
        }
    }

    /// <remarks/>
    public ushort Duration
    {
        get
        {
            return this.durationField;
        }
        set
        {
            this.durationField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool DurationSpecified
    {
        get
        {
            return this.durationFieldSpecified;
        }
        set
        {
            this.durationFieldSpecified = value;
        }
    }
}

