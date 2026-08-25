// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="TimeRange"/> Value Object.
/// </summary>
public sealed class TimeRangeTests
{
    [Fact]
    public void Create_WhenStandardRange_CalculatesDurationCorrectly()
    {
        var start = new TimeOnly(9, 0);
        var end = new TimeOnly(17, 0);
        var range = TimeRange.Create(start, end).Value;

        range.CrossesMidnight.Should().BeFalse();
        range.Duration.Should().Be(TimeSpan.FromHours(8));
        range.Contains(new TimeOnly(12, 0)).Should().BeTrue();
        range.Contains(new TimeOnly(8, 59)).Should().BeFalse();
        range.Contains(new TimeOnly(17, 0)).Should().BeFalse(); // [start, end)
    }

    [Fact]
    public void Create_WhenStartEqualsEnd_ReturnsEmptyError()
    {
        var time = new TimeOnly(10, 0);
        var result = TimeRange.Create(time, time);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("TimeRange.Empty");
    }

    [Fact]
    public void Create_WhenStartAfterEndWithoutOvernightFlag_ReturnsStartAfterEndError()
    {
        var start = new TimeOnly(22, 0);
        var end = new TimeOnly(6, 0);
        var result = TimeRange.Create(start, end, allowOvernight: false);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("TimeRange.StartAfterEnd");
    }

    [Fact]
    public void Create_WhenOvernightShiftAllowed_CrossesMidnightCorrectly()
    {
        var start = new TimeOnly(22, 0);
        var end = new TimeOnly(6, 0);
        var range = TimeRange.Create(start, end, allowOvernight: true).Value;

        range.CrossesMidnight.Should().BeTrue();
        range.Duration.Should().Be(TimeSpan.FromHours(8));
        range.Contains(new TimeOnly(23, 0)).Should().BeTrue();
        range.Contains(new TimeOnly(3, 0)).Should().BeTrue();
        range.Contains(new TimeOnly(12, 0)).Should().BeFalse();
    }

    [Fact]
    public void Overlaps_WhenStandardRangesOverlap_ReturnsTrue()
    {
        var a = TimeRange.Create(new TimeOnly(9, 0), new TimeOnly(12, 0)).Value;
        var b = TimeRange.Create(new TimeOnly(11, 0), new TimeOnly(14, 0)).Value;
        a.Overlaps(b).Should().BeTrue();
        b.Overlaps(a).Should().BeTrue();
    }

    [Fact]
    public void Overlaps_WhenStandardRangesAreDisjoint_ReturnsFalse()
    {
        var a = TimeRange.Create(new TimeOnly(9, 0), new TimeOnly(12, 0)).Value;
        var b = TimeRange.Create(new TimeOnly(13, 0), new TimeOnly(16, 0)).Value;
        a.Overlaps(b).Should().BeFalse();
        b.Overlaps(a).Should().BeFalse();
    }

    [Fact]
    public void Overlaps_WhenStandardRangesAreAdjacent_ReturnsFalse()
    {
        // End of a == Start of b — they touch but don't overlap (interval is [start, end))
        var a = TimeRange.Create(new TimeOnly(9, 0), new TimeOnly(12, 0)).Value;
        var b = TimeRange.Create(new TimeOnly(12, 0), new TimeOnly(15, 0)).Value;
        a.Overlaps(b).Should().BeFalse();
        b.Overlaps(a).Should().BeFalse();
    }

