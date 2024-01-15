using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Configuration;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Processes;
using Lakea_Stream_Assistant.EventProcessing.Processing;
using Lakea_Stream_Assistant.EventProcessing.Commands;
using Lakea_Stream_Assistant.Models.Tokens;
using Lakea_Stream_Assistant.Static;
using Lakea_Stream_Assistant.Models.Events.EventLists;

namespace Lakea_Stream_Assistant
{

    //Start Object
    class Program
    {
        static KeepAliveToken keepAliveToken;
        static EventInput eventHandler;
        static ExternalProcesses externalProcesses;

        //Point of Entry
        static void Main(string[] args)
        {
            Logs.Instance.Initiliase();
            initiliase(args);
            //Pauses main thread to prevent application terminating
            while (keepAliveToken.IsAlive)
            {
                Thread.Sleep(1000);
            }
            shutdown();
        }

        #region Initiliase

        //initiliase Lakea
        static void initiliase(string[] args = null)
        {
            Config config;
            string filePath;
            if(args == null || args.Length == 0)
            {
                filePath = selectProfile();
            }
            else
            {
                filePath = Environment.CurrentDirectory + "\\Configurations\\" + args[0];
            }
            config = new LoadConfig().LoadConfigFromFile(filePath);
            if (config != null)
            {
                Terminal.Output("Lakea is waking up...");
                Terminal.StartTerminalThread();
                Logs.Instance.SetErrorLogLevel(config.Settings.LogLevel);
                Logs.Instance.NewLog(LogLevel.Info, "Configuration file loaded -> " + Path.GetFileName(filePath));
                Terminal.UpdateLogLevel(config.Settings.LogLevel);
                Terminal.UpdateRefreshRate(config.Settings.TerminalRefreshRate);
                keepAliveToken = new KeepAliveToken();
                externalProcesses = new ExternalProcesses(config.Applications);
                DefaultCommands lakeaCommands = new DefaultCommands(config.Settings.Commands, externalProcesses, keepAliveToken);
                eventHandler = new EventInput(config, lakeaCommands);
                externalProcesses.StartAllExternalProcesses();
                OBS.Init(eventHandler, config.OBS.IP, config.OBS.Port, config.OBS.Password);
                Twitch.Init(config, eventHandler, lakeaCommands);
                eventHandler.NewEvent(new LakeaTimer(EventSource.Lakea, EventType.Lakea_Timer_Start));
                eventHandler.NewEvent(new EventItem(EventSource.Lakea, EventType.Lakea_Start_Up, EventTarget.Null, EventGoal.Null, "Lakea Start Up"));
                Terminal.Output("Lakea: All set and ready to go!");
                Logs.Instance.NewLog(LogLevel.Info, "Lakeas all set and ready to go!");
            }
            else
            {
                Console.Clear();
                Terminal.Output("Error loading configuration, check error log for details");
                Console.WriteLine("Error loading configuration, check error log for details\n\nPress Enter to continue");
                Console.ReadLine();
                Console.Clear();
                initiliase();
            }
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
                Console.WriteLine("\nStarting Terminal...");
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
            Terminal.EndRefresh();
        }

        #endregion
    }
}