// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic.UnitTests;

public sealed class NcfTypeTests
{
    [Fact]
    public void NcfType_TaxCreditProperties_AreAccurate()
    {
        // 01
        NcfType.CreditFiscal.Code.Should().Be("01");
        NcfType.CreditFiscal.Name.Should().Be("Factura de Crédito Fiscal");
        NcfType.CreditFiscal.Description.Should().Be("Válido para crédito fiscal y deducción de costos y gastos.");
        NcfType.CreditFiscal.TaxCreditEligible.Should().BeTrue();
        NcfType.CreditFiscal.RequiresBuyerTaxpayerId.Should().BeTrue();

        // 02
        NcfType.Consumer.Code.Should().Be("02");
        NcfType.Consumer.Name.Should().Be("Factura de Consumo");
        NcfType.Consumer.Description.Should().Be("Válido para consumidores finales. No deduce crédito fiscal.");
        NcfType.Consumer.TaxCreditEligible.Should().BeFalse();
        NcfType.Consumer.RequiresBuyerTaxpayerId.Should().BeFalse();

        // 03
        NcfType.DebitNote.Code.Should().Be("03");
        NcfType.DebitNote.Name.Should().Be("Nota de Débito");
        NcfType.DebitNote.Description.Should().Be("Modifica el valor de comprobantes previamente emitidos aumentando el balance.");
        NcfType.DebitNote.TaxCreditEligible.Should().BeTrue();
        NcfType.DebitNote.RequiresBuyerTaxpayerId.Should().BeTrue();

        // 04
        NcfType.CreditNote.Code.Should().Be("04");
        NcfType.CreditNote.Name.Should().Be("Nota de Crédito");
        NcfType.CreditNote.Description.Should().Be("Modifica el valor de comprobantes previamente emitidos disminuyendo el balance.");
        NcfType.CreditNote.TaxCreditEligible.Should().BeTrue();
        NcfType.CreditNote.RequiresBuyerTaxpayerId.Should().BeTrue();

        // 11
        NcfType.InformalSuppliers.Code.Should().Be("11");
        NcfType.InformalSuppliers.Name.Should().Be("Registro de Proveedores Informales");
        NcfType.InformalSuppliers.Description.Should().Be("Emitido por el comprador para compras a personas no registradas en DGII.");
        NcfType.InformalSuppliers.TaxCreditEligible.Should().BeTrue();
        NcfType.InformalSuppliers.RequiresBuyerTaxpayerId.Should().BeFalse();

        // 12
        NcfType.SingleIncomeRegister.Code.Should().Be("12");
        NcfType.SingleIncomeRegister.Name.Should().Be("Registro Único de Ingresos");
        NcfType.SingleIncomeRegister.Description.Should().Be("Resumen diario de ventas a consumidores por debajo del umbral de identificación.");
        NcfType.SingleIncomeRegister.TaxCreditEligible.Should().BeFalse();
        NcfType.SingleIncomeRegister.RequiresBuyerTaxpayerId.Should().BeFalse();

        // 13
        NcfType.MinorExpenses.Code.Should().Be("13");
        NcfType.MinorExpenses.Name.Should().Be("Registro de Gastos Menores");
        NcfType.MinorExpenses.Description.Should().Be("Comprobante emitido para compras menores de caja chica.");
        NcfType.MinorExpenses.TaxCreditEligible.Should().BeTrue();
        NcfType.MinorExpenses.RequiresBuyerTaxpayerId.Should().BeFalse();

        // 14
        NcfType.SpecialRegimes.Code.Should().Be("14");
        NcfType.SpecialRegimes.Name.Should().Be("Regímenes Especiales");
        NcfType.SpecialRegimes.Description.Should().Be("Emitido a contribuyentes acogidos a regímenes de exención (Zonas Francas, etc.).");
        NcfType.SpecialRegimes.TaxCreditEligible.Should().BeTrue();
        NcfType.SpecialRegimes.RequiresBuyerTaxpayerId.Should().BeTrue();

        // 15
        NcfType.Governmental.Code.Should().Be("15");
        NcfType.Governmental.Name.Should().Be("Gubernamental");
        NcfType.Governmental.Description.Should().Be("Emitido a instituciones del Estado dominicano.");
        NcfType.Governmental.TaxCreditEligible.Should().BeTrue();
        NcfType.Governmental.RequiresBuyerTaxpayerId.Should().BeTrue();

        // 16
        NcfType.Exports.Code.Should().Be("16");
        NcfType.Exports.Name.Should().Be("Exportaciones");
        NcfType.Exports.Description.Should().Be("Emitido para ventas de bienes o servicios al extranjero exentas de ITBIS.");
        NcfType.Exports.TaxCreditEligible.Should().BeFalse();
        NcfType.Exports.RequiresBuyerTaxpayerId.Should().BeFalse();

        // 17
        NcfType.ForeignPayments.Code.Should().Be("17");
        NcfType.ForeignPayments.Name.Should().Be("Pagos al Exterior");
        NcfType.ForeignPayments.Description.Should().Be("Emitido para pagos por servicios prestados desde el exterior con retención ISR.");
        NcfType.ForeignPayments.TaxCreditEligible.Should().BeTrue();
        NcfType.ForeignPayments.RequiresBuyerTaxpayerId.Should().BeFalse();
    }

