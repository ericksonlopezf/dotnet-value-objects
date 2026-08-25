// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Colombia;

/// <summary>
/// Represents a Colombian 5-digit DANE Municipal Code (División Político-Administrativa de Colombia - DIVIPOLA),
/// required by DIAN for territorial tax reporting (ICA) and electronic invoicing addresses.
///
/// <para><b>Format:</b> Exactly 5 numeric digits: first 2 digits represent the Department (<see cref="DepartmentCode"/>),
/// and the last 3 digits represent the Municipality (<see cref="MunicipalityCode"/>), e.g. <c>"11001"</c> (Bogotá, D.C.).</para>
/// </summary>
[ValueObject]
public readonly record struct DaneMunicipalityCode : ISpanParsable<DaneMunicipalityCode>, IEquatable<DaneMunicipalityCode>
{
    private readonly string _code;

    private DaneMunicipalityCode(string code) => _code = code;

    /// <summary>
    /// Gets the 5-digit DANE municipal code.
    /// </summary>
    public string Code => _code;

    /// <summary>
    /// Gets the 2-digit department component.
    /// </summary>
    public string DepartmentCode => _code[..2];

    /// <summary>
    /// Gets the 3-digit municipality component.
    /// </summary>
    public string MunicipalityCode => _code[2..];

    /// <summary>
    /// Creates a validated <see cref="DaneMunicipalityCode"/> instance from a 5-digit string.
    /// </summary>
    /// <param name="code">The 5-digit DANE municipality code (2-digit department + 3-digit municipality).</param>
    /// <returns>A <see cref="Result{DaneMunicipalityCode}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<DaneMunicipalityCode> Create(string? code) =>
        Create(code.AsSpan());

    /// <summary>
    /// Creates a validated <see cref="DaneMunicipalityCode"/> instance from a character span.
    /// </summary>
    /// <param name="input">A character span containing the 5-digit DANE municipality code.</param>
    /// <returns>A <see cref="Result{DaneMunicipalityCode}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<DaneMunicipalityCode> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();
        if (trimmed.Length != 5)
        {
            return Result<DaneMunicipalityCode>.Failure(Error.Validation(
                "DaneMunicipalityCode.InvalidLength", "The DANE municipality code must contain exactly 5 numeric digits."));
        }

        foreach (char c in trimmed)
        {
            if (!char.IsDigit(c))
            {
                return Result<DaneMunicipalityCode>.Failure(Error.Validation(
                    "DaneMunicipalityCode.InvalidCharacters", "The DANE code must only contain numeric characters."));
            }
        }

        return Result<DaneMunicipalityCode>.Success(new DaneMunicipalityCode(trimmed.ToString()));
    }

    /// <inheritdoc/>
    public override string ToString() => _code;

    /// <inheritdoc/>
    public static DaneMunicipalityCode Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid DANE code: '{s}'.");

    /// <inheritdoc/>
    public static DaneMunicipalityCode Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid DANE code: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out DaneMunicipalityCode result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out DaneMunicipalityCode result) =>
        TryParse(s.AsSpan(), provider, out result);
}



