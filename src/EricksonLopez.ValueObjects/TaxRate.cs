// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a tax rate percentage value between 0 and 100 inclusive.
/// </summary>
public readonly record struct TaxRate : IValueObject<TaxRate>, IComparable<TaxRate>, IComparable, IParsable<TaxRate>, ISpanParsable<TaxRate>, IFormattable, ISpanFormattable
{
    /// <summary>
    /// Gets the numerical tax rate percentage.
    /// </summary>
    public decimal Value { get; }

    private TaxRate(decimal value) => Value = value;

    /// <summary>
    /// Creates a validated <see cref="TaxRate"/> instance from a percentage value in the range [0, 100].
    /// </summary>
    /// <param name="value">The tax percentage amount.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the validated tax rate, or a validation failure.</returns>
    public static Result<TaxRate> Create(decimal value)
    {
        var result = Percentage.ValidatePercentage(value, nameof(TaxRate));
        return result.IsFailure
            ? Result<TaxRate>.Failure(result.Error)
            : Result<TaxRate>.Success(new TaxRate(value));
    }

    /// <summary>
    /// Gets the fractional representation of the tax rate (value divided by 100).
    /// </summary>
    public decimal Fraction => Value / 100m;

    /// <summary>
    /// Gets the fractional representation of the tax rate.
    /// </summary>
    /// <remarks>This property is an alias of <see cref="Fraction"/> provided for fluent readability.</remarks>
    public decimal AsFraction => Fraction;

    /// <summary>
    /// Calculates the tax amount for a specified base numeric value.
    /// </summary>
    /// <param name="baseAmount">The base monetary or numeric value.</param>
    /// <returns>The calculated tax amount rounded to 6 decimal places.</returns>
    public decimal CalculateTax(decimal baseAmount) => Math.Round(baseAmount * Fraction, 6);

    /// <summary>
    /// Calculates the tax amount for a specified monetary value using banker's rounding.
    /// </summary>
    /// <param name="baseAmount">The base monetary value.</param>
    /// <returns>The calculated tax <see cref="Money"/> amount.</returns>
    public Money CalculateTax(Money baseAmount)
    {
        var percentage = Percentage.Create(Value).Value;
        return baseAmount.ApplyPercentage(percentage);
    }

    /// <summary>
    /// Gets a value indicating whether the tax rate is zero (tax exempt).
    /// </summary>
    public bool IsExempt => Value == 0m;

    /// <summary>
    /// Gets a tax rate representing full tax exemption (0%).
    /// </summary>
    public static readonly TaxRate Exempt = new(0m);

    /// <summary>
    /// Compares this tax rate with another tax rate.
    /// </summary>
    /// <param name="other">The other tax rate to compare against.</param>
    /// <returns>A value indicating the relative order of the tax rates being compared.</returns>
    public int CompareTo(TaxRate other) => Value.CompareTo(other.Value);

    /// <inheritdoc/>
    /// <exception cref="ArgumentException"><paramref name="obj"/> is not of type <see cref="TaxRate"/></exception>
    public int CompareTo(object? obj) =>
        obj is TaxRate other ? CompareTo(other) : throw new ArgumentException("Object is not a TaxRate", nameof(obj));

    /// <summary>
    /// Determines whether the left tax rate is less than the right tax rate.
    /// </summary>
    /// <param name="left">The first tax rate to compare.</param>
    /// <param name="right">The second tax rate to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(TaxRate left, TaxRate right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left tax rate is less than or equal to the right tax rate.
    /// </summary>
    /// <param name="left">The first tax rate to compare.</param>
    /// <param name="right">The second tax rate to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(TaxRate left, TaxRate right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left tax rate is greater than the right tax rate.
    /// </summary>
    /// <param name="left">The first tax rate to compare.</param>
    /// <param name="right">The second tax rate to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(TaxRate left, TaxRate right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left tax rate is greater than or equal to the right tax rate.
    /// </summary>
    /// <param name="left">The first tax rate to compare.</param>
    /// <param name="right">The second tax rate to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(TaxRate left, TaxRate right) => left.CompareTo(right) >= 0;

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
    /// Parses a string into a <see cref="TaxRate"/>.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <returns>The parsed <see cref="TaxRate"/>.</returns>
    /// <exception cref="FormatException"><paramref name="s"/> is not in a valid tax rate format</exception>
    public static TaxRate Parse(string s, IFormatProvider? provider = null)
    {
        ArgumentNullException.ThrowIfNull(s);
        string cleaned = s.Trim().TrimEnd('%').Trim();
        if (decimal.TryParse(cleaned, NumberStyles.Number, provider ?? CultureInfo.InvariantCulture, out decimal val))
        {
            var result = Create(val);
            if (result.IsSuccess) return result.Value;
            throw new FormatException(result.Error.Description);
        }

        throw new FormatException($"Cannot parse '{s}' as TaxRate.");
    }

    /// <summary>
    /// Attempts to parse a string into a <see cref="TaxRate"/>.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <param name="result">When this method returns, contains the parsed tax rate if successful; otherwise, default.</param>
    /// <returns><see langword="true"/> if parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(string? s, IFormatProvider? provider, out TaxRate result)
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
    /// Parses a span of characters into a <see cref="TaxRate"/>.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <returns>The parsed <see cref="TaxRate"/>.</returns>
    /// <exception cref="FormatException"><paramref name="s"/> is not in a valid tax rate format</exception>
    public static TaxRate Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null)
    {
        var cleaned = s.Trim().TrimEnd('%').Trim();
        if (decimal.TryParse(cleaned, NumberStyles.Number, provider ?? CultureInfo.InvariantCulture, out decimal val))
        {
            var result = Create(val);
            if (result.IsSuccess) return result.Value;
            throw new FormatException(result.Error.Description);
        }

        throw new FormatException($"Cannot parse '{s.ToString()}' as TaxRate.");
    }

    /// <summary>
    /// Attempts to parse a span of characters into a <see cref="TaxRate"/>.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <param name="result">When this method returns, contains the parsed tax rate if successful; otherwise, default.</param>
    /// <returns><see langword="true"/> if parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out TaxRate result)
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

