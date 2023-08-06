using Lakea_Stream_Assistant.Enums;
using Lakea_Stream_Assistant.Models.Events.EventAbstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.PubSub.Events;

namespace Lakea_Stream_Assistant.Models.Events
{
    public class TwitchFollow : Event
    {
        private OnFollowArgs args;

        public TwitchFollow(EventSource source, EventType type, OnFollowArgs args)
        {
            this.source = source;
            this.type = type;
            this.args = args;
        }

        public override EventSource Source { get { return source; } }
        public override EventType Type { get { return type; } }
        public OnFollowArgs Args { get { return args; } }
    }
}
