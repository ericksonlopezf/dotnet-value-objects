// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Mexico;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Mexico.UnitTests;

public sealed class CfdiUsageCodeTests
{
    [Theory]
    [InlineData("G01", "Adquisición de mercancías")]
    [InlineData("G02", "Devoluciones, descuentos o bonificaciones")]
    [InlineData("G03", "Gastos en general")]
    [InlineData("CP01", "Pagos")]
    [InlineData("CN01", "Nómina")]
    [InlineData("S01", "Sin efectos fiscales")]
    [InlineData("  G01  ", "Adquisición de mercancías")]
    public void Create_ValidOfficialCodes_ExtractsAllProperties(string input, string expectedDescription)
    {
        var result = CfdiUsageCode.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Code.Should().Be(input.Trim());
        result.Value.Description.Should().Be(expectedDescription);
        result.Value.ToString().Should().Be($"{input.Trim()} - {expectedDescription}");
    }

    [Fact]
    public void StaticFields_DefaultState_ExhaustiveVerification()
    {
        CfdiUsageCode.GoodsAcquisition.Code.Should().Be("G01");
        CfdiUsageCode.ReturnsDiscounts.Code.Should().Be("G02");
        CfdiUsageCode.GeneralExpenses.Code.Should().Be("G03");
        CfdiUsageCode.Payments.Code.Should().Be("CP01");
        CfdiUsageCode.Payroll.Code.Should().Be("CN01");
        CfdiUsageCode.WithoutTaxEffects.Code.Should().Be("S01");
    }

    [Theory]
    [InlineData("G0")]
    [InlineData("G0422")]
    [InlineData("---")]
    [InlineData("invalid")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_InvalidFormat_ReturnsError(string? input)
    {
        var result = CfdiUsageCode.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().StartWith("CfdiUsageCode.Invalid");
    }

    [Theory]
    [InlineData("G04")]
    [InlineData("XXX")]
    [InlineData("D04")]
    public void Create_UnknownButValidFormatCode_ReturnsDynamicCatalog(string input)
    {
        var result = CfdiUsageCode.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Code.Should().Be(input);
        result.Value.Description.Should().Be("Uso CFDI (Catálogo Dinámico)");
    }

    [Fact]
    public void CfdiUsageCode_DefaultState_ComparisonOperators()
    {
        var u1 = CfdiUsageCode.GoodsAcquisition;
        var u2 = CfdiUsageCode.GeneralExpenses;
        var u1Clone = CfdiUsageCode.Create("G01").Value;

        (u1 < u2).Should().BeTrue();
        (u1 <= u2).Should().BeTrue();
        (u2 > u1).Should().BeTrue();
        (u2 >= u1).Should().BeTrue();

        (u1 < u1Clone).Should().BeFalse();
        (u1 > u1Clone).Should().BeFalse();
        (u1 <= u1Clone).Should().BeTrue();
        (u1 >= u1Clone).Should().BeTrue();
        u1.CompareTo(u2).Should().BeNegative();
        u2.CompareTo(u1).Should().BePositive();
        u1.CompareTo(u1Clone).Should().Be(0);
    }

    [Fact]
    public void CfdiUsageCode_DefaultState_ParseAndTryParse()
    {
        var parsed1 = CfdiUsageCode.Parse("G01", CultureInfo.InvariantCulture);
        parsed1.Code.Should().Be("G01");

        var parsed2 = CfdiUsageCode.Parse("G01".AsSpan(), CultureInfo.InvariantCulture);
        parsed2.Code.Should().Be("G01");

        CfdiUsageCode.TryParse("G01", null, out var tryRes1).Should().BeTrue();
        tryRes1.Code.Should().Be("G01");

        CfdiUsageCode.TryParse("G01".AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Code.Should().Be("G01");

        Action invalidParseStr = () => CfdiUsageCode.Parse("invalid", CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid CfdiUsageCode: 'invalid'.");

        Action invalidParseSpan = () => CfdiUsageCode.Parse("invalid".AsSpan(), CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid CfdiUsageCode: 'invalid'.");

        CfdiUsageCode.TryParse("invalid", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(CfdiUsageCode));

        CfdiUsageCode.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(CfdiUsageCode));

        CfdiUsageCode.TryParse("invalid".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(CfdiUsageCode));
    }
}




