// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic.UnitTests;

public sealed class SecurityCodeTests
{
    [Fact]
    public void Create_6Alphanumeric_Succeeds()
    {
        var result = SecurityCode.Create("aB39Zq");

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("aB39Zq");
        result.Value.ToString().Should().Be("aB39Zq");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_NullOrWhitespace_ReturnsRequiredError(string? invalid)
    {
        var result = SecurityCode.Create(invalid);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SecurityCode.Required");
    }

    [Fact]
    public void Create_InvalidLength_Fails()
    {
        SecurityCode.Create("12345").Error.Code.Should().Be("SecurityCode.TooShort");
        SecurityCode.Create("1234567").Error.Code.Should().Be("SecurityCode.TooLong");
    }

    [Theory]

    [InlineData("123456")]
    [InlineData("ABCDEF")]
    [InlineData("abcdef")]
    [InlineData("aB39Zq")]
    [InlineData("  aB39Zq  ")]
    public void Create_ValidPatterns_Succeeds(string input)
    {
        var result = SecurityCode.Create(input);
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(input.Trim());
    }

    [Theory]
    [InlineData("ABCDE#")]
    [InlineData("AB CDE")]
    [InlineData("ABCDEá")]
    [InlineData("12345!")]
    [InlineData("$$$$$$")]
    public void Create_InvalidCharacters_FailsWithInvalidFormat(string input)
    {
        var result = SecurityCode.Create(input);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SecurityCode.InvalidFormat");
        result.Error.Description.Should().Be("Security code must be exactly 6 alphanumeric characters.");
    }

    [Fact]
    public void SecurityCode_Equality_EvaluatesCorrectly()
    {
        var a = SecurityCode.Create("AB12CD").Value;
        var b = SecurityCode.Create("  AB12CD  ").Value;
        var c = SecurityCode.Create("ZZ99XX").Value;

        (a == b).Should().BeTrue();
        (a != c).Should().BeTrue();
    }
}





