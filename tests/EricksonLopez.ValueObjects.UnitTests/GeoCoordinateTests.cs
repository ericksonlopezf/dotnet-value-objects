// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

public sealed class GeoCoordinateTests
{
    [Fact]
    public void Create_ValidCoordinates_Succeeds()
    {
        // Santo Domingo: 18.486058, -69.931212
        var result = GeoCoordinate.Create(18.486058, -69.931212);

        result.IsSuccess.Should().BeTrue();
        result.Value.Latitude.Should().Be(18.486058);
        result.Value.Longitude.Should().Be(-69.931212);
    }

    [Theory]
    [InlineData(-90.1, 0.0)]
    [InlineData(90.1, 0.0)]
    [InlineData(double.NaN, 0.0)]
    public void Create_InvalidLatitude_ReturnsLatitudeError(double lat, double lon)
    {
        var result = GeoCoordinate.Create(lat, lon);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("GeoCoordinate.InvalidLatitude");
    }

    [Theory]
    [InlineData(0.0, -180.1)]
    [InlineData(0.0, 180.1)]
    [InlineData(0.0, double.NaN)]
    public void Create_InvalidLongitude_ReturnsLongitudeError(double lat, double lon)
    {
        var result = GeoCoordinate.Create(lat, lon);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("GeoCoordinate.InvalidLongitude");
    }

    [Fact]
    public void DistanceToKilometers_CalculatesHaversineCorrectly()
    {
        // Santo Domingo (18.486058, -69.931212) to Santiago de los Caballeros (19.451700, -70.697030)
        // Approximate distance ~130-140 km
        var sd = GeoCoordinate.Create(18.486058, -69.931212).Value;
        var stgo = GeoCoordinate.Create(19.451700, -70.697030).Value;

        double distanceKm = sd.DistanceToKilometers(stgo);
        distanceKm.Should().BeGreaterThan(130.0);
        distanceKm.Should().BeLessThan(145.0);

        double distanceM = sd.DistanceToMeters(stgo);
        distanceM.Should().BeApproximately(distanceKm * 1000.0, 0.001);
    }

    [Fact]
    public void Constructor_MustNotBePubliclyAccessible()
    {
        var constructors = typeof(GeoCoordinate).GetConstructors(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        constructors.Should().BeEmpty("GeoCoordinate must only be instantiated via static factory Create()");
    }

    [Fact]
    public void ToString_And_Parse_RoundTripsCorrectly()
    {
        var original = GeoCoordinate.Create(18.486058, -69.931212).Value;
        string str = original.ToString();
        str.Should().Be("18.486058, -69.931212");

        var parsed = GeoCoordinate.Parse(str);
        parsed.Latitude.Should().BeApproximately(original.Latitude, 0.000001);
        parsed.Longitude.Should().BeApproximately(original.Longitude, 0.000001);

        var spanParsed = GeoCoordinate.Parse(str.AsSpan());
        spanParsed.Latitude.Should().BeApproximately(original.Latitude, 0.000001);
        spanParsed.Longitude.Should().BeApproximately(original.Longitude, 0.000001);
    }

    [Fact]
    public void TryParse_StringAndSpan_ReturnsTrueForValidCoordinates()
    {
        GeoCoordinate.TryParse("  18.486058 , -69.931212  ", null, out var r1).Should().BeTrue();
        r1.Latitude.Should().BeApproximately(18.486058, 0.000001);
        r1.Longitude.Should().BeApproximately(-69.931212, 0.000001);

        GeoCoordinate.TryParse("0.0, 0.0".AsSpan(), null, out var r2).Should().BeTrue();
        r2.Latitude.Should().Be(0.0);
        r2.Longitude.Should().Be(0.0);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("invalid")]
    [InlineData("18.486058")]
    [InlineData("18.486058, ")]
    [InlineData("100.0, 0.0")]
    [InlineData("0.0, 200.0")]
    public void TryParse_InvalidInput_ReturnsFalse(string? input)
    {
        GeoCoordinate.TryParse(input, null, out var result).Should().BeFalse();
        result.Should().Be(default(GeoCoordinate));
    }

    [Fact]
    public void Parse_InvalidInput_ThrowsExpectedExceptions()
    {
        Action nullAct = () => GeoCoordinate.Parse((string)null!);
        nullAct.Should().Throw<ArgumentNullException>();

        Action invalidFormat = () => GeoCoordinate.Parse("invalid-coordinate");
        invalidFormat.Should().Throw<FormatException>()
            .WithMessage("*was not recognized as a valid GeoCoordinate*");

        Action outOfRange = () => GeoCoordinate.Parse("95.0, 0.0");
        outOfRange.Should().Throw<FormatException>();

        Action invalidSpan = () => GeoCoordinate.Parse("invalid".AsSpan());
        invalidSpan.Should().Throw<FormatException>();
    }

    [Fact]
    public void PrivateConstructor_InvalidValues_ThrowsArgumentOutOfRangeException()
    {
        var ctor = typeof(GeoCoordinate).GetConstructors(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)[0];

        Action actLat = () =>
        {
            try { ctor.Invoke(new object[] { 100.0, 0.0 }); }
            catch (System.Reflection.TargetInvocationException ex) { throw ex.InnerException!; }
        };
        actLat.Should().Throw<ArgumentOutOfRangeException>();

        Action actLon = () =>
        {
            try { ctor.Invoke(new object[] { 0.0, 200.0 }); }
            catch (System.Reflection.TargetInvocationException ex) { throw ex.InnerException!; }
        };
        actLon.Should().Throw<ArgumentOutOfRangeException>();
    }
}
