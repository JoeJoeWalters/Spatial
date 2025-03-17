using FluentAssertions;
using System;
using Spatial.Core.Documents;
using Xunit;
using Spatial.Core.Common;
using System.Text.Json;

namespace Spatial.Tests
{
    public class GPXTests : TestBase
    {
        private readonly GPXFile gpxTrackFile;

        public GPXTests()
        {
            gpxTrackFile = base.GetXMLData<GPXFile>("GPXFiles/GPXRouteOnly.gpx");
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
			GPXPoint cloned = JsonSerializer.Deserialize<GPXPoint>(JsonSerializer.Serialize<GPXPoint>(point, Shared.SerialiserOptions), Shared.SerialiserOptions); // Serialise and then deserialise the object to break the references to new objects

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
            Int32 origionalCount = 0;
            Int32 transformedCount = 0;

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
            Int32 transformedCount = 0;
            GeoFile geoFile = gpxTrackFile.ToGeoFile();
            Int32 origionalCount = geoFile.Routes[0].Points.Count;
            GPXFile gpxFile = new GPXFile();

            // ACT
            Boolean success = gpxFile.FromGeoFile(geoFile);
            transformedCount = gpxFile.Routes[0].RoutePoints.Count; // Count of transformed track

            // ASSERT
            success.Should().BeTrue();
            gpxFile.Routes.Should().NotBeEmpty();
            gpxFile.Routes[0].RoutePoints.Should().NotBeEmpty();
            origionalCount.Should().Be(transformedCount);
        }
    }
}
