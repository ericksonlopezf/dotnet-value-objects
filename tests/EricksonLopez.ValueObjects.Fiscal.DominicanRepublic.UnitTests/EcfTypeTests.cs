// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic.UnitTests;

public sealed class EcfTypeTests
{
    [Fact]
    public void EcfType_Properties_AreAccurate()
    {
        // 31
        EcfType.ElectronicCreditFiscal.Code.Should().Be("31");
        EcfType.ElectronicCreditFiscal.Name.Should().Be("Factura de Crédito Fiscal Electrónica");
        EcfType.ElectronicCreditFiscal.Description.Should().Be("e-CF para sustentar costos, gastos y crédito fiscal de ITBIS.");
        EcfType.ElectronicCreditFiscal.TaxCreditEligible.Should().BeTrue();
        EcfType.ElectronicCreditFiscal.RequiresBuyerTaxpayerId.Should().BeTrue();

        // 32
        EcfType.ElectronicConsumer.Code.Should().Be("32");
        EcfType.ElectronicConsumer.Name.Should().Be("Factura de Consumo Electrónica");
        EcfType.ElectronicConsumer.Description.Should().Be("e-CF para consumidores finales.");
        EcfType.ElectronicConsumer.TaxCreditEligible.Should().BeFalse();
        EcfType.ElectronicConsumer.RequiresBuyerTaxpayerId.Should().BeFalse();

        // 33
        EcfType.ElectronicDebitNote.Code.Should().Be("33");
        EcfType.ElectronicDebitNote.Name.Should().Be("Nota de Débito Electrónica");
        EcfType.ElectronicDebitNote.Description.Should().Be("e-CF para aumentar el valor de comprobantes emitidos previamente.");
        EcfType.ElectronicDebitNote.TaxCreditEligible.Should().BeTrue();
        EcfType.ElectronicDebitNote.RequiresBuyerTaxpayerId.Should().BeTrue();

        // 34
        EcfType.ElectronicCreditNote.Code.Should().Be("34");
        EcfType.ElectronicCreditNote.Name.Should().Be("Nota de Crédito Electrónica");
        EcfType.ElectronicCreditNote.Description.Should().Be("e-CF para anular o disminuir el valor de comprobantes emitidos.");
        EcfType.ElectronicCreditNote.TaxCreditEligible.Should().BeTrue();
        EcfType.ElectronicCreditNote.RequiresBuyerTaxpayerId.Should().BeTrue();

        // 41
        EcfType.ElectronicPurchases.Code.Should().Be("41");
        EcfType.ElectronicPurchases.Name.Should().Be("Compras Electrónico");
        EcfType.ElectronicPurchases.Description.Should().Be("e-CF emitido por el comprador a proveedores informales.");
        EcfType.ElectronicPurchases.TaxCreditEligible.Should().BeTrue();
        EcfType.ElectronicPurchases.RequiresBuyerTaxpayerId.Should().BeFalse();

        // 43
        EcfType.ElectronicMinorExpenses.Code.Should().Be("43");
        EcfType.ElectronicMinorExpenses.Name.Should().Be("Gastos Menores Electrónico");
        EcfType.ElectronicMinorExpenses.Description.Should().Be("e-CF emitido para consumos menores y pagos de caja chica.");
        EcfType.ElectronicMinorExpenses.TaxCreditEligible.Should().BeTrue();
        EcfType.ElectronicMinorExpenses.RequiresBuyerTaxpayerId.Should().BeFalse();

        // 44
        EcfType.ElectronicSpecialRegimes.Code.Should().Be("44");
        EcfType.ElectronicSpecialRegimes.Name.Should().Be("Regímenes Especiales Electrónico");
        EcfType.ElectronicSpecialRegimes.Description.Should().Be("e-CF emitido a entidades acogidas a regímenes de exención fiscal.");
        EcfType.ElectronicSpecialRegimes.TaxCreditEligible.Should().BeTrue();
        EcfType.ElectronicSpecialRegimes.RequiresBuyerTaxpayerId.Should().BeTrue();

        // 45
        EcfType.ElectronicGovernmental.Code.Should().Be("45");
        EcfType.ElectronicGovernmental.Name.Should().Be("Gubernamental Electrónico");
        EcfType.ElectronicGovernmental.Description.Should().Be("e-CF emitido a instituciones del Estado dominicano.");
        EcfType.ElectronicGovernmental.TaxCreditEligible.Should().BeTrue();
        EcfType.ElectronicGovernmental.RequiresBuyerTaxpayerId.Should().BeTrue();

        // 46
        EcfType.ElectronicExports.Code.Should().Be("46");
        EcfType.ElectronicExports.Name.Should().Be("Exportaciones Electrónico");
        EcfType.ElectronicExports.Description.Should().Be("e-CF emitido para ventas al exterior exentas de ITBIS.");
        EcfType.ElectronicExports.TaxCreditEligible.Should().BeFalse();
        EcfType.ElectronicExports.RequiresBuyerTaxpayerId.Should().BeFalse();

        // 47
        EcfType.ElectronicForeignPayments.Code.Should().Be("47");
        EcfType.ElectronicForeignPayments.Name.Should().Be("Pagos al Exterior Electrónico");
        EcfType.ElectronicForeignPayments.Description.Should().Be("e-CF emitido para pagos por servicios al exterior sujetos a retención ISR.");
        EcfType.ElectronicForeignPayments.TaxCreditEligible.Should().BeTrue();
        EcfType.ElectronicForeignPayments.RequiresBuyerTaxpayerId.Should().BeFalse();
    }

