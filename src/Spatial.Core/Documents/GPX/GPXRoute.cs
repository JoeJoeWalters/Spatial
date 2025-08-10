using Spatial.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Spatial.Core.Documents
{
    /// <summary>
    /// rte represents route - an ordered list of waypoints representing a series of turn points leading to a destination.
    /// </summary>
    [Serializable]
    public class GPXRoute : XmlBase
    {
        /// <summary>
        /// GPS name of route.
        /// </summary>
        [XmlElement("name")]
        public String Name { get; set; } = String.Empty;

        /// <summary>
        /// GPS comment for route.
        /// </summary>
        [XmlElement("cmt")]
        public String Comment { get; set; } = String.Empty;

        /// <summary>
        /// Text description of route for user. Not sent to GPS.
        /// </summary>
        [XmlElement("desc")]
        public String Description { get; set; } = String.Empty;

        /// <summary>
        /// Source of data.Included to give user some idea of reliability and accuracy of data.
        /// </summary>
        [XmlElement("src")]
        public String Source { get; set; } = String.Empty;

        /// <summary>
        /// Links to external information about the route. 
        /// </summary>
        [XmlElement("link")]
        public GPXLink Link { get; set; } = new GPXLink();

        /// <summary>
        /// GPS route number.
        /// </summary>
        [XmlElement("number")]
        public Int32 RouteNumber { get; set; } = 0;

        /// <summary>
        /// Type (classification) of route.
        /// </summary>
        [XmlElement("type")]
        public String Type { get; set; } = String.Empty;

        /// <summary>
        /// You can add extend GPX by adding your own elements from another schema here.
        /// </summary>
        [XmlElement("extensions")]
        public List<GPXExtension> Extensions { get; set; } = new List<GPXExtension>();

        /// <summary>
        /// A list of route points.
        /// </summary>
        [XmlElement("rtept")]
        public List<GPXWaypoint> RoutePoints { get; set; } = new List<GPXWaypoint>();

        /// <summary>
        /// Convert the list of points to a list of common coordinates
        /// </summary>
        /// <returns></returns>
        public List<GeoCoordinateExtended> ToCoords() => RoutePoints.Select(pt => pt.ToCoord()).ToList();
    }
}
