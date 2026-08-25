// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Peru;

using EricksonLopez.ValueObjects.Attributes;

/// <summary>
/// Represents an official SUNAT IGV Tax Affectation Type Code (Tipo de Afectación al IGV, Catálogo N° 07).
///
/// <para><b>Common Codes:</b>
/// <list type="bullet">
///   <item><term>10</term><description>Gravado - Operación Onerosa</description></item>
///   <item><term>20</term><description>Exonerado - Operación Onerosa</description></item>
///   <item><term>30</term><description>Inafecto - Operación Onerosa</description></item>
///   <item><term>40</term><description>Exportación de Bienes o Servicios</description></item>
/// </list>
/// </para>
/// </summary>
[RegulatoryRule("TAX.CAT.001")]
[ValueObject]
public readonly record struct AffectationTypeCode : ISpanParsable<AffectationTypeCode>, IComparable<AffectationTypeCode>
{
    /// <summary>Gets affectation code 10 (Gravado - Operación Onerosa).</summary>
    public static AffectationTypeCode GravadoOneroso => new("10", "Gravado - Operación Onerosa", true, false, false, false);
    /// <summary>Gets affectation code 20 (Exonerado - Operación Onerosa).</summary>
    public static AffectationTypeCode ExoneradoOneroso => new("20", "Exonerado - Operación Onerosa", false, true, false, false);
    /// <summary>Gets affectation code 30 (Inafecto - Operación Onerosa).</summary>
    public static AffectationTypeCode InafectoOneroso => new("30", "Inafecto - Operación Onerosa", false, false, true, false);
    /// <summary>Gets affectation code 40 (Exportación de Bienes o Servicios).</summary>
    public static AffectationTypeCode Exportacion => new("40", "Exportación de Bienes o Servicios", false, false, false, true);


    private readonly string _code;
    private readonly string _description;
    private readonly bool _isTaxable;
    private readonly bool _isExempt;
    private readonly bool _isUnaffected;
    private readonly bool _isExportation;

    private AffectationTypeCode(string code, string description, bool isTaxable, bool isExempt, bool isUnaffected, bool isExportation)
    {
        _code = code;
        _description = description;
        _isTaxable = isTaxable;
        _isExempt = isExempt;
        _isUnaffected = isUnaffected;
        _isExportation = isExportation;
    }

    /// <summary>
    /// Gets the 2-digit SUNAT code.
    /// </summary>
    public string Code => _code;

    /// <summary>
    /// Gets the official description.
    /// </summary>
    public string Description => _description;

    /// <summary>Gets a value indicating whether this operation is taxable by IGV (Gravado).</summary>
    public bool IsTaxable => _isTaxable;

    /// <summary>Gets a value indicating whether this operation is exempt from IGV (Exonerado).</summary>
    public bool IsExempt => _isExempt;

    /// <summary>Gets a value indicating whether this operation is unaffected by IGV (Inafecto).</summary>
    public bool IsUnaffected => _isUnaffected;

    /// <summary>Gets a value indicating whether this operation is an export (Exportación).</summary>
    public bool IsExportation => _isExportation;

    /// <summary>
    /// Creates a validated <see cref="AffectationTypeCode"/> from a 2-digit code.
    /// </summary>
    public static Result<AffectationTypeCode> Create(string? code) =>
        Create(code.AsSpan());

    /// <summary>
    /// Creates a validated <see cref="AffectationTypeCode"/> from a character span.
    /// </summary>
    public static Result<AffectationTypeCode> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();
        return trimmed switch
        {
            "10" => Result<AffectationTypeCode>.Success(GravadoOneroso),
            "20" => Result<AffectationTypeCode>.Success(ExoneradoOneroso),
            "30" => Result<AffectationTypeCode>.Success(InafectoOneroso),
            "40" => Result<AffectationTypeCode>.Success(Exportacion),
            _ => Result<AffectationTypeCode>.Failure(Error.Validation(
                "AffectationTypeCode.InvalidCode", $"The IGV affectation code '{trimmed.ToString()}' is not recognized or supported."))
        };
    }

    /// <inheritdoc/>
    public override string ToString() => $"{_code} - {_description}";

    /// <inheritdoc/>
    public int CompareTo(AffectationTypeCode other) => string.Compare(_code, other._code, StringComparison.Ordinal);

        /// <summary>
    /// Determines whether the left <see cref="AffectationTypeCode"/> is less than the right <see cref="AffectationTypeCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="AffectationTypeCode"/> to compare.</param>
    /// <param name="right">The second <see cref="AffectationTypeCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(AffectationTypeCode left, AffectationTypeCode right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="AffectationTypeCode"/> is less than or equal to the right <see cref="AffectationTypeCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="AffectationTypeCode"/> to compare.</param>
    /// <param name="right">The second <see cref="AffectationTypeCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(AffectationTypeCode left, AffectationTypeCode right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="AffectationTypeCode"/> is greater than the right <see cref="AffectationTypeCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="AffectationTypeCode"/> to compare.</param>
    /// <param name="right">The second <see cref="AffectationTypeCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(AffectationTypeCode left, AffectationTypeCode right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="AffectationTypeCode"/> is greater than or equal to the right <see cref="AffectationTypeCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="AffectationTypeCode"/> to compare.</param>
    /// <param name="right">The second <see cref="AffectationTypeCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(AffectationTypeCode left, AffectationTypeCode right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public static AffectationTypeCode Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid AffectationTypeCode: '{s}'.");

    /// <inheritdoc/>
    public static AffectationTypeCode Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid AffectationTypeCode: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out AffectationTypeCode result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out AffectationTypeCode result) =>
        TryParse(s.AsSpan(), provider, out result);
}



