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
            TimeSpan minimumTime = new TimeSpan(TimeSpan.TicksPerSecond * 60 * 4); // 4 minutes (I can't run faster than that)

            // ACT
            result = trackPoints.Fastest(1000.0, true); // Get the fastest kilometer
            timeDifference = result[result.Count - 1].Time - result[0].Time;

            // ASSERT
            result.Count.Should().NotBe(0);
            timeDifference.Should().BeGreaterOrEqualTo(minimumTime);
        }
    }
}
