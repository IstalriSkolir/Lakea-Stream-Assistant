namespace Lakea_Stream_Assistant.Models.Events.EventItems
{
    public class Callbacks
    {
        private string callbackID;
        private int delay;

        public Callbacks(ConfigEventEventTargetCallback callback)
        {
            this.callbackID = callback.EventID;
            this.delay = callback.Delay;
        }

        public string CallbackID { get { return callbackID; } }
        public int Delay { get { return delay; } }
    }
}
