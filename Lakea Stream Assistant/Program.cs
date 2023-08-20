using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Configuration;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Processes;
using Lakea_Stream_Assistant.EventProcessing.Processing;
using Lakea_Stream_Assistant.EventProcessing.Commands;

namespace Lakea_Stream_Assistant
{

    //Start Object
    class Program
    {
        static EventInput eventHandler;
        static ExternalProcesses externalProcesses;

        //Point of Entry
        static void Main(string[] args)
        {
            initiliase();
            bool exit = false;
            //Pauses main thread to prevent application terminating, until told by the user
            while (!exit)
            {
                string input = Console.ReadLine();
                if ("exit".Equals(input.ToLower()))
                {
                    shutdown();
                    exit = true;
                }
            }
        }

        #region Initiliase

        //initiliase Lakea
        static void initiliase()
        {
            Logs.Instance.Initiliase();
            string filePath = selectProfile();
            Console.WriteLine("Lakea is waking up...");
            Config config = new LoadConfig().LoadConfigFromFile(filePath);
            Logs.Instance.SetErrorLogLevel(config.Settings.LogLevel);
            Logs.Instance.NewLog(LogLevel.Info, "Configuration file loaded -> " + Path.GetFileName(filePath));
            InternalCommands lakeaCommands = new InternalCommands(config.Settings.Commands);
            eventHandler = new EventInput(config.Events, lakeaCommands);
            externalProcesses = new ExternalProcesses(config.Applications);
            externalProcesses.StartAllExternalProcesses();
            OBS.Init(config);
            while (!OBS.Initiliased)
            {
                Thread.Sleep(100);
            }
            Twitch.Init(config, eventHandler, lakeaCommands);
            while (Twitch.ServicesConnected.Item1 < Twitch.ServicesConnected.Item2)
            {
                Thread.Sleep(100);
            }
            eventHandler.NewEvent(new LakeaTimer(EventSource.Lakea, EventType.Lakea_Timer));
            Console.WriteLine("Lakea: All set and ready to go!");
            Logs.Instance.NewLog(LogLevel.Info, "Lakeas all set and ready to go!");
        }

        //Lists avaliable config files and has the user select one
        static string selectProfile()
        {
            try
            {
                string[] files = Directory.GetFiles(Environment.CurrentDirectory + "\\Configurations\\", "*.xml");
                Console.WriteLine("Avaliable Configurations:\n");
                int length = (Environment.CurrentDirectory + "\\Configurations\\").Length;
                for (int i = 0; i < files.Length; i++)
                {
                    string fileName = Path.GetFileName(files[i]);
                    fileName = fileName.Remove(fileName.Length - 4, 4);
                    Console.WriteLine((i + 1) + ". " + fileName);
                }
                Console.Write("\nEnter the name/number of your chosen configuration files: ");
                string input = Console.ReadLine();
                string filePath = getFilePathFromInput(input, files);
                if(filePath == string.Empty)
                {
                    Console.WriteLine("\nInvalid Input: " + input + "\n\nPress Enter to continue");
                    Console.ReadLine();
                    Console.Clear();
                    selectProfile();
                }
                Console.Clear();
                return filePath;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Fatal Error Loading Configuration List -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Fatal, ex);
                Console.ReadLine();
                Environment.Exit(1);
            }
            return string.Empty;
        }

        //Get the file path of the chosen configuration file from the file list
        static string getFilePathFromInput(string input, string[] files)
        {
            if(input == null)
            {
                return string.Empty;
            }
            else if(int.TryParse(input, out int index))
            {
                index--;
                if(index >= 0 &&  index < files.Length)
                {
                    return files[index];
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                string filePath = string.Empty;
                foreach(string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    fileName = fileName.Remove(fileName.Length - 4, 4);
                    if(fileName.ToLower() == input.ToLower())
                    {
                        filePath = file;
                        break;
                    }
                }
                return filePath;       
            }
        }

        #endregion

        #region Shutdown

        static void shutdown()
        {
            externalProcesses.StopAllExternalProcesses();
        }

        #endregion
    }
}