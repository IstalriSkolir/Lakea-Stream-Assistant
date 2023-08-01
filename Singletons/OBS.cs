using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Types;
using Lakea_Stream_Assistant.Models.OBS;

namespace Lakea_Stream_Assistant.Singletons
{
    public sealed class OBS
    {
        public static bool Initiliased = false;
        private static Config config;
        private static OBSResources resources;
        private static OBSWebsocket client;
        private static CancellationTokenSource keepAliveTokenSource;
        private static readonly int keepAliveInterval = 500;

        #region Initiliase

        public static void Init(Config newConfig)
        {
            try
            {
                Console.WriteLine("OBS: Connecting...");
                config = newConfig;
                client = new OBSWebsocket();
                client.Connected += onConnect;
                client.ConnectAsync("ws://" + config.OBS.IP + ":" + config.OBS.Port, config.OBS.Password);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to connect to OBS: " + ex.Message);
                Console.ReadLine();
                Environment.Exit(1);
            }
        }

        private static void onConnect(object sender, EventArgs e)
        {
            Console.WriteLine("OBS: Connected");
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

        private static void getResources()
        {
            try
            {
                Console.WriteLine("OBS: Fetching Scenes...");
                var scenes = client.ListScenes();
                List<string> sceneList = new List<string>();
                foreach (var scene in scenes)
                {
                    sceneList.Add(scene.Name);
                }
                Console.WriteLine("OBS: Fetching Sources...");
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
                Console.WriteLine("OBS: Initialising Resources...");
                resources = new OBSResources(sceneList, sourceDict);
            }
            catch (Exception ex)
            {
                Console.WriteLine("OBS: Failed to Get Resources -> " + ex.Message);
                Console.ReadLine();
                Environment.Exit(1);
            }
        }

        #endregion

        public static string GetCurrentScene()
        {
            try
            {
                return client.GetCurrentProgramScene();
            }
            catch (Exception ex)
            {
                Console.WriteLine("OBS: Failed to Get Current Scene -> " + ex.Message);
            }
            return string.Empty;
        }

        public static void ChangeScene(string scene)
        {
            try
            {
                Console.WriteLine("OBS: Changing scene -> " + scene);
                client.SetCurrentProgramScene(scene);
            }
            catch (Exception ex)
            {
                Console.WriteLine("OBS: Failed to Change Scenes -> " + ex.Message);
            }
        }

        public static void SetSourceEnabled(string scene, string source, bool active)
        {
            try
            {
                Console.WriteLine("OBS: Setting Source Enabled '" + active + "' -> '" + source + "' in '" + scene + "'");
                int sourceID = resources.GetSourceId(source);
                client.SetSceneItemEnabled(scene, sourceID, active);
            }
            catch (Exception ex)
            {
                Console.WriteLine("OBS: Failed to Set Source Enabled -> " + ex.Message);
            }
        }
        public static void SetSourceEnabled(string source, bool active)
        {
            try
            {
                string curScene = client.GetCurrentProgramScene();
                Console.WriteLine("OBS: Setting Source Enabled '" + active + "' -> '" + source + "' in '" + curScene + "'");
                var sceneSources = client.GetSceneItemList(curScene);
                //bool found = searchForSource(curScene);
                //foreach(var scenesource in sceneSources)
                //{
                //    if(scenesource.SourceName == source)
                //    {
                //        found = true;
                //        break;
                //    }
                //}
                int sourceID = resources.GetSourceId(source);
                //client.SetSceneItemEnabled(curScene, sourceID, active);
            }
            catch (Exception ex)
            {
                Console.WriteLine("OBS: Failed to Set Source Enabled -> " + ex.Message);
            }
        }

        private bool searchForSource(SceneItemDetails scene)
        {
            return false;
        }
    }
}
