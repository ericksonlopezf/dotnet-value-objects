// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Peru;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Peru.UnitTests;

public sealed class AffectationTypeCodeTests
{
    [Theory]
    [InlineData("10", "Gravado - Operación Onerosa", true, false, false, false)]
    [InlineData("20", "Exonerado - Operación Onerosa", false, true, false, false)]
    [InlineData("30", "Inafecto - Operación Onerosa", false, false, true, false)]
    [InlineData("40", "Exportación de Bienes o Servicios", false, false, false, true)]
    [InlineData("  10  ", "Gravado - Operación Onerosa", true, false, false, false)]
    public void Create_ValidOfficialCodes_ExtractsAllProperties(
        string input,
        string expectedDescription,
        bool isTaxable,
        bool isExempt,
        bool isUnaffected,
        bool isExportation)
    {
        var result = AffectationTypeCode.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Code.Should().Be(input.Trim());
        result.Value.Description.Should().Be(expectedDescription);
        result.Value.IsTaxable.Should().Be(isTaxable);
        result.Value.IsExempt.Should().Be(isExempt);
        result.Value.IsUnaffected.Should().Be(isUnaffected);
        result.Value.IsExportation.Should().Be(isExportation);
        result.Value.ToString().Should().Be($"{input.Trim()} - {expectedDescription}");
    }

    [Fact]
    public void StaticFields_DefaultState_ExhaustiveVerification()
    {
        AffectationTypeCode.GravadoOneroso.Code.Should().Be("10");
        AffectationTypeCode.ExoneradoOneroso.Code.Should().Be("20");
        AffectationTypeCode.InafectoOneroso.Code.Should().Be("30");
        AffectationTypeCode.Exportacion.Code.Should().Be("40");
    }

    [Theory]
    [InlineData("99")]
    [InlineData("00")]
    [InlineData("invalid")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_InvalidCode_ReturnsError(string? input)
    {
        var result = AffectationTypeCode.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("AffectationTypeCode.InvalidCode");
    }

    [Fact]
    public void AffectationTypeCode_DefaultState_ComparisonOperators()
    {
        var a1 = AffectationTypeCode.GravadoOneroso;
        var a2 = AffectationTypeCode.Exportacion;
        var a1Clone = AffectationTypeCode.Create("10").Value;

        (a1 < a2).Should().BeTrue();
        (a1 <= a2).Should().BeTrue();
        (a2 > a1).Should().BeTrue();
        (a2 >= a1).Should().BeTrue();

        (a1 < a1Clone).Should().BeFalse();
        (a1 > a1Clone).Should().BeFalse();
        (a1 <= a1Clone).Should().BeTrue();
        (a1 >= a1Clone).Should().BeTrue();
        a1.CompareTo(a2).Should().BeNegative();
        a2.CompareTo(a1).Should().BePositive();
        a1.CompareTo(a1Clone).Should().Be(0);
    }

    [Fact]
    public void AffectationTypeCode_DefaultState_ParseAndTryParse()
    {
        var parsed1 = AffectationTypeCode.Parse("10", CultureInfo.InvariantCulture);
        parsed1.Code.Should().Be("10");

        var parsed2 = AffectationTypeCode.Parse("10".AsSpan(), CultureInfo.InvariantCulture);
        parsed2.Code.Should().Be("10");

        AffectationTypeCode.TryParse("10", null, out var tryRes1).Should().BeTrue();
        tryRes1.Code.Should().Be("10");

        AffectationTypeCode.TryParse("10".AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Code.Should().Be("10");

        Action invalidParseStr = () => AffectationTypeCode.Parse("99", CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid AffectationTypeCode: '99'.");

        Action invalidParseSpan = () => AffectationTypeCode.Parse("99".AsSpan(), CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid AffectationTypeCode: '99'.");

        AffectationTypeCode.TryParse("99", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(AffectationTypeCode));

        AffectationTypeCode.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(AffectationTypeCode));

        AffectationTypeCode.TryParse("99".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(AffectationTypeCode));
    }
}




