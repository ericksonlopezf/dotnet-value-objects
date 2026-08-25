// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using System.Text;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Argentina;

using EricksonLopez.ValueObjects.Attributes;

/// <summary>
/// Represents an Argentine CUIT (Clave Única de Identificación Tributaria)
/// administered by ARCA (Agencia de Recaudación y Control Aduanero, formerly AFIP).
///
/// <para><b>Structure:</b> Exactly 11 numeric digits structured as <c>XY-XXXXXXXX-Z</c>:
/// <list type="bullet">
///   <item><term>XY (Prefix)</term><description>20, 23, 24, 27 (Physical persons) or 30, 33, 34 (Legal entities).</description></item>
///   <item><term>XXXXXXXX (Document)</term><description>8-digit national identity number or corporate registration.</description></item>
///   <item><term>Z (Check digit)</term><description>Modulo 11 verification digit computed with weights <c>[5, 4, 3, 2, 7, 6, 5, 4, 3, 2]</c>.</description></item>
/// </list>
/// </para>
/// </summary>
[RegulatoryRule("AR.CUIT.001")]
[ValueObject]
public readonly record struct Cuit : ISpanParsable<Cuit>, IUtf8SpanParsable<Cuit>, IComparable<Cuit>
{
    private readonly string _value;

    private Cuit(string value) => _value = value;


    /// <summary>
    /// Gets the raw 11-digit numeric value of the CUIT.
    /// </summary>
    public string Value => _value;

    /// <summary>
    /// Gets the 2-digit type prefix (e.g. 20, 27, 30).
    /// </summary>
    public int TypePrefix => int.Parse(_value.AsSpan(0, 2), CultureInfo.InvariantCulture);

    /// <summary>
    /// Gets the middle 8-digit document sequence.
    /// </summary>
    public string DocumentNumber => _value[2..10];

    /// <summary>
    /// Gets the verification check digit.
    /// </summary>
    public int VerificationDigit => _value[10] - '0';

    /// <summary>
    /// Gets a value indicating whether this CUIT belongs to a physical person (prefixes 20, 23, 24, 27).
    /// </summary>
    public bool IsIndividual => TypePrefix is 20 or 23 or 24 or 27;

    /// <summary>
    /// Gets a value indicating whether this CUIT belongs to a legal entity (prefixes 30, 33, 34).
    /// </summary>
    public bool IsCompany => TypePrefix is 30 or 33 or 34;

    /// <summary>
    /// Creates a validated <see cref="Cuit"/> from an 11-digit raw string or formatted <c>XX-XXXXXXXX-X</c>.
    /// </summary>
    /// <param name="value">The raw or formatted 11-digit CUIT string.</param>
    /// <returns>A <see cref="Result{Cuit}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<Cuit> Create(string? value) =>
        Create(value.AsSpan());

    /// <summary>
    /// Creates a validated <see cref="Cuit"/> from a character span.
    /// </summary>
    /// <param name="input">A character span containing the raw or formatted CUIT digits.</param>
    /// <returns>A <see cref="Result{Cuit}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<Cuit> Create(ReadOnlySpan<char> input)
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
                    return Result<Cuit>.Failure(Error.Validation(
                        "Cuit.InvalidLength", "The CUIT must contain exactly 11 numeric digits."));
                }
                buffer[count++] = c;
            }
            else if (c != '-' && c != '.')
            {
                return Result<Cuit>.Failure(Error.Validation(
                    "Cuit.InvalidCharacters", "The CUIT contains invalid characters."));
            }
        }

        if (count != 11)
        {
            return Result<Cuit>.Failure(Error.Validation(
                "Cuit.InvalidLength", "The CUIT must contain exactly 11 numeric digits."));
        }

        int prefix = (buffer[0] - '0') * 10 + (buffer[1] - '0');
        if (prefix is not (20 or 23 or 24 or 27 or 30 or 33 or 34))
        {
            return Result<Cuit>.Failure(Error.Validation(
                "Cuit.InvalidPrefix", $"The prefix '{prefix.ToString(CultureInfo.InvariantCulture)}' is not a recognized CUIT prefix for ARCA/AFIP."));
        }

        int computedDv = CalculateVerificationDigit(buffer[..10]);
        int providedDv = buffer[10] - '0';

        if (computedDv != providedDv)
        {
            return Result<Cuit>.Failure(Error.Validation(
                "Cuit.InvalidVerificationDigit",
                $"The verification check digit '{providedDv.ToString(CultureInfo.InvariantCulture)}' is invalid (expected: '{computedDv.ToString(CultureInfo.InvariantCulture)}')."));
        }

        return Result<Cuit>.Success(new Cuit(buffer.ToString()));
    }

    /// <summary>
    /// Calculates the Modulo 11 check digit for the first 10 digits of a CUIT.
    /// </summary>
    /// <param name="first10Digits">A 10-character span containing the first 10 CUIT digits.</param>
    /// <returns>The computed Modulo 11 verification digit (0 to 9).</returns>
    public static int CalculateVerificationDigit(ReadOnlySpan<char> first10Digits)
    {
        ReadOnlySpan<int> weights = [5, 4, 3, 2, 7, 6, 5, 4, 3, 2];
        int sum = 0;
        for (int i = 0; i < 10; i++)
        {
            sum += (first10Digits[i] - '0') * weights[i];
        }

        int remainder = sum % 11;
        return remainder switch
        {
            0 => 0,
            1 => 9, // Special case for collision fallback or 11 - remainder
            _ => 11 - remainder
        };
    }


    /// <summary>
    /// Formats the CUIT in its standard canonical ARCA format: <c>XX-XXXXXXXX-X</c>.
    /// </summary>
    public string Formatted => $"{_value[..2]}-{_value[2..10]}-{_value[10]}";

    /// <inheritdoc/>
    public override string ToString() => Formatted;

    /// <inheritdoc/>
    public int CompareTo(Cuit other) => string.Compare(_value, other._value, StringComparison.Ordinal);

        /// <summary>
    /// Determines whether the left <see cref="Cuit"/> is less than the right <see cref="Cuit"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Cuit"/> to compare.</param>
    /// <param name="right">The second <see cref="Cuit"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(Cuit left, Cuit right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="Cuit"/> is less than or equal to the right <see cref="Cuit"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Cuit"/> to compare.</param>
    /// <param name="right">The second <see cref="Cuit"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(Cuit left, Cuit right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="Cuit"/> is greater than the right <see cref="Cuit"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Cuit"/> to compare.</param>
    /// <param name="right">The second <see cref="Cuit"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(Cuit left, Cuit right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="Cuit"/> is greater than or equal to the right <see cref="Cuit"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Cuit"/> to compare.</param>
    /// <param name="right">The second <see cref="Cuit"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(Cuit left, Cuit right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public static Cuit Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid CUIT: '{s}'.");

    /// <inheritdoc/>
    public static Cuit Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid CUIT: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Cuit result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out Cuit result) =>
        TryParse(s.AsSpan(), provider, out result);

    /// <inheritdoc/>
    public static Cuit Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider = null) =>
        TryParse(utf8Text, provider, out var res) ? res : throw new FormatException("Invalid UTF-8 CUIT representation.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out Cuit result)
    {
        Span<char> chars = stackalloc char[utf8Text.Length];
        Encoding.UTF8.TryGetChars(utf8Text, chars, out int written);
        return TryParse(chars[..written], provider, out result);
    }
}





