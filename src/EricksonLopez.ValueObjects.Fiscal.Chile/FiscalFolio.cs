// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Chile;

/// <summary>
/// Represents a Chilean Fiscal Folio Number (Folio DTE)
/// authorized by the SII via CAF (Código de Autorización de Folios).
///
/// <para><b>Rules:</b> Strictly positive integer between 1 and 2,147,483,647 (<see cref="int.MaxValue"/>).</para>
/// </summary>
[ValueObject]
public readonly record struct FiscalFolio : ISpanParsable<FiscalFolio>, IComparable<FiscalFolio>
{
    private readonly int _value;

    private FiscalFolio(int value) => _value = value;

    /// <summary>
    /// Gets the integer folio value.
    /// </summary>
    public int Value => _value;

    /// <summary>
    /// Creates a validated <see cref="FiscalFolio"/> from an integer value.
    /// </summary>
    /// <param name="value">The strictly positive folio integer value.</param>
    /// <returns>A <see cref="Result{FiscalFolio}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<FiscalFolio> Create(int value)
    {
        if (value <= 0)
        {
            return Result<FiscalFolio>.Failure(Error.Validation(
                "FiscalFolio.OutOfRange", "The fiscal folio must be an integer greater than zero."));
        }

        return Result<FiscalFolio>.Success(new FiscalFolio(value));
    }

    /// <summary>
    /// Creates a validated <see cref="FiscalFolio"/> from a text span.
    /// </summary>
    /// <param name="input">A character span containing the numeric folio value.</param>
    /// <returns>A <see cref="Result{FiscalFolio}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<FiscalFolio> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();
        if (!int.TryParse(trimmed, CultureInfo.InvariantCulture, out int value))
        {
            return Result<FiscalFolio>.Failure(Error.Validation(
                "FiscalFolio.InvalidFormat", "The fiscal folio must be a valid integer."));
        }

        return Create(value);
    }

    /// <summary>
    /// Creates a validated <see cref="FiscalFolio"/> from a nullable string.
    /// </summary>
    /// <param name="input">A string containing the numeric folio value.</param>
    /// <returns>A <see cref="Result{FiscalFolio}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<FiscalFolio> Create(string? input) =>
        Create(input.AsSpan());

    /// <inheritdoc/>
    public override string ToString() => _value.ToString(CultureInfo.InvariantCulture);

    /// <inheritdoc/>
    public int CompareTo(FiscalFolio other) => _value.CompareTo(other._value);

        /// <summary>
    /// Determines whether the left <see cref="FiscalFolio"/> is less than the right <see cref="FiscalFolio"/>.
    /// </summary>
    /// <param name="left">The first <see cref="FiscalFolio"/> to compare.</param>
    /// <param name="right">The second <see cref="FiscalFolio"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(FiscalFolio left, FiscalFolio right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="FiscalFolio"/> is less than or equal to the right <see cref="FiscalFolio"/>.
    /// </summary>
    /// <param name="left">The first <see cref="FiscalFolio"/> to compare.</param>
    /// <param name="right">The second <see cref="FiscalFolio"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(FiscalFolio left, FiscalFolio right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="FiscalFolio"/> is greater than the right <see cref="FiscalFolio"/>.
    /// </summary>
    /// <param name="left">The first <see cref="FiscalFolio"/> to compare.</param>
    /// <param name="right">The second <see cref="FiscalFolio"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(FiscalFolio left, FiscalFolio right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="FiscalFolio"/> is greater than or equal to the right <see cref="FiscalFolio"/>.
    /// </summary>
    /// <param name="left">The first <see cref="FiscalFolio"/> to compare.</param>
    /// <param name="right">The second <see cref="FiscalFolio"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(FiscalFolio left, FiscalFolio right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public static FiscalFolio Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid FiscalFolio: '{s}'.");

    /// <inheritdoc/>
    public static FiscalFolio Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid FiscalFolio: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out FiscalFolio result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out FiscalFolio result) =>
        TryParse(s.AsSpan(), provider, out result);
}




