﻿using FluentAssertions;
using System;
using System.Collections.Generic;
using Spatial.Documents;
using Spatial.Helpers;
using Xunit;

namespace Spatial.Tests
{
    public class SplitTests : TestBase
    {
        // Same data in different formats
        private readonly GPXFile gpxTrackFile;

        public SplitTests()
        {
            gpxTrackFile = base.GetXMLData<GPXFile>("GPXFiles/HalfMarathon.gpx");
        }

        [Fact]
        public void Split_Track_By_TimeSpan_Middle()
        {
            // ARRANGE
            GeoFile gpxConversion = gpxTrackFile.ToGeoFile();
            List<List<GeoCoordinateExtended>> result;
            TimeSpan splitTime = new TimeSpan(1, 0, 0); // 1 hour split for a half marathon seems reasonables
            DateTime compareTime = gpxConversion.Routes[0].Points[0].Time.Add(splitTime); // Work out the actual time of the split

            // ACT
            result = gpxConversion.Routes[0].Points.Split(splitTime);
            DateTime part1EndTime = result[0][result[0].Count - 1].Time;
            DateTime part2StartTime = result[1][0].Time;

            // ASSERT
            part1EndTime.Ticks.Should().BeLessThan(compareTime.Ticks);
            part1EndTime.Ticks.Should().BeLessThan(part2StartTime.Ticks);
            part2StartTime.Ticks.Should().BeGreaterThan(compareTime.Ticks);
        }

        [Fact]
        public void Split_Track_By_TimeSpan_BeforeStart()
        {
            // ARRANGE
            GeoFile gpxConversion = gpxTrackFile.ToGeoFile();
            List<List<GeoCoordinateExtended>> result;
            TimeSpan splitTime = new TimeSpan(0, 0, 0); // Absolute start
            DateTime compareTime = gpxConversion.Routes[0].Points[0].Time.Add(splitTime); // Work out the actual time of the split

            // ACT
            result = gpxConversion.Routes[0].Points.Split(splitTime);
            Int32 part1Count = result[0].Count;
            Int32 part2Count = result[1].Count;

            // ASSERT
            part1Count.Should().Be(0);
            gpxConversion.Routes[0].Points.Count.Should().Be(part2Count);
        }

        [Fact]
        public void Split_Track_By_TimeSpan_AfterEnd()
        {
            // ARRANGE
            GeoFile gpxConversion = gpxTrackFile.ToGeoFile();
            List<List<GeoCoordinateExtended>> result;
            TimeSpan splitTime = new TimeSpan(5, 0, 0); // 5 hours should be enough to guarantee the split will be after the end
            DateTime compareTime = gpxConversion.Routes[0].Points[0].Time.Add(splitTime); // Work out the actual time of the split

            // ACT
            result = gpxConversion.Routes[0].Points.Split(splitTime);
            Int32 part1Count = result[0].Count;
            Int32 part2Count = result[1].Count;

            // ASSERT
            gpxConversion.Routes[0].Points.Count.Should().Be(part1Count);
            part2Count.Should().Be(0);
        }
    }
}
