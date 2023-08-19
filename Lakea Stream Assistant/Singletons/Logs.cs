using Lakea_Stream_Assistant.Enums;
using System.IO;

namespace Lakea_Stream_Assistant.Singletons
{
    public sealed class Logs
    {
        private static Logs instance = null;
        private static readonly object padlock = new object();
        private static LogLevel logLevel = LogLevel.Warning;
        private static string currentFilePath;
        private static bool firstLog;

        Logs() {}

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

        public void Initiliase(Config config)
        {
            try
            {
                EnumConverter enums = new EnumConverter();
                logLevel = enums.ConvertLogLevelString(config.Settings.LogLevel);
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

        private static string getCurrentFilePath()
        {
            string file = "Log [" + DateTime.Now.ToString("MM/dd/yyyy") + "].txt";
            file = file.Replace("/", "-");
            string path = Environment.CurrentDirectory + "\\Logs\\" + file;
            return path;
        }

        #endregion

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
                    string log = DateTime.Now.ToString() + ": Log Level -> " + level + ", " + ex.Message + ", Stack Track - " + ex.StackTrace;
                    using (StreamWriter writer = new StreamWriter(currentFilePath, true))
                    {
                        writer.WriteLine(log);
                    }
                }
            }
            catch(Exception newEx)
            {
                Console.WriteLine("Lakea: Error Writing Log to File -> " + newEx.Message);
            }
        }

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
                    string log = DateTime.Now.ToString() + ": Log Level -> " + level + ", " + message;
                    using (StreamWriter writer = new StreamWriter(currentFilePath, true))
                    {
                        writer.WriteLine(log);
                    }
                }
            }
            catch (Exception newEx)
            {
                Console.WriteLine("Lakea: Error Writing Log to File -> " + newEx.Message);
            }
        }

        private static void firstLogMade()
        {
            firstLog = false;
            if (!Directory.Exists(Environment.CurrentDirectory + "\\Logs"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + "\\Logs\\");
                File.Create(currentFilePath);
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
                writer.WriteLine("--------------------------------------------------------------------------------");
                writer.WriteLine("\nNew Session Log - " + DateTime.Now.ToString("HH:mm:ss") + "\nLog Level: " + logLevel + "\n");
                writer.WriteLine("--------------------------------------------------------------------------------");
            }
        }
    }
}
