namespace Lakea_Stream_Assistant.Models.Events.EventItems
{
    public class Callbacks
    {
        private string id;
        private int delay;

        public Callbacks(ConfigEventEventTargetCallback callback)
        {
            this.id = callback.EventID;
            this.delay = callback.Delay;
        }

        public string ID { get { return id; } }
        public int Delay { get { return delay; } }
    }
}
