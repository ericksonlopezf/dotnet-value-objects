// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Mexico;

using EricksonLopez.ValueObjects.Attributes;

/// <summary>
/// Represents an official SAT CFDI Usage Code (c_UsoCFDI, Anexo 20 CFDI 4.0).
///
/// <para><b>Common Codes:</b>
/// <list type="bullet">
///   <item><term>G01</term><description>Adquisición de mercancías</description></item>
///   <item><term>G02</term><description>Devoluciones, descuentos o bonificaciones</description></item>
///   <item><term>G03</term><description>Gastos en general</description></item>
///   <item><term>CP01</term><description>Pagos</description></item>
///   <item><term>CN01</term><description>Nómina</description></item>
///   <item><term>S01</term><description>Sin efectos fiscales</description></item>
/// </list>
/// </para>
/// </summary>
[RegulatoryRule("CAT.VAL.002")]
[ValueObject]
public readonly record struct CfdiUsageCode : ISpanParsable<CfdiUsageCode>, IComparable<CfdiUsageCode>
{
    /// <summary>Gets CFDI usage G01 (Adquisición de mercancías).</summary>
    public static CfdiUsageCode GoodsAcquisition => new("G01", "Adquisición de mercancías");
    /// <summary>Gets CFDI usage G02 (Devoluciones, descuentos o bonificaciones).</summary>
    public static CfdiUsageCode ReturnsDiscounts => new("G02", "Devoluciones, descuentos o bonificaciones");
    /// <summary>Gets CFDI usage G03 (Gastos en general).</summary>
    public static CfdiUsageCode GeneralExpenses => new("G03", "Gastos en general");
    /// <summary>Gets CFDI usage CP01 (Pagos).</summary>
    public static CfdiUsageCode Payments => new("CP01", "Pagos");
    /// <summary>Gets CFDI usage CN01 (Nómina).</summary>
    public static CfdiUsageCode Payroll => new("CN01", "Nómina");
    /// <summary>Gets CFDI usage S01 (Sin efectos fiscales).</summary>
    public static CfdiUsageCode WithoutTaxEffects => new("S01", "Sin efectos fiscales");


    private readonly string _code;
    private readonly string _description;

    private CfdiUsageCode(string code, string description)
    {
        _code = code;
        _description = description;
    }

    /// <summary>
    /// Gets the SAT CFDI usage code.
    /// </summary>
    public string Code => _code;

    /// <summary>
    /// Gets the official description.
    /// </summary>
    public string Description => _description;

    /// <summary>
    /// Creates a validated <see cref="CfdiUsageCode"/> from an official code string.
    /// </summary>
    /// <param name="code">The SAT CFDI usage code (e.g. <c>G01</c>, <c>CP01</c>, <c>S01</c>).</param>
    /// <returns>A <see cref="Result{CfdiUsageCode}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<CfdiUsageCode> Create(string? code) =>
        Create(code.AsSpan());

    /// <summary>
    /// Creates a validated <see cref="CfdiUsageCode"/> from a character span.
    /// </summary>
    /// <param name="input">A character span containing the SAT CFDI usage code.</param>
    /// <returns>A <see cref="Result{CfdiUsageCode}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<CfdiUsageCode> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();

        if (trimmed.Length is < 3 or > 4)
        {
            return Result<CfdiUsageCode>.Failure(Error.Validation(
                "CfdiUsageCode.InvalidLength", "The CFDI usage code must contain between 3 and 4 characters."));
        }

        foreach (char c in trimmed)
        {
            if (!char.IsAsciiLetterOrDigit(c))
            {
                return Result<CfdiUsageCode>.Failure(Error.Validation(
                    "CfdiUsageCode.InvalidCharacters", "The CFDI usage code must only contain letters or digits."));
            }
        }

        return trimmed switch
        {
            "G01" => Result<CfdiUsageCode>.Success(GoodsAcquisition),
            "G02" => Result<CfdiUsageCode>.Success(ReturnsDiscounts),
            "G03" => Result<CfdiUsageCode>.Success(GeneralExpenses),
            "CP01" => Result<CfdiUsageCode>.Success(Payments),
            "CN01" => Result<CfdiUsageCode>.Success(Payroll),
            "S01" => Result<CfdiUsageCode>.Success(WithoutTaxEffects),
            _ => Result<CfdiUsageCode>.Success(new CfdiUsageCode(trimmed.ToString(), "Uso CFDI (Catálogo Dinámico)"))
        };
    }

    /// <inheritdoc/>
    public override string ToString() => $"{_code} - {_description}";

    /// <inheritdoc/>
    public int CompareTo(CfdiUsageCode other) => string.Compare(_code, other._code, StringComparison.Ordinal);

        /// <summary>
    /// Determines whether the left <see cref="CfdiUsageCode"/> is less than the right <see cref="CfdiUsageCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="CfdiUsageCode"/> to compare.</param>
    /// <param name="right">The second <see cref="CfdiUsageCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(CfdiUsageCode left, CfdiUsageCode right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="CfdiUsageCode"/> is less than or equal to the right <see cref="CfdiUsageCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="CfdiUsageCode"/> to compare.</param>
    /// <param name="right">The second <see cref="CfdiUsageCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(CfdiUsageCode left, CfdiUsageCode right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="CfdiUsageCode"/> is greater than the right <see cref="CfdiUsageCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="CfdiUsageCode"/> to compare.</param>
    /// <param name="right">The second <see cref="CfdiUsageCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(CfdiUsageCode left, CfdiUsageCode right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="CfdiUsageCode"/> is greater than or equal to the right <see cref="CfdiUsageCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="CfdiUsageCode"/> to compare.</param>
    /// <param name="right">The second <see cref="CfdiUsageCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(CfdiUsageCode left, CfdiUsageCode right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public static CfdiUsageCode Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid CfdiUsageCode: '{s}'.");

    /// <inheritdoc/>
    public static CfdiUsageCode Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid CfdiUsageCode: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out CfdiUsageCode result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out CfdiUsageCode result) =>
        TryParse(s.AsSpan(), provider, out result);
}



