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
    public class RunEvent : WebSocketBehavior
    {
        private EventInput eventInput;
        private JSONConvertor convertor;

        public RunEvent()
        {
            eventInput = Server.EventInput;
            convertor = Server.JSONConvertor;
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            Send("LakeaWebsocket: RunEvent -> Connection Confirmed");
            Terminal.Output("Socket: Open Service -> RunEvent");
            Logs.Instance.NewLog(Enums.LogLevel.Info, "Socket Service Opened -> RunEvent");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            base.OnMessage(e);
            Send("LakeaWebsocket: RunEvent -> Message Received");
            Terminal.Output("Socket: Message Service -> RunEvent, " + e.Data);
            Logs.Instance.NewLog(Enums.LogLevel.Info, "Socket Service Message -> RunEvent, " + e.Data);
            runNewEvent(e);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);
            if (e.Reason == "")
            {
                Terminal.Output("Socket: Close Service -> RunEvent");
                Logs.Instance.NewLog(Enums.LogLevel.Info, "Socket Service Close -> RunEvent");
            }
            else
            {
                Terminal.Output("Socket: Close Service -> RunEvent, " + e.Reason);
                Logs.Instance.NewLog(Enums.LogLevel.Warning, "Socket Service Close -> RunEvent, " + e.Reason);
            }
        }

        protected override void OnError(WebSocketSharp.ErrorEventArgs e)
        {
            base.OnError(e);
            Terminal.Output("Socket: Errored Service -> RunEvent, " + e.Message);
            Logs.Instance.NewLog(Enums.LogLevel.Error, e.Message);
        }

        private void runNewEvent(MessageEventArgs args)
        {
            try
            {
                JObject json = JObject.Parse(args.Data);
                EventItem item = convertor.CreateEventItem(json);
                eventInput.NewEvent(item);
            }
            catch (Exception ex)
            {
                Terminal.Output("Socket: Error Getting Event Data -> " + ex.Message);
                Logs.Instance.NewLog(Enums.LogLevel.Error, ex.Message);
            }
        }
    }
}
