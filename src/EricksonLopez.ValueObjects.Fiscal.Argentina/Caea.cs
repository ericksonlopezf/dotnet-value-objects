// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Argentina;

/// <summary>
/// Represents an Argentine CAEA (Código de Autorización Electrónico Anticipado)
/// granted by ARCA/AFIP for the anticipated authorization regime (RG 2926).
///
/// <para><b>Structure:</b> Exactly 14 numeric digits and an associated expiration date.</para>
/// </summary>
[ValueObject]
public readonly record struct Caea : ISpanParsable<Caea>, IEquatable<Caea>
{
    private readonly string _code;
    private readonly DateOnly _expirationDate;

    private Caea(string code, DateOnly expirationDate)
    {
        _code = code;
        _expirationDate = expirationDate;
    }

    /// <summary>
    /// Gets the 14-digit CAEA code.
    /// </summary>
    public string Code => _code;

    /// <summary>
    /// Gets the authorization expiration date.
    /// </summary>
    public DateOnly ExpirationDate => _expirationDate;

    /// <summary>
    /// Determines whether this CAEA authorization is expired relative to the specified date.
    /// </summary>
    /// <param name="currentDate">The reference date to evaluate expiration against.</param>
    /// <returns><see langword="true"/> if the authorization is expired; otherwise, <see langword="false"/>.</returns>
    public bool IsExpired(DateOnly currentDate) => currentDate > _expirationDate;

    /// <summary>
    /// Creates a validated <see cref="Caea"/> instance from a 14-digit code and an expiration date.
    /// </summary>
    /// <param name="code">The 14-digit authorization code string.</param>
    /// <param name="expirationDate">The authorization expiration date.</param>
    /// <returns>A <see cref="Result{T}"/> containing the created <see cref="Caea"/> or a validation error.</returns>
    public static Result<Caea> Create(string? code, DateOnly expirationDate) =>
        Create(code.AsSpan(), expirationDate);

    /// <summary>
    /// Creates a validated <see cref="Caea"/> instance from a character span and an expiration date.
    /// </summary>
    /// <param name="input">The character span representing the 14-digit authorization code.</param>
    /// <param name="expirationDate">The authorization expiration date.</param>
    /// <returns>A <see cref="Result{T}"/> containing the created <see cref="Caea"/> or a validation error.</returns>
    public static Result<Caea> Create(ReadOnlySpan<char> input, DateOnly expirationDate)
    {
        ReadOnlySpan<char> trimmed = input.Trim();
        if (trimmed.Length != 14)
        {
            return Result<Caea>.Failure(Error.Validation(
                "Caea.InvalidLength", "The CAEA must contain exactly 14 numeric digits."));
        }

        foreach (char c in trimmed)
        {
            if (!char.IsDigit(c))
            {
                return Result<Caea>.Failure(Error.Validation(
                    "Caea.InvalidCharacters", "The CAEA must only contain numeric characters."));
            }
        }

        return Result<Caea>.Success(new Caea(trimmed.ToString(), expirationDate));
    }

    /// <inheritdoc/>
    public override string ToString() => $"{_code} (Vto: {_expirationDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)})";

    /// <inheritdoc/>
    public static Caea Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid CAEA: '{s}'.");

    /// <inheritdoc/>
    public static Caea Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid CAEA: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Caea result)
    {
        var res = Create(s, DateOnly.MaxValue);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out Caea result) =>
        TryParse(s.AsSpan(), provider, out result);
}




