using Spatial.Core.Common;
using Spatial.Core.Documents;
using Spatial.Core.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Transactions;

namespace Spatial.Core.Helpers
{
    public static class TrackHelper
    {
        public static List<GeoCoordinateExtended> InfillPositions(this List<GeoCoordinateExtended> points)
        {
            GeoCoordinateExtended lastValidPosition = null;
            points.ForEach(pt =>
            {
                // Not a bad coordinate?
                if (!pt.BadCoordinate)
                {
                    lastValidPosition = pt; // Assign this as the last known good position 
                }
                else if (pt.BadCoordinate && lastValidPosition != null)
                {
                    // Infill the position from the last known good
                    pt.Latitude = lastValidPosition.Latitude;
                    pt.Longitude = lastValidPosition.Longitude;
                    pt.Altitude = lastValidPosition.Altitude;
                    pt.BadCoordinate = false;
                    lastValidPosition = pt; // Reassign this as the last known good
                }
            });

            return points;
        }

        public static List<GeoCoordinateExtended> CalculateSpeeds(this List<GeoCoordinateExtended> points)
        {
            // Loop the coords from start to finish missing the first 
            // to make sure we always have the end point one step ahead
            for (var coordId = 1; coordId < points.Count; coordId++)
            {
                if (coordId == 1) { points[0].Speed = 0; }
                else if (coordId > 1)
                {
                    // Calculate speed from last point to this one
                    points[coordId].CalculateSpeed(points[coordId - 1]);
                }
            }

            return points;
        }

        public static Double CalculateTotalDistance(this List<GeoCoordinateExtended> points)
        {
            Double distance = 0D;

            // Loop the coords from start to finish missing the first 
            // to make sure we always have the end point one step ahead
            for (var coordId = 1; coordId < points.Count; coordId++)
            {
                distance += points[coordId].GetDistanceTo(points[coordId - 1]);
            }

            return distance;
        }


        /// <summary>
        /// Calculates to the total time for the track in different ways
        /// depending on the calculation type, relies on the speeds having already been calculated by the appropriate method
        /// </summary>
        /// <param name="timeCalculationType"></param>
        /// <returns></returns>
        public static TimeSpan TotalTime(this List<GeoCoordinateExtended> points, TimeCalculationType timeCalculationType)
        {
            TimeSpan result = new TimeSpan();

            // Enforce speed calculation first, we can't assume someone has already done this or it might need re-calculating
            points = points.CalculateSpeeds();

            switch (timeCalculationType)
            {
                case TimeCalculationType.ActualTime:
                    result = points[points.Count - 1].Time - points[0].Time; // Difference in time between the first and last points in the track
                    break;

                case TimeCalculationType.MovingTime:

                    // Loop all points in the track and only count those that had speed (movement) between the two points
                    for (var coordId = 1; coordId < points.Count; coordId++)
                    {
                        // Any movement recorded?
                        if (points[coordId].Speed > 0)
                        {
                            result = result.Add(points[coordId].Time - points[coordId - 1].Time); // Add the timespan between the points to the total
                        }
                    }

                    break;
            }

            return result;
        }

