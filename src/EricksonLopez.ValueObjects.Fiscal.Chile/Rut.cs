// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using System.Text;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Chile;

using EricksonLopez.ValueObjects.Attributes;

/// <summary>
/// Represents a Chilean RUT (Rol Único Tributario) / RUN (Rol Único Nacional)
/// administered by the SII (Servicio de Impuestos Internos) and Registro Civil.
///
/// <para><b>Structure:</b> A base number between 1 and 99,999,999 and a Modulo 11 verification digit (<c>'0'</c>-<c>'9'</c> or <c>'K'</c>).</para>
/// <para><b>Formats:</b> Canonical <c>"12345678-K"</c>, formatted with thousands dots <c>"12.345.678-K"</c>.</para>
/// </summary>
[RegulatoryRule("CL.RUT.001")]
[ValueObject]
public readonly record struct Rut : ISpanParsable<Rut>, IUtf8SpanParsable<Rut>, IComparable<Rut>
{
    private readonly int _body;
    private readonly char _dv;

    private Rut(int body, char dv)
    {
        _body = body;
        _dv = dv;
    }

    /// <summary>
    /// Gets the base numeric sequence of the RUT (Cuerpo).
    /// </summary>
    public int Body => _body;

    /// <summary>
    /// Gets the uppercase verification check digit (DV, '0'-'9' or 'K').
    /// </summary>
    public char Dv => _dv;

    /// <summary>
    /// Creates a validated <see cref="Rut"/> from a numeric body value, computing its check digit.
    /// </summary>
    /// <param name="body">The RUT body (cuerpo) integer value between 1 and 99,999,999.</param>
    /// <returns>A <see cref="Result{Rut}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<Rut> Create(int body)
    {
        if (body is < 1 or > 99_999_999)
        {
            return Result<Rut>.Failure(Error.Validation(
                "Rut.OutOfRange", "El cuerpo del RUT debe ser un número entero entre 1 y 99999999."));
        }

        char dv = CalculateVerificationDigit(body);
        return Result<Rut>.Success(new Rut(body, dv));
    }

    /// <summary>
    /// Creates a validated <see cref="Rut"/> from a raw or formatted text span (e.g. <c>"12.345.678-K"</c> or <c>"12345678-K"</c> or <c>"12345678K"</c>).
    /// </summary>
    /// <param name="input">A character span containing the RUT in any recognized format.</param>
    /// <returns>A <see cref="Result{Rut}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<Rut> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();
        if (trimmed.IsEmpty)
        {
            return Result<Rut>.Failure(Error.Validation(
                "Rut.Required", "The RUT is required."));
        }

        Span<char> cleanDigits = stackalloc char[8];
        int bodyCount = 0;
        char providedDv = '\0';

        // Extract body and DV
        int hyphenIndex = trimmed.IndexOf('-');
        ReadOnlySpan<char> bodySpan;
        if (hyphenIndex >= 0)
        {
            bodySpan = trimmed[..hyphenIndex];
            ReadOnlySpan<char> dvSpan = trimmed[(hyphenIndex + 1)..].Trim();
            if (dvSpan.Length != 1)
            {
                return Result<Rut>.Failure(Error.Validation(
                    "Rut.InvalidDv", "The check digit must be 1 character ('0'-'9' or 'K')."));
            }
            providedDv = char.ToUpperInvariant(dvSpan[0]);
        }
        else
        {
            bodySpan = trimmed[..^1];
            providedDv = char.ToUpperInvariant(trimmed[^1]);
        }

        foreach (char c in bodySpan)
        {
            if (char.IsDigit(c))
            {
                if (bodyCount >= 8)
                {
                    return Result<Rut>.Failure(Error.Validation(
                        "Rut.OutOfRange", "The RUT body cannot exceed 8 numeric digits."));
                }
                cleanDigits[bodyCount++] = c;
            }
            else if (c != '.')
            {
                return Result<Rut>.Failure(Error.Validation(
                    "Rut.InvalidCharacters", "The RUT body contains invalid characters."));
            }
        }

        if (bodyCount == 0 || !int.TryParse(cleanDigits[..bodyCount], CultureInfo.InvariantCulture, out int body))
        {
            return Result<Rut>.Failure(Error.Validation(
                "Rut.InvalidBody", "The RUT body is invalid."));
        }

        if (body < 1)
        {
            return Result<Rut>.Failure(Error.Validation(
                "Rut.OutOfRange", "The RUT body must be an integer between 1 and 99999999."));
        }

        char computedDv = CalculateVerificationDigit(body);

        if (providedDv != '\0' && providedDv != computedDv)
        {
            return Result<Rut>.Failure(Error.Validation(
                "Rut.InvalidVerificationDigit",
                $"The verification check digit '{providedDv}' is invalid (expected: '{computedDv}')."));
        }

        return Result<Rut>.Success(new Rut(body, computedDv));
    }

    /// <summary>
    /// Creates a validated <see cref="Rut"/> from a nullable string.
    /// </summary>
    /// <param name="input">A string containing the RUT in any recognized format.</param>
    /// <returns>A <see cref="Result{Rut}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<Rut> Create(string? input) =>
        Create(input.AsSpan());

    /// <summary>
    /// Calculates the official Modulo 11 check digit for a Chilean RUT body.
    /// </summary>
    public static char CalculateVerificationDigit(int body)
    {
        ReadOnlySpan<int> weights = [2, 3, 4, 5, 6, 7];
        int sum = 0;
        int current = body;
        int weightIndex = 0;

        while (current > 0)
        {
            int digit = current % 10;
            sum += digit * weights[weightIndex];
            weightIndex = (weightIndex + 1) % weights.Length;
            current /= 10;
        }

        int remainder = sum % 11;
        int dvNumber = 11 - remainder;

        return dvNumber switch
        {
            11 => '0',
            10 => 'K',
            _ => (char)('0' + dvNumber)
        };
    }

    /// <summary>
    /// Formats the RUT in canonical format without dots: <c>12345678-K</c>.
    /// </summary>
    public string ToCanonicalString() => $"{_body.ToString(CultureInfo.InvariantCulture)}-{_dv}";

    /// <summary>
    /// Formats the RUT with thousands separators: <c>12.345.678-K</c>.
    /// </summary>
    public string ToFormattedString() => $"{_body.ToString("N0", new CultureInfo("es-CL"))}-{_dv}";

    /// <inheritdoc/>
    public override string ToString() => ToCanonicalString();

    /// <inheritdoc/>
    public int CompareTo(Rut other) => _body.CompareTo(other._body);

        /// <summary>
    /// Determines whether the left <see cref="Rut"/> is less than the right <see cref="Rut"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Rut"/> to compare.</param>
    /// <param name="right">The second <see cref="Rut"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(Rut left, Rut right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="Rut"/> is less than or equal to the right <see cref="Rut"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Rut"/> to compare.</param>
    /// <param name="right">The second <see cref="Rut"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(Rut left, Rut right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="Rut"/> is greater than the right <see cref="Rut"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Rut"/> to compare.</param>
    /// <param name="right">The second <see cref="Rut"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(Rut left, Rut right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="Rut"/> is greater than or equal to the right <see cref="Rut"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Rut"/> to compare.</param>
    /// <param name="right">The second <see cref="Rut"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(Rut left, Rut right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public static Rut Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid RUT: '{s}'.");

    /// <inheritdoc/>
    public static Rut Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid RUT: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Rut result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out Rut result) =>
        TryParse(s.AsSpan(), provider, out result);

    /// <inheritdoc/>
    public static Rut Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider = null) =>
        TryParse(utf8Text, provider, out var res) ? res : throw new FormatException("Invalid UTF-8 RUT representation.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out Rut result)
    {
        Span<char> chars = stackalloc char[utf8Text.Length];
        Encoding.UTF8.TryGetChars(utf8Text, chars, out int written);
        return TryParse(chars[..written], provider, out result);
    }
}





