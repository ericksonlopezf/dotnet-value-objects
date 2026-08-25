// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Argentina;

using EricksonLopez.ValueObjects.Attributes;

/// <summary>
/// Represents an Argentine Fiscal Point of Sale (Punto de Venta)
/// registered before ARCA/AFIP (Resolución General 1415/2003).
///
/// <para><b>Rules:</b> Numeric value strictly between 1 and 99,999. Standard format is 5 digits zero-padded (<c>00001</c>).</para>
/// </summary>
[RegulatoryRule("DOC.SEQ.002")]
[ValueObject]
public readonly record struct PointOfSale : ISpanParsable<PointOfSale>, IComparable<PointOfSale>
{
    private readonly int _value;

    private PointOfSale(int value) => _value = value;

    /// <summary>
    /// Gets the integer value of the point of sale.
    /// </summary>
    public int Value => _value;

    /// <summary>
    /// Creates a validated <see cref="PointOfSale"/> from an integer value.
    /// </summary>
    /// <param name="value">The point of sale integer value (between 1 and 99,999).</param>
    /// <returns>A <see cref="Result{PointOfSale}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<PointOfSale> Create(int value)
    {
        if (value is < 1 or > 99_999)
        {
            return Result<PointOfSale>.Failure(Error.Validation(
                "PointOfSale.OutOfRange", $"The point of sale must be between 1 and 99999. Received: {value.ToString(CultureInfo.InvariantCulture)}."));
        }

        return Result<PointOfSale>.Success(new PointOfSale(value));
    }

    /// <summary>
    /// Creates a validated <see cref="PointOfSale"/> from a numeric text span.
    /// </summary>
    /// <param name="input">A character span containing the numeric point of sale value.</param>
    /// <returns>A <see cref="Result{PointOfSale}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<PointOfSale> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();
        if (!int.TryParse(trimmed, CultureInfo.InvariantCulture, out int number))
        {
            return Result<PointOfSale>.Failure(Error.Validation(
                "PointOfSale.InvalidFormat", "The point of sale must be a valid integer."));
        }

        return Create(number);
    }

    /// <summary>
    /// Creates a validated <see cref="PointOfSale"/> from a nullable string.
    /// </summary>
    /// <param name="input">A string containing the numeric point of sale value.</param>
    /// <returns>A <see cref="Result{PointOfSale}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<PointOfSale> Create(string? input) =>
        Create(input.AsSpan());

    /// <summary>
    /// Formats the point of sale as a 5-digit zero-padded string (<c>00001</c>).
    /// </summary>
    public string Formatted => _value.ToString("D5", CultureInfo.InvariantCulture);

    /// <inheritdoc/>
    public override string ToString() => Formatted;

    /// <inheritdoc/>
    public int CompareTo(PointOfSale other) => _value.CompareTo(other._value);

        /// <summary>
    /// Determines whether the left <see cref="PointOfSale"/> is less than the right <see cref="PointOfSale"/>.
    /// </summary>
    /// <param name="left">The first <see cref="PointOfSale"/> to compare.</param>
    /// <param name="right">The second <see cref="PointOfSale"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(PointOfSale left, PointOfSale right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="PointOfSale"/> is less than or equal to the right <see cref="PointOfSale"/>.
    /// </summary>
    /// <param name="left">The first <see cref="PointOfSale"/> to compare.</param>
    /// <param name="right">The second <see cref="PointOfSale"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(PointOfSale left, PointOfSale right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="PointOfSale"/> is greater than the right <see cref="PointOfSale"/>.
    /// </summary>
    /// <param name="left">The first <see cref="PointOfSale"/> to compare.</param>
    /// <param name="right">The second <see cref="PointOfSale"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(PointOfSale left, PointOfSale right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="PointOfSale"/> is greater than or equal to the right <see cref="PointOfSale"/>.
    /// </summary>
    /// <param name="left">The first <see cref="PointOfSale"/> to compare.</param>
    /// <param name="right">The second <see cref="PointOfSale"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(PointOfSale left, PointOfSale right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public static PointOfSale Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid PointOfSale: '{s}'.");

    /// <inheritdoc/>
    public static PointOfSale Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid PointOfSale: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out PointOfSale result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out PointOfSale result) =>
        TryParse(s.AsSpan(), provider, out result);
}




