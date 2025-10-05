using Spatial.Core.Common;
using Spatial.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Spatial.Core.Documents
{
    public class TCXTrack : XmlBase
    {
        [XmlElement("Trackpoint")]
        public List<TCXTrackPoint> TrackPoints { get; set; }

        public List<GeoCoordinateExtended> ToCoords(Boolean infill = true)
        {
            var ret = TrackPoints
                .Select(trkpt => trkpt.ToCoord())
                .ToList();

            if (infill)
                return ret.InfillPositions();
            else
                return ret;
        }
    }
}
