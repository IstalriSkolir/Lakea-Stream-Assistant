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
                Config config = new Config();
                XmlSerializer serializer = new XmlSerializer(config.GetType());
                TextReader reader = new StreamReader(filePath);
                config = (Config)serializer.Deserialize(reader);
                reader.Close();
                return config;
            }
            catch (Exception ex)
            {
                string fileName = Path.GetFileName(filePath);
                Terminal.Output("Fatal Error: Failed to Load Configuration File -> " + fileName + ", " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Fatal, ex);
                Console.ReadLine();
                Environment.Exit(1);
            }
            return null;
        }
    }
}