    [Fact]
    public void Overlaps_WhenOvernightRangesEvaluated_DetectsOverlapCorrectly()
    {
        var overnight = TimeRange.Create(new TimeOnly(22, 0), new TimeOnly(6, 0), allowOvernight: true).Value;

        overnight.Contains(new TimeOnly(22, 0)).Should().BeTrue(); // at Start
        overnight.Contains(new TimeOnly(6, 0)).Should().BeFalse(); // at End
        overnight.Contains(new TimeOnly(23, 0)).Should().BeTrue();
        overnight.Contains(new TimeOnly(5, 0)).Should().BeTrue();
        overnight.Contains(new TimeOnly(12, 0)).Should().BeFalse();

        var morningStandard = TimeRange.Create(new TimeOnly(5, 0), new TimeOnly(9, 0)).Value;
        var eveningStandard = TimeRange.Create(new TimeOnly(20, 0), new TimeOnly(23, 0)).Value;
        var dayDisjoint = TimeRange.Create(new TimeOnly(12, 0), new TimeOnly(15, 0)).Value;
        var otherOvernight = TimeRange.Create(new TimeOnly(23, 0), new TimeOnly(7, 0), allowOvernight: true).Value;

        overnight.Overlaps(morningStandard).Should().BeTrue();
        morningStandard.Overlaps(overnight).Should().BeTrue();

        overnight.Overlaps(eveningStandard).Should().BeTrue();
        eveningStandard.Overlaps(overnight).Should().BeTrue();

        overnight.Overlaps(dayDisjoint).Should().BeFalse();
        dayDisjoint.Overlaps(overnight).Should().BeFalse();

        overnight.Overlaps(otherOvernight).Should().BeTrue();
        otherOvernight.Overlaps(overnight).Should().BeTrue();

        Action nullOverlap = () => overnight.Overlaps(null!);
        nullOverlap.Should().Throw<ArgumentNullException>();

        overnight.ToString().Should().Be("[22:00:00 .. 06:00:00]");
    }

    [Fact]
    public void Overlaps_WhenOneOvernightOverlapsWithStandard_ReturnsTrue()
    {
        // Overnight: 22:00 → 02:00 overlaps with standard 01:00 → 03:00
        var night = TimeRange.Create(new TimeOnly(22, 0), new TimeOnly(2, 0), allowOvernight: true).Value;
        var morning = TimeRange.Create(new TimeOnly(1, 0), new TimeOnly(3, 0)).Value;
        night.Overlaps(morning).Should().BeTrue();
    }

    [Fact]
    public void Overlaps_WhenOneOvernightDoesNotOverlapWithMorning_ReturnsFalse()
    {
        // Overnight: 22:00 → 01:00 does NOT overlap with 05:00 → 08:00
        var night = TimeRange.Create(new TimeOnly(22, 0), new TimeOnly(1, 0), allowOvernight: true).Value;
        var morning = TimeRange.Create(new TimeOnly(5, 0), new TimeOnly(8, 0)).Value;
        night.Overlaps(morning).Should().BeFalse();
    }

    [Fact]
    public void Contains_BoundaryConditions_StartIsInclusiveAndEndIsExclusive()
    {
        var standard = TimeRange.Create(new TimeOnly(9, 0), new TimeOnly(17, 0)).Value;
        standard.Contains(new TimeOnly(9, 0)).Should().BeTrue();
        standard.Contains(new TimeOnly(17, 0)).Should().BeFalse();

        var overnight = TimeRange.Create(new TimeOnly(22, 0), new TimeOnly(6, 0), allowOvernight: true).Value;
        overnight.Contains(new TimeOnly(22, 0)).Should().BeTrue();
        overnight.Contains(new TimeOnly(6, 0)).Should().BeFalse();
    }

    [Fact]
    public void Overlaps_WhenBothOvernight_DetectsOverlapCorrectly()
    {
        var a = TimeRange.Create(new TimeOnly(22, 0), new TimeOnly(2, 0), allowOvernight: true).Value;
        var b = TimeRange.Create(new TimeOnly(23, 0), new TimeOnly(3, 0), allowOvernight: true).Value;
        a.Overlaps(b).Should().BeTrue();
    }

