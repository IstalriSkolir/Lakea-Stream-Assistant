using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
using Lakea_Stream_Assistant.Models.Resources.Lakea;
using Lakea_Stream_Assistant.Singletons;
using System.Runtime.InteropServices;

namespace Lakea_Stream_Assistant.Static
{
    //Static class for writing to the console
    public sealed class Terminal
    {
        private static Thread refreshThread;
        private static List<string> output = new List<string>();
        private static List<string> logs = new List<string>();
        private static CurrentSystem system;
        private static string logLevel;
        private static bool outputUpdated;
        private static bool logUpdated;
        private static bool logLevelUpdated;
        private static int refreshRate;
        private static bool refreshActive;

        #region Dll Imports

        private const int MF_BYCOMMAND = 0x00000000;
        private const int SC_CLOSE = 0xF060;
        private const int SC_MINIMIZE = 0xF020;
        private const int SC_MAXIMIZE = 0xF030;
        private const int SC_SIZE = 0xF000;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        #endregion

        #region Public Functions

        //Adds new message to the terminal display list
        public static void Output(string message)
        {
            if(message.Length > 115)
            {
                message = message.Substring(0, 112) + "...";
            }
            output.Add(message);
            outputUpdated = true;
        }

        //Adds new log to display log list
        public static void Log(string message)
        {
            if(message.Length >= 115)
            {
                message = message.Substring(0, 112) + "...";
            }
            logs.Add(message);
            logUpdated = true;
        }

        //Starts the thread for the terminal, draws console and refreshes it on loop
        public static void StartTerminalThread()
        {
            //Sets these two variables no to avoid potential thread clashing later
            refreshRate = 1000;
            logLevel = "Warning";
            outputUpdated = true;
            logUpdated = true;
            logLevelUpdated = false;
            refreshThread = new Thread(initiliase);
            refreshThread.Start();
        }

        //Sets bool to let the refresh thread end
        public static void EndRefresh()
        {
            refreshActive = false;
        }

        //Update the log level display
        public static void UpdateLogLevel(string newLogLevel)
        {
            logLevelUpdated = true;
            logLevel = newLogLevel;
        }

        //Update the refresh delay of the terminal
        public static void UpdateRefreshRate(int newRefreshRate)
        {
            refreshRate = newRefreshRate * 1000;
        }

        //Resets the Terminal with a fresh thread
        public static void ResetTerminal()
        {
            Task.Run(() =>
            {
                Output("Resetting Terminal...");
                Logs.Instance.NewLog(LogLevel.Info, "Resetting Terminal...");
                EndRefresh();
                Thread.Sleep(refreshRate);
                refreshThread = new Thread(initiliase);
                refreshThread.Start();
                Output("Terminal Reset");
            });
        }

        #endregion

        #region Initiliase

        //Initiliase the static class
        private static void initiliase()
        {
            system = new CurrentSystem();
            Console.Clear();
            Console.Title = "Lakea Moonlight - Stream Assistant";
            Console.SetWindowSize(151, 40);
            Console.SetBufferSize(153, 42);
            var largestWindowX = Console.WindowWidth;
            var largestWindowY = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth = largestWindowX;
            Console.BufferHeight = Console.WindowHeight = largestWindowY;
            IntPtr handle = GetConsoleWindow();
            IntPtr sysMenu = GetSystemMenu(handle, false);
            if (handle != IntPtr.Zero)
            {
                DeleteMenu(sysMenu, SC_CLOSE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_MINIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);
            }
            Console.CursorVisible = false;
            refreshActive = true;
            drawBoxes();
            drawDetails();
            drawSystemInfo();
            writeText("LogLevel: Warning", 34, 24);
            terminalRefresh();
        }

        //Draw the box areas to the console
        private static void drawBoxes()
        {
            Draw.RectangleFromTop(32, 38, 0, 0, ConsoleColor.Blue);
            Draw.RectangleFromTop(118, 6, 32, 0, ConsoleColor.Blue);
            Draw.RectangleFromTop(118, 14, 32, 7, ConsoleColor.Blue);
            Draw.RectangleFromTop(118, 16, 32, 22, ConsoleColor.Blue);
        }

        //Write the static text to the console
        private static void drawDetails()
        {
            writeText("Connections:", 2, 2);
            writeText("OBS           ->", 2, 4);
            writeText("Twitch Client ->", 2, 5);
            writeText("Twitch PubSub ->", 2, 6);
            writeText("Events:", 2, 10);
            writeText("Twitch ->", 2, 12);
            writeText("OBS    ->", 2, 13);
            writeText("Lakea  ->", 2, 14);
            writeText("Camp   ->", 2, 15);
            writeText("Battle ->", 2, 16);
            writeText("Total  ->", 2, 17);
        }

