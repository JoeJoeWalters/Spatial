using AwesomeAssertions;
using Spatial.Core.Documents;
using Spatial.Core.Helpers;
using Spatial.Core.Types;
using System;
using System.Collections.Generic;
using Xunit;

namespace Spatial.Core.Tests.Unit
{
    public class GeoTrackHelperTests : TestBase
    {
        public GeoTrackHelperTests() : base()
        {
        }

        [Fact]
        public void GeoCoordinates_WhenClones_Should_BeEquivalent()
        {
            // ARRANGE
            List<GeoCoordinateExtended> original = new List<GeoCoordinateExtended>();
            original.Add(
                new GeoCoordinateExtended
                {
                    Latitude = 1,
                    Longitude = 2,
                    Altitude = 3,
                    Speed = 6,
                    Time = DateTime.UtcNow
                });
            double roundedTo = 10D;

            // ACT
            List<GeoCoordinateExtended> rounded = original.Round(roundedTo);

            // ASSERT
            rounded.Count.Should().Be(original.Count);
        }
    }
}
