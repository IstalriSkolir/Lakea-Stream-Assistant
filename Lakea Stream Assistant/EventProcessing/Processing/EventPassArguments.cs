using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;
using TwitchLib.Api.Helix.Models.Bits;

namespace Lakea_Stream_Assistant.EventProcessing.Processing
{
    //Get arguments from the triggering events and replace any templates with their corresponding values
    public class EventPassArguments
    {
        private Dictionary<string, Func<Dictionary<string, string>, string, string>> functionCalls;
        
        public EventPassArguments()
        {
            functionCalls = new Dictionary<string, Func<Dictionary<string, string>, string, string>>()
            {
                { "[takeobsscreenshot]", getOBSScreenshot },
                { "[saveobsscreenshot]", saveOBSScreenshot },
                { "[twitchbitsscore]", getTwitchBitsData },
                { "[twitchbitsrank]", getTwitchBitsData }
            };
        }
  
        public EventItem GetEventArgs(EventItem storedItem, IncomingEvent eve)
        {
            try
            {
                if (!checkEventForArgs(storedItem))
                {
                    Dictionary<string, string> currentEveArgs = storedItem.GetArgs();
                    Dictionary<string, string> newEveArgs = eve.Args;
                    foreach (var arg in currentEveArgs)
                    {
                        if (!newEveArgs.ContainsKey(arg.Key))
                        {
                            newEveArgs.Add(arg.Key, arg.Value);
                        }
                        else
                        {
                            newEveArgs.Remove(arg.Key);
                            newEveArgs.Add(arg.Key, arg.Value);
                        }
                    }
                    EventItem newItem = new EventItem(storedItem, newEveArgs);
                    return newItem;
                }
                Dictionary<string, string> triggerArgs = eve.Args;
                Dictionary<string, string> currentArgs = storedItem.GetArgs();
                Dictionary<string, string> adjustedArgs = new Dictionary<string, string>();
                foreach (var arg in triggerArgs)
                {
                    if (!adjustedArgs.ContainsKey(arg.Key))
                    {
                        adjustedArgs.Add(arg.Key, arg.Value);
                    }
                    else
                    {
                        adjustedArgs.Remove(arg.Key);
                        adjustedArgs.Add(arg.Key, arg.Value);
                    }
                }
                foreach (var arg in currentArgs)
                {
                    string value = arg.Value;
                    if (value.Contains('{') && value.Contains('}'))
                    {
                        value = replaceTemplate(triggerArgs, value);
                    }
                    else if (value.Contains('[') && value.Contains(']'))
                    {
                        value = makeFunctionCall(value, adjustedArgs);
                    }
                    adjustedArgs.Add(arg.Key, value);
                }
                EventItem item = new EventItem(storedItem, adjustedArgs);
                return item;
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Pass Event Argument Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }

        //Checks if any of the arguments have templates that need replacing with their corresponding values
        private bool checkEventForArgs(EventItem Item)
        {
            foreach (var arg in Item.GetArgs())
            {
                if ((arg.Value.Contains('{') && arg.Value.Contains('}')) || (arg.Value.Contains('[') && arg.Value.Contains(']')))
                {
                    return true;
                }
            }
            return false;
        }

        //Replaces templates with their corresponding values from the dictionary
        private string replaceTemplate(Dictionary<string, string> triggerArgs, string value)
        {
            int startIndex = value.IndexOf('{');
            int endIndex = value.IndexOf('}');
            int length = endIndex - startIndex;
            string template = value.Substring(startIndex, length + 1);
            string key = template.Remove(template.Length - 1, 1).Remove(0, 1);
            string arg = triggerArgs[key];
            value = value.Replace(template, arg);
            if (value.Contains('{') && value.Contains('}'))
            {
                value = replaceTemplate(triggerArgs, value);
            }
            return value;
        }

        // Replace value with return date of function call, run recursively for multiple templates
        private string makeFunctionCall(string value, Dictionary<string, string> currentArgs)
        {
            int startIndex = value.IndexOf('[');
            int endIndex = value.IndexOf(']');
            int length = endIndex - startIndex;
            string template = value.Substring(startIndex, length + 1);
            if (functionCalls.ContainsKey(template))
            {
                string replacement = functionCalls[template].Invoke(currentArgs, template);
                value = value.Replace(template, replacement);
            }
            if(value.Contains("[") && value.Contains("]"))
                value = makeFunctionCall(value, currentArgs);
            return value;
        }

        // Get OBS screenshot from the source named in the stored events
        private string getOBSScreenshot(Dictionary<string, string> currentArgs, string template)
        {
            if (currentArgs.ContainsKey("SourceName"))
                return OBS.TakeScreenshot(currentArgs["SourceName"]);
            return string.Empty;
        }

        // Save OBS screenshot from the source named in the stored events
        private string saveOBSScreenshot(Dictionary<string, string> currentArgs, string template)
        {
            if (currentArgs.ContainsKey("SourceName"))
            {
                string imageFormat = currentArgs.ContainsKey("ImageFormat") ? currentArgs["ImageFormat"] : "png";
                if (OBS.SaveScreenshot(currentArgs["SourceName"], currentArgs["ImageFilePath"], imageFormat)) return currentArgs["ImageFilePath"];
                else return string.Empty;
            }
            return string.Empty;
        }

        // Get Twitch users total bits, only works on Twitch input events
        private string getTwitchBitsData(Dictionary<string, string> currentArgs, string template)
        {
            if (currentArgs.ContainsKey("AccountID"))
            {
                GetBitsLeaderboardResponse response = Twitch.GetBitsLeaderBoard(1, currentArgs["AccountID"]).Result;
                if (template == "[twitchbitsrank]") return response.Listings[0].Rank.ToString();
                else if (template == "[twitchbitsscore]") return response.Listings[0].Score.ToString();
            }
            return "0";
        }
    }
}
