// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.Attributes;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;

/// <summary>
/// DGII-required informational subtotals and balance invariant for electronic invoice (e-CF) documents
/// according to Norma General 05-2019 and Law 32-23.
/// </summary>
[RegulatoryRule("DO.ECF.TOT.001")]
public sealed record EcfInformationalSubtotal : ValueObject
{
    /// <summary>Maximum allowed rounding tolerance in DGII tax calculations (RD$ 0.50).</summary>
    public static readonly decimal DgiiTolerance = 0.50m;

    /// <summary>Sum of items taxed at standard 18% ITBIS rate.</summary>
    public Money TaxedAmountTotal { get; }

    /// <summary>Sum of items taxed at reduced 16% ITBIS rate (tourism / food service).</summary>
    public Money TaxedAmountReduced { get; }

    /// <summary>Sum of exempt items (Monto Exento).</summary>
    public Money ExemptAmount { get; }

    /// <summary>Total ITBIS on 18% items.</summary>
    public Money TotalItbis { get; }

    /// <summary>Total ITBIS on 16% items.</summary>
    public Money TotalItbisReduced { get; }

    /// <summary>Non-billable amount (tips, deposits, retenciones legales informativas).</summary>
    public Money NonBillableAmount { get; }

    /// <summary>Grand total of the document (Monto Total).</summary>
    public Money TotalAmount { get; }

    /// <summary>Non-taxable amount (Monto No Gravado).</summary>
    public Money NonTaxableAmount { get; }

    /// <summary>Impuesto Selectivo al Consumo (ISC - Excise tax).</summary>
    public Money IscAmount { get; }

    /// <summary>Other taxes not classified in standard buckets (Otros Impuestos / Tasas).</summary>
    public Money OtherTaxesAmount { get; }

    private EcfInformationalSubtotal(
        Money taxedAmountTotal,
        Money taxedAmountReduced,
        Money exemptAmount,
        Money totalItbis,
        Money totalItbisReduced,
        Money nonBillableAmount,
        Money totalAmount,
        Money nonTaxableAmount,
        Money iscAmount,
        Money otherTaxesAmount)
    {
        TaxedAmountTotal = taxedAmountTotal;
        TaxedAmountReduced = taxedAmountReduced;
        ExemptAmount = exemptAmount;
        TotalItbis = totalItbis;
        TotalItbisReduced = totalItbisReduced;
        NonBillableAmount = nonBillableAmount;
        TotalAmount = totalAmount;
        NonTaxableAmount = nonTaxableAmount;
        IscAmount = iscAmount;
        OtherTaxesAmount = otherTaxesAmount;
    }

    /// <summary>
    /// Creates a validated <see cref="EcfInformationalSubtotal"/> instance enforcing DGII balance invariants.
    /// </summary>
    public static Result<EcfInformationalSubtotal> Create(
        Money taxedAmountTotal,
        Money taxedAmountReduced,
        Money exemptAmount,
        Money totalItbis,
        Money totalItbisReduced,
        Money nonBillableAmount,
        Money totalAmount,
        Money nonTaxableAmount,
        Money iscAmount,
        Money otherTaxesAmount)
    {
        CurrencyCode currency = totalAmount.Currency;

        if (taxedAmountTotal.Currency != currency ||
            taxedAmountReduced.Currency != currency ||
            exemptAmount.Currency != currency ||
            totalItbis.Currency != currency ||
            totalItbisReduced.Currency != currency ||
            nonBillableAmount.Currency != currency ||
            nonTaxableAmount.Currency != currency ||
            iscAmount.Currency != currency ||
            otherTaxesAmount.Currency != currency)
        {
            return Result<EcfInformationalSubtotal>.Failure(Error.Validation(
                "EcfInformationalSubtotal.CurrencyMismatch", "All subtotal fields must share the exact same currency code."));
        }

        // DGII Balance Invariant:
        // TotalAmount = TaxedAmountTotal + TaxedAmountReduced + ExemptAmount + NonTaxableAmount + TotalItbis + TotalItbisReduced + IscAmount + OtherTaxesAmount + NonBillableAmount
        decimal calculatedTotal = taxedAmountTotal.Amount
            + taxedAmountReduced.Amount
            + exemptAmount.Amount
            + nonTaxableAmount.Amount
            + totalItbis.Amount
            + totalItbisReduced.Amount
            + iscAmount.Amount
            + otherTaxesAmount.Amount
            + nonBillableAmount.Amount;

        decimal difference = Math.Abs(totalAmount.Amount - calculatedTotal);
        if (difference > DgiiTolerance)
        {
            return Result<EcfInformationalSubtotal>.Failure(Error.Validation(
                "EcfInformationalSubtotal.BalanceMismatch",
                $"DGII e-CF totals balance error. Stated total: {totalAmount.Amount}, Calculated sum: {calculatedTotal}, Difference: {difference} exceeds tolerance {DgiiTolerance}."));
        }

        return Result<EcfInformationalSubtotal>.Success(new EcfInformationalSubtotal(
            taxedAmountTotal, taxedAmountReduced, exemptAmount,
            totalItbis, totalItbisReduced, nonBillableAmount,
            totalAmount, nonTaxableAmount, iscAmount, otherTaxesAmount));
    }
}
