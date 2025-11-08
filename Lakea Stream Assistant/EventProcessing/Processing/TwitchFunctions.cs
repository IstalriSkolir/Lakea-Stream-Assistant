using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.EventProcessing.Commands;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;

namespace Lakea_Stream_Assistant.EventProcessing.Processing
{
    // Functions for handling Twitch Events
    public class TwitchFunctions
    {
        private EventPassArguments passArgs;
        private Dictionary<EventType, Dictionary<string, EventItem>> events;
        private Dictionary<string, EventItem> follows;
        private Dictionary<string, EventItem> bits;
        private Dictionary<string, EventItem> redeems;
        private Dictionary<string, EventItem> commands;
        private Dictionary<string, EventItem> raids;
        private Dictionary<string, EventItem> watchStreaks;
        private Dictionary<string, EventItem> subscriptions;
        private Dictionary<string, EventItem> resubscriptions;
        private Dictionary<string, EventItem> primePaidSubscriptions;
        private Dictionary<string, EventItem> giftedSubscriptions;
        private Dictionary<string, EventItem> continuedGiftedSubscriptions;
        private Dictionary<string, EventItem> firstTimeChatters;
        private List<Tuple<int, string>> bitsOrder;
        private List<Tuple<int, string>> watchStreakOrder;
        Random random;

        // Contructor stores list of events to check against when it receives a new event
        public TwitchFunctions(ConfigEvent[] newEvents, EventPassArguments passArgs, DefaultCommands defaultCommands)
        {
            random = new Random();
            this.passArgs = passArgs;
            follows = new Dictionary<string, EventItem>();
            bits = new Dictionary<string, EventItem>();
            redeems = new Dictionary<string, EventItem>();
            commands = new Dictionary<string, EventItem>();
            raids = new Dictionary<string, EventItem>();
            watchStreaks = new Dictionary<string, EventItem>();
            subscriptions = new Dictionary<string, EventItem>();
            resubscriptions = new Dictionary<string, EventItem>();
            primePaidSubscriptions = new Dictionary<string, EventItem>();
            giftedSubscriptions = new Dictionary<string, EventItem>();
            continuedGiftedSubscriptions = new Dictionary<string, EventItem>();
            firstTimeChatters = new Dictionary<string, EventItem>();
            events = new Dictionary<EventType, Dictionary<string, EventItem>>
            {
                { EventType.Twitch_Follow, follows },
                { EventType.Twitch_Bits, bits },
                { EventType.Twitch_Redeem, redeems },
                { EventType.Twitch_Command, commands },
                { EventType.Twitch_Raid, raids },
                { EventType.Twitch_Watch_Streak, watchStreaks },
                { EventType.Twitch_Subscription, subscriptions },
                { EventType.Twitch_Resubscription, resubscriptions },
                { EventType.Twitch_Prime_Paid_Subscription, primePaidSubscriptions },
                { EventType.Twitch_Gifted_Subscription, giftedSubscriptions },
                { EventType.Twitch_Continued_Gifted_Subscription, continuedGiftedSubscriptions },
                { EventType.Twitch_First_Time_Chatter, firstTimeChatters }
            };
            EnumConverter enums = new EnumConverter();
            foreach (ConfigEvent eve in newEvents)
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
                            case EventType.Twitch_Watch_Streak:
                                watchStreaks.Add(eve.EventDetails.ID, new EventItem(eve));
                                break;
                            case EventType.Twitch_Subscription:
                                subscriptions.Add(eve.EventDetails.ID, new EventItem(eve));
                                break;
                            case EventType.Twitch_Resubscription:
                                resubscriptions.Add(eve.EventDetails.ID, new EventItem(eve));
                                break;
                            case EventType.Twitch_Prime_Paid_Subscription:
                                primePaidSubscriptions.Add(eve.EventDetails.ID, new EventItem(eve));
                                break;
                            case EventType.Twitch_Gifted_Subscription:
                                giftedSubscriptions.Add(eve.EventDetails.ID, new EventItem(eve));
                                break;
                            case EventType.Twitch_Continued_Gifted_Subscription:
                                continuedGiftedSubscriptions.Add(eve.EventDetails.ID, new EventItem(eve));
                                break;
                            case EventType.Twitch_First_Time_Chatter:
                                firstTimeChatters.Add(eve.EventDetails.ID, new EventItem(eve));
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
            watchStreakOrder = sortWatchStreakOrder();
        }

