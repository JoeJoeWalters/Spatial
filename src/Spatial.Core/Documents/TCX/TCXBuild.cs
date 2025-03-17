using Spatial.Core.Common;
using System.Xml.Serialization;

namespace Spatial.Core.Documents
{
    public class TCXBuild : XmlBase
    {
        [XmlElement("Version")]
        public TCXVersion Version { get; set; }
    }
}
