using System.Xml;
using System.Xml.Serialization;

namespace Spatial.Core.Common
{
    public abstract class XmlBase
    {
        [XmlAnyElement]
        public XmlElement[] Unsupported { get; set; }
    }

}
