// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Argentina;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Argentina.UnitTests;

public sealed class VatRateTests
{
    [Fact]
    public void KnownInstances_DefaultState_HaveExactProperties()
    {
        // 0%
        VatRate.Zero.Percentage.Should().Be(0m);
        VatRate.Zero.Fraction.Should().Be(0m);
        VatRate.Zero.Description.Should().Be("0% - Exento / No Gravado");
        VatRate.Zero.ToString().Should().Be("0%");

        // 2.5%
        VatRate.Rate2_5.Percentage.Should().Be(2.5m);
        VatRate.Rate2_5.Fraction.Should().Be(0.025m);
        VatRate.Rate2_5.Description.Should().Be("2.5% - Diferencial");
        VatRate.Rate2_5.ToString().Should().Be("2.5%");

        // 5%
        VatRate.Rate5.Percentage.Should().Be(5m);
        VatRate.Rate5.Fraction.Should().Be(0.05m);
        VatRate.Rate5.Description.Should().Be("5% - Reducida especial");

        // 10.5%
        VatRate.Rate10_5.Percentage.Should().Be(10.5m);
        VatRate.Rate10_5.Fraction.Should().Be(0.105m);
        VatRate.Rate10_5.Description.Should().Be("10.5% - Reducida");

        // 21%
        VatRate.Rate21.Percentage.Should().Be(21m);
        VatRate.Rate21.Fraction.Should().Be(0.21m);
        VatRate.Rate21.Description.Should().Be("21% - General");

        // 27%
        VatRate.Rate27.Percentage.Should().Be(27m);
        VatRate.Rate27.Fraction.Should().Be(0.27m);
        VatRate.Rate27.Description.Should().Be("27% - Incrementada");
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(2.5, 2.5)]
    [InlineData(5, 5)]
    [InlineData(10.5, 10.5)]
    [InlineData(21, 21)]
    [InlineData(27, 27)]
    public void Create_LegalPercentages_Succeed(double inputPct, double expectedPct)
    {
        var result = VatRate.Create((decimal)inputPct);

        result.IsSuccess.Should().BeTrue();
        result.Value.Percentage.Should().Be((decimal)expectedPct);
    }

    [Theory]
    [InlineData("21", 21)]
    [InlineData("21%", 21)]
    [InlineData(" 10.5% ", 10.5)]
    [InlineData("0", 0)]
    public void Create_StringWithOrWithoutPercent_Succeeds(string input, double expectedPct)
    {
        var result = VatRate.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Percentage.Should().Be((decimal)expectedPct);
    }

    [Theory]
    [InlineData(18)]
    [InlineData(19)]
    [InlineData(100)]
    public void Create_InvalidPercentage_ReturnsInvalidRateError(double invalidPct)
    {
        var result = VatRate.Create((decimal)invalidPct);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("VatRate.InvalidRate");
    }

    [Theory]
    [InlineData("ABC")]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_InvalidFormat_ReturnsInvalidFormatError(string invalid)
    {
        var result = VatRate.Create(invalid);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("VatRate.InvalidFormat");
    }

    [Fact]
    public void VatRate_DefaultState_ParseAndTryParse()
    {
        var parsed1 = VatRate.Parse("21%", System.Globalization.CultureInfo.InvariantCulture);
        parsed1.Percentage.Should().Be(21m);

        var parsed2 = VatRate.Parse("21%".AsSpan(), System.Globalization.CultureInfo.InvariantCulture);
        parsed2.Percentage.Should().Be(21m);

        VatRate.TryParse("21%", null, out var tryRes1).Should().BeTrue();
        tryRes1.Percentage.Should().Be(21m);

        VatRate.TryParse("21%".AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Percentage.Should().Be(21m);

        Action invalidParseStr = () => VatRate.Parse("18%", System.Globalization.CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid VatRate: '18%'.");

        Action invalidParseSpan = () => VatRate.Parse("18%".AsSpan(), System.Globalization.CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid VatRate: '18%'.");

        VatRate.TryParse("18%", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(VatRate));

        VatRate.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(VatRate));

        VatRate.TryParse("18%".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(VatRate));

        VatRate.Create((string?)null).IsFailure.Should().BeTrue();
    }

    [Fact]
    public void VatRate_ComparisonsAndOperators_Exhaustive()
    {
        var a = VatRate.Rate10_5; // 10.5
        var aCopy = VatRate.Create(10.5m).Value;
        var b = VatRate.Rate21;   // 21

        a.ShouldSatisfyEqualityContract(aCopy, b, (x, y) => x == y, (x, y) => x != y);
        a.ShouldSatisfyComparisonContract(aCopy, b,
            (x, y) => x < y,
            (x, y) => x <= y,
            (x, y) => x > y,
            (x, y) => x >= y);
    }
}




