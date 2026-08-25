// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Peru;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Peru.UnitTests;

public sealed class CpeTypeCodeTests
{
    [Theory]
    [InlineData("01", "Factura")]
    [InlineData("03", "Boleta de Venta")]
    [InlineData("07", "Nota de Crédito")]
    [InlineData("08", "Nota de Débito")]
    [InlineData("09", "Guía de Remisión Remitente")]
    [InlineData("31", "Guía de Remisión Transportista")]
    [InlineData("  01  ", "Factura")]
    public void Create_ValidOfficialCodes_ExtractsAllProperties(string input, string expectedName)
    {
        var result = CpeTypeCode.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Code.Should().Be(input.Trim());
        result.Value.Name.Should().Be(expectedName);
        result.Value.ToString().Should().Be($"{input.Trim()} - {expectedName}");
    }

    [Fact]
    public void StaticFields_DefaultState_ExhaustiveVerification()
    {
        CpeTypeCode.Factura.Code.Should().Be("01");
        CpeTypeCode.Boleta.Code.Should().Be("03");
        CpeTypeCode.NotaCredito.Code.Should().Be("07");
        CpeTypeCode.NotaDebito.Code.Should().Be("08");
        CpeTypeCode.GuiaRemitente.Code.Should().Be("09");
        CpeTypeCode.GuiaTransportista.Code.Should().Be("31");
    }

    [Theory]
    [InlineData("99")]
    [InlineData("00")]
    [InlineData("02")]
    [InlineData("invalid")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_InvalidCode_ReturnsError(string? input)
    {
        var result = CpeTypeCode.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("CpeTypeCode.InvalidCode");
    }

    [Fact]
    public void CpeTypeCode_DefaultState_ComparisonOperators()
    {
        var f = CpeTypeCode.Factura;
        var b = CpeTypeCode.Boleta;
        var fClone = CpeTypeCode.Create("01").Value;

        (f < b).Should().BeTrue();
        (f <= b).Should().BeTrue();
        (b > f).Should().BeTrue();
        (b >= f).Should().BeTrue();

        (f < fClone).Should().BeFalse();
        (f > fClone).Should().BeFalse();
        (f <= fClone).Should().BeTrue();
        (f >= fClone).Should().BeTrue();
        f.CompareTo(b).Should().BeNegative();
        b.CompareTo(f).Should().BePositive();
        f.CompareTo(fClone).Should().Be(0);
    }

    [Fact]
    public void CpeTypeCode_DefaultState_ParseAndTryParse()
    {
        var parsed1 = CpeTypeCode.Parse("01", CultureInfo.InvariantCulture);
        parsed1.Code.Should().Be("01");

        var parsed2 = CpeTypeCode.Parse("01".AsSpan(), CultureInfo.InvariantCulture);
        parsed2.Code.Should().Be("01");

        CpeTypeCode.TryParse("01", null, out var tryRes1).Should().BeTrue();
        tryRes1.Code.Should().Be("01");

        CpeTypeCode.TryParse("01".AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Code.Should().Be("01");

        Action invalidParseStr = () => CpeTypeCode.Parse("99", CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid CpeTypeCode: '99'.");

        Action invalidParseSpan = () => CpeTypeCode.Parse("99".AsSpan(), CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid CpeTypeCode: '99'.");

        CpeTypeCode.TryParse("99", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(CpeTypeCode));

        CpeTypeCode.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(CpeTypeCode));

        CpeTypeCode.TryParse("99".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(CpeTypeCode));
    }
}




