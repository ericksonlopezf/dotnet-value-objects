// Copyright © Erickson Lopez. MIT License.
using System;
using System.Text;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Argentina;

/// <summary>
/// Represents an Argentine CUIL (Código Único de Identificación Laboral)
/// administered by ANSES and ARCA for individuals in labor, social security, and tax relationships.
///
/// <para><b>Structure:</b> Exactly 11 numeric digits structured as <c>XY-XXXXXXXX-Z</c> with personal prefixes <c>20, 23, 24, 27</c>.</para>
/// </summary>
[ValueObject]
public readonly record struct Cuil : ISpanParsable<Cuil>, IUtf8SpanParsable<Cuil>, IComparable<Cuil>
{
    private readonly Cuit _cuit;

    private Cuil(Cuit cuit) => _cuit = cuit;

    /// <summary>
    /// Gets the raw 11-digit numeric value of the CUIL.
    /// </summary>
    public string Value => _cuit.Value;

    /// <summary>
    /// Gets the underlying <see cref="Cuit"/> representation.
    /// </summary>
    public Cuit AsCuit => _cuit;

    /// <summary>
    /// Creates a validated <see cref="Cuil"/> from an 11-digit raw string or formatted <c>XX-XXXXXXXX-X</c>.
    /// </summary>
    /// <param name="value">The raw or formatted 11-digit CUIL string.</param>
    /// <returns>A <see cref="Result{Cuil}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<Cuil> Create(string? value) =>
        Create(value.AsSpan());

    /// <summary>
    /// Creates a validated <see cref="Cuil"/> from a character span.
    /// </summary>
    /// <param name="input">A character span containing the raw or formatted CUIL digits.</param>
    /// <returns>A <see cref="Result{Cuil}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<Cuil> Create(ReadOnlySpan<char> input)
    {
        var cuitResult = Cuit.Create(input);
        if (cuitResult.IsFailure)
        {
            return Result<Cuil>.Failure(cuitResult.Error);
        }

        if (!cuitResult.Value.IsIndividual)
        {
            return Result<Cuil>.Failure(Error.Validation(
                "Cuil.InvalidPrefix", "The CUIL only accepts prefixes assigned to individual persons (20, 23, 24, 27)."));
        }

        return Result<Cuil>.Success(new Cuil(cuitResult.Value));
    }

    /// <summary>
    /// Formats the CUIL in standard format: <c>XX-XXXXXXXX-X</c>.
    /// </summary>
    public string Formatted => _cuit.Formatted;

    /// <inheritdoc/>
    public override string ToString() => Formatted;

    /// <inheritdoc/>
    public int CompareTo(Cuil other) => _cuit.CompareTo(other._cuit);

        /// <summary>
    /// Determines whether the left <see cref="Cuil"/> is less than the right <see cref="Cuil"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Cuil"/> to compare.</param>
    /// <param name="right">The second <see cref="Cuil"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(Cuil left, Cuil right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="Cuil"/> is less than or equal to the right <see cref="Cuil"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Cuil"/> to compare.</param>
    /// <param name="right">The second <see cref="Cuil"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(Cuil left, Cuil right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="Cuil"/> is greater than the right <see cref="Cuil"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Cuil"/> to compare.</param>
    /// <param name="right">The second <see cref="Cuil"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(Cuil left, Cuil right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="Cuil"/> is greater than or equal to the right <see cref="Cuil"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Cuil"/> to compare.</param>
    /// <param name="right">The second <see cref="Cuil"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(Cuil left, Cuil right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public static Cuil Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid CUIL: '{s}'.");

    /// <inheritdoc/>
    public static Cuil Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid CUIL: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Cuil result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out Cuil result) =>
        TryParse(s.AsSpan(), provider, out result);

    /// <inheritdoc/>
    public static Cuil Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider = null) =>
        TryParse(utf8Text, provider, out var res) ? res : throw new FormatException("Invalid UTF-8 CUIL representation.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out Cuil result)
    {
        Span<char> chars = stackalloc char[utf8Text.Length];
        Encoding.UTF8.TryGetChars(utf8Text, chars, out int written);
        return TryParse(chars[..written], provider, out result);
    }
}





