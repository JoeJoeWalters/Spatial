using FluentAssertions;
using System;
using Spatial.Core.Documents;
using Xunit;
using Spatial.Core.Helpers;

namespace Spatial.Tests
{
    public class TCXTests : TestBase
    {
        private readonly TCXFile tcxTrackFile;

        public TCXTests()
        {
            tcxTrackFile = base.GetXMLData<TCXFile>("TCXFiles/HalfMarathon.tcx");
        }

        [Fact]
        public void Track_Compare_ToGeoFile_Conversion()
        {
            // ARRANGE
            Int32 origionalCount = 0;
            Int32 transformedCount = 0;

            // ACT
            origionalCount = tcxTrackFile.Activities.Activity[0].ToCoords().Count; // Count of origional
            transformedCount = tcxTrackFile.ToGeoFile().Routes[0].Points.Count; // Do conversion and count

            // ASSERT
            origionalCount.Should().Be(transformedCount);
        }

        [Fact]
        public void Track_Compare_FromGeoFile_Conversion()
        {
            // ARRANGE
            Int32 transformedCount = 0;
            GeoFile geoFile = tcxTrackFile.ToGeoFile();
            Int32 origionalCount = geoFile.Routes[0].Points.Count;
            TCXFile tcxFile = new TCXFile();

            // ACT
            Boolean success = tcxFile.FromGeoFile(geoFile);
            Double totalDistance = geoFile.Routes[0].Points.CalculateTotalDistance();
            transformedCount = tcxFile.Activities.Activity[0].Laps[0].Track.TrackPoints.Count; // Count of transformed track

            // ASSERT
            success.Should().BeTrue();
            tcxFile.Activities.Activity.Should().NotBeEmpty();
            tcxFile.Activities.Activity[0].Laps.Should().NotBeEmpty();
            tcxFile.Activities.Activity[0].Laps[0].DistanceMeters.Should().Be(totalDistance);
            origionalCount.Should().Be(transformedCount);
        }
    }
}
