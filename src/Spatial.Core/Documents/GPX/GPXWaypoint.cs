using Spatial.Core.Common;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Spatial.Core.Documents
{
    /// <summary>
    /// wpt represents a waypoint, point of interest, or named feature on a map.
    /// </summary>
    [Serializable]
    public class GPXWaypoint : XmlBase
    {
        /// <summary>
        /// Elevation (in meters)
        /// </summary>
        [XmlElement("ele")]
        public Decimal Elevation { get; set; } = 0.0M;

        /// <summary>
        /// Creation/modification timestamp for element. Date and time in are in Universal Coordinated Time (UTC), not local time! Conforms to ISO 8601 specification for date/time representation. Fractional seconds are allowed for millisecond timing in tracklogs.
        /// </summary>
        [XmlIgnore]
        public DateTime CreatedDateTime = DateTime.MinValue;
        [XmlElement("time")]
        public String Time
        {
            get { return CreatedDateTime.ToString(GPXType.DateTimeFormat); }
            set { this.CreatedDateTime = DateTime.Parse(value); }
        }

        /// <summary>
        /// Magnetic variation (in degrees) at the point (0.0 to 360.0)
        /// </summary>
        [XmlElement("magvar")]
        public Decimal MagneticVariation { get; set; } = 0.0M;

        /// <summary>
        /// Height (in meters) of geoid (mean sea level) above WGS84 earth ellipsoid. As defined in NMEA GGA message.
        /// </summary>
        [XmlElement("geoidheight")]
        public Decimal Height { get; set; } = 0.0M;

        /// <summary>
        /// The GPS name of the waypoint. This field will be transferred to and from the GPS. GPX does not place restrictions on the length of this field or the characters contained in it. It is up to the receiving application to validate the field before sending it to the GPS.
        /// </summary>
        [XmlElement("name")]
        public String Name { get; set; } = String.Empty;

        /// <summary>
        /// GPS waypoint comment. Sent to GPS as comment.
        /// </summary>
        [XmlElement("cmt")]
        public String Comment { get; set; } = String.Empty;

        /// <summary>
        /// A text description of the element. Holds additional information about the element intended for the user, not the GPS.
        /// </summary>
        [XmlElement("desc")]
        public String Description { get; set; } = String.Empty;

        /// <summary>
        /// Source of data. Included to give user some idea of reliability and accuracy of data. "Garmin eTrex", "USGS quad Boston North", e.g.
        /// </summary>
        [XmlElement("src")]
        public String Source { get; set; } = String.Empty;

        /// <summary>
        /// Link to additional information about the waypoint.
        /// </summary>
        [XmlElement("link")]
        public GPXLink Link { get; set; } = new GPXLink();

        /// <summary>
        /// Text of GPS symbol name. For interchange with other programs, use the exact spelling of the symbol as displayed on the GPS. If the GPS abbreviates words, spell them out.
        /// </summary>
        [XmlElement("sym")]
        public String Symbol { get; set; } = String.Empty;

        /// <summary>
        /// Type (classification) of the waypoint.
        /// </summary>
        [XmlElement("type")]
        public String Type { get; set; } = String.Empty;

        /*
        Accuracy Information
        */

        /// <summary>
        /// Type of GPX fix. (Types can be none, 2d, 3d, dgps, pps)
        /// </summary>
        [XmlElement("fix")]
        public String Fix { get; set; } = String.Empty;

        /// <summary>
        /// Number of satellites used to calculate the GPX fix.
        /// </summary>
        [XmlElement("sat")]
        public Int32 Satellites { get; set; } = 0;

        /// <summary>
        /// Horizontal dilution of precision.
        /// </summary>
        [XmlElement("hdop")]
        public Decimal DilutionHorizontal { get; set; } = 0.0M;

        /// <summary>
        /// Vertical dilution of precision.
        /// </summary>
        [XmlElement("vdop")]
        public Decimal DilutionVertical { get; set; } = 0.0M;

        /// <summary>
        /// Position dilution of precision.
        /// </summary>
        [XmlElement("pdop")]
        public Decimal DilutionPosition { get; set; } = 0.0M;

        /// <summary>
        /// Number of seconds since last DGPS update.
        /// </summary>
        [XmlElement("ageofdgpsdata")]
        public Decimal Age { get; set; } = 0.0M;

        /// <summary>
        /// ID of DGPS station used in differential correction.
        /// </summary>
        [XmlElement("dgpsid")]
        public GPXStation Station { get; set; } = new GPXStation();

        /// <summary>
        /// You can add extend GPX by adding your own elements from another schema here.
        /// </summary>
        [XmlElement("extensions")]
        public List<GPXExtension> Extensions { get; set; } = new List<GPXExtension>();

        /// <summary>
        /// The latitude of the point.This is always in decimal degrees, and always in WGS84 datum.
        /// </summary>
        [XmlAttribute("lat")]
        public Decimal Latitude { get; set; } = 0.0M;

        /// <summary>
        /// The longitude of the point. This is always in decimal degrees, and always in WGS84 datum.
        /// </summary>
        [XmlAttribute("lon")]
        public Decimal Longitude { get; set; } = 0.0M;

        /// <summary>
        /// Convert point to native coordinate
        /// </summary>
        /// <returns></returns>
        public GeoCoordinateExtended ToCoord()
        {
            return new GeoCoordinateExtended(
                (Double)this.Latitude,
                (Double)this.Longitude,
                (Double)this.Elevation,
                this.CreatedDateTime
                );
        }

        public static GPXWaypoint FromCoord(GeoCoordinateExtended coord)
        {
            return new GPXWaypoint()
            {
                CreatedDateTime = coord.Time,
                Height = (decimal)coord.Altitude,
                Latitude = (decimal)coord.Latitude,
                Longitude = (decimal)coord.Longitude
            };
        }
    }
}
