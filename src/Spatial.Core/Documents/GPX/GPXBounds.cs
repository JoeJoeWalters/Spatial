using Spatial.Core.Common;
using System;
using System.Xml.Serialization;

namespace Spatial.Core.Documents
{
    /// <summary>
    /// Two lat/lon pairs defining the extent of an element.
    /// </summary>
    [Serializable]
    public class GPXBounds : XmlBase
    {
        /// <summary>
        /// The minimum latitude.
        /// </summary>
        [XmlElement("minlat")]
        public Decimal MinLatitude { get; set; } = 0.0M;

        /// <summary>
        /// The minimum longitude.
        /// </summary>
        [XmlElement("minlon")]
        public Decimal MinLongitude { get; set; } = 0.0M;

        /// <summary>
        /// The maximum latitude.
        /// </summary>
        [XmlElement("maxlat")]
        public Decimal MaxLatitude { get; set; } = 0.0M;

        /// <summary>
        /// The maximum longitude.
        /// </summary>
        [XmlElement("maxlon")]
        public Decimal MaxLongitude { get; set; } = 0.0M;
    }
}
