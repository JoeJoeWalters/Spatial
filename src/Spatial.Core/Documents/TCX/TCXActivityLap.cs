using Spatial.Core.Common;
using System;
using System.Xml.Serialization;

namespace Spatial.Core.Documents
{
    public class TCXActivityLap : XmlBase
    {
        [XmlAttribute("StartTime")]
        public String StartTime { get; set; }

        [XmlElement("TotalTimeSeconds")]
        public Double TotalTimeSeconds { get; set; }

        [XmlElement("DistanceMeters")]
        public Double DistanceMeters { get; set; }

        [XmlElement("MaximumSpeed")]
        public Double MaximumSpeed { get; set; }

        [XmlElement("Calories")]
        public Int32 Calories { get; set; }

        [XmlElement("AverageHeartRateBpm")]
        public TCXHeartRateInBeatsPerMinute AverageHeartRateBpm { get; set; }

        [XmlElement("MaximumHeartRateBpm")]
        public TCXHeartRateInBeatsPerMinute MaximumHeartRateBpm { get; set; }

        [XmlElement("Intensity")]
        public String Intensity { get; set; }

        [XmlElement("Cadence")]
        public Byte Cadence { get; set; }

        [XmlElement("TriggerMethod")]
        public String TriggerMethod { get; set; }

        [XmlElement("Track")]
        public TCXTrack Track { get; set; }

        [XmlElement("Notes")]
        public String Notes { get; set; }

        [XmlElement("Extensions")]
        public TCXExtensions Extensions { get; set; }
    }
}
