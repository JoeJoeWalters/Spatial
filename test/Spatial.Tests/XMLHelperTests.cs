using Spatial.Core.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace Spatial.Tests
{
    public class XMLHelperTests : TestBase
    {
        public XMLHelperTests() { }

        [Fact]
        public void OnDeserialise_WithValidXML_Should_ThrowException()
        {
            // ARRANGE

            // ACT
            Action act = () => base.GetXMLData<GPXFile>("GPXFiles/HalfMarathon.gpx");

            // ASSERT
            act.Should().NotThrow<Exception>();
        }

        [Fact]
        public void OnDeserialise_WithBadXML_Should_ThrowException()
        {
            // ARRANGE

            // ACT
            Action act = () => base.GetXMLData<GPXFile>("GPXFiles/Bad.gpx");

            // ASSERT
            act.Should().Throw<Exception>();
        }
    }
}
