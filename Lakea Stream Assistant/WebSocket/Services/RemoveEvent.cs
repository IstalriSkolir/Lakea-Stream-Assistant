using Lakea_Stream_Assistant.EventProcessing.Processing;
using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;
using Lakea_Stream_Assistant.WebSocket.Utilities;
using Newtonsoft.Json.Linq;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Lakea_Stream_Assistant.WebSocket.Services
{
    public class RemoveEvent : WebSocketBehavior
    {
        private EventInput eventInput;
        private JSONConvertor convertor;

        public RemoveEvent()
        {
            eventInput = Server.EventInput;
            convertor = Server.JSONConvertor;
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            Send("Lakea Websocket: Connection Confirmed");
            Terminal.Output("Socket: Open Service -> RemoveEvent");
            Logs.Instance.NewLog(Enums.LogLevel.Info, "Socket Service Opened -> RemoveEvent");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            base.OnMessage(e);
            try
            {
                JObject json = JObject.Parse(e.Data);
                string key = (string)json["Key"];
                EventItem item = convertor.CreateEventItem(json);
                eventInput.UpdateEventDictionaries(key, item, remove: true);
            }
            catch (Exception ex)
            {
                Terminal.Output("Socket: Error Getting Event Key -> " + ex.Message);
                Logs.Instance.NewLog(Enums.LogLevel.Error, ex.Message);
            }
        }

        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);
            if (e.Reason == "")
            {
                Terminal.Output("Socket: Close Service -> RemoveEvent");
                Logs.Instance.NewLog(Enums.LogLevel.Info, "Socket Service Close -> RemoveEvent");
            }
            else
            {
                Terminal.Output("Socket: Close Service -> RemoveEvent, " + e.Reason);
                Logs.Instance.NewLog(Enums.LogLevel.Warning, "Socket Service Close -> RemoveEvent, " + e.Reason);
            }
        }

        protected override void OnError(WebSocketSharp.ErrorEventArgs e)
        {
            base.OnError(e);
            Terminal.Output("Socket: Errored Service -> RemoveEvent, " + e.Message);
            Logs.Instance.NewLog(Enums.LogLevel.Error, e.Message);
        }
    }
}
