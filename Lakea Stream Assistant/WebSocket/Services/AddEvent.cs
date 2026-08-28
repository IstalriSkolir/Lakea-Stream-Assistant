using Lakea_Stream_Assistant.Models.Events.EventLists;
using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;
using Lakea_Stream_Assistant.WebSocket.Utilities;
using Newtonsoft.Json.Linq;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Lakea_Stream_Assistant.WebSocket.Services
{
    public class AddEvent : WebSocketBehavior
    {
        private JSONConvertor convertor;

        public AddEvent()
        {
            convertor = Server.JSONConvertor;
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            Send("LakeaWebsocket: AddEvent -> Connection Confirmed");
            Terminal.Output("Socket: Open Service -> AddEvent");
            Logs.Instance.NewLog(LogLevel.Info, "Socket Service Opened -> AddEvent");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            base.OnMessage(e);
            Send("LakeaWebsocket: AddEvent -> Message Received");
            Terminal.Output("Socket: Message Service -> AddEvent, " + e.Data);
            Logs.Instance.NewLog(LogLevel.Info, "Socket Service Message -> AddEvent, " +  e.Data);
            createNewEvent(e);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);
            if(e.Reason == "")
            {
                Terminal.Output("Socket: Close Service -> AddEvent");
                Logs.Instance.NewLog(LogLevel.Info, "Socket Service Close -> AddEvent");
            }
            else
            {
                Terminal.Output("Socket: Close Service -> AddEvent, " + e.Reason);
                Logs.Instance.NewLog(LogLevel.Warning, "Socket Service Close -> AddEvent, " + e.Reason);
            }
        }

        protected override void OnError(WebSocketSharp.ErrorEventArgs e)
        {
            base.OnError(e);
            Terminal.Output("Socket: Errored Service -> AddEvent, " + e.Message);
            Logs.Instance.NewLog(LogLevel.Error, e.Message);
        }

        private void createNewEvent(MessageEventArgs args)
        {
            try
            {
                JObject json = JObject.Parse(args.Data);
                string key = (string)json["Key"];
                EventItem item = convertor.CreateEventItem(json);
                StreamAssistant.EventHandler.UpdateEventDictionaries(key, item);
            }
            catch (Exception ex)
            {
                Terminal.Output("Socket: Error Getting Event Data -> " + ex.Message);
                Logs.Instance.NewLog(LogLevel.Error, ex.Message);
            }
        }
    }
}
