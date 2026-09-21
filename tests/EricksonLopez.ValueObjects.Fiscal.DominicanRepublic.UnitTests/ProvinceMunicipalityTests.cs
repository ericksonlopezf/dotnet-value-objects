// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic.UnitTests;

public sealed class ProvinceMunicipalityTests
{
    [Fact]
    public void Create_ValidCode_Succeeds()
    {
        var result = ProvinceMunicipality.Create("01001");

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("01001");
        result.Value.ProvinceCode.Should().Be("01");
        result.Value.MunicipalityCode.Should().Be("001");
        result.Value.ProvinceName.Should().Be("Distrito Nacional");
    }

    [Fact]
    public void Create_SantoDomingoCode_ResolvesCorrectProvinceName()
    {
        var result = ProvinceMunicipality.Create("32001");

        result.IsSuccess.Should().BeTrue();
        result.Value.ProvinceName.Should().Be("Santo Domingo");
    }

    [Theory]
    [InlineData("00001")]
    [InlineData("33001")]
    [InlineData("99001")]
    public void Create_InvalidProvinceCode_ReturnsFailure(string invalidProvince)
    {
        var result = ProvinceMunicipality.Create(invalidProvince);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("ProvinceMunicipality.InvalidProvince");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_NullOrWhitespace_ReturnsRequiredError(string? invalid)
    {
        var result = ProvinceMunicipality.Create(invalid);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("ProvinceMunicipality.Required");
    }

    [Theory]
    [InlineData("0101")]
    [InlineData("010001")]
    public void Create_InvalidLength_ReturnsInvalidLengthError(string invalid)
    {
        var result = ProvinceMunicipality.Create(invalid);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("ProvinceMunicipality.InvalidLength");
    }

    [Fact]
    public void Create_NonNumeric_ReturnsInvalidFormatError()
    {
        var result = ProvinceMunicipality.Create("01A01");
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("ProvinceMunicipality.InvalidFormat");
    }

    [Fact]
    public void Properties_And_Parsing_WorkCorrectly()
    {
        var pm = ProvinceMunicipality.Create("01001").Value;
        pm.Code.Should().Be("01001");
        pm.DisplayName.Should().Be("Distrito Nacional (01001)");

        ProvinceMunicipality.TryCreate("01001", out var parsed).Should().BeTrue();
        parsed.Should().NotBeNull();
        parsed!.Value.Should().Be("01001");

        ProvinceMunicipality.TryCreate("INVALID", out var invalidParsed).Should().BeFalse();
        invalidParsed.Should().BeNull();
    }
}