        /// <summary>
        /// Remove not moving points and time from the list of coordinates
        /// </summary>
        /// <param name="points">The list of points to be cleaned of not moving time</param>
        /// <returns>The list of points with the not moving time removed</returns>
        public static List<GeoCoordinateExtended> RemoveNotMoving(this List<GeoCoordinateExtended> points)
        {
            List<GeoCoordinateExtended> reference = points.Clone().CalculateSpeeds(); // Make a copy of the points first to break the reference and re-calculate the speeds
            List<KeyValuePair<TimeSpan, GeoCoordinateExtended>> timeDiffArray = new List<KeyValuePair<TimeSpan, GeoCoordinateExtended>>(); // New list of points with the timedifference between them as the key
            List<GeoCoordinateExtended> cleaned = new List<GeoCoordinateExtended>() { reference[0] }; // Create the new clean array, the first point always has no speed so always add this to the cleaned array as a starting point

            // Loop all points in the track and only count those that had speed (movement) between the two points
            var coordId = 1; // Start from the second point as the first will always have no speed (from another point) 
            while (coordId <= reference.Count - 1)
            {
                if (reference[coordId].Speed != 0)
                {
                    TimeSpan timeDiff = reference[coordId].Time - reference[coordId - 1].Time; // Calculate the time to the previous coordinate
                    timeDiffArray.Add(new KeyValuePair<TimeSpan, GeoCoordinateExtended>(timeDiff, reference[coordId])); // Add the coordinate that shows movement with the time difference to the last coordinate
                }

                coordId++;
            }

            // Loop the array with the none moving time stripped out and re-calculate the coordinate times based on the time differences
            DateTime pointInTime = cleaned[0].Time; // Take the start time of the origional track to be the new start time
            for (var refId = 0; refId < timeDiffArray.Count; refId++)
            {
                GeoCoordinateExtended manipulated = timeDiffArray[refId].Value; // Create a reference to the point on the track to have the time manipluated

                pointInTime += timeDiffArray[refId].Key; // Add the time difference to the rolling point in time
                manipulated.Time = pointInTime; // Set the new time to the point so the new point is still the same difference in time between the GPS pings to the last point that moved

                cleaned.Add(manipulated); // Add the time manipulated point to the cleaned array
            }

            // Re-calculate the speeds on the new points
            cleaned.CalculateSpeeds();

            // Return the cleaned list of points
            return cleaned;
        }

        /// <summary>
        /// Clone an existing set of points so they can be modified breaking the reference to the origional list
        /// </summary>
        /// <param name="points">The set of points to clone</param>
        /// <returns>The new list of points cloned from the source</returns>
        public static List<GeoCoordinateExtended> Clone(this List<GeoCoordinateExtended> points)
            => JsonSerializer.Deserialize<List<GeoCoordinateExtended>>(JsonSerializer.Serialize<List<GeoCoordinateExtended>>(points, Shared.SerialiserOptions), Shared.SerialiserOptions); // Serialise and then deserialise the object to break the references to new objects

        /// <summary>
        /// Take a set of points and modify them to be rounded to the nearest X meters
        /// </summary>
        /// <param name="points">The set of points to round</param>
        /// <param name="roundingMeters">The meter preceision to round to</param>
        /// <returns></returns>
        public static List<GeoCoordinateExtended> Round(this List<GeoCoordinateExtended> points, Double meters)
            => points.Clone().Select(coord => coord.Round(meters)).ToList();

        /// <summary>
        /// Compare one set of points to another to give a similarity score based on how common they are
        /// </summary>
        /// <param name="points">The set of points to compare</param>
        /// <param name="compareTo">The set of points to compare the list of points to</param>
        /// <param name="activityType">What type of activity is it (mainly to reduce or increase the comparison fuzziness)</param>
        /// <returns></returns>
        public static Double Compare(this List<GeoCoordinateExtended> points, List<GeoCoordinateExtended> compareTo, ActivityType activityType)
        {
            Double score = 0.0D;

            // Score based on if the 
            List<GeoCoordinateExtended> matches = points.Delta(compareTo, activityType, CompareType.Matches);

            score = (1.0 / points.Count) * matches.Count;
            score = (score < 0) ? 0 : score;

            return score;
        }

        /// <summary>
        /// Work out the delta between two sets of points rounded to a given activity type
        /// </summary>
        /// <param name="points">The set of points to compare</param>
        /// <param name="compareTo">The set of points to compare the list of points to</param>
        /// <param name="activityType">What type of activity is it (mainly to reduce or increase the comparison fuzziness)</param>
        /// <returns></returns>
        public static List<GeoCoordinateExtended> Delta(this List<GeoCoordinateExtended> points, List<GeoCoordinateExtended> compareTo, ActivityType activityType, CompareType compareType)
        {
            List<GeoCoordinateExtended> sourceRounded = points.Round(5D);
            List<GeoCoordinateExtended> compareRounded = compareTo.Round(5D);

            switch (compareType)
            {
                case CompareType.Matches:
                    return sourceRounded.Where(source => compareRounded.Any(compare => source == compare)).ToList();
                default:
                    return sourceRounded.Where(source => !compareRounded.Any(compare => source == compare)).ToList();
            }
        }

