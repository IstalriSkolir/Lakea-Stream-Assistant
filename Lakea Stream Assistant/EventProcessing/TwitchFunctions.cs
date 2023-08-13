using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Configuration;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Singletons;

namespace Lakea_Stream_Assistant.EventProcessing
{
    // Functions for handling Twitch Events
    public class TwitchFunctions
    {
        private EventProcesser processer;
        private IDictionary<string, EventItem> follows;
        private IDictionary<string, EventItem> bits;
        private IDictionary<string, EventItem> redeems;
        private IDictionary<string, EventItem> commands;
        private IDictionary<string, EventItem> raids;
        private List<Tuple<int, string>> bitsOrder;

        //Contructor stores list of events to check against when it receives a new event
        public TwitchFunctions(ConfigEvent[] events, EventProcesser processer)
        {
            this.processer = processer;
            follows = new Dictionary<string, EventItem>();
            bits = new Dictionary<string, EventItem>();
            redeems = new Dictionary<string, EventItem>();
            commands = new Dictionary<string, EventItem>();
            raids = new Dictionary<string, EventItem>();
            EnumConverter enums = new EnumConverter();
            foreach (ConfigEvent eve in events)
            {
                try
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
                            case EventType.Twitch_Command:
                                commands.Add(eve.EventDetails.ID, new EventItem(eve));
                                break;
                            case EventType.Twitch_Raid:
                                raids.Add(eve.EventDetails.ID, new EventItem(eve));
                                break;
                            default:
                                Console.WriteLine("Lakea: Invalid 'EventType' in 'TwitchFunctions' Constructor -> " + type);
                                Logs.Instance.NewLog(LogLevel.Warning, new Exception("Lakea: Invalid 'EventType' in 'TwitchFunctions' Constructor -> " + type));
                                break;
                        }
                    }
                }
                catch(Exception ex)
                {
                    Console.Error.WriteLine("Lakea: Error Loading Event -> " + eve.EventDetails.Name);
                    Logs.Instance.NewLog(LogLevel.Error, ex);
                }
            }
            bitsOrder = sortBitsOrder();
        }

        // Sort out bits in order of amount so that we can call events based on bit amount
        private List<Tuple<int, string>> sortBitsOrder()
        {
            List<Tuple<int, string>> bitsOrder = new List<Tuple<int, string>>();
            foreach (var eve in bits)
            {
                int bitAmount = int.Parse(eve.Value.Args["BitsAmount"]);
                string id = eve.Value.ID;
                Tuple<int, string> tuple = Tuple.Create(bitAmount, id);
                bitsOrder.Add(tuple);
            }
            bitsOrder.Sort();
            return bitsOrder;
        }

        //When a follow event is triggered, checks the follow dictionary for event before triggering events effect
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
                    Console.WriteLine("Lakea: Unrecognised Follow Channel -> " + eve.Args.FollowedChannelId);
                    Logs.Instance.NewLog(LogLevel.Warning, "Unrecognised Follow Channel Event -> " + eve.Args.FollowedChannelId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lakea: Twitch Follows Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        //When a channel redeem event is triggered, checks the bits dictionary for event before triggering the events effect
        public void NewBits(TwitchBits eve)
        {
            bool eventFound = false;
            int bitAmount = eve.Args.BitsUsed;
            for (int i = 0; i < bitsOrder.Count; i++)
            {
                if (i + 1 != bitsOrder.Count)
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
                    if (bitAmount >= bitsOrder[i].Item1)
                    {
                        eventFound = true;
                        string id = bitsOrder[i].Item2;
                        processer.ProcessEvent(bits[id]);
                    }
                }
            }
            if (!eventFound)
            {
                Console.WriteLine("Lakea: Bit Event Warning-> " + eve.Args.BitsUsed);
                Logs.Instance.NewLog(LogLevel.Warning, "Bit Event Warning -> " + eve.Args.BitsUsed);
            }
        }

        //When a channel redeem event is triggered, checks the redeem dictionary for event before triggering the events effect
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
                    Logs.Instance.NewLog(LogLevel.Warning, "Unrecognised Channel Redeem -> " + eve.Args.RewardRedeemed.Redemption.Reward.Title + " - " + eve.Args.RewardRedeemed.Redemption.Reward.Id);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lakea: Twitch Redeem Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        //When a chat command event is triggered, checks the commands dictionary for event before triggering the events effect
        public void NewCommand(TwitchCommand eve)
        {
            try
            {
                if (commands.ContainsKey(eve.Args.Command.CommandText))
                {
                    processer.ProcessEvent(commands[eve.Args.Command.CommandText]);
                }
                else
                {
                    Console.WriteLine("Lakea: Unrecognised Channel Command -> " + eve.Args.Command.CommandIdentifier + eve.Args.Command.CommandText);
                    Logs.Instance.NewLog(LogLevel.Warning, "Unrecognised Channel Command -> " + eve.Args.Command.CommandIdentifier + eve.Args.Command.CommandText);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lakea: Twitch Command Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }
    
        //When a channel raid event is triggered, checks the raid dictionary for event before triggering the events effect
        public void NewRaid(TwitchRaid eve)
        {
            try
            {
                string id = "Twitch_Raid_" + eve.Args.RaidNotification.DisplayName;
                if(raids.ContainsKey(id))
                {
                    processer.ProcessEvent(raids[id]);
                }
                else if (raids.ContainsKey("Twitch_Raid_Default"))
                {
                    processer.ProcessEvent(raids["Twitch_Raid_Default"]);
                }
                else if(raids.Count > 0)
                {
                    Console.WriteLine("Lakea: Unrecognised Raid Event, No Default Event Set -> " + eve.Args.RaidNotification.DisplayName);
                    Logs.Instance.NewLog(LogLevel.Warning, "Lakea: Unrecognised Raid Event, No Default Event Set -> " + eve.Args.RaidNotification.DisplayName);
                }
                else
                {
                    Console.WriteLine("Lakea: No Raid Events Configured");
                    Logs.Instance.NewLog(LogLevel.Info, "No Raid Events Configured -> " + eve.Args.RaidNotification.DisplayName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lakea: Twitch Raid Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }
    }
}
