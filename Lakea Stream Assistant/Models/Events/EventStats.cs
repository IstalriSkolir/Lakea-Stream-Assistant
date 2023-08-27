using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events.EventAbstracts;

namespace Lakea_Stream_Assistant.Models.Events
{
    //Sealed class withs static members to keep track of event stats
    public sealed class EventStats
    {
        private static uint baseCampEventCount = 0;
        private static uint lakeaEventCount = 0;
        private static uint twitchEventCount = 0;
        private static uint totalEventCount = 0;

        public static uint BaseCampEventCount { get { return baseCampEventCount; } }
        public static uint LakeaEventCount { get { return lakeaEventCount; } }
        public static uint TwitchEventCount { get {  return twitchEventCount; } }
        public static uint TotalEventCount { get { return totalEventCount; } }

        //When a new event occurs, determine source and increment event count for the relevant counters
        public static void NewEvent(Event eve)
        {
            switch (eve.Source)
            {
                case EventSource.Base_Camp: incrementBaseCampEventCount(); break;
                case EventSource.Lakea: incrementLakeaEventCount(); break;
                case EventSource.Twitch: incrementTwitchEventCount(); break;
            }
        }

        //Increment the counter for Woodland Events
        private static void incrementBaseCampEventCount()
        {
            baseCampEventCount++;
            incrementTotalEventCount();
        }

        //Increment the counter for Lakea Events
        private static void incrementLakeaEventCount()
        {
            lakeaEventCount++;
            incrementTotalEventCount();
        }

        //Increment the counter for Twitch Events
        private static void incrementTwitchEventCount()
        {
            twitchEventCount++;
            incrementTotalEventCount();
        }

        //Increment the counter for Total Events
        private static void incrementTotalEventCount()
        {
            totalEventCount++;
        }
    }
}
