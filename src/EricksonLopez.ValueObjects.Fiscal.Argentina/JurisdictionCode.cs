// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Argentina;

/// <summary>
/// Represents an Argentine Provincial Tax Jurisdiction Code (Código de Jurisdicción SIFERE / Convenio Multilateral).
///
/// <para><b>Range:</b> Strictly between <c>901</c> (CABA) and <c>924</c> (Tucumán).</para>
/// </summary>
[ValueObject]
public readonly record struct JurisdictionCode : ISpanParsable<JurisdictionCode>, IComparable<JurisdictionCode>
{
    /// <summary>Gets the Ciudad Autónoma de Buenos Aires jurisdiction (901).</summary>
    public static JurisdictionCode Caba => new(901, "Ciudad Autónoma de Buenos Aires");
    /// <summary>Gets the Buenos Aires province jurisdiction (902).</summary>
    public static JurisdictionCode BuenosAires => new(902, "Buenos Aires");
    /// <summary>Gets the Córdoba province jurisdiction (904).</summary>
    public static JurisdictionCode Cordoba => new(904, "Córdoba");
    /// <summary>Gets the Santa Fe province jurisdiction (921).</summary>
    public static JurisdictionCode SantaFe => new(921, "Santa Fe");
    /// <summary>Gets the Mendoza province jurisdiction (913).</summary>
    public static JurisdictionCode Mendoza => new(913, "Mendoza");


    private readonly int _code;
    private readonly string _name;

    private JurisdictionCode(int code, string name)
    {
        _code = code;
        _name = name;
    }

    /// <summary>
    /// Gets the 3-digit integer jurisdiction code (e.g. 901).
    /// </summary>
    public int Code => _code;

    /// <summary>
    /// Gets the jurisdiction name.
    /// </summary>
    public string Name => _name;

    /// <summary>
    /// Creates a validated <see cref="JurisdictionCode"/> from an integer value.
    /// </summary>
    /// <param name="code">The 3-digit integer jurisdiction code (between 901 and 924).</param>
    /// <returns>A <see cref="Result{JurisdictionCode}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<JurisdictionCode> Create(int code)
    {
        if (code is < 901 or > 924)
        {
            return Result<JurisdictionCode>.Failure(Error.Validation(
                "JurisdictionCode.OutOfRange", $"The SIFERE jurisdiction code must be between 901 and 924. Received: {code.ToString(CultureInfo.InvariantCulture)}."));
        }

        string name = GetJurisdictionName(code);
        return Result<JurisdictionCode>.Success(new JurisdictionCode(code, name));
    }

    /// <summary>
    /// Creates a validated <see cref="JurisdictionCode"/> from a text span.
    /// </summary>
    /// <param name="input">A character span containing the numeric jurisdiction code.</param>
    /// <returns>A <see cref="Result{JurisdictionCode}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<JurisdictionCode> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();
        if (!int.TryParse(trimmed, CultureInfo.InvariantCulture, out int code))
        {
            return Result<JurisdictionCode>.Failure(Error.Validation(
                "JurisdictionCode.InvalidFormat", "The jurisdiction code must be numeric."));
        }

        return Create(code);
    }

    /// <summary>
    /// Creates a validated <see cref="JurisdictionCode"/> from a nullable string.
    /// </summary>
    /// <param name="input">A string containing the numeric jurisdiction code.</param>
    /// <returns>A <see cref="Result{JurisdictionCode}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<JurisdictionCode> Create(string? input) =>
        Create(input.AsSpan());

    private static string GetJurisdictionName(int code)
    {
        ReadOnlySpan<string> names =
        [
            "Ciudad Autónoma de Buenos Aires",
            "Buenos Aires",
            "Catamarca",
            "Córdoba",
            "Corrientes",
            "Chaco",
            "Chubut",
            "Entre Ríos",
            "Formosa",
            "Jujuy",
            "La Pampa",
            "La Rioja",
            "Mendoza",
            "Misiones",
            "Neuquén",
            "Río Negro",
            "Salta",
            "San Juan",
            "San Luis",
            "Santa Cruz",
            "Santa Fe",
            "Santiago del Estero",
            "Tierra del Fuego",
            "Tucumán"
        ];

        return names[code - 901];
    }


    /// <inheritdoc/>
    public override string ToString() => $"{_code.ToString(CultureInfo.InvariantCulture)} - {_name}";

    /// <inheritdoc/>
    public int CompareTo(JurisdictionCode other) => _code.CompareTo(other._code);

        /// <summary>
    /// Determines whether the left <see cref="JurisdictionCode"/> is less than the right <see cref="JurisdictionCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="JurisdictionCode"/> to compare.</param>
    /// <param name="right">The second <see cref="JurisdictionCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(JurisdictionCode left, JurisdictionCode right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="JurisdictionCode"/> is less than or equal to the right <see cref="JurisdictionCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="JurisdictionCode"/> to compare.</param>
    /// <param name="right">The second <see cref="JurisdictionCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(JurisdictionCode left, JurisdictionCode right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="JurisdictionCode"/> is greater than the right <see cref="JurisdictionCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="JurisdictionCode"/> to compare.</param>
    /// <param name="right">The second <see cref="JurisdictionCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(JurisdictionCode left, JurisdictionCode right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="JurisdictionCode"/> is greater than or equal to the right <see cref="JurisdictionCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="JurisdictionCode"/> to compare.</param>
    /// <param name="right">The second <see cref="JurisdictionCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(JurisdictionCode left, JurisdictionCode right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public static JurisdictionCode Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid JurisdictionCode: '{s}'.");

    /// <inheritdoc/>
    public static JurisdictionCode Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid JurisdictionCode: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out JurisdictionCode result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out JurisdictionCode result) =>
        TryParse(s.AsSpan(), provider, out result);
}




