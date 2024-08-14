using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;
using TwitchLib.Client.Events;

namespace Lakea_Stream_Assistant.EventProcessing.Misc
{
    // Class for detecting scam messages from public chats
    public class ScamMessageDetector
    {
        private ScamActionMode actionMode = ScamActionMode.Nothing;
        private decimal actionThreshold;
        
        private Dictionary<string, decimal> keyWordValues;
        private List<string> keyWords;
        private List<string> bannedPhrases;
        private decimal firstTimeMessageMultiplier;
        private decimal hasLinkMultiplier;


        // Class Constructor
        public ScamMessageDetector(SettingsScamMessageDetection settings)
        {
            try
            {
                actionMode = new EnumConverter().ConvertScamActionModeString(settings.ActionMode);
            }
            catch(Exception ex)
            {
                Terminal.Output("Scam Detector: Error Intialising -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            actionThreshold = settings.ActionThreshold;
            firstTimeMessageMultiplier = settings.Multipliers.FirstTimeMessage;
            hasLinkMultiplier = settings.Multipliers.HasLink;
            bannedPhrases = new List<string>();
            keyWordValues = new Dictionary<string, decimal>();
            keyWords = new List<string>();
            foreach(SettingsScamMessageDetectionKeyWord key in settings.KeyWords)
            {
                string newWord = key.Word.ToLower();
                keyWordValues.Add(newWord, key.Score);
                keyWords.Add(newWord);
            }
            foreach(string phrase in settings.BannedPhrases.Phrase)
            {
                bannedPhrases.Add(phrase.ToLower());
            }
        }

        // Check if a incoming message is from a mod and evaluate for potential scam message, check action level if potential scam is found
        public void CheckChatMessage(OnMessageReceivedArgs args)
        {
            if (!args.ChatMessage.IsModerator && !args.ChatMessage.IsBroadcaster)
            {
                Tuple<bool, string> eval = checkMessageRisk(args);
                if (eval.Item1)
                {
                    checkActionLevel(args, eval.Item2);
                }
            }
        }

        // Check for banned phrases and evaluate message for potential scam
        private Tuple<bool, string> checkMessageRisk(OnMessageReceivedArgs args)
        {
            // Convert message to lower case
            string message = args.ChatMessage.Message.ToLower();
            
            // Check message for banned phrases, return true if message contains banned phrase
            foreach(string phrase in bannedPhrases)
            {
                if (message.Contains(phrase))
                {
                    return new Tuple<bool, string>(true, "Banned Phrase Detected");
                }
            }
            
            // Evalute the risk of the message, check for key words and create risk value to measure against the action threshold
            decimal risk = 0;
            string[] words = message.Split(' ');
            int hits = 0;
            foreach (string word in words)
            {
                if (keyWords.Contains(word))
                {
                    risk += keyWordValues[word];
                    hits++;
                }
            }
            risk *= hits;

            // If its a chatters first message then apply first time chatter multiplier
            if(args.ChatMessage.IsFirstMessage)
            {
                risk *= firstTimeMessageMultiplier;
            }

            // If the message has a link then apply has link multiplier
            if(1 == 2)
            {
                risk *= hasLinkMultiplier;
            }
            if (risk >= actionThreshold)
            {
                return new Tuple<bool, string>(true, "High Risk Value: " + risk + ", Threshold: " + actionThreshold);
            }
            return new Tuple<bool, string>(false, "No Bot Detected");
        }

        // If action needs to be taken, check what the action mode is set to
        private void checkActionLevel(OnMessageReceivedArgs args, string reason)
        {
            Terminal.Output("Scam Detector: Scam Message Detected -> " + args.ChatMessage.DisplayName + ", " + reason);
            Logs.Instance.NewLog(LogLevel.Warning, "Scam Message Detected -> " + args.ChatMessage + ", " + reason);
            switch (actionMode)
            {
                case ScamActionMode.Nothing:
                    break;
                case ScamActionMode.SendChatMessage:
                    Twitch.ReplyToChatMessage(args.ChatMessage.Id, "Well this looks like a bot message...");
                    break;
                case ScamActionMode.DeleteMessage:
                    Twitch.DeleteChatMessage(args.ChatMessage.Id);
                    break;
                case ScamActionMode.BanUser:
                    Twitch.BanChatUser(args.ChatMessage.UserId, "Suspected Bot Account");
                    break;
                default:
                    Terminal.Output("Scam Detector: Unrecognised Action Mode -> " + actionMode.ToString());
                    Logs.Instance.NewLog(LogLevel.Warning, "Unrecognised Action Mode -> " + actionMode.ToString());
                    break;
            }
        }
    }
}
