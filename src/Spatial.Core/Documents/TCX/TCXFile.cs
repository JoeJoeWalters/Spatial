using Spatial.Core.Common;
using Spatial.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

/// <summary>
/// https://www8.garmin.com/xmlschemas/TrainingCenterDatabasev2.xsd
/// </summary>
namespace Spatial.Core.Documents
{
    /// <summary>
    /// Implementation of https://www8.garmin.com/xmlschemas/TrainingCenterDatabasev2.xsd
    /// Folders, workouts, 
    /// s element(s) not mapped because we only care about the core activity data we can extract right now
    /// </summary>
    [Serializable]
    [XmlRoot("TrainingCenterDatabase", Namespace = "http://www.garmin.com/xmlschemas/TrainingCenterDatabase/v2")]
    public class TCXFile : XmlBase, IGeoFileConvertable
    {
        // ISO 8601 formatter instead of using roundtrip kind parsing as needed for read and write (get and set)
        public static String DateTimeFormat = "yyyy-MM-ddTHH:mm:sszzz";

        [XmlElement("Activities")]
        public TCXActivities Activities { get; set; }

        [XmlElement("Author")]
        public TCXAbstractSource Author { get; set; }

        [XmlElement("Extensions")]
        public TCXExtensions Extensions { get; set; }

        public GeoFile ToGeoFile()
        {
            GeoFile result = new GeoFile();

            // Transform the activity to the route information
            result.Routes = this.Activities.Activity.Select(activity => new GeoFileRoute() { Name = activity.Id, Points = activity.ToCoords().CalculateSpeeds() }).ToList();

            return result;
        }

        /// <summary>
        /// Convert a GeoFile to the native format
        /// </summary>
        /// <param name="file">The GeoFile format to convert from</param>
        /// <returns>Success Or Failure flag</returns>
        public Boolean FromGeoFile(GeoFile file)
        {
            this.Activities =
                new TCXActivities()
                {
                    Activity = file.Routes.Select(rt =>
                    new TCXActivity()
                    {
                        Creator = new TCXAbstractSource()
                        {
                        },
                        Laps = new List<TCXActivityLap>()
                        {
                            new TCXActivityLap()
                            {
                                AverageHeartRateBpm = new TCXHeartRateInBeatsPerMinute() { Value = 0 },
                                Cadence = 0,
                                Calories = 0,
                                DistanceMeters = rt.Points.CalculateTotalDistance(),
                                Extensions = new TCXExtensions() { },
                                Intensity = String.Empty,
                                MaximumHeartRateBpm = new TCXHeartRateInBeatsPerMinute() { Value = 0 },
                                MaximumSpeed = 0,
                                Notes = String.Empty,
                                StartTime = String.Empty,
                                TotalTimeSeconds = 0,
                                TriggerMethod = String.Empty,
                                Track = new TCXTrack()
                                {
                                    TrackPoints = rt.Points.Select(pt =>
                                        new TCXTrackPoint()
                                        {
                                            AltitudeMeters = pt.Altitude,
                                            Cadence = 0, // SPM for running, revolutions for cycling (apparantly)
                                            CreatedDateTime = pt.Time,
                                            DistanceMeters = 0,
                                            Extensions = new TCXExtensions(){ },
                                            HeartRateBpm = new TCXHeartRateInBeatsPerMinute(){ },
                                            Position = new TCXPosition()
                                            {
                                                LatitudeDegrees = pt.Latitude,
                                                LongitudeDegrees = pt.Longitude
                                            },
                                            SensorState = String.Empty,
                                            Time = ((pt.Time == null) ? DateTime.MinValue : pt.Time).ToString(TCXFile.DateTimeFormat)
                                        }
                                        ).ToList()
                                }
                            }
                        }
                    }).ToList()

                };

            return true;
        }
    }
}