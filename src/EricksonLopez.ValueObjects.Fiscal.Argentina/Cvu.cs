// Copyright © Erickson Lopez. MIT License.
using System;
using System.Text;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Argentina;

/// <summary>
/// Represents an Argentine CVU (Clave Virtual Uniforme)
/// standardized by the Central Bank of Argentina (BCRA Comunicación "A" 6586) for Payment Service Providers (PSP / Fintechs).
///
/// <para><b>Structure:</b> Exactly 22 numeric digits starting with <c>"000"</c> (PSP identifier),
/// structured into two blocks with double Modulo 10 verification check digits.</para>
/// </summary>
[ValueObject]
public readonly record struct Cvu : ISpanParsable<Cvu>, IUtf8SpanParsable<Cvu>, IComparable<Cvu>
{
    private readonly Cbu _cbu;

    private Cvu(Cbu cbu) => _cbu = cbu;

    /// <summary>
    /// Gets the raw 22-digit CVU string.
    /// </summary>
    public string Value => _cbu.Value;

    /// <summary>
    /// Gets the 8-digit PSP routing block.
    /// </summary>
    public string PspCode => _cbu.Value[..8];

    /// <summary>
    /// Gets the 14-digit virtual account block.
    /// </summary>
    public string AccountNumber => _cbu.Value[8..];

    /// <summary>
    /// Creates a validated <see cref="Cvu"/> from a 22-digit numeric string.
    /// </summary>
    /// <param name="value">The 22-digit CVU string starting with <c>"000"</c>.</param>
    /// <returns>A <see cref="Result{Cvu}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<Cvu> Create(string? value) =>
        Create(value.AsSpan());

    /// <summary>
    /// Creates a validated <see cref="Cvu"/> from a character span.
    /// </summary>
    /// <param name="input">A character span containing the 22-digit CVU digits.</param>
    /// <returns>A <see cref="Result{Cvu}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<Cvu> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();
        if (!trimmed.StartsWith("000", StringComparison.Ordinal))
        {
            return Result<Cvu>.Failure(Error.Validation(
                "Cvu.InvalidPrefix", "The CVU must start with the digits '000' (PSP code)."));
        }

        var cbuResult = Cbu.Create(trimmed);
        if (cbuResult.IsFailure)
        {
            return Result<Cvu>.Failure(cbuResult.Error);
        }

        return Result<Cvu>.Success(new Cvu(cbuResult.Value));
    }

    /// <inheritdoc/>
    public override string ToString() => Value;

    /// <inheritdoc/>
    public int CompareTo(Cvu other) => _cbu.CompareTo(other._cbu);

        /// <summary>
    /// Determines whether the left <see cref="Cvu"/> is less than the right <see cref="Cvu"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Cvu"/> to compare.</param>
    /// <param name="right">The second <see cref="Cvu"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(Cvu left, Cvu right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="Cvu"/> is less than or equal to the right <see cref="Cvu"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Cvu"/> to compare.</param>
    /// <param name="right">The second <see cref="Cvu"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(Cvu left, Cvu right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="Cvu"/> is greater than the right <see cref="Cvu"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Cvu"/> to compare.</param>
    /// <param name="right">The second <see cref="Cvu"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(Cvu left, Cvu right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="Cvu"/> is greater than or equal to the right <see cref="Cvu"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Cvu"/> to compare.</param>
    /// <param name="right">The second <see cref="Cvu"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(Cvu left, Cvu right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public static Cvu Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid CVU: '{s}'.");

    /// <inheritdoc/>
    public static Cvu Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid CVU: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Cvu result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out Cvu result) =>
        TryParse(s.AsSpan(), provider, out result);

    /// <inheritdoc/>
    public static Cvu Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider = null) =>
        TryParse(utf8Text, provider, out var res) ? res : throw new FormatException("Invalid UTF-8 CVU representation.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out Cvu result)
    {
        Span<char> chars = stackalloc char[utf8Text.Length];
        Encoding.UTF8.TryGetChars(utf8Text, chars, out int written);
        return TryParse(chars[..written], provider, out result);
    }
}





