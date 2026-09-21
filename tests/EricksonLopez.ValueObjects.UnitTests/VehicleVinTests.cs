// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

public sealed class VehicleVinTests
{
    // Valid ISO 3779 VIN: 1HGCR2F85HA000000 (Honda Accord 2017)
    // 1=USA/Honda, H=Honda, G=Passenger car, CR2F8=Trim/Body, 5=Check digit, H=2017, A=Marysville, 000000=Seq
    [Fact]
    public void Create_ValidVin_Succeeds()
    {
        var result = VehicleVin.Create("1HGCR2F85HA000000");

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("1HGCR2F85HA000000");
        result.Value.Wmi.Should().Be("1HG");
        result.Value.Vds.Should().Be("CR2F85");
        result.Value.Vis.Should().Be("HA000000");
        result.Value.ModelYear.Should().Be(2017);
    }

    [Theory]
    [InlineData("1HGCR2F85IA000000")] // Contains 'I'
    [InlineData("1HGCR2F85OA000000")] // Contains 'O'
    [InlineData("1HGCR2F85QA000000")] // Contains 'Q'
    public void Create_ForbiddenCharacters_ReturnsForbiddenCharactersError(string invalid)
    {
        var result = VehicleVin.Create(invalid);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("VehicleVin.ForbiddenCharacters");
    }

    [Theory]
    [InlineData("12345")]
    [InlineData("1HGCR2F85HA0000000")] // 18 chars
    public void Create_InvalidLength_ReturnsInvalidLengthError(string invalid)
    {
        var result = VehicleVin.Create(invalid);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("VehicleVin.InvalidLength");
    }

    [Fact]
    public void Create_InvalidCheckDigit_ReturnsCheckDigitError()
    {
        // Change check digit from '5' to '9'
        var result = VehicleVin.Create("1HGCR2F89HA000000");
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("VehicleVin.InvalidCheckDigit");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_NullOrWhitespace_ReturnsRequiredError(string? invalid)
    {
        var result = VehicleVin.Create(invalid);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("VehicleVin.Required");
    }

    [Fact]
    public void Create_WithCheckDigitX_Succeeds()
    {
        var result = VehicleVin.Create("1FAFP53UX1A000008");
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("1FAFP53UX1A000008");
    }

    [Fact]
    public void Create_NonAlphanumeric_ReturnsInvalidFormatError()
    {
        var result = VehicleVin.Create("1HGCR2F85HA00000!");
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("VehicleVin.InvalidFormat");
    }

    [Fact]
    public void Properties_AliasesAndInternalFactories_WorkCorrectly()
    {
        var vin = VehicleVin.Create("1HGCR2F85HA000000").Value;
        vin.WorldManufacturerIdentifier.Should().Be("1HG");
        vin.VehicleDescriptorSection.Should().Be("CR2F85");
        vin.VehicleIdentifierSection.Should().Be("HA000000");

        var reconstituted = VehicleVin.Reconstitute("1HGCR2F85HA000000");
        reconstituted.Value.Should().Be("1HGCR2F85HA000000");

        var hydrated = VehicleVin.Hydrate("1HGCR2F85HA000000");
        hydrated.Value.Should().Be("1HGCR2F85HA000000");
    }
}
