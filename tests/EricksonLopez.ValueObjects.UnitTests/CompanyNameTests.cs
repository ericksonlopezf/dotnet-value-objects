// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="CompanyName"/> Value Object.
/// </summary>
public sealed class CompanyNameTests
{
    [Fact]
    public void CompanyName_ValidName_ShouldSucceed()
    {
        var result = CompanyName.Create("Jeiyel Technology, S.R.L.");
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("Jeiyel Technology, S.R.L.");
        result.Value.ToString().Should().Be("Jeiyel Technology, S.R.L.");
    }

    [Fact]
    public void CompanyName_WithAmpersand_AndPunctuation_ShouldSucceed()
    {
        var result = CompanyName.Create("  Johnson   &   Johnson (Holdings) / S.A.  ");
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("Johnson & Johnson (Holdings) / S.A.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("A")] // < 2 chars
    public void CompanyName_Invalid_ShouldFail(string? invalid)
    {
        var result = CompanyName.Create(invalid);
        result.IsFailure.Should().BeTrue();
        if (string.IsNullOrWhiteSpace(invalid)) result.Error.Code.Should().Be("CompanyName.Required");
        else if (invalid == "A") result.Error.Code.Should().Be("CompanyName.TooShort");
    }

    [Fact]
    public void CompanyName_Invalid_ReturnsSpecificErrors()
    {
        CompanyName.Create(new string('a', 181)).Error.Code.Should().Be("CompanyName.TooLong");
        var invalidPattern = CompanyName.Create("Company<script>");
        invalidPattern.Error.Code.Should().Be("CompanyName.InvalidFormat");
        invalidPattern.Error.Description.Should().Be("Company name can contain letters, digits, spaces, and common business punctuation.");
    }

    [Fact]
    public void CompanyName_Equality_SameValue_AreEqual()
    {
        var a = CompanyName.Create("Acme Corp").Value;
        var b = CompanyName.Create("  Acme   Corp  ").Value;
        a.Should().Be(b);
        (a == b).Should().BeTrue();
    }
}




