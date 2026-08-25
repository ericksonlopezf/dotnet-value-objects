// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Argentina;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Argentina.UnitTests;

public sealed class PointOfSaleTests
{
    [Theory]
    [InlineData(1, "00001")]
    [InlineData(42, "00042")]
    [InlineData(99999, "99999")]
    public void Create_ValidInt_FormatsCorrectly(int value, string expectedFormatted)
    {
        var pos = PointOfSale.Create(value).Value;

        pos.Value.Should().Be(value);
        pos.Formatted.Should().Be(expectedFormatted);
        pos.ToString().Should().Be(expectedFormatted);
    }

    [Theory]
    [InlineData("1", 1)]
    [InlineData("00042", 42)]
    [InlineData("99999", 99999)]
    public void Create_ValidString_Succeeds(string input, int expectedValue)
    {
        var result = PointOfSale.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(expectedValue);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(100_000)]
    public void Create_OutOfRange_ReturnsOutOfRangeError(int invalid)
    {
        var result = PointOfSale.Create(invalid);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("PointOfSale.OutOfRange");
    }

    [Theory]
    [InlineData("ABC")]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_InvalidFormat_ReturnsInvalidFormatError(string invalid)
    {
        var result = PointOfSale.Create(invalid);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("PointOfSale.InvalidFormat");
    }

    [Fact]
    public void PointOfSale_DefaultState_ParseAndTryParse()
    {
        var parsed1 = PointOfSale.Parse("00001", System.Globalization.CultureInfo.InvariantCulture);
        parsed1.Value.Should().Be(1);

        var parsed2 = PointOfSale.Parse("00001".AsSpan(), System.Globalization.CultureInfo.InvariantCulture);
        parsed2.Value.Should().Be(1);

        PointOfSale.TryParse("00001", null, out var tryRes1).Should().BeTrue();
        tryRes1.Value.Should().Be(1);

        PointOfSale.TryParse("00001".AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Value.Should().Be(1);

        Action invalidParseStr = () => PointOfSale.Parse("invalid", System.Globalization.CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid PointOfSale: 'invalid'.");

        Action invalidParseSpan = () => PointOfSale.Parse("invalid".AsSpan(), System.Globalization.CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid PointOfSale: 'invalid'.");

        PointOfSale.TryParse("invalid", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(PointOfSale));

        PointOfSale.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(PointOfSale));

        PointOfSale.TryParse("invalid".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(PointOfSale));

        PointOfSale.Create((string?)null).IsFailure.Should().BeTrue();
    }

    [Fact]
    public void PointOfSale_ComparisonsAndOperators_Exhaustive()
    {
        var a = PointOfSale.Create(1).Value;
        var aCopy = PointOfSale.Create(1).Value;
        var b = PointOfSale.Create(2).Value;

        a.ShouldSatisfyEqualityContract(aCopy, b, (x, y) => x == y, (x, y) => x != y);
        a.ShouldSatisfyComparisonContract(aCopy, b,
            (x, y) => x < y,
            (x, y) => x <= y,
            (x, y) => x > y,
            (x, y) => x >= y);
    }
}




