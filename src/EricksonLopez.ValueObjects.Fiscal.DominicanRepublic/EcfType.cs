// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;

/// <summary>
/// Represents the Dominican Republic Electronic NCF (e-CF) Type (Serie E) established under Law 32-23.
/// </summary>
public readonly record struct EcfType : IComparable<EcfType>, IComparable
{
    /// <summary>Gets the 2-digit e-CF type code (e.g. "31").</summary>
    public string Code { get; }
    /// <summary>Gets the official name of the e-CF type.</summary>
    public string Name { get; }
    /// <summary>Gets the description and statutory purpose of the e-CF type.</summary>
    public string Description { get; }

    /// <summary>Indicates if this electronic invoice type confers tax credit (deductible for ITBIS and ISR).</summary>
    public bool TaxCreditEligible { get; }

    /// <summary>Indicates if the buyer's RNC/Cédula is legally mandatory on this invoice type.</summary>
    public bool RequiresBuyerTaxpayerId { get; }

    private EcfType(string code, string name, string description, bool taxCreditEligible, bool requiresBuyerTaxpayerId)
    {
        Code = code;
        Name = name;
        Description = description;
        TaxCreditEligible = taxCreditEligible;
        RequiresBuyerTaxpayerId = requiresBuyerTaxpayerId;
    }

    /// <summary>Gets e-CF type 31 (Factura de Crédito Fiscal Electrónica).</summary>
    public static EcfType ElectronicCreditFiscal => new("31", "Factura de Crédito Fiscal Electrónica", "e-CF para sustentar costos, gastos y crédito fiscal de ITBIS.", true, true);
    /// <summary>Gets e-CF type 32 (Factura de Consumo Electrónica).</summary>
    public static EcfType ElectronicConsumer => new("32", "Factura de Consumo Electrónica", "e-CF para consumidores finales.", false, false);
    /// <summary>Gets e-CF type 33 (Nota de Débito Electrónica).</summary>
    public static EcfType ElectronicDebitNote => new("33", "Nota de Débito Electrónica", "e-CF para aumentar el valor de comprobantes emitidos previamente.", true, true);
    /// <summary>Gets e-CF type 34 (Nota de Crédito Electrónica).</summary>
    public static EcfType ElectronicCreditNote => new("34", "Nota de Crédito Electrónica", "e-CF para anular o disminuir el valor de comprobantes emitidos.", true, true);
    /// <summary>Gets e-CF type 41 (Compras Electrónico).</summary>
    public static EcfType ElectronicPurchases => new("41", "Compras Electrónico", "e-CF emitido por el comprador a proveedores informales.", true, false);
    /// <summary>Gets e-CF type 43 (Gastos Menores Electrónico).</summary>
    public static EcfType ElectronicMinorExpenses => new("43", "Gastos Menores Electrónico", "e-CF emitido para consumos menores y pagos de caja chica.", true, false);
    /// <summary>Gets e-CF type 44 (Regímenes Especiales Electrónico).</summary>
    public static EcfType ElectronicSpecialRegimes => new("44", "Regímenes Especiales Electrónico", "e-CF emitido a entidades acogidas a regímenes de exención fiscal.", true, true);
    /// <summary>Gets e-CF type 45 (Gubernamental Electrónico).</summary>
    public static EcfType ElectronicGovernmental => new("45", "Gubernamental Electrónico", "e-CF emitido a instituciones del Estado dominicano.", true, true);
    /// <summary>Gets e-CF type 46 (Exportaciones Electrónico).</summary>
    public static EcfType ElectronicExports => new("46", "Exportaciones Electrónico", "e-CF emitido para ventas al exterior exentas de ITBIS.", false, false);
    /// <summary>Gets e-CF type 47 (Pagos al Exterior Electrónico).</summary>
    public static EcfType ElectronicForeignPayments => new("47", "Pagos al Exterior Electrónico", "e-CF emitido para pagos por servicios al exterior sujetos a retención ISR.", true, false);

    /// <summary>
    /// Creates a validated <see cref="EcfType"/> from a 2-digit code string.
    /// </summary>
    /// <param name="code">The 2-digit e-CF type code.</param>
    /// <returns>A <see cref="Result{EcfType}"/> containing the created instance or a validation error.</returns>
    public static Result<EcfType> Create(string? code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return Result<EcfType>.Failure(Error.Validation(
                "EcfType.Required", "e-CF Type code is required."));
        }

        string trimmed = code.Trim();
        return trimmed switch
        {
            "31" => Result<EcfType>.Success(ElectronicCreditFiscal),
            "32" => Result<EcfType>.Success(ElectronicConsumer),
            "33" => Result<EcfType>.Success(ElectronicDebitNote),
            "34" => Result<EcfType>.Success(ElectronicCreditNote),
            "41" => Result<EcfType>.Success(ElectronicPurchases),
            "43" => Result<EcfType>.Success(ElectronicMinorExpenses),
            "44" => Result<EcfType>.Success(ElectronicSpecialRegimes),
            "45" => Result<EcfType>.Success(ElectronicGovernmental),
            "46" => Result<EcfType>.Success(ElectronicExports),
            "47" => Result<EcfType>.Success(ElectronicForeignPayments),
            _ => Result<EcfType>.Failure(Error.Validation(
                "EcfType.Invalid",
                $"Unknown e-CF Type code '{trimmed}'. Valid e-CF types are: 31, 32, 33, 34, 41, 43, 44, 45, 46, 47."))
        };
    }

    /// <inheritdoc/>
    public int CompareTo(EcfType other) => string.Compare(Code, other.Code, StringComparison.Ordinal);

    /// <inheritdoc/>
    public int CompareTo(object? obj) =>
        obj is EcfType other ? CompareTo(other) : throw new ArgumentException("Object is not an EcfType", nameof(obj));

        /// <summary>
    /// Determines whether the left <see cref="EcfType"/> is less than the right <see cref="EcfType"/>.
    /// </summary>
    /// <param name="left">The first <see cref="EcfType"/> to compare.</param>
    /// <param name="right">The second <see cref="EcfType"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(EcfType left, EcfType right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="EcfType"/> is less than or equal to the right <see cref="EcfType"/>.
    /// </summary>
    /// <param name="left">The first <see cref="EcfType"/> to compare.</param>
    /// <param name="right">The second <see cref="EcfType"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(EcfType left, EcfType right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="EcfType"/> is greater than the right <see cref="EcfType"/>.
    /// </summary>
    /// <param name="left">The first <see cref="EcfType"/> to compare.</param>
    /// <param name="right">The second <see cref="EcfType"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(EcfType left, EcfType right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="EcfType"/> is greater than or equal to the right <see cref="EcfType"/>.
    /// </summary>
    /// <param name="left">The first <see cref="EcfType"/> to compare.</param>
    /// <param name="right">The second <see cref="EcfType"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(EcfType left, EcfType right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public override string ToString() => Code;
}


