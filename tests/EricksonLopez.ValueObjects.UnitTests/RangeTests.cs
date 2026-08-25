// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

public sealed class RangeTests
{
    [Fact]
    public void Create_ShouldReturnSuccess_WhenStartIsLessThanOrEqualToEnd()
    {
        var range = Range<int>.Create(10, 20);

        range.IsSuccess.Should().BeTrue();
        range.Value.Start.Should().Be(10);
        range.Value.End.Should().Be(20);
    }

    [Fact]
    public void Create_ShouldReturnSuccess_WhenStartEqualsEnd()
    {
        var range = Range<int>.Create(15, 15);

        range.IsSuccess.Should().BeTrue();
        range.Value.Start.Should().Be(15);
        range.Value.End.Should().Be(15);
    }

    [Fact]
    public void Create_ShouldReturnFailure_WhenStartIsGreaterThanEnd()
    {
        var range = Range<int>.Create(30, 20);

        range.IsFailure.Should().BeTrue();
        range.Error.Code.Should().Be("Range.InvalidBounds");
    }

    [Fact]
    public void Contains_Value_ShouldReturnTrue_WhenValueIsWithinBoundsOrOnEdge()
    {
        var range = Range<int>.Create(10, 20).Value;

        range.Contains(10).Should().BeTrue();
        range.Contains(15).Should().BeTrue();
        range.Contains(20).Should().BeTrue();
        range.Contains(9).Should().BeFalse();
        range.Contains(21).Should().BeFalse();
    }

    [Fact]
    public void Contains_Range_ShouldReturnTrue_WhenOtherIsFullyContained()
    {
        var range = Range<int>.Create(10, 50).Value;
        var subRange = Range<int>.Create(15, 45).Value;
        var edgeRange = Range<int>.Create(10, 50).Value;
        var outsideRange = Range<int>.Create(5, 25).Value;
        var outsideEndRange = Range<int>.Create(25, 55).Value;

        range.Contains(subRange).Should().BeTrue();
        range.Contains(edgeRange).Should().BeTrue();
        range.Contains(outsideRange).Should().BeFalse();
        range.Contains(outsideEndRange).Should().BeFalse();
    }

    [Fact]
    public void Overlaps_ShouldReturnTrue_WhenRangesIntersect()
    {
        var range = Range<int>.Create(10, 30).Value;
        var overlapLeft = Range<int>.Create(5, 15).Value;
        var overlapRight = Range<int>.Create(25, 35).Value;
        var overlapPointLeft = Range<int>.Create(5, 10).Value;
        var overlapPointRight = Range<int>.Create(30, 40).Value;
        var disjointBefore = Range<int>.Create(1, 9).Value;
        var disjointAfter = Range<int>.Create(31, 40).Value;

        range.Overlaps(overlapLeft).Should().BeTrue();
        range.Overlaps(overlapRight).Should().BeTrue();
        range.Overlaps(overlapPointLeft).Should().BeTrue();
        range.Overlaps(overlapPointRight).Should().BeTrue();
        range.Overlaps(disjointBefore).Should().BeFalse();
        range.Overlaps(disjointAfter).Should().BeFalse();
    }

    [Fact]
    public void Intersects_ShouldReturnIntersection_WhenRangesOverlap()
    {
        var range1 = Range<int>.Create(10, 30).Value;
        var range2 = Range<int>.Create(20, 40).Value;

        var intersects = range1.Intersects(range2, out var intersection);

        intersects.Should().BeTrue();
        intersection.Start.Should().Be(20);
        intersection.End.Should().Be(30);

        // Sub-range fully inside
        var outer = Range<int>.Create(10, 50).Value;
        var inner = Range<int>.Create(20, 40).Value;
        outer.Intersects(inner, out var subInter1).Should().BeTrue();
        subInter1.Start.Should().Be(20);
        subInter1.End.Should().Be(40);

        inner.Intersects(outer, out var subInter2).Should().BeTrue();
        subInter2.Start.Should().Be(20);
        subInter2.End.Should().Be(40);

        // Point intersection
        var rA = Range<int>.Create(10, 20).Value;
        var rB = Range<int>.Create(20, 30).Value;
        rA.Intersects(rB, out var pointInter).Should().BeTrue();
        pointInter.Start.Should().Be(20);
        pointInter.End.Should().Be(20);
    }

    [Fact]
    public void Intersects_ShouldReturnFalseAndDefault_WhenRangesDoNotOverlap()
    {
        var range1 = Range<int>.Create(10, 20).Value;
        var range2 = Range<int>.Create(25, 35).Value;

        var intersects = range1.Intersects(range2, out var intersection);

        intersects.Should().BeFalse();
        intersection.Should().Be(default(Range<int>));
    }

    [Fact]
    public void Intersects_WithEqualStartsAndEqualEnds_PreservesInstancePrecedence()
    {
        var p1 = new TestCustomPoint(10, "R1Start");
        var p2 = new TestCustomPoint(20, "R1End");
        var p3 = new TestCustomPoint(10, "R2Start");
        var p4 = new TestCustomPoint(20, "R2End");

        var r1 = Range<TestCustomPoint>.Create(p1, p2).Value;
        var r2 = Range<TestCustomPoint>.Create(p3, p4).Value;

        var hasIntersection = r1.Intersects(r2, out var intersection);
        hasIntersection.Should().BeTrue();
        intersection.Start.Tag.Should().Be("R2Start");
        intersection.End.Tag.Should().Be("R2End");
    }

    [Fact]
    public void CompareTo_Typed_ShouldOrderCorrectly()
    {
        var r1 = Range<int>.Create(10, 20).Value;
        var r2 = Range<int>.Create(10, 25).Value;
        var r3 = Range<int>.Create(15, 20).Value;
        var r4 = Range<int>.Create(10, 20).Value;

        r1.CompareTo(r2).Should().BeNegative();
        r2.CompareTo(r1).Should().BePositive();
        r1.CompareTo(r3).Should().BeNegative();
        r1.CompareTo(r4).Should().Be(0);
    }

    [Fact]
    public void CompareTo_Object_ShouldOrderCorrectlyOrThrow()
    {
        var r1 = Range<int>.Create(10, 20).Value;
        var r2 = Range<int>.Create(10, 25).Value;

        r1.CompareTo((object?)r2).Should().BeNegative();
        r1.CompareTo((object?)null).Should().Be(1);

        Action act = () => r1.CompareTo("not-a-range");
        act.Should().Throw<ArgumentException>()
            .WithMessage($"Object must be of type Range<{typeof(int).Name}>*");
    }

    [Fact]
    public void ComparisonOperators_DefaultState_ShouldEvaluateCorrectly()
    {
        var r1 = Range<int>.Create(10, 20).Value;
        var r1Copy = Range<int>.Create(10, 20).Value;
        var r2 = Range<int>.Create(15, 25).Value;

        r1.ShouldSatisfyEqualityContract(r1Copy, r2, (a, b) => a == b, (a, b) => a != b);
        r1.ShouldSatisfyComparisonContract(r1Copy, r2,
            (a, b) => a < b,
            (a, b) => a <= b,
            (a, b) => a > b,
            (a, b) => a >= b);
    }

    [Fact]
    public void ToString_DefaultState_ShouldReturnFormattedInterval()
    {
        var range = Range<int>.Create(10, 20).Value;

        range.ToString().Should().Be("[10 .. 20]");
    }
}



