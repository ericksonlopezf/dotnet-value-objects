// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Colombia;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Colombia.UnitTests;

public sealed class TaxTypeCodeTests
{
    [Theory]
    [InlineData("01", "IVA (Impuesto sobre las Ventas)", true, false, false, false, false)]
    [InlineData("02", "INC (Impuesto Nacional al Consumo)", false, true, false, false, false)]
    [InlineData("03", "ICA (Impuesto de Industria y Comercio)", false, false, true, false, false)]
    [InlineData("22", "IBUA (Bebidas Ultraprocesadas Azucaradas)", false, false, false, true, false)]
    [InlineData("23", "ICUI (Comestibles Ultraprocesados)", false, false, false, false, true)]
    [InlineData("  01  ", "IVA (Impuesto sobre las Ventas)", true, false, false, false, false)]
    public void Create_ValidOfficialCodes_ExtractsAllProperties(
        string input,
        string expectedDescription,
        bool isIva,
        bool isInc,
        bool isIca,
        bool isIbua,
        bool isIcui)
    {
        var result = TaxTypeCode.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Code.Should().Be(input.Trim());
        result.Value.Description.Should().Be(expectedDescription);
        result.Value.IsIva.Should().Be(isIva);
        result.Value.IsInc.Should().Be(isInc);
        result.Value.IsIca.Should().Be(isIca);
        result.Value.IsIbua.Should().Be(isIbua);
        result.Value.IsIcui.Should().Be(isIcui);
        result.Value.ToString().Should().Be($"{input.Trim()} - {expectedDescription}");
    }

    [Fact]
    public void StaticFields_DefaultState_ExhaustiveVerification()
    {
        TaxTypeCode.Iva.Code.Should().Be("01");
        TaxTypeCode.Inc.Code.Should().Be("02");
        TaxTypeCode.Ica.Code.Should().Be("03");
        TaxTypeCode.Ibua.Code.Should().Be("22");
        TaxTypeCode.Icui.Code.Should().Be("23");
    }

    [Theory]
    [InlineData("99")]
    [InlineData("00")]
    [InlineData("04")]
    [InlineData("invalid")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_InvalidCode_ReturnsError(string? input)
    {
        var result = TaxTypeCode.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("TaxTypeCode.InvalidCode");
    }

    [Fact]
    public void TaxTypeCode_DefaultState_Equality()
    {
        var t1 = TaxTypeCode.Iva;
        var t2 = TaxTypeCode.Create("01").Value;
        var tDiff = TaxTypeCode.Inc;

        (t1 == t2).Should().BeTrue();
        (t1 != tDiff).Should().BeTrue();
        t1.Equals(t2).Should().BeTrue();
        t1.Equals((object)t2).Should().BeTrue();
        t1.Equals(tDiff).Should().BeFalse();
        t1.GetHashCode().Should().Be(t2.GetHashCode());
    }

    [Fact]
    public void TaxTypeCode_DefaultState_ParseAndTryParse()
    {
        var parsed1 = TaxTypeCode.Parse("01", CultureInfo.InvariantCulture);
        parsed1.Code.Should().Be("01");

        var parsed2 = TaxTypeCode.Parse("01".AsSpan(), CultureInfo.InvariantCulture);
        parsed2.Code.Should().Be("01");

        TaxTypeCode.TryParse("01", null, out var tryRes1).Should().BeTrue();
        tryRes1.Code.Should().Be("01");

        TaxTypeCode.TryParse("01".AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Code.Should().Be("01");

        Action invalidParseStr = () => TaxTypeCode.Parse("99", CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid DIAN tax code: '99'.");

        Action invalidParseSpan = () => TaxTypeCode.Parse("99".AsSpan(), CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid DIAN tax code: '99'.");

        TaxTypeCode.TryParse("99", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(TaxTypeCode));

        TaxTypeCode.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(TaxTypeCode));

        TaxTypeCode.TryParse("99".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(TaxTypeCode));
    }
}




