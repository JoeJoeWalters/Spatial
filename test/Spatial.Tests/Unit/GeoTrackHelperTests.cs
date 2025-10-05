using AwesomeAssertions;
using Spatial.Core.Documents;
using Spatial.Core.Helpers;
using Spatial.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [Theory]
        [InlineData(0, -1)]
        [InlineData(1, 0)]
        public void GeoCoordinates_WhenInfills_Should_BePointCount(int position, int diff)
        {
            // ARRANGE
            GeoFile geoFile = tcxTrackFile.ToGeoFile();
            geoFile.Routes[0].Points[position] = new GeoCoordinateExtended() { BadCoordinate = true }; // Add a bad coordinate

            // ACT
            var infilled = geoFile.Routes[0].Points.InfillPositions();

            // ASSERT
            infilled.Count.Should().Be(geoFile.Routes[0].Points.Count + diff);

        }
    }
}
