// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Argentina;

/// <summary>
/// Represents an Argentine CAI (Código de Autorización de Impresión)
/// for traditional paper/pre-printed backup vouchers (RG 100/98).
///
/// <para><b>Structure:</b> Exactly 14 numeric digits and an associated expiration date.</para>
/// </summary>
[ValueObject]
public readonly record struct Cai : ISpanParsable<Cai>, IEquatable<Cai>
{
    private readonly string _code;
    private readonly DateOnly _expirationDate;

    private Cai(string code, DateOnly expirationDate)
    {
        _code = code;
        _expirationDate = expirationDate;
    }

    /// <summary>
    /// Gets the 14-digit CAI code.
    /// </summary>
    public string Code => _code;

    /// <summary>
    /// Gets the authorization expiration date.
    /// </summary>
    public DateOnly ExpirationDate => _expirationDate;

    /// <summary>
    /// Determines whether this CAI authorization is expired relative to the specified date.
    /// </summary>
    /// <param name="currentDate">The reference date to evaluate expiration against.</param>
    /// <returns><see langword="true"/> if the authorization is expired; otherwise, <see langword="false"/>.</returns>
    public bool IsExpired(DateOnly currentDate) => currentDate > _expirationDate;

    /// <summary>
    /// Creates a validated <see cref="Cai"/> instance from a 14-digit code and an expiration date.
    /// </summary>
    /// <param name="code">The 14-digit authorization code string.</param>
    /// <param name="expirationDate">The authorization expiration date.</param>
    /// <returns>A <see cref="Result{T}"/> containing the created <see cref="Cai"/> or a validation error.</returns>
    public static Result<Cai> Create(string? code, DateOnly expirationDate) =>
        Create(code.AsSpan(), expirationDate);

    /// <summary>
    /// Creates a validated <see cref="Cai"/> instance from a character span and an expiration date.
    /// </summary>
    /// <param name="input">The character span representing the 14-digit authorization code.</param>
    /// <param name="expirationDate">The authorization expiration date.</param>
    /// <returns>A <see cref="Result{T}"/> containing the created <see cref="Cai"/> or a validation error.</returns>
    public static Result<Cai> Create(ReadOnlySpan<char> input, DateOnly expirationDate)
    {
        ReadOnlySpan<char> trimmed = input.Trim();
        if (trimmed.Length != 14)
        {
            return Result<Cai>.Failure(Error.Validation(
                "Cai.InvalidLength", "The CAI must contain exactly 14 numeric digits."));
        }

        foreach (char c in trimmed)
        {
            if (!char.IsDigit(c))
            {
                return Result<Cai>.Failure(Error.Validation(
                    "Cai.InvalidCharacters", "The CAI must only contain numeric characters."));
            }
        }

        return Result<Cai>.Success(new Cai(trimmed.ToString(), expirationDate));
    }

    /// <inheritdoc/>
    public override string ToString() => $"{_code} (Vto: {_expirationDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)})";

    /// <inheritdoc/>
    public static Cai Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid CAI: '{s}'.");

    /// <inheritdoc/>
    public static Cai Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid CAI: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Cai result)
    {
        var res = Create(s, DateOnly.MaxValue);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out Cai result) =>
        TryParse(s.AsSpan(), provider, out result);
}




