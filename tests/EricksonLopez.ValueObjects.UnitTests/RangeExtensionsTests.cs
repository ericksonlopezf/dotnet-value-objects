// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

public sealed class RangeExtensionsTests
{
    [Fact]
    public void Duration_ShouldCalculateDifference_ForDateTimeOffsetRange()
    {
        var start = new DateTimeOffset(2026, 1, 1, 10, 0, 0, TimeSpan.Zero);
        var end = new DateTimeOffset(2026, 1, 1, 15, 30, 0, TimeSpan.Zero);
        var range = Range<DateTimeOffset>.Create(start, end).Value;

        var duration = range.Duration();

        duration.Should().Be(TimeSpan.FromHours(5.5));
    }

    [Fact]
    public void Days_ShouldCalculateDayDifference_ForDateOnlyRange()
    {
        var start = new DateOnly(2026, 1, 1);
        var end = new DateOnly(2026, 1, 15);
        var range = Range<DateOnly>.Create(start, end).Value;

        var days = range.Days();

        days.Should().Be(14);
    }
}



