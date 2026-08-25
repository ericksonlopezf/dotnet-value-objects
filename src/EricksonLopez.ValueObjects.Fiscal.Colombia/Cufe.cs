// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Colombia;

using EricksonLopez.ValueObjects.Attributes;

/// <summary>
/// Represents a Colombian CUFE (Código Único de Factura Electrónica),
/// the mandatory cryptographic identifier for Electronic Sales Invoices (Resolución 000165/2023).
///
/// <para><b>Structure:</b> A 96-character lowercase hexadecimal string generated via SHA-384 hashing
/// concatenating invoice fields and the technical key (TechnicalKey).</para>
/// </summary>
[RegulatoryRule("CO.CUFE.001")]
[ValueObject]
public readonly record struct Cufe : ISpanParsable<Cufe>, IEquatable<Cufe>
{
    private readonly string _value;

    private Cufe(string value) => _value = value;

    /// <summary>
    /// Gets the raw 96-character hexadecimal CUFE value.
    /// </summary>
    public string Value => _value;

    /// <summary>
    /// Creates a validated <see cref="Cufe"/> instance from a 96-character hexadecimal string.
    /// </summary>
    /// <param name="value">A string containing the 96-hex-character SHA-384 CUFE code.</param>
    /// <returns>A <see cref="Result{Cufe}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<Cufe> Create(string? value) =>
        Create(value.AsSpan());

    /// <summary>
    /// Creates a validated <see cref="Cufe"/> instance from a character span.
    /// </summary>
    /// <param name="input">A character span containing the 96-hex-character SHA-384 CUFE code.</param>
    /// <returns>A <see cref="Result{Cufe}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<Cufe> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();
        if (trimmed.Length != 96)
        {
            return Result<Cufe>.Failure(Error.Validation(
                "Cufe.InvalidLength", "The CUFE must contain exactly 96 hexadecimal characters (SHA-384)."));
        }

        foreach (char c in trimmed)
        {
            if (!char.IsAsciiHexDigit(c))
            {
                return Result<Cufe>.Failure(Error.Validation(
                    "Cufe.InvalidCharacters", "The CUFE contains characters that are not valid hexadecimal digits."));
            }
        }

        return Result<Cufe>.Success(new Cufe(trimmed.ToString().ToLowerInvariant()));
    }

    /// <inheritdoc/>
    public override string ToString() => _value;

    /// <inheritdoc/>
    public static Cufe Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid CUFE: '{s}'.");

    /// <inheritdoc/>
    public static Cufe Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid CUFE: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Cufe result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out Cufe result) =>
        TryParse(s.AsSpan(), provider, out result);
}



