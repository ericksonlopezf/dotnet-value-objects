// Copyright © Erickson Lopez. MIT License.
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Colombia;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Colombia.UnitTests;

public sealed class AuthorizationRangeTests
{
    [Theory]
    [InlineData(1L, 5000L, 5000L)]
    [InlineData(100L, 100L, 1L)]
    [InlineData(1001L, 2000L, 1000L)]
    public void Create_ValidRange_ComputesTotalCountAndProperties(long from, long to, long expectedTotal)
    {
        var result = AuthorizationRange.Create(from, to);

        result.IsSuccess.Should().BeTrue();
        result.Value.From.Should().Be(from);
        result.Value.To.Should().Be(to);
        result.Value.TotalCount.Should().Be(expectedTotal);
        result.Value.ToString().Should().Be($"[{from}..{to}]");
    }

    [Theory]
    [InlineData(0L, 5000L)]
    [InlineData(-1L, 5000L)]
    [InlineData(-100L, 100L)]
    public void Create_InvalidFrom_ReturnsError(long from, long to)
    {
        var result = AuthorizationRange.Create(from, to);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("AuthorizationRange.InvalidFrom");
    }

    [Theory]
    [InlineData(5000L, 1000L)]
    [InlineData(100L, 99L)]
    public void Create_InvalidToLessThanFrom_ReturnsError(long from, long to)
    {
        var result = AuthorizationRange.Create(from, to);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("AuthorizationRange.InvalidTo");
    }

    [Fact]
    public void Contains_DefaultState_EvaluatesCorrectly()
    {
        var range = AuthorizationRange.Create(100, 200).Value;

        range.Contains(100).Should().BeTrue();
        range.Contains(200).Should().BeTrue();
        range.Contains(150).Should().BeTrue();
        range.Contains(99).Should().BeFalse();
        range.Contains(201).Should().BeFalse();
        range.Contains(0).Should().BeFalse();
    }

    [Fact]
    public void AuthorizationRange_DefaultState_Equality()
    {
        var r1 = AuthorizationRange.Create(1, 100).Value;
        var r2 = AuthorizationRange.Create(1, 100).Value;
        var rDiff = AuthorizationRange.Create(1, 200).Value;

        (r1 == r2).Should().BeTrue();
        (r1 != rDiff).Should().BeTrue();
        r1.Equals(r2).Should().BeTrue();
        r1.Equals((object)r2).Should().BeTrue();
        r1.Equals(rDiff).Should().BeFalse();
        r1.GetHashCode().Should().Be(r2.GetHashCode());
    }
}




