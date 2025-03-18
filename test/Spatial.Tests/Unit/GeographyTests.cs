using FluentAssertions;
using Spatial.Core.Common;
using Spatial.Core.Documents;
using Spatial.Core.Helpers;
using Spatial.Core.Types;
using System;
using System.Collections.Generic;
using Xunit;

namespace Spatial.Core.Tests.Unit
{
	public class GeographyTests : TestBase
	{
		private readonly GeoFile geoTrackFile;
		private readonly GeoFile geoFile;
		private readonly GeoFile geoCompare1;
		private readonly GeoFile geoCompare2;

		public GeographyTests()
		{
			geoTrackFile = GetXMLData<GPXFile>("Data/GPXFiles/GPXRouteOnly.gpx").ToGeoFile();
			geoFile = GetXMLData<GPXFile>("Data/GPXFiles/HalfMarathon.gpx").ToGeoFile();
			geoCompare1 = GetXMLData<GPXFile>("Data/GPXFiles/Compare1.gpx").ToGeoFile();
			geoCompare2 = GetXMLData<GPXFile>("Data/GPXFiles/Compare2.gpx").ToGeoFile();
		}

		[Fact]
		public void Calculate_Distance()
		{
			// ARRANGE
			List<GeoCoordinateExtended> points = geoFile.Routes[0].Points;

            // ACT
            double distance = Math.Round(points.CalculateTotalDistance() / 1000, 2);

			// ASSERT
			distance.Should().Be(HalfMarathonDistance);
		}

		[Fact]
		public void Calculate_Actual_Time()
		{
			// ARRANGE
			List<GeoCoordinateExtended> points = geoFile.Routes[0].Points;

			// ACT]
			TimeSpan calculatedSpan = points.TotalTime(TimeCalculationType.ActualTime);

			// ASSERT
			calculatedSpan.TotalMinutes.Should().BeApproximately(HalfMarathonTotalMinutes, 1);
		}

		[Fact]
		public void Calculate_Moving_Time()
		{
			// ARRANGE
			List<GeoCoordinateExtended> points = geoFile.Routes[0].Points;

			// ACT]
			TimeSpan calculatedSpan = points.TotalTime(TimeCalculationType.MovingTime);

			// ASSERT
			calculatedSpan.TotalMinutes.Should().BeApproximately(HalfMarathonMovingTime, 1);
		}

		[Fact]
		public void Track_Array_Mapping_Success()
		{
            // ARRANGE

            // ACT
            int trackCount = (int)geoTrackFile?.Routes?.Count;
            int trackpointCount = (int)geoTrackFile?.Routes[0].Points.Count;

			// ASSERT
			trackCount.Should().Be(1);
			trackpointCount.Should().BeGreaterThan(1);
		}

		[Fact]
		public void Track_Array_To_Coords()
		{
            // ARRANGE

            // ACT
            int coordCount = geoTrackFile.Routes[0].Points.Count;

			// ASSERT
			coordCount.Should().Be(523);
		}

		[Fact]
		public void Track_Compare_Positive()
		{
			// ARRANGE
			List<GeoCoordinateExtended> compare1 = geoCompare1.Routes[0].Points;
			List<GeoCoordinateExtended> compare2 = geoCompare1.Routes[0].Points;

            // ACT
            double score = compare1.Compare(compare2, ActivityType.Running);

			// ASSERT
			score.Should().Be(1.0D); // Should be a perfect match
		}

		[Fact]
		public void Track_Compare_Negative()
		{
			// ARRANGE
			List<GeoCoordinateExtended> compare1 = geoCompare1.Routes[0].Points;
			List<GeoCoordinateExtended> compare2 = geoTrackFile.Routes[0].Points;

            // ACT
            double score = compare1.Compare(compare2, ActivityType.Running);

			// ASSERT
			score.Should().Be(0.0D); // Should be a total mismatch
		}

		[Fact]
		public void Track_Compare_Near()
		{
			// ARRANGE
			List<GeoCoordinateExtended> compare1 = geoCompare1.Routes[0].Points;
			List<GeoCoordinateExtended> compare2 = geoCompare2.Routes[0].Points;

            // ACT
            double score = compare1.Compare(compare2, ActivityType.Running);

			// ASSERT
			score.Should().BeGreaterThan(0.75); // Should be a partial match
		}

		[Fact]
		public void Round_Coordinates_By_Meters()
		{
			// ARRANGE
			GeoCoordinateExtended source = geoFile.Routes[0].Points[0];
			GeoCoordinateExtended compareTo = source.Clone();
            double roundingMeters = 2D;

			// ACT
			compareTo.Round(roundingMeters); // Round second coordinate to 2 meter grid point
            double distance = compareTo.GetDistanceTo(source); // Calculate the distance in meters 
            double hypotenuse = Math.Sqrt(Math.Pow(roundingMeters, 2) + Math.Pow(roundingMeters, 2));

			// ASSERT
			hypotenuse.Should().BeGreaterThan(distance); // Should be smaller than the hypotenuse
		}

		[Fact]
		public void InterpolateBetweenToPoints_Should_GiveCorrectDistance()
		{
			// ARRANGE
			GeoCoordinateExtended from = new GeoCoordinateExtended(52.0166763, -0.6209997, 0);
			GeoCoordinateExtended to = new GeoCoordinateExtended(52.009572, -0.4573654, 1000);

			// ACT
			double actualDistance = from.GetDistanceTo(to);
			GeoCoordinateExtended interpolated = from.Interpolate(to, actualDistance / 2);

			// ASSERT
			double distance = from.GetDistanceTo(interpolated);
			interpolated.Altitude.Should().BeApproximately(to.Altitude / 2, 0.1);
			distance.Should().BeApproximately(actualDistance / 2, 1); // Should be the same as requested distance (with a small margin of error due to curvature of the earth and altitude)
		}
	}
}
