using Spatial.Core.Documents;

namespace Spatial.Core.Tests.Unit
{
    public class SmoothTests : TestBase
    {
        private readonly GeoFile trackFile;

        public SmoothTests()
        {
            trackFile = GetXMLData<GPXFile>("Data/GPXFiles/HalfMarathon.gpx").ToGeoFile();
        }
    }
}
