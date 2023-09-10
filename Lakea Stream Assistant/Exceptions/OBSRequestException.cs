using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lakea_Stream_Assistant.Exceptions
{
    public class OBSRequestException : Exception
    {
        public OBSRequestException() { }
        public OBSRequestException(string message) : base(message) { }
        public OBSRequestException(string message, Exception innerException) : base(message, innerException) { }
    }
}
