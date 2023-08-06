using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events.EventAbstracts;
using Lakea_Stream_Assistant.Models.Configuration;

namespace Lakea_Stream_Assistant.Models.Events.EventLists
{
    //Inherits from Event, stores event information for what to do when a Event is triggered
    public class EventItem : Event
    {
        private readonly IDictionary<string, string> args;
        private readonly EventTarget target;
        private readonly EventGoal goal;
        private readonly string name;
        private readonly string id;
        private readonly int duration;
        private readonly string callback;
        private readonly bool usePreviousArguments;

        public EventItem(ConfigEvent eve)
        {
            EnumConverter enums = new EnumConverter();
            this.source = enums.ConvertEventSourceString(eve.EventDetails.Source);
            this.type = enums.ConvertEventTypeString(eve.EventDetails.Type);
            this.target = enums.ConvertEventTargetString(eve.EventTarget.Target);
            this.goal = enums.ConvertEventGoalString(eve.EventTarget.Goal);       
            this.name = eve.EventDetails.Name;
            this.id = eve.EventDetails.ID;
            //this.args = eve.EventTarget.Args;
            this.duration = eve.EventTarget.Duration;
            this.callback = eve.EventTarget.Callback;
            this.usePreviousArguments = eve.EventTarget.UsePreviousArguments;
            this.args = new Dictionary<string, string>();
            foreach(ConfigEventEventTargetArg arg in eve.EventTarget.Args)
            {
                this.args.Add(arg.Key, arg.Value);
            }
        }

        public EventItem(EventItem item, IDictionary<string, string> args)
        {
            this.source = item.Source;
            this.target = item.target;
            this.goal = item.goal;
            this.name = item.name;
            this.id = item.id;
            this.duration = item.duration;
            this.callback = item.callback;
            this.args = args;
        }

        public override EventSource Source { get { return source; } }
        public override EventType Type { get { return type; } }
        public EventTarget EventTarget { get { return target; } }
        public EventGoal EventGoal { get { return goal; } }
        public string Name { get { return name; } }
        public string ID { get { return id; } }
        public IDictionary<string, string> Args { get { return args; } }
        public int Duration { get { return duration; } }
        public string Callback { get { return callback; } }
        public bool UsePreviousArguments { get {  return usePreviousArguments; } }
    }
}
