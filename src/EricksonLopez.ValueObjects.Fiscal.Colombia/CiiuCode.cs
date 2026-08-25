// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Colombia;

/// <summary>
/// Represents a 4-digit Colombian CIIU economic activity code (Clasificación Industrial Internacional Uniforme, Revisión 4 adaptada para Colombia).
/// Required by the DIAN in the RUT (Registro Único Tributario) and electronic documents.
/// </summary>
[ValueObject]
public readonly record struct CiiuCode : ISpanParsable<CiiuCode>, IEquatable<CiiuCode>
{
    private readonly string _code;

    private CiiuCode(string code) => _code = code;

    /// <summary>
    /// Gets the 4-digit CIIU code.
    /// </summary>
    public string Code => _code;

    /// <summary>
    /// Creates a validated <see cref="CiiuCode"/> instance from a 4-digit string.
    /// </summary>
    /// <param name="code">The CIIU activity code string (e.g. <c>"6201"</c>).</param>
    /// <returns>A <see cref="Result{CiiuCode}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<CiiuCode> Create(string? code) =>
        Create(code.AsSpan());

    /// <summary>
    /// Creates a validated <see cref="CiiuCode"/> instance from a character span.
    /// </summary>
    /// <param name="input">A character span containing the CIIU activity code.</param>
    /// <returns>A <see cref="Result{CiiuCode}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<CiiuCode> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();
        if (trimmed.Length != 4)
        {
            return Result<CiiuCode>.Failure(Error.Validation(
                "CiiuCode.InvalidLength", "The CIIU economic activity code must contain exactly 4 numeric digits."));
        }

        foreach (char c in trimmed)
        {
            if (!char.IsDigit(c))
            {
                return Result<CiiuCode>.Failure(Error.Validation(
                    "CiiuCode.InvalidCharacters", "The CIIU code must only contain numeric characters."));
            }
        }

        return Result<CiiuCode>.Success(new CiiuCode(trimmed.ToString()));
    }

    /// <inheritdoc/>
    public override string ToString() => _code;

    /// <inheritdoc/>
    public static CiiuCode Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid CIIU code: '{s}'.");

    /// <inheritdoc/>
    public static CiiuCode Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid CIIU code: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out CiiuCode result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out CiiuCode result) =>
        TryParse(s.AsSpan(), provider, out result);
}



