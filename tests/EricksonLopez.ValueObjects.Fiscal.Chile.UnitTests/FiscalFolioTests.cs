// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Chile;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Chile.UnitTests;

public sealed class FiscalFolioTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(10520)]
    [InlineData(int.MaxValue)]
    public void Create_ValidInt_Succeeds(int value)
    {
        var result = FiscalFolio.Create(value);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(value);
        result.Value.ToString().Should().Be(value.ToString(CultureInfo.InvariantCulture));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-99999)]
    [InlineData(int.MinValue)]
    public void Create_IntOutOfRange_ReturnsError(int value)
    {
        var result = FiscalFolio.Create(value);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("FiscalFolio.OutOfRange");
    }

    [Theory]
    [InlineData("1", 1)]
    [InlineData("10520", 10520)]
    [InlineData("  10520  ", 10520)]
    [InlineData("2147483647", 2147483647)]
    public void Create_ValidString_Succeeds(string input, int expected)
    {
        var result = FiscalFolio.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(expected);
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("10.5")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_InvalidFormat_ReturnsError(string? input)
    {
        var result = FiscalFolio.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("FiscalFolio.InvalidFormat");
    }

    [Theory]
    [InlineData("0")]
    [InlineData("-1")]
    [InlineData("-500")]
    public void Create_StringOutOfRange_ReturnsError(string input)
    {
        var result = FiscalFolio.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("FiscalFolio.OutOfRange");
    }

    [Fact]
    public void FiscalFolio_DefaultState_ComparisonOperators()
    {
        var f1 = FiscalFolio.Create(100).Value;
        var f2 = FiscalFolio.Create(200).Value;
        var f1Clone = FiscalFolio.Create(100).Value;

        (f1 < f2).Should().BeTrue();
        (f1 <= f2).Should().BeTrue();
        (f2 > f1).Should().BeTrue();
        (f2 >= f1).Should().BeTrue();

        (f1 < f1Clone).Should().BeFalse();
        (f1 > f1Clone).Should().BeFalse();
        (f1 <= f1Clone).Should().BeTrue();
        (f1 >= f1Clone).Should().BeTrue();
        f1.CompareTo(f2).Should().BeNegative();
        f2.CompareTo(f1).Should().BePositive();
        f1.CompareTo(f1Clone).Should().Be(0);
    }

    [Fact]
    public void FiscalFolio_DefaultState_ParseAndTryParse()
    {
        var parsed1 = FiscalFolio.Parse("10520", CultureInfo.InvariantCulture);
        parsed1.Value.Should().Be(10520);

        var parsed2 = FiscalFolio.Parse("10520".AsSpan(), CultureInfo.InvariantCulture);
        parsed2.Value.Should().Be(10520);

        FiscalFolio.TryParse("10520", null, out var tryRes1).Should().BeTrue();
        tryRes1.Value.Should().Be(10520);

        FiscalFolio.TryParse("10520".AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Value.Should().Be(10520);

        Action invalidParseStr = () => FiscalFolio.Parse("invalid", CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid FiscalFolio: 'invalid'.");

        Action invalidParseSpan = () => FiscalFolio.Parse("invalid".AsSpan(), CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid FiscalFolio: 'invalid'.");

        FiscalFolio.TryParse("invalid", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(FiscalFolio));

        FiscalFolio.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(FiscalFolio));

        FiscalFolio.TryParse("invalid".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(FiscalFolio));
    }
}




