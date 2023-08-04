using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events.EventAbstracts;

namespace Lakea_Stream_Assistant.Models.Events.EventItems
{
    //Inherits from LakeaEvent, stores event information for what to do when a Lakea Event is triggered
    public class LakeaEventItem : LakeaEvent
    {
        private readonly EventTarget target;
        private readonly EventGoal goal;
        private readonly string name;
        private readonly string id;
        private readonly string[] args;
        private readonly int duration;
        private readonly string callback;
        private readonly bool usePreviousArguments;

        public LakeaEventItem(ConfigEvent eve)
        {
            switch (eve.EventTarget.Target.ToLower())
            {
                case "obs":
                    this.target = EventTarget.OBS;
                    break;
                case "twitch":
                    this.target = EventTarget.Twitch;
                    break;
                case "base_camp":
                    this.target = EventTarget.Base_Camp;
                    break;
                default:
                    Console.WriteLine("Error Parsing Event '" + eve.EventDetails.Name + "' -> Unrecognised Event Target: " + eve.EventTarget.Target);
                    break;
            }
            switch (eve.EventTarget.Goal.ToLower())
            {
                case "activate_source":
                    this.goal = EventGoal.Enable_OBS_Source;
                    break;
                case "deactivate_source":
                    this.goal = EventGoal.Disable_OBS_Source;
                    break;
                case "enable_random_source":
                    this.goal = EventGoal.Enable_Random_OBS_Source;
                    break;
                case "disable_random_source":
                    this.goal = EventGoal.Disable_Random_OBS_Source;
                    break;
                case "change_scene":
                    this.goal = EventGoal.Change_OBS_Scene;
                    break;
                case "send_chat_message":
                    this.goal = EventGoal.Send_Twitch_Chat_Message;
                    break;
                default:
                    Console.WriteLine("Error Parsing Event '" + eve.EventDetails.Name + "' -> Unrecognised Event Goal: " + eve.EventTarget.Goal);
                    break;
            }
            this.source = EventSource.Lakea;
            this.name = eve.EventDetails.Name;
            this.id = eve.EventDetails.ID;
            this.args = eve.EventTarget.Args;
            this.duration = eve.EventTarget.Duration;
            this.callback = eve.EventTarget.Callback;
            this.usePreviousArguments = eve.EventTarget.UsePreviousArguments;
        }

        public LakeaEventItem(LakeaEventItem item, string[] args)
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
        public override LakeaEventType EventType { get { return eventType; } }
        public EventTarget EventTarget { get { return target; } }
        public EventGoal EventGoal { get { return goal; } }
        public string Name { get { return name; } }
        public string ID { get { return id; } }
        public string[] Args { get { return args; } }
        public int Duration { get { return duration; } }
        public string Callback { get { return callback; } }
        public bool UsePreviousArguments { get {  return usePreviousArguments; } }
    }
}
