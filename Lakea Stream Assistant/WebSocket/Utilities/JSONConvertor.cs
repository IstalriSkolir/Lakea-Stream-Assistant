using Lakea_Stream_Assistant.Models.Events.EventLists;
using Newtonsoft.Json.Linq;

namespace Lakea_Stream_Assistant.WebSocket.Utilities
{
    public class JSONConvertor
    {
        public EventItem CreateEventItem(JObject json)
        {
            ConfigEvent eve = new ConfigEvent();
            eve.EventDetails = new ConfigEventEventDetails();
            eve.EventTarget = new ConfigEventEventTarget();
            eve.EventTarget.Args = new ConfigEventEventTargetArg[0];
            JObject eventDetails = (JObject)json["EventItem"]["EventDetails"];
            eve.EventDetails.Source = (string)eventDetails["Source"];
            eve.EventDetails.Type = (string)eventDetails["Type"];
            if (eventDetails.ContainsKey("ID")) { eve.EventDetails.ID = (string)eventDetails["ID"]; } else { eve.EventDetails.ID = "null"; }
            if (eventDetails.ContainsKey("Name")) { eve.EventDetails.Name = (string)eventDetails["Name"]; } else { eve.EventDetails.Name = "null"; }
            JObject eventTarget = (JObject)json["EventItem"]["EventTarget"];
            if(eventTarget != null)
            {
                if (eventTarget.ContainsKey("Target")) { eve.EventTarget.Target = (string)eventTarget["Target"]; } else { eve.EventTarget.Target = "null"; }
                if (eventTarget.ContainsKey("Goal")) { eve.EventTarget.Goal = (string)eventTarget["Goal"]; } else { eve.EventTarget.Goal = "null"; }
                if (eventTarget.ContainsKey("UsePreviousArguments")) { eve.EventTarget.UsePreviousArguments = (bool)eventTarget["UsePreviousArguments"]; }
                if (eventTarget.ContainsKey("Duration")) { eve.EventTarget.UsePreviousArguments = (bool)eventTarget["Duration"]; }
                if (eventTarget.ContainsKey("Callback"))
                {
                    eve.EventTarget.Callback = new ConfigEventEventTargetCallback();
                    eve.EventTarget.Callback.EventID = (string)eventTarget["Callback"]["EventID"];
                    eve.EventTarget.Callback.Delay = (int)eventTarget["Callback"]["Delay"];

                }
                if (eventTarget.ContainsKey("Args"))
                {
                    JArray args = (JArray)eventTarget["Args"];
                    eve.EventTarget.Args = new ConfigEventEventTargetArg[args.Count()];
                    for (int index = 0; index < args.Count(); index++)
                    {
                        eve.EventTarget.Args[index] = new ConfigEventEventTargetArg();
                        eve.EventTarget.Args[index].Key = (string)args[index]["Key"];
                        eve.EventTarget.Args[index].Value = (string)args[index]["Value"];
                    }
                }
            }
            else
            {
                eve.EventTarget.Target = "null";
                eve.EventTarget.Goal = "null";
            }
            return new EventItem(eve);
        }
    }
}
