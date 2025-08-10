using Spatial.Core.Common;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Spatial.Core.Documents
{
    /// <summary>
    /// trk represents a track - an ordered list of points describing a path.
    /// </summary>
    [Serializable]
    public class GPXTrack : XmlBase
    {
        /// <summary>
        /// GPS name of track.
        /// </summary>
        [XmlElement("name")]
        public String Name { get; set; } = String.Empty;

        /// <summary>
        /// GPS comment for track.
        /// </summary>
        [XmlElement("cmt")]
        public String Comment { get; set; } = String.Empty;

        /// <summary>
        /// User description of track. 
        /// </summary>
        [XmlElement("desc")]
        public String Description { get; set; } = String.Empty;

        /// <summary>
        /// Source of data. Included to give user some idea of reliability and accuracy of data.
        /// </summary>
        [XmlElement("src")]
        public String Source { get; set; } = String.Empty;

        /// <summary>
        /// Links to external information about the route. 
        /// </summary>
        [XmlElement("link")]
        public GPXLink Link { get; set; }

        /// <summary>
        /// GPS track number.
        /// </summary>
        [XmlElement("number")]
        public Int32 TrackNumber { get; set; } = 0;

        /// <summary>
        /// Type (classification) of track.
        /// </summary>
        [XmlElement("type")]
        public String Type { get; set; } = String.Empty;

        /// <summary>
        /// You can add extend GPX by adding your own elements from another schema here.
        /// </summary>
        [XmlElement("extensions")]
        public List<GPXExtension> Extensions { get; set; } = new List<GPXExtension>();

        /// <summary>
        /// A Track Segment holds a list of Track Points which are logically connected in order. To represent a single GPS track where GPS reception was lost, or the GPS receiver was turned off, start a new Track Segment for each continuous span of track data.
        /// </summary>
        [XmlElement("trkseg")]
        public List<GPXTrackSegment> TrackSegments { get; set; } = new List<GPXTrackSegment>();

        /// <summary>
        /// Convert the list of points to a list of common coordinates
        /// </summary>
        /// <returns></returns>
        public List<GeoCoordinateExtended> ToCoords()
        {
            List<GeoCoordinateExtended> merged = new List<GeoCoordinateExtended>();
            TrackSegments.ForEach(seg => merged.AddRange(seg.ToCoords()));
            return merged;
        }
    }
}
