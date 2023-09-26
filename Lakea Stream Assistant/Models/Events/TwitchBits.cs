using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events.EventAbstracts;
using TwitchLib.PubSub.Events;

namespace Lakea_Stream_Assistant.Models.Events
{
    public class TwitchBits : Event
    {
        private OnBitsReceivedV2Args args;

        public TwitchBits(EventSource source, EventType type, OnBitsReceivedV2Args args) 
        {
            this.source = source;
            this.type = type;
            this.args = args;
        }

        public OnBitsReceivedV2Args Args { get { return args; } }

        public override Dictionary<string, string> GetArgs()
        {
            Dictionary<string, string> bitsArgs = new Dictionary<string, string>
            {
                { "Bits", args.BitsUsed.ToString() },
                { "TotalBits", args.TotalBitsUsed.ToString() },
                { "IsAnonymous", args.IsAnonymous.ToString() },
                { "DisplayName", args.UserName },
                { "ChannelName", args.ChannelName },
                { "ChatMessage", args.ChatMessage },
                { "AccountID", args.UserId }
            };
            return bitsArgs;
        }
    }
}
