using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;

namespace Lakea_Stream_Assistant.Models.Twitch
{
    // Functions for handle Twitch Events
    public class TwitchFunctions
    {
        private EventOutputs outputs;
        private IDictionary<string, TwitchEventItem> redeems = new Dictionary<string, TwitchEventItem>();

        //Contructor stores list of Twitch events to check against when it receives a new event
        public TwitchFunctions(ConfigEvent[] events, EventOutputs outputs)
        {
            this.outputs = outputs;
            foreach (ConfigEvent eve in events)
            {
                if ("Twitch".Equals(eve.EventDetails.Source))
                {
                    switch (eve.EventDetails.Type)
                    {
                        case "Redeem":
                            redeems.Add(eve.EventDetails.ID, new TwitchEventItem(eve));
                            break;
                        default:
                            Console.WriteLine("Error Parsing Event '" + eve.EventDetails.Name + "' -> Unrecognised Event Type: " + eve.EventDetails.Type);
                            break;
                    }
                }
            }
        }

        //When a channel redeem event is triggered, checks dictionary for event before triggering the events effect
        public void NewRedeem(TwitchRedeem eve)
        {
            if (redeems.ContainsKey(eve.Args.RewardRedeemed.Redemption.Reward.Id))
            {
                processTwitchEvent(redeems[eve.Args.RewardRedeemed.Redemption.Reward.Id]);
            }
            else
            {
                Console.WriteLine("Unrecognised Channel Redeem: " + eve.Args.RewardRedeemed.Redemption.Reward.Title + " - " + eve.Args.RewardRedeemed.Redemption.Reward.Id);
            }
        }

        //Checks events target app/platform
        private void processTwitchEvent(TwitchEventItem item)
        {
            switch (item.EventTarget)
            {
                case EventTarget.Base_Camp:
                    break;
                case EventTarget.OBS:
                    eventTargetOBS(item);
                    break;
                case EventTarget.Twitch:
                    break;
            }
        }

        //Checks events OBS target and calls relevent function in 'outputs' object
        private void eventTargetOBS(TwitchEventItem item)
        {
            switch (item.EventGoal)
            {
                case EventGoal.Disable_OBS_Source:
                    outputs.SetActiveOBSSource(item.Object, item.Duration, false);
                    break;
                case EventGoal.Enable_OBS_Source:
                    outputs.SetActiveOBSSource(item.Object, item.Duration, true);
                    break;
                case EventGoal.Change_OBS_Scene:
                    outputs.ChangeOBSScene(item.Object);
                    break;
            }
        }
    }
}
