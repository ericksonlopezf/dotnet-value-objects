// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;

/// <summary>
/// Represents the standard Dominican Republic NCF (Número de Comprobante Fiscal) Type (Serie B).
/// </summary>
public readonly record struct NcfType : IComparable<NcfType>, IComparable
{
    /// <summary>Gets the 2-digit NCF type code (e.g. "01").</summary>
    public string Code { get; }
    /// <summary>Gets the official name of the NCF type.</summary>
    public string Name { get; }
    /// <summary>Gets the description and statutory purpose of the NCF type.</summary>
    public string Description { get; }

    /// <summary>Indicates if this invoice type confers tax credit (deductible for ITBIS and ISR).</summary>
    public bool TaxCreditEligible { get; }

    /// <summary>Indicates if the buyer's RNC/Cédula is legally mandatory on this invoice type.</summary>
    public bool RequiresBuyerTaxpayerId { get; }

    private NcfType(string code, string name, string description, bool taxCreditEligible, bool requiresBuyerTaxpayerId)
    {
        Code = code;
        Name = name;
        Description = description;
        TaxCreditEligible = taxCreditEligible;
        RequiresBuyerTaxpayerId = requiresBuyerTaxpayerId;
    }

    /// <summary>Gets NCF type 01 (Factura de Crédito Fiscal).</summary>
    public static NcfType CreditFiscal => new("01", "Factura de Crédito Fiscal", "Válido para crédito fiscal y deducción de costos y gastos.", true, true);
    /// <summary>Gets NCF type 02 (Factura de Consumo).</summary>
    public static NcfType Consumer => new("02", "Factura de Consumo", "Válido para consumidores finales. No deduce crédito fiscal.", false, false);
    /// <summary>Gets NCF type 03 (Nota de Débito).</summary>
    public static NcfType DebitNote => new("03", "Nota de Débito", "Modifica el valor de comprobantes previamente emitidos aumentando el balance.", true, true);
    /// <summary>Gets NCF type 04 (Nota de Crédito).</summary>
    public static NcfType CreditNote => new("04", "Nota de Crédito", "Modifica el valor de comprobantes previamente emitidos disminuyendo el balance.", true, true);
    /// <summary>Gets NCF type 11 (Registro de Proveedores Informales).</summary>
    public static NcfType InformalSuppliers => new("11", "Registro de Proveedores Informales", "Emitido por el comprador para compras a personas no registradas en DGII.", true, false);
    /// <summary>Gets NCF type 12 (Registro Único de Ingresos).</summary>
    public static NcfType SingleIncomeRegister => new("12", "Registro Único de Ingresos", "Resumen diario de ventas a consumidores por debajo del umbral de identificación.", false, false);
    /// <summary>Gets NCF type 13 (Registro de Gastos Menores).</summary>
    public static NcfType MinorExpenses => new("13", "Registro de Gastos Menores", "Comprobante emitido para compras menores de caja chica.", true, false);
    /// <summary>Gets NCF type 14 (Regímenes Especiales).</summary>
    public static NcfType SpecialRegimes => new("14", "Regímenes Especiales", "Emitido a contribuyentes acogidos a regímenes de exención (Zonas Francas, etc.).", true, true);
    /// <summary>Gets NCF type 15 (Gubernamental).</summary>
    public static NcfType Governmental => new("15", "Gubernamental", "Emitido a instituciones del Estado dominicano.", true, true);
    /// <summary>Gets NCF type 16 (Exportaciones).</summary>
    public static NcfType Exports => new("16", "Exportaciones", "Emitido para ventas de bienes o servicios al extranjero exentas de ITBIS.", false, false);
    /// <summary>Gets NCF type 17 (Pagos al Exterior).</summary>
    public static NcfType ForeignPayments => new("17", "Pagos al Exterior", "Emitido para pagos por servicios prestados desde el exterior con retención ISR.", true, false);

    /// <summary>
    /// Creates a validated <see cref="NcfType"/> from a 2-digit code string.
    /// </summary>
    /// <param name="code">The 2-digit NCF type code.</param>
    /// <returns>A <see cref="Result{NcfType}"/> containing the created instance or a validation error.</returns>
    public static Result<NcfType> Create(string? code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return Result<NcfType>.Failure(Error.Validation(
                "NcfType.Required", "NCF Type code is required."));
        }

        string trimmed = code.Trim();
        return trimmed switch
        {
            "01" => Result<NcfType>.Success(CreditFiscal),
            "02" => Result<NcfType>.Success(Consumer),
            "03" => Result<NcfType>.Success(DebitNote),
            "04" => Result<NcfType>.Success(CreditNote),
            "11" => Result<NcfType>.Success(InformalSuppliers),
            "12" => Result<NcfType>.Success(SingleIncomeRegister),
            "13" => Result<NcfType>.Success(MinorExpenses),
            "14" => Result<NcfType>.Success(SpecialRegimes),
            "15" => Result<NcfType>.Success(Governmental),
            "16" => Result<NcfType>.Success(Exports),
            "17" => Result<NcfType>.Success(ForeignPayments),
            _ => Result<NcfType>.Failure(Error.Validation(
                "NcfType.Invalid",
                $"Unknown NCF Type code '{trimmed}'. Valid types are: 01, 02, 03, 04, 11, 12, 13, 14, 15, 16, 17."))
        };
    }

    /// <inheritdoc/>
    public int CompareTo(NcfType other) => string.Compare(Code, other.Code, StringComparison.Ordinal);

    /// <inheritdoc/>
    public int CompareTo(object? obj) =>
        obj is NcfType other ? CompareTo(other) : throw new ArgumentException("Object is not an NcfType", nameof(obj));

        /// <summary>
    /// Determines whether the left <see cref="NcfType"/> is less than the right <see cref="NcfType"/>.
    /// </summary>
    /// <param name="left">The first <see cref="NcfType"/> to compare.</param>
    /// <param name="right">The second <see cref="NcfType"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(NcfType left, NcfType right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="NcfType"/> is less than or equal to the right <see cref="NcfType"/>.
    /// </summary>
    /// <param name="left">The first <see cref="NcfType"/> to compare.</param>
    /// <param name="right">The second <see cref="NcfType"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(NcfType left, NcfType right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="NcfType"/> is greater than the right <see cref="NcfType"/>.
    /// </summary>
    /// <param name="left">The first <see cref="NcfType"/> to compare.</param>
    /// <param name="right">The second <see cref="NcfType"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(NcfType left, NcfType right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="NcfType"/> is greater than or equal to the right <see cref="NcfType"/>.
    /// </summary>
    /// <param name="left">The first <see cref="NcfType"/> to compare.</param>
    /// <param name="right">The second <see cref="NcfType"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(NcfType left, NcfType right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public override string ToString() => Code;
}


