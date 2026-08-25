// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using System.Text;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Colombia;

using EricksonLopez.ValueObjects.Attributes;

/// <summary>
/// Represents a Colombian Tax Identification Number (NIT - Número de Identificación Tributaria)
/// issued by the DIAN (Dirección de Impuestos y Aduanas Nacionales).
///
/// <para><b>Structure:</b> A base numeric sequence of 7 to 15 digits and an integrated Verification Digit (DV)
/// calculated using the official DIAN Modulo 11 weighted algorithm (Orden Administrativa 004/1989).</para>
///
/// <para><b>Rules:</b>
/// <list type="bullet">
///   <item><description>Base number must be between 1,000,000 and 999,999,999,999,999.</description></item>
///   <item><description>Verification digit is derived deterministically from the 15 prime factors: <c>[71, 67, 59, 53, 47, 43, 41, 37, 29, 23, 19, 17, 13, 7, 3]</c>.</description></item>
///   <item><description>Canonical format: <c>{BaseNumber}-{DV}</c> (e.g. <c>830099999-1</c>).</description></item>
/// </list>
/// </para>
/// </summary>
[RegulatoryRule("CO.NIT.001")]
[ValueObject]
public readonly record struct Nit : ISpanParsable<Nit>, IUtf8SpanParsable<Nit>, IComparable<Nit>
{
    private readonly long _baseNumber;
    private readonly byte _verificationDigit;

    private Nit(long baseNumber, byte verificationDigit)
    {
        _baseNumber = baseNumber;
        _verificationDigit = verificationDigit;
    }

    /// <summary>
    /// Gets the base numeric component of the NIT (excluding the verification digit).
    /// </summary>
    public long BaseNumber => _baseNumber;

    /// <summary>
    /// Gets the computed DIAN Modulo 11 verification digit (DV, between 0 and 9).
    /// </summary>
    public byte VerificationDigit => _verificationDigit;

    /// <summary>
    /// Creates a validated <see cref="Nit"/> instance from a numeric base value, computing its verification digit.
    /// </summary>
    /// <param name="baseNumber">The base numeric NIT value.</param>
    /// <returns>A <see cref="Result{T}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<Nit> Create(long baseNumber)
    {
        if (baseNumber is < 1_000_000L or > 999_999_999_999_999L)
        {
            return Result<Nit>.Failure(Error.Validation(
                "Nit.OutOfRange", "The NIT must contain between 7 and 15 numeric digits."));
        }

        byte dv = CalculateVerificationDigit(baseNumber);
        return Result<Nit>.Success(new Nit(baseNumber, dv));
    }

    /// <summary>
    /// Creates a validated <see cref="Nit"/> instance from a text representation (e.g. <c>"830099999-1"</c> or <c>"830099999"</c>).
    /// </summary>
    /// <param name="input">The text containing the NIT.</param>
    /// <returns>A <see cref="Result{T}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<Nit> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();
        if (trimmed.IsEmpty)
        {
            return Result<Nit>.Failure(Error.Validation(
                "Nit.Required", "The NIT is required."));
        }

        int hyphenIndex = trimmed.IndexOf('-');
        ReadOnlySpan<char> baseSpan = hyphenIndex >= 0 ? trimmed[..hyphenIndex] : trimmed;

        if (!long.TryParse(baseSpan, CultureInfo.InvariantCulture, out long baseNumber))
        {
            return Result<Nit>.Failure(Error.Validation(
                "Nit.InvalidCharacters", "The base NIT must contain only numeric digits."));
        }

        if (baseNumber is < 1_000_000L or > 999_999_999_999_999L)
        {
            return Result<Nit>.Failure(Error.Validation(
                "Nit.OutOfRange", "The NIT must contain between 7 and 15 numeric digits."));
        }

        byte computedDv = CalculateVerificationDigit(baseNumber);

        if (hyphenIndex != -1)
        {
            ReadOnlySpan<char> dvSpan = trimmed[(hyphenIndex + 1)..];
            if (dvSpan.Length != 1 || !byte.TryParse(dvSpan, CultureInfo.InvariantCulture, out byte providedDv) || providedDv != computedDv)
            {
                return Result<Nit>.Failure(Error.Validation(
                    "Nit.InvalidVerificationDigit", $"The verification digit '{dvSpan}' is invalid (expected: '{computedDv}')."));
            }
        }

        return Result<Nit>.Success(new Nit(baseNumber, computedDv));

    }

    /// <summary>
    /// Creates a validated <see cref="Nit"/> instance from a nullable string.
    /// </summary>
    /// <param name="input">A string containing the NIT in raw or formatted (<c>NNNNNNNNN-D</c>) form.</param>
    /// <returns>A <see cref="Result{Nit}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<Nit> Create(string? input) =>
        Create(input.AsSpan());

    /// <summary>
    /// Calculates the official DIAN Modulo 11 verification digit for a given base number.
    /// </summary>
    /// <param name="baseNumber">The base numeric NIT value.</param>
    /// <returns>The verification digit (0 to 9).</returns>
    public static byte CalculateVerificationDigit(long baseNumber)
    {
        ReadOnlySpan<int> primeWeights = [71, 67, 59, 53, 47, 43, 41, 37, 29, 23, 19, 17, 13, 7, 3];
        Span<char> buffer = stackalloc char[15];
        baseNumber.TryFormat(buffer, out int written, provider: CultureInfo.InvariantCulture);

        int padCount = 15 - written;
        int totalSum = 0;

        for (int i = 0; i < written; i++)
        {
            int digit = buffer[i] - '0';
            int weightIndex = padCount + i;
            totalSum += digit * primeWeights[weightIndex];
        }

        int remainder = totalSum % 11;
        return remainder switch
        {
            0 => 0,
            1 => 1,
            _ => (byte)(11 - remainder)
        };
    }

    /// <summary>
    /// Formats the NIT in its canonical DIAN representation: <c>{BaseNumber}-{DV}</c>.
    /// </summary>
    public string ToCanonicalString() => $"{_baseNumber.ToString(CultureInfo.InvariantCulture)}-{_verificationDigit.ToString(CultureInfo.InvariantCulture)}";

    /// <inheritdoc/>
    public override string ToString() => ToCanonicalString();

    /// <inheritdoc/>
    public int CompareTo(Nit other) => _baseNumber.CompareTo(other._baseNumber);

        /// <summary>
    /// Determines whether the left <see cref="Nit"/> is less than the right <see cref="Nit"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Nit"/> to compare.</param>
    /// <param name="right">The second <see cref="Nit"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(Nit left, Nit right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="Nit"/> is less than or equal to the right <see cref="Nit"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Nit"/> to compare.</param>
    /// <param name="right">The second <see cref="Nit"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(Nit left, Nit right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="Nit"/> is greater than the right <see cref="Nit"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Nit"/> to compare.</param>
    /// <param name="right">The second <see cref="Nit"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(Nit left, Nit right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="Nit"/> is greater than or equal to the right <see cref="Nit"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Nit"/> to compare.</param>
    /// <param name="right">The second <see cref="Nit"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(Nit left, Nit right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public static Nit Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid NIT: '{s}'.");

    /// <inheritdoc/>
    public static Nit Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid NIT: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Nit result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out Nit result) =>
        TryParse(s.AsSpan(), provider, out result);

    /// <inheritdoc/>
    public static Nit Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider = null) =>
        TryParse(utf8Text, provider, out var res) ? res : throw new FormatException("Invalid UTF-8 NIT representation.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out Nit result)
    {
        Span<char> chars = stackalloc char[utf8Text.Length];
        Encoding.UTF8.TryGetChars(utf8Text, chars, out int written);
        return TryParse(chars[..written], provider, out result);
    }
}





