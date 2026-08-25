// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Peru;

using System.Globalization;
using EricksonLopez.ValueObjects.Attributes;

/// <summary>
/// Represents a SUNAT Product Code (Catálogo 25 - UNSPSC de 8 dígitos).
/// Implements deferred temporal regression (rule mandatory starting from 01-Jan-2027).
/// </summary>
[RegulatoryRule("CAT.VAL.001")]
[ValueObject]
public readonly record struct SunatProductCode : ISpanParsable<SunatProductCode>, IComparable<SunatProductCode>
{
    private readonly string _code;

    private SunatProductCode(string code) => _code = code;

    /// <summary>
    /// Gets the SUNAT Product Code (Catálogo 25).
    /// </summary>
    public string Code => _code;

    /// <summary>
    /// Creates a validated <see cref="SunatProductCode"/> evaluating the temporal regression rule CAT.VAL.001.
    /// </summary>
    public static Result<SunatProductCode> Create(string? code, DateOnly effectiveDate) =>
        Create(code.AsSpan(), effectiveDate);

    /// <summary>
    /// Creates a validated <see cref="SunatProductCode"/> evaluating the temporal regression rule CAT.VAL.001.
    /// </summary>
    public static Result<SunatProductCode> Create(ReadOnlySpan<char> input, DateOnly effectiveDate)
    {
        bool isStrict = effectiveDate >= new DateOnly(2027, 1, 1);
        ReadOnlySpan<char> trimmed = input.Trim();

        if (!isStrict && trimmed.Length != 8)
        {
            // CAT.VAL.001: Not strictly mandatory until 31/12/2026.
            return Result<SunatProductCode>.Success(new SunatProductCode(trimmed.ToString()));
        }

        return Create(input);
    }

    /// <summary>
    /// Creates a validated <see cref="SunatProductCode"/> using strict validation (defaulting to post-2027 behavior).
    /// </summary>
    public static Result<SunatProductCode> Create(string? code) =>
        Create(code.AsSpan());

    /// <summary>
    /// Creates a validated <see cref="SunatProductCode"/> using strict validation (defaulting to post-2027 behavior).
    /// </summary>
    public static Result<SunatProductCode> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();

        if (trimmed.Length != 8)
        {
            return Result<SunatProductCode>.Failure(Error.Validation(
                "SunatProductCode.InvalidLength", "The SUNAT product code must contain exactly 8 digits."));
        }

        foreach (char c in trimmed)
        {
            if (!char.IsDigit(c))
            {
                return Result<SunatProductCode>.Failure(Error.Validation(
                    "SunatProductCode.InvalidCharacters", "The SUNAT product code must only contain numeric digits."));
            }
        }

        return Result<SunatProductCode>.Success(new SunatProductCode(trimmed.ToString()));
    }

    /// <inheritdoc/>
    public override string ToString() => _code ?? string.Empty;

    /// <inheritdoc/>
    public int CompareTo(SunatProductCode other) => string.Compare(_code, other._code, StringComparison.Ordinal);

        /// <summary>
    /// Determines whether the left <see cref="SunatProductCode"/> is less than the right <see cref="SunatProductCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="SunatProductCode"/> to compare.</param>
    /// <param name="right">The second <see cref="SunatProductCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(SunatProductCode left, SunatProductCode right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="SunatProductCode"/> is less than or equal to the right <see cref="SunatProductCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="SunatProductCode"/> to compare.</param>
    /// <param name="right">The second <see cref="SunatProductCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(SunatProductCode left, SunatProductCode right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="SunatProductCode"/> is greater than the right <see cref="SunatProductCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="SunatProductCode"/> to compare.</param>
    /// <param name="right">The second <see cref="SunatProductCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(SunatProductCode left, SunatProductCode right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="SunatProductCode"/> is greater than or equal to the right <see cref="SunatProductCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="SunatProductCode"/> to compare.</param>
    /// <param name="right">The second <see cref="SunatProductCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(SunatProductCode left, SunatProductCode right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public static SunatProductCode Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid SunatProductCode: '{s}'.");

    /// <inheritdoc/>
    public static SunatProductCode Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid SunatProductCode: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out SunatProductCode result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out SunatProductCode result) =>
        TryParse(s.AsSpan(), provider, out result);
}



