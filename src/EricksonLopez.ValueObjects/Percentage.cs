// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a bounded percentage value between 0 and 100 inclusive.
/// </summary>
public readonly record struct Percentage : IValueObject<Percentage>, IComparable<Percentage>, IComparable, IParsable<Percentage>, ISpanParsable<Percentage>, IFormattable, ISpanFormattable
{
    /// <summary>
    /// Gets the numerical percentage value between 0 and 100.
    /// </summary>
    public decimal Value { get; }

    /// <summary>
    /// Gets the fractional representation of the percentage (value divided by 100).
    /// </summary>
    public decimal Fraction => Value / 100m;

    /// <summary>
    /// Gets the fractional representation of the percentage.
    /// </summary>
    /// <remarks>This property is an alias of <see cref="Fraction"/> provided for fluent readability.</remarks>
    public decimal AsFraction => Fraction;

    /// <summary>
    /// Gets a percentage instance representing 0%.
    /// </summary>
    public static readonly Percentage Zero = new(0m);

    /// <summary>
    /// Gets a percentage instance representing 100%.
    /// </summary>
    public static readonly Percentage Hundred = new(100m);

    /// <summary>
    /// Gets a percentage instance representing 100%.
    /// </summary>
    public static readonly Percentage Full = Hundred;

    private Percentage(decimal value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a validated <see cref="Percentage"/> instance from a numeric value in the range [0, 100].
    /// </summary>
    /// <param name="value">The percentage amount between 0 and 100.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the validated percentage, or a validation failure.</returns>
    public static Result<Percentage> Create(decimal value)
    {
        var result = ValidatePercentage(value, nameof(Percentage));
        return result.IsFailure
            ? Result<Percentage>.Failure(result.Error)
            : Result<Percentage>.Success(new Percentage(value));
    }

    /// <summary>
    /// Creates a validated <see cref="Percentage"/> instance from a fractional value in the range [0.0, 1.0].
    /// </summary>
    /// <param name="fraction">The fraction multiplier between 0.0 and 1.0.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the validated percentage, or a validation failure.</returns>
    public static Result<Percentage> FromFraction(decimal fraction) =>
        Create(fraction * 100m);

    /// <summary>
    /// Validates that a decimal percentage value falls between 0 and 100 with at most 6 decimal places.
    /// </summary>
    /// <param name="value">The numeric percentage value to validate.</param>
    /// <param name="fieldName">The name of the field used for error reporting.</param>
    /// <returns>A successful <see cref="global::EricksonLopez.Result.Result"/> if valid; otherwise, a validation failure.</returns>
    public static global::EricksonLopez.Result.Result ValidatePercentage(decimal value, string fieldName)
    {
        if (value is < 0m or > 100m)
        {
            return global::EricksonLopez.Result.Result.Failure(Error.Validation($"{fieldName}.OutOfRange", $"{fieldName} must be between 0 and 100."));
        }

        if (!NumericValidation.IsScaleAtMost(value, 6))
        {
            return global::EricksonLopez.Result.Result.Failure(Error.Validation($"{fieldName}.TooManyDecimals", $"{fieldName} supports at most 6 decimal places."));
        }

        return global::EricksonLopez.Result.Result.Success();
    }

    /// <summary>
    /// Calculates the percentage share of a specified base amount.
    /// </summary>
    /// <param name="baseAmount">The base amount to apply the percentage to.</param>
    /// <returns>The calculated portion rounded to 6 decimal places.</returns>
    public decimal ApplyTo(decimal baseAmount) => Math.Round(baseAmount * Fraction, 6);

    /// <summary>
    /// Gets a value indicating whether the percentage is zero.
    /// </summary>
    public bool IsZero => Value == 0m;

    /// <summary>
    /// Compares this percentage with another percentage.
    /// </summary>
    /// <param name="other">The other percentage to compare against.</param>
    /// <returns>A value indicating the relative order of the percentages being compared.</returns>
    public int CompareTo(Percentage other) => Value.CompareTo(other.Value);

    /// <inheritdoc/>
    /// <exception cref="ArgumentException"><paramref name="obj"/> is not of type <see cref="Percentage"/></exception>
    public int CompareTo(object? obj) =>
        obj is Percentage other ? CompareTo(other) : throw new ArgumentException("Object is not a Percentage", nameof(obj));

    /// <summary>
    /// Determines whether the left percentage is less than the right percentage.
    /// </summary>
    /// <param name="left">The first percentage to compare.</param>
    /// <param name="right">The second percentage to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(Percentage left, Percentage right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left percentage is less than or equal to the right percentage.
    /// </summary>
    /// <param name="left">The first percentage to compare.</param>
    /// <param name="right">The second percentage to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(Percentage left, Percentage right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left percentage is greater than the right percentage.
    /// </summary>
    /// <param name="left">The first percentage to compare.</param>
    /// <param name="right">The second percentage to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(Percentage left, Percentage right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left percentage is greater than or equal to the right percentage.
    /// </summary>
    /// <param name="left">The first percentage to compare.</param>
    /// <param name="right">The second percentage to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(Percentage left, Percentage right) => left.CompareTo(right) >= 0;

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
    /// Parses a string into a <see cref="Percentage"/>.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <returns>The parsed <see cref="Percentage"/>.</returns>
    /// <exception cref="FormatException"><paramref name="s"/> is not in a valid percentage format</exception>
    public static Percentage Parse(string s, IFormatProvider? provider = null)
    {
        ArgumentNullException.ThrowIfNull(s);
        string cleaned = s.Trim().TrimEnd('%').Trim();
        if (decimal.TryParse(cleaned, NumberStyles.Number, provider ?? CultureInfo.InvariantCulture, out decimal val))
        {
            var result = Create(val);
            if (result.IsSuccess) return result.Value;
            throw new FormatException(result.Error.Description);
        }

        throw new FormatException($"Cannot parse '{s}' as Percentage.");
    }

    /// <summary>
    /// Attempts to parse a string into a <see cref="Percentage"/>.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <param name="result">When this method returns, contains the parsed percentage if successful; otherwise, default.</param>
    /// <returns><see langword="true"/> if parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(string? s, IFormatProvider? provider, out Percentage result)
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
    /// Parses a span of characters into a <see cref="Percentage"/>.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <returns>The parsed <see cref="Percentage"/>.</returns>
    /// <exception cref="FormatException"><paramref name="s"/> is not in a valid percentage format</exception>
    public static Percentage Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null)
    {
        var cleaned = s.Trim().TrimEnd('%').Trim();
        if (decimal.TryParse(cleaned, NumberStyles.Number, provider ?? CultureInfo.InvariantCulture, out decimal val))
        {
            var result = Create(val);
            if (result.IsSuccess) return result.Value;
            throw new FormatException(result.Error.Description);
        }

        throw new FormatException($"Cannot parse '{s.ToString()}' as Percentage.");
    }

    /// <summary>
    /// Attempts to parse a span of characters into a <see cref="Percentage"/>.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <param name="result">When this method returns, contains the parsed percentage if successful; otherwise, default.</param>
    /// <returns><see langword="true"/> if parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Percentage result)
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

