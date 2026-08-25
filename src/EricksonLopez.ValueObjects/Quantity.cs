// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a discrete, non-negative integer quantity.
/// </summary>
public readonly record struct Quantity : IValueObject<Quantity>, IComparable<Quantity>, IComparable, IParsable<Quantity>, ISpanParsable<Quantity>, IFormattable, ISpanFormattable
{
    private const NumberStyles QuantityNumberStyles = NumberStyles.Integer | NumberStyles.AllowThousands;

    /// <summary>
    /// Gets the integer quantity value.
    /// </summary>
    public int Value { get; }

    private Quantity(int value) => Value = value;

    /// <summary>
    /// Creates a validated <see cref="Quantity"/> instance from a non-negative integer value.
    /// </summary>
    /// <param name="value">The discrete quantity value (greater than or equal to zero).</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the validated quantity, or a validation failure.</returns>
    public static Result<Quantity> Create(int value)
    {
        if (value < 0)
        {
            return Result<Quantity>.Failure(Error.Validation(
                "Quantity.Negative", "Quantity cannot be negative."));
        }

        return Result<Quantity>.Success(new Quantity(value));
    }

    /// <summary>
    /// Adds another quantity to this instance.
    /// </summary>
    /// <param name="other">The quantity to add.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the resulting sum.</returns>
    public Result<Quantity> Add(Quantity other) => Create(Value + other.Value);

    /// <summary>
    /// Subtracts another quantity from this instance.
    /// </summary>
    /// <param name="other">The quantity to subtract.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the resulting quantity, or a failure if insufficient quantity.</returns>
    public Result<Quantity> Subtract(Quantity other)
    {
        if (other.Value > Value)
        {
            return Result<Quantity>.Failure(Error.Validation(
                "Quantity.InsufficientQuantity", "Cannot subtract more than available quantity."));
        }

        return Create(Value - other.Value);
    }

    /// <summary>
    /// Gets a value indicating whether the quantity is zero.
    /// </summary>
    public bool IsZero => Value == 0;

    /// <summary>
    /// Gets a zero quantity instance.
    /// </summary>
    public static Quantity Zero => new(0);

    /// <summary>
    /// Compares this quantity with another quantity.
    /// </summary>
    /// <param name="other">The other quantity to compare against.</param>
    /// <returns>A value indicating the relative order of the quantities being compared.</returns>
    public int CompareTo(Quantity other) => Value.CompareTo(other.Value);

    /// <inheritdoc/>
    /// <exception cref="ArgumentException"><paramref name="obj"/> is not of type <see cref="Quantity"/></exception>
    public int CompareTo(object? obj) =>
        obj is Quantity other ? CompareTo(other) : throw new ArgumentException("Object is not a Quantity", nameof(obj));

    /// <summary>
    /// Determines whether the left quantity is less than the right quantity.
    /// </summary>
    /// <param name="left">The first quantity to compare.</param>
    /// <param name="right">The second quantity to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(Quantity left, Quantity right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left quantity is less than or equal to the right quantity.
    /// </summary>
    /// <param name="left">The first quantity to compare.</param>
    /// <param name="right">The second quantity to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(Quantity left, Quantity right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left quantity is greater than the right quantity.
    /// </summary>
    /// <param name="left">The first quantity to compare.</param>
    /// <param name="right">The second quantity to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(Quantity left, Quantity right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left quantity is greater than or equal to the right quantity.
    /// </summary>
    /// <param name="left">The first quantity to compare.</param>
    /// <param name="right">The second quantity to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(Quantity left, Quantity right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public override string ToString() => ToString(null, null);

    /// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
    public string ToString(string? format, IFormatProvider? formatProvider) =>
        Value.ToString(format, formatProvider ?? CultureInfo.InvariantCulture);

    /// <inheritdoc/>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) =>
        Value.TryFormat(destination, out charsWritten, format, provider ?? CultureInfo.InvariantCulture);

    /// <summary>
    /// Parses a string into a <see cref="Quantity"/>.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <returns>The parsed <see cref="Quantity"/>.</returns>
    /// <exception cref="FormatException"><paramref name="s"/> is not in a valid quantity format</exception>
    public static Quantity Parse(string s, IFormatProvider? provider = null)
    {
        ArgumentNullException.ThrowIfNull(s);
        if (int.TryParse(s, QuantityNumberStyles, provider ?? CultureInfo.InvariantCulture, out int val))
        {
            var result = Create(val);
            if (result.IsSuccess) return result.Value;
            throw new FormatException(result.Error.Description);
        }

        throw new FormatException($"Cannot parse '{s}' as Quantity.");
    }

    /// <summary>
    /// Attempts to parse a string into a <see cref="Quantity"/>.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <param name="result">When this method returns, contains the parsed quantity if successful; otherwise, default.</param>
    /// <returns><see langword="true"/> if parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(string? s, IFormatProvider? provider, out Quantity result)
    {
        if (int.TryParse(s, QuantityNumberStyles, provider ?? CultureInfo.InvariantCulture, out int val))
        {
            var res = Create(val);
            if (res.IsSuccess)
            {
                result = res.Value;
                return true;
            }
        }

        result = default;
        return false;
    }

    /// <summary>
    /// Parses a span of characters into a <see cref="Quantity"/>.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <returns>The parsed <see cref="Quantity"/>.</returns>
    /// <exception cref="FormatException"><paramref name="s"/> is not in a valid quantity format</exception>
    public static Quantity Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null)
    {
        if (int.TryParse(s, QuantityNumberStyles, provider ?? CultureInfo.InvariantCulture, out int val))
        {
            var result = Create(val);
            if (result.IsSuccess) return result.Value;
            throw new FormatException(result.Error.Description);
        }

        throw new FormatException($"Cannot parse '{s.ToString()}' as Quantity.");
    }

    /// <summary>
    /// Attempts to parse a span of characters into a <see cref="Quantity"/>.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <param name="result">When this method returns, contains the parsed quantity if successful; otherwise, default.</param>
    /// <returns><see langword="true"/> if parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Quantity result)
    {
        if (int.TryParse(s, QuantityNumberStyles, provider ?? CultureInfo.InvariantCulture, out int val))
        {
            var res = Create(val);
            if (res.IsSuccess)
            {
                result = res.Value;
                return true;
            }
        }

        result = default;
        return false;
    }
}

