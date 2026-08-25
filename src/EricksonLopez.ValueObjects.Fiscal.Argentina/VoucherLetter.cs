// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Argentina;

/// <summary>
/// Represents an Argentine Fiscal Voucher Class Letter (Letra de Comprobante: A, B, C, E, M, T, R)
/// defined by ARCA/AFIP (Resolución General 1415/2003).
/// </summary>
[ValueObject]
public readonly record struct VoucherLetter : ISpanParsable<VoucherLetter>, IComparable<VoucherLetter>
{
    /// <summary>Gets voucher letter 'A'.</summary>
    public static VoucherLetter A => new('A');
    /// <summary>Gets voucher letter 'B'.</summary>
    public static VoucherLetter B => new('B');
    /// <summary>Gets voucher letter 'C'.</summary>
    public static VoucherLetter C => new('C');
    /// <summary>Gets voucher letter 'E'.</summary>
    public static VoucherLetter E => new('E');
    /// <summary>Gets voucher letter 'M'.</summary>
    public static VoucherLetter M => new('M');
    /// <summary>Gets voucher letter 'T'.</summary>
    public static VoucherLetter T => new('T');
    /// <summary>Gets voucher letter 'R'.</summary>
    public static VoucherLetter R => new('R');


    private readonly char _letter;

    private VoucherLetter(char letter) => _letter = letter;

    /// <summary>
    /// Gets the character representation of the voucher letter.
    /// </summary>
    public char Letter => _letter;

    /// <summary>
    /// Creates a validated <see cref="VoucherLetter"/> from a character.
    /// </summary>
    /// <param name="letter">The voucher class letter character (A, B, C, E, M, T, or R).</param>
    /// <returns>A <see cref="Result{VoucherLetter}"/> containing the matched letter or a domain validation error.</returns>
    public static Result<VoucherLetter> Create(char letter)
    {
        char upper = char.ToUpperInvariant(letter);
        return upper switch
        {
            'A' => Result<VoucherLetter>.Success(A),
            'B' => Result<VoucherLetter>.Success(B),
            'C' => Result<VoucherLetter>.Success(C),
            'E' => Result<VoucherLetter>.Success(E),
            'M' => Result<VoucherLetter>.Success(M),
            'T' => Result<VoucherLetter>.Success(T),
            'R' => Result<VoucherLetter>.Success(R),
            _ => Result<VoucherLetter>.Failure(Error.Validation(
                "VoucherLetter.InvalidLetter", $"The voucher letter '{letter}' is invalid (allowed: A, B, C, E, M, T, R)."))
        };
    }

    /// <summary>
    /// Creates a validated <see cref="VoucherLetter"/> from a text span.
    /// </summary>
    /// <param name="input">A character span containing a single voucher letter character.</param>
    /// <returns>A <see cref="Result{VoucherLetter}"/> containing the matched letter or a domain validation error.</returns>
    public static Result<VoucherLetter> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();
        if (trimmed.Length != 1)
        {
            return Result<VoucherLetter>.Failure(Error.Validation(
                "VoucherLetter.InvalidLength", "The voucher letter must contain exactly 1 character."));
        }

        return Create(trimmed[0]);
    }

    /// <summary>
    /// Creates a validated <see cref="VoucherLetter"/> from a nullable string.
    /// </summary>
    /// <param name="input">A string containing a single voucher letter character.</param>
    /// <returns>A <see cref="Result{VoucherLetter}"/> containing the matched letter or a domain validation error.</returns>
    public static Result<VoucherLetter> Create(string? input) =>
        Create(input.AsSpan());

    /// <inheritdoc/>
    public override string ToString() => _letter.ToString();

    /// <inheritdoc/>
    public int CompareTo(VoucherLetter other) => _letter.CompareTo(other._letter);

        /// <summary>
    /// Determines whether the left <see cref="VoucherLetter"/> is less than the right <see cref="VoucherLetter"/>.
    /// </summary>
    /// <param name="left">The first <see cref="VoucherLetter"/> to compare.</param>
    /// <param name="right">The second <see cref="VoucherLetter"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(VoucherLetter left, VoucherLetter right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="VoucherLetter"/> is less than or equal to the right <see cref="VoucherLetter"/>.
    /// </summary>
    /// <param name="left">The first <see cref="VoucherLetter"/> to compare.</param>
    /// <param name="right">The second <see cref="VoucherLetter"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(VoucherLetter left, VoucherLetter right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="VoucherLetter"/> is greater than the right <see cref="VoucherLetter"/>.
    /// </summary>
    /// <param name="left">The first <see cref="VoucherLetter"/> to compare.</param>
    /// <param name="right">The second <see cref="VoucherLetter"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(VoucherLetter left, VoucherLetter right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="VoucherLetter"/> is greater than or equal to the right <see cref="VoucherLetter"/>.
    /// </summary>
    /// <param name="left">The first <see cref="VoucherLetter"/> to compare.</param>
    /// <param name="right">The second <see cref="VoucherLetter"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(VoucherLetter left, VoucherLetter right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public static VoucherLetter Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid VoucherLetter: '{s}'.");

    /// <inheritdoc/>
    public static VoucherLetter Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid VoucherLetter: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out VoucherLetter result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out VoucherLetter result) =>
        TryParse(s.AsSpan(), provider, out result);
}