        //Write System Info to the console
        private static void drawSystemInfo()
        {
            writeText("Processor: " + system.ProcessorName, 34, 2);
            writeText("Processor Cores: " + system.ProcessorCount, 34, 3);
            writeText("Processor Speed: " + system.ProcessorSpeed, 34, 4);
            writeText("System Memory: " + system.SystemMemory + "MB", 34, 5);
            writeText("CPU: " + system.GetCPUUsage(), 120, 2);
            writeText("RAM: " + system.GetRAMUsage(), 120, 3);
            writeText("of " + system.SystemMemory + "MB", 122, 4);
        }

        #endregion

        #region Update Terminal Display

        //Refresh loop for updating the console
        private static void terminalRefresh()
        {
            try
            {
                while (refreshActive)
                {
                    updateDetails();
                    updateSystemInfo();
                    updateLogLevel();
                    updateTerminalOutput();
                    updateTerminalLog();
                    Thread.Sleep(refreshRate);
                }
            }
            catch (ThreadAbortException ex)
            {
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        //Updates detail values
        private static void updateDetails()
        {
            bool obs;
            bool twitchClient;
            bool twitchPubSub;
            try { obs = OBS.IsConnected; } catch { obs = false; }
            try { twitchClient = Twitch.IsClientConnected; } catch { twitchClient = false; }
            try { twitchPubSub = Twitch.IsPubSubConnected; } catch { twitchPubSub = false; }
            if(obs) { writeText("Connected   ", 19, 4, ConsoleColor.Green); }
            else { writeText("Disconnected", 19, 4, ConsoleColor.Red); }
            if(twitchClient) { writeText("Connected   ", 19, 5, ConsoleColor.Green); }
            else { writeText("Disconnected", 19, 5, ConsoleColor.Red); }
            if(twitchPubSub) { writeText("Connected   ", 19, 6, ConsoleColor.Green); }
            else { writeText("Disconnected", 19, 6, ConsoleColor.Red); }
            writeText(EventStats.TwitchEventCount.ToString(), 12, 12, ConsoleColor.Cyan);
            writeText(EventStats.OBSEventCount.ToString(), 12, 13, ConsoleColor.Cyan);
            writeText(EventStats.LakeaEventCount.ToString(), 12, 14, ConsoleColor.Cyan);
            writeText(EventStats.BaseCampEventCount.ToString(), 12, 15, ConsoleColor.Cyan);
            writeText(EventStats.BattleSimulatorEventCount.ToString(), 12, 16, ConsoleColor.Cyan);
            writeText(EventStats.TotalEventCount.ToString(), 12, 17, ConsoleColor.Cyan);
        }

        //Updates system information values
        private static void updateSystemInfo()
        {
            writeText(system.GetCPUUsage() + "   ", 125, 2);
            writeText(system.GetRAMUsage() + "   ", 125, 3);
        }

        //Updates the terminal output display
        private static void updateTerminalOutput()
        {
            if (outputUpdated)
            {
                outputUpdated = false;
                clearArea(115, 12, 34, 9);
                if(output.Count > 12)
                {
                    while(output.Count > 12)
                    {
                        output.RemoveAt(0);
                    }
                }
                for(int index = 0; index < output.Count; index++)
                {
                    writeText(output[output.Count - (index + 1)], 34, 9 + index);
                }
            }
        }

        //Updates the log display
        private static void updateTerminalLog()
        {
            if (logUpdated)
            {
                logUpdated = false;
                clearArea(115, 12, 34, 26);
                if(logs.Count > 12)
                {
                    while(logs.Count > 12)
                    {
                        logs.RemoveAt(0);
                    }
                }
                for(int index = 0; index < logs.Count; index++)
                {
                    writeText(logs[logs.Count - (index + 1)], 34, 26 + index);
                }
            }
        }

        private static void updateLogLevel()
        {
            if (logLevelUpdated)
            {
                logLevelUpdated = false;
                writeText(logLevel + "          ", 44, 24);
            }
        }

        #endregion

        #region Misc

        //Write text to the console
        private static void writeText(string text, int xpos, int ypos, ConsoleColor colour = ConsoleColor.White)
        {
            Console.SetCursorPosition(xpos, ypos);
            Console.ForegroundColor = colour;
            Console.Write(text);
        }

        //Clear an area of the console
        private static void clearArea(int width, int height, int xpos, int ypos)
        {
            for (int y = ypos; y < ypos + height; y++)
            {
                string length = "";
                for (int x = 0; x < width; x++)
                {
                    length += " ";
                }
                Console.SetCursorPosition(xpos, y);
                Console.Write(length);
            }
        }

        #endregion
    }
}
