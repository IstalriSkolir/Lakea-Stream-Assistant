using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lakea_Stream_Assistant.Exceptions
{
    public class ConfigValidationException : Exception
    {
        public ConfigValidationException() { }
        public ConfigValidationException(string message) : base(message) { }
        public ConfigValidationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
