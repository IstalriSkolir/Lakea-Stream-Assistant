using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;

namespace Lakea_Stream_Assistant.EventProcessing.Processing
{
    //Functions for handling Twitch Events
    public class TwitchFunctions
    {
        private EventProcesser processer;
        private EventPassArguments passArgs;
        private Dictionary<string, EventItem> follows;
        private Dictionary<string, EventItem> bits;
        private Dictionary<string, EventItem> redeems;
        private Dictionary<string, EventItem> commands;
        private Dictionary<string, EventItem> raids;
        private Dictionary<string, EventItem> subscriptions;
        private List<Tuple<int, string>> bitsOrder;

        //Contructor stores list of events to check against when it receives a new event
        public TwitchFunctions(ConfigEvent[] events, EventProcesser processer, EventPassArguments passArgs)
        {
            this.processer = processer;
            this.passArgs = passArgs;
            follows = new Dictionary<string, EventItem>();
            bits = new Dictionary<string, EventItem>();
            redeems = new Dictionary<string, EventItem>();
            commands = new Dictionary<string, EventItem>();
            raids = new Dictionary<string, EventItem>();
            subscriptions = new Dictionary<string, EventItem>();
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
                                commands.Add(eve.EventDetails.ID.ToLower(), new EventItem(eve));
                                break;
                            case EventType.Twitch_Raid:
                                raids.Add(eve.EventDetails.ID, new EventItem(eve));
                                break;
                            case EventType.Twitch_Subscription:
                                subscriptions.Add(eve.EventDetails.ID, new EventItem(eve));
                                break;
                            default:
                                Terminal.Output("Lakea: Invalid 'EventType' in 'TwitchFunctions' Constructor -> " + type);
                                Logs.Instance.NewLog(LogLevel.Warning, new Exception("Lakea: Invalid 'EventType' in 'TwitchFunctions' Constructor -> " + type));
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Terminal.Output("Lakea: Error Loading Event -> " + eve.EventDetails.Name);
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
                int bitAmount = int.Parse(eve.Value.GetArgs()["BitsAmount"]);
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
                    EventItem item = follows[eve.Args.FollowedChannelId];
                    item = passArgs.GetEventArgs(item, eve);
                    if (item != null)
                    {
                        processer.ProcessEvent(item);
                    }
                }
                else
                {
                    Terminal.Output("Lakea: Unrecognised Follow Channel -> " + eve.Args.FollowedChannelId);
                    Logs.Instance.NewLog(LogLevel.Warning, "Unrecognised Follow Channel Event -> " + eve.Args.FollowedChannelId);
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Twitch Follows Error -> " + ex.Message);
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
                        EventItem item = passArgs.GetEventArgs(bits[id], eve);
                        if (item != null)
                        {
                            processer.ProcessEvent(item);
                        }
                    }
                }
                else
                {
                    if (bitAmount >= bitsOrder[bitsOrder.Count - 1].Item1)
                    {
                        eventFound = true;
                        string id = bitsOrder[bitsOrder.Count - 1].Item2;
                        EventItem item = passArgs.GetEventArgs(bits[id], eve);
                        if (item != null)
                        {
                            processer.ProcessEvent(item);
                        }
                    }
                }
            }
            if (!eventFound)
            {
                Terminal.Output("Lakea: Bit Event Warning-> " + eve.Args.BitsUsed);
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
                    EventItem item = passArgs.GetEventArgs(redeems[eve.Args.RewardRedeemed.Redemption.Reward.Id], eve);
                    if (item != null)
                    {
                        processer.ProcessEvent(item);
                    }
                }
                else
                {
                    Terminal.Output("Lakea: Unrecognised Channel Redeem -> " + eve.Args.RewardRedeemed.Redemption.Reward.Title + " - " + eve.Args.RewardRedeemed.Redemption.Reward.Id);
                    Logs.Instance.NewLog(LogLevel.Warning, "Unrecognised Channel Redeem -> " + eve.Args.RewardRedeemed.Redemption.Reward.Title + " - " + eve.Args.RewardRedeemed.Redemption.Reward.Id);
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Twitch Redeem Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        //When a chat command event is triggered, checks the commands dictionary for event before triggering the events effect
        public void NewCommand(TwitchCommand eve)
        {
            try
            {
                string command = eve.Args.Command.CommandText.ToLower();
                if (commands.ContainsKey(command))
                {
                    EventItem item = passArgs.GetEventArgs(commands[command], eve);
                    if (item != null)
                    {
                        processer.ProcessEvent(item);
                    }
                }
                else
                {
                    Terminal.Output("Lakea: Unrecognised Channel Command -> " + eve.Args.Command.CommandIdentifier + eve.Args.Command.CommandText);
                    Logs.Instance.NewLog(LogLevel.Warning, "Unrecognised Channel Command -> " + eve.Args.Command.CommandIdentifier + eve.Args.Command.CommandText);
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Twitch Command Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        //When a channel raid event is triggered, checks the raid dictionary for event before triggering the events effect
        public void NewRaid(TwitchRaid eve)
        {
            try
            {
                string id = "Twitch_Raid_" + eve.Args.RaidNotification.DisplayName;
                if (raids.ContainsKey(id))
                {
                    EventItem item = raids[id];
                    item = passArgs.GetEventArgs(item, eve);
                    if (item != null)
                    {
                        processer.ProcessEvent(item);
                    }
                }
                else if (raids.ContainsKey("Twitch_Raid_Default"))
                {
                    EventItem item = raids["Twitch_Raid_Default"];
                    item = passArgs.GetEventArgs(item, eve);
                    if (item != null)
                    {
                        processer.ProcessEvent(item);
                    }
                }
                else if (raids.Count > 0)
                {
                    Terminal.Output("Lakea: Unrecognised Raid Event, No Default Event Set -> " + eve.Args.RaidNotification.DisplayName);
                    Logs.Instance.NewLog(LogLevel.Warning, "Lakea: Unrecognised Raid Event, No Default Event Set -> " + eve.Args.RaidNotification.DisplayName);
                }
                else
                {
                    Terminal.Output("Lakea: No Raid Events Configured");
                    Logs.Instance.NewLog(LogLevel.Info, "No Raid Events Configured -> " + eve.Args.RaidNotification.DisplayName);
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Twitch Raid Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        //When a subscription event is triggered, checks the subscription dictionary for event before triggering the events effect
        public void newSubscription(TwitchPubSubSubscription eve)
        {
            try
            {
                string id = "Twitch_Subscription_" + eve.Args.Subscription.SubscriptionPlan.ToString();
                if (subscriptions.ContainsKey(id))
                {
                    EventItem item = subscriptions[id];
                    item = passArgs.GetEventArgs(item, eve);
                    if (item != null)
                    {
                        processer.ProcessEvent(item);
                    }
                }
                else if (subscriptions.ContainsKey("Twitch_Subscription_Default"))
                {
                    EventItem item = subscriptions["Twitch_Subscription_Default"];
                    item = passArgs.GetEventArgs(item, eve);
                    if (item != null)
                    {
                        processer.ProcessEvent(item);
                    }
                }
                else if (subscriptions.Count > 0)
                {
                    Terminal.Output("Lakea: Unrecognised Subscription Event, No Default Event Set -> " + eve.Args.Subscription.SubscriptionPlan.ToString() + ", " + eve.Args.Subscription.DisplayName);
                    Logs.Instance.NewLog(LogLevel.Warning, "Lakea: Unrecognised Subscription Event, No Default Event Set -> " + eve.Args.Subscription.SubscriptionPlan.ToString() + ", " + eve.Args.Subscription.DisplayName);
                }
                else
                {
                    Terminal.Output("Lakea: No Subscription Events Configured");
                    Logs.Instance.NewLog(LogLevel.Info, "No Subscription Events Configured -> " + eve.Args.Subscription.SubscriptionPlan.ToString() + ", " + eve.Args.Subscription.DisplayName);
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Twitch Subscription Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }
    }
}
