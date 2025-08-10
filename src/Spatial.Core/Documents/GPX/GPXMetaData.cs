using Spatial.Core.Common;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Spatial.Core.Documents
{
    [Serializable]
    public class GPXMetaData : XmlBase
    {
        /// <summary>
        /// The name of the GPX file.
        /// </summary>
        [XmlElement("name")]
        public String Name { get; set; } = String.Empty;

        /// <summary>
        /// A description of the contents of the GPX file.
        /// </summary>
        [XmlElement("desc")]
        public String Description { get; set; } = String.Empty;

        /// <summary>
        /// The person or organization who created the GPX file.
        /// </summary>
        [XmlElement("author")]
        public GPXPerson Author { get; set; } = new GPXPerson();

        /// <summary>
        /// Copyright and license information governing use of the file.
        /// </summary>
        [XmlElement("copyright")]
        public GPXCopyright Copyright { get; set; } = new GPXCopyright();

        /// <summary>
        /// URLs associated with the location described in the file.
        /// </summary>
        [XmlElement("link")]
        public GPXLink Link { get; set; } = new GPXLink();

        /// <summary>
        /// The creation date of the file.
        /// </summary>
        [XmlIgnore]
        public DateTime CreatedDateTime = DateTime.MinValue;
        [XmlElement("time")]
        public String Time { get; set; }

        /// <summary>
        /// Keywords associated with the file. Search engines or databases can use this information to classify the data.
        /// </summary>
        [XmlElement("keywords")]
        public String Keywords { get; set; } = String.Empty;

        /// <summary>
        /// Minimum and maximum coordinates which describe the extent of the coordinates in the file.
        /// </summary>
        [XmlElement("bounds")]
        public GPXBounds Bounds { get; set; } = new GPXBounds();

        /// <summary>
        /// You can add extend GPX by adding your own elements from another schema here.
        /// </summary>
        [XmlElement("extensions")]
        public List<GPXExtension> Extensions { get; set; } = new List<GPXExtension>();
    }
}
