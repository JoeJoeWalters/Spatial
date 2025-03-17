using Spatial.Core.Common;
using System;
using System.Xml.Serialization;

namespace Spatial.Core.Documents
{
    public class TCXAbstractSource : XmlBase
    {
        [XmlElement("Name")]
        public String Name { get; set; }

        [XmlElement("UnitId")]
        public String UnitID { get; set; }

        [XmlElement("ProductID")]
        public String ProductID { get; set; }

        [XmlElement("Version")]
        public TCXVersion Version { get; set; }

        [XmlElement("Build")]
        public TCXBuild Build { get; set; }

        [XmlElement("PartNumber")]
        public String PartNumber { get; set; }

        [XmlElement("LangID")]
        public String LangID { get; set; }
    }
}
