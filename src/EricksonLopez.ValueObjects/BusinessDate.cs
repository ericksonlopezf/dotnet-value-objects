// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a calendar date in a business domain without time or time zone components.
/// </summary>
public readonly record struct BusinessDate : IValueObject<BusinessDate>, IComparable<BusinessDate>, IComparable, IParsable<BusinessDate>, ISpanParsable<BusinessDate>, IFormattable, ISpanFormattable
{
    /// <summary>
    /// Gets the underlying <see cref="DateOnly"/> value.
    /// </summary>
    public DateOnly Value { get; }

    private BusinessDate(DateOnly value) => Value = value;

    /// <summary>
    /// Creates a validated <see cref="BusinessDate"/> instance from a <see cref="DateOnly"/>.
    /// </summary>
    /// <param name="value">The date value to encapsulate.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the validated business date, or a validation failure.</returns>
    public static Result<BusinessDate> Create(DateOnly value)
    {
        if (value == DateOnly.MinValue || value == DateOnly.MaxValue)
        {
            return Result<BusinessDate>.Failure(Error.Validation(
                "BusinessDate.OutOfRange",
                "Business date cannot be DateOnly.MinValue or DateOnly.MaxValue."));
        }

        return Result<BusinessDate>.Success(new BusinessDate(value));
    }

    /// <summary>
    /// Creates a <see cref="BusinessDate"/> instance from the UTC date component of a <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="value">The date and time value to extract the UTC date from.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the validated business date.</returns>
    public static Result<BusinessDate> FromDateTimeOffset(DateTimeOffset value) =>
        Create(DateOnly.FromDateTime(value.UtcDateTime));

    /// <summary>
    /// Compares this business date with another business date.
    /// </summary>
    /// <param name="other">The other business date to compare against.</param>
    /// <returns>A value indicating the relative order of the dates being compared.</returns>
    public int CompareTo(BusinessDate other) => Value.CompareTo(other.Value);

    /// <inheritdoc/>
    /// <exception cref="ArgumentException"><paramref name="obj"/> is not of type <see cref="BusinessDate"/></exception>
    public int CompareTo(object? obj) =>
        obj is BusinessDate other ? CompareTo(other) : throw new ArgumentException("Object is not a BusinessDate", nameof(obj));

    /// <summary>
    /// Determines whether the left business date is earlier than the right business date.
    /// </summary>
    /// <param name="left">The first business date to compare.</param>
    /// <param name="right">The second business date to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is earlier than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(BusinessDate left, BusinessDate right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left business date is earlier than or equal to the right business date.
    /// </summary>
    /// <param name="left">The first business date to compare.</param>
    /// <param name="right">The second business date to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is earlier than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(BusinessDate left, BusinessDate right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left business date is later than the right business date.
    /// </summary>
    /// <param name="left">The first business date to compare.</param>
    /// <param name="right">The second business date to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is later than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(BusinessDate left, BusinessDate right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left business date is later than or equal to the right business date.
    /// </summary>
    /// <param name="left">The first business date to compare.</param>
    /// <param name="right">The second business date to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is later than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(BusinessDate left, BusinessDate right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public override string ToString() => ToString(null, null);

    /// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
    public string ToString(string? format, IFormatProvider? formatProvider) =>
        Value.ToString(format ?? "yyyy-MM-dd", formatProvider ?? CultureInfo.InvariantCulture);

    /// <inheritdoc/>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) =>
        Value.TryFormat(destination, out charsWritten, format.IsEmpty ? "yyyy-MM-dd" : format, provider ?? CultureInfo.InvariantCulture);

    /// <summary>
    /// Parses a string into a <see cref="BusinessDate"/>.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <returns>The parsed <see cref="BusinessDate"/>.</returns>
    /// <exception cref="FormatException"><paramref name="s"/> is not in a valid date format</exception>
    public static BusinessDate Parse(string s, IFormatProvider? provider = null)
    {
        ArgumentNullException.ThrowIfNull(s);
        if (DateOnly.TryParse(s, provider ?? CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly date))
        {
            var result = Create(date);
            if (result.IsSuccess) return result.Value;
            throw new FormatException(result.Error.Description);
        }

        throw new FormatException($"Cannot parse '{s}' as BusinessDate.");
    }

    /// <summary>
    /// Attempts to parse a string into a <see cref="BusinessDate"/>.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <param name="result">When this method returns, contains the parsed business date if successful; otherwise, default.</param>
    /// <returns><see langword="true"/> if parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(string? s, IFormatProvider? provider, out BusinessDate result)
    {
        if (DateOnly.TryParse(s, provider ?? CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly date))
        {
            var res = Create(date);
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
    /// Parses a span of characters into a <see cref="BusinessDate"/>.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <returns>The parsed <see cref="BusinessDate"/>.</returns>
    /// <exception cref="FormatException"><paramref name="s"/> is not in a valid date format</exception>
    public static BusinessDate Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null)
    {
        if (DateOnly.TryParse(s, provider ?? CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly date))
        {
            var result = Create(date);
            if (result.IsSuccess) return result.Value;
            throw new FormatException(result.Error.Description);
        }

        throw new FormatException($"Cannot parse '{s.ToString()}' as BusinessDate.");
    }

    /// <summary>
    /// Attempts to parse a span of characters into a <see cref="BusinessDate"/>.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <param name="result">When this method returns, contains the parsed business date if successful; otherwise, default.</param>
    /// <returns><see langword="true"/> if parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out BusinessDate result)
    {
        if (DateOnly.TryParse(s, provider ?? CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly date))
        {
            var res = Create(date);
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

