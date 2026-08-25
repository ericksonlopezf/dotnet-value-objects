// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Argentina;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Argentina.UnitTests;

public sealed class JurisdictionCodeTests
{
    [Fact]
    public void KnownInstances_DefaultState_HaveExactProperties()
    {
        JurisdictionCode.Caba.Code.Should().Be(901);
        JurisdictionCode.Caba.Name.Should().Be("Ciudad Autónoma de Buenos Aires");

        JurisdictionCode.BuenosAires.Code.Should().Be(902);
        JurisdictionCode.BuenosAires.Name.Should().Be("Buenos Aires");

        JurisdictionCode.Cordoba.Code.Should().Be(904);
        JurisdictionCode.Cordoba.Name.Should().Be("Córdoba");

        JurisdictionCode.SantaFe.Code.Should().Be(921);
        JurisdictionCode.SantaFe.Name.Should().Be("Santa Fe");

        JurisdictionCode.Mendoza.Code.Should().Be(913);
        JurisdictionCode.Mendoza.Name.Should().Be("Mendoza");

        JurisdictionCode.Caba.ToString().Should().Be("901 - Ciudad Autónoma de Buenos Aires");
    }

    [Theory]
    [InlineData(901, "Ciudad Autónoma de Buenos Aires")]
    [InlineData(902, "Buenos Aires")]
    [InlineData(903, "Catamarca")]
    [InlineData(904, "Córdoba")]
    [InlineData(905, "Corrientes")]
    [InlineData(906, "Chaco")]
    [InlineData(907, "Chubut")]
    [InlineData(908, "Entre Ríos")]
    [InlineData(909, "Formosa")]
    [InlineData(910, "Jujuy")]
    [InlineData(911, "La Pampa")]
    [InlineData(912, "La Rioja")]
    [InlineData(913, "Mendoza")]
    [InlineData(914, "Misiones")]
    [InlineData(915, "Neuquén")]
    [InlineData(916, "Río Negro")]
    [InlineData(917, "Salta")]
    [InlineData(918, "San Juan")]
    [InlineData(919, "San Luis")]
    [InlineData(920, "Santa Cruz")]
    [InlineData(921, "Santa Fe")]
    [InlineData(922, "Santiago del Estero")]
    [InlineData(923, "Tierra del Fuego")]
    [InlineData(924, "Tucumán")]
    public void Create_All24Jurisdictions_Succeed(int code, string expectedName)
    {
        var result = JurisdictionCode.Create(code);

        result.IsSuccess.Should().BeTrue();
        result.Value.Code.Should().Be(code);
        result.Value.Name.Should().Be(expectedName);
    }

    [Theory]
    [InlineData("901", 901)]
    [InlineData("924", 924)]
    public void Create_ValidNumericString_Succeeds(string input, int expectedCode)
    {
        var result = JurisdictionCode.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Code.Should().Be(expectedCode);
    }

    [Theory]
    [InlineData(900)]
    [InlineData(925)]
    [InlineData(0)]
    public void Create_OutOfRange_ReturnsOutOfRangeError(int invalidCode)
    {
        var result = JurisdictionCode.Create(invalidCode);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("JurisdictionCode.OutOfRange");
    }

    [Theory]
    [InlineData("ABC")]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_InvalidFormat_ReturnsInvalidFormatError(string invalid)
    {
        var result = JurisdictionCode.Create(invalid);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("JurisdictionCode.InvalidFormat");
    }

    [Fact]
    public void JurisdictionCode_DefaultState_ParseAndTryParse()
    {
        var parsed1 = JurisdictionCode.Parse("901", System.Globalization.CultureInfo.InvariantCulture);
        parsed1.Code.Should().Be(901);

        var parsed2 = JurisdictionCode.Parse("901".AsSpan(), System.Globalization.CultureInfo.InvariantCulture);
        parsed2.Code.Should().Be(901);

        JurisdictionCode.TryParse("901", null, out var tryRes1).Should().BeTrue();
        tryRes1.Code.Should().Be(901);

        JurisdictionCode.TryParse("901".AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Code.Should().Be(901);

        Action invalidParseStr = () => JurisdictionCode.Parse("999", System.Globalization.CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid JurisdictionCode: '999'.");

        Action invalidParseSpan = () => JurisdictionCode.Parse("999".AsSpan(), System.Globalization.CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid JurisdictionCode: '999'.");

        JurisdictionCode.TryParse("999", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(JurisdictionCode));

        JurisdictionCode.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(JurisdictionCode));

        JurisdictionCode.TryParse("999".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(JurisdictionCode));

        JurisdictionCode.Create((string?)null).IsFailure.Should().BeTrue();
    }

    [Fact]
    public void JurisdictionCode_ComparisonsAndOperators_Exhaustive()
    {
        var a = JurisdictionCode.Caba; // 901
        var aCopy = JurisdictionCode.Create(901).Value;
        var b = JurisdictionCode.BuenosAires; // 902

        a.ShouldSatisfyEqualityContract(aCopy, b, (x, y) => x == y, (x, y) => x != y);
        a.ShouldSatisfyComparisonContract(aCopy, b,
            (x, y) => x < y,
            (x, y) => x <= y,
            (x, y) => x > y,
            (x, y) => x >= y);
    }
}




