// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Colombia;

using EricksonLopez.ValueObjects.Attributes;

/// <summary>
/// Represents a Colombian CUNE (Código Único de Nómina Electrónica),
/// the mandatory cryptographic identifier for Electronic Payroll Support Documents (Resolución 000013/2021).
///
/// <para><b>Structure:</b> A 96-character lowercase hexadecimal string generated via SHA-384 hashing
/// concatenating employer NIT, employee identification, payroll dates, amounts, and software PIN.</para>
/// </summary>
[RegulatoryRule("CO.CUNE.001")]
[ValueObject]
public readonly record struct Cune : ISpanParsable<Cune>, IEquatable<Cune>
{
    private readonly string _value;

    private Cune(string value) => _value = value;

    /// <summary>
    /// Gets the raw 96-character hexadecimal CUNE value.
    /// </summary>
    public string Value => _value;

    /// <summary>
    /// Creates a validated <see cref="Cune"/> instance from a 96-character hexadecimal string.
    /// </summary>
    /// <param name="value">A string containing the 96-hex-character SHA-384 CUNE code.</param>
    /// <returns>A <see cref="Result{Cune}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<Cune> Create(string? value) =>
        Create(value.AsSpan());

    /// <summary>
    /// Creates a validated <see cref="Cune"/> instance from a character span.
    /// </summary>
    /// <param name="input">A character span containing the 96-hex-character SHA-384 CUNE code.</param>
    /// <returns>A <see cref="Result{Cune}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<Cune> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();
        if (trimmed.Length != 96)
        {
            return Result<Cune>.Failure(Error.Validation(
                "Cune.InvalidLength", "The CUNE must contain exactly 96 hexadecimal characters (SHA-384)."));
        }

        foreach (char c in trimmed)
        {
            if (!char.IsAsciiHexDigit(c))
            {
                return Result<Cune>.Failure(Error.Validation(
                    "Cune.InvalidCharacters", "The CUNE contains characters that are not valid hexadecimal digits."));
            }
        }

        return Result<Cune>.Success(new Cune(trimmed.ToString().ToLowerInvariant()));
    }

    /// <inheritdoc/>
    public override string ToString() => _value;

    /// <inheritdoc/>
    public static Cune Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid CUNE: '{s}'.");

    /// <inheritdoc/>
    public static Cune Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid CUNE: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Cune result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out Cune result) =>
        TryParse(s.AsSpan(), provider, out result);
}



