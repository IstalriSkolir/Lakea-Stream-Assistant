using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Exceptions;
using Lakea_Stream_Assistant.Models.Events.EventItems;
using Lakea_Stream_Assistant.Singletons;
using System.IO;
using System.Reflection;

namespace Lakea_Stream_Assistant.Models.Configuration
{
    //Class for checking if a loaded configuration is valid
    public class ValidateConfig
    {
        private EnumConverter enums;
        private List<string> ids;
        private List<string> enumStrings;
        private List<string> nullableOrZero;
        private List<string> errors;
        private bool valid;

        //Class constructor
        public ValidateConfig()
        {
            enums = new EnumConverter();
            ids = new List<string>();
            enumStrings = new List<string>
            {
                "Source",
                "Type",
                "Target",
                "Goal"
            };
            nullableOrZero = new List<string>
            {
                "Callback",
                "Delay",
                "Duration",
                "Length"
            };
            errors = new List<string>();
            valid = true;
        }

        //Public function that starts the validation process
        public bool ValidateConfiguration(Config config)
        {
            try
            {
                if(config != null)
                {
                    checkObjectAndChildren(config.Settings, "Config.Settings", "Settings");
                    checkObjectAndChildren(config.OBS, "Config.OBS", "OBS");
                    checkObjectAndChildren(config.Twitch, "Config.Twitch", "Twitch");
                    checkArrayOfObjectsAndChildren(config.Applications, "Config.Applications", "Application");
                    checkArrayOfObjectsAndChildren(config.Events, "Config.Events", "Event");
                    if (!valid)
                    {
                        logErrors();
                        return false;
                    }
                    return true;
                }
                else
                {
                    throw new ConfigValidationException("Config object is null");
                }
            }
            catch(ConfigValidationException ex)
            {
                Logs.Instance.NewLog(LogLevel.Error, ex);
                return false;
            }
            catch(Exception ex)
            {
                logErrors();
                Logs.Instance.NewLog(LogLevel.Error, ex);
                return false;
            }
        }

        //For object arrays, calls checkObjectAndChildren() for each element in the array
        private void checkArrayOfObjectsAndChildren(object[] obj, string path, string objType)
        {
            if(obj != null)
            {
                for (int index = 0; index < obj.Length; index++)
                {
                    checkObjectAndChildren(obj[index], path + "." + objType + (index + 1), objType);
                }
            }
            else
            {
                errorFound(path + " is null");
            }
        }

        //Recursive function, checks properties of and object for invalid values, if property is a custom class then it recalls itself with that property
        private void checkObjectAndChildren(object obj, string path, string name)
        {
            if(obj != null)
            {
                foreach (PropertyInfo property in obj.GetType().GetProperties())
                {
                    if (property.GetValue(obj) == null && !nullableOrZero.Contains(property.Name))
                    {
                        errorFound(path + "." + property.Name + " is null");
                    }
                    else if (property.PropertyType == typeof(string))
                    {
                        if((string)property.GetValue(obj) == string.Empty || (string)property.GetValue(obj) == "")
                        {
                            errorFound(path + "." + property.Name + " is empty");
                        }
                        else if(enumStrings.Contains(property.Name))
                        {
                            checkEnums(obj, property, path);
                        }
                        else if ("ID".Equals(property.Name))
                        {
                            checkIDList(obj, property, path);
                        }
                    }
                    else if (property.PropertyType == typeof(int) && (int)property.GetValue(obj) == 0 && !nullableOrZero.Contains(property.Name))
                    {
                        errorFound(path + "." + property.Name + " can not be equal to zero");
                    }
                    else if (property.PropertyType == typeof(bool) && ((bool)property.GetValue(obj) != false && (bool)property.GetValue(obj) != true))
                    {
                        errorFound(path + "." + property.Name + " can only be of value 'true' or 'false'");
                    }
                    else if (property.PropertyType.Assembly.FullName.Contains("Lakea Stream Assistant") && property.GetType().GetProperties().Length > 0)
                    {
                        if ("Callback".Equals(property.Name))
                        {
                            checkForCallbackLoop((Callbacks)property.GetValue(obj), path);
                        }
                        checkObjectAndChildren(property.GetValue(obj), path + "." + property.Name, property.Name);
                    }
                }
            }
            else if(!nullableOrZero.Contains(name))
            {
                errorFound(path + " is null");
            }
        }

        //Checks if a string value can be parsed into the corresponding Enum type
        private void checkEnums(object obj, PropertyInfo property, string path)
        {
            try
            {
                switch (property.Name)
                {
                    case "Source":
                        EventSource testSource = enums.ConvertEventSourceString((string)property.GetValue(obj));
                        break;
                    case "Type":
                        EventType testType = enums.ConvertEventTypeString((string)property.GetValue(obj));
                        break;
                    case "Target":
                        EventTarget testTarget = enums.ConvertEventTargetString((string)property.GetValue(obj));
                        break;
                    case "Goal":
                        EventGoal testGoal = enums.ConvertEventGoalString((string)property.GetValue(obj));
                        break;
                }
            }
            catch(EnumConversionException)
            {
                errorFound(path + "." + property.Name + " could not be parsed into Enum type");
            }
        }

        //Check that each ID is unique and isn't duplicated
        private void checkIDList(object obj, PropertyInfo property, string path)
        {
            string id = (string)property.GetValue(obj);
            if (ids.Contains(id))
            {
                errorFound(path + "." + property.Name + " has ID that already exists: " + id);
            }
            else
            { 
                ids.Add(id);
            }
        }

        //Todo
        //Check for Callback loop
        private void checkForCallbackLoop(Callbacks callback, string path)
        {

        }

        //Sets the validaty of the config to false and adds an error messages to the error list
        private void errorFound(string errorMessage)
        {
            valid = false;
            errors.Add(errorMessage);
        }

        //Sends every error message collected to the logging singleton to write to file
        private void logErrors()
        {
            for(int index = 0; index < errors.Count; index++)
            {
                Logs.Instance.NewLog(LogLevel.Error, "Config Validation Error " + (index + 1) + " of " + errors.Count + " - " + errors[index]);
            }
        }
    }
}
