using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;
using System.Xml.Serialization;

namespace Lakea_Stream_Assistant.Models.Configuration
{
    //Class for loading and deserialising Config.xml, config file location is currently hardcoded
    public class LoadConfig
    {
        public Config LoadConfigFromFile(string filePath)
        {
            try
            {
                Terminal.Output("Lakea: Loading Configuration File...");
                Console.WriteLine("Lakea: Loading Configuration File...");
                Config config = new Config();
                XmlSerializer serializer = new XmlSerializer(config.GetType());
                TextReader reader = new StreamReader(filePath);
                config = (Config)serializer.Deserialize(reader);
                reader.Close();
                bool validConfig = new ValidateConfig().ValidateConfiguration(config);
                if(validConfig)
                {
                    return config;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                string fileName = Path.GetFileName(filePath);
                Logs.Instance.NewLog(LogLevel.Error, ex);
                return null;
            }
        }
    }
}
