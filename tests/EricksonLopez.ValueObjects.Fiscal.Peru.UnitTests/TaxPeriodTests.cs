// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Peru;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Peru.UnitTests;

public sealed class TaxPeriodTests
{
    [Theory]
    [InlineData(2026, 8, "202608")]
    [InlineData(2000, 1, "200001")]
    [InlineData(2100, 12, "210012")]
    public void Create_FromYearAndMonth_Succeeds(int year, int month, string expectedFormatted)
    {
        var result = TaxPeriod.Create(year, month);

        result.IsSuccess.Should().BeTrue();
        result.Value.Year.Should().Be(year);
        result.Value.Month.Should().Be((byte)month);
        result.Value.Formatted.Should().Be(expectedFormatted);
        result.Value.ToString().Should().Be(expectedFormatted);
    }

    [Theory]
    [InlineData("202608", 2026, 8)]
    [InlineData("200001", 2000, 1)]
    [InlineData("210012", 2100, 12)]
    [InlineData("  202608  ", 2026, 8)]
    public void Create_FromValidString_ExtractsComponents(string input, int expectedYear, int expectedMonth)
    {
        var result = TaxPeriod.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Year.Should().Be(expectedYear);
        result.Value.Month.Should().Be((byte)expectedMonth);
    }

    [Theory]
    [InlineData(1999, 8)]
    [InlineData(2101, 8)]
    public void Create_InvalidYear_ReturnsError(int year, int month)
    {
        var result = TaxPeriod.Create(year, month);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("TaxPeriod.InvalidYear");
    }

    [Theory]
    [InlineData(2026, 0)]
    [InlineData(2026, 13)]
    public void Create_InvalidMonth_ReturnsError(int year, int month)
    {
        var result = TaxPeriod.Create(year, month);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("TaxPeriod.InvalidMonth");
    }

    [Theory]
    [InlineData("20260")]   // 5
    [InlineData("2026088")] // 7
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_InvalidLength_ReturnsError(string? input)
    {
        var result = TaxPeriod.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("TaxPeriod.InvalidLength");
    }

    [Theory]
    [InlineData("20260A")]
    [InlineData("202A08")]
    [InlineData("ABCDEF")]
    public void Create_InvalidFormat_ReturnsError(string input)
    {
        var result = TaxPeriod.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("TaxPeriod.InvalidFormat");
    }

    [Fact]
    public void TaxPeriod_DefaultState_ComparisonOperators()
    {
        var p1 = TaxPeriod.Create(2026, 1).Value;
        var p2 = TaxPeriod.Create(2026, 2).Value;
        var p3 = TaxPeriod.Create(2027, 1).Value;
        var p1Clone = TaxPeriod.Create("202601").Value;

        (p1 < p2).Should().BeTrue();
        (p1 <= p2).Should().BeTrue();
        (p2 > p1).Should().BeTrue();
        (p2 >= p1).Should().BeTrue();

        (p1 < p3).Should().BeTrue();

        (p1 < p1Clone).Should().BeFalse();
        (p1 > p1Clone).Should().BeFalse();
        (p1 <= p1Clone).Should().BeTrue();
        (p1 >= p1Clone).Should().BeTrue();
        p1.CompareTo(p2).Should().BeNegative();
        p2.CompareTo(p1).Should().BePositive();
        p1.CompareTo(p1Clone).Should().Be(0);
    }

    [Fact]
    public void TaxPeriod_DefaultState_ParseAndTryParse()
    {
        var validStr = "202608";
        var parsed1 = TaxPeriod.Parse(validStr, CultureInfo.InvariantCulture);
        parsed1.Formatted.Should().Be(validStr);

        var parsed2 = TaxPeriod.Parse(validStr.AsSpan(), CultureInfo.InvariantCulture);
        parsed2.Formatted.Should().Be(validStr);

        TaxPeriod.TryParse(validStr, null, out var tryRes1).Should().BeTrue();
        tryRes1.Formatted.Should().Be(validStr);

        TaxPeriod.TryParse(validStr.AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Formatted.Should().Be(validStr);

        Action invalidParseStr = () => TaxPeriod.Parse("invalid", CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid TaxPeriod: 'invalid'.");

        Action invalidParseSpan = () => TaxPeriod.Parse("invalid".AsSpan(), CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid TaxPeriod: 'invalid'.");

        TaxPeriod.TryParse("invalid", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(TaxPeriod));

        TaxPeriod.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(TaxPeriod));

        TaxPeriod.TryParse("invalid".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(TaxPeriod));
    }
}




