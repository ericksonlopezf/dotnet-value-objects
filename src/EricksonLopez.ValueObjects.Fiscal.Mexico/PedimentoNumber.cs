// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Mexico;

/// <summary>
/// Represents a Mexican Customs Declaration Number (Número de Pedimento Aduanal),
/// governed by Anexo 22 de las Reglas Generales de Comercio Exterior (SAT).
///
/// <para><b>Structure:</b> Exactly 15 numeric digits structured as:
/// <list type="bullet">
///   <item><term>Validation Year (2 digits)</term><description>Last 2 digits of the validation year.</description></item>
///   <item><term>Customs Office (2 digits)</term><description>Code of the customs clearance office.</description></item>
///   <item><term>Customs Patent (4 digits)</term><description>Customs broker patent number.</description></item>
///   <item><term>Sequential (7 digits)</term><description>Sequential document progress number.</description></item>
/// </list>
/// Canonical string without spaces: <c>15 digits</c>. Formatted: <c>YY  AA  CCCC  NNNNNNN</c>.
/// </para>
/// </summary>
[ValueObject]
public readonly record struct PedimentoNumber : ISpanParsable<PedimentoNumber>, IComparable<PedimentoNumber>
{
    private readonly string _digits;

    private PedimentoNumber(string digits) => _digits = digits;

    /// <summary>
    /// Gets the raw 15-digit pedimento string.
    /// </summary>
    public string Digits => _digits;

    /// <summary>
    /// Gets the 2-digit validation year component.
    /// </summary>
    public string Year => _digits[..2];

    /// <summary>
    /// Gets the 2-digit customs office component.
    /// </summary>
    public string CustomsOffice => _digits[2..4];

    /// <summary>
    /// Gets the 4-digit customs patent agent component.
    /// </summary>
    public string Patent => _digits[4..8];

    /// <summary>
    /// Gets the 7-digit sequential progress component.
    /// </summary>
    public string Sequential => _digits[8..];

    /// <summary>
    /// Creates a validated <see cref="PedimentoNumber"/> from a 15-digit string (with or without spaces).
    /// </summary>
    /// <param name="value">A string containing the 15 pedimento digits, optionally separated by spaces.</param>
    /// <returns>A <see cref="Result{PedimentoNumber}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<PedimentoNumber> Create(string? value) =>
        Create(value.AsSpan());

    /// <summary>
    /// Creates a validated <see cref="PedimentoNumber"/> from a character span.
    /// </summary>
    /// <param name="input">A character span containing the 15 pedimento digits, optionally separated by spaces.</param>
    /// <returns>A <see cref="Result{PedimentoNumber}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<PedimentoNumber> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();
        Span<char> buffer = stackalloc char[15];
        int count = 0;

        foreach (char c in trimmed)
        {
            if (char.IsDigit(c))
            {
                if (count >= 15)
                {
                    return Result<PedimentoNumber>.Failure(Error.Validation(
                        "PedimentoNumber.InvalidLength", "The pedimento number must contain exactly 15 numeric digits."));
                }
                buffer[count++] = c;
            }
            else if (!char.IsWhiteSpace(c))
            {
                return Result<PedimentoNumber>.Failure(Error.Validation(
                    "PedimentoNumber.InvalidCharacters", "The pedimento number contains disallowed characters."));
            }
        }

        if (count != 15)
        {
            return Result<PedimentoNumber>.Failure(Error.Validation(
                "PedimentoNumber.InvalidLength", "The pedimento number must contain exactly 15 numeric digits."));
        }

        return Result<PedimentoNumber>.Success(new PedimentoNumber(buffer.ToString()));
    }

    /// <summary>
    /// Formats the pedimento in the official SAT representation with double spaces: <c>YY  AA  CCCC  NNNNNNN</c>.
    /// </summary>
    public string Formatted => $"{Year}  {CustomsOffice}  {Patent}  {Sequential}";

    /// <inheritdoc/>
    public override string ToString() => Formatted;

    /// <inheritdoc/>
    public int CompareTo(PedimentoNumber other) => string.Compare(_digits, other._digits, StringComparison.Ordinal);

        /// <summary>
    /// Determines whether the left <see cref="PedimentoNumber"/> is less than the right <see cref="PedimentoNumber"/>.
    /// </summary>
    /// <param name="left">The first <see cref="PedimentoNumber"/> to compare.</param>
    /// <param name="right">The second <see cref="PedimentoNumber"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(PedimentoNumber left, PedimentoNumber right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="PedimentoNumber"/> is less than or equal to the right <see cref="PedimentoNumber"/>.
    /// </summary>
    /// <param name="left">The first <see cref="PedimentoNumber"/> to compare.</param>
    /// <param name="right">The second <see cref="PedimentoNumber"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(PedimentoNumber left, PedimentoNumber right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="PedimentoNumber"/> is greater than the right <see cref="PedimentoNumber"/>.
    /// </summary>
    /// <param name="left">The first <see cref="PedimentoNumber"/> to compare.</param>
    /// <param name="right">The second <see cref="PedimentoNumber"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(PedimentoNumber left, PedimentoNumber right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="PedimentoNumber"/> is greater than or equal to the right <see cref="PedimentoNumber"/>.
    /// </summary>
    /// <param name="left">The first <see cref="PedimentoNumber"/> to compare.</param>
    /// <param name="right">The second <see cref="PedimentoNumber"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(PedimentoNumber left, PedimentoNumber right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public static PedimentoNumber Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid PedimentoNumber: '{s}'.");

    /// <inheritdoc/>
    public static PedimentoNumber Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid PedimentoNumber: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out PedimentoNumber result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out PedimentoNumber result) =>
        TryParse(s.AsSpan(), provider, out result);
}



