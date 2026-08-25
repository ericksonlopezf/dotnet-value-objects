// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic.UnitTests;

public sealed class FiscalPeriodTests
{
    [Fact]
    public void Create_FromInts_CalculatesDatesAndFilingDeadline()
    {
        var period = FiscalPeriod.Create(2026, 8).Value;

        period.Year.Should().Be(2026);
        period.Month.Should().Be(8);
        period.Start.Should().Be(new DateOnly(2026, 8, 1));
        period.End.Should().Be(new DateOnly(2026, 8, 31));
        period.FilingDeadline.Should().Be(new DateOnly(2026, 9, 20));
        period.ToString().Should().Be("202608");
    }

    [Fact]
    public void Create_FromString_AcceptsBothFormats()
    {
        var p1 = FiscalPeriod.Create("202608").Value;
        var p2 = FiscalPeriod.Create("2026-08").Value;

        p1.Should().Be(p2);
    }

    [Fact]
    public void IsDue_DefaultState_EvaluatesFilingDeadline()
    {
        var period = FiscalPeriod.Create(2026, 8).Value;

        period.IsDue(new DateOnly(2026, 9, 15)).Should().BeFalse();
        period.IsDue(new DateOnly(2026, 9, 20)).Should().BeFalse();
        period.IsDue(new DateOnly(2026, 9, 21)).Should().BeTrue();
    }

    [Fact]
    public void Next_And_Previous_NavigateCalendarMonths()
    {
        var dec = FiscalPeriod.Create(2026, 12).Value;
        dec.FilingDeadline.Should().Be(new DateOnly(2027, 1, 20));

        var jan = dec.Next();
        jan.Year.Should().Be(2027);
        jan.Month.Should().Be(1);

        jan.Previous().Should().Be(dec);

        var midYear = FiscalPeriod.Create(2026, 6).Value;
        midYear.Next().Month.Should().Be(7);
        midYear.Previous().Month.Should().Be(5);
    }

    [Fact]
    public void FromDate_DefaultState_CreatesMatchingPeriod()
    {
        var period = FiscalPeriod.FromDate(new DateOnly(2026, 8, 15));
        period.Year.Should().Be(2026);
        period.Month.Should().Be(8);
    }

    [Theory]
    [InlineData(1999, 1)]
    [InlineData(2101, 1)]
    public void Create_InvalidYear_ReturnsInvalidYearError(int year, int month)
    {
        var result = FiscalPeriod.Create(year, month);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("FiscalPeriod.InvalidYear");
    }

    [Fact]
    public void Create_YearBoundaries_2000_And_2100_Succeed()
    {
        FiscalPeriod.Create(2000, 1).IsSuccess.Should().BeTrue();
        FiscalPeriod.Create(2100, 12).IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData(2026, 0)]
    [InlineData(2026, 13)]
    public void Create_InvalidMonth_ReturnsInvalidMonthError(int year, int month)
    {
        var result = FiscalPeriod.Create(year, month);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("FiscalPeriod.InvalidMonth");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_NullOrWhitespace_ReturnsRequiredError(string? invalid)
    {
        var result = FiscalPeriod.Create(invalid);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("FiscalPeriod.Required");
    }

    [Theory]
    [InlineData("2026")]
    [InlineData("2026088")]
    [InlineData("2026AB")]
    [InlineData("ABCDEF")]
    public void Create_InvalidStringFormat_ReturnsInvalidFormatError(string invalid)
    {
        var result = FiscalPeriod.Create(invalid);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("FiscalPeriod.InvalidFormat");
    }

    [Fact]
    public void FiscalPeriod_ComparisonsAndOperators_Exhaustive()
    {
        var p202512 = FiscalPeriod.Create(2025, 12).Value;
        var p202601 = FiscalPeriod.Create(2026, 1).Value;
        var p202601Copy = FiscalPeriod.Create(2026, 1).Value;
        var p202608 = FiscalPeriod.Create(2026, 8).Value;

        p202601.ShouldSatisfyEqualityContract(p202601Copy, p202512, (x, y) => x == y, (x, y) => x != y);
        p202601.ShouldSatisfyComparisonContract(p202601Copy, p202608,
            (x, y) => x < y,
            (x, y) => x <= y,
            (x, y) => x > y,
            (x, y) => x >= y);

        p202512.ShouldSatisfyComparisonContract(p202512, p202601,
            (x, y) => x < y,
            (x, y) => x <= y,
            (x, y) => x > y,
            (x, y) => x >= y);

        p202601.CompareTo((object)p202601Copy).Should().Be(0);

        Action invalidObj = () => p202601.CompareTo("not-a-period");
        invalidObj.Should().Throw<ArgumentException>()
            .WithMessage("*Object is not a FiscalPeriod*");
    }
}





