using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;
using System.Xml.Serialization;

namespace Lakea_Stream_Assistant.EventProcessing.Commands
{
    public class QuoteCommand : CommandBase
    {
        private List<string> quotes;
        private bool initiliased = false;
        private bool cooldown = false;
        private string filePath;
        private int quoteCooldown;

        public bool Initilaised { get { return initiliased; } }

        public QuoteCommand(string resourcePath, int newQuoteCooldown)
        {
            quoteCooldown = newQuoteCooldown;
            quotes = initiliaseQuotes(resourcePath);
        }

        public override EventItem Run(IncomingEvent eve)
        {
            string command = eve.Args["CommandText"];
            Terminal.Output($"Lakea: Quote Command -> {command}");
            Logs.Instance.NewLog(LogLevel.Info, $"Quote Command -> {command}");
            Dictionary<string, string> args = newQuoteCommand(eve);
            if (args != null)
            {
                if ("quotefest".Equals(command))
                {
                    return new EventItem(eve.Source, EventType.Lakea_Command, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message_List, "Quote Command", "Lakea_Quote_Command", args: args);
                }
                else
                {
                    return new EventItem(eve.Source, EventType.Lakea_Command, EventTarget.Twitch, EventGoal.Twitch_Send_Chat_Message, "Quote Command", "Lakea_Quote_Command", args: args);
                }
            }
            return null;
        }

        private Dictionary<string, string> newQuoteCommand(IncomingEvent eve)
        {
            try
            {
                if (!cooldown)
                {
                    string commandText = eve.Args["CommandText"].ToLower();
                    string argumentsAsString = eve.Args["ArgumentsAsString"];
                    Dictionary<string, string> quote = new Dictionary<string, string>();
                    switch (commandText)
                    {
                        case "quotecount":
                            quote.Add($"Message", $"We have {quotes.Count} quotes stored! Some of these make me wonder why mistakes I made to end up here...");
                            break;
                        case "addquote":
                        case "quoteadd":
                            addquote(argumentsAsString);
                            quote.Add("Message", $"Quote added! We now have {quotes.Count} quotes!");
                            break;
                        case "quote":
                            string quoteString = getQuote(eve);
                            quote.Add("Message", quoteString);
                            break;
                        case "quotefest":
                            quote = getQuoteFest();
                            break;

                    }
                    cooldown = true;
                    Task.Delay(1000 * quoteCooldown).ContinueWith(t => { cooldown = false; });
                    return quote;
                }
                else
                    return new Dictionary<string, string> { { "Message", "The quotes are currently on cooldown!" } };
            }
            catch (Exception ex)
            {
                Terminal.Output($"Lakea: Quote Command Error -> {ex.Message}");
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }

        private void addquote(string quote)
        {
            quotes.Add(quote);
            saveQuotesToFile();
        }

        private string getQuote(IncomingEvent eve)
        {
            if (!eve.Args.ContainsKey("CommandArg1"))
            {
                int index = Dice.Roll(quotes.Count);
                return "Here's a random quote, '" + quotes[index] + "'";
            }
            else
            {
                try
                {
                    int index = int.Parse(eve.Args["CommandArg1"]) - 1;
                    if(index >= 0 && index < quotes.Count) return $"Mmm, so that quote is '{quotes[index]}'";
                    else if(index < 0) return "How the heck am I meant to get you a negative quote? Learn to count, you twit!";
                    else return $"{(index + 1)} isn't a quote I can get you, we only have {quotes.Count} quotes!";
                }
                catch (Exception ex)
                {
                    Logs.Instance.NewLog(LogLevel.Warning, $"Failed to Parse '{eve.Args["CommandArg1"]}' for Quote Index");
                    int index = Dice.Roll(quotes.Count);
                    return $"Couldn't figure out which quote you wanted so heres a random one instead, '{quotes[index]}'";
                }
            }
        }

        private Dictionary<string, string> getQuoteFest()
        {
            Dictionary<string, string> messages = new Dictionary<string, string>();
            if (quotes.Count > 5)
            {
                int quoteAmount = Dice.Roll(3, 6);
                if (quoteAmount > quotes.Count)
                    quoteAmount = quotes.Count;
                List<string> quotesToSend = new List<string>();
                List<int> indexes = new List<int>();
                
                
                while(indexes.Count < quoteAmount)
                {
                    int index = Dice.Roll(quotes.Count);
                    if (!indexes.Contains(index))
                        indexes.Add(index);
                }
                foreach (int index in indexes)
                    quotesToSend.Add(quotes[index]);
                
               
                messages.Add("Message0", "Are we ready for a quotefest? Lets go!");
                for(int i = 1; i <= quotesToSend.Count; i++)
                {
                    messages.Add("Message" + i, quotesToSend[i - 1]);
                }
            }
            else
            {
                messages.Add("Message1", "We don't have enough quotes yet! We need at least 5 quotes before we can have a quotefest!");
            }
            return messages;
        }

        #region IO Functions

        private List<string> initiliaseQuotes(string resourcePath)
        {
            try
            {
                Terminal.Output("Lakea: Loading Quotes File...");
                Logs.Instance.NewLog(LogLevel.Info, "Loading Quotes from File...");
                resourcePath = resourcePath.ToLower();
                if(resourcePath == null || resourcePath == string.Empty || resourcePath.Equals("default"))
                {
                    filePath = Environment.CurrentDirectory + "\\Resources\\Quotes.xml";
                }
                else
                {
                    filePath = resourcePath + "\\Quotes.xml";
                }
                if(File.Exists(filePath))
                {
                    List<string> list = loadQuotesFromFile(filePath);
                    if(list != null)
                    {
                        initiliased = true;
                        return list;
                    }
                }
                else
                {
                    initiliased = true;
                    return new List<string>();
                }
            }
            catch (Exception ex)             
            {
                Terminal.Output($"Lakea: Error Initialsing Quotes -> {ex.Message}");
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }

        private List<string> loadQuotesFromFile(string filePath)
        {
            try
            {
                List<string> quotes = new List<string>();
                XmlSerializer seriliaser = new XmlSerializer(quotes.GetType());
                TextReader reader = new StreamReader(filePath);
                quotes = (List<string>)seriliaser.Deserialize(reader);
                reader.Close();
                return quotes;
            }
            catch (Exception ex)
            {
                Terminal.Output($"Lakea: Error Loading Quotes from File -> {ex.Message}");
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return null;
        }

        private void saveQuotesToFile()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(quotes.GetType());
                TextWriter writer = new StreamWriter(filePath);
                serializer.Serialize(writer, quotes);
                writer.Close();
            }
            catch (Exception ex)
            {
                Terminal.Output($"Lakea: Error Saving Quotes to File -> {ex.Message}");
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        #endregion
    }
}
