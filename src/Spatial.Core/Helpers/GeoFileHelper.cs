using Spatial.Core.Common;
using Spatial.Core.Documents;
using Spatial.Core.Helpers;
using Spatial.Core.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;

namespace Spatial.Core.Helpers
{
    public static class GeoFileHelper
    {
        public static GeoFile InfillPositions(this GeoFile file)
        {
            GeoFile clone = file.Clone();
            clone.Routes.ForEach(route => route.Points = route.Points.InfillPositions());
            return clone;
        }

        public static GeoFile CalculateSpeeds(this GeoFile file)
        {
            GeoFile clone = file.Clone();
            clone.Routes.ForEach(route => route.Points = route.Points.CalculateSpeeds());
            return clone;
        }

        public static Double CalculateTotalDistance(this GeoFile file)
            => file.Routes.Select(route => route.Points.CalculateTotalDistance()).Sum();

        public static TimeSpan TotalTime(this GeoFile file, TimeCalculationType timeCalculationType)
        {
            TimeSpan totalTime = new TimeSpan(0,0,0);
            file.Routes.ForEach(route => totalTime = totalTime.Add(route.Points.TotalTime(timeCalculationType)));
            return totalTime;
        }

        public static GeoFile RemoveNotMoving(this GeoFile file)
        {
            GeoFile clone = file.Clone();
            clone.Routes.ForEach(route => route.Points = route.Points.RemoveNotMoving());
            return clone;
        }

        public static GeoFile Clone(this GeoFile file)
            => JsonSerializer.Deserialize<GeoFile>(JsonSerializer.Serialize<GeoFile>(file, Shared.SerialiserOptions), Shared.SerialiserOptions); // Serialise and then deserialise the object to break the references to new objects

        public static Double Compare(this GeoFile fileFrom, GeoFile fileTo, ActivityType activityType, TrackCompareMethods method) 
            => fileFrom.Routes[0].Points.Compare(fileTo.Routes[0].Points, activityType, method);

        public static List<List<GeoCoordinateExtended>> Split(this GeoFile file, TimeSpan splitTime) 
            => file.Routes[0].Points.Split(splitTime);

        public static List<GeoCoordinateExtended> Merge(this List<GeoFile> files) 
            => files.Select(geo => geo.Routes[0].Points).ToList().Merge();

    }
}
