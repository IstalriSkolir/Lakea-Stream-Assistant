using Lakea_Stream_Assistant.Models.Configuration;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Singletons;

namespace Lakea_Stream_Assistant
{
    class Program
    {
        //Point of Entry
        static void Main(string[] args)
        {
            Console.WriteLine("Twitcher Initiliasing...");
            Config config = new LoadConfig().LoadConfigFromFile();
            HandleEvents eventHandler = new HandleEvents(config.Events);
            OBS.Init(config);
            while (!OBS.Initiliased)
            {
                Thread.Sleep(100);
            }
            Twitch.Init(config, eventHandler);
            while (!Twitch.Initiliased)
            {
                Thread.Sleep(100);
            }

            //Pauses main thread to prevent application terminating
            Console.ReadLine();
        }
    }
}