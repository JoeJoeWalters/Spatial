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

            // ACT
            result = trackPoints.Fastest(1000.0, true); // Get the fastest kilometer

            // ASSERT
            result.Count.Should().NotBe(0);
        }
    }
}
