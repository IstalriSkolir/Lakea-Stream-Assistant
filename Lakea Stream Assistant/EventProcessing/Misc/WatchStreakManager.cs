using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;
using System.Xml.Serialization;

namespace Lakea_Stream_Assistant.EventProcessing.Misc
{
    public class WatchStreakManager
    {
        private bool initialised = false;
        private string filePath;
        private int watchStreakEventMultiple;
        private DateOnly currentDate;
        private DateOnly previousDate;
        private List<string> sessionList;
        private Dictionary<string, WatchStreakDataStreak> watchStreaks;
        const string dateFormat = "dd-MM-yyyy";

        public WatchStreakManager(string resourcePath, int watchStreakTrigger)
        {
            currentDate = DateOnly.FromDateTime(DateTime.Now);
            sessionList = new List<string>();
            watchStreakEventMultiple = watchStreakTrigger;
            Tuple<DateOnly, Dictionary<string, WatchStreakDataStreak>> data = initialise(resourcePath);
            if (data != null)
            {
                previousDate = data.Item1;
                watchStreaks = data.Item2;
                saveWatchStreaksToFile(filePath, currentDate, watchStreaks);
                initialised = true;
            }
        }

        public WatchStreakDataStreak GetWatchStreak(string accountID)
        {
            if (watchStreaks.ContainsKey(accountID))
                return watchStreaks[accountID];
            else
            {
                WatchStreakDataStreak data = new WatchStreakDataStreak();
                data.TwitchID = accountID;
                data.CurrentStreak = 1;
                data.LastSeen = currentDate;
                data.LastSeenString = currentDate.ToString(dateFormat);
                return data;
            }
        }

        public void checkForWatchStreak(string accountID, string displayName)
        {
            if (initialised && !sessionList.Contains(accountID))
            {
                sessionList.Add(accountID);
                if (!watchStreaks.ContainsKey(accountID))
                {
                    WatchStreakDataStreak data = new WatchStreakDataStreak();
                    data.TwitchID = accountID;
                    data.CurrentStreak = 1;
                    data.LastSeen = currentDate;
                    data.LastSeenString = currentDate.ToString(dateFormat);
                    watchStreaks.Add(accountID, data);
                }
                else
                {
                    WatchStreakDataStreak data = watchStreaks[accountID];
                    if (!previousDate.Equals(data.LastSeen))
                    {
                        data.CurrentStreak = 1;
                    }
                    else
                    {
                        data.CurrentStreak++;
                        if (data.CurrentStreak % watchStreakEventMultiple == 0)
                        {
                            int nextStreakGoal = data.CurrentStreak + watchStreakEventMultiple;
                            Dictionary<string, string> args = new Dictionary<string, string>()
                            {
                                { "AccountID", accountID },
                                { "DisplayName", displayName },
                                { "WatchStreak", data.CurrentStreak.ToString() },
                                { "NextStreakGoal", nextStreakGoal.ToString() }
                            };
                            Task.Run(() => { StreamAssistant.EventHandler.NewEvent(new IncomingEvent(EventSource.Twitch, EventType.Twitch_Watch_Streak, args)); });                         
                        }
                    }
                    data.LastSeen = currentDate;
                    data.LastSeenString = currentDate.ToString(dateFormat);
                    watchStreaks[accountID] = data;
                }
                saveWatchStreaksToFile(filePath, currentDate, watchStreaks);
            }
        }

        #region IO Functions

        private Tuple<DateOnly, Dictionary<string, WatchStreakDataStreak>> initialise(string resourcePath)
        {
            try
            {
                Terminal.Output("Lakea: Loading Watch Streaks File...");
                Logs.Instance.NewLog(LogLevel.Info, "Loading Watch Streaks from File...");
                resourcePath = resourcePath.ToLower();
                if (resourcePath == null || resourcePath == string.Empty || resourcePath.Equals("default"))
                {
                    filePath = Environment.CurrentDirectory + "\\Resources\\WatchStreaks.xml";
                }
                else
                {
                    filePath = resourcePath + "\\WatchStreaks.xml";
                }
                if (File.Exists(filePath))
                {
                    WatchStreakData data = loadWatchStreaksFromFile(filePath);
                    if (data != null)
                    {
                        Dictionary<string, WatchStreakDataStreak> streakData = new Dictionary<string, WatchStreakDataStreak>();
                        DateOnly lastStreamDate = DateOnly.Parse(data.LastStreamDate);
                        foreach (WatchStreakDataStreak streak in data.Streaks)
                        {
                            streak.LastSeen = DateOnly.Parse(streak.LastSeenString);
                            streakData.Add(streak.TwitchID, streak);
                        }
                        return new Tuple<DateOnly, Dictionary<string, WatchStreakDataStreak>>(lastStreamDate, streakData);
                    }
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Error Initialsing Watch Streaks -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }

        private WatchStreakData loadWatchStreaksFromFile(string filePath)
        {
            try
            {
                WatchStreakData data = new WatchStreakData();
                XmlSerializer seriliaser = new XmlSerializer(data.GetType());
                TextReader reader = new StreamReader(filePath);
                data = (WatchStreakData)seriliaser.Deserialize(reader);
                reader.Close();
                return data;
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Error Loading Quotes from File -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }

        private void saveWatchStreaksToFile(string filePath, DateOnly date, Dictionary<string, WatchStreakDataStreak> streaks)
        {
            try
            {
                WatchStreakData data = new WatchStreakData();
                data.LastStreamDate = date.ToString(dateFormat);
                data.Streaks = new WatchStreakDataStreak[streaks.Count];
                int index = 0;
                foreach(KeyValuePair<string, WatchStreakDataStreak> pair in streaks)
                {
                    data.Streaks[index] = pair.Value;
                    //data.Streaks[index].LastSeen = null;
                    index++;
                }
                XmlSerializer serializer = new XmlSerializer(data.GetType());
                TextWriter writer = new StreamWriter(filePath);
                serializer.Serialize(writer, data);
                writer.Close();
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Error Saving Watch Streaks to File -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        #endregion
    }
}
