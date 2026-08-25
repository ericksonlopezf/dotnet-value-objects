// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Chile;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Chile.UnitTests;

public sealed class DteTypeCodeTests
{
    [Theory]
    [InlineData(33, "Factura Electrónica")]
    [InlineData(34, "Factura No Afecta o Exenta Electrónica")]
    [InlineData(39, "Boleta Electrónica")]
    [InlineData(41, "Boleta Exenta Electrónica")]
    [InlineData(46, "Factura de Compra Electrónica")]
    [InlineData(52, "Guía de Despacho Electrónica")]
    [InlineData(56, "Nota de Débito Electrónica")]
    [InlineData(61, "Nota de Crédito Electrónica")]
    [InlineData(110, "Factura de Exportación Electrónica")]
    [InlineData(111, "Nota de Débito de Exportación Electrónica")]
    [InlineData(112, "Nota de Crédito de Exportación Electrónica")]
    public void Create_ValidOfficialCodes_ExtractsAllProperties(int code, string expectedName)
    {
        var result = DteTypeCode.Create(code);

        result.IsSuccess.Should().BeTrue();
        result.Value.Code.Should().Be(code);
        result.Value.Name.Should().Be(expectedName);
        result.Value.ToString().Should().Be($"{code.ToString(CultureInfo.InvariantCulture)} - {expectedName}");
    }

    [Fact]
    public void StaticFields_DefaultState_ExhaustiveVerification()
    {
        DteTypeCode.FacturaElectronica.Code.Should().Be(33);
        DteTypeCode.FacturaExenta.Code.Should().Be(34);
        DteTypeCode.BoletaElectronica.Code.Should().Be(39);
        DteTypeCode.BoletaExenta.Code.Should().Be(41);
        DteTypeCode.FacturaCompra.Code.Should().Be(46);
        DteTypeCode.GuiaDespacho.Code.Should().Be(52);
        DteTypeCode.NotaDebito.Code.Should().Be(56);
        DteTypeCode.NotaCredito.Code.Should().Be(61);
        DteTypeCode.FacturaExportacion.Code.Should().Be(110);
        DteTypeCode.NotaDebitoExportacion.Code.Should().Be(111);
        DteTypeCode.NotaCreditoExportacion.Code.Should().Be(112);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(35)]
    [InlineData(999)]
    public void Create_InvalidCode_ReturnsError(int code)
    {
        var result = DteTypeCode.Create(code);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("DteTypeCode.InvalidCode");
    }

    [Theory]
    [InlineData("33", 33)]
    [InlineData("  33  ", 33)]
    [InlineData("61", 61)]
    [InlineData("110", 110)]
    public void Create_ValidString_Succeeds(string input, int expectedCode)
    {
        var result = DteTypeCode.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Code.Should().Be(expectedCode);
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("33.5")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_InvalidFormat_ReturnsError(string? input)
    {
        var result = DteTypeCode.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("DteTypeCode.InvalidFormat");
    }

    [Theory]
    [InlineData("999")]
    [InlineData("0")]
    public void Create_UnrecognizedCodeString_ReturnsError(string input)
    {
        var result = DteTypeCode.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("DteTypeCode.InvalidCode");
    }

    [Fact]
    public void DteTypeCode_DefaultState_ComparisonOperators()
    {
        var fe = DteTypeCode.FacturaElectronica;
        var nc = DteTypeCode.NotaCredito;
        var feClone = DteTypeCode.Create(33).Value;

        (fe < nc).Should().BeTrue();
        (fe <= nc).Should().BeTrue();
        (nc > fe).Should().BeTrue();
        (nc >= fe).Should().BeTrue();

        (fe < feClone).Should().BeFalse();
        (fe > feClone).Should().BeFalse();
        (fe <= feClone).Should().BeTrue();
        (fe >= feClone).Should().BeTrue();
        fe.CompareTo(nc).Should().BeNegative();
        nc.CompareTo(fe).Should().BePositive();
        fe.CompareTo(feClone).Should().Be(0);
    }

    [Fact]
    public void DteTypeCode_DefaultState_ParseAndTryParse()
    {
        var parsed1 = DteTypeCode.Parse("33", CultureInfo.InvariantCulture);
        parsed1.Code.Should().Be(33);

        var parsed2 = DteTypeCode.Parse("33".AsSpan(), CultureInfo.InvariantCulture);
        parsed2.Code.Should().Be(33);

        DteTypeCode.TryParse("33", null, out var tryRes1).Should().BeTrue();
        tryRes1.Code.Should().Be(33);

        DteTypeCode.TryParse("33".AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Code.Should().Be(33);

        Action invalidParseStr = () => DteTypeCode.Parse("999", CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid DteTypeCode: '999'.");

        Action invalidParseSpan = () => DteTypeCode.Parse("999".AsSpan(), CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid DteTypeCode: '999'.");

        DteTypeCode.TryParse("999", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(DteTypeCode));

        DteTypeCode.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(DteTypeCode));

        DteTypeCode.TryParse("999".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(DteTypeCode));
    }
}




