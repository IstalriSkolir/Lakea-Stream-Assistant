using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;
using System.Text.Json;
using TwitchLib.Api.Helix.Models.ChannelPoints.CreateCustomReward;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Lakea_Stream_Assistant.WebSocket.Services
{
    public class CreateChannelRedeem : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            base.OnOpen();
            Send("LakeaWebsocket: CreateChannelRedeem -> ConnectionConfirmed");
            Terminal.Output("Socket: Open Service -> CreateChannelRedeem");
            Logs.Instance.NewLog(Enums.LogLevel.Info, "Socket Service Opened -> CreateChannelRedeem");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            base.OnMessage(e);
            Task.Run(() => {
                Terminal.Output("Socket: Message Service -> CreateChannelRedeem, " + e.Data);
                Logs.Instance.NewLog(Enums.LogLevel.Info, "Socket Service Message -> CreateChannelRedeem, " + e.Data);
            });
            string response = processMessage(e);
            Send("LakeaWebsocket: CreateChannelRedeem -> Message Received:" + response);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);
            if (e.Reason == "")
            {
                Terminal.Output("Socket: Close Service -> CreateChannelRedeem");
                Logs.Instance.NewLog(Enums.LogLevel.Info, "Socket Service Close -> CreateChannelRedeem");
            }
            else
            {
                Terminal.Output("Socket: Close Service -> CreateChannelRedeem, " + e.Reason);
                Logs.Instance.NewLog(Enums.LogLevel.Warning, "Socket Service Close -> CreateChannelRedeem, " + e.Reason);
            }
        }

        protected override void OnError(WebSocketSharp.ErrorEventArgs e)
        {
            base.OnError(e);
            Terminal.Output("Socket: Errored Service -> CreateChannelRedeem, " + e.Message);
            Logs.Instance.NewLog(Enums.LogLevel.Error, e.Message);
        }

        private string processMessage(MessageEventArgs args)
        {
            try
            {
                CreateCustomRewardsRequest request = JsonSerializer.Deserialize<CreateCustomRewardsRequest>(args.Data);
                CreateCustomRewardsResponse reponse = Twitch.CreateChannelRedeem(request).Result;
                string jsonString = JsonSerializer.Serialize(reponse);
                return jsonString;
            }
            catch (Exception ex)
            {
                Terminal.Output("Socket: Error Getting Channel Redeem Data -> " + ex.Message);
                Logs.Instance.NewLog(Enums.LogLevel.Error, ex.Message);
            }
            return "Failed to Create Channel Redeem";
        }
    }
}
