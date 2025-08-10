using AwesomeAssertions;
using Spatial.Core.Common;
using System;
using Xunit;

namespace Spatial.Core.Tests.Unit
{
    /// <summary>
    /// Tests for the geographical location class that is determined by latitude and longitude
    /// coordinates. May also include altitude, accuracy, speed, and course information.
    /// 
    /// 1:1 Port of the retired Microsoft Device class. Credit in this case to Geoffrey Huntley for the port and the test coverage
    /// https://github.com/ghuntley/geocoordinate/blob/master/src/GeoCoordinatePortableTests/GeoCoordinateTests.cs
    /// 
    /// However, converted to XUnit, added test fixtures and ported to fluent assertions for readability and commonality
    /// </summary>
    public class GeoCoordinateTests
    {
        public GeoCoordinateTests() : base()
        {
        }

        [Fact]
        public void GeoCoordinate_ConstructorWithDefaultValues_DoesNotThrow()
        {
            GeoCoordinate coordinate = new GeoCoordinate(double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN);
        }

        [Fact]
        public void GeoCoordinate_ConstructorWithParameters_ReturnsInstanceWithExpectedValues()
        {
            var latitude = 42D;
            var longitude = 44D;
            var altitude = 46D;
            var horizontalAccuracy = 48D;
            var verticalAccuracy = 50D;
            var speed = 52D;
            var course = 1.0D;
            var isUnknown = false;
            
            GeoCoordinate coordinate = new GeoCoordinate(latitude, longitude, altitude, horizontalAccuracy, verticalAccuracy, speed, course);

            latitude.Should().Be(coordinate.Latitude);
            longitude.Should().Be(coordinate.Longitude);
            altitude.Should().Be(coordinate.Altitude);
            horizontalAccuracy.Should().Be(coordinate.HorizontalAccuracy);
            verticalAccuracy.Should().Be(coordinate.VerticalAccuracy);
            speed.Should().Be(coordinate.Speed);
            course.Should().Be(coordinate.Course);
            isUnknown.Should().Be(coordinate.IsUnknown);
            
        }

        [Fact]
        public void GeoCoordinate_DefaultConstructor_ReturnsInstanceWithDefaultValues()
        {
            GeoCoordinate coordinate = new GeoCoordinate(double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN);
            coordinate.Altitude.Should().Be(double.NaN);
            coordinate.Course.Should().Be(double.NaN);
            coordinate.HorizontalAccuracy.Should().Be(double.NaN);
            coordinate.IsUnknown.Should().BeTrue();
            coordinate.Latitude.Should().Be(double.NaN);
            coordinate.Longitude.Should().Be(double.NaN);
            coordinate.Speed.Should().Be(double.NaN);
            coordinate.VerticalAccuracy.Should().Be(double.NaN);
            
        }

        [Fact]
        public void GeoCoordinate_EqualsOperatorWithNullParameters_DoesNotThrow()
        {
            GeoCoordinate first = null;
            GeoCoordinate second = null;
            (first == second).Should().BeTrue();

            first = new GeoCoordinate();
            (first == second).Should().BeFalse();

            first = null;
            second = new GeoCoordinate();
            (first == second).Should().BeFalse();
            
        }

        [Fact]
        public void GeoCoordinate_EqualsTwoInstancesWithDifferentValuesExceptLongitudeAndLatitude_ReturnsTrue()
        {
            var first = new GeoCoordinate(11, 12, 13, 14, 15, 16, 17);
            var second = new GeoCoordinate(11, 12, 14, 15, 16, 17, 18);

            first.Equals(second).Should().BeTrue();
            
        }

        [Fact]
        public void GeoCoordinate_EqualsTwoInstancesWithSameValues_ReturnsTrue()
        {
            var first = new GeoCoordinate(11, 12, 13, 14, 15, 16, 17);
            var second = new GeoCoordinate(11, 12, 13, 14, 15, 16, 17);

            first.Equals(second).Should().BeTrue();
            
        }

        [Fact]
        public void GeoCoordinate_EqualsWithOtherTypes_ReturnsFalse()
        {
            GeoCoordinate coordinate = new GeoCoordinate(double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN);
            var something = new int?(42);
            coordinate.Equals(something).Should().BeFalse();
            
        }

        [Fact]
        public void GeoCoordinate_GetDistanceTo_ReturnsExpectedDistance()
        {
            var start = new GeoCoordinate(1, 1);
            var end = new GeoCoordinate(5, 5);
            var distance = start.GetDistanceTo(end);
            var expected = 629060.759879635;
            var delta = distance - expected;

            (delta < 1e-8).Should().BeTrue();

        }

