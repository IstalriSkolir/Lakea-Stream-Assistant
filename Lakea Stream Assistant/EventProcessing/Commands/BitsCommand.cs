using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;
using System.Xml.Serialization;

namespace Lakea_Stream_Assistant.EventProcessing.Commands
{
    public class BitsCommand
    {
        private string filePath;
        private Dictionary<string, int> totalBits;

        public BitsCommand(string resourcePath)
        { 
            totalBits = InitialiseBits(resourcePath);
        }

        public Dictionary<string, string> NewTotalBitsCommand(LakeaCommand command)
        {
            int userBits = 0;
            if (totalBits.ContainsKey(command.Args.Command.ChatMessage.UserId))
            {
                userBits = totalBits[command.Args.Command.ChatMessage.UserId];
            }
            Dictionary<string, string> args = new Dictionary<string, string>()
            {
                { "Message", "@" + command.Args.Command.ChatMessage.DisplayName + " has cheered a total of " + userBits + " bits! Thank you for supporting Materies"}
            };
            return args;
        }

        public void NewBitsEvent(TwitchBits eve)
        {
            if (totalBits.ContainsKey(eve.Args.UserId))
            {
                totalBits[eve.Args.UserId] = eve.Args.TotalBitsUsed;
            }
            else
            {
                totalBits.Add(eve.Args.UserId, eve.Args.TotalBitsUsed);
            }
            saveTotalBits();
        }

        #region IO Functions

        public Dictionary<string, int> InitialiseBits(string resourcePath)
        {
            try
            {
                Terminal.Output("Lakea: Loading Bits File...");
                Logs.Instance.NewLog(LogLevel.Info, "Loading Bits from File...");
                resourcePath = resourcePath.ToLower();
                if (resourcePath == null || resourcePath == string.Empty || resourcePath.Equals("default"))
                {
                    filePath = Environment.CurrentDirectory + "\\Resources\\TotalBits.xml";
                }
                else
                {
                    filePath = resourcePath + "\\TotalBits.xml";
                }
                if (File.Exists(filePath))
                {
                    return loadTotalBits(filePath);
                }
                else
                {
                    return new Dictionary<string, int>();
                }
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Error Initialsing Total Bits -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return new Dictionary<string, int>();
        }

        private void saveTotalBits() 
        {
            try
            {
                List<Item> items= new List<Item>();
                foreach(var item in totalBits)
                {
                    items.Add(new Item(item.Key, item.Value));
                }
                XmlSerializer serializer = new XmlSerializer(items.GetType());
                TextWriter writer = new StreamWriter(filePath);
                serializer.Serialize(writer, items);
                writer.Close();
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Error Saving Total Bits to File -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        private Dictionary<string, int> loadTotalBits(string filePath)
        {
            try
            {
                List<Item> items = new List<Item>();
                XmlSerializer seriliaser = new XmlSerializer(items.GetType());
                TextReader reader = new StreamReader(filePath);
                items = (List<Item>)seriliaser.Deserialize(reader);
                reader.Close();
                Dictionary<string, int> bits = new Dictionary<string, int>();
                foreach (Item item in items)
                {
                    bits.Add(item.Key, item.Value);
                }
                return bits;
            }
            catch (Exception ex)
            {
                Terminal.Output("Lakea: Error Loading Total Bits to File -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return new Dictionary<string, int>();
        }

        #endregion
    }

    public class Item
    {
        public string Key;
        public int Value;

        public Item(string key, int value)
        {
            this.Key = key;
            this.Value = value;
        }

        public Item() { }
    }
}
