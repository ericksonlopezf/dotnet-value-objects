// Copyright © Erickson Lopez. MIT License.
using System;
using System.Text.RegularExpressions;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents an ISO 4217 three-letter currency code.
/// </summary>
/// <remarks>
/// Encapsulates currency formatting, standard decimal scale information, and relational comparison.
/// </remarks>
public readonly partial record struct CurrencyCode : IValueObject<CurrencyCode>, IComparable<CurrencyCode>, IComparable, IParsable<CurrencyCode>, ISpanParsable<CurrencyCode>
{
    [GeneratedRegex(@"^[A-Z]{3}$")]
    private static partial Regex Iso4217Regex();

    /// <summary>
    /// Gets the three-letter ISO 4217 currency code string.
    /// </summary>
    public string Value { get; }

    private CurrencyCode(string value) => Value = value;

    /// <summary>
    /// Creates a validated <see cref="CurrencyCode"/> instance from an ISO 4217 code string.
    /// </summary>
    /// <param name="value">The raw three-letter currency string.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the validated currency code, or a validation failure.</returns>
    public static Result<CurrencyCode> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result<CurrencyCode>.Failure(Error.Validation(
                "CurrencyCode.Required", "Currency code is required."));
        }

        string normalized = value.Trim().ToUpperInvariant();

        if (!Iso4217Regex().IsMatch(normalized))
        {
            return Result<CurrencyCode>.Failure(Error.Validation(
                "CurrencyCode.InvalidFormat",
                $"Currency code must be exactly 3 uppercase letters (ISO 4217), got '{value}'."));
        }

        return Result<CurrencyCode>.Success(new CurrencyCode(normalized));
    }

    /// <summary>
    /// Gets the standard number of decimal places defined by ISO 4217 for this currency.
    /// </summary>
    public int DecimalPlaces => Value switch
    {
        // 0 decimal places
        "BIF" or "CLP" or "DJF" or "GNF" or "ISK" or "JPY" or "KMF" or "KRW" or "MGA" or
        "PYG" or "RWF" or "UGX" or "VND" or "VUV" or "XAF" or "XOF" or "XPF" => 0,

        // 3 decimal places
        "BHD" or "IQD" or "JOD" or "KWD" or "LYD" or "OMR" or "TND" => 3,

        // 4 decimal places
        "CLF" or "UYW" => 4,

        // All others default to 2
        _ => 2,
    };

    /// <summary>
    /// Compares this currency code with another currency code based on ordinal character order.
    /// </summary>
    /// <param name="other">The other currency code to compare with.</param>
    /// <returns>A value indicating the relative order of the currency codes being compared.</returns>
    public int CompareTo(CurrencyCode other) => string.Compare(Value, other.Value, StringComparison.Ordinal);

    /// <inheritdoc/>
    /// <exception cref="ArgumentException"><paramref name="obj"/> is not of type <see cref="CurrencyCode"/></exception>
    public int CompareTo(object? obj) =>
        obj is CurrencyCode other ? CompareTo(other) : throw new ArgumentException("Object is not a CurrencyCode", nameof(obj));

    /// <summary>
    /// Determines whether the left currency code is less than the right currency code.
    /// </summary>
    /// <param name="left">The first currency code to compare.</param>
    /// <param name="right">The second currency code to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(CurrencyCode left, CurrencyCode right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left currency code is less than or equal to the right currency code.
    /// </summary>
    /// <param name="left">The first currency code to compare.</param>
    /// <param name="right">The second currency code to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(CurrencyCode left, CurrencyCode right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left currency code is greater than the right currency code.
    /// </summary>
    /// <param name="left">The first currency code to compare.</param>
    /// <param name="right">The second currency code to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(CurrencyCode left, CurrencyCode right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left currency code is greater than or equal to the right currency code.
    /// </summary>
    /// <param name="left">The first currency code to compare.</param>
    /// <param name="right">The second currency code to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(CurrencyCode left, CurrencyCode right) => left.CompareTo(right) >= 0;

    // ── Well-known instances ──

    /// <summary>
    /// Gets the Dominican Peso (DOP) currency code.
    /// </summary>
    public static CurrencyCode DOP => new("DOP");

    /// <summary>
    /// Gets the United States Dollar (USD) currency code.
    /// </summary>
    public static CurrencyCode USD => new("USD");

    /// <summary>
    /// Gets the Euro (EUR) currency code.
    /// </summary>
    public static CurrencyCode EUR => new("EUR");

    /// <summary>
    /// Gets the British Pound Sterling (GBP) currency code.
    /// </summary>
    public static CurrencyCode GBP => new("GBP");

    /// <summary>
    /// Gets the Japanese Yen (JPY) currency code.
    /// </summary>
    public static CurrencyCode JPY => new("JPY");

    /// <summary>
    /// Gets the Bahraini Dinar (BHD) currency code.
    /// </summary>
    public static CurrencyCode BHD => new("BHD");

    /// <summary>
    /// Gets the Kuwaiti Dinar (KWD) currency code.
    /// </summary>
    public static CurrencyCode KWD => new("KWD");

    /// <inheritdoc/>
    public override string ToString() => Value ?? string.Empty;

    /// <summary>
    /// Parses a string into a <see cref="CurrencyCode"/>.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <returns>The parsed <see cref="CurrencyCode"/>.</returns>
    /// <exception cref="FormatException"><paramref name="s"/> is not in a valid currency code format</exception>
    public static CurrencyCode Parse(string s, IFormatProvider? provider = null)
    {
        var result = Create(s);
        return result.IsSuccess ? result.Value : throw new FormatException(result.Error.Description);
    }

    /// <summary>
    /// Attempts to parse a string into a <see cref="CurrencyCode"/>.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <param name="result">When this method returns, contains the parsed currency code if successful; otherwise, default.</param>
    /// <returns><see langword="true"/> if parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(string? s, IFormatProvider? provider, out CurrencyCode result)
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
    /// Parses a span of characters into a <see cref="CurrencyCode"/>.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <returns>The parsed <see cref="CurrencyCode"/>.</returns>
    /// <exception cref="FormatException"><paramref name="s"/> is not in a valid currency code format</exception>
    public static CurrencyCode Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null)
    {
        if (TryParse(s, provider, out var result))
        {
            return result;
        }

        throw new FormatException($"Cannot parse '{s.ToString()}' as CurrencyCode.");
    }

    /// <summary>
    /// Attempts to parse a span of characters into a <see cref="CurrencyCode"/>.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <param name="result">When this method returns, contains the parsed currency code if successful; otherwise, default.</param>
    /// <returns><see langword="true"/> if parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out CurrencyCode result)
    {
        var trimmed = s.Trim();
        if (trimmed.Length != 3)
        {
            result = default;
            return false;
        }

        if (trimmed.Equals("USD", StringComparison.OrdinalIgnoreCase)) { result = USD; return true; }
        if (trimmed.Equals("EUR", StringComparison.OrdinalIgnoreCase)) { result = EUR; return true; }
        if (trimmed.Equals("DOP", StringComparison.OrdinalIgnoreCase)) { result = DOP; return true; }
        if (trimmed.Equals("GBP", StringComparison.OrdinalIgnoreCase)) { result = GBP; return true; }
        if (trimmed.Equals("JPY", StringComparison.OrdinalIgnoreCase)) { result = JPY; return true; }
        if (trimmed.Equals("BHD", StringComparison.OrdinalIgnoreCase)) { result = BHD; return true; }
        if (trimmed.Equals("KWD", StringComparison.OrdinalIgnoreCase)) { result = KWD; return true; }

        var res = Create(trimmed.ToString());
        if (res.IsSuccess)
        {
            result = res.Value;
            return true;
        }

        result = default;
        return false;
    }
}

