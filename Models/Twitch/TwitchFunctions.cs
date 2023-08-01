using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;

namespace Lakea_Stream_Assistant.Models.Twitch
{
    public class TwitchFunctions
    {
        private IDictionary<string, TwitchEventItem> redeems = new Dictionary<string, TwitchEventItem>();

        public TwitchFunctions(ConfigEvent[] events)
        {
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

        #region Check Events

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

        #endregion

        #region Process Events

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

        #region Base Camp

        #endregion

        #region OBS

        private void eventTargetOBS(TwitchEventItem item)
        {
            bool active = false; ;
            switch (item.EventGoal)
            {
                case EventGoal.Disable_OBS_Source:
                    active = false;
                    //setEnableOBSSource(item, false);
                    break;
                case EventGoal.Enable_OBS_Source:
                    active = true;
                    //setEnableOBSSource(item, true);
                    break;
            }
            setEnableOBSSource(item, active);
            if (item.Duration > 0)
            {
                Task.Delay(item.Duration * 1000).ContinueWith(t => setEnableOBSSource(item, !active));
            }
        }

        private void setEnableOBSSource(TwitchEventItem item, bool active)
        {
            Singletons.OBS.SetSourceEnabled(item.Object, active);
        }

        #endregion

        #region Twitch

        #endregion

        #endregion
    }
}
