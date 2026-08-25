// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using System.Text;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Peru;

using EricksonLopez.ValueObjects.Attributes;

/// <summary>
/// Represents a Peruvian RUC (Registro Único de Contribuyentes)
/// administered by SUNAT (Superintendencia Nacional de Aduanas y de Administración Tributaria).
///
/// <para><b>Structure:</b> Exactly 11 numeric digits:
/// <list type="bullet">
///   <item><term>Prefix (2 digits)</term><description>10 (Natural person with DNI), 15 (Natural person with CE), 17 (Natural person with Passport), 20 (Legal entity).</description></item>
///   <item><term>Middle (8 digits)</term><description>Identity document sequence or company registration sequence.</description></item>
///   <item><term>Check Digit (1 digit)</term><description>Modulo 11 verification digit weighted by <c>[5, 4, 3, 2, 7, 6, 5, 4, 3, 2]</c>.</description></item>
/// </list>
/// </para>
/// </summary>
[RegulatoryRule("PE.RUC.001")]
[ValueObject]
public readonly record struct Ruc : ISpanParsable<Ruc>, IUtf8SpanParsable<Ruc>, IComparable<Ruc>
{
    private readonly string _value;

    private Ruc(string value) => _value = value;

    /// <summary>
    /// Gets the raw 11-digit numeric RUC string.
    /// </summary>
    public string Value => _value;

    /// <summary>
    /// Gets the 2-digit prefix (10, 15, 17, 20).
    /// </summary>
    public int Prefix => int.Parse(_value.AsSpan(0, 2), CultureInfo.InvariantCulture);

    /// <summary>
    /// Gets a value indicating whether this RUC belongs to a natural person (prefixes 10, 15, 17).
    /// </summary>
    public bool IsNaturalPerson => Prefix is 10 or 15 or 17;

    /// <summary>
    /// Gets a value indicating whether this RUC belongs to a legal entity (prefix 20).
    /// </summary>
    public bool IsLegalEntity => Prefix == 20;

    /// <summary>
    /// Creates a validated <see cref="Ruc"/> from an 11-digit string.
    /// </summary>
    public static Result<Ruc> Create(string? value) =>
        Create(value.AsSpan());

    /// <summary>
    /// Creates a validated <see cref="Ruc"/> from a character span.
    /// </summary>
    public static Result<Ruc> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();
        if (trimmed.Length != 11)
        {
            return Result<Ruc>.Failure(Error.Validation(
                "Ruc.InvalidLength", "The RUC must contain exactly 11 numeric digits."));
        }

        foreach (char c in trimmed)
        {
            if (!char.IsDigit(c))
            {
                return Result<Ruc>.Failure(Error.Validation(
                    "Ruc.InvalidCharacters", "The RUC must only contain numeric digits."));
            }
        }

        int prefix = (trimmed[0] - '0') * 10 + (trimmed[1] - '0');
        if (prefix is not (10 or 15 or 17 or 20))
        {
            return Result<Ruc>.Failure(Error.Validation(
                "Ruc.InvalidPrefix", $"The prefix '{prefix.ToString(CultureInfo.InvariantCulture)}' is invalid for RUC (allowed: 10, 15, 17, 20)."));
        }

        int computedDv = CalculateVerificationDigit(trimmed[..10]);
        int providedDv = trimmed[10] - '0';

        if (computedDv != providedDv)
        {
            return Result<Ruc>.Failure(Error.Validation(
                "Ruc.InvalidVerificationDigit",
                $"The verification check digit '{providedDv.ToString(CultureInfo.InvariantCulture)}' is invalid (expected: '{computedDv.ToString(CultureInfo.InvariantCulture)}')."));
        }

        return Result<Ruc>.Success(new Ruc(trimmed.ToString()));
    }

    /// <summary>
    /// Computes the SUNAT Modulo 11 check digit for the first 10 digits of a RUC.
    /// </summary>
    public static int CalculateVerificationDigit(ReadOnlySpan<char> first10Digits)
    {
        ReadOnlySpan<int> weights = [5, 4, 3, 2, 7, 6, 5, 4, 3, 2];
        int sum = 0;
        for (int i = 0; i < 10; i++)
        {
            sum += (first10Digits[i] - '0') * weights[i];
        }

        int remainder = sum % 11;
        int check = 11 - remainder;

        return check switch
        {
            10 => 0,
            11 => 1,
            _ => check
        };
    }

    /// <inheritdoc/>
    public override string ToString() => _value;

    /// <inheritdoc/>
    public int CompareTo(Ruc other) => string.Compare(_value, other._value, StringComparison.Ordinal);

        /// <summary>
    /// Determines whether the left <see cref="Ruc"/> is less than the right <see cref="Ruc"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Ruc"/> to compare.</param>
    /// <param name="right">The second <see cref="Ruc"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(Ruc left, Ruc right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="Ruc"/> is less than or equal to the right <see cref="Ruc"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Ruc"/> to compare.</param>
    /// <param name="right">The second <see cref="Ruc"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(Ruc left, Ruc right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="Ruc"/> is greater than the right <see cref="Ruc"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Ruc"/> to compare.</param>
    /// <param name="right">The second <see cref="Ruc"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(Ruc left, Ruc right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="Ruc"/> is greater than or equal to the right <see cref="Ruc"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Ruc"/> to compare.</param>
    /// <param name="right">The second <see cref="Ruc"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(Ruc left, Ruc right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public static Ruc Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid RUC: '{s}'.");

    /// <inheritdoc/>
    public static Ruc Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid RUC: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Ruc result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out Ruc result) =>
        TryParse(s.AsSpan(), provider, out result);

    /// <inheritdoc/>
    public static Ruc Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider = null) =>
        TryParse(utf8Text, provider, out var res) ? res : throw new FormatException("Invalid UTF-8 RUC representation.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out Ruc result)
    {
        Span<char> chars = stackalloc char[utf8Text.Length];
        Encoding.UTF8.TryGetChars(utf8Text, chars, out int written);
        return TryParse(chars[..written], provider, out result);
    }
}






