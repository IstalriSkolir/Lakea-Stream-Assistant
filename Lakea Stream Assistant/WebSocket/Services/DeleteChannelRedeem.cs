using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Lakea_Stream_Assistant.WebSocket.Services
{
    public class DeleteChannelRedeem : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            base.OnOpen();
            Send("LakeaWebsocket: DeleteChannelRedeem -> Connection Confirmed");
            Terminal.Output("Socket: Open Service -> DeleteChannelRedeem");
            Logs.Instance.NewLog(LogLevel.Info, "Socket Service Opened -> DeleteChannelRedeem");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            base.OnMessage(e);
            Send("LakeaWebsocket: DeleteChannelRedeem -> Message Received");
            Terminal.Output("Socket: Message Service -> DeleteChannelRedeem, " + e.Data);
            Logs.Instance.NewLog(LogLevel.Info, "Socket Service Message -> DeleteChannelRedeem, " + e.Data);
            Twitch.DeleteChannelRedeem(e.Data);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);
            if (e.Reason == "")
            {
                Terminal.Output("Socket: Close Service -> DeleteChannelRedeem");
                Logs.Instance.NewLog(LogLevel.Info, "Socket Service Close -> DeleteChannelRedeem");
            }
            else
            {
                Terminal.Output("Socket: Close Service -> DeleteChannelRedeem, " + e.Reason);
                Logs.Instance.NewLog(LogLevel.Warning, "Socket Service Close -> DeleteChannelRedeem, " + e.Reason);
            }
        }

        protected override void OnError(WebSocketSharp.ErrorEventArgs e)
        {
            base.OnError(e);
            Terminal.Output("Socket: Errored Service -> DeleteChannelRedeem, " + e.Message);
            Logs.Instance.NewLog(LogLevel.Error, e.Message);
        }
    }
}
