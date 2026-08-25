// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Colombia;

using EricksonLopez.ValueObjects.Attributes;

/// <summary>
/// Represents a Colombian CUDE (Código Único de Documento Electrónico),
/// the mandatory cryptographic identifier for Credit Notes, Debit Notes, Support Documents,
/// and Electronic POS (Resolución 000165/2023).
///
/// <para><b>Structure:</b> A 96-character lowercase hexadecimal string generated via SHA-384 hashing
/// using the software provider PIN (SoftwarePin).</para>
/// </summary>
[RegulatoryRule("CO.CUDE.001")]
[ValueObject]
public readonly record struct Cude : ISpanParsable<Cude>, IEquatable<Cude>
{
    private readonly string _value;

    private Cude(string value) => _value = value;

    /// <summary>
    /// Gets the raw 96-character hexadecimal CUDE value.
    /// </summary>
    public string Value => _value;

    /// <summary>
    /// Creates a validated <see cref="Cude"/> instance from a 96-character hexadecimal string.
    /// </summary>
    /// <param name="value">A string containing the 96-hex-character SHA-384 CUDE code.</param>
    /// <returns>A <see cref="Result{Cude}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<Cude> Create(string? value) =>
        Create(value.AsSpan());

    /// <summary>
    /// Creates a validated <see cref="Cude"/> instance from a character span.
    /// </summary>
    /// <param name="input">A character span containing the 96-hex-character SHA-384 CUDE code.</param>
    /// <returns>A <see cref="Result{Cude}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<Cude> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();
        if (trimmed.Length != 96)
        {
            return Result<Cude>.Failure(Error.Validation(
                "Cude.InvalidLength", "The CUDE must contain exactly 96 hexadecimal characters (SHA-384)."));
        }

        foreach (char c in trimmed)
        {
            if (!char.IsAsciiHexDigit(c))
            {
                return Result<Cude>.Failure(Error.Validation(
                    "Cude.InvalidCharacters", "The CUDE contains characters that are not valid hexadecimal digits."));
            }
        }

        return Result<Cude>.Success(new Cude(trimmed.ToString().ToLowerInvariant()));
    }

    /// <inheritdoc/>
    public override string ToString() => _value;

    /// <inheritdoc/>
    public static Cude Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid CUDE: '{s}'.");

    /// <inheritdoc/>
    public static Cude Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid CUDE: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Cude result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out Cude result) =>
        TryParse(s.AsSpan(), provider, out result);
}



