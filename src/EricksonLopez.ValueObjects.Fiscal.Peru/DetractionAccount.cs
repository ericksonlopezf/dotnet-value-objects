// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Peru;

/// <summary>
/// Represents a Peruvian SPOT Detraction Bank Account Number (Cuenta de Detracciones Banco de la Nación)
/// governed by Decreto Legislativo N° 940 and SUNAT regulations.
///
/// <para><b>Rules:</b> Exactly 11 numeric digits starting with the mandatory prefix <c>"00"</c>.</para>
/// </summary>
[ValueObject]
public readonly record struct DetractionAccount : ISpanParsable<DetractionAccount>, IComparable<DetractionAccount>
{
    private readonly string _accountNumber;

    private DetractionAccount(string accountNumber) => _accountNumber = accountNumber;

    /// <summary>
    /// Gets the 11-digit detraction account string.
    /// </summary>
    public string AccountNumber => _accountNumber;

    /// <summary>
    /// Creates a validated <see cref="DetractionAccount"/> from an 11-digit string.
    /// </summary>
    public static Result<DetractionAccount> Create(string? value) =>
        Create(value.AsSpan());

    /// <summary>
    /// Creates a validated <see cref="DetractionAccount"/> from a character span.
    /// </summary>
    public static Result<DetractionAccount> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();
        if (trimmed.Length != 11)
        {
            return Result<DetractionAccount>.Failure(Error.Validation(
                "DetractionAccount.InvalidLength", "The Banco de la Nacion detraction account must contain exactly 11 digits."));
        }

        if (!trimmed.StartsWith("00", StringComparison.Ordinal))
        {
            return Result<DetractionAccount>.Failure(Error.Validation(
                "DetractionAccount.InvalidPrefix", "The Banco de la Nacion detraction account must start with '00'."));
        }

        foreach (char c in trimmed)
        {
            if (!char.IsDigit(c))
            {
                return Result<DetractionAccount>.Failure(Error.Validation(
                    "DetractionAccount.InvalidCharacters", "The detraction account must only contain numeric digits."));
            }
        }

        return Result<DetractionAccount>.Success(new DetractionAccount(trimmed.ToString()));
    }

    /// <inheritdoc/>
    public override string ToString() => _accountNumber;

    /// <inheritdoc/>
    public int CompareTo(DetractionAccount other) => string.Compare(_accountNumber, other._accountNumber, StringComparison.Ordinal);

        /// <summary>
    /// Determines whether the left <see cref="DetractionAccount"/> is less than the right <see cref="DetractionAccount"/>.
    /// </summary>
    /// <param name="left">The first <see cref="DetractionAccount"/> to compare.</param>
    /// <param name="right">The second <see cref="DetractionAccount"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(DetractionAccount left, DetractionAccount right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="DetractionAccount"/> is less than or equal to the right <see cref="DetractionAccount"/>.
    /// </summary>
    /// <param name="left">The first <see cref="DetractionAccount"/> to compare.</param>
    /// <param name="right">The second <see cref="DetractionAccount"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(DetractionAccount left, DetractionAccount right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="DetractionAccount"/> is greater than the right <see cref="DetractionAccount"/>.
    /// </summary>
    /// <param name="left">The first <see cref="DetractionAccount"/> to compare.</param>
    /// <param name="right">The second <see cref="DetractionAccount"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(DetractionAccount left, DetractionAccount right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="DetractionAccount"/> is greater than or equal to the right <see cref="DetractionAccount"/>.
    /// </summary>
    /// <param name="left">The first <see cref="DetractionAccount"/> to compare.</param>
    /// <param name="right">The second <see cref="DetractionAccount"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(DetractionAccount left, DetractionAccount right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public static DetractionAccount Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid DetractionAccount: '{s}'.");

    /// <inheritdoc/>
    public static DetractionAccount Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid DetractionAccount: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out DetractionAccount result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out DetractionAccount result) =>
        TryParse(s.AsSpan(), provider, out result);
}



