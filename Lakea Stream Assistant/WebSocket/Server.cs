using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.EventProcessing.Processing;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;
using Lakea_Stream_Assistant.WebSocket.Services;
using Lakea_Stream_Assistant.WebSocket.Utilities;
using WebSocketSharp.Server;

namespace Lakea_Stream_Assistant.WebSocket
{
    // Singleton Class for handling the websocket server
    public sealed class Server
    {
        private static Server instance = null;
        private static readonly object padlock = new object();
        private static WebSocketServer server;
        private static EventInput eventHandler;
        private static JSONConvertor convertor;
        private static bool enabled;
        private static string ip;
        private static int port;

        public static bool Enabled { get { return enabled; } }
        public static EventInput EventInput { get { return eventHandler; } }
        public static JSONConvertor JSONConvertor { get { return convertor; } }

        // Set class up for thread safe singleton instance
        Server(){}
        public static Server Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Server();
                    }
                    return instance;
                }
            }
        }

        // initialise the websocket server
        public static async void Initialise(Config config, EventInput newEventHandler)
        {
            enabled = config.Settings.WebSocket.Enabled;
            if(!enabled)
            {
                return;
            }
            Terminal.Output("Socket: Server Initialising...");
            Logs.Instance.NewLog(LogLevel.Info, "Initialising Websocket Server...");
            ip = config.Settings.WebSocket.IP;
            port = config.Settings.WebSocket.Port;
            eventHandler = newEventHandler;
            convertor = new JSONConvertor();
            server = new WebSocketServer("ws://" + ip + ":" + port);
            server.AddWebSocketService<AddEvent>("/AddEvent");
            server.AddWebSocketService<RunEvent>("/RunEvent");
            server.AddWebSocketService<RemoveEvent>("/RemoveEvent");
            server.AddWebSocketService<CreateChannelRedeem>("/CreateChannelRedeem");
            server.AddWebSocketService<DeleteChannelRedeem>("/DeleteChannelRedeem");
            server.AddWebSocketService<UpdateChannelRedeem>("/UpdateChannelRedeem");
            server.Start();
        }

        // Shutdown the websocket server
        public static void Shutdown()
        {
            server.Stop();
        }
    }
}
