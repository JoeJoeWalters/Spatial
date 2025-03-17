using FluentAssertions;
using System;
using System.Collections.Generic;
using Spatial.Core.Documents;
using Spatial.Core.Helpers;
using Xunit;

namespace Spatial.Tests
{
    public class MergeTests : TestBase
    {
        // Same data in different formats
        private readonly GeoFile trackFile;
        private readonly List<GeoCoordinateExtended> part1;
        private readonly List<GeoCoordinateExtended> part2;

        public MergeTests()
        {
            // Split a track file in two (testing for this is done elesewhere) and then 
            // keep the origional so we can compare it back to the origional
            trackFile = base.GetXMLData<GPXFile>("GPXFiles/HalfMarathon.gpx").ToGeoFile();
            List<List<GeoCoordinateExtended>> splits = trackFile.Routes[0].Points.Split(new TimeSpan(1, 0, 0));
            part1 = splits[0];
            part2 = splits[1];
        }

        [Fact]
        public void Merge_Track()
        {
            // ARRANGE
            List<GeoCoordinateExtended> result;
            List<GeoCoordinateExtended> trackPoints = trackFile.Routes[0].Points;

            // ACT
            result = new List<List<GeoCoordinateExtended>>() { part1, part2 }.Merge();

            // ASSERT
            trackPoints.Count.Should().Be(result.Count); // Same total as origional
            trackPoints[0].Time.Should().Be(result[0].Time); // Start point is the right time
            trackPoints[trackPoints.Count - 1].Time.Should().Be(result[result.Count - 1].Time); // End point is the right time
        }
    }
}
