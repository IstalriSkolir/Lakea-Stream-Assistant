using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lakea_Stream_Assistant.Models.Tokens
{
    public class KeepAliveToken
    {
        private bool keepAlive;

        public KeepAliveToken()
        {
            this.keepAlive = true;
        }

        public bool IsAlive { get { return this.keepAlive; } }

        public void Kill()
        {
            keepAlive = false;
        }
    }
}
