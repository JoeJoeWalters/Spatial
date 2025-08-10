using Spatial.Core.Common;
using System;
using System.Xml.Serialization;

namespace Spatial.Core.Documents
{
    public class TCXTrackPoint : XmlBase
    {
        /// <summary>
        /// Creation/modification timestamp for element. Date and time in are in Univeral Coordinated Time (UTC), not local time! Conforms to ISO 8601 specification for date/time representation. Fractional seconds are allowed for millisecond timing in tracklogs.
        /// </summary>
        [XmlIgnore]
        public DateTime CreatedDateTime = DateTime.MinValue;

        [XmlElement("Time")]
        public String Time
        {
            get { return CreatedDateTime.ToString(TCXFile.DateTimeFormat); }
            set { this.CreatedDateTime = DateTime.Parse(value); }
        }

        [XmlElement("Position")]
        public TCXPosition Position { get; set; }

        [XmlElement("AltitudeMeters")]
        public Double AltitudeMeters { get; set; } = 0D;

        [XmlElement("DistanceMeters")]
        public Double DistanceMeters { get; set; } = 0D;

        [XmlElement("HeartRateBpm")]
        public TCXHeartRateInBeatsPerMinute HeartRateBpm { get; set; } = new TCXHeartRateInBeatsPerMinute();

        [XmlElement("Cadence")]
        public Byte Cadence { get; set; } = 0;

        [XmlElement("SensorState")]
        public String SensorState { get; set; } = String.Empty;

        [XmlElement("Extensions")]
        public TCXExtensions Extensions { get; set; } = new TCXExtensions();

        public GeoCoordinateExtended ToCoord()
            => (this.Position == null) ? new GeoCoordinateExtended(0, 0, this.AltitudeMeters, this.CreatedDateTime) { BadCoordinate = true } : new GeoCoordinateExtended(this.Position.LatitudeDegrees, this.Position.LongitudeDegrees, this.AltitudeMeters, this.CreatedDateTime);
    }
}
