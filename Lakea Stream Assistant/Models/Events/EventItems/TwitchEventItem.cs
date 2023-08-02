using Lakea_Stream_Assistant.Enums;

namespace Lakea_Stream_Assistant.Models.Events.EventLists
{
    //Inherits from TwitchEvent, stores event information for what to do when a Twitch Event is triggered
    public class TwitchEventItem : TwitchEvent
    {
        private readonly EventTarget target;
        private readonly EventGoal goal;
        private readonly string name;
        private readonly string id;
        private readonly string obj;
        private readonly int duration;

        public TwitchEventItem(ConfigEvent eve)
        {
            switch (eve.EventDetails.Type)
            {
                case "Redeem":
                    this.eventType = TwitchEventType.Redeem;
                    break;
                default:
                    Console.WriteLine("Error Parsing Event '" + eve.EventDetails.Name + "' -> Unrecognised Event Type: " + eve.EventDetails.Type);
                    break;
            }
            switch (eve.EventTarget.Target)
            {
                case "OBS":
                    this.target = EventTarget.OBS;
                    break;
                default:
                    Console.WriteLine("Error Parsing Event '" + eve.EventDetails.Name + "' -> Unrecognised Event Target: " + eve.EventTarget.Target);
                    break;
            }
            switch (eve.EventTarget.Goal)
            {
                case "Activate_Source":
                    this.goal = EventGoal.Enable_OBS_Source;
                    break;
                case "Deactivate_Source":
                    this.goal = EventGoal.Disable_OBS_Source;
                    break;
                case "Change_Scene":
                    this.goal = EventGoal.Change_OBS_Scene;
                    break;
                default:
                    Console.WriteLine("Error Parsing Event '" + eve.EventDetails.Name + "' -> Unrecognised Event Goal: " + eve.EventTarget.Goal);
                    break;
            }
            this.source = EventSource.Twitch;
            this.name = eve.EventDetails.Name;
            this.id = eve.EventDetails.ID;
            this.obj = eve.EventTarget.Object;
            this.duration = eve.EventTarget.Duration;
        }

        public override EventSource Source { get { return source; } }
        public override TwitchEventType EventType { get { return eventType; } }
        public EventTarget EventTarget { get { return target; } }
        public EventGoal EventGoal { get { return goal; } }
        public string Name { get { return name; } }
        public string ID { get { return id; } }
        public string Object { get { return obj; } }
        public int Duration { get { return duration; } }
    }
}
