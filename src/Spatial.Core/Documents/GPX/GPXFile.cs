using Spatial.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

/// <summary>
/// https://docs.microsoft.com/en-us/dotnet/standard/serialization/controlling-xml-serialization-using-attributes
/// </summary>
namespace Spatial.Core.Documents
{
    /// <summary>
    /// Implementation of http://www.topografix.com/GPX/1/1/gpx.xsd
    /// where GPXType is the base 1.1 version which hasn't changed since 2004
    /// but done in this way incase it should be system future proof
    /// </summary>
    [Serializable]
    [XmlRoot("gpx", Namespace = "http://www.topografix.com/GPX/1/1")]
    public class GPXFile : GPXType, IGeoFileConvertable { }

    [Serializable]
    public class GPXType : XmlBase
    {
        // ISO 8601 formatter instead of using roundtrip kind parsing as needed for read and write (get and set)
        public static String DateTimeFormat = "yyyy-MM-ddTHH:mm:sszzz";

        [XmlAttribute("creator")]
        public String Creator { get; set; } = String.Empty;

        [XmlAttribute("version")]
        public Decimal Version { get; set; } = 1.1M;

        [XmlElement("metadata")]
        public GPXMetaData MetaData { get; set; } = new GPXMetaData();

        [XmlElement("wpt")]
        public List<GPXWaypoint> Waypoints { get; set; } = new List<GPXWaypoint>();

        [XmlElement("rte")]
        public List<GPXRoute> Routes { get; set; } = new List<GPXRoute>();

        [XmlElement("trk")]
        public List<GPXTrack> Tracks { get; set; } = new List<GPXTrack>();

        [XmlElement("extensions")]
        public List<GPXExtension> Extensions { get; set; } = new List<GPXExtension>();

        /// <summary>
        /// Convert this GPX Type to the common GeoFile format
        /// </summary>
        /// <returns>The GPX converted to GeoFile format</returns>
        public GeoFile ToGeoFile()
        {
            // Create a blank new file with the basic information
            GeoFile result = new GeoFile()
            {
                Name = this.MetaData.Name,
                Author = this.Creator,
                Routes = new List<GeoFileRoute>()
            };

            // Loop the routes and add them to the GeoFile version
            result.Routes = this.Routes.Select(route =>
                new GeoFileRoute()
                {
                    Name = route.Name,
                    Points = route.ToCoords()
                }).ToList();

            // Loop the tracks and add them to the GeoFile version
            result.Routes.AddRange(this.Tracks.Select(track =>
                new GeoFileRoute()
                {
                    Name = track.Name,
                    Points = track.ToCoords()
                }).ToList());

            return result;
        }

        /// <summary>
        /// Convert a GeoFile to the native format
        /// </summary>
        /// <param name="file">The GeoFile format to convert from</param>
        /// <returns>Success Or Failure flag</returns>
        public Boolean FromGeoFile(GeoFile file)
        {
            this.Routes = file.Routes
                .Select(rt =>
                    new GPXRoute()
                    {
                        RoutePoints = rt.Points
                            .Where(pt => !pt.BadCoordinate && !pt.IsUnknown)
                            .Select(pt =>
                                GPXWaypoint.FromCoord(pt)
                            ).ToList()
                    })
                .ToList();

            return true;
        }
    }
}
