// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Argentina;

using EricksonLopez.ValueObjects.Attributes;

/// <summary>
/// Represents an Argentine Fiscal Voucher Sequential Number (Número de Comprobante)
/// authorized by ARCA/AFIP (Resolución General 1415/2003).
///
/// <para><b>Rules:</b> Numeric value strictly between 1 and 99,999,999. Standard format is 8 digits zero-padded (<c>00000001</c>).</para>
/// </summary>
[RegulatoryRule("DOC.SEQ.002")]
[ValueObject]
public readonly record struct VoucherNumber : ISpanParsable<VoucherNumber>, IComparable<VoucherNumber>
{
    private readonly int _value;

    private VoucherNumber(int value) => _value = value;

    /// <summary>
    /// Gets the integer sequential value.
    /// </summary>
    public int Value => _value;

    /// <summary>
    /// Creates a validated <see cref="VoucherNumber"/> from an integer value.
    /// </summary>
    /// <param name="value">The sequential voucher number (between 1 and 99,999,999).</param>
    /// <returns>A <see cref="Result{VoucherNumber}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<VoucherNumber> Create(int value)
    {
        if (value is < 1 or > 99_999_999)
        {
            return Result<VoucherNumber>.Failure(Error.Validation(
                "VoucherNumber.OutOfRange", $"The voucher number must be between 1 and 99999999. Received: {value.ToString(CultureInfo.InvariantCulture)}."));
        }

        return Result<VoucherNumber>.Success(new VoucherNumber(value));
    }

    /// <summary>
    /// Creates a validated <see cref="VoucherNumber"/> from a numeric text span.
    /// </summary>
    /// <param name="input">A character span containing the numeric voucher number.</param>
    /// <returns>A <see cref="Result{VoucherNumber}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<VoucherNumber> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();
        if (!int.TryParse(trimmed, CultureInfo.InvariantCulture, out int number))
        {
            return Result<VoucherNumber>.Failure(Error.Validation(
                "VoucherNumber.InvalidFormat", "The voucher number must be a valid integer."));
        }

        return Create(number);
    }

    /// <summary>
    /// Creates a validated <see cref="VoucherNumber"/> from a nullable string.
    /// </summary>
    /// <param name="input">A string containing the numeric voucher number.</param>
    /// <returns>A <see cref="Result{VoucherNumber}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<VoucherNumber> Create(string? input) =>
        Create(input.AsSpan());

    /// <summary>
    /// Formats the voucher number as an 8-digit zero-padded string (<c>00000001</c>).
    /// </summary>
    public string Formatted => _value.ToString("D8", CultureInfo.InvariantCulture);

    /// <inheritdoc/>
    public override string ToString() => Formatted;

    /// <inheritdoc/>
    public int CompareTo(VoucherNumber other) => _value.CompareTo(other._value);

        /// <summary>
    /// Determines whether the left <see cref="VoucherNumber"/> is less than the right <see cref="VoucherNumber"/>.
    /// </summary>
    /// <param name="left">The first <see cref="VoucherNumber"/> to compare.</param>
    /// <param name="right">The second <see cref="VoucherNumber"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(VoucherNumber left, VoucherNumber right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="VoucherNumber"/> is less than or equal to the right <see cref="VoucherNumber"/>.
    /// </summary>
    /// <param name="left">The first <see cref="VoucherNumber"/> to compare.</param>
    /// <param name="right">The second <see cref="VoucherNumber"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(VoucherNumber left, VoucherNumber right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="VoucherNumber"/> is greater than the right <see cref="VoucherNumber"/>.
    /// </summary>
    /// <param name="left">The first <see cref="VoucherNumber"/> to compare.</param>
    /// <param name="right">The second <see cref="VoucherNumber"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(VoucherNumber left, VoucherNumber right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="VoucherNumber"/> is greater than or equal to the right <see cref="VoucherNumber"/>.
    /// </summary>
    /// <param name="left">The first <see cref="VoucherNumber"/> to compare.</param>
    /// <param name="right">The second <see cref="VoucherNumber"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(VoucherNumber left, VoucherNumber right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public static VoucherNumber Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid VoucherNumber: '{s}'.");

    /// <inheritdoc/>
    public static VoucherNumber Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid VoucherNumber: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out VoucherNumber result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out VoucherNumber result) =>
        TryParse(s.AsSpan(), provider, out result);
}