    [Fact]
    public void Create_KnownCodes_ReturnInstances()
    {
        EcfType.Create("31").Value.Should().Be(EcfType.ElectronicCreditFiscal);
        EcfType.Create("32").Value.Should().Be(EcfType.ElectronicConsumer);
        EcfType.Create("33").Value.Should().Be(EcfType.ElectronicDebitNote);
        EcfType.Create("34").Value.Should().Be(EcfType.ElectronicCreditNote);
        EcfType.Create("41").Value.Should().Be(EcfType.ElectronicPurchases);
        EcfType.Create("43").Value.Should().Be(EcfType.ElectronicMinorExpenses);
        EcfType.Create("44").Value.Should().Be(EcfType.ElectronicSpecialRegimes);
        EcfType.Create("45").Value.Should().Be(EcfType.ElectronicGovernmental);
        EcfType.Create("46").Value.Should().Be(EcfType.ElectronicExports);
        EcfType.Create("47").Value.Should().Be(EcfType.ElectronicForeignPayments);

        EcfType.ElectronicCreditFiscal.Name.Should().Be("Factura de Crédito Fiscal Electrónica");
        EcfType.ElectronicCreditFiscal.Description.Should().Contain("crédito fiscal");
        EcfType.ElectronicCreditFiscal.ToString().Should().Be("31");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_NullOrWhitespace_ReturnsRequiredError(string? invalid)
    {
        var result = EcfType.Create(invalid);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("EcfType.Required");
    }

    [Fact]
    public void Create_UnknownCode_ReturnsInvalidError()
    {
        var result = EcfType.Create("99");
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("EcfType.Invalid");
        result.Error.Description.Should().Be("Unknown e-CF Type code '99'. Valid e-CF types are: 31, 32, 33, 34, 41, 43, 44, 45, 46, 47.");
    }


    [Fact]
    public void EcfType_ComparisonsAndOperators_Exhaustive()
    {
        var a = EcfType.ElectronicCreditFiscal; // "31"
        var aCopy = EcfType.Create("31").Value;
        var b = EcfType.ElectronicConsumer;     // "32"

        a.ShouldSatisfyEqualityContract(aCopy, b, (x, y) => x == y, (x, y) => x != y);
        a.ShouldSatisfyComparisonContract(aCopy, b,
            (x, y) => x < y,
            (x, y) => x <= y,
            (x, y) => x > y,
            (x, y) => x >= y);

        a.CompareTo((object)aCopy).Should().Be(0);

        Action invalidObj = () => a.CompareTo("not-an-ecftype");
        invalidObj.Should().Throw<ArgumentException>()
            .WithMessage("*Object is not an EcfType*");
    }
}





