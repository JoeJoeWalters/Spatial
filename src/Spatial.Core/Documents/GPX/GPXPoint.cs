using Spatial.Core.Common;
using System;
using System.Xml.Serialization;

namespace Spatial.Core.Documents
{
    /// <summary>
    /// A geographic point with optional elevation and time. Available for use by other schemas.
    /// </summary>
    [Serializable]
    public class GPXPoint : XmlBase
    {
        /// <summary>
        /// The elevation (in meters) of the point.
        /// </summary>
        [XmlElement("ele")]
        public Decimal Elevation { get; set; } = 0.0M;

        /// <summary>
        /// The time that the point was recorded.
        /// </summary>
        [XmlIgnore]
        public DateTime CreatedDateTime = DateTime.MinValue;
        [XmlElement("time")]
        public String Time { get; set; }

        /// <summary>
        /// The latitude of the point. Decimal degrees, WGS84 datum.
        /// </summary>
        [XmlElement("lat")]
        public Decimal Latitude { get; set; } = 0.0M;

        /// <summary>
        /// The longitude of the point. Decimal degrees, WGS84 datum.
        /// </summary>
        [XmlElement("lon")]
        public Decimal Longitude { get; set; } = 0.0M;
    }
}
