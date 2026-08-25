// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Argentina;

/// <summary>
/// Represents a statutory Argentine VAT Rate (Alícuota de IVA)
/// under the Value Added Tax Law (Ley de Impuesto al Valor Agregado N° 23.349, Art. 28).
///
/// <para><b>Legal Rates:</b>
/// <list type="bullet">
///   <item><term>0%</term><description>Exento / No alcanzado</description></item>
///   <item><term>2.5%</term><description>Alícuota diferencial bienes de capital</description></item>
///   <item><term>5%</term><description>Alícuota reducida especial</description></item>
///   <item><term>10.5%</term><description>Alícuota reducida (alimentos, obras de arte, capital)</description></item>
///   <item><term>21%</term><description>Alícuota general</description></item>
///   <item><term>27%</term><description>Alícuota incrementada (servicios públicos, telecomunicaciones, energía)</description></item>
/// </list>
/// </para>
/// </summary>
[ValueObject]
public readonly record struct VatRate : ISpanParsable<VatRate>, IComparable<VatRate>
{
    /// <summary>Gets the 0% exempt VAT rate.</summary>
    public static VatRate Zero => new(0m, "0% - Exento / No Gravado");

    /// <summary>Gets the 2.5% differential VAT rate.</summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Represents official 2.5% statutory VAT rate in Argentina")]
    public static VatRate Rate2_5 => new(2.5m, "2.5% - Diferencial");

    /// <summary>Gets the 5% special reduced VAT rate.</summary>
    public static VatRate Rate5 => new(5m, "5% - Reducida especial");

    /// <summary>Gets the 10.5% reduced VAT rate.</summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Represents official 10.5% statutory VAT rate in Argentina")]
    public static VatRate Rate10_5 => new(10.5m, "10.5% - Reducida");

    /// <summary>Gets the 21% standard general VAT rate.</summary>
    public static VatRate Rate21 => new(21m, "21% - General");

    /// <summary>Gets the 27% increased VAT rate for utilities.</summary>
    public static VatRate Rate27 => new(27m, "27% - Incrementada");


    private readonly decimal _percentage;
    private readonly string _description;

    private VatRate(decimal percentage, string description)
    {
        _percentage = percentage;
        _description = description;
    }

    /// <summary>
    /// Gets the decimal percentage value (e.g. 21m).
    /// </summary>
    public decimal Percentage => _percentage;

    /// <summary>
    /// Gets the fractional rate for direct calculation (e.g. 0.21m).
    /// </summary>
    public decimal Fraction => _percentage / 100m;

    /// <summary>
    /// Gets the official rate description.
    /// </summary>
    public string Description => _description;

    /// <summary>
    /// Creates a validated <see cref="VatRate"/> from a numeric percentage.
    /// </summary>
    /// <param name="percentage">The statutory VAT percentage (0, 2.5, 5, 10.5, 21, or 27).</param>
    /// <returns>A <see cref="Result{VatRate}"/> containing the matched statutory rate or a validation error.</returns>
    public static Result<VatRate> Create(decimal percentage)
    {
        return percentage switch
        {
            0m => Result<VatRate>.Success(Zero),
            2.5m => Result<VatRate>.Success(Rate2_5),
            5m => Result<VatRate>.Success(Rate5),
            10.5m => Result<VatRate>.Success(Rate10_5),
            21m => Result<VatRate>.Success(Rate21),
            27m => Result<VatRate>.Success(Rate27),
            _ => Result<VatRate>.Failure(Error.Validation(
                "VatRate.InvalidRate", $"The VAT rate '{percentage.ToString(CultureInfo.InvariantCulture)}%' is not a valid statutory rate in Argentina."))
        };
    }

    /// <summary>
    /// Creates a validated <see cref="VatRate"/> from a text span.
    /// </summary>
    /// <param name="input">A character span containing the percentage value, optionally suffixed with <c>%</c>.</param>
    /// <returns>A <see cref="Result{VatRate}"/> containing the matched statutory rate or a validation error.</returns>
    public static Result<VatRate> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim().TrimEnd('%');
        if (!decimal.TryParse(trimmed, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal percentage))
        {
            return Result<VatRate>.Failure(Error.Validation(
                "VatRate.InvalidFormat", "The VAT rate format must be numeric."));
        }

        return Create(percentage);
    }

    /// <summary>
    /// Creates a validated <see cref="VatRate"/> from a nullable string.
    /// </summary>
    /// <param name="input">A string containing the percentage value, optionally suffixed with <c>%</c>.</param>
    /// <returns>A <see cref="Result{VatRate}"/> containing the matched statutory rate or a validation error.</returns>
    public static Result<VatRate> Create(string? input) =>
        Create(input.AsSpan());

    /// <inheritdoc/>
    public override string ToString() => $"{_percentage.ToString(CultureInfo.InvariantCulture)}%";

    /// <inheritdoc/>
    public int CompareTo(VatRate other) => _percentage.CompareTo(other._percentage);

        /// <summary>
    /// Determines whether the left <see cref="VatRate"/> is less than the right <see cref="VatRate"/>.
    /// </summary>
    /// <param name="left">The first <see cref="VatRate"/> to compare.</param>
    /// <param name="right">The second <see cref="VatRate"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(VatRate left, VatRate right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="VatRate"/> is less than or equal to the right <see cref="VatRate"/>.
    /// </summary>
    /// <param name="left">The first <see cref="VatRate"/> to compare.</param>
    /// <param name="right">The second <see cref="VatRate"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(VatRate left, VatRate right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="VatRate"/> is greater than the right <see cref="VatRate"/>.
    /// </summary>
    /// <param name="left">The first <see cref="VatRate"/> to compare.</param>
    /// <param name="right">The second <see cref="VatRate"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(VatRate left, VatRate right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="VatRate"/> is greater than or equal to the right <see cref="VatRate"/>.
    /// </summary>
    /// <param name="left">The first <see cref="VatRate"/> to compare.</param>
    /// <param name="right">The second <see cref="VatRate"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(VatRate left, VatRate right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public static VatRate Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid VatRate: '{s}'.");

    /// <inheritdoc/>
    public static VatRate Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid VatRate: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out VatRate result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out VatRate result) =>
        TryParse(s.AsSpan(), provider, out result);
}