    [Fact]
    public void Create_KnownCodes_ReturnExactInstances()
    {
        NcfType.Create("01").Value.Should().Be(NcfType.CreditFiscal);
        NcfType.Create("02").Value.Should().Be(NcfType.Consumer);
        NcfType.Create("03").Value.Should().Be(NcfType.DebitNote);
        NcfType.Create("04").Value.Should().Be(NcfType.CreditNote);
        NcfType.Create("11").Value.Should().Be(NcfType.InformalSuppliers);
        NcfType.Create("12").Value.Should().Be(NcfType.SingleIncomeRegister);
        NcfType.Create("13").Value.Should().Be(NcfType.MinorExpenses);
        NcfType.Create("14").Value.Should().Be(NcfType.SpecialRegimes);
        NcfType.Create("15").Value.Should().Be(NcfType.Governmental);
        NcfType.Create("16").Value.Should().Be(NcfType.Exports);
        NcfType.Create("17").Value.Should().Be(NcfType.ForeignPayments);

        NcfType.CreditFiscal.Name.Should().Be("Factura de Crédito Fiscal");
        NcfType.CreditFiscal.Description.Should().Contain("crédito fiscal");
        NcfType.CreditFiscal.ToString().Should().Be("01");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_NullOrWhitespace_ReturnsRequiredError(string? invalid)
    {
        var result = NcfType.Create(invalid);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("NcfType.Required");
    }

    [Fact]
    public void Create_UnknownCode_ReturnsInvalidError()
    {
        var result = NcfType.Create("99");
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("NcfType.Invalid");
        result.Error.Description.Should().Be("Unknown NCF Type code '99'. Valid types are: 01, 02, 03, 04, 11, 12, 13, 14, 15, 16, 17.");
    }


    [Fact]
    public void NcfType_ComparisonsAndOperators_Exhaustive()
    {
        var a = NcfType.CreditFiscal; // "01"
        var aCopy = NcfType.Create("01").Value;
        var b = NcfType.Consumer;     // "02"

        a.ShouldSatisfyEqualityContract(aCopy, b, (x, y) => x == y, (x, y) => x != y);
        a.ShouldSatisfyComparisonContract(aCopy, b,
            (x, y) => x < y,
            (x, y) => x <= y,
            (x, y) => x > y,
            (x, y) => x >= y);

        a.CompareTo((object)aCopy).Should().Be(0);

        Action invalidObj = () => a.CompareTo("not-an-ncftype");
        invalidObj.Should().Throw<ArgumentException>()
            .WithMessage("*Object is not an NcfType*");
    }
}





