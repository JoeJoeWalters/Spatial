using Spatial.Core.Common;
using System;
using System.Xml.Serialization;

namespace Spatial.Core.Documents
{
    public class TCXPosition : XmlBase
    {
        [XmlElement("LatitudeDegrees")]
        public Double LatitudeDegrees { get; set; }

        [XmlElement("LongitudeDegrees")]
        public Double LongitudeDegrees { get; set; }
    }
}
