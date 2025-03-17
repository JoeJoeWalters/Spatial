using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Spatial.Core.Common
{
    public static class Shared
    {
        public static double EarthRadius = 40010040D; // What is the earth's radius in meters
        public static double LatitudeDistance = EarthRadius / 360.0D; // What is 1 degree of latitude

        public static JsonSerializerOptions SerialiserOptions = new JsonSerializerOptions() { NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals }; // To handle Infinity and NaN
    }
}
