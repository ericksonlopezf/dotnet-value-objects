// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Mexico;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Mexico.UnitTests;

public sealed class TaxRegimeCodeTests
{
    [Theory]
    [InlineData("601", "General de Ley Personas Morales", false, true)]
    [InlineData("605", "Sueldos y Salarios", true, false)]
    [InlineData("606", "Arrendamiento", true, false)]
    [InlineData("612", "Personas Físicas con Actividades Empresariales y Profesionales", true, false)]
    [InlineData("626", "Régimen Simplificado de Confianza", true, true)]
    [InlineData("  601  ", "General de Ley Personas Morales", false, true)]
    public void Create_ValidOfficialCodes_ExtractsAllProperties(
        string input,
        string expectedDescription,
        bool appliesToPhysical,
        bool appliesToMoral)
    {
        var result = TaxRegimeCode.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Code.Should().Be(input.Trim());
        result.Value.Description.Should().Be(expectedDescription);
        result.Value.AppliesToPhysical.Should().Be(appliesToPhysical);
        result.Value.AppliesToMoral.Should().Be(appliesToMoral);
        result.Value.ToString().Should().Be($"{input.Trim()} - {expectedDescription}");
    }

    [Fact]
    public void StaticFields_DefaultState_ExhaustiveVerification()
    {
        TaxRegimeCode.GeneralPersonasMorales.Code.Should().Be("601");
        TaxRegimeCode.SueldosYSalarios.Code.Should().Be("605");
        TaxRegimeCode.Arrendamiento.Code.Should().Be("606");
        TaxRegimeCode.ActividadesEmpresariales.Code.Should().Be("612");
        TaxRegimeCode.Resico.Code.Should().Be("626");
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("60")]
    [InlineData("6000")]
    [InlineData("ABC")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_InvalidFormat_ReturnsError(string? input)
    {
        var result = TaxRegimeCode.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().StartWith("TaxRegimeCode.Invalid");
    }

    [Theory]
    [InlineData("999")]
    [InlineData("000")]
    [InlineData("600")]
    public void Create_UnknownButValidFormatCode_ReturnsDynamicCatalog(string input)
    {
        var result = TaxRegimeCode.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Code.Should().Be(input);
        result.Value.Description.Should().Be("Régimen Fiscal (Catálogo Dinámico)");
        result.Value.AppliesToPhysical.Should().BeFalse();
        result.Value.AppliesToMoral.Should().BeFalse();
    }

    [Fact]
    public void TaxRegimeCode_DefaultState_ComparisonOperators()
    {
        var r1 = TaxRegimeCode.GeneralPersonasMorales;
        var r2 = TaxRegimeCode.Resico;
        var r1Clone = TaxRegimeCode.Create("601").Value;

        (r1 < r2).Should().BeTrue();
        (r1 <= r2).Should().BeTrue();
        (r2 > r1).Should().BeTrue();
        (r2 >= r1).Should().BeTrue();

        (r1 < r1Clone).Should().BeFalse();
        (r1 > r1Clone).Should().BeFalse();
        (r1 <= r1Clone).Should().BeTrue();
        (r1 >= r1Clone).Should().BeTrue();
        r1.CompareTo(r2).Should().BeNegative();
        r2.CompareTo(r1).Should().BePositive();
        r1.CompareTo(r1Clone).Should().Be(0);
    }

    [Fact]
    public void TaxRegimeCode_DefaultState_ParseAndTryParse()
    {
        var parsed1 = TaxRegimeCode.Parse("601", CultureInfo.InvariantCulture);
        parsed1.Code.Should().Be("601");

        var parsed2 = TaxRegimeCode.Parse("601".AsSpan(), CultureInfo.InvariantCulture);
        parsed2.Code.Should().Be("601");

        TaxRegimeCode.TryParse("601", null, out var tryRes1).Should().BeTrue();
        tryRes1.Code.Should().Be("601");

        TaxRegimeCode.TryParse("601".AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Code.Should().Be("601");

        Action invalidParseStr = () => TaxRegimeCode.Parse("invalid", CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid TaxRegimeCode: 'invalid'.");

        Action invalidParseSpan = () => TaxRegimeCode.Parse("invalid".AsSpan(), CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid TaxRegimeCode: 'invalid'.");

        TaxRegimeCode.TryParse("invalid", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(TaxRegimeCode));

        TaxRegimeCode.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(TaxRegimeCode));

        TaxRegimeCode.TryParse("invalid".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(TaxRegimeCode));
    }
}




