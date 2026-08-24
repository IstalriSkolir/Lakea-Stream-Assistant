using Lakea_Stream_Assistant.Models.Misc;

namespace Lakea_Stream_Assistant.EventProcessing.Misc
{
    public class TwitchUserManager
    {
        private JSONIO json;
        private string userDirectory;
        public TwitchUserManager(string resourcePath)
        {
            userDirectory = $"{resourcePath}\\TwitchUsers\\";
            json = new JSONIO();
        }

        public void UserRedeemedPoints(Dictionary<string, string> data)
        {
            string userFile = $"{userDirectory}\\{data["AccountID"]}.json";
            if (!File.Exists(userFile))
            {
                createNewUserFile(data, userFile);
                return;
            }
            TwitchUser user = json.ReadJSONFile<TwitchUser>(userFile);
            int index = int.MinValue;
            string redeemID = data["RedeemID"];
            for(int x = 0; x < user.ChannelPoints.Redeems.Length; x++)
            {
                if (redeemID.Equals(user.ChannelPoints.Redeems[x].RedeemID))
                {
                    index = x;
                    break;
                }
            }
            if (index != int.MinValue)
            {
                user.ChannelPoints.Redeems[index].RedemptionCount++;
                user.ChannelPoints.Redeems[index].RedemptionTotalSpent += Convert.ToInt64(data["RedeemCost"]);
            }
            else
            {
                Redeem redeem = new Redeem();
                redeem.RedeemID = redeemID;
                redeem.RedeemName = data["RedeemTitle"];
                redeem.RedemptionCount = 1;
                redeem.RedemptionTotalSpent = Convert.ToInt64(data["RedeemCost"]);
                user.ChannelPoints.Redeems = user.ChannelPoints.Redeems.Concat([redeem]).ToArray();
            }
            user.ChannelPoints.TotalSpent += Convert.ToInt64(data["RedeemCost"]);
            json.WriteJSONFile(userFile, user);
        }

        private void createNewUserFile(Dictionary<string, string> data, string userFile)
        {
            TwitchUser user = new TwitchUser();
            user.TwitchID = Convert.ToInt32(data["AccountID"]);
            user.TwitchUsername = data["DisplayName"];
            user.ChannelPoints = new Channelpoints();
            user.WatchStreak = new Watchstreak();
            user.ChannelPoints.TotalSpent = data.ContainsKey("RedeemCost") ? Convert.ToInt64(data["RedeemCost"]) : 0;
            user.ChannelPoints.Redeems = data.ContainsKey("RedeemID") ? new Redeem[1] : new Redeem[0];
            if (data.ContainsKey("RedeemID"))
            {
                user.ChannelPoints.Redeems[0] = new Redeem();
                user.ChannelPoints.Redeems[0].RedeemID = data["RedeemID"];
                user.ChannelPoints.Redeems[0].RedeemName = data["RedeemTitle"];
                user.ChannelPoints.Redeems[0].RedemptionCount = 1;
                user.ChannelPoints.Redeems[0].RedemptionTotalSpent = Convert.ToInt64(data["RedeemCost"]);
            }
            json.WriteJSONFile(userFile, user);
        }
    }
}
