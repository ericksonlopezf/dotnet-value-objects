// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Peru;

/// <summary>
/// Represents a Peruvian Tax Period (Período Tributario SIRE / PLE / Declara Fácil SUNAT).
///
/// <para><b>Format:</b> Exactly 6 numeric digits <c>YYYYMM</c> (e.g. <c>"202608"</c> for August 2026).</para>
/// </summary>
[ValueObject]
public readonly record struct TaxPeriod : ISpanParsable<TaxPeriod>, IComparable<TaxPeriod>
{
    private readonly int _year;
    private readonly byte _month;

    private TaxPeriod(int year, byte month)
    {
        _year = year;
        _month = month;
    }

    /// <summary>
    /// Gets the 4-digit tax year.
    /// </summary>
    public int Year => _year;

    /// <summary>
    /// Gets the tax month (1 to 12).
    /// </summary>
    public byte Month => _month;

    /// <summary>
    /// Creates a validated <see cref="TaxPeriod"/> from year and month components.
    /// </summary>
    public static Result<TaxPeriod> Create(int year, int month)
    {
        if (year is < 2000 or > 2100)
        {
            return Result<TaxPeriod>.Failure(Error.Validation(
                "TaxPeriod.InvalidYear", $"The tax year ({year.ToString(CultureInfo.InvariantCulture)}) must be between 2000 and 2100."));
        }

        if (month is < 1 or > 12)
        {
            return Result<TaxPeriod>.Failure(Error.Validation(
                "TaxPeriod.InvalidMonth", $"The tax month ({month.ToString(CultureInfo.InvariantCulture)}) must be between 1 and 12."));
        }

        return Result<TaxPeriod>.Success(new TaxPeriod(year, (byte)month));
    }

    /// <summary>
    /// Creates a validated <see cref="TaxPeriod"/> from a 6-digit <c>YYYYMM</c> string.
    /// </summary>
    public static Result<TaxPeriod> Create(string? value) =>
        Create(value.AsSpan());

    /// <summary>
    /// Creates a validated <see cref="TaxPeriod"/> from a character span.
    /// </summary>
    public static Result<TaxPeriod> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();
        if (trimmed.Length != 6)
        {
            return Result<TaxPeriod>.Failure(Error.Validation(
                "TaxPeriod.InvalidLength", "The tax period must have exactly 6 digits (YYYYMM format)."));
        }

        if (!int.TryParse(trimmed[..4], CultureInfo.InvariantCulture, out int year) ||
            !int.TryParse(trimmed[4..], CultureInfo.InvariantCulture, out int month))
        {
            return Result<TaxPeriod>.Failure(Error.Validation(
                "TaxPeriod.InvalidFormat", "The tax period must contain only numeric digits."));
        }

        return Create(year, month);
    }

    /// <summary>
    /// Formats the period in standard <c>YYYYMM</c> representation.
    /// </summary>
    public string Formatted => $"{_year.ToString(CultureInfo.InvariantCulture)}{_month.ToString("D2", CultureInfo.InvariantCulture)}";

    /// <inheritdoc/>
    public override string ToString() => Formatted;

    /// <inheritdoc/>
    public int CompareTo(TaxPeriod other)
    {
        int yearComp = _year.CompareTo(other._year);
        return yearComp != 0 ? yearComp : _month.CompareTo(other._month);
    }

        /// <summary>
    /// Determines whether the left <see cref="TaxPeriod"/> is less than the right <see cref="TaxPeriod"/>.
    /// </summary>
    /// <param name="left">The first <see cref="TaxPeriod"/> to compare.</param>
    /// <param name="right">The second <see cref="TaxPeriod"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(TaxPeriod left, TaxPeriod right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="TaxPeriod"/> is less than or equal to the right <see cref="TaxPeriod"/>.
    /// </summary>
    /// <param name="left">The first <see cref="TaxPeriod"/> to compare.</param>
    /// <param name="right">The second <see cref="TaxPeriod"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(TaxPeriod left, TaxPeriod right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="TaxPeriod"/> is greater than the right <see cref="TaxPeriod"/>.
    /// </summary>
    /// <param name="left">The first <see cref="TaxPeriod"/> to compare.</param>
    /// <param name="right">The second <see cref="TaxPeriod"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(TaxPeriod left, TaxPeriod right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="TaxPeriod"/> is greater than or equal to the right <see cref="TaxPeriod"/>.
    /// </summary>
    /// <param name="left">The first <see cref="TaxPeriod"/> to compare.</param>
    /// <param name="right">The second <see cref="TaxPeriod"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(TaxPeriod left, TaxPeriod right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public static TaxPeriod Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid TaxPeriod: '{s}'.");

    /// <inheritdoc/>
    public static TaxPeriod Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid TaxPeriod: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out TaxPeriod result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out TaxPeriod result) =>
        TryParse(s.AsSpan(), provider, out result);
}




