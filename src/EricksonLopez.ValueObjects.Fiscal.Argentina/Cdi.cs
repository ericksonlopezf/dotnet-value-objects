// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Argentina;

/// <summary>
/// Represents an Argentine CDI (Clave de Identificación)
/// issued by ARCA/AFIP to individuals or entities who do not have a CUIT or CUIL (e.g. non-residents opening bank accounts or acquiring property).
/// </summary>
[ValueObject]
public readonly record struct Cdi : ISpanParsable<Cdi>, IComparable<Cdi>
{
    private readonly string _value;

    private Cdi(string value) => _value = value;

    /// <summary>
    /// Gets the raw 11-digit numeric value of the CDI.
    /// </summary>
    public string Value => _value;

    /// <summary>
    /// Creates a validated <see cref="Cdi"/> from an 11-digit raw string or formatted <c>XX-XXXXXXXX-X</c>.
    /// </summary>
    /// <param name="value">The raw or formatted 11-digit CDI string.</param>
    /// <returns>A <see cref="Result{Cdi}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<Cdi> Create(string? value) =>
        Create(value.AsSpan());

    /// <summary>
    /// Creates a validated <see cref="Cdi"/> from a character span.
    /// </summary>
    /// <param name="input">A character span containing the raw or formatted CDI digits.</param>
    /// <returns>A <see cref="Result{Cdi}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<Cdi> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();
        Span<char> buffer = stackalloc char[11];
        int count = 0;

        foreach (char c in trimmed)
        {
            if (char.IsDigit(c))
            {
                if (count >= 11)
                {
                    return Result<Cdi>.Failure(Error.Validation(
                        "Cdi.InvalidLength", "The CDI must contain exactly 11 numeric digits."));
                }
                buffer[count++] = c;
            }
            else if (c != '-' && c != '.')
            {
                return Result<Cdi>.Failure(Error.Validation(
                    "Cdi.InvalidCharacters", "The CDI contains invalid characters."));
            }
        }

        if (count != 11)
        {
            return Result<Cdi>.Failure(Error.Validation(
                "Cdi.InvalidLength", "The CDI must contain exactly 11 numeric digits."));
        }

        return Result<Cdi>.Success(new Cdi(buffer.ToString()));
    }

    /// <summary>
    /// Formats the CDI in standard format: <c>XX-XXXXXXXX-X</c>.
    /// </summary>
    public string Formatted => $"{_value[..2]}-{_value[2..10]}-{_value[10]}";

    /// <inheritdoc/>
    public override string ToString() => Formatted;

    /// <inheritdoc/>
    public int CompareTo(Cdi other) => string.Compare(_value, other._value, StringComparison.Ordinal);

        /// <summary>
    /// Determines whether the left <see cref="Cdi"/> is less than the right <see cref="Cdi"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Cdi"/> to compare.</param>
    /// <param name="right">The second <see cref="Cdi"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(Cdi left, Cdi right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="Cdi"/> is less than or equal to the right <see cref="Cdi"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Cdi"/> to compare.</param>
    /// <param name="right">The second <see cref="Cdi"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(Cdi left, Cdi right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="Cdi"/> is greater than the right <see cref="Cdi"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Cdi"/> to compare.</param>
    /// <param name="right">The second <see cref="Cdi"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(Cdi left, Cdi right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="Cdi"/> is greater than or equal to the right <see cref="Cdi"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Cdi"/> to compare.</param>
    /// <param name="right">The second <see cref="Cdi"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(Cdi left, Cdi right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public static Cdi Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid CDI: '{s}'.");

    /// <inheritdoc/>
    public static Cdi Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid CDI: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Cdi result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out Cdi result) =>
        TryParse(s.AsSpan(), provider, out result);
}



