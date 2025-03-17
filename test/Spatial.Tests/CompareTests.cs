using FluentAssertions;
using System;
using Spatial.Core.Documents;
using Spatial.Core.Helpers;
using Spatial.Core.Types;
using Xunit;

namespace Spatial.Tests
{
    public class CompareTests : TestBase
    {
        // Same data in different formats
        private readonly TCXFile tcxTrackFile;
        private readonly GPXFile gpxTrackFile;

        public CompareTests()
        {
            tcxTrackFile = base.GetXMLData<TCXFile>("TCXFiles/HalfMarathon.tcx");
            gpxTrackFile = base.GetXMLData<GPXFile>("GPXFiles/HalfMarathon.gpx");
        }

        [Fact]
        public void File_Format_Compare_Distance()
        {
            // ARRANGE
            GeoFile tcxConversion = tcxTrackFile.ToGeoFile();
            GeoFile gpxConversion = gpxTrackFile.ToGeoFile();

            // ACT
            Double tcxDistance = Math.Round(tcxConversion.Routes[0].Points.CalculateTotalDistance(), 0);
            Double gpxDIstance = Math.Round(gpxConversion.Routes[0].Points.CalculateTotalDistance(), 0);

            // ASSERT
            tcxDistance.Should().Be(gpxDIstance);
        }

        [Fact]
        public void File_Format_Compare_Actual_Time()
        {
            // ARRANGE
            GeoFile tcxConversion = tcxTrackFile.ToGeoFile();
            GeoFile gpxConversion = gpxTrackFile.ToGeoFile();
            TimeSpan tcxSpeed;
            TimeSpan gpxSpeed;

            // ACT
            tcxSpeed = tcxConversion.Routes[0].Points.TotalTime(TimeCalculationType.ActualTime);
            gpxSpeed = gpxConversion.Routes[0].Points.TotalTime(TimeCalculationType.ActualTime);

            // ASSERT
            tcxSpeed.TotalMinutes.Should().BeApproximately(gpxSpeed.TotalMinutes, 1.0);
        }

        [Fact]
        public void File_Format_Compare_Moving_Time()
        {
            // ARRANGE
            GeoFile tcxConversion = tcxTrackFile.ToGeoFile();
            GeoFile gpxConversion = gpxTrackFile.ToGeoFile();
            TimeSpan tcxSpeed;
            TimeSpan gpxSpeed;

            // ACT
            tcxSpeed = tcxConversion.Routes[0].Points.TotalTime(TimeCalculationType.MovingTime);
            gpxSpeed = gpxConversion.Routes[0].Points.TotalTime(TimeCalculationType.MovingTime);

            // ASSERT
            tcxSpeed.TotalMinutes.Should().BeApproximately(gpxSpeed.TotalMinutes, 1.0);
        }
    }
}
