// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic.UnitTests;

public sealed class TaxpayerIdTests
{
    [Fact]
    public void Create_9Digits_IdentifiesAsRnc()
    {
        var result = TaxpayerId.Create("1-31-88073-8");

        result.IsSuccess.Should().BeTrue();
        result.Value.Type.Should().Be(TaxpayerIdType.Rnc);
        result.Value.Value.Should().Be("131880738");
        result.Value.AsRnc.Should().NotBeNull();
        result.Value.AsCedula.Should().BeNull();
    }

    [Fact]
    public void Create_11Digits_IdentifiesAsCedula()
    {
        var result = TaxpayerId.Create("001-1234567-3");

        result.IsSuccess.Should().BeTrue();
        result.Value.Type.Should().Be(TaxpayerIdType.Cedula);
        result.Value.Value.Should().Be("00112345673");
        result.Value.AsCedula.Should().NotBeNull();
        result.Value.AsRnc.Should().BeNull();
    }

    [Fact]
    public void Create_OtherLength_ReturnsFailure()
    {
        var result = TaxpayerId.Create("12345678");

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("TaxpayerId.InvalidLength");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_NullOrWhitespace_ReturnsRequiredError(string? invalid)
    {
        var result = TaxpayerId.Create(invalid);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("TaxpayerId.Required");
    }

    [Fact]
    public void Create_Invalid9Digits_PropagatesRncFailure()
    {
        var result = TaxpayerId.Create("1-31-88073-9");
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Rnc.InvalidCheckDigit");
    }

    [Fact]
    public void Create_Invalid11Digits_PropagatesCedulaFailure()
    {
        var result = TaxpayerId.Create("001-1234567-9");
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Cedula.InvalidCheckDigit");
    }

    [Fact]
    public void FromRnc_And_FromCedula_PreValidatedInstances()
    {
        var rnc = Rnc.Create("1-31-88073-8").Value;
        var cedula = Cedula.Create("001-1234567-3").Value;

        var fromRnc = TaxpayerId.FromRnc(rnc);
        fromRnc.Type.Should().Be(TaxpayerIdType.Rnc);
        fromRnc.Formatted.Should().Be("1-31-88073-8");
        fromRnc.AsRnc.Should().Be(rnc);
        fromRnc.AsCedula.Should().BeNull();
        fromRnc.ToString().Should().Be("131880738");

        var fromCedula = TaxpayerId.FromCedula(cedula);
        fromCedula.Type.Should().Be(TaxpayerIdType.Cedula);
        fromCedula.Formatted.Should().Be("001-1234567-3");
        fromCedula.AsCedula.Should().Be(cedula);
        fromCedula.AsRnc.Should().BeNull();
        fromCedula.ToString().Should().Be("00112345673");

        Action nullRnc = () => TaxpayerId.FromRnc(null!);
        nullRnc.Should().Throw<ArgumentNullException>();

        Action nullCedula = () => TaxpayerId.FromCedula(null!);
        nullCedula.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void TaxpayerId_Equality_EvaluatesCorrectly()
    {
        var a = TaxpayerId.Create("1-31-88073-8").Value;
        var b = TaxpayerId.Create("131880738").Value;
        var c = TaxpayerId.Create("001-1234567-3").Value;

        (a == b).Should().BeTrue();
        (a != c).Should().BeTrue();
        a.GetHashCode().Should().Be(b.GetHashCode());
    }
}




