using System.Xml.Serialization;

namespace Lakea_Stream_Assistant.Models.Configuration
{
    //Class for loading and deserialising Config.xml, config file location is currently hardcoded
    public class LoadConfig
    {
        public Config LoadConfigFromFile()
        {
            Console.WriteLine("Loading Configuration File...");
            Config config = new Config();
            XmlSerializer serializer = new XmlSerializer(config.GetType());
            TextReader reader = new StreamReader(Environment.CurrentDirectory + "\\Config.xml");
            config = (Config)serializer.Deserialize(reader);
            reader.Close();
            return config;
        }
    }
}
