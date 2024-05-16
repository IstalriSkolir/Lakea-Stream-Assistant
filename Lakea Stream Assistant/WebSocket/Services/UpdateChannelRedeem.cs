using Lakea_Stream_Assistant.Singletons;
using Lakea_Stream_Assistant.Static;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using TwitchLib.Api.Helix.Models.ChannelPoints.UpdateCustomReward;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Lakea_Stream_Assistant.WebSocket.Services
{
    public class UpdateChannelRedeem : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            base.OnOpen();
            Send("LakeaWebsocket: UpdateChannelRedeem -> ConnectionConfirmed");
            Terminal.Output("Socket: Open Service -> UpdateChannelRedeem");
            Logs.Instance.NewLog(Enums.LogLevel.Info, "Socket Service Opened -> UpdateChannelRedeem");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            base.OnMessage(e);
            Task.Run(() => {
                Terminal.Output("Socket: Message Service -> UpdateChannelRedeem, " + e.Data);
                Logs.Instance.NewLog(Enums.LogLevel.Info, "Socket Service Message -> UpdateChannelRedeem, " + e.Data);
            });
            string response = processMessage(e);
            Send("LakeaWebsocket: UpdateChannelRedeem -> Message Received:" + response);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);
            if (e.Reason == "")
            {
                Terminal.Output("Socket: Close Service -> UpdateChannelRedeem");
                Logs.Instance.NewLog(Enums.LogLevel.Info, "Socket Service Close -> UpdateChannelRedeem");
            }
            else
            {
                Terminal.Output("Socket: Close Service -> UpdateChannelRedeem, " + e.Reason);
                Logs.Instance.NewLog(Enums.LogLevel.Warning, "Socket Service Close -> UpdateChannelRedeem, " + e.Reason);
            }
        }

        protected override void OnError(WebSocketSharp.ErrorEventArgs e)
        {
            base.OnError(e);
            Terminal.Output("Socket: Errored Service -> UpdateChannelRedeem, " + e.Message);
            Logs.Instance.NewLog(Enums.LogLevel.Error, e.Message);
        }

        private string processMessage(MessageEventArgs args)
        {
            try
            {
                JObject json = JObject.Parse(args.Data);
                string redeemID = (string)json["RedeemID"];
                string redeemDataString = json["RedeemData"].ToString();
                UpdateCustomRewardRequest redeemData = JsonSerializer.Deserialize<UpdateCustomRewardRequest>(redeemDataString);
                UpdateCustomRewardResponse response = Twitch.UpdateChannelRedeem(redeemID, redeemData).Result;
                string responseString = JsonSerializer.Serialize(response);
                return responseString;
            }
            catch (Exception ex)
            {
                Terminal.Output("Socket: Error Getting Channel Redeem Data -> " + ex.Message);
                Logs.Instance.NewLog(Enums.LogLevel.Error, ex.Message);
            }
            return "Failed to Update Channel Redeem";
        }
    }
}
