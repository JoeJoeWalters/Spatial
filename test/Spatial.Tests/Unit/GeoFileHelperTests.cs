using AwesomeAssertions;
using Spatial.Core.Documents;
using Spatial.Core.Helpers;
using Spatial.Core.Types;
using System;
using System.Collections.Generic;
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

        [Fact]
        public void TotalTime_Should_BeSumOfAllRoutes()
        {
            // ARRANGE
            DateTime now = DateTime.UtcNow;
            DateTime last = now.AddMinutes(2);
            Random random = new Random((int)now.Ticks);

            GeoFile multiRouteFile = new GeoFile
            {
                Name = "Multi Route File",
                Author = "Test Author",
                Routes = new List<GeoFileRoute>
                {
                    new GeoFileRoute
                    {
                        Name = "Route 1",
                        Points = new List<GeoCoordinateExtended>
                        {
                            new GeoCoordinateExtended { Time = now, Latitude = 90 / random.Next(), Longitude = 90 / random.Next() },
                            new GeoCoordinateExtended { Time = now.AddMinutes(1), Latitude = 90 / random.Next(), Longitude = 90 / random.Next() },
                        }
                    },
                    new GeoFileRoute
                    {
                        Name = "Route 2",
                        Points = new List<GeoCoordinateExtended>
                        {
                            new GeoCoordinateExtended { Time = last.AddMinutes(-1), Latitude = 90 / random.Next(), Longitude = 90 / random.Next() },
                            new GeoCoordinateExtended { Time = last, Latitude = 90 / random.Next(), Longitude = 90 / random.Next() }
                        }
                    }
                }
            };

            // ACT
            TimeSpan diff = multiRouteFile.TotalTime(TimeCalculationType.ActualTime);

            // ASSERT
            diff.Should().Be(last - now);
        }
    }
}
