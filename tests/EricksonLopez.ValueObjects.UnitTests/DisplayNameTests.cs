// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="DisplayName"/> Value Object.
/// </summary>
public sealed class DisplayNameTests
{
    [Fact]
    public void DisplayName_ValidName_CollapsesWhitespace()
    {
        var result = DisplayName.Create("  Dev Team   Lead  ");

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("Dev Team Lead");
        result.Value.ToString().Should().Be("Dev Team Lead");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void DisplayName_Invalid_ShouldFail(string? invalid)
    {
        var result = DisplayName.Create(invalid);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("DisplayName.Required");
    }

    [Fact]
    public void DisplayName_TooShortOrTooLong_ShouldFail()
    {
        var tooShort = DisplayName.Create("A");
        tooShort.IsFailure.Should().BeTrue();
        tooShort.Error.Code.Should().Be("DisplayName.TooShort");

        var tooLong = DisplayName.Create(new string('a', 121));
        tooLong.IsFailure.Should().BeTrue();
        tooLong.Error.Code.Should().Be("DisplayName.TooLong");
    }

    [Fact]
    public void DisplayName_InvalidCharacters_ShouldFailWithExpectedMessage()
    {
        var result = DisplayName.Create("John <Dev>");
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("DisplayName.InvalidFormat");
        result.Error.Description.Should().Be("Display name can contain letters, digits, spaces, and common business punctuation.");
    }

    [Fact]
    public void DisplayName_EqualityAndComparisonContract()
    {
        var d1 = DisplayName.Create("Alice").Value;
        var d1Copy = DisplayName.Create("Alice").Value;
        var d2 = DisplayName.Create("Bob").Value;

        d1.ShouldSatisfyEqualityContract(d1Copy, d2, (a, b) => a == b, (a, b) => a != b);
    }
}