        /// <summary>
        /// Split an array of points in to two pieces by a time frame relative to the start of the points
        /// e.g. 10 minutes in or 1 hour etc.
        /// </summary>
        /// <param name="points">The origional set of points</param>
        /// <param name="splitTime"></param>
        /// <returns>An array containing two arrays of geographic points representing the two parts for the split</returns>
        public static List<List<GeoCoordinateExtended>> Split(this List<GeoCoordinateExtended> points, TimeSpan splitTime)
        {
            // Calculate the actual point in time of the split by adding the timeframe to the start time of the points
            DateTime splitDatTime = points[0].Time.Add(splitTime);

            // Take the two segments
            List<GeoCoordinateExtended> part1 = points.Where(pt => pt.Time < splitDatTime).ToList();
            List<GeoCoordinateExtended> part2 = points.Where(pt => pt.Time >= splitDatTime).ToList();

            // Return an array of the split
            return new List<List<GeoCoordinateExtended>>() { part1, part2 };
        }

        /// <summary>
        /// Merge two or more tracks and ALL their points together and re-order the timeline, be aware no point vs time matching will be performed 
        /// and that you will need to apply an re-calculations again such as speed
        /// </summary>
        /// <param name="trackList">The array of tracks to be merged</param>
        /// <returns>The merged track</returns>
        public static List<GeoCoordinateExtended> Merge(this List<List<GeoCoordinateExtended>> trackList)
        {
            List<GeoCoordinateExtended> merged = new List<GeoCoordinateExtended>();
            trackList.ForEach(track => merged.AddRange(track)); // For each track, merge the points
            merged.ForEach(point => point.Speed = 0); // Destroy the speed calculations as some points may intersect now
            return merged.Clone().OrderBy(item => item.Time).ToList(); // Clone the points to break the byref linkage and then order by time so everything is in the right order
        }

        /// <summary>
        /// Get the fastest section of a track for a given distance
        /// </summary>
        /// <param name="points">The array of coordinates to check against</param>
        /// <param name="targetDistance">The target distance in meters to extract the quickest section for</param>
        /// <returns></returns>
        public static List<GeoCoordinateExtended> Fastest(this List<GeoCoordinateExtended> trackList, double targetDistance, bool removeNotMoving)
        {
            List<GeoCoordinateExtended> result = new List<GeoCoordinateExtended>();
            double shortest = double.MaxValue;
            TimeSpan timeCheck = TimeSpan.MaxValue;

            for (int start = 0; start < trackList.Count; start++)
            {
                double distance = 0.0;
                double speed = 0.0;
                for (int pointCount = start + 1; pointCount < trackList.Count; pointCount++)
                {
                    double pointDistance = trackList[pointCount - 1].GetDistanceTo(trackList[pointCount]);
                    distance += pointDistance;
                    speed += trackList[pointCount].Speed;
                    if (distance > targetDistance)
                    {
                        if (speed < shortest) // Seperate check as distance and speed could have a break between to end the loop
                        {
                            shortest = speed;
                            TimeSpan spanCheck = trackList[pointCount].Time - trackList[start].Time;
                            if (spanCheck < timeCheck)
                            {
                                timeCheck = spanCheck;
                                result = trackList.GetRange(start, (pointCount - start) + 1);
                            }
                        }
                        break;
                    }
                }
            }

            return result;
        }

        public static List<List<GeoCoordinateExtended>> Sections(this List<GeoCoordinateExtended> trackList, double sectionSize)
        {
            List<List<GeoCoordinateExtended>> result = new List<List<GeoCoordinateExtended>>();
            List<GeoCoordinateExtended> current = new List<GeoCoordinateExtended>();

            double distance = 0.0;
            for (int start = 1; start < trackList.Count; start++)
            {
                double pointDistance = trackList[start - 1].GetDistanceTo(trackList[start]);
                distance += pointDistance;
                if (distance > sectionSize)
                {
                    distance = 0;
                    result.Add(current);
                    current = new List<GeoCoordinateExtended>();
                }
                else
                {
                    current.Add(trackList[start - 1].Clone());
                }
            }

            // Anything left over?
            if (current.Count > 0)
                result.Add(current);

            return result;
        }
    }
}
