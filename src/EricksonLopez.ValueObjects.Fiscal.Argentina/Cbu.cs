// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using System.Text;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Argentina;

/// <summary>
/// Represents an Argentine CBU (Clave Bancaria Uniforme)
/// standardized by the Central Bank of Argentina (BCRA Comunicación "A" 2622).
///
/// <para><b>Structure:</b> Exactly 22 numeric digits divided into two blocks with double Modulo 10 check digits:
/// <list type="bullet">
///   <item><term>Block 1 (8 digits)</term><description>Bank Code (3), Branch Code (4), Check Digit 1 (1) weighted by <c>[7, 1, 3, 9, 7, 1, 3]</c>.</description></item>
///   <item><term>Block 2 (14 digits)</term><description>Account Identifier (13), Check Digit 2 (1) weighted by <c>[3, 9, 7, 1, 3, 9, 7, 1, 3, 9, 7, 1, 3]</c>.</description></item>
/// </list>
/// </para>
/// </summary>
[ValueObject]
public readonly record struct Cbu : ISpanParsable<Cbu>, IUtf8SpanParsable<Cbu>, IComparable<Cbu>
{
    private readonly string _value;

    private Cbu(string value) => _value = value;


    /// <summary>
    /// Gets the raw 22-digit CBU string.
    /// </summary>
    public string Value => _value;

    /// <summary>
    /// Gets the 3-digit bank code.
    /// </summary>
    public string BankCode => _value[..3];

    /// <summary>
    /// Gets the 4-digit branch code.
    /// </summary>
    public string BranchCode => _value[3..7];

    /// <summary>
    /// Gets the 13-digit account identifier.
    /// </summary>
    public string AccountNumber => _value[8..21];

    /// <summary>
    /// Creates a validated <see cref="Cbu"/> from a 22-digit numeric string.
    /// </summary>
    /// <param name="value">The 22-digit CBU string, optionally containing hyphens or spaces that will be stripped.</param>
    /// <returns>A <see cref="Result{Cbu}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<Cbu> Create(string? value) =>
        Create(value.AsSpan());

    /// <summary>
    /// Creates a validated <see cref="Cbu"/> from a character span.
    /// </summary>
    /// <param name="input">A character span containing the 22-digit CBU digits.</param>
    /// <returns>A <see cref="Result{Cbu}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<Cbu> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();
        if (trimmed.Length != 22)
        {
            return Result<Cbu>.Failure(Error.Validation(
                "Cbu.InvalidLength", "The CBU must contain exactly 22 numeric digits."));
        }

        foreach (char c in trimmed)
        {
            if (!char.IsDigit(c))
            {
                return Result<Cbu>.Failure(Error.Validation(
                    "Cbu.InvalidCharacters", "The CBU must only contain numeric digits."));
            }
        }

        int dv1 = CalculateBlock1CheckDigit(trimmed[..7]);
        int providedDv1 = trimmed[7] - '0';
        if (dv1 != providedDv1)
        {
            return Result<Cbu>.Failure(Error.Validation(
                "Cbu.InvalidCheckDigit1", $"The first check digit of the CBU is invalid (expected: {dv1.ToString(CultureInfo.InvariantCulture)})."));
        }

        int dv2 = CalculateBlock2CheckDigit(trimmed[8..21]);
        int providedDv2 = trimmed[21] - '0';
        if (dv2 != providedDv2)
        {
            return Result<Cbu>.Failure(Error.Validation(
                "Cbu.InvalidCheckDigit2", $"The second check digit of the CBU is invalid (expected: {dv2.ToString(CultureInfo.InvariantCulture)})."));
        }

        return Result<Cbu>.Success(new Cbu(trimmed.ToString()));
    }

    /// <summary>
    /// Computes the Modulo 10 check digit for the first 7 digits (Bank + Branch).
    /// </summary>
    /// <param name="digits7">A 7-character span containing the bank and branch digits.</param>
    /// <returns>The computed check digit (0 to 9).</returns>
    public static int CalculateBlock1CheckDigit(ReadOnlySpan<char> digits7)
    {
        ReadOnlySpan<int> block1Weights = [7, 1, 3, 9, 7, 1, 3];
        int sum = 0;
        for (int i = 0; i < 7; i++)
        {
            sum += (digits7[i] - '0') * block1Weights[i];
        }

        int remainder = sum % 10;
        return remainder == 0 ? 0 : 10 - remainder;
    }

    /// <summary>
    /// Computes the Modulo 10 check digit for the middle 13 digits (Account).
    /// </summary>
    /// <param name="digits13">A 13-character span containing the account identifier digits.</param>
    /// <returns>The computed check digit (0 to 9).</returns>
    public static int CalculateBlock2CheckDigit(ReadOnlySpan<char> digits13)
    {
        ReadOnlySpan<int> block2Weights = [3, 9, 7, 1, 3, 9, 7, 1, 3, 9, 7, 1, 3];
        int sum = 0;
        for (int i = 0; i < 13; i++)
        {
            sum += (digits13[i] - '0') * block2Weights[i];
        }

        int remainder = sum % 10;
        return remainder == 0 ? 0 : 10 - remainder;
    }


    /// <inheritdoc/>
    public override string ToString() => _value;

    /// <inheritdoc/>
    public int CompareTo(Cbu other) => string.Compare(_value, other._value, StringComparison.Ordinal);

        /// <summary>
    /// Determines whether the left <see cref="Cbu"/> is less than the right <see cref="Cbu"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Cbu"/> to compare.</param>
    /// <param name="right">The second <see cref="Cbu"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(Cbu left, Cbu right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="Cbu"/> is less than or equal to the right <see cref="Cbu"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Cbu"/> to compare.</param>
    /// <param name="right">The second <see cref="Cbu"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(Cbu left, Cbu right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="Cbu"/> is greater than the right <see cref="Cbu"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Cbu"/> to compare.</param>
    /// <param name="right">The second <see cref="Cbu"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(Cbu left, Cbu right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="Cbu"/> is greater than or equal to the right <see cref="Cbu"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Cbu"/> to compare.</param>
    /// <param name="right">The second <see cref="Cbu"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(Cbu left, Cbu right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public static Cbu Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid CBU: '{s}'.");

    /// <inheritdoc/>
    public static Cbu Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid CBU: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Cbu result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out Cbu result) =>
        TryParse(s.AsSpan(), provider, out result);

    /// <inheritdoc/>
    public static Cbu Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider = null) =>
        TryParse(utf8Text, provider, out var res) ? res : throw new FormatException("Invalid UTF-8 CBU representation.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out Cbu result)
    {
        Span<char> chars = stackalloc char[utf8Text.Length];
        Encoding.UTF8.TryGetChars(utf8Text, chars, out int written);
        return TryParse(chars[..written], provider, out result);
    }
}