    [Fact]
    public void Overlaps_WhenOvernightTouchesBoundaryWithoutOverlap_ReturnsFalse()
    {
        var overnight = TimeRange.Create(new TimeOnly(22, 0), new TimeOnly(6, 0), allowOvernight: true).Value;
        var touchStart = TimeRange.Create(new TimeOnly(20, 0), new TimeOnly(22, 0)).Value;
        var touchEnd = TimeRange.Create(new TimeOnly(6, 0), new TimeOnly(8, 0)).Value;

        // other.End == this.Start (22:00) -> touch without overlap
        overnight.Overlaps(touchStart).Should().BeFalse();
        touchStart.Overlaps(overnight).Should().BeFalse();

        // other.Start == this.End (06:00) -> touch without overlap
        overnight.Overlaps(touchEnd).Should().BeFalse();
        touchEnd.Overlaps(overnight).Should().BeFalse();
    }

    [Fact]
    public void Overlaps_WhenOvernightAndDisjointDaytime_ReturnsFalse()
    {
        var overnight = TimeRange.Create(new TimeOnly(22, 0), new TimeOnly(6, 0), allowOvernight: true).Value;
        var daytime = TimeRange.Create(new TimeOnly(10, 0), new TimeOnly(14, 0)).Value;
        var eveningBefore = TimeRange.Create(new TimeOnly(18, 0), new TimeOnly(21, 0)).Value;
        var morningAfter = TimeRange.Create(new TimeOnly(7, 0), new TimeOnly(9, 0)).Value;

        overnight.Overlaps(daytime).Should().BeFalse();
        daytime.Overlaps(overnight).Should().BeFalse();

        overnight.Overlaps(eveningBefore).Should().BeFalse();
        eveningBefore.Overlaps(overnight).Should().BeFalse();

        overnight.Overlaps(morningAfter).Should().BeFalse();
        morningAfter.Overlaps(overnight).Should().BeFalse();
    }

    [Fact]
    public void Overlaps_WhenTargetIsNull_ThrowsArgumentNullException()
    {
        var a = TimeRange.Create(new TimeOnly(9, 0), new TimeOnly(12, 0)).Value;

        Action act = () => a.Overlaps(null!);
        act.Should().Throw<ArgumentNullException>();

        a.ToString().Should().Be("[09:00:00 .. 12:00:00]");
    }

    [Fact]
    public void Overlaps_WhenStandardRangeEngulfsAnother_ReturnsTrue()
    {
        var outer = TimeRange.Create(new TimeOnly(8, 0), new TimeOnly(18, 0)).Value;
        var inner = TimeRange.Create(new TimeOnly(10, 0), new TimeOnly(14, 0)).Value;

        outer.Overlaps(inner).Should().BeTrue();
        inner.Overlaps(outer).Should().BeTrue();
    }

    [Fact]
    public void Overlaps_WhenOvernightRangeEngulfsStandard_ReturnsTrue()
    {
        var overnight = TimeRange.Create(new TimeOnly(22, 0), new TimeOnly(6, 0), allowOvernight: true).Value;
        var eveningInner = TimeRange.Create(new TimeOnly(23, 0), new TimeOnly(23, 30)).Value;
        var morningInner = TimeRange.Create(new TimeOnly(1, 0), new TimeOnly(3, 0)).Value;

        overnight.Overlaps(eveningInner).Should().BeTrue();
        eveningInner.Overlaps(overnight).Should().BeTrue();

        overnight.Overlaps(morningInner).Should().BeTrue();
        morningInner.Overlaps(overnight).Should().BeTrue();
    }

    [Fact]
    public void EqualityContract_WhenValidTimeRanges_SatisfiesContract()
    {
        var range1 = TimeRange.Create(new TimeOnly(9, 0), new TimeOnly(17, 0)).Value;
        var range1Copy = TimeRange.Create(new TimeOnly(9, 0), new TimeOnly(17, 0)).Value;
        var range2 = TimeRange.Create(new TimeOnly(9, 0), new TimeOnly(18, 0)).Value;

        range1.ShouldSatisfyEqualityContract(range1Copy, range2, (a, b) => a == b, (a, b) => a != b);
    }
}




