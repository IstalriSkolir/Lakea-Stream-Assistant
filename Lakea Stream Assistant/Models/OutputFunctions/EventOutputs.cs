using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events;
namespace Lakea_Stream_Assistant.Models.OutputFunctions
{
    //This class handles the outputs that are triggered from events
    public class EventOutputs
    {
        private EventInput handleEvents;
        private Random random = new Random();

        public EventOutputs(EventInput handleEvents)
        {
            this.handleEvents = handleEvents;
        }

        #region OBS Outputs

        //Set OBS source active status, resets after duration expires if there is a duration
        public void SetActiveOBSSource(string source, int duration, bool active, string callback, bool invoked = false)
        {
            Singletons.OBS.SetSourceEnabled(source, active);
            if (!invoked && duration > 0)
            {
                Task.Delay(duration * 1000).ContinueWith(t => SetActiveOBSSource(source, duration, !active, string.Empty, true));
            }
            if (callback != null && callback != string.Empty)
            {
                IDictionary<string, string> callbackArgs = new Dictionary<string, string>
                {
                    { "source", source },
                    { "duraction", duration.ToString() },
                    { "active", active.ToString() }
                };
                createCallback(callbackArgs, callback);
            }
        }

        //Set random OBS source active, resets after duration expires if there is a duration
        public void SetRandomActiveOBSSource(string[] args, int duration, bool active, string callback)
        {
            int ran = random.Next(0, args.Length);
            string source = args[ran];
            Singletons.OBS.SetSourceEnabled(source, active);
            if (duration > 0)
            {
                Task.Delay(duration * 1000).ContinueWith(t => { SetActiveOBSSource(source, duration, !active, string.Empty, true); });
            }
            if (callback != null && callback != string.Empty)
            {
                IDictionary<string, string> callbackArgs = new Dictionary<string, string>
                {
                    { "sourceName", source },
                    { "sourceNumber", ran.ToString() },
                    { "duration", duration.ToString() },
                    { "active", active.ToString() }
                };
                for (int i = 0; i < args.Length; i++)
                {
                    callbackArgs.Add("arg" + i, args[i]);
                }
                createCallback(callbackArgs, callback);
            }
        }

        //Changes OBS scene
        public void ChangeOBSScene(string scene, string callback)
        {
            Singletons.OBS.ChangeScene(scene);
            if (callback != null && callback != string.Empty)
            {
                IDictionary<string, string> callbackArgs = new Dictionary<string, string>
                {
                    { "scene", scene },
                };
                createCallback(callbackArgs, callback);
            }
        }

        #endregion

        #region Twitch Outputs

        public void SendTwitchChatMessage(string message, string callback)
        {
            Singletons.Twitch.WriteToChat(message);
            if(callback != null && callback != string.Empty)
            {
                IDictionary<string, string> callbackArgs = new Dictionary<string, string>
                {
                    { "message", message }
                };
                createCallback(callbackArgs, callback);
            }
        }

        #endregion

        //For null events that don't require any actions
        public void NullEvent(string message)
        {
            Console.WriteLine("Lakea: " + message);
        }

        //Creates a callback object with the passed arguments and reruns the New Event function
        private void createCallback(IDictionary<string, string> args, string callback)
        {
            handleEvents.NewEvent(new LakeaCallback(EventSource.Lakea, EventType.Lakea_Callback, callback, args));
        }
    }
}
