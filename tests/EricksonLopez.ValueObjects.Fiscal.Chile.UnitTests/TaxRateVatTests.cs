// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Chile;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Chile.UnitTests;

public sealed class TaxRateVatTests
{
    [Fact]
    public void General19_DefaultState_Properties()
    {
        var rate = TaxRateVat.General19;
        rate.Percentage.Should().Be(19m);
        rate.Fraction.Should().Be(0.19m);
        rate.Description.Should().Be("19% - Tasa General IVA");
        rate.ToString().Should().Be("19%");
    }

    [Fact]
    public void Exempt0_DefaultState_Properties()
    {
        var rate = TaxRateVat.Exempt0;
        rate.Percentage.Should().Be(0m);
        rate.Fraction.Should().Be(0m);
        rate.Description.Should().Be("0% - Exento / No Gravado");
        rate.ToString().Should().Be("0%");
    }

    [Theory]
    [InlineData(19, 19, 0.19)]
    [InlineData(0, 0, 0.0)]
    public void Create_ValidDecimals_Succeeds(decimal input, decimal expectedPercentage, decimal expectedFraction)
    {
        var result = TaxRateVat.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Percentage.Should().Be(expectedPercentage);
        result.Value.Fraction.Should().Be(expectedFraction);
    }

    [Theory]
    [InlineData(21)]
    [InlineData(10.5)]
    [InlineData(-1)]
    public void Create_InvalidDecimal_ReturnsError(decimal input)
    {
        var result = TaxRateVat.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("TaxRateVat.InvalidRate");
    }

    [Theory]
    [InlineData("19%", 19)]
    [InlineData("19", 19)]
    [InlineData("  19%  ", 19)]
    [InlineData("0%", 0)]
    [InlineData("0", 0)]
    public void Create_ValidStrings_Succeeds(string input, decimal expectedPercentage)
    {
        var result = TaxRateVat.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Percentage.Should().Be(expectedPercentage);
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_InvalidFormat_ReturnsError(string? input)
    {
        var result = TaxRateVat.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("TaxRateVat.InvalidFormat");
    }

    [Theory]
    [InlineData("21%")]
    [InlineData("10.5%")]
    public void Create_UnrecognizedRateString_ReturnsError(string input)
    {
        var result = TaxRateVat.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("TaxRateVat.InvalidRate");
    }

    [Fact]
    public void TaxRateVat_DefaultState_ComparisonOperators()
    {
        var rate0 = TaxRateVat.Exempt0;
        var rate19 = TaxRateVat.General19;
        var rate0Clone = TaxRateVat.Create(0m).Value;

        (rate0 < rate19).Should().BeTrue();
        (rate0 <= rate19).Should().BeTrue();
        (rate19 > rate0).Should().BeTrue();
        (rate19 >= rate0).Should().BeTrue();

        (rate0 < rate0Clone).Should().BeFalse();
        (rate0 > rate0Clone).Should().BeFalse();
        (rate0 <= rate0Clone).Should().BeTrue();
        (rate0 >= rate0Clone).Should().BeTrue();
        rate0.CompareTo(rate19).Should().BeNegative();
        rate19.CompareTo(rate0).Should().BePositive();
        rate0.CompareTo(rate0Clone).Should().Be(0);
    }

    [Fact]
    public void TaxRateVat_DefaultState_ParseAndTryParse()
    {
        var parsed1 = TaxRateVat.Parse("19%", CultureInfo.InvariantCulture);
        parsed1.Percentage.Should().Be(19m);

        var parsed2 = TaxRateVat.Parse("19%".AsSpan(), CultureInfo.InvariantCulture);
        parsed2.Percentage.Should().Be(19m);

        TaxRateVat.TryParse("19%", null, out var tryRes1).Should().BeTrue();
        tryRes1.Percentage.Should().Be(19m);

        TaxRateVat.TryParse("19%".AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Percentage.Should().Be(19m);

        Action invalidParseStr = () => TaxRateVat.Parse("21%", CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid TaxRateVat: '21%'.");

        Action invalidParseSpan = () => TaxRateVat.Parse("21%".AsSpan(), CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid TaxRateVat: '21%'.");

        TaxRateVat.TryParse("21%", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(TaxRateVat));

        TaxRateVat.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(TaxRateVat));

        TaxRateVat.TryParse("21%".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(TaxRateVat));
    }
}




