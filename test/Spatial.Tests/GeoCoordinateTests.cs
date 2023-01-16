﻿using FluentAssertions;
using Spatial.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Spatial.Tests
{
    /// <summary>
    /// Tests for the geographical location class that is determined by latitude and longitude
    /// coordinates. May also include altitude, accuracy, speed, and course information.
    /// 
    /// 1:1 Port of the retired Microsoft Device class. Credit in this case to Geoffrey Huntley for the port and the test coverage
    /// https://github.com/ghuntley/geocoordinate/blob/master/src/GeoCoordinatePortableTests/GeoCoordinateTests.cs
    /// 
    /// However, converted to XUnit, added test fixture and ported to fluent assertions for readability and commonality
    /// </summary>
    public class GeoCoordinateTestFixture : IDisposable
    {
        public GeoCoordinate UnitUnderTest;

        public GeoCoordinateTestFixture()
        {
            UnitUnderTest = new GeoCoordinate();
        }

        public void Dispose()
        {
            // throw new NotImplementedException();
        }
    }

    public class GeoCoordinateTests
    {
        //GeoCoordinateTestFixture _fixture;

        public GeoCoordinateTests()
        {
        }

        [Fact]
        public void GeoCoordinate_ConstructorWithDefaultValues_DoesNotThrow()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN);
            
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
            var course = 54D;
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
            GeoCoordinate coordinate = new GeoCoordinate(Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN);
            coordinate.Altitude.Should().Be(Double.NaN);
            coordinate.Course.Should().Be(Double.NaN);
            coordinate.HorizontalAccuracy.Should().Be(Double.NaN);
            coordinate.IsUnknown.Should().BeTrue();
            coordinate.Latitude.Should().Be(Double.NaN);
            coordinate.Longitude.Should().Be(Double.NaN);
            coordinate.Speed.Should().Be(Double.NaN);
            coordinate.VerticalAccuracy.Should().Be(Double.NaN);
            
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
            GeoCoordinate coordinate = new GeoCoordinate(Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN);
            var something = new Nullable<Int32>(42);
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

        [Fact]
        public void GeoCoordinate_GetDistanceToWithNaNCoordinates_ThrowsArgumentException()
        {
            Action act;

            act = () => new GeoCoordinate(Double.NaN, 1).GetDistanceTo(new GeoCoordinate(5, 5));
            act.Should().Throw<ArgumentException>();

            act = () => new GeoCoordinate(1, Double.NaN).GetDistanceTo(new GeoCoordinate(5, 5));
            act.Should().Throw<ArgumentException>();

            act = () => new GeoCoordinate(1, 1).GetDistanceTo(new GeoCoordinate(Double.NaN, 5));
            act.Should().Throw<ArgumentException>();

            act = () => new GeoCoordinate(1, 1).GetDistanceTo(new GeoCoordinate(5, Double.NaN));
            act.Should().Throw<ArgumentException>();
            


            /*
            Assertations.Assert.Throws<ArgumentException>(() => new GeoCoordinate(Double.NaN, 1).GetDistanceTo(new GeoCoordinate(5, 5)));
            Assertations.Assert.Throws<ArgumentException>(() => new GeoCoordinate(1, Double.NaN).GetDistanceTo(new GeoCoordinate(5, 5)));
            Assertations.Assert.Throws<ArgumentException>(() => new GeoCoordinate(1, 1).GetDistanceTo(new GeoCoordinate(Double.NaN, 5)));
            Assertations.Assert.Throws<ArgumentException>(() => new GeoCoordinate(1, 1).GetDistanceTo(new GeoCoordinate(5, Double.NaN)));
            */
        }

        [Fact]
        public void GeoCoordinate_GetHashCode_OnlyReactsOnLongitudeAndLatitude()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN);

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
            GeoCoordinate coordinate = new GeoCoordinate(Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN);

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
            GeoCoordinate coordinate = new GeoCoordinate(Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN);

            coordinate.Longitude = 1;
            coordinate.Latitude = Double.NaN;
            coordinate.IsUnknown.Should().BeFalse();

            coordinate.Longitude = Double.NaN;
            coordinate.Latitude = 1;
            coordinate.IsUnknown.Should().BeFalse();

            coordinate.Longitude = Double.NaN;
            coordinate.Latitude = Double.NaN;
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
            GeoCoordinate coordinate = new GeoCoordinate(Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN);

            coordinate.Altitude.Should().Be(Double.NaN);

            coordinate.Altitude = 0;
            coordinate.Altitude.Should().Be(0);

            coordinate.Altitude = Double.MinValue;
            coordinate.Altitude.Should().Be(Double.MinValue);

            coordinate.Altitude = Double.MaxValue;
            coordinate.Altitude.Should().Be(Double.MaxValue);
            
        }

        /*
        [Fact]
        public void GeoCoordinate_SetCourse_ThrowsOnInvalidValues()
        {
            Assertations.Assert.Throws<ArgumentOutOfRangeException>(() => coordinate.Course = -0.1);
            Assertations.Assert.Throws<ArgumentOutOfRangeException>(() => coordinate.Course = 360.1);
            Assertations.Assert.Throws<ArgumentOutOfRangeException>(() => new GeoCoordinate(Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, -0.1));
            Assertations.Assert.Throws<ArgumentOutOfRangeException>(() => new GeoCoordinate(Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, 360.1));
        }

        [Fact]
        public void GeoCoordinate_SetHorizontalAccuracy_ThrowsOnInvalidValues()
        {
            Assertations.Assert.Throws<ArgumentOutOfRangeException>(() => coordinate.HorizontalAccuracy = -0.1);
            Assertations.Assert.Throws<ArgumentOutOfRangeException>(() => new GeoCoordinate(Double.NaN, Double.NaN, Double.NaN, -0.1, Double.NaN, Double.NaN, Double.NaN));
        }

        [Fact]
        public void GeoCoordinate_SetHorizontalAccuracyToZero_ReturnsNaNInProperty()
        {
            coordinate = new GeoCoordinate(Double.NaN, Double.NaN, Double.NaN, 0, Double.NaN, Double.NaN, Double.NaN);
            coordinate.HorizontalAccuracy.Should().Be(Double.NaN);
        }

        [Fact]
        public void GeoCoordinate_SetLatitude_ThrowsOnInvalidValues()
        {
            Assertations.Assert.Throws<ArgumentOutOfRangeException>(() => coordinate.Latitude = 90.1);
            Assertations.Assert.Throws<ArgumentOutOfRangeException>(() => coordinate.Latitude = -90.1);
            Assertations.Assert.Throws<ArgumentOutOfRangeException>(() => new GeoCoordinate(90.1, Double.NaN));
            Assertations.Assert.Throws<ArgumentOutOfRangeException>(() => new GeoCoordinate(-90.1, Double.NaN));
        }

        [Fact]
        public void GeoCoordinate_SetLongitude_ThrowsOnInvalidValues()
        {
            Assertations.Assert.Throws<ArgumentOutOfRangeException>(() => coordinate.Longitude = 180.1);
            Assertations.Assert.Throws<ArgumentOutOfRangeException>(() => coordinate.Longitude = -180.1);
            Assertations.Assert.Throws<ArgumentOutOfRangeException>(() => new GeoCoordinate(Double.NaN, 180.1));
            Assertations.Assert.Throws<ArgumentOutOfRangeException>(() => new GeoCoordinate(Double.NaN, -180.1));
        }

        [Fact]
        public void GeoCoordinate_SetSpeed_ThrowsOnInvalidValues()
        {
            Assertations.Assert.Throws<ArgumentOutOfRangeException>(() => coordinate.Speed = -0.1);
            Assertations.Assert.Throws<ArgumentOutOfRangeException>(() => new GeoCoordinate(Double.NaN, Double.NaN, Double.NaN, Double.NaN, Double.NaN, -1, Double.NaN));
        }

        [Fact]
        public void GeoCoordinate_SetVerticalAccuracy_ThrowsOnInvalidValues()
        {
            Assertations.Assert.Throws<ArgumentOutOfRangeException>(() => coordinate.VerticalAccuracy = -0.1);
            Assertations.Assert.Throws<ArgumentOutOfRangeException>(() => new GeoCoordinate(Double.NaN, Double.NaN, Double.NaN, Double.NaN, -0.1, Double.NaN, Double.NaN));
        }
        */

        [Fact]
        public void GeoCoordinate_SetVerticalAccuracyToZero_ReturnsNaNInProperty()
        {
            GeoCoordinate coordinate = new GeoCoordinate(Double.NaN, Double.NaN, Double.NaN, Double.NaN, 0, Double.NaN, Double.NaN);
            coordinate.VerticalAccuracy.Should().Be(Double.NaN);
            
        }

        /*
        [Fact]
        public void GeoCoordinate_ToString_ReturnsLongitudeAndLatitude()
        {
            coordinate.ToString().Should().Be("Unknown");

            coordinate.Latitude = 1;
            coordinate.Longitude = Double.NaN;
            Assert.AreEqual("1, NaN", coordinate.ToString());

            coordinate.Latitude = Double.NaN;
            coordinate.Longitude = 1;
            Assert.AreEqual("NaN, 1", coordinate.ToString());
        }
        */
    }
}
