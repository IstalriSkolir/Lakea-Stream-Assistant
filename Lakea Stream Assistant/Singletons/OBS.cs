using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Types;
using Lakea_Stream_Assistant.Models.OBS;
using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Static;
using OBSWebsocketDotNet.Communication;

namespace Lakea_Stream_Assistant.Singletons
{
    //Singleton that connects to OBS and manages calls via the OBS Web Socket library
    public sealed class OBS
    {
        public static bool Initiliased = false;
        private static OBSResources resources;
        private static OBSWebsocket client;
        private static CancellationTokenSource keepAliveTokenSource;
        private static readonly int keepAliveInterval = 500;
        private static string ip;
        private static int port;
        private static string password;

        public static bool IsConnected
        {
            get
            {
                if(client != null) return client.IsConnected;
                else return false;
            }
        }

        #region Initiliase

        //Initialises the connected with the passed in configuration data
        public static void Init(string newIP, int newPort, string newPassword)
        {
            try
            {
                ip = newIP;
                port = newPort;
                password = newPassword;
                Terminal.Output("OBS: Connecting...");
                Logs.Instance.NewLog(LogLevel.Info, "Connecting to OBS...");
                client = new OBSWebsocket();
                client.Connected += onConnect;
                client.Disconnected += onDisconnect;
                client.ConnectAsync("ws://" + ip + ":" + port, password);
            }
            catch (Exception ex)
            {
                Terminal.Output("Fatal Error: Failed to Connect to OBS -> " + ex.Message);
                Terminal.Output("Terminating Lakea...");
                Logs.Instance.NewLog(LogLevel.Fatal, ex);
                Thread.Sleep(5000);
                Environment.Exit(1);
            }
        }

