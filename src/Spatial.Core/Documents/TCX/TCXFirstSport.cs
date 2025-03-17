using Spatial.Core.Common;
using System.Xml.Serialization;

namespace Spatial.Core.Documents
{
    public class TCXFirstSport : XmlBase
    {
        [XmlElement("Activity")]
        public TCXActivity Activity { get; set; }
    }
}
