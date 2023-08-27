using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events.EventAbstracts;
using TwitchLib.Api.Core.Extensions.System;
using TwitchLib.Client.Events;
using TwitchLib.PubSub.Events;

namespace Lakea_Stream_Assistant.Models.Events
{
    public class TwitchCommand : Event
    {
        private OnChatCommandReceivedArgs args;

        public TwitchCommand(EventSource source, EventType type, OnChatCommandReceivedArgs args)
        {
            this.source = source;
            this.type = type;
            this.args = args;
        }

        public OnChatCommandReceivedArgs Args { get { return args; } }

        public override Dictionary<string, string> GetArgs()
        {
            Dictionary<string, string> commandArgs = new Dictionary<string, string>
            {
                { "CommandIdentifier", args.Command.CommandIdentifier.ToString() },
                { "CommandText", args.Command.CommandText },
                { "IsModerator", args.Command.ChatMessage.IsModerator.ToString() },
                { "IsSubscriber", args.Command.ChatMessage.IsSubscriber.ToString() },
                { "IsPartner", args.Command.ChatMessage.IsPartner.ToString() },
                { "IsVip", args.Command.ChatMessage.IsVip.ToString() },
                { "DisplayName", args.Command.ChatMessage.DisplayName },
                { "ChatMessage", args.Command.ChatMessage.Message }
            };
            for (int i = 0; i < args.Command.ArgumentsAsList.Count; i++)
            {
                commandArgs.Add("CommandArg" + (i + 1), args.Command.ArgumentsAsList[i]);
            }
            return commandArgs;
        }
    }
}
