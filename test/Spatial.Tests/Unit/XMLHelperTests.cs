using AwesomeAssertions;
using Spatial.Core.Documents;
using System;
using Xunit;

namespace Spatial.Core.Tests.Unit
{
    public class XMLHelperTests : TestBase
    {
        public XMLHelperTests() { }

        [Fact]
        public void OnDeserialise_WithValidXML_Should_ThrowException()
        {
            // ARRANGE

            // ACT
            Action act = () => GetXMLData<GPXFile>("Data/GPXFiles/HalfMarathon.gpx");

            // ASSERT
            act.Should().NotThrow<Exception>();
        }

        [Fact]
        public void OnDeserialise_WithBadXML_Should_ThrowException()
        {
            // ARRANGE

            // ACT
            Action act = () => GetXMLData<GPXFile>("Data/GPXFiles/Bad.gpx");

            // ASSERT
            act.Should().Throw<Exception>();
        }
    }
}
