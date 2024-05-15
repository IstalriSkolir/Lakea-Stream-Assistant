/*

Code below has been generated from Config.xml, it should not be manually editted. Any changes is data structure should be done by editting the
xml file and regenerating this structure from the editted xml. To regenerate structure, select all the code below and delete it. Then select all
in the xml file before returning here and selecting Edit -> Paste Special -> Paste XML As Classes

NOTE: For some reason, the last several changes using the above method have cause Visual Studio to produce the code in a invalid setup, changes
have now been applied by hand until this issue is resolved

*/
// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class Config
{

    private ConfigSettings settingsField;

    private ConfigOBS oBSField;

    private ConfigTwitch twitchField;

    private ConfigApplication[] applicationsField;

    private ConfigEvent[] eventsField;

    /// <remarks/>
    public ConfigSettings Settings
    {
        get
        {
            return this.settingsField;
        }
        set
        {
            this.settingsField = value;
        }
    }

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
    [System.Xml.Serialization.XmlArrayItemAttribute("Application", IsNullable = false)]
    public ConfigApplication[] Applications
    {
        get
        {
            return this.applicationsField;
        }
        set
        {
            this.applicationsField = value;
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


// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class ConfigSettings
{

    private string logLevelField;

    private int terminalRefreshRateField;

    private string resourcePathField;

    private string pythonExePathField;

    private SettingsCommands commandsField;

    private SettingsCapturedEventRetort capturedEventRetortsField;

    private SettingsWebSocket webSocketField;

    /// <remarks/>
    public string LogLevel
    {
        get
        {
            return this.logLevelField;
        }
        set
        {
            this.logLevelField = value;
        }
    }

    /// <remarks/>
    public int TerminalRefreshRate
    {
        get
        {
            return this.terminalRefreshRateField;
        }
        set
        {
            this.terminalRefreshRateField = value;
        }
    }

    /// <remarks/>
    public string ResourcePath
    {
        get
        {
            return this.resourcePathField;
        }
        set
        {
            this.resourcePathField = value;
        }
    }

    /// <remarks/>
    public string PythonExePath
    {
        get
        {
            return this.pythonExePathField;
        }
        set
        {
            this.pythonExePathField = value;
        }
    }

    /// <remarks/>
    public SettingsCommands Commands
    {
        get
        {
            return this.commandsField;
        }
        set
        {
            this.commandsField = value;
        }
    }

    /// <remarks/>
    public SettingsCapturedEventRetort CapturedEventRetorts
    {
        get
        {
            return this.capturedEventRetortsField;
        }
        set
        {
            this.capturedEventRetortsField = value;
        }
    }

    public SettingsWebSocket WebSocket
    {
        get
        {
            return this.webSocketField;
        }
        set
        {
            this.webSocketField = value;
        }
    }
}

// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class SettingsCommands
{

    private CommandsTotalBits totalBitsField;

    private CommandsResetTerminal resetTerminalField;

    private CommandsProcess processField;

    private CommandsShoutOut shoutOutField;

    private CommandsStatus statusField;

    private CommandsQuotes quotesField;

    private CommandsExit exitField;

    /// <remarks/>
    public CommandsTotalBits TotalBits
    {
        get
        {
            return this.totalBitsField;
        }
        set
        {
            this.totalBitsField = value;
        }
    }

    /// <remarks/>
    public CommandsResetTerminal ResetTerminal
    {
        get
        {
            return this.resetTerminalField;
        }
        set
        {
            this.resetTerminalField = value;
        }
    }

    /// <remarks/>
    public CommandsProcess Process
    {
        get
        {
            return this.processField;
        }
        set
        {
            this.processField = value;
        }
    }

    /// <remarks/>
    public CommandsShoutOut ShoutOut
    {
        get
        {
            return this.shoutOutField;
        }
        set
        {
            this.shoutOutField = value;
        }
    }

    /// <remarks/>
    public CommandsStatus Status
    {
        get
        {
            return this.statusField;
        }
        set
        {
            this.statusField = value;
        }
    }

    /// <remarks/>
    public CommandsQuotes Quotes
    {
        get
        {
            return this.quotesField;
        }
        set
        {
            this.quotesField = value;
        }
    }

    /// <remarks/>
    public CommandsExit Exit
    {
        get
        {
            return this.exitField;
        }
        set
        {
            this.exitField = value;
        }
    }
}

// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class SettingsCapturedEventRetort
{

    private string[] eventTypeField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("EventType")]
    public string[] EventType
    {
        get
        {
            return this.eventTypeField;
        }
        set
        {
            this.eventTypeField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class CommandsTotalBits
{

    private bool enabledField;

    private bool modOnlyField;

    /// <remarks/>
    public bool Enabled
    {
        get
        {
            return this.enabledField;
        }
        set
        {
            this.enabledField = value;
        }
    }

    /// <remarks/>
    public bool ModOnly
    {
        get
        {
            return this.modOnlyField;
        }
        set
        {
            this.modOnlyField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class CommandsResetTerminal
{

    private bool enabledField;

    private bool modOnlyField;

    /// <remarks/>
    public bool Enabled
    {
        get
        {
            return this.enabledField;
        }
        set
        {
            this.enabledField = value;
        }
    }

    /// <remarks/>
    public bool ModOnly
    {
        get
        {
            return this.modOnlyField;
        }
        set
        {
            this.modOnlyField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class CommandsProcess
{

    private bool enabledField;

    private bool modOnlyField;

    /// <remarks/>
    public bool Enabled
    {
        get
        {
            return this.enabledField;
        }
        set
        {
            this.enabledField = value;
        }
    }

    /// <remarks/>
    public bool ModOnly
    {
        get
        {
            return this.modOnlyField;
        }
        set
        {
            this.modOnlyField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class CommandsShoutOut
{

    private bool enabledField;

    private bool modOnlyField;

    /// <remarks/>
    public bool Enabled
    {
        get
        {
            return this.enabledField;
        }
        set
        {
            this.enabledField = value;
        }
    }

    /// <remarks/>
    public bool ModOnly
    {
        get
        {
            return this.modOnlyField;
        }
        set
        {
            this.modOnlyField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class CommandsStatus
{

    private bool enabledField;

    private bool modOnlyField;

    /// <remarks/>
    public bool Enabled
    {
        get
        {
            return this.enabledField;
        }
        set
        {
            this.enabledField = value;
        }
    }

    /// <remarks/>
    public bool ModOnly
    {
        get
        {
            return this.modOnlyField;
        }
        set
        {
            this.modOnlyField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class CommandsQuotes
{

    private bool enabledField;

    private bool modOnlyField;

    /// <remarks/>
    public bool Enabled
    {
        get
        {
            return this.enabledField;
        }
        set
        {
            this.enabledField = value;
        }
    }

    /// <remarks/>
    public bool ModOnly
    {
        get
        {
            return this.modOnlyField;
        }
        set
        {
            this.modOnlyField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class CommandsExit
{

    private bool enabledField;

    private bool modOnlyField;

    /// <remarks/>
    public bool Enabled
    {
        get
        {
            return this.enabledField;
        }
        set
        {
            this.enabledField = value;
        }
    }

    /// <remarks/>
    public bool ModOnly
    {
        get
        {
            return this.modOnlyField;
        }
        set
        {
            this.modOnlyField = value;
        }
    }
}

// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class SettingsWebSocket
{
    private bool enabledField;

    private string ipField;

    private ushort portField;

    /// <remarks/>
    public bool Enabled
    {
        get
        { 
            return this.enabledField;
        }
        set
        {
            this.enabledField = value;
        }
    }

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
}



/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class ConfigOBS
{

    private string ipField;

    private int portField;

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
    public int Port
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

    private int idField;

    private string clientIDField;

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
    public int ID
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
    public string ClientID
    {
        get
        {
            return this.clientIDField;
        }
        set
        {
            this.clientIDField = value;
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


// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class Applications
{

    private ConfigApplication[] applicationField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Application")]
    public ConfigApplication[] Application
    {
        get
        {
            return this.applicationField;
        }
        set
        {
            this.applicationField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class ConfigApplication
{

    private string nameField;

    private string pathField;

    private string windowStyleField;

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
    public string Path
    {
        get
        {
            return this.pathField;
        }
        set
        {
            this.pathField = value;
        }
    }

    /// <remarks/>
    public string WindowStyle
    {
        get
        {
            return this.windowStyleField;
        }
        set
        {
            this.windowStyleField = value;
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

    private ConfigEventEventTargetCallback callbackField;

    private int durationField;

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
    public ConfigEventEventTargetCallback Callback
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
    public int Duration
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

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class ConfigEventEventTargetCallback
{

    private string eventIDField;

    private int delayField;

    /// <remarks/>
    public string EventID
    {
        get
        {
            return this.eventIDField;
        }
        set
        {
            this.eventIDField = value;
        }
    }

    /// <remarks/>
    public int Delay
    {
        get
        {
            return this.delayField;
        }
        set
        {
            this.delayField = value;
        }
    }
}