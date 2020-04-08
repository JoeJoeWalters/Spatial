using System;
using TNDStudios.Spatial.Documents;
using Xunit;

namespace TNDStudios.Spatial.Tests
{
    public class TCXTests : TestBase
    {
        private readonly TCXFile tcxTrackFile;

        public TCXTests()
        {
            tcxTrackFile = base.GetXMLData<TCXFile>("TCXFiles/HalfMarathon.tcx");
        }

        [Fact]
        public void Track_Compare_Conversion()
        {
            // ARRANGE
            Int32 origionalCount = 0;
            Int32 transformedCount = 0;

            // ACT
            origionalCount = tcxTrackFile.Activities.Activity[0].ToCoords().Count; // Count of origional
            transformedCount = tcxTrackFile.ToGeoFile().Routes[0].Points.Count; // Do conversion and count

            // ASSERT
            Assert.Equal(origionalCount, transformedCount);
        }
    }
}
