// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Mexico;

using EricksonLopez.ValueObjects.Attributes;

/// <summary>
/// Represents an official SAT Tax Regime Code (c_RegimenFiscal, Anexo 20 CFDI 4.0).
///
/// <para><b>Common Codes:</b>
/// <list type="bullet">
///   <item><term>601</term><description>General de Ley Personas Morales</description></item>
///   <item><term>605</term><description>Sueldos y Salarios e Ingresos Asimilados a Salarios</description></item>
///   <item><term>606</term><description>Arrendamiento</description></item>
///   <item><term>612</term><description>Personas Físicas con Actividades Empresariales y Profesionales</description></item>
///   <item><term>626</term><description>Régimen Simplificado de Confianza (RESICO)</description></item>
/// </list>
/// </para>
/// </summary>
[RegulatoryRule("CAT.VAL.002")]
[ValueObject]
public readonly record struct TaxRegimeCode : ISpanParsable<TaxRegimeCode>, IComparable<TaxRegimeCode>
{
    /// <summary>Gets tax regime 601 (General de Ley Personas Morales).</summary>
    public static TaxRegimeCode GeneralPersonasMorales => new("601", "General de Ley Personas Morales", false, true);
    /// <summary>Gets tax regime 605 (Sueldos y Salarios).</summary>
    public static TaxRegimeCode SueldosYSalarios => new("605", "Sueldos y Salarios", true, false);
    /// <summary>Gets tax regime 606 (Arrendamiento).</summary>
    public static TaxRegimeCode Arrendamiento => new("606", "Arrendamiento", true, false);
    /// <summary>Gets tax regime 612 (Personas Físicas con Actividades Empresariales y Profesionales).</summary>
    public static TaxRegimeCode ActividadesEmpresariales => new("612", "Personas Físicas con Actividades Empresariales y Profesionales", true, false);
    /// <summary>Gets tax regime 626 (Régimen Simplificado de Confianza - RESICO).</summary>
    public static TaxRegimeCode Resico => new("626", "Régimen Simplificado de Confianza", true, true);


    private readonly string _code;
    private readonly string _description;
    private readonly bool _appliesToPhysical;
    private readonly bool _appliesToMoral;

    private TaxRegimeCode(string code, string description, bool appliesToPhysical, bool appliesToMoral)
    {
        _code = code;
        _description = description;
        _appliesToPhysical = appliesToPhysical;
        _appliesToMoral = appliesToMoral;
    }

    /// <summary>
    /// Gets the 3-digit SAT regime code.
    /// </summary>
    public string Code => _code;

    /// <summary>
    /// Gets the official description.
    /// </summary>
    public string Description => _description;

    /// <summary>Gets a value indicating whether this regime applies to physical persons (Personas Físicas).</summary>
    public bool AppliesToPhysical => _appliesToPhysical;

    /// <summary>Gets a value indicating whether this regime applies to legal entities (Personas Morales).</summary>
    public bool AppliesToMoral => _appliesToMoral;

    /// <summary>
    /// Creates a validated <see cref="TaxRegimeCode"/> from a 3-digit code.
    /// </summary>
    /// <param name="code">The 3-digit SAT tax regime code (e.g. <c>601</c>, <c>626</c>).</param>
    /// <returns>A <see cref="Result{TaxRegimeCode}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<TaxRegimeCode> Create(string? code) =>
        Create(code.AsSpan());

    /// <summary>
    /// Creates a validated <see cref="TaxRegimeCode"/> from a character span.
    /// </summary>
    /// <param name="input">A character span containing the 3-digit SAT tax regime code.</param>
    /// <returns>A <see cref="Result{TaxRegimeCode}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<TaxRegimeCode> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();

        if (trimmed.Length != 3)
        {
            return Result<TaxRegimeCode>.Failure(Error.Validation(
                "TaxRegimeCode.InvalidLength", "The tax regime code must contain exactly 3 characters."));
        }

        foreach (char c in trimmed)
        {
            if (!char.IsDigit(c))
            {
                return Result<TaxRegimeCode>.Failure(Error.Validation(
                    "TaxRegimeCode.InvalidCharacters", "The tax regime code must only contain numeric digits."));
            }
        }

        return trimmed switch
        {
            "601" => Result<TaxRegimeCode>.Success(GeneralPersonasMorales),
            "605" => Result<TaxRegimeCode>.Success(SueldosYSalarios),
            "606" => Result<TaxRegimeCode>.Success(Arrendamiento),
            "612" => Result<TaxRegimeCode>.Success(ActividadesEmpresariales),
            "626" => Result<TaxRegimeCode>.Success(Resico),
            _ => Result<TaxRegimeCode>.Success(new TaxRegimeCode(trimmed.ToString(), "Régimen Fiscal (Catálogo Dinámico)", false, false))
        };
    }

    /// <inheritdoc/>
    public override string ToString() => $"{_code} - {_description}";

    /// <inheritdoc/>
    public int CompareTo(TaxRegimeCode other) => string.Compare(_code, other._code, StringComparison.Ordinal);

        /// <summary>
    /// Determines whether the left <see cref="TaxRegimeCode"/> is less than the right <see cref="TaxRegimeCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="TaxRegimeCode"/> to compare.</param>
    /// <param name="right">The second <see cref="TaxRegimeCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(TaxRegimeCode left, TaxRegimeCode right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="TaxRegimeCode"/> is less than or equal to the right <see cref="TaxRegimeCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="TaxRegimeCode"/> to compare.</param>
    /// <param name="right">The second <see cref="TaxRegimeCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(TaxRegimeCode left, TaxRegimeCode right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="TaxRegimeCode"/> is greater than the right <see cref="TaxRegimeCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="TaxRegimeCode"/> to compare.</param>
    /// <param name="right">The second <see cref="TaxRegimeCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(TaxRegimeCode left, TaxRegimeCode right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="TaxRegimeCode"/> is greater than or equal to the right <see cref="TaxRegimeCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="TaxRegimeCode"/> to compare.</param>
    /// <param name="right">The second <see cref="TaxRegimeCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(TaxRegimeCode left, TaxRegimeCode right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public static TaxRegimeCode Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid TaxRegimeCode: '{s}'.");

    /// <inheritdoc/>
    public static TaxRegimeCode Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid TaxRegimeCode: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out TaxRegimeCode result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out TaxRegimeCode result) =>
        TryParse(s.AsSpan(), provider, out result);
}



