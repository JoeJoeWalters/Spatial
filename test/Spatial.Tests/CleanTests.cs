using FluentAssertions;
using System;
using System.Collections.Generic;
using Spatial.Core.Documents;
using Spatial.Core.Helpers;
using Spatial.Types;
using Xunit;

namespace Spatial.Tests
{
    public class CleanTests : TestBase
    {
        private readonly GeoFile trackFile;

        public CleanTests()
        {
            trackFile = base.GetXMLData<GPXFile>("GPXFiles/HalfMarathon.gpx").ToGeoFile();
        }

        [Fact]
        public void Remove_Not_Moving_Time()
        {
            // ARRANGE
            List<GeoCoordinateExtended> points = trackFile.Routes[0].Points;
            TimeSpan tolerenceAmount = new TimeSpan(0, 0, 2); // 2 second tolerence each way on result
            TimeSpan movingTime = points.TotalTime(TimeCalculationType.MovingTime); // Work out the moving time of the track
            TimeSpan tolerenceLower = movingTime.Subtract(tolerenceAmount); // lower tolerence bound
            TimeSpan tolerenceUpper = movingTime.Add(tolerenceAmount); // upper tolerence bound

            // ACT
            List<GeoCoordinateExtended> cleanedPoints = points.RemoveNotMoving(); // Call the process to remove the not moving time and heal
            TimeSpan cleanedTime = cleanedPoints.TotalTime(TimeCalculationType.ActualTime); // Calculate the new actual time of the track

            // ASSERT
            cleanedTime.Should().BeGreaterOrEqualTo(tolerenceLower);
            cleanedTime.Should().BeLessOrEqualTo(tolerenceUpper);
        }

    }
}
