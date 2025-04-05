using FluentAssertions;
using System;
using System.Collections.Generic;
using Spatial.Core.Documents;
using Spatial.Core.Helpers;
using Xunit;

namespace Spatial.Core.Tests.Unit
{
    public class AnalysisTests : TestBase
    {
        public AnalysisTests()
        {
        }

        [Fact]
        public void Get_FastestSection_From_TrackData()
        {
            // ARRANGE
            List<GeoCoordinateExtended> trackPoints = geoFile.Routes[0].Points;
            List<GeoCoordinateExtended> result;
            TimeSpan timeDifference;
            TimeSpan minimumTime = new TimeSpan(TimeSpan.TicksPerSecond * 60 * 8); // 8 minutes (I can't run faster than that)
            double requiredDistance = 1609.34; // 1 mile
            double actualDistance = 0.0;

            // ACT
            result = trackPoints.Fastest(requiredDistance, true); // Get the fastest mile
            timeDifference = result[result.Count - 1].Time - result[0].Time;
            actualDistance = result.CalculateTotalDistance();

            // ASSERT
            result.Count.Should().NotBe(0);
            timeDifference.Should().BeGreaterOrEqualTo(minimumTime);
            actualDistance.Should().BeGreaterOrEqualTo(requiredDistance);
        }
    }
}
