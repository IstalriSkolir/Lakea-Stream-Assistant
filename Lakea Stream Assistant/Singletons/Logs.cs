using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Static;

namespace Lakea_Stream_Assistant.Singletons
{
    //Sealed class for logging events and errors that occur to file
    public sealed class Logs
    {
        private static Logs instance = null;
        private static readonly object padlock = new object();
        private static LogLevel logLevel = LogLevel.Warning;
        private static string currentFilePath;
        private static bool firstLog;

        Logs() {}

        //Singleton so that this instance can be accessed from anywhere
        public static Logs Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Logs();
                    }
                    return instance;
                }
            }
        }

        #region Initiliase

        //Initiliases the logging instance
        public void Initiliase()
        {
            try
            {
                currentFilePath = getCurrentFilePath();
                firstLog = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lakea: Failed to Initiliase Logs -> " + ex.Message);
                Console.ReadLine();
                Environment.Exit(1);
            }
        }

        //Sets the current file path
        private string getCurrentFilePath()
        {
            string file = "Log [" + DateTime.Now.ToString("MM/dd/yyyy") + "].txt";
            file = file.Replace("/", "-");
            string path = Environment.CurrentDirectory + "\\Logs\\" + file;
            return path;
        }

        //Sets the logging level for the session instance
        public void SetErrorLogLevel(string level)
        {
            EnumConverter enums = new EnumConverter();
            logLevel = enums.ConvertLogLevelString(level);
        }

        #endregion

        //Writes new log event to file, receieves an Exception as an argument
        public void NewLog(LogLevel level, Exception ex)
        {
            try
            {
                if (firstLog)
                {
                    firstLogMade();
                }
                if (((int)level) >= (int)logLevel)
                {
                    string log = DateTime.Now.ToString("HH:mm:ss") + ": Log Level -> " + level + ", " + ex.GetType() + ex.Message + ",\nStack Trace - " + ex.StackTrace;
                    using (StreamWriter writer = new StreamWriter(currentFilePath, true))
                    {
                        writer.WriteLine(log);
                    }
                    Terminal.Log(log);
                }
            }
            catch(Exception newEx)
            {
                Terminal.Output("Lakea: Error Writing Log to File -> " + newEx.Message);
            }
        }

        //Writes new log event to file, receieves a string as an argument
        public void NewLog(LogLevel level, string message)
        {
            try
            {
                if (firstLog)
                {
                    firstLogMade();
                }
                if (((int)level) >= (int)logLevel)
                {
                    string log = DateTime.Now.ToString("HH:mm:ss") + ": Log Level -> " + level + ", " + message;
                    using (StreamWriter writer = new StreamWriter(currentFilePath, true))
                    {
                        writer.WriteLine(log);
                    }
                    Terminal.Log(log);
                }
            }
            catch (Exception newEx)
            {
                Terminal.Output("Lakea: Error Writing Log to File -> " + newEx.Message);
            }
        }

        //Checks if its the first log of this session and writes new log info to the file
        private static void firstLogMade()
        {
            firstLog = false;
            if (!Directory.Exists(Environment.CurrentDirectory + "\\Logs"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + "\\Logs\\");
                File.Create(currentFilePath).Dispose();
            }
            else
            {
                if (!File.Exists(currentFilePath))
                {
                    File.CreateText(currentFilePath).Dispose();
                }
            }
            using (StreamWriter writer = new StreamWriter(currentFilePath, true))
            {
                writer.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------------------");
                writer.WriteLine("\nNew Session Log - " + DateTime.Now.ToString("HH:mm:ss") + "\nLog Level: " + logLevel + "\n");
                writer.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------------------");
            }
        }
    }
}
