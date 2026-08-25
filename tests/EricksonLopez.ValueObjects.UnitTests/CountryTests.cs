// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="Country"/> Value Object.
/// </summary>
public sealed class CountryTests
{
    [Fact]
    public void Country_ValidAlpha2_NormalizesUppercase()
    {
        var result = Country.Create("do");
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("DO");
        result.Value.ToString().Should().Be("DO");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("DOM")] // 3 letters
    [InlineData("D1")]  // digits
    [InlineData("D@")]  // symbols
    public void Country_InvalidCode_ShouldFail(string? invalid)
    {
        var result = Country.Create(invalid);
        result.IsFailure.Should().BeTrue();
        if (string.IsNullOrWhiteSpace(invalid)) result.Error.Code.Should().Be("Country.Required");
        else if (invalid == "DOM") result.Error.Code.Should().Be("Country.TooLong");
        else result.Error.Code.Should().Be("Country.InvalidFormat");
    }

    [Fact]
    public void Country_Equality_SameCode_AreEqual()
    {
        var a = Country.Create("US").Value;
        var b = Country.Create("us").Value;
        a.Should().Be(b);
        (a == b).Should().BeTrue();
    }
}




