// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic.UnitTests;

public sealed class CedulaTests
{
    [Fact]
    public void Create_ValidFormattedCedula_StripsFormattingAndSucceeds()
    {
        var result = Cedula.Create("001-1234567-3");

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("00112345673");
        result.Value.Formatted.Should().Be("001-1234567-3");
    }

    [Fact]
    public void Create_InvalidCheckDigit_ReturnsValidationFailure()
    {
        var result = Cedula.Create("001-1234567-9");

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Cedula.InvalidCheckDigit");
        result.Error.Description.Should().Contain("invalid Modulo 10 check digit");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_NullOrWhitespace_ReturnsRequiredError(string? invalid)
    {
        var result = Cedula.Create(invalid);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Cedula.Required");
    }

    [Theory]
    [InlineData("12345")]
    [InlineData("001-1234567-33")]
    public void Create_InvalidLength_ReturnsInvalidLengthError(string invalid)
    {
        var result = Cedula.Create(invalid);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Cedula.InvalidLength");
    }

    [Fact]
    public void Cedula_Equality_AndToString()
    {
        var a = Cedula.Create("001-1234567-3").Value;
        var b = Cedula.Create("00112345673").Value;
        var c = Cedula.Create("40200000004").Value;

        (a == b).Should().BeTrue();
        (a != c).Should().BeTrue();
        a.ToString().Should().Be("00112345673");
    }
}




