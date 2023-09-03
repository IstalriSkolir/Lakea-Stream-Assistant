using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lakea_Stream_Assistant.Models.Configuration
{
    public class CommandConfiguration
    {
        private string name;
        private bool enabled;
        private bool modOnly;

        public CommandConfiguration(string name, bool enabled, bool modOnly)
        {
            this.name = name;
            this.enabled = enabled;
            this.modOnly = modOnly;
        }

        public string Name { get { return name; } }
        public bool IsEnabled { get { return enabled; } }
        public bool ModOnly { get {  return modOnly; } }
    }
}
