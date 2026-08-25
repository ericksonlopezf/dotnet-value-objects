// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a commercial discount rate percentage value between 0 and 100 inclusive.
/// </summary>
public readonly record struct DiscountRate : IValueObject<DiscountRate>, IComparable<DiscountRate>, IComparable, IParsable<DiscountRate>, ISpanParsable<DiscountRate>, IFormattable, ISpanFormattable
{
    /// <summary>
    /// Gets the numerical discount rate percentage.
    /// </summary>
    public decimal Value { get; }

    private DiscountRate(decimal value) => Value = value;

    /// <summary>
    /// Creates a validated <see cref="DiscountRate"/> instance from a percentage value in the range [0, 100].
    /// </summary>
    /// <param name="value">The discount percentage amount.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the validated discount rate, or a validation failure.</returns>
    public static Result<DiscountRate> Create(decimal value)
    {
        var result = Percentage.ValidatePercentage(value, nameof(DiscountRate));
        return result.IsFailure
            ? Result<DiscountRate>.Failure(result.Error)
            : Result<DiscountRate>.Success(new DiscountRate(value));
    }

    /// <summary>
    /// Gets the fractional representation of the discount rate (value divided by 100).
    /// </summary>
    public decimal Fraction => Value / 100m;

    /// <summary>
    /// Gets the fractional representation of the discount rate.
    /// </summary>
    public decimal AsFraction => Fraction;

    /// <summary>
    /// Calculates the absolute discount amount for a specified base numeric value.
    /// </summary>
    /// <param name="baseAmount">The base monetary or numeric value.</param>
    /// <returns>The calculated discount amount rounded to 6 decimal places.</returns>
    public decimal CalculateDiscount(decimal baseAmount) => Math.Round(baseAmount * Fraction, 6);

    /// <summary>
    /// Calculates the net amount remaining after applying the discount to a base numeric value.
    /// </summary>
    /// <param name="baseAmount">The base numeric amount before discount.</param>
    /// <returns>The net amount remaining after applying the discount, rounded to 6 decimal places.</returns>
    public decimal ApplyTo(decimal baseAmount) => Math.Round(baseAmount * (1m - Fraction), 6);

    /// <summary>
    /// Calculates the net monetary amount remaining after applying the discount using banker's rounding.
    /// </summary>
    /// <param name="baseAmount">The base monetary amount before discount.</param>
    /// <returns>The net <see cref="Money"/> remaining after discount.</returns>
    public Money ApplyTo(Money baseAmount)
    {
        var percentage = Percentage.Create(Value).Value;
        return baseAmount - baseAmount.ApplyPercentage(percentage);
    }

    /// <summary>
    /// Gets a value indicating whether the discount rate is zero.
    /// </summary>
    public bool IsZero => Value == 0m;

    /// <summary>
    /// Gets a discount rate representing zero discount (0%).
    /// </summary>
    public static readonly DiscountRate None = new(0m);

    /// <summary>
    /// Compares this discount rate with another discount rate.
    /// </summary>
    /// <param name="other">The other discount rate to compare against.</param>
    /// <returns>A value indicating the relative order of the discount rates being compared.</returns>
    public int CompareTo(DiscountRate other) => Value.CompareTo(other.Value);

    /// <inheritdoc/>
    /// <exception cref="ArgumentException"><paramref name="obj"/> is not of type <see cref="DiscountRate"/></exception>
    public int CompareTo(object? obj) =>
        obj is DiscountRate other ? CompareTo(other) : throw new ArgumentException("Object is not a DiscountRate", nameof(obj));

    /// <summary>
    /// Determines whether the left discount rate is less than the right discount rate.
    /// </summary>
    /// <param name="left">The first discount rate to compare.</param>
    /// <param name="right">The second discount rate to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(DiscountRate left, DiscountRate right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left discount rate is less than or equal to the right discount rate.
    /// </summary>
    /// <param name="left">The first discount rate to compare.</param>
    /// <param name="right">The second discount rate to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(DiscountRate left, DiscountRate right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left discount rate is greater than the right discount rate.
    /// </summary>
    /// <param name="left">The first discount rate to compare.</param>
    /// <param name="right">The second discount rate to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(DiscountRate left, DiscountRate right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left discount rate is greater than or equal to the right discount rate.
    /// </summary>
    /// <param name="left">The first discount rate to compare.</param>
    /// <param name="right">The second discount rate to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(DiscountRate left, DiscountRate right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public override string ToString() => ToString(null, null);

    /// <inheritdoc cref="IFormattable.ToString(string?, IFormatProvider?)"/>
    public string ToString(string? format, IFormatProvider? formatProvider) =>
        $"{Value.ToString(format, formatProvider ?? CultureInfo.InvariantCulture)}%";

    /// <inheritdoc/>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        string formatted = ToString(format.ToString(), provider);
        if (formatted.Length <= destination.Length)
        {
            formatted.AsSpan().CopyTo(destination);
            charsWritten = formatted.Length;
            return true;
        }

        charsWritten = 0;
        return false;
    }

    /// <summary>
    /// Parses a string into a <see cref="DiscountRate"/>.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <returns>The parsed <see cref="DiscountRate"/>.</returns>
    /// <exception cref="FormatException"><paramref name="s"/> is not in a valid discount rate format</exception>
    public static DiscountRate Parse(string s, IFormatProvider? provider = null)
    {
        ArgumentNullException.ThrowIfNull(s);
        string cleaned = s.Trim().TrimEnd('%').Trim();
        if (decimal.TryParse(cleaned, NumberStyles.Number, provider ?? CultureInfo.InvariantCulture, out decimal val))
        {
            var result = Create(val);
            if (result.IsSuccess) return result.Value;
            throw new FormatException(result.Error.Description);
        }

        throw new FormatException($"Cannot parse '{s}' as DiscountRate.");
    }

    /// <summary>
    /// Attempts to parse a string into a <see cref="DiscountRate"/>.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <param name="result">When this method returns, contains the parsed discount rate if successful; otherwise, default.</param>
    /// <returns><see langword="true"/> if parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(string? s, IFormatProvider? provider, out DiscountRate result)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            result = default;
            return false;
        }

        string cleaned = s.Trim().TrimEnd('%').Trim();
        if (decimal.TryParse(cleaned, NumberStyles.Number, provider ?? CultureInfo.InvariantCulture, out decimal val))
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
    /// Parses a span of characters into a <see cref="DiscountRate"/>.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <returns>The parsed <see cref="DiscountRate"/>.</returns>
    /// <exception cref="FormatException"><paramref name="s"/> is not in a valid discount rate format</exception>
    public static DiscountRate Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null)
    {
        var cleaned = s.Trim().TrimEnd('%').Trim();
        if (decimal.TryParse(cleaned, NumberStyles.Number, provider ?? CultureInfo.InvariantCulture, out decimal val))
        {
            var result = Create(val);
            if (result.IsSuccess) return result.Value;
            throw new FormatException(result.Error.Description);
        }

        throw new FormatException($"Cannot parse '{s.ToString()}' as DiscountRate.");
    }

    /// <summary>
    /// Attempts to parse a span of characters into a <see cref="DiscountRate"/>.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <param name="result">When this method returns, contains the parsed discount rate if successful; otherwise, default.</param>
    /// <returns><see langword="true"/> if parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out DiscountRate result)
    {
        var cleaned = s.Trim().TrimEnd('%').Trim();
        if (decimal.TryParse(cleaned, NumberStyles.Number, provider ?? CultureInfo.InvariantCulture, out decimal val))
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

