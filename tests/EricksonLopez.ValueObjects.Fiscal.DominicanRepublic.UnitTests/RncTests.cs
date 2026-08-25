// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic.UnitTests;

public sealed class RncTests
{
    [Fact]
    public void Create_ValidFormattedRnc_StripsFormattingAndSucceeds()
    {
        var result = Rnc.Create("1-31-88073-8");

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("131880738");
        result.Value.Formatted.Should().Be("1-31-88073-8");
    }

    [Fact]
    public void Create_InvalidCheckDigit_ReturnsValidationFailure()
    {
        var result = Rnc.Create("1-31-88073-9");

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Rnc.InvalidCheckDigit");
        result.Error.Description.Should().Contain("invalid DGII Modulo 11 check digit");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_NullOrWhitespace_ReturnsRequiredError(string? invalid)
    {
        var result = Rnc.Create(invalid);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Rnc.Required");
    }

    [Theory]
    [InlineData("12345")]
    [InlineData("1-31-88073-88")]
    public void Create_InvalidLength_ReturnsValidationFailure(string invalid)
    {
        var result = Rnc.Create(invalid);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Rnc.InvalidLength");
        result.Error.Description.Should().Be("RNC must contain exactly 9 numeric digits.");
    }


    [Fact]
    public void Rnc_Equality_AndToString()
    {
        var a = Rnc.Create("1-31-88073-8").Value;
        var b = Rnc.Create("131880738").Value;
        var c = Rnc.Create("101000015").Value;

        (a == b).Should().BeTrue();
        (a != c).Should().BeTrue();
        a.ToString().Should().Be("131880738");
    }
}




