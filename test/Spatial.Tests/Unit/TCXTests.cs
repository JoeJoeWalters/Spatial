using AwesomeAssertions;
using Spatial.Core.Common;
using Spatial.Core.Documents;
using Spatial.Core.Documents.TCX.Exceptions;
using Spatial.Core.Helpers;
using System;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace Spatial.Core.Tests.Unit
{
	public class TCXTests : TestBase
	{
		public TCXTests()
		{
		}

		[Fact]
		public void TCXMultiSport_Mapping_Success()
		{
			// ARRANGE
			TCXFile tcxFile = tcxMultisportFile;

			// ACT

			// ASSERT
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

			TCXTrackPoint cloned = JsonSerializer.Deserialize<TCXTrackPoint>(JsonSerializer.Serialize(point, Shared.SerialiserOptions), Shared.SerialiserOptions); // Serialise and then deserialise the object to break the references to new objects

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
			int origionalCount = 0;
			int transformedCount = 0;

			// ACT
			origionalCount = tcxTrackFile.Activities.Activity[0].ToCoords().Count; // Count of origional
			transformedCount = tcxTrackFile.ToGeoFile().Routes[0].Points.Count; // Do conversion and count

			// ASSERT
			origionalCount.Should().Be(transformedCount);
		}

		[Fact]
		public void TrackCompare_FromGeoFile_ShouldBeSuccess()
		{
			// ARRANGE
			int transformedCount = 0;
			GeoFile geoFile = tcxTrackFile.ToGeoFile();
			int origionalCount = geoFile.Routes[0].Points.Count;
			TCXFile tcxFile = new TCXFile();

			// ACT
			bool success = tcxFile.FromGeoFile(geoFile);
			double totalDistance = geoFile.Routes[0].Points.CalculateTotalDistance();
			transformedCount = tcxFile.Activities.Activity[0].Laps[0].Track.TrackPoints.Count; // Count of transformed track

			// ASSERT
			success.Should().BeTrue();
			tcxFile.Activities.Activity.Should().NotBeEmpty();
			tcxFile.Activities.Activity.Count.Should().BeGreaterThan(0);
			var activity = tcxFile.Activities.Activity[0];
            activity.Id.Should().NotBeNull();
            activity.Notes.Should().NotBeNull();
            activity.Extensions.Should().NotBeNull();
            activity.Laps.Should().NotBeEmpty();
            activity.Laps[0].DistanceMeters.Should().Be(totalDistance);
			origionalCount.Should().Be(transformedCount);
        }

        [Fact]
        public void TrackCompare_FromGeoFile_WithNoRoutes_ShouldThrowException()
        {
            // ARRANGE
            GeoFile geoFile = tcxTrackFile.ToGeoFile();
            geoFile.Routes.Clear(); // Clear the routes so nothing to convert
            TCXFile tcxFile = new TCXFile();

            // ACT
            Action act = () => tcxFile.FromGeoFile(geoFile);

            // ASSERT
            act.Should().Throw<TCXConversionException>()
                .WithMessage("No routes or points to convert");
        }

        [Fact]
		public void TrackCompare_FromGeoFile_WithNoPointsInRoutes_ShouldThrowException()
		{
            // ARRANGE
            GeoFile geoFile = tcxTrackFile.ToGeoFile();
			geoFile.Routes[0].Points.Clear(); // Clear the points so nothing to convert
            TCXFile tcxFile = new TCXFile();

            // ACT
            Action act = () => tcxFile.FromGeoFile(geoFile);

            // ASSERT
            act.Should().Throw<TCXConversionException>()
				.WithMessage("No routes or points to convert");
        }


        [Fact]
		public void TCXMultiSport_Check_XMLConversion()
		{
            // ARRANGE

            // ACT

            // ASSERT
            tcxMultisportFile.Activities.MultiSportSession.Should().NotBeNull();
            tcxMultisportFile.Activities.MultiSportSession.Id.Should().NotBeNull();
            tcxMultisportFile.Activities.MultiSportSession.NextSport.Should().NotBeNull();
            tcxMultisportFile.Activities.MultiSportSession.NextSport.Count.Should().BeGreaterThan(0);
            tcxMultisportFile.Activities.MultiSportSession.FirstSport.Should().NotBeNull();
        }

		[Fact]
		public void TCXActivity_Should_Infill_IfBadCoordinate()
		{
			// ARRANGE
			var activity = tcxTrackFile.Activities.Activity[0];
			activity.Laps[0].Track.TrackPoints[1].Position = null; // Invalidate one of the points, position so last coordinate can be used (0)

            // ACT
            var cloned = activity.ToCoords();
			var sumOfAllLaps = activity.Laps.SelectMany(x => x.Track.TrackPoints).Count();

            // ASSERT
            cloned.Count.Should().Be(sumOfAllLaps);
        }

        [Fact]
        public void TCXActivity_Should_NotInfill_IfNoBadCoordinates()
        {
            // ARRANGE
            var activity = CleanTCXPointNulls(tcxTrackFile.Activities.Activity[0]);

            // ACT
            var cloned = activity.ToCoords();
            var sumOfAllLaps = activity.Laps.SelectMany(x => x.Track.TrackPoints).Count();

            // ASSERT
            cloned.Count.Should().Be(sumOfAllLaps);
        }

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
        public void TCXTrack_ToCoordsInfill_Should_HaveSamePoints(Boolean infill)
		{
			// ARRANGE
			var track = tcxTrackFile.Activities.Activity[0].Laps[0].Track;
			int origionalCount = track.TrackPoints.Count;

			// ACT
			var coords = track.ToCoords(infill);
			int newCount = coords.Count;
			
			// ASSERT
			newCount.Should().Be(origionalCount);
        }

        private TCXActivity CleanTCXPointNulls(TCXActivity activity)
		{
			TCXActivity clean = activity.Clone();
            activity.Laps.ForEach(lap =>
			{
				lap.Track.TrackPoints = lap.Track.TrackPoints.Where(pt => pt.Position != null).ToList();
            });
			return clean;
        }
    }
}
