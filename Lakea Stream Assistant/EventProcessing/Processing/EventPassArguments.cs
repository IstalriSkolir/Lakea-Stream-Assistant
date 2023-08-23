using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events.EventAbstracts;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Singletons;

namespace Lakea_Stream_Assistant.EventProcessing.Processing
{
    //Get arguments from the triggering events and replace any templates with their corresponding values
    public class EventPassArguments
    {
        public EventItem GetEventArgs(EventItem storedItem, Event eve)
        {
            try
            {
                if (!checkEventForArgs(storedItem))
                {
                    Dictionary<string, string> currentEveArgs = storedItem.GetArgs();
                    Dictionary<string, string> newEveArgs = eve.GetArgs();
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
                Dictionary<string, string> triggerArgs = eve.GetArgs();
                Dictionary<string, string> currentArgs = storedItem.GetArgs();
                Dictionary<string, string> adjustedArgs = new Dictionary<string, string>();
                foreach (var arg in currentArgs)
                {
                    string value = arg.Value;
                    if (value.Contains('{') && value.Contains('}'))
                    {
                        value = replaceTemplate(triggerArgs, value);
                    }
                    adjustedArgs.Add(arg.Key, value);
                }
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
                EventItem item = new EventItem(storedItem, adjustedArgs);
                return item;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lakea: Pass Event Argument Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }

        //Checks if any of the arguments have templates that need replacing with their corresponding values
        private bool checkEventForArgs(EventItem Item)
        {
            foreach (var arg in Item.GetArgs())
            {
                if (arg.Value.Contains('{') && arg.Value.Contains('}'))
                {
                    return true;
                }
            }
            return false;
        }

        //Replaces temples with their corresponding values from the dictionary
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
    }
}