        [Theory]
        [InlineData(0.0D, 10.0D, 0.0D)]
        [InlineData(0.0D, -10.0D, 180.0D)]
        [InlineData(10.0D, 0.0D, 90.0D)]
        [InlineData(-10.0D, 0.0D, 270.0D)]
        public void GeoCoordinate_GetAngleTo_ReturnsExpectedAngle(double latitude, double longitude, double expectedResult)
        {
            var start = new GeoCoordinate(0, 0);
            var end = new GeoCoordinate(latitude, longitude);
            var angle = start.GetAngleTo(end);
            var delta = angle - expectedResult;

            (delta < 1e-8).Should().BeTrue();

        }

        [Fact]
        public void GeoCoordinate_GetDistanceToWithNaNCoordinates_ThrowsArgumentException()
        {
            Action act;

            act = () => new GeoCoordinate(double.NaN, 1).GetDistanceTo(new GeoCoordinate(5, 5));
            act.Should().Throw<ArgumentException>();

            act = () => new GeoCoordinate(1, double.NaN).GetDistanceTo(new GeoCoordinate(5, 5));
            act.Should().Throw<ArgumentException>();

            act = () => new GeoCoordinate(1, 1).GetDistanceTo(new GeoCoordinate(double.NaN, 5));
            act.Should().Throw<ArgumentException>();

            act = () => new GeoCoordinate(1, 1).GetDistanceTo(new GeoCoordinate(5, double.NaN));
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GeoCoordinate_GetHashCode_OnlyReactsOnLongitudeAndLatitude()
        {
            GeoCoordinate coordinate = new GeoCoordinate(double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN);

            coordinate.Latitude = 2;
            coordinate.Longitude = 3;
            var firstHash = coordinate.GetHashCode();

            coordinate.Altitude = 4;
            coordinate.Course = 5;
            coordinate.HorizontalAccuracy = 6;
            coordinate.Speed = 7;
            coordinate.VerticalAccuracy = 8;
            var secondHash = coordinate.GetHashCode();

            firstHash.Should().Be(secondHash);
            
        }

        [Fact]
        public void GeoCoordinate_GetHashCode_SwitchingLongitudeAndLatitudeReturnsSameHashCodes()
        {
            GeoCoordinate coordinate = new GeoCoordinate(double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN);

            coordinate.Latitude = 2;
            coordinate.Longitude = 3;
            var firstHash = coordinate.GetHashCode();

            coordinate.Latitude = 3;
            coordinate.Longitude = 2;
            var secondHash = coordinate.GetHashCode();

            firstHash.Should().Be(secondHash);
            
        }

        [Fact]
        public void GeoCoordinate_IsUnknownIfLongitudeAndLatitudeIsNaN_ReturnsTrue()
        {
            GeoCoordinate coordinate = new GeoCoordinate(double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN);

            coordinate.Longitude = 1;
            coordinate.Latitude = double.NaN;
            coordinate.IsUnknown.Should().BeFalse();

            coordinate.Longitude = double.NaN;
            coordinate.Latitude = 1;
            coordinate.IsUnknown.Should().BeFalse();

            coordinate.Longitude = double.NaN;
            coordinate.Latitude = double.NaN;
            coordinate.IsUnknown.Should().BeTrue();
            
        }

        [Fact]
        public void GeoCoordinate_NotEqualsOperatorWithNullParameters_DoesNotThrow()
        {
            GeoCoordinate first = null;
            GeoCoordinate second = null;
            (first != second).Should().BeFalse();

            first = new GeoCoordinate();
            (first != second).Should().BeTrue();

            first = null;
            second = new GeoCoordinate();
            (first != second).Should().BeTrue();
            
        }

        [Fact]
        public void GeoCoordinate_SetAltitude_ReturnsCorrectValue()
        {
            GeoCoordinate coordinate = new GeoCoordinate(double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN);

            coordinate.Altitude.Should().Be(double.NaN);

            coordinate.Altitude = 0;
            coordinate.Altitude.Should().Be(0);

            coordinate.Altitude = double.MinValue;
            coordinate.Altitude.Should().Be(double.MinValue);

            coordinate.Altitude = double.MaxValue;
            coordinate.Altitude.Should().Be(double.MaxValue);
            
        }

        [Fact]
        public void GeoCoordinate_SetCourse_ThrowsOnInvalidValues()
        {
            Action act;

            GeoCoordinate coordinate = new GeoCoordinate(double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN);

            act = () => { coordinate.Course = -0.1; };
            act.Should().Throw<ArgumentOutOfRangeException>();

            act = () => { coordinate.Course = 360.1; };
            act.Should().Throw<ArgumentOutOfRangeException>();

            act = () => new GeoCoordinate(double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, -0.1);
            act.Should().Throw<ArgumentOutOfRangeException>();

            act = () => new GeoCoordinate(double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, 360.1);
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void GeoCoordinate_SetHorizontalAccuracy_ThrowsOnInvalidValues()
        {
            Action act;

            GeoCoordinate coordinate = new GeoCoordinate(double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN);

            act = () => { coordinate.HorizontalAccuracy = -0.1; };
            act.Should().Throw<ArgumentOutOfRangeException>();

            act = () => new GeoCoordinate(double.NaN, double.NaN, double.NaN, -0.1, double.NaN, double.NaN, double.NaN);
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void GeoCoordinate_SetHorizontalAccuracyToZero_ReturnsNaNInProperty()
        {
            GeoCoordinate coordinate = new GeoCoordinate(double.NaN, double.NaN, double.NaN, 0, double.NaN, double.NaN, double.NaN);
            coordinate.HorizontalAccuracy.Should().Be(double.NaN);
        }

        [Fact]
        public void GeoCoordinate_SetLatitude_ThrowsOnInvalidValues()
        {
            Action act;

            GeoCoordinate coordinate = new GeoCoordinate(double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN);

            act = () => { coordinate.Latitude = 90.1; };
            act.Should().Throw<ArgumentOutOfRangeException>();

            act = () => { coordinate.Latitude = -90.1; };
            act.Should().Throw<ArgumentOutOfRangeException>();

            act = () => new GeoCoordinate(90.1, double.NaN);
            act.Should().Throw<ArgumentOutOfRangeException>();

            act = () => new GeoCoordinate(-90.1, double.NaN);
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void GeoCoordinate_SetLongitude_ThrowsOnInvalidValues()
        {
            Action act;

            GeoCoordinate coordinate = new GeoCoordinate(double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN);

            act = () => { coordinate.Longitude = 180.1; };
            act.Should().Throw<ArgumentOutOfRangeException>();

            act = () => { coordinate.Longitude = -180.1; };
            act.Should().Throw<ArgumentOutOfRangeException>();

            act = () => new GeoCoordinate(double.NaN, 180.1);
            act.Should().Throw<ArgumentOutOfRangeException>();

            act = () => new GeoCoordinate(double.NaN, -180.1);
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void GeoCoordinate_SetSpeed_ThrowsOnInvalidValues()
        {
            Action act;

            GeoCoordinate coordinate = new GeoCoordinate(double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN);

            act = () => { coordinate.Speed = -0.1; };
            act.Should().Throw<ArgumentOutOfRangeException>();

            act = () => new GeoCoordinate(double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, -1, double.NaN);
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void GeoCoordinate_SetVerticalAccuracy_ThrowsOnInvalidValues()
        {
            Action act;

            GeoCoordinate coordinate = new GeoCoordinate(double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN);

            act = () => { coordinate.VerticalAccuracy = -0.1; };
            act.Should().Throw<ArgumentOutOfRangeException>();

            act = () => new GeoCoordinate(double.NaN, double.NaN, double.NaN, double.NaN, -0.1, double.NaN, double.NaN);
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void GeoCoordinate_SetVerticalAccuracyToZero_ReturnsNaNInProperty()
        {
            GeoCoordinate coordinate = new GeoCoordinate(double.NaN, double.NaN, double.NaN, double.NaN, 0, double.NaN, double.NaN);
            coordinate.VerticalAccuracy.Should().Be(double.NaN);            
        }

        [Fact]
        public void GeoCoordinate_ToString_ReturnsLongitudeAndLatitude()
        {
            GeoCoordinate coordinate = new GeoCoordinate(double.NaN, double.NaN, double.NaN, double.NaN, 0, double.NaN, double.NaN);
            coordinate.ToString().Should().Be("Unknown");

            coordinate.Latitude = 1;
            coordinate.Longitude = double.NaN;
            coordinate.ToString().Should().Be("1, NaN");

            coordinate.Latitude = double.NaN;
            coordinate.Longitude = 1;
            coordinate.ToString().Should().Be("NaN, 1");
        }
    }
}
