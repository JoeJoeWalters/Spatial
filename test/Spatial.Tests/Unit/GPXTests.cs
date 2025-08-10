using AwesomeAssertions;
using Spatial.Core.Common;
using Spatial.Core.Documents;
using System;
using System.Text.Json;
using Xunit;

namespace Spatial.Core.Tests.Unit
{
    public class GPXTests : TestBase
    {
        public GPXTests()
        {
        }

        [Fact]
        public void WaypointGPXDeserialise_Should_RetainData()
        {
            // ARRANGE

            // ACT

            // ASSERT
            gpxWithWaypoints.Waypoints.Should().NotBeEmpty("GPX file should have at least one waypoint defined.");
            gpxWithWaypoints.Waypoints[0].Time.Should().NotBeNullOrEmpty("GPX file should have a time defined for the first waypoint.");
        }

        [Fact]
        public void TrackGPXDeserialise_Should_RetainData()
        {
            // ARRANGE

            // ACT

            // ASSERT
            gpxTrackFile.Tracks.Should().NotBeEmpty("GPX file should have at least one track defined.");
            gpxTrackFile.Tracks[0].TrackSegments.Should().NotBeEmpty("GPX file should have at least one track segment defined.");
            gpxTrackFile.Tracks[0].TrackSegments[0].TrackPoints.Should().NotBeEmpty("GPX file should have at least one track point defined in the first track segment.");
            gpxTrackFile.Tracks[0].TrackSegments[0].TrackPoints[0].Time.Should().NotBeNullOrEmpty("GPX file should have a time defined for the first track point.");
        }

        [Fact]
        public void GPXRoute_Should_ConvertToListOfGeoCoordinates()
        {
            // ARRANGE
            GPXRoute gpxRoute = gpxWithWaypoints.Routes[0]; // Get the first route from the GPX file

            // ACT
            var geoCoordinates = gpxRoute.ToCoords(); // Convert the GPX route to a list of GeoCoordinates

            // ASSERT
#warning "TODO: This may need attention as Microsoft's base Geocoordinate used doubles and GPX files are stated to use decimal although right now it's not an issue in terms of precision"
            geoCoordinates.Should().NotBeEmpty("GPX route should convert to a non-empty list of GeoCoordinates.");
            geoCoordinates[0].Latitude.Should().Be((double)gpxRoute.RoutePoints[0].Latitude, "The first GeoCoordinate's latitude should match the first GPX route point's latitude.");
            geoCoordinates[0].Longitude.Should().Be((double)gpxRoute.RoutePoints[0].Longitude, "The first GeoCoordinate's longitude should match the first GPX route point's longitude.");
        }

        [Fact]
        public void GPXPoint_Compare_Setters()
        {
			// ARRANGE
			GPXPoint point = new GPXPoint();

			// ACT
			point.Latitude = 1.0M;
			point.Longitude = 2.0M;
			point.Elevation = 3.0M;
			point.Time = DateTime.UtcNow.ToString();
			GPXPoint cloned = JsonSerializer.Deserialize<GPXPoint>(JsonSerializer.Serialize(point, Shared.SerialiserOptions), Shared.SerialiserOptions); // Serialise and then deserialise the object to break the references to new objects

			// ASSERT
            cloned.Latitude.Should().Be(point.Latitude);
            cloned.Longitude.Should().Be(point.Longitude);
            cloned.Elevation.Should().Be(point.Elevation);
            cloned.Time.Should().Be(point.Time);
		}

		[Fact]
        public void Track_Compare_ToGeoFile_Conversion()
        {
            // ARRANGE
            int origionalCount = 0;
            int transformedCount = 0;

            // ACT
            origionalCount = gpxTrackFile.Tracks[0].TrackSegments[0].TrackPoints.Count; // Count of origional
            transformedCount = gpxTrackFile.ToGeoFile().Routes[0].Points.Count; // Do conversion and count

            // ASSERT
            origionalCount.Should().Be(transformedCount);
        }

        [Fact]
        public void Track_Compare_FromGeoFile_Conversion()
        {
            // ARRANGE
            int transformedCount = 0;
            GeoFile geoFile = gpxTrackFile.ToGeoFile();
            int origionalCount = geoFile.Routes[0].Points.Count;
            GPXFile gpxFile = new GPXFile();

            // ACT
            bool success = gpxFile.FromGeoFile(geoFile);
            transformedCount = gpxFile.Routes[0].RoutePoints.Count; // Count of transformed track

            // ASSERT
            success.Should().BeTrue();
            gpxFile.Routes.Should().NotBeEmpty();
            gpxFile.Routes[0].RoutePoints.Should().NotBeEmpty();
            origionalCount.Should().Be(transformedCount);
        }
    }
}
