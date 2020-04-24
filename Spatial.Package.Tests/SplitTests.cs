﻿using System;
using System.Collections.Generic;
using System.Linq;
using TNDStudios.Spatial.Documents;
using TNDStudios.Spatial.Helpers;
using TNDStudios.Spatial.Types;
using Xunit;

namespace TNDStudios.Spatial.Tests
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
        public void Split_Track_By_TimeSpan()
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
            Assert.True(part1EndTime < compareTime);
            Assert.True(part1EndTime < part2StartTime);
            Assert.True(part2StartTime > compareTime); // Seems obvious as the last two compared it anyway in a different way but ..
        }
    }
}
