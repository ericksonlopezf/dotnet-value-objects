// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic.UnitTests;

public sealed class EcfInformationalSubtotalTests
{
    private static readonly CurrencyCode Dop = CurrencyCode.DOP;

    [Fact]
    public void Create_BalancedSubtotals_Succeeds()
    {
        // TaxedAmountTotal = 1000, TotalItbis = 180
        // ExemptAmount = 200
        // Total = 1000 + 180 + 200 = 1380
        var result = EcfInformationalSubtotal.Create(
            taxedAmountTotal: Money.Create(1000m, Dop).Value,
            taxedAmountReduced: Money.Create(0m, Dop).Value,
            exemptAmount: Money.Create(200m, Dop).Value,
            totalItbis: Money.Create(180m, Dop).Value,
            totalItbisReduced: Money.Create(0m, Dop).Value,
            nonBillableAmount: Money.Create(0m, Dop).Value,
            totalAmount: Money.Create(1380m, Dop).Value,
            nonTaxableAmount: Money.Create(0m, Dop).Value,
            iscAmount: Money.Create(0m, Dop).Value,
            otherTaxesAmount: Money.Create(0m, Dop).Value);

        result.IsSuccess.Should().BeTrue();
        result.Value.TotalAmount.Amount.Should().Be(1380m);
        result.Value.TaxedAmountTotal.Amount.Should().Be(1000m);
        result.Value.TaxedAmountReduced.Amount.Should().Be(0m);
        result.Value.ExemptAmount.Amount.Should().Be(200m);
        result.Value.TotalItbis.Amount.Should().Be(180m);
        result.Value.TotalItbisReduced.Amount.Should().Be(0m);
        result.Value.NonBillableAmount.Amount.Should().Be(0m);
        result.Value.NonTaxableAmount.Amount.Should().Be(0m);
        result.Value.IscAmount.Amount.Should().Be(0m);
        result.Value.OtherTaxesAmount.Amount.Should().Be(0m);
    }

    [Fact]
    public void Create_BalanceMismatch_ReturnsError()
    {
        // Stated total 2000 vs calculated 1380 (diff = 620 > tolerance 0.50)
        var result = EcfInformationalSubtotal.Create(
            taxedAmountTotal: Money.Create(1000m, Dop).Value,
            taxedAmountReduced: Money.Create(0m, Dop).Value,
            exemptAmount: Money.Create(200m, Dop).Value,
            totalItbis: Money.Create(180m, Dop).Value,
            totalItbisReduced: Money.Create(0m, Dop).Value,
            nonBillableAmount: Money.Create(0m, Dop).Value,
            totalAmount: Money.Create(2000m, Dop).Value,
            nonTaxableAmount: Money.Create(0m, Dop).Value,
            iscAmount: Money.Create(0m, Dop).Value,
            otherTaxesAmount: Money.Create(0m, Dop).Value);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("EcfInformationalSubtotal.BalanceMismatch");
    }

    [Fact]
    public void Create_CurrencyMismatch_ReturnsError()
    {
        var result = EcfInformationalSubtotal.Create(
            taxedAmountTotal: Money.Create(1000m, CurrencyCode.USD).Value,
            taxedAmountReduced: Money.Create(0m, Dop).Value,
            exemptAmount: Money.Create(200m, Dop).Value,
            totalItbis: Money.Create(180m, Dop).Value,
            totalItbisReduced: Money.Create(0m, Dop).Value,
            nonBillableAmount: Money.Create(0m, Dop).Value,
            totalAmount: Money.Create(1380m, Dop).Value,
            nonTaxableAmount: Money.Create(0m, Dop).Value,
            iscAmount: Money.Create(0m, Dop).Value,
            otherTaxesAmount: Money.Create(0m, Dop).Value);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("EcfInformationalSubtotal.CurrencyMismatch");
    }
}
