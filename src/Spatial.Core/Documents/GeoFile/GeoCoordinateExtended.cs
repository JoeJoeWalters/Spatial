using Spatial.Core.Common;
using Spatial.Core.Helpers;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Spatial.Core.Documents
{
    /// <summary>
    /// Geocoordinate extended to add time of the coordinate so it
    /// can be used to calculate speed between two points
    /// </summary>
    public class GeoCoordinateExtended : GeoCoordinate
    {
        private static JsonSerializerOptions serialiserOptions = new JsonSerializerOptions() { NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals }; // To handle Infinity and NaN

        public DateTime Time { get; set; } = DateTime.MinValue;

        /// <summary>
        /// If a coordinate cannot represent the longitude and latitude e.g. in TCX files where the signal is paused no position is given,
        /// so used as a trigger to infill coordinates from previous entries on conversion.
        /// </summary>
        public Boolean BadCoordinate { get; set; } = false;

        /// <summary>
        /// Default constructor for Newtonsoft to perform serialisation etc.
        /// </summary>
        public GeoCoordinateExtended()
        {
        }

        public GeoCoordinateExtended(Double latitude, Double longitude, Double altitude, DateTime time) : base(latitude, longitude, altitude)
        {
            Time = time;
        }

        public GeoCoordinateExtended(Double latitude, Double longitude, Double altitude) : base(latitude, longitude, altitude)
        {
        }

        /// <summary>
        /// Calculate the speed from a previous point
        /// </summary>
        /// <param name="previous"></param>
        public void CalculateSpeed(GeoCoordinateExtended previous)
        {
            Double distance = this.GetDistanceTo(previous);
            Double seconds = ((TimeSpan)(this.Time - previous.Time)).TotalSeconds;
            this.Speed = distance / seconds;
        }

        /// <summary>
        /// Round this coordinate to the nearest grid coordinate of X meters
        /// </summary>
        /// <param name="meters"></param>
        public GeoCoordinateExtended Round(Double meters)
        {
            // Coordinate offsets in radians
            Double latitudeMeters = this.Latitude * Shared.LatitudeDistance;
            Double longitudeMeters = this.Longitude * (Shared.EarthRadius * Math.Cos(this.Latitude) / 360.0D);

            Double roundedLatitude = meters * Math.Round(latitudeMeters / meters, 0);
            Double roundedLongitude = meters * Math.Round(longitudeMeters / meters, 0);

            this.Latitude = roundedLatitude / Shared.LatitudeDistance;
            this.Longitude = roundedLongitude / (Shared.EarthRadius * Math.Cos(this.Latitude) / 360.0D);

            return this;
        }

        /// <summary>
        /// Clone this coordinate to a new instance
        /// </summary>
        /// <returns>The new instance of the coordinate</returns>
        public GeoCoordinateExtended Clone()
            => JsonSerializer.Deserialize<GeoCoordinateExtended>(JsonSerializer.Serialize<GeoCoordinateExtended>(this, serialiserOptions), serialiserOptions);

        /// <summary>
        /// Interpolate a point between two points in a track based on a distance between the two points
        /// </summary>
        /// <param name="to">The point to travel to</param>
        /// <param name="distance">How far to travel to the other point</param>
        /// <returns></returns>
        public GeoCoordinateExtended Interpolate(GeoCoordinateExtended to, double distance)
        {
            double distanceCalc = distance / this.GetDistanceTo(to);
            double dx = to.Longitude - this.Longitude;
            double dy = to.Latitude - this.Latitude;
            double newLat = this.Latitude + distanceCalc * dy;
            double newLon = this.Longitude + distanceCalc * dx;
            return new GeoCoordinateExtended(newLat, newLon, 0); // TODO : Altitude
        }

    }
}
