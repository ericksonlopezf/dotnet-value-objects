// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Argentina;

/// <summary>
/// Represents an Argentine CAE (Código de Autorización Electrónico)
/// granted by ARCA/AFIP for standard electronic invoices (RG 4291/18).
///
/// <para><b>Structure:</b> Exactly 14 numeric digits and an associated expiration date (<see cref="ExpirationDate"/>).</para>
/// </summary>
[ValueObject]
public readonly record struct Cae : ISpanParsable<Cae>, IEquatable<Cae>
{
    private readonly string _code;
    private readonly DateOnly _expirationDate;

    private Cae(string code, DateOnly expirationDate)
    {
        _code = code;
        _expirationDate = expirationDate;
    }

    /// <summary>
    /// Gets the 14-digit CAE code.
    /// </summary>
    public string Code => _code;

    /// <summary>
    /// Gets the authorization expiration date.
    /// </summary>
    public DateOnly ExpirationDate => _expirationDate;

    /// <summary>
    /// Determines whether this CAE authorization is expired relative to the specified date.
    /// </summary>
    /// <param name="currentDate">The reference date to evaluate expiration against.</param>
    /// <returns><see langword="true"/> if the authorization is expired; otherwise, <see langword="false"/>.</returns>
    public bool IsExpired(DateOnly currentDate) => currentDate > _expirationDate;

    /// <summary>
    /// Creates a validated <see cref="Cae"/> instance from a 14-digit code and an expiration date.
    /// </summary>
    /// <param name="code">The 14-digit authorization code string.</param>
    /// <param name="expirationDate">The authorization expiration date.</param>
    /// <returns>A <see cref="Result{T}"/> containing the created <see cref="Cae"/> or a validation error.</returns>
    public static Result<Cae> Create(string? code, DateOnly expirationDate) =>
        Create(code.AsSpan(), expirationDate);

    /// <summary>
    /// Creates a validated <see cref="Cae"/> instance from a character span and an expiration date.
    /// </summary>
    /// <param name="input">The character span representing the 14-digit authorization code.</param>
    /// <param name="expirationDate">The authorization expiration date.</param>
    /// <returns>A <see cref="Result{T}"/> containing the created <see cref="Cae"/> or a validation error.</returns>
    public static Result<Cae> Create(ReadOnlySpan<char> input, DateOnly expirationDate)
    {
        ReadOnlySpan<char> trimmed = input.Trim();
        if (trimmed.Length != 14)
        {
            return Result<Cae>.Failure(Error.Validation(
                "Cae.InvalidLength", "The CAE must contain exactly 14 numeric digits."));
        }

        foreach (char c in trimmed)
        {
            if (!char.IsDigit(c))
            {
                return Result<Cae>.Failure(Error.Validation(
                    "Cae.InvalidCharacters", "The CAE must only contain numeric characters."));
            }
        }

        return Result<Cae>.Success(new Cae(trimmed.ToString(), expirationDate));
    }

    /// <inheritdoc/>
    public override string ToString() => $"{_code} (Vto: {_expirationDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)})";

    /// <inheritdoc/>
    public static Cae Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid CAE: '{s}'.");

    /// <inheritdoc/>
    public static Cae Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid CAE: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Cae result)
    {
        // When parsing a bare 14-digit string without date context, default to DateOnly.MaxValue
        var res = Create(s, DateOnly.MaxValue);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out Cae result) =>
        TryParse(s.AsSpan(), provider, out result);
}




