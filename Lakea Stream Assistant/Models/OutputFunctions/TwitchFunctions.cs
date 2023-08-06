using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Configuration;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Models.OutputFunctions;

namespace Lakea_Stream_Assistant.Models.Twitch
{
    // Functions for handling Twitch Events
    public class TwitchFunctions
    {
        private EventProcesser processer;
        private IDictionary<string, EventItem> follows;
        private IDictionary<string, EventItem> bits;
        private IDictionary<string, EventItem> redeems;
        private List<Tuple<int, string>> bitsOrder;
        
        //Contructor stores list of events to check against when it receives a new event
        public TwitchFunctions(ConfigEvent[] events, EventProcesser processer)
        {
            this.processer = processer;
            this.follows = new Dictionary<string, EventItem>();
            this.bits = new Dictionary<string, EventItem>();
            this.redeems = new Dictionary<string, EventItem>();
            EnumConverter enums = new EnumConverter();
            foreach (ConfigEvent eve in events)
            {
                EventSource source = enums.ConvertEventSourceString(eve.EventDetails.Source);
                if (source == EventSource.Twitch)
                {
                    EventType type = enums.ConvertEventTypeString(eve.EventDetails.Type);
                    switch (type)
                    {
                        case EventType.Twitch_Follow:
                            follows.Add(eve.EventDetails.ID, new EventItem(eve));
                            break;
                        case EventType.Twitch_Bits:
                            bits.Add(eve.EventDetails.ID, new EventItem(eve));
                            break;
                        case EventType.Twitch_Redeem:
                            redeems.Add(eve.EventDetails.ID, new EventItem(eve));
                            break;
                        default:
                            Console.WriteLine("Lakea: Invalid 'EventType' in 'TwitchFunctions' Constructor -> " + type);
                            break;
                    }                   
                }
            }
            bitsOrder = sortBitsOrder();
        }

        // Sort out bits in order of amount so that we can call events based on bit amount
        private List<Tuple<int, string>> sortBitsOrder()
        {
            List<Tuple<int, string>> bitsOrder = new List<Tuple<int, string>>();
            foreach(var eve in bits)
            {
                int bitAmount = Int32.Parse(eve.Value.Args[1]);
                string id = eve.Value.ID;
                Tuple<int, string> tuple = Tuple.Create(bitAmount, id);
                bitsOrder.Add(tuple);
            }
            bitsOrder.Sort();
            return bitsOrder;
        }

        //When a follow event is triggered, checks dictionary for event before triggering events effect
        public void NewFollow(TwitchFollow eve)
        {
            try
            {
                if (follows.ContainsKey(eve.Args.FollowedChannelId))
                {
                    processer.ProcessEvent(follows[eve.Args.FollowedChannelId]);
                }
                else
                {
                    Console.WriteLine("Lakea: Unrecognised Follow Channel-> " + eve.Args.FollowedChannelId);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Lakea: Twitch Follows Error -> " + e.Message);
            }
        }

        //When a channel redeem event is triggered, checks dictionary for event before triggering the events effect
        public void NewBits(TwitchBits eve)
        {
            bool eventFound = false;
            int bitAmount = eve.Args.BitsUsed;
            for(int i = 0; i < bitsOrder.Count; i++)
            {
                if(i + 1 != bitsOrder.Count)
                {
                    if (bitAmount >= bitsOrder[i].Item1 && bitAmount < bitsOrder[i + 1].Item1)
                    {
                        eventFound = true;
                        string id = bitsOrder[i].Item2;
                        processer.ProcessEvent(bits[id]);
                    }
                }
                else
                {
                    if(bitAmount >= bitsOrder[i].Item1)
                    {
                        eventFound = true;
                        string id = bitsOrder[i].Item2;
                        processer.ProcessEvent(bits[id]);
                    }
                }
            }
            if (!eventFound)
            {
                Console.WriteLine("Lakea: Bit Event Error -> " + eve.Args.BitsUsed);
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
