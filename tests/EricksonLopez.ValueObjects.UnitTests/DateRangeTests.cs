// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="DateRange"/> Value Object.
/// </summary>
public sealed class DateRangeTests
{
    [Fact]
    public void Create_WhenValidBoundaries_CalculatesDurationCorrectly()
    {
        var start = new DateOnly(2026, 1, 1);
        var end = new DateOnly(2026, 1, 10);
        var range = DateRange.Create(start, end).Value;

        range.Start.Should().Be(start);
        range.End.Should().Be(end);
        range.DurationInDays.Should().Be(10);
        range.ToString().Should().Be("[2026-01-01 .. 2026-01-10]");
    }

    [Fact]
    public void Create_WhenStartIsAfterEnd_ReturnsStartAfterEndError()
    {
        var result = DateRange.Create(new DateOnly(2026, 12, 31), new DateOnly(2026, 1, 1));
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("DateRange.StartAfterEnd");
    }

    [Fact]
    public void Create_WhenDatesAreMinOrMaxValues_ReturnsOutOfRangeError()
    {
        var minResult = DateRange.Create(DateOnly.MinValue, new DateOnly(2026, 1, 1));
        minResult.IsFailure.Should().BeTrue();
        minResult.Error.Code.Should().Be("DateRange.OutOfRange");

        var maxResult = DateRange.Create(new DateOnly(2026, 1, 1), DateOnly.MaxValue);
        maxResult.IsFailure.Should().BeTrue();
        maxResult.Error.Code.Should().Be("DateRange.OutOfRange");
    }

    [Fact]
    public void Contains_WhenDateIsWithinOrOutsideRange_ReturnsExpectedBoolean()
    {
        var range = DateRange.Create(new DateOnly(2026, 1, 1), new DateOnly(2026, 1, 10)).Value;

        range.Contains(new DateOnly(2026, 1, 1)).Should().BeTrue();
        range.Contains(new DateOnly(2026, 1, 5)).Should().BeTrue();
        range.Contains(new DateOnly(2026, 1, 10)).Should().BeTrue();
        range.Contains(new DateOnly(2026, 1, 11)).Should().BeFalse();
        range.Contains(new DateOnly(2025, 12, 31)).Should().BeFalse();
    }

    [Fact]
    public void Overlaps_WhenRangesOverlapOrDisjoint_DetectsOverlapCorrectly()
    {
        var a = DateRange.Create(new DateOnly(2026, 1, 1), new DateOnly(2026, 6, 30)).Value;
        var b = DateRange.Create(new DateOnly(2026, 6, 1), new DateOnly(2026, 12, 31)).Value;
        var c = DateRange.Create(new DateOnly(2026, 7, 1), new DateOnly(2026, 12, 31)).Value;
        var pointOverlap = DateRange.Create(new DateOnly(2026, 6, 30), new DateOnly(2026, 12, 31)).Value;

        a.Overlaps(b).Should().BeTrue();
        b.Overlaps(a).Should().BeTrue();
        a.Overlaps(pointOverlap).Should().BeTrue();
        pointOverlap.Overlaps(a).Should().BeTrue();
        a.Overlaps(c).Should().BeFalse();
        c.Overlaps(a).Should().BeFalse();

        // Same start and end date (single day range)
        var singleDay = DateRange.Create(new DateOnly(2026, 5, 1), new DateOnly(2026, 5, 1)).Value;
        singleDay.DurationInDays.Should().Be(1);
    }

    [Fact]
    public void EqualityContract_WhenValidDateRanges_SatisfiesContract()
    {
        var range1 = DateRange.Create(new DateOnly(2026, 1, 1), new DateOnly(2026, 1, 10)).Value;
        var range1Copy = DateRange.Create(new DateOnly(2026, 1, 1), new DateOnly(2026, 1, 10)).Value;
        var range2 = DateRange.Create(new DateOnly(2026, 1, 1), new DateOnly(2026, 1, 20)).Value;

        range1.ShouldSatisfyEqualityContract(range1Copy, range2, (a, b) => a == b, (a, b) => a != b);
    }
}




