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

    private ConfigTwitchBotChannel botChannelField;

    private string commandIdentifierField;

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
    public ConfigTwitchBotChannel BotChannel
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

    /// <remarks/>
    public string CommandIdentifier
    {
        get
        {
            return this.commandIdentifierField;
        }
        set
        {
            this.commandIdentifierField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class ConfigTwitchStreamingChannel
{

    private string userNameField;

    private uint idField;

    private string authKeyField;

    /// <remarks/>
    public string UserName
    {
        get
        {
            return this.userNameField;
        }
        set
        {
            this.userNameField = value;
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
public partial class ConfigTwitchBotChannel
{

    private string userNameField;

    private string userTokenField;

    private string channelConnectionField;

    /// <remarks/>
    public string UserName
    {
        get
        {
            return this.userNameField;
        }
        set
        {
            this.userNameField = value;
        }
    }

    /// <remarks/>
    public string UserToken
    {
        get
        {
            return this.userTokenField;
        }
        set
        {
            this.userTokenField = value;
        }
    }

    /// <remarks/>
    public string ChannelConnection
    {
        get
        {
            return this.channelConnectionField;
        }
        set
        {
            this.channelConnectionField = value;
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

    private ConfigEventEventTargetArg[] argsField;

    private bool usePreviousArgumentsField;

    private bool usePreviousArgumentsFieldSpecified;

    private string callbackField;

    private ushort durationField;

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
    [System.Xml.Serialization.XmlArrayItemAttribute("Arg", IsNullable = false)]
    public ConfigEventEventTargetArg[] Args
    {
        get
        {
            return this.argsField;
        }
        set
        {
            this.argsField = value;
        }
    }

    /// <remarks/>
    public bool UsePreviousArguments
    {
        get
        {
            return this.usePreviousArgumentsField;
        }
        set
        {
            this.usePreviousArgumentsField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool UsePreviousArgumentsSpecified
    {
        get
        {
            return this.usePreviousArgumentsFieldSpecified;
        }
        set
        {
            this.usePreviousArgumentsFieldSpecified = value;
        }
    }

    /// <remarks/>
    public string Callback
    {
        get
        {
            return this.callbackField;
        }
        set
        {
            this.callbackField = value;
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
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class ConfigEventEventTargetArg
{

    private string keyField;

    private string valueField;

    /// <remarks/>
    public string Key
    {
        get
        {
            return this.keyField;
        }
        set
        {
            this.keyField = value;
        }
    }

    /// <remarks/>
    public string Value
    {
        get
        {
            return this.valueField;
        }
        set
        {
            this.valueField = value;
        }
    }
}

