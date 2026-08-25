// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Chile;

using EricksonLopez.ValueObjects.Attributes;

/// <summary>
/// Represents a Chilean Professional Service Fee Withholding Rate (Retención de Honorarios)
/// governed by the gradual increase established in Ley 21.133.
///
/// <para><b>Statutory Rate Schedule:</b>
/// <list type="bullet">
///   <item><description>2019: 10.00%</description></item>
///   <item><description>2020: 10.75%</description></item>
///   <item><description>2021: 11.50%</description></item>
///   <item><description>2022: 12.25%</description></item>
///   <item><description>2023: 13.00%</description></item>
///   <item><description>2024: 13.75%</description></item>
///   <item><description>2025: 14.50%</description></item>
///   <item><description>2026: 15.25%</description></item>
///   <item><description>2027: 16.00%</description></item>
///   <item><description>2028+: 17.00%</description></item>
/// </list>
/// </para>
/// </summary>
[RegulatoryRule("TAX.RATE")]
[ValueObject]
public readonly record struct WithholdingRate : ISpanParsable<WithholdingRate>, IComparable<WithholdingRate>
{
    private readonly decimal _percentage;

    private WithholdingRate(decimal percentage) => _percentage = percentage;

    /// <summary>
    /// Gets the percentage value (e.g. 13.75m).
    /// </summary>
    public decimal Percentage => _percentage;

    /// <summary>
    /// Gets the fractional rate for direct calculation (e.g. 0.1375m).
    /// </summary>
    public decimal Fraction => _percentage / 100m;

    /// <summary>
    /// Creates a validated <see cref="WithholdingRate"/> from a percentage rate.
    /// </summary>
    /// <param name="percentage">The statutory withholding percentage under Ley 21.133 (e.g. 10.75, 13.75).</param>
    /// <returns>A <see cref="Result{WithholdingRate}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<WithholdingRate> Create(decimal percentage)
    {
        return percentage switch
        {
            10.0m or 10.75m or 11.5m or 12.25m or 13.0m or 13.75m or 14.5m or 15.25m or 16.0m or 17.0m =>
                Result<WithholdingRate>.Success(new WithholdingRate(percentage)),
            _ => Result<WithholdingRate>.Failure(Error.Validation(
                "WithholdingRate.InvalidRate", $"The fee withholding rate '{percentage.ToString(CultureInfo.InvariantCulture)}%' does not match the statutory schedule of Law 21.133."))
        };
    }


    /// <summary>
    /// Gets the statutory withholding rate for a given calendar year under Ley 21.133.
    /// </summary>
    public static WithholdingRate ForYear(int year)
    {
        decimal rate = year switch
        {
            <= 2019 => 10.00m,
            2020 => 10.75m,
            2021 => 11.50m,
            2022 => 12.25m,
            2023 => 13.00m,
            2024 => 13.75m,
            2025 => 14.50m,
            2026 => 15.25m,
            2027 => 16.00m,
            _ => 17.00m
        };

        return new WithholdingRate(rate);
    }

    /// <summary>
    /// Creates a validated <see cref="WithholdingRate"/> from a text span.
    /// </summary>
    /// <param name="input">A character span containing the percentage value, optionally suffixed with <c>%</c>.</param>
    /// <returns>A <see cref="Result{WithholdingRate}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<WithholdingRate> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim().TrimEnd('%');
        if (!decimal.TryParse(trimmed, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal percentage))
        {
            return Result<WithholdingRate>.Failure(Error.Validation(
                "WithholdingRate.InvalidFormat", "The withholding rate must be a valid decimal number."));
        }

        return Create(percentage);
    }

    /// <summary>
    /// Creates a validated <see cref="WithholdingRate"/> from a nullable string.
    /// </summary>
    /// <param name="input">A string containing the percentage value, optionally suffixed with <c>%</c>.</param>
    /// <returns>A <see cref="Result{WithholdingRate}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<WithholdingRate> Create(string? input) =>
        Create(input.AsSpan());

    /// <inheritdoc/>
    public override string ToString() => $"{_percentage.ToString("0.##", CultureInfo.InvariantCulture)}%";

    /// <inheritdoc/>
    public int CompareTo(WithholdingRate other) => _percentage.CompareTo(other._percentage);

        /// <summary>
    /// Determines whether the left <see cref="WithholdingRate"/> is less than the right <see cref="WithholdingRate"/>.
    /// </summary>
    /// <param name="left">The first <see cref="WithholdingRate"/> to compare.</param>
    /// <param name="right">The second <see cref="WithholdingRate"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(WithholdingRate left, WithholdingRate right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="WithholdingRate"/> is less than or equal to the right <see cref="WithholdingRate"/>.
    /// </summary>
    /// <param name="left">The first <see cref="WithholdingRate"/> to compare.</param>
    /// <param name="right">The second <see cref="WithholdingRate"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(WithholdingRate left, WithholdingRate right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="WithholdingRate"/> is greater than the right <see cref="WithholdingRate"/>.
    /// </summary>
    /// <param name="left">The first <see cref="WithholdingRate"/> to compare.</param>
    /// <param name="right">The second <see cref="WithholdingRate"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(WithholdingRate left, WithholdingRate right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="WithholdingRate"/> is greater than or equal to the right <see cref="WithholdingRate"/>.
    /// </summary>
    /// <param name="left">The first <see cref="WithholdingRate"/> to compare.</param>
    /// <param name="right">The second <see cref="WithholdingRate"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(WithholdingRate left, WithholdingRate right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public static WithholdingRate Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid WithholdingRate: '{s}'.");

    /// <inheritdoc/>
    public static WithholdingRate Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid WithholdingRate: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out WithholdingRate result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out WithholdingRate result) =>
        TryParse(s.AsSpan(), provider, out result);
}




