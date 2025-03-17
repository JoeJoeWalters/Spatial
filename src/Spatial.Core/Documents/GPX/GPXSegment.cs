using Spatial.Core.Common;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Spatial.Core.Documents
{
    /// <summary>
    /// An ordered sequence of points. (for polygons or polylines, e.g.)
    /// </summary>
    [Serializable]
    public class GPXSegment : XmlBase
    {
        /// <summary>
        /// Ordered list of geographic points.
        /// </summary>
        [XmlElement("pt")]
        public List<GPXPoint> Points { get; set; } = new List<GPXPoint>();
    }
}
