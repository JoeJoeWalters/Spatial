using Spatial.Core.Common;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Spatial.Core.Documents
{
    public class TCXMultiSportSession : XmlBase
    {
        [XmlElement("Id")]
        public String Id { get; set; }

        [XmlElement("FirstSport")]
        public TCXFirstSport FirstSport { get; set; }

        [XmlElement("NextSport")]
        public List<TCXNextSport> NextSport { get; set; }

        [XmlElement("Notes")]
        public String Notes { get; set; }
    }
}
