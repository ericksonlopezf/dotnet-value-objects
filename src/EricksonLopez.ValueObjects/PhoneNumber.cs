// Copyright © Erickson Lopez. MIT License.
using System;
using System.Linq;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a telephone number formatted according to the E.164 international standard.
/// </summary>
public readonly record struct PhoneNumber : IValueObject<PhoneNumber>, IComparable<PhoneNumber>, IComparable, IParsable<PhoneNumber>, ISpanParsable<PhoneNumber>
{
    /// <summary>
    /// Gets the international telephone number string including the leading <c>+</c> prefix.
    /// </summary>
    public string Value { get; }

    private PhoneNumber(string value) => Value = value;

    /// <summary>
    /// Creates a validated <see cref="PhoneNumber"/> instance from an input telephone string.
    /// </summary>
    /// <param name="value">The raw telephone string.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the validated phone number, or a validation failure.</returns>
    public static Result<PhoneNumber> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result<PhoneNumber>.Failure(Error.Validation(
                "PhoneNumber.Required", "Phone number is required."));
        }

        string digits = value.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");

        if (!digits.StartsWith('+'))
        {
            return Result<PhoneNumber>.Failure(Error.Validation(
                "PhoneNumber.MissingCountryCode",
                "Phone number must start with '+' country code (E.164 format)."));
        }

        string numberPart = digits[1..];
        if (numberPart.Length is < 8 or > 15 || !numberPart.All(char.IsDigit))
        {
            return Result<PhoneNumber>.Failure(Error.Validation(
                "PhoneNumber.InvalidFormat",
                "Phone number must be E.164 format with 8-15 digits after '+'."));
        }

        return Result<PhoneNumber>.Success(new PhoneNumber(digits));
    }

    /// <summary>
    /// Gets the numerical telephone digits excluding the leading <c>+</c> prefix.
    /// </summary>
    public string DigitsOnly => Value?[1..] ?? string.Empty;

    /// <summary>
    /// Gets a human-readable formatted representation of the phone number when applicable.
    /// </summary>
    public string Formatted => Value is not null && Value.Length == 12 && Value.StartsWith("+1", StringComparison.Ordinal)
        ? $"({Value[2..5]}) {Value[5..8]}-{Value[8..12]}"
        : Value ?? string.Empty;

    /// <summary>
    /// Compares this phone number with another phone number using ordinal string comparison.
    /// </summary>
    /// <param name="other">The other phone number to compare against.</param>
    /// <returns>A value indicating the relative order of the phone numbers being compared.</returns>
    public int CompareTo(PhoneNumber other) => string.Compare(Value, other.Value, StringComparison.Ordinal);

    /// <inheritdoc/>
    /// <exception cref="ArgumentException"><paramref name="obj"/> is not of type <see cref="PhoneNumber"/></exception>
    public int CompareTo(object? obj) =>
        obj is PhoneNumber other ? CompareTo(other) : throw new ArgumentException("Object is not a PhoneNumber", nameof(obj));

    /// <summary>
    /// Determines whether the left phone number is less than the right phone number.
    /// </summary>
    /// <param name="left">The first phone number to compare.</param>
    /// <param name="right">The second phone number to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(PhoneNumber left, PhoneNumber right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left phone number is less than or equal to the right phone number.
    /// </summary>
    /// <param name="left">The first phone number to compare.</param>
    /// <param name="right">The second phone number to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(PhoneNumber left, PhoneNumber right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left phone number is greater than the right phone number.
    /// </summary>
    /// <param name="left">The first phone number to compare.</param>
    /// <param name="right">The second phone number to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(PhoneNumber left, PhoneNumber right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left phone number is greater than or equal to the right phone number.
    /// </summary>
    /// <param name="left">The first phone number to compare.</param>
    /// <param name="right">The second phone number to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(PhoneNumber left, PhoneNumber right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public override string ToString() => Value ?? string.Empty;

    /// <summary>
    /// Parses a string into a <see cref="PhoneNumber"/>.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <returns>The parsed <see cref="PhoneNumber"/>.</returns>
    /// <exception cref="FormatException"><paramref name="s"/> is not in a valid phone number format</exception>
    public static PhoneNumber Parse(string s, IFormatProvider? provider = null)
    {
        var result = Create(s);
        return result.IsSuccess ? result.Value : throw new FormatException(result.Error.Description);
    }

    /// <summary>
    /// Attempts to parse a string into a <see cref="PhoneNumber"/>.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <param name="result">When this method returns, contains the parsed phone number if successful; otherwise, default.</param>
    /// <returns><see langword="true"/> if parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(string? s, IFormatProvider? provider, out PhoneNumber result)
    {
        var res = Create(s);
        if (res.IsSuccess)
        {
            result = res.Value;
            return true;
        }

        result = default;
        return false;
    }

    /// <summary>
    /// Parses a span of characters into a <see cref="PhoneNumber"/>.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <returns>The parsed <see cref="PhoneNumber"/>.</returns>
    public static PhoneNumber Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        Parse(s.ToString(), provider);

    /// <summary>
    /// Attempts to parse a span of characters into a <see cref="PhoneNumber"/>.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <param name="result">When this method returns, contains the parsed phone number if successful; otherwise, default.</param>
    /// <returns><see langword="true"/> if parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out PhoneNumber result) =>
        TryParse(s.ToString(), provider, out result);
}


