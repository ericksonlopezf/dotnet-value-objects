// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Argentina;

/// <summary>
/// Represents an official ARCA/AFIP Voucher Type Code (Tipo de Comprobante, Tabla AFIP).
///
/// <para><b>Common Types:</b>
/// <list type="bullet">
///   <item><term>1</term><description>Factura A</description></item>
///   <item><term>2</term><description>Nota de Débito A</description></item>
///   <item><term>3</term><description>Nota de Crédito A</description></item>
///   <item><term>6</term><description>Factura B</description></item>
///   <item><term>7</term><description>Nota de Débito B</description></item>
///   <item><term>8</term><description>Nota de Crédito B</description></item>
///   <item><term>11</term><description>Factura C</description></item>
///   <item><term>12</term><description>Nota de Débito C</description></item>
///   <item><term>13</term><description>Nota de Crédito C</description></item>
///   <item><term>19</term><description>Factura de Exportación E</description></item>
///   <item><term>201</term><description>Factura de Crédito Electrónica MiPyME A</description></item>
///   <item><term>206</term><description>Factura de Crédito Electrónica MiPyME B</description></item>
///   <item><term>211</term><description>Factura de Crédito Electrónica MiPyME C</description></item>
/// </list>
/// </para>
/// </summary>
[ValueObject]
public readonly record struct VoucherType : ISpanParsable<VoucherType>, IComparable<VoucherType>
{
    /// <summary>Gets voucher type Factura A (1).</summary>
    public static VoucherType InvoiceA => new(1, "Factura A", 'A');
    /// <summary>Gets voucher type Nota de Débito A (2).</summary>
    public static VoucherType DebitNoteA => new(2, "Nota de Débito A", 'A');
    /// <summary>Gets voucher type Nota de Crédito A (3).</summary>
    public static VoucherType CreditNoteA => new(3, "Nota de Crédito A", 'A');
    /// <summary>Gets voucher type Factura B (6).</summary>
    public static VoucherType InvoiceB => new(6, "Factura B", 'B');
    /// <summary>Gets voucher type Nota de Débito B (7).</summary>
    public static VoucherType DebitNoteB => new(7, "Nota de Débito B", 'B');
    /// <summary>Gets voucher type Nota de Crédito B (8).</summary>
    public static VoucherType CreditNoteB => new(8, "Nota de Crédito B", 'B');
    /// <summary>Gets voucher type Factura C (11).</summary>
    public static VoucherType InvoiceC => new(11, "Factura C", 'C');
    /// <summary>Gets voucher type Nota de Débito C (12).</summary>
    public static VoucherType DebitNoteC => new(12, "Nota de Débito C", 'C');
    /// <summary>Gets voucher type Nota de Crédito C (13).</summary>
    public static VoucherType CreditNoteC => new(13, "Nota de Crédito C", 'C');
    /// <summary>Gets voucher type Factura de Exportación E (19).</summary>
    public static VoucherType InvoiceE => new(19, "Factura de Exportación E", 'E');
    /// <summary>Gets voucher type Factura de Crédito Electrónica MiPyME A (201).</summary>
    public static VoucherType FceInvoiceA => new(201, "Factura de Crédito Electrónica MiPyME A", 'A');
    /// <summary>Gets voucher type Factura de Crédito Electrónica MiPyME B (206).</summary>
    public static VoucherType FceInvoiceB => new(206, "Factura de Crédito Electrónica MiPyME B", 'B');
    /// <summary>Gets voucher type Factura de Crédito Electrónica MiPyME C (211).</summary>
    public static VoucherType FceInvoiceC => new(211, "Factura de Crédito Electrónica MiPyME C", 'C');


    private readonly int _code;
    private readonly string _description;
    private readonly char _letter;

    private VoucherType(int code, string description, char letter)
    {
        _code = code;
        _description = description;
        _letter = letter;
    }

    /// <summary>
    /// Gets the integer ARCA voucher type code.
    /// </summary>
    public int Code => _code;

    /// <summary>
    /// Gets the official description.
    /// </summary>
    public string Description => _description;

    /// <summary>
    /// Gets the fiscal voucher letter ('A', 'B', 'C', 'E', etc.).
    /// </summary>
    public char Letter => _letter;

    /// <summary>
    /// Creates a validated <see cref="VoucherType"/> from an integer code.
    /// </summary>
    /// <param name="code">The integer ARCA/AFIP voucher type code.</param>
    /// <returns>A <see cref="Result{VoucherType}"/> containing the matched type or a domain validation error.</returns>
    public static Result<VoucherType> Create(int code)
    {
        return code switch
        {
            1 => Result<VoucherType>.Success(InvoiceA),
            2 => Result<VoucherType>.Success(DebitNoteA),
            3 => Result<VoucherType>.Success(CreditNoteA),
            6 => Result<VoucherType>.Success(InvoiceB),
            7 => Result<VoucherType>.Success(DebitNoteB),
            8 => Result<VoucherType>.Success(CreditNoteB),
            11 => Result<VoucherType>.Success(InvoiceC),
            12 => Result<VoucherType>.Success(DebitNoteC),
            13 => Result<VoucherType>.Success(CreditNoteC),
            19 => Result<VoucherType>.Success(InvoiceE),
            201 => Result<VoucherType>.Success(FceInvoiceA),
            206 => Result<VoucherType>.Success(FceInvoiceB),
            211 => Result<VoucherType>.Success(FceInvoiceC),
            _ => Result<VoucherType>.Failure(Error.Validation(
                "VoucherType.InvalidCode", $"The ARCA voucher type code '{code.ToString(CultureInfo.InvariantCulture)}' is invalid."))
        };
    }

    /// <summary>
    /// Creates a validated <see cref="VoucherType"/> from a numeric text span.
    /// </summary>
    /// <param name="input">A character span containing the numeric voucher type code.</param>
    /// <returns>A <see cref="Result{VoucherType}"/> containing the matched type or a domain validation error.</returns>
    public static Result<VoucherType> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();
        if (!int.TryParse(trimmed, CultureInfo.InvariantCulture, out int code))
        {
            return Result<VoucherType>.Failure(Error.Validation(
                "VoucherType.InvalidFormat", "The voucher type code must be numeric."));
        }

        return Create(code);
    }

    /// <summary>
    /// Creates a validated <see cref="VoucherType"/> from a nullable string.
    /// </summary>
    /// <param name="input">A string containing the numeric voucher type code.</param>
    /// <returns>A <see cref="Result{VoucherType}"/> containing the matched type or a domain validation error.</returns>
    public static Result<VoucherType> Create(string? input) =>
        Create(input.AsSpan());

    /// <inheritdoc/>
    public override string ToString() => $"{_code.ToString(CultureInfo.InvariantCulture)} - {_description}";

    /// <inheritdoc/>
    public int CompareTo(VoucherType other) => _code.CompareTo(other._code);

        /// <summary>
    /// Determines whether the left <see cref="VoucherType"/> is less than the right <see cref="VoucherType"/>.
    /// </summary>
    /// <param name="left">The first <see cref="VoucherType"/> to compare.</param>
    /// <param name="right">The second <see cref="VoucherType"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(VoucherType left, VoucherType right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="VoucherType"/> is less than or equal to the right <see cref="VoucherType"/>.
    /// </summary>
    /// <param name="left">The first <see cref="VoucherType"/> to compare.</param>
    /// <param name="right">The second <see cref="VoucherType"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(VoucherType left, VoucherType right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="VoucherType"/> is greater than the right <see cref="VoucherType"/>.
    /// </summary>
    /// <param name="left">The first <see cref="VoucherType"/> to compare.</param>
    /// <param name="right">The second <see cref="VoucherType"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(VoucherType left, VoucherType right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="VoucherType"/> is greater than or equal to the right <see cref="VoucherType"/>.
    /// </summary>
    /// <param name="left">The first <see cref="VoucherType"/> to compare.</param>
    /// <param name="right">The second <see cref="VoucherType"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(VoucherType left, VoucherType right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public static VoucherType Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid VoucherType: '{s}'.");

    /// <inheritdoc/>
    public static VoucherType Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid VoucherType: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out VoucherType result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out VoucherType result) =>
        TryParse(s.AsSpan(), provider, out result);
}




