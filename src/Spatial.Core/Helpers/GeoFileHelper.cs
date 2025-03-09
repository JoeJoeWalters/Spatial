using Spatial.Documents;
using Spatial.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Spatial.Helpers
{
    public static class GeoFileHelper
    {
        private static JsonSerializerOptions serialiserOptions = new JsonSerializerOptions() { NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals }; // To handle Infinity and NaN

        public static GeoFile InfillPositions(this GeoFile file)
        {
            GeoFile clone = file.Clone();
            foreach (var route in clone.Routes)
            {
                route.Points = route.Points.InfillPositions();
            }
            return clone;
        }

        public static GeoFile CalculateSpeeds(this GeoFile file)
        {
            GeoFile clone = file.Clone();
            foreach (var route in clone.Routes)
            {
                route.Points = route.Points.CalculateSpeeds();
            }
            return clone;
        }

        public static Double CalculateTotalDistance(this GeoFile file)
        {
            Double totalDistance = 0.0D;
            foreach (var route in file.Routes)
            {
                totalDistance += route.Points.CalculateTotalDistance();
            }
            return totalDistance;
        }

        public static TimeSpan TotalTime(this GeoFile file, TimeCalculationType timeCalculationType)
        {
            Boolean isFirst = true;
            TimeSpan totalTime = new TimeSpan();
            foreach (var route in file.Routes)
            {
                TimeSpan thisTime = route.Points.TotalTime(timeCalculationType);
                if (isFirst)
                    totalTime = thisTime;
                else
                    totalTime.Add(thisTime);

                isFirst = false;
            }
            return totalTime;
        }

        public static GeoFile RemoveNotMoving(this GeoFile file)
        {
            GeoFile clone = file.Clone();
            foreach (var route in clone.Routes)
            {
                route.Points = route.Points.RemoveNotMoving();
            }
            return clone;
        }

        public static GeoFile Clone(this GeoFile file)
            => JsonSerializer.Deserialize<GeoFile>(JsonSerializer.Serialize<GeoFile>(file, serialiserOptions), serialiserOptions); // Serialise and then deserialise the object to break the references to new objects

        public static Double Compare(this GeoFile fileFrom, GeoFile fileTo, ActivityType activityType) 
            => fileFrom.Routes[0].Points.Compare(fileTo.Routes[0].Points, activityType);

        public static List<GeoCoordinateExtended> Delta(this GeoFile fileFrom, GeoFile fileTo, ActivityType activityType, CompareType compareType) 
            => fileFrom.Routes[0].Points.Delta(fileTo.Routes[0].Points, activityType, compareType);

        public static List<List<GeoCoordinateExtended>> Split(this GeoFile file, TimeSpan splitTime) 
            => file.Routes[0].Points.Split(splitTime);

        public static List<GeoCoordinateExtended> Merge(this List<GeoFile> files) 
            => files.Select(geo => geo.Routes[0].Points).ToList().Merge();

    }
}
