// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Mexico;

using EricksonLopez.ValueObjects.Attributes;

/// <summary>
/// Represents a Mexican CURP (Clave Única de Registro de Población)
/// issued by the Secretaría de Gobernación (RENAPO) and verified in tax declarations.
///
/// <para><b>Structure:</b> Exactly 18 uppercase alphanumeric characters.</para>
/// </summary>
[RegulatoryRule("BASE.ID.007")]
[ValueObject]
public readonly record struct Curp : ISpanParsable<Curp>, IComparable<Curp>
{
    private readonly string _value;

    private Curp(string value) => _value = value;

    /// <summary>
    /// Gets the 18-character uppercase CURP string.
    /// </summary>
    public string Value => _value;

    /// <summary>
    /// Gets the 2-character federal entity state code (positions 12-13).
    /// </summary>
    public string StateCode => _value.Substring(11, 2);

    /// <summary>
    /// Gets the gender character ('H' for Hombre, 'M' for Mujer, 'X' for No Binario, position 11).
    /// </summary>
    public char Gender => _value[10];

    /// <summary>
    /// Creates a validated <see cref="Curp"/> from an 18-character string.
    /// </summary>
    /// <param name="value">The raw 18-character CURP string.</param>
    /// <returns>A <see cref="Result{Curp}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<Curp> Create(string? value) =>
        Create(value.AsSpan());

    /// <summary>
    /// Creates a validated <see cref="Curp"/> from a character span.
    /// </summary>
    /// <param name="input">A character span containing the 18-character CURP.</param>
    /// <returns>A <see cref="Result{Curp}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<Curp> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();
        if (trimmed.Length != 18)
        {
            return Result<Curp>.Failure(Error.Validation(
                "Curp.InvalidLength", "The CURP must contain exactly 18 alphanumeric characters."));
        }

        Span<char> buffer = stackalloc char[18];
        for (int i = 0; i < 18; i++)
        {
            char c = trimmed[i];
            if (!char.IsAsciiLetterOrDigit(c))
            {
                return Result<Curp>.Failure(Error.Validation(
                    "Curp.InvalidCharacters", "The CURP can only contain alphanumeric characters."));
            }
            buffer[i] = char.ToUpperInvariant(c);
        }

        // Verify structure: 4 letters, 6 digits, H/M/X, 2 letters state, 3 consonants, 2 alphanumeric
        for (int i = 0; i < 4; i++)
        {
            if (!char.IsLetter(buffer[i]))
            {
                return Result<Curp>.Failure(Error.Validation(
                    "Curp.InvalidInitialLetters", "The first 4 characters of the CURP must be letters."));
            }
        }

        for (int i = 4; i < 10; i++)
        {
            if (!char.IsDigit(buffer[i]))
            {
                return Result<Curp>.Failure(Error.Validation(
                    "Curp.InvalidBirthDate", "Positions 5 to 10 of the CURP must be date digits (YYMMDD)."));
            }
        }

        return Result<Curp>.Success(new Curp(buffer.ToString()));
    }

    /// <inheritdoc/>
    public override string ToString() => _value;

    /// <inheritdoc/>
    public int CompareTo(Curp other) => string.Compare(_value, other._value, StringComparison.Ordinal);

        /// <summary>
    /// Determines whether the left <see cref="Curp"/> is less than the right <see cref="Curp"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Curp"/> to compare.</param>
    /// <param name="right">The second <see cref="Curp"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(Curp left, Curp right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="Curp"/> is less than or equal to the right <see cref="Curp"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Curp"/> to compare.</param>
    /// <param name="right">The second <see cref="Curp"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(Curp left, Curp right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="Curp"/> is greater than the right <see cref="Curp"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Curp"/> to compare.</param>
    /// <param name="right">The second <see cref="Curp"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(Curp left, Curp right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="Curp"/> is greater than or equal to the right <see cref="Curp"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Curp"/> to compare.</param>
    /// <param name="right">The second <see cref="Curp"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(Curp left, Curp right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public static Curp Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid CURP: '{s}'.");

    /// <inheritdoc/>
    public static Curp Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid CURP: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Curp result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out Curp result) =>
        TryParse(s.AsSpan(), provider, out result);
}