        //Once connected, sets the keep alive token to avoid disconnected and calls for resources to be collected
        private static void onConnect(object sender, EventArgs e)
        {
            Terminal.Output("OBS: Connected");
            Logs.Instance.NewLog(LogLevel.Info, "Connected to OBS");
            keepAliveTokenSource = new CancellationTokenSource();
            CancellationToken keepAliveToken = keepAliveTokenSource.Token;
            Task statPollKeepAlive = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Thread.Sleep(keepAliveInterval);
                    if (keepAliveToken.IsCancellationRequested)
                    {
                        break;
                    }
                }
            }, keepAliveToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            getResources();
            Initiliased = true;
        }

        //If OBS disconnects, then reconnect to OBS
        private static void onDisconnect(object sender, ObsDisconnectionInfo e)
        {
            Terminal.Output("OBS: Disconnected -> " + e.DisconnectReason);
            Logs.Instance.NewLog(LogLevel.Warning, "Disconnected from OBS, " + e.DisconnectReason);
            Terminal.Output("OBS: Attempting to reconnected...");
            Init(ip, port, password);
        }

        //Once connected, gether source and scene information that can be referred back to later in the 'resources' object
        private static void getResources()
        {
            try
            {
                Terminal.Output("OBS: Fetching Scenes...");
                Logs.Instance.NewLog(LogLevel.Info, "Fetching OBS Scenes...");
                var scenes = client.ListScenes();
                List<string> sceneList = new List<string>();
                foreach (var scene in scenes)
                {
                    sceneList.Add(scene.Name);
                }
                Terminal.Output("OBS: Fetching Sources...");
                Logs.Instance.NewLog(LogLevel.Info, "Fetching OBS Sources...");
                IDictionary<string, int> sourceDict = new Dictionary<string, int>();
                foreach (var scene in scenes)
                {
                    var sceneSources = client.GetSceneItemList(scene.Name);
                    foreach (var source in sceneSources)
                    {
                        if (!sourceDict.ContainsKey(source.SourceName))
                        {
                            sourceDict.Add(source.SourceName, source.ItemId);
                        }
                    }
                }
                Terminal.Output("OBS: Fetching Transitions...");
                Logs.Instance.NewLog(LogLevel.Info, "Fetching OBS Transitions...");
                var sceneTransitions = client.GetSceneTransitionList();
                List<string> transitionNames = new List<string>();
                foreach (var transition in sceneTransitions.Transitions)
                {
                    transitionNames.Add(transition.Name);
                }
                Terminal.Output("OBS: Initialising Resources...");
                Logs.Instance.NewLog(LogLevel.Info, "Initialising OBS Resources...");
                resources = new OBSResources(sceneList, sourceDict, transitionNames);
                Terminal.Output("OBS: Initialised OBS Resources");
                Logs.Instance.NewLog(LogLevel.Info, "Initialised OBS Resources");
            }
            catch (Exception ex)
            {
                Terminal.Output("Fatal Error: Failed to Fetch OBS Resources -> " + ex.Message);
                Terminal.Output("Terminating Lakea...");
                Logs.Instance.NewLog(LogLevel.Fatal, ex);
                Thread.Sleep(5000);
                Environment.Exit(1);
            }
        }

        #endregion

        //Returns the current active scene
        public static string GetCurrentScene()
        {
            try
            {
                return client.GetCurrentProgramScene();
            }
            catch (Exception ex)
            {
                Terminal.Output("OBS: Failed to Get Current Scene -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
            return string.Empty;
        }

        //Changes the scene to the passed in scene
        public static void ChangeScene(string scene)
        {
            try
            {
                Terminal.Output("OBS: Changing Scene -> " + scene);
                Logs.Instance.NewLog(LogLevel.Info, "Changing OBS Scene -> " + scene);
                client.SetCurrentProgramScene(scene);
            }
            catch (Exception ex)
            {
                Terminal.Output("OBS: Failed to Change Scenes -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        //Changes the scene to the passed in scene with the passesd in transition
        //Todo: Client resets the default transition after 3 seconds, will cause issues for transitions that are longer than
        //3 seconds, should get transition length then reset after that period of time
        public static void ChangeScene(string scene, string transition)
        {
            try
            {
                Terminal.Output("OBS: Changing Scene with Transition -> Scene - " + scene + ", Transition - " + transition);
                Logs.Instance.NewLog(LogLevel.Info, "Changing OBS Scene with Transition -> Scene - " + scene + ", Transition - " + transition);
                string curTransition = client.GetCurrentSceneTransition().Name;
                client.SetCurrentSceneTransition(transition);
                client.SetCurrentProgramScene(scene);
                Task.Delay(3000).ContinueWith(t => { client.SetCurrentSceneTransition(curTransition); });
            }
            catch (Exception ex)
            {
                Terminal.Output("OBS: Failed to Change Scenes with Transition -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        //Disables or enables a source with a scene arguement passed in
        public static void SetSourceEnabled(string scene, string source, bool active)
        {
            try
            {
                Terminal.Output("OBS: Setting Source State '" + active + "' -> '" + source + "' in '" + scene + "'");
                Logs.Instance.NewLog(LogLevel.Info, "Setting OBS Source State '" + active + "' -> '" + source + "' in '" + scene + "'");
                int sourceID = resources.GetSourceId(source);
                client.SetSceneItemEnabled(scene, sourceID, active);
            }
            catch (Exception ex)
            {
                Terminal.Output("OBS: Failed to Set Source Enabled -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        //Disables or enables a source by searching for it in the active scene and any nested scenes
        public static void SetSourceEnabled(string source, bool active)
        {
            try
            {
                string curScene = client.GetCurrentProgramScene();
                Terminal.Output("OBS: Setting Source State '" + active + "' -> '" + source + "' in '" + curScene + "'");
                Logs.Instance.NewLog(LogLevel.Info, "Setting OBS Source State '" + active + "' -> '" + source + "' in '" + curScene + "'");
                string scene = searchForSource(curScene, source);
                int sourceID = resources.GetSourceId(source);
                client.SetSceneItemEnabled(scene, sourceID, active);
            }
            catch (Exception ex)
            {
                Terminal.Output("OBS: Failed to Set Source Enabled -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex);
            }
        }

        //Will search any nested scenes in a scene for a source
        private static string searchForSource(string scene, string source)
        {
            var sources = client.GetSceneItemList(scene);
            foreach (var item in sources)
            {
                if(item.SourceName == source)
                {
                    return scene;
                }
                else if(item.SourceType == SceneItemSourceType.OBS_SOURCE_TYPE_SCENE)
                {
                    string newScene = searchForSource(item.SourceName, source);
                    if(newScene != string.Empty)
                    {
                        return newScene;
                    }
                }
            }
            return string.Empty;
        }
    }
}
