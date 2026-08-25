// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Chile;

/// <summary>
/// Represents a Chilean statutory VAT Rate (Tasa de Impuesto al Valor Agregado - IVA)
/// under Decreto Ley N° 825 de 1974 (Ley sobre Impuesto a las Ventas y Servicios).
///
/// <para><b>Legal Rates:</b>
/// <list type="bullet">
///   <item><term>19%</term><description>Tasa General de IVA (Art. 14 DL 825)</description></item>
///   <item><term>0%</term><description>Operaciones Exentas o No Gravadas</description></item>
/// </list>
/// </para>
/// </summary>
[ValueObject]
public readonly record struct TaxRateVat : ISpanParsable<TaxRateVat>, IComparable<TaxRateVat>
{
    /// <summary>Gets the 19% general VAT rate in Chile.</summary>
    public static TaxRateVat General19 => new(19m, "19% - Tasa General IVA");
    /// <summary>Gets the 0% exempt VAT rate in Chile.</summary>
    public static TaxRateVat Exempt0 => new(0m, "0% - Exento / No Gravado");


    private readonly decimal _percentage;
    private readonly string _description;

    private TaxRateVat(decimal percentage, string description)
    {
        _percentage = percentage;
        _description = description;
    }

    /// <summary>
    /// Gets the numeric percentage (19m or 0m).
    /// </summary>
    public decimal Percentage => _percentage;

    /// <summary>
    /// Gets the fractional rate (0.19m or 0.00m).
    /// </summary>
    public decimal Fraction => _percentage / 100m;

    /// <summary>
    /// Gets the official description.
    /// </summary>
    public string Description => _description;

    /// <summary>
    /// Creates a validated <see cref="TaxRateVat"/> from a percentage rate (must be 19 or 0).
    /// </summary>
    /// <param name="percentage">The statutory VAT percentage (19 for general rate or 0 for exempt).</param>
    /// <returns>A <see cref="Result{TaxRateVat}"/> containing the matched statutory rate or a domain validation error.</returns>
    public static Result<TaxRateVat> Create(decimal percentage)
    {
        return percentage switch
        {
            19m => Result<TaxRateVat>.Success(General19),
            0m => Result<TaxRateVat>.Success(Exempt0),
            _ => Result<TaxRateVat>.Failure(Error.Validation(
                "TaxRateVat.InvalidRate", $"The VAT rate '{percentage.ToString(CultureInfo.InvariantCulture)}%' is invalid in Chile (only 19% general or 0% exempt is allowed)."))
        };
    }

    /// <summary>
    /// Creates a validated <see cref="TaxRateVat"/> from a text span.
    /// </summary>
    /// <param name="input">A character span containing the percentage value, optionally suffixed with <c>%</c>.</param>
    /// <returns>A <see cref="Result{TaxRateVat}"/> containing the matched statutory rate or a domain validation error.</returns>
    public static Result<TaxRateVat> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim().TrimEnd('%');
        if (!decimal.TryParse(trimmed, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal percentage))
        {
            return Result<TaxRateVat>.Failure(Error.Validation(
                "TaxRateVat.InvalidFormat", "The VAT rate must be a valid decimal number."));
        }

        return Create(percentage);
    }

    /// <summary>
    /// Creates a validated <see cref="TaxRateVat"/> from a nullable string.
    /// </summary>
    /// <param name="input">A string containing the percentage value, optionally suffixed with <c>%</c>.</param>
    /// <returns>A <see cref="Result{TaxRateVat}"/> containing the matched statutory rate or a domain validation error.</returns>
    public static Result<TaxRateVat> Create(string? input) =>
        Create(input.AsSpan());

    /// <inheritdoc/>
    public override string ToString() => $"{_percentage.ToString(CultureInfo.InvariantCulture)}%";

    /// <inheritdoc/>
    public int CompareTo(TaxRateVat other) => _percentage.CompareTo(other._percentage);

        /// <summary>
    /// Determines whether the left <see cref="TaxRateVat"/> is less than the right <see cref="TaxRateVat"/>.
    /// </summary>
    /// <param name="left">The first <see cref="TaxRateVat"/> to compare.</param>
    /// <param name="right">The second <see cref="TaxRateVat"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(TaxRateVat left, TaxRateVat right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="TaxRateVat"/> is less than or equal to the right <see cref="TaxRateVat"/>.
    /// </summary>
    /// <param name="left">The first <see cref="TaxRateVat"/> to compare.</param>
    /// <param name="right">The second <see cref="TaxRateVat"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(TaxRateVat left, TaxRateVat right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="TaxRateVat"/> is greater than the right <see cref="TaxRateVat"/>.
    /// </summary>
    /// <param name="left">The first <see cref="TaxRateVat"/> to compare.</param>
    /// <param name="right">The second <see cref="TaxRateVat"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(TaxRateVat left, TaxRateVat right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="TaxRateVat"/> is greater than or equal to the right <see cref="TaxRateVat"/>.
    /// </summary>
    /// <param name="left">The first <see cref="TaxRateVat"/> to compare.</param>
    /// <param name="right">The second <see cref="TaxRateVat"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(TaxRateVat left, TaxRateVat right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public static TaxRateVat Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid TaxRateVat: '{s}'.");

    /// <inheritdoc/>
    public static TaxRateVat Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid TaxRateVat: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out TaxRateVat result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out TaxRateVat result) =>
        TryParse(s.AsSpan(), provider, out result);
}




