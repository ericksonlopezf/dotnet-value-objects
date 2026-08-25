// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="Barcode"/> Domain Primitive.
/// </summary>
public sealed class BarcodeTests
{
    [Fact]
    public void Barcode_Valid_NormalizesUppercase()
    {
        var barcode = Barcode.Create("  7501031311309-a  ").Value;
        barcode.Value.Should().Be("7501031311309-A");
        barcode.ToString().Should().Be("7501031311309-A");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("12")] // < 3 chars
    public void Barcode_Invalid_ShouldFail(string? invalid)
    {
        var result = Barcode.Create(invalid);
        result.IsFailure.Should().BeTrue();
        if (string.IsNullOrWhiteSpace(invalid)) result.Error.Code.Should().Be("Barcode.Required");
        else if (invalid == "12") result.Error.Code.Should().Be("Barcode.TooShort");
    }

    [Fact]
    public void Barcode_BoundariesAndPattern_ShouldFail()
    {
        Barcode.Create(new string('1', 81)).Error.Code.Should().Be("Barcode.TooLong");
        var invalid = Barcode.Create("123@#$");
        invalid.Error.Code.Should().Be("Barcode.InvalidFormat");
        invalid.Error.Description.Should().Be("Barcode can contain uppercase letters, digits, spaces, periods, or hyphens.");
    }
}




