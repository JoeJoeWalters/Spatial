using Spatial.Core.Documents;

namespace Spatial.Tests
{
    public class SmoothTests : TestBase
    {
        private readonly GeoFile trackFile;

        public SmoothTests()
        {
            trackFile = base.GetXMLData<GPXFile>("GPXFiles/HalfMarathon.gpx").ToGeoFile();
        }
    }
}
