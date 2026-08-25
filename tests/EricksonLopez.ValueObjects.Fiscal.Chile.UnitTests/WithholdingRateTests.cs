// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Chile;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Chile.UnitTests;

public sealed class WithholdingRateTests
{
    [Theory]
    [InlineData(10.0, 0.10)]
    [InlineData(10.75, 0.1075)]
    [InlineData(11.5, 0.115)]
    [InlineData(12.25, 0.1225)]
    [InlineData(13.0, 0.13)]
    [InlineData(13.75, 0.1375)]
    [InlineData(14.5, 0.145)]
    [InlineData(15.25, 0.1525)]
    [InlineData(16.0, 0.16)]
    [InlineData(17.0, 0.17)]
    public void Create_ValidScheduleRates_Succeeds(decimal percentage, decimal expectedFraction)
    {
        var result = WithholdingRate.Create(percentage);

        result.IsSuccess.Should().BeTrue();
        result.Value.Percentage.Should().Be(percentage);
        result.Value.Fraction.Should().Be(expectedFraction);
        result.Value.ToString().Should().Contain("%");
    }

    [Theory]
    [InlineData(2018, 10.00)]
    [InlineData(2019, 10.00)]
    [InlineData(2020, 10.75)]
    [InlineData(2021, 11.50)]
    [InlineData(2022, 12.25)]
    [InlineData(2023, 13.00)]
    [InlineData(2024, 13.75)]
    [InlineData(2025, 14.50)]
    [InlineData(2026, 15.25)]
    [InlineData(2027, 16.00)]
    [InlineData(2028, 17.00)]
    [InlineData(2030, 17.00)]
    public void ForYear_AllYearBranches_ReturnsExactStatutoryRate(int year, decimal expectedPercentage)
    {
        var rate = WithholdingRate.ForYear(year);

        rate.Percentage.Should().Be(expectedPercentage);
        rate.Fraction.Should().Be(expectedPercentage / 100m);
    }

    [Theory]
    [InlineData(18.00)]
    [InlineData(9.00)]
    [InlineData(12.00)]
    [InlineData(-1.00)]
    public void Create_InvalidDecimalRate_ReturnsError(decimal percentage)
    {
        var result = WithholdingRate.Create(percentage);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("WithholdingRate.InvalidRate");
    }

    [Theory]
    [InlineData("13.75%", 13.75)]
    [InlineData("13.75", 13.75)]
    [InlineData("  13.75%  ", 13.75)]
    [InlineData("10%", 10.0)]
    [InlineData("17%", 17.0)]
    public void Create_ValidStrings_Succeeds(string input, decimal expectedPercentage)
    {
        var result = WithholdingRate.Create(input);

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
        var result = WithholdingRate.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("WithholdingRate.InvalidFormat");
    }

    [Theory]
    [InlineData("18%")]
    [InlineData("9%")]
    public void Create_UnrecognizedRateString_ReturnsError(string input)
    {
        var result = WithholdingRate.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("WithholdingRate.InvalidRate");
    }

    [Fact]
    public void WithholdingRate_ToString_FormatsPercentage()
    {
        WithholdingRate.Create(13.75m).Value.ToString().Should().Be("13.75%");
        WithholdingRate.Create(10.0m).Value.ToString().Should().Be("10%");
    }

    [Fact]
    public void WithholdingRate_DefaultState_ComparisonOperators()
    {
        var rate10 = WithholdingRate.ForYear(2019);
        var rate17 = WithholdingRate.ForYear(2028);
        var rate10Clone = WithholdingRate.Create(10.0m).Value;

        (rate10 < rate17).Should().BeTrue();
        (rate10 <= rate17).Should().BeTrue();
        (rate17 > rate10).Should().BeTrue();
        (rate17 >= rate10).Should().BeTrue();

        (rate10 < rate10Clone).Should().BeFalse();
        (rate10 > rate10Clone).Should().BeFalse();
        (rate10 <= rate10Clone).Should().BeTrue();
        (rate10 >= rate10Clone).Should().BeTrue();
        rate10.CompareTo(rate17).Should().BeNegative();
        rate17.CompareTo(rate10).Should().BePositive();
        rate10.CompareTo(rate10Clone).Should().Be(0);
    }

    [Fact]
    public void WithholdingRate_DefaultState_ParseAndTryParse()
    {
        var parsed1 = WithholdingRate.Parse("13.75%", CultureInfo.InvariantCulture);
        parsed1.Percentage.Should().Be(13.75m);

        var parsed2 = WithholdingRate.Parse("13.75%".AsSpan(), CultureInfo.InvariantCulture);
        parsed2.Percentage.Should().Be(13.75m);

        WithholdingRate.TryParse("13.75%", null, out var tryRes1).Should().BeTrue();
        tryRes1.Percentage.Should().Be(13.75m);

        WithholdingRate.TryParse("13.75%".AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Percentage.Should().Be(13.75m);

        Action invalidParseStr = () => WithholdingRate.Parse("18%", CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid WithholdingRate: '18%'.");

        Action invalidParseSpan = () => WithholdingRate.Parse("18%".AsSpan(), CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid WithholdingRate: '18%'.");

        WithholdingRate.TryParse("18%", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(WithholdingRate));

        WithholdingRate.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(WithholdingRate));

        WithholdingRate.TryParse("18%".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(WithholdingRate));
    }
}