        // Update the Twitch events during runtime
        public void UpdateDictionary(string id, EventItem item, bool remove)
        {
            try
            {
                Dictionary<string, EventItem> toUpdate = events[item.Type];
                if (remove)
                {
                    if (toUpdate.ContainsKey(id))
                    {
                        Terminal.Output("Lakea: Removing Twitch Event -> " + toUpdate[id].Name);
                        Logs.Instance.NewLog(LogLevel.Info, "Removing Twitch Event -> " + toUpdate[id].Name);
                        toUpdate.Remove(id);
                    }
                    else
                    {
                        Terminal.Output("Lakea: No Twitch Event Found -> " + id);
                        Logs.Instance.NewLog(LogLevel.Warning, "No Twitch Event Found -> " + id);
                    }
                }
                else
                {
                    toUpdate.Add(id, item);
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Error Updating Twitch Events -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex.Message);
            }
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

        // Sort out watch streaks order of amount so that we can call events based on watch streak amount
        private List<Tuple<int, string>> sortWatchStreakOrder()
        {
            List<Tuple<int, string>> watchStreaksOrder = new List<Tuple<int, string>>();
            foreach(var eve in watchStreaks)
            {
                int watchStreakAmount = int.Parse(eve.Value.GetArgs()["StreakGoal"]);
                string id = eve.Value.ID;
                Tuple<int, string> tuple = Tuple.Create(watchStreakAmount, id);
                watchStreaksOrder.Add(tuple);
            }
            watchStreaksOrder.Sort();
            return watchStreaksOrder;
        }

        // When a follow event is triggered, checks the follow dictionary for event before triggering events effect
        public EventItem NewFollow(IncomingEvent eve)
        {
            try
            {
                string channelID = eve.Args["ChannelID"];
                if (follows.ContainsKey(channelID))
                {
                    EventItem item = follows[channelID];
                    item = passArgs.GetEventArgs(item, eve);
                    if (item != null)
                    {
                        return item;
                    }
                }
                else
                {
                    Terminal.Output("Lakea: Unrecognised Follow Channel -> " + channelID);
                    Logs.Instance.NewLog(LogLevel.Warning, "Unrecognised Follow Channel Event -> " + channelID);
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Twitch Follows Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }

        // When a channel redeem event is triggered, checks the bits dictionary for event before triggering the events effect
        public EventItem NewBits(IncomingEvent eve)
        {
            //bitsCommands.NewBitsEvent(eve);
            bool eventFound = false;
            int bitAmount = int.Parse(eve.Args["Bits"]);
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
                            return item;
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
                            return item;
                        }
                    }
                }
            }
            if (!eventFound)
            {
                Terminal.Output("Lakea: Bit Event Warning-> " + bitAmount);
                Logs.Instance.NewLog(LogLevel.Warning, "Bit Event Warning -> " + bitAmount);
            }
            return null;
        }

