using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Models.OutputFunctions;

namespace Lakea_Stream_Assistant.Models.Twitch
{
    // Functions for handling Twitch Events
    public class TwitchFunctions
    {
        private EventProcesser processer;
        private IDictionary<string, EventItem> redeems = new Dictionary<string, EventItem>();

        //Contructor stores list of Twitch events to check against when it receives a new event
        public TwitchFunctions(ConfigEvent[] events, EventProcesser processer)
        {
            this.processer = processer;
            foreach (ConfigEvent eve in events)
            {
                if ("twitch".Equals(eve.EventDetails.Source.ToLower()))
                {
                    switch (eve.EventDetails.Type.ToLower())
                    {
                        case "twitch_redeem":
                            redeems.Add(eve.EventDetails.ID, new EventItem(eve));
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
            try
            {
                if (redeems.ContainsKey(eve.Args.RewardRedeemed.Redemption.Reward.Id))
                {
                    processer.ProcessEvent(redeems[eve.Args.RewardRedeemed.Redemption.Reward.Id]);
                }
                else
                {
                    Console.WriteLine("Lakea: Unrecognised Channel Redeem -> " + eve.Args.RewardRedeemed.Redemption.Reward.Title + " - " + eve.Args.RewardRedeemed.Redemption.Reward.Id);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Lakea: Twitch Redeem Error -> " + e.Message);
            }
        }
    }
}
