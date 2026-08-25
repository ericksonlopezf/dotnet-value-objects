// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Peru;

/// <summary>
/// Represents a Peruvian 6-digit Ubigeo Code (Código de Ubicación Geográfica INEI / SUNAT),
/// required for electronic invoicing, tax addresses, and dispatch guides.
///
/// <para><b>Structure:</b> Exactly 6 numeric digits: Department (2), Province (2), District (2), e.g. <c>"150101"</c> (Lima, Lima, Lima).</para>
/// </summary>
[ValueObject]
public readonly record struct UbigeoCode : ISpanParsable<UbigeoCode>, IComparable<UbigeoCode>
{
    private readonly string _code;

    private UbigeoCode(string code) => _code = code;

    /// <summary>
    /// Gets the 6-digit Ubigeo string.
    /// </summary>
    public string Code => _code;

    /// <summary>
    /// Gets the 2-digit Department code.
    /// </summary>
    public string DepartmentCode => _code[..2];

    /// <summary>
    /// Gets the 2-digit Province code.
    /// </summary>
    public string ProvinceCode => _code[2..4];

    /// <summary>
    /// Gets the 2-digit District code.
    /// </summary>
    public string DistrictCode => _code[4..];

    /// <summary>
    /// Creates a validated <see cref="UbigeoCode"/> from a 6-digit string.
    /// </summary>
    public static Result<UbigeoCode> Create(string? code) =>
        Create(code.AsSpan());

    /// <summary>
    /// Creates a validated <see cref="UbigeoCode"/> from a character span.
    /// </summary>
    public static Result<UbigeoCode> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();
        if (trimmed.Length != 6)
        {
            return Result<UbigeoCode>.Failure(Error.Validation(
                "UbigeoCode.InvalidLength", "The Ubigeo code must contain exactly 6 numeric digits."));
        }

        foreach (char c in trimmed)
        {
            if (!char.IsDigit(c))
            {
                return Result<UbigeoCode>.Failure(Error.Validation(
                    "UbigeoCode.InvalidCharacters", "The Ubigeo code must only contain numeric digits."));
            }
        }

        return Result<UbigeoCode>.Success(new UbigeoCode(trimmed.ToString()));
    }

    /// <inheritdoc/>
    public override string ToString() => _code;

    /// <inheritdoc/>
    public int CompareTo(UbigeoCode other) => string.Compare(_code, other._code, StringComparison.Ordinal);

        /// <summary>
    /// Determines whether the left <see cref="UbigeoCode"/> is less than the right <see cref="UbigeoCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="UbigeoCode"/> to compare.</param>
    /// <param name="right">The second <see cref="UbigeoCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(UbigeoCode left, UbigeoCode right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="UbigeoCode"/> is less than or equal to the right <see cref="UbigeoCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="UbigeoCode"/> to compare.</param>
    /// <param name="right">The second <see cref="UbigeoCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(UbigeoCode left, UbigeoCode right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="UbigeoCode"/> is greater than the right <see cref="UbigeoCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="UbigeoCode"/> to compare.</param>
    /// <param name="right">The second <see cref="UbigeoCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(UbigeoCode left, UbigeoCode right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="UbigeoCode"/> is greater than or equal to the right <see cref="UbigeoCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="UbigeoCode"/> to compare.</param>
    /// <param name="right">The second <see cref="UbigeoCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(UbigeoCode left, UbigeoCode right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public static UbigeoCode Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid UbigeoCode: '{s}'.");

    /// <inheritdoc/>
    public static UbigeoCode Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid UbigeoCode: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out UbigeoCode result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out UbigeoCode result) =>
        TryParse(s.AsSpan(), provider, out result);
}