        // When a channel redeem event is triggered, checks the redeem dictionary for event before triggering the events effect
        public EventItem NewRedeem(IncomingEvent eve)
        {
            try
            {
                string redeemTitle = eve.Args["RedeemTitle"];
                string redeemID = eve.Args["RedeemID"];
                if (redeems.ContainsKey(redeemID))
                {
                    EventItem item = passArgs.GetEventArgs(redeems[redeemID], eve);
                    if (item != null)
                    {
                        return item;
                    }
                }
                else
                {
                    Terminal.Output("Lakea: Unrecognised Channel Redeem -> " + redeemTitle + " - " + redeemID);
                    Logs.Instance.NewLog(LogLevel.Warning, "Unrecognised Channel Redeem -> " + redeemTitle + " - " + redeemID);
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Twitch Redeem Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }

        // When a chat command event is triggered, checks the commands dictionary for event before triggering the events effect
        public EventItem NewCommand(IncomingEvent eve)
        {
            try
            {
                string commandIdentifier = eve.Args["CommandIdentifier"];
                string commandLower = eve.Args["CommandText"].ToLower();
                string command = eve.Args["CommandText"];
                if (commands.ContainsKey(commandLower))
                {
                    EventItem item = passArgs.GetEventArgs(commands[commandLower], eve);
                    if (item != null)
                    {
                        return item;
                    }
                }
                else
                {
                    Terminal.Output("Lakea: Unrecognised Channel Command -> " + commandIdentifier + command);
                    Logs.Instance.NewLog(LogLevel.Warning, "Unrecognised Channel Command -> " + commandIdentifier + command);
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Twitch Command Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }

        // When a user reaches a watch streak goal
        public EventItem NewWatchStreak(IncomingEvent eve)
        {
            bool eventFound = false;
            int watchStreakAmount = int.Parse(eve.Args["WatchStreak"]);
            for (int i = 0; i < watchStreakOrder.Count; i++)
            {
                if (i + 1 != watchStreakOrder.Count)
                {
                    if (watchStreakAmount >= watchStreakOrder[i].Item1 && watchStreakAmount < watchStreakOrder[i + 1].Item1)
                    {
                        eventFound = true;
                        string id = watchStreakOrder[i].Item2;
                        EventItem item = passArgs.GetEventArgs(watchStreaks[id], eve);
                        if (item != null)
                        {
                            return item;
                        }
                    }
                }
                else
                {
                    if (watchStreakAmount >= watchStreakOrder[bitsOrder.Count - 1].Item1)
                    {
                        eventFound = true;
                        string id = watchStreakOrder[watchStreakOrder.Count - 1].Item2;
                        EventItem item = passArgs.GetEventArgs(watchStreaks[id], eve);
                        if (item != null)
                        {
                            return item;
                        }
                    }
                }
            }
            if (!eventFound)
            {
                Terminal.Output("Lakea: Watch Streak Event Warning-> " + watchStreakAmount);
                Logs.Instance.NewLog(LogLevel.Warning, "Watch Streak Event Warning -> " + watchStreakAmount);
            }
            return null;
        }

        // When a channel raid event is triggered, checks the raid dictionary for event before triggering the events effect
        public EventItem NewRaid(IncomingEvent eve)
        {
            try
            {
                string displayName = eve.Args["DisplayName"];
                string id = "Twitch_Raid_" + displayName;
                if (raids.ContainsKey(id))
                {
                    EventItem item = raids[id];
                    item = passArgs.GetEventArgs(item, eve);
                    if (item != null)
                    {
                        return item;
                    }
                }
                else if (raids.ContainsKey("Twitch_Raid_Default"))
                {
                    EventItem item = raids["Twitch_Raid_Default"];
                    item = passArgs.GetEventArgs(item, eve);
                    if (item != null)
                    {
                        return item;
                    }
                }
                else if (raids.Count > 0)
                {
                    Terminal.Output("Lakea: Unrecognised Raid Event, No Default Event Set -> " + displayName);
                    Logs.Instance.NewLog(LogLevel.Warning, "Lakea: Unrecognised Raid Event, No Default Event Set -> " + displayName);
                }
                else
                {
                    Terminal.Output("Lakea: No Raid Events Configured");
                    Logs.Instance.NewLog(LogLevel.Info, "No Raid Events Configured -> " + displayName);
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Twitch Raid Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }

        // When a subscription event is triggered, check the subscription dictionary for event before triggering the events effect
        public EventItem NewSubscription(IncomingEvent eve)
        {
            try
            {
                string subscriptionPlan = eve.Args["SubscriptionPlan"];
                string subscriptionPlanName = eve.Args["SubscriptionPlanName"];
                if (subscriptions.ContainsKey(subscriptionPlan))
                {
                    EventItem item = subscriptions[subscriptionPlan];
                    item = passArgs.GetEventArgs(item, eve);
                    if (item != null)
                    {
                        return item;
                    }
                }
                else if (subscriptions.ContainsKey("Twitch_Subscriber_Default"))
                {
                    EventItem item = subscriptions["Twitch_Subscriber_Default"];
                    item = passArgs.GetEventArgs(item, eve);
                    if (item != null)
                    {
                        return item;
                    }
                }
                else
                {
                    Terminal.Output("Lakea: Unrecognised Twitch Subscription -> " + subscriptionPlanName);
                    Logs.Instance.NewLog(LogLevel.Warning, "Unrecognised Twitch Subscription Event -> " + subscriptionPlanName);
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Twitch Subscription Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }

        // When a resubscription event is triggered, check the resubscription dictionary for event before triggering the events effects
        public EventItem NewResubscription(IncomingEvent eve)
        {
            try
            {
                string subscriptionPlan = eve.Args["SubscriptionPlan"];
                string subscriptionPlanName = eve.Args["SubscriptionPlanName"];
                if (resubscriptions.ContainsKey(subscriptionPlan))
                {
                    EventItem item = resubscriptions[subscriptionPlan];
                    item = passArgs.GetEventArgs(item, eve);
                    if (item != null)
                    {
                        return item;
                    }
                }
                else if (resubscriptions.ContainsKey("Twitch_Resubscriber_Default"))
                {
                    EventItem item = resubscriptions["Twitch_Resubscriber_Default"];
                    item = passArgs.GetEventArgs(item, eve);
                    if (item != null)
                    {
                        return item;
                    }
                }
                else
                {
                    Terminal.Output("Lakea: Unrecognised Twitch Resubscription -> " + subscriptionPlanName);
                    Logs.Instance.NewLog(LogLevel.Warning, "Unrecognised Twitch Resubscription Event -> " + subscriptionPlanName);
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Twitch Resubscription Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }

        // When a prime paid subscription event is triggered, check the prime paid subscription dictionary for event before triggering the events effects
        public EventItem NewPrimePaidSubscription(IncomingEvent eve)
        {
            try
            {
                string subscriptionPlan = eve.Args["SubscriptionPlan"];
                string subscriptionPlanName = eve.Args["SubscriptionPlanName"];
                if (primePaidSubscriptions.ContainsKey(subscriptionPlan))
                {
                    EventItem item = primePaidSubscriptions[subscriptionPlan];
                    item = passArgs.GetEventArgs(item, eve);
                    if (item != null)
                    {
                        return item;
                    }
                }
                else if (primePaidSubscriptions.ContainsKey("Twitch_Prime_Paid_Subscriber_Default"))
                {
                    EventItem item = primePaidSubscriptions["Twitch_Prime_Paid_Subscriber_Default"];
                    item = passArgs.GetEventArgs(item, eve);
                    if (item != null)
                    {
                        return item;
                    }
                }
                else
                {
                    Terminal.Output("Lakea: Unrecognised Twitch Prime Paid Subscription -> " + subscriptionPlanName);
                    Logs.Instance.NewLog(LogLevel.Warning, "Unrecognised Twitch Prime Paid Subscription Event -> " + subscriptionPlanName);
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Twitch Prime Paid Subscription Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }

        // When a gifted subscription event is triggered, check the gifted subscription dictionary for event before triggering the events effects
        public EventItem NewGiftedSubscription(IncomingEvent eve)
        {
            try
            {
                string subscriptionPlan = eve.Args["SubscriptionPlan"];
                string subscriptionPlanName = eve.Args["SubscriptionPlanName"];
                if (giftedSubscriptions.ContainsKey(subscriptionPlan))
                {
                    EventItem item = giftedSubscriptions[subscriptionPlan];
                    item = passArgs.GetEventArgs(item, eve);
                    if (item != null)
                    {
                        return item;
                    }
                }
                else if (giftedSubscriptions.ContainsKey("Twitch_Gifted_Subscriber_Default"))
                {
                    EventItem item = giftedSubscriptions["Twitch_Gifted_Subscriber_Default"];
                    item = passArgs.GetEventArgs(item, eve);
                    if (item != null)
                    {
                        return item;
                    }
                }
                else
                {
                    Terminal.Output("Lakea: Unrecognised Twitch Gifted Subscription -> " + subscriptionPlanName);
                    Logs.Instance.NewLog(LogLevel.Warning, "Unrecognised Twitch Gift Subscription Event -> " + subscriptionPlanName);
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Twitch Gift Subscription Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }

        // When a gifted subscription continued event is triggered, check the gifted subscription dictionary for event before triggering the events effects
        public EventItem NewGiftedSubscriptionContinued(IncomingEvent eve)
        {
            try
            {
                string continueGiftedSubscription = eve.Args["ContinuedGiftedSubscription"];
                if (continuedGiftedSubscriptions.ContainsKey("Twitch_Gifted_Subscriber_Continued_Default"))
                {
                    EventItem item = continuedGiftedSubscriptions["Twitch_Gifted_Subscriber_Continued_Default"];
                    item = passArgs.GetEventArgs(item, eve);
                    if (item != null)
                    {
                        return item;
                    }
                }
                else
                {
                    Terminal.Output("Lakea: Unrecognised Twitch Gifted Subscription Continued -> " + continueGiftedSubscription);
                    Logs.Instance.NewLog(LogLevel.Warning, "Unrecognised Twitch Gifted Subscription Continued Event -> " + continueGiftedSubscription);
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Twitch Gifted Subscription Continued Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }

        // When a first time chatter sends a message in chat, get a random EventItem from the dictionary
        public EventItem NewFirstTimeChatter(IncomingEvent eve)
        {
            try
            {
                if(firstTimeChatters.Count > 0)
                {
                    EventItem item = firstTimeChatters.ElementAt(random.Next(0, firstTimeChatters.Count)).Value;
                    item = passArgs.GetEventArgs(item, eve);
                    if(item != null)
                    {
                        return item;
                    }
                }
                else
                {
                    Terminal.Output($"Lakea: No First Time Chatter Events -> " + eve.Args["DisplayName"]);
                    Logs.Instance.NewLog(LogLevel.Warning, "No First Time Chatter Events -> " + eve.Args["DisplayName"]);
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: First Time Chatter Error -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }
    }
}
