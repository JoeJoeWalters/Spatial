using System;
using System.Collections.Generic;
using System.Text;

namespace Spatial.Core.Documents.TCX.Exceptions
{
    public class TCXConversionException : Exception
    {
        public TCXConversionException() : base() { }
        public TCXConversionException(String message) : base(message) { }
        public TCXConversionException(String message, Exception inner) : base(message, inner) { }
    }
}
