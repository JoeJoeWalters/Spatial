using FluentAssertions;
using System;
using Spatial.Core.Documents;
using Xunit;
using Spatial.Core.Helpers;
using Spatial.Core.Common;
using System.Text.Json;

namespace Spatial.Tests
{
	public class TCXTests : TestBase
	{
		private readonly TCXFile tcxTrackFile;

		public TCXTests()
		{
			tcxTrackFile = base.GetXMLData<TCXFile>("TCXFiles/HalfMarathon.tcx");
		}

		[Fact]
		public void TCXPoint_Compare_Setters()
		{
			// ARRANGE
			TCXTrackPoint point = new TCXTrackPoint();

			// ACT
			point.Position = new TCXPosition()
			{
				LatitudeDegrees = 1.0D,
				LongitudeDegrees = 1.0D
			};
			point.AltitudeMeters = 3.0D;
			point.Time = DateTime.UtcNow.ToString();

			TCXTrackPoint cloned = JsonSerializer.Deserialize<TCXTrackPoint>(JsonSerializer.Serialize<TCXTrackPoint>(point, Shared.SerialiserOptions), Shared.SerialiserOptions); // Serialise and then deserialise the object to break the references to new objects

			// ASSERT
			cloned.Position.LatitudeDegrees.Should().Be(point.Position.LatitudeDegrees);
			cloned.Position.LongitudeDegrees.Should().Be(point.Position.LongitudeDegrees);
			cloned.AltitudeMeters.Should().Be(point.AltitudeMeters);
			cloned.Time.Should().Be(point.Time);
		}

		[Fact]
		public void Track_Compare_ToGeoFile_Conversion()
		{
			// ARRANGE
			Int32 origionalCount = 0;
			Int32 transformedCount = 0;

			// ACT
			origionalCount = tcxTrackFile.Activities.Activity[0].ToCoords().Count; // Count of origional
			transformedCount = tcxTrackFile.ToGeoFile().Routes[0].Points.Count; // Do conversion and count

			// ASSERT
			origionalCount.Should().Be(transformedCount);
		}

		[Fact]
		public void Track_Compare_FromGeoFile_Conversion()
		{
			// ARRANGE
			Int32 transformedCount = 0;
			GeoFile geoFile = tcxTrackFile.ToGeoFile();
			Int32 origionalCount = geoFile.Routes[0].Points.Count;
			TCXFile tcxFile = new TCXFile();

			// ACT
			Boolean success = tcxFile.FromGeoFile(geoFile);
			Double totalDistance = geoFile.Routes[0].Points.CalculateTotalDistance();
			transformedCount = tcxFile.Activities.Activity[0].Laps[0].Track.TrackPoints.Count; // Count of transformed track

			// ASSERT
			success.Should().BeTrue();
			tcxFile.Activities.Activity.Should().NotBeEmpty();
			tcxFile.Activities.Activity[0].Laps.Should().NotBeEmpty();
			tcxFile.Activities.Activity[0].Laps[0].DistanceMeters.Should().Be(totalDistance);
			origionalCount.Should().Be(transformedCount);
		}
	}
}
