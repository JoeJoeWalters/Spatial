using Spatial.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Spatial.Core.Documents
{
    /// <summary>
    /// A Track Segment holds a list of Track Points which are logically connected in order. To represent a single GPS track where GPS reception was lost, or the GPS receiver was turned off, start a new Track Segment for each continuous span of track data.
    /// </summary>
    [Serializable]
    public class GPXTrackSegment : XmlBase
    {
        /// <summary>
        /// A Track Point holds the coordinates, elevation, timestamp, and metadata for a single point in a track.
        /// </summary>
        [XmlElement("trkpt")]
        public List<GPXWaypoint> TrackPoints { get; set; } = new List<GPXWaypoint>();

        /// <summary>
        /// You can add extend GPX by adding your own elements from another schema here.
        /// </summary>
        [XmlElement("extensions")]
        public List<GPXExtension> Extensions { get; set; } = new List<GPXExtension>();

        /// <summary>
        /// Convert the list of points to a list of common coordinates
        /// </summary>
        /// <returns></returns>
        public List<GeoCoordinateExtended> ToCoords() => TrackPoints.Select(pt => pt.ToCoord()).ToList();
    }
}
