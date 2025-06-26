using AwesomeAssertions;
using Spatial.Core.Documents;
using Spatial.Core.Helpers;
using Spatial.Core.Types;
using System;
using Xunit;

namespace Spatial.Core.Tests.Unit
{
    public class GeoFileHelperTests : TestBase
    {
        public GeoFileHelperTests() : base()
        {
        }

        [Fact]
        public void GeoFile_WithOneTrack_When_InfillPositions_Should_HaveSamePoints()
        {
            // ARRANGE
            GeoFile processed = geoTrackFile.InfillPositions();

            // ACT

            // ASSERT
            processed.Routes[0].Points.Count.Should().Be(geoTrackFile.Routes[0].Points.Count);
        }

        [Fact]
        public void GeoFile_WithOneTrack_When_InfillPositions_Should_HaveSameDistance()
        {
            // ARRANGE
            GeoFile processed = geoFile.InfillPositions();

            // ACT
            double processedDistance = Math.Round(processed.CalculateTotalDistance() / 1000, 2);

            // ASSERT
            processedDistance.Should().Be(HalfMarathonDistance);
            geoDistance.Should().Be(processedDistance);
        }

        [Fact]
        public void GeoFile_WithOneTrack_When_CalculateSpeeds_Should_HaveSamePoints()
        {
            // ARRANGE
            GeoFile processed = geoTrackFile.CalculateSpeeds();

            // ACT

            // ASSERT
            processed.Routes[0].Points.Count.Should().Be(geoTrackFile.Routes[0].Points.Count);
        }

        [Fact]
        public void GeoFile_WithOneTrack_When_CalculateSpeeds_Should_HaveSameDistanceAndTime()
        {
            // ARRANGE
            GeoFile processed = geoFile.CalculateSpeeds();

            // ACT
            double processedDistance = Math.Round(processed.CalculateTotalDistance() / 1000, 2);
            TimeSpan processedTime = processed.TotalTime(TimeCalculationType.ActualTime);
            TimeSpan processedMovingTime = processed.TotalTime(TimeCalculationType.MovingTime);

            // ASSERT
            geoDistance.Should().Be(processedDistance);
            processedDistance.Should().Be(HalfMarathonDistance);
            processedTime.TotalMinutes.Should().BeApproximately(HalfMarathonTotalMinutes, 1);
            processedMovingTime.TotalMinutes.Should().BeApproximately(HalfMarathonMovingTime, 1);
        }

        [Fact]
        public void GeoFile_WhenCloned_Should_BeEquivalent()
        {
            // ARRANGE
            GeoFile processed = geoTrackFile.Clone();

            // ACT

            // ASSERT
            processed.Should().BeEquivalentTo(geoTrackFile);
        }
    }
}
