using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lakea_Stream_Assistant.Exceptions
{
    public class EnumConversionException : Exception
    {
        public EnumConversionException() { }
        public EnumConversionException(string message) : base(message) { }
        public EnumConversionException(string message,  Exception innerException) : base(message, innerException) { }
    }
}
