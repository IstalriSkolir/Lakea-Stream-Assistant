using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;

namespace Lakea_Stream_Assistant.EventProcessing.Processing
{
    // Class for checking for duplicate incoming event payloads
    public class HashChecker
    {
        // Hashset for storing current event hashes
        private HashSet<int> hashes;

        // Milliseconds before hash times out
        private const int timeOut = 150000;

        // Constructor initialises hashset
        public HashChecker()
        {
            hashes = new HashSet<int>();
        }

        // Hash incoming string and add to hashset, if it fails to add its a duplicate
        public bool CheckPayloadIsntDuplicate(string eve, string payload)
        {
            int hash = payload.GetHashCode();
            bool isSuccessful;
            lock (hashes)
            {
                isSuccessful = hashes.Add(hash);
            }
            if (isSuccessful)
            {
                Task.Delay(timeOut).ContinueWith(t => { hashTimeOut(eve, hash); });
            }
            else
            {
                Terminal.Output("Hash Checker: Duplicate Event Detected -> " + eve + ", " + payload);
                Logs.Instance.NewLog(LogLevel.Warning, "Hash Checker Duplicate Event Detected -> " + eve + ", " + payload);
            }
            return isSuccessful;
        }

        // Once a hash times out, remove from hashset
        private void hashTimeOut(string eve, int hash)
        {
            Logs.Instance.NewLog(LogLevel.Info, "Hash Checker Hash Timeout -> " + eve + ", " + hash);
            lock (hashes)
            {
                hashes.Remove(hash);
            }
        }
    }
}
