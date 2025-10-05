using System;
using System.Collections.Generic;
using System.Text;

namespace Spatial.Core.Documents.TCX.Exceptions
{
    public class TCXConversionException : Exception
    {
        public TCXConversionException(String message) : base(message) { }
    }
}
