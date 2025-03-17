using Spatial.Core.Common;
using Spatial.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Spatial.Core.Documents
{
    /// <summary>
    /// Activity type with the Training element dropped as we only care about getting to the movement data
    /// </summary>
    public class TCXActivity : XmlBase
    {
        [XmlElement("Id")]
        public String Id { get; set; }

        [XmlElement("Lap")]
        public List<TCXActivityLap> Laps { get; set; }

        [XmlElement("Notes")]
        public String Notes { get; set; }

        [XmlElement("Creator")]
        public TCXAbstractSource Creator { get; set; }

        [XmlElement("Extensions")]
        public TCXExtensions Extensions { get; set; }

        /// <summary>
        /// Convert the list of points to a list of common coordinates
        /// </summary>
        /// <returns></returns>
        public List<GeoCoordinateExtended> ToCoords()
        {
            List<GeoCoordinateExtended> merged = new List<GeoCoordinateExtended>();
            Laps.ForEach(lap => merged.AddRange(lap.Track.ToCoords()));

            // Infilling is done for each track but there could be bad coordinates left over at the start or end of tracks that
            // might still need dealing with so check first rather than always doing it.
            if (merged.Where(pt => pt.BadCoordinate).Count() > 0)
                return merged.InfillPositions(); // Infill the positions that might still exist in boundaries between tracks before returning
            else
                return merged; // No need for infilling as no bad coordinates
        }
    }
}
