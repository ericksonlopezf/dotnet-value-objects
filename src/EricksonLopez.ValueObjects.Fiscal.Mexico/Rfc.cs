// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using System.Text;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Mexico;

using EricksonLopez.ValueObjects.Attributes;

/// <summary>
/// Represents a Mexican RFC (Registro Federal de Contribuyentes)
/// issued by the SAT (Servicio de Administración Tributaria, CFF Art. 27).
///
/// <para><b>Structure:</b>
/// <list type="bullet">
///   <item><term>Personas Morales (12 chars)</term><description>3 letters + 6 digits (YYMMDD) + 3 homoclave characters (e.g. <c>"ABC680524P-76"</c> without hyphen -> <c>"ABC680524P76"</c>).</description></item>
///   <item><term>Personas Físicas (13 chars)</term><description>4 letters + 6 digits (YYMMDD) + 3 homoclave characters (e.g. <c>"GODE561231GR8"</c>).</description></item>
///   <item><term>Genéricos</term><description><c>"XAXX010101000"</c> (Público en General), <c>"XEXX010101000"</c> (Extranjeros).</description></item>
/// </list>
/// </para>
/// </summary>
[RegulatoryRule("MX.RFC.001")]
[ValueObject]
public readonly record struct Rfc : ISpanParsable<Rfc>, IUtf8SpanParsable<Rfc>, IComparable<Rfc>
{
    /// <summary>Generic RFC constant for Mexican national general public (XAXX010101000).</summary>
    public const string GenericNational = "XAXX010101000";
    /// <summary>Generic RFC constant for foreign customers (XEXX010101000).</summary>
    public const string GenericForeigner = "XEXX010101000";

    private readonly string _value;

    private Rfc(string value) => _value = value;

    /// <summary>
    /// Gets the uppercase RFC value.
    /// </summary>
    public string Value => _value;

    /// <summary>
    /// Gets a value indicating whether this RFC corresponds to a physical person (13 characters).
    /// </summary>
    public bool IsIndividual => _value.Length == 13;

    /// <summary>
    /// Gets a value indicating whether this RFC corresponds to a legal entity (12 characters).
    /// </summary>
    public bool IsCompany => _value.Length == 12;

    /// <summary>
    /// Gets a value indicating whether this RFC is the generic national public-in-general RFC (<c>XAXX010101000</c>).
    /// </summary>
    public bool IsGenericNational => _value == GenericNational;

    /// <summary>
    /// Gets a value indicating whether this RFC is the generic foreign customer RFC (<c>XEXX010101000</c>).
    /// </summary>
    public bool IsGenericForeigner => _value == GenericForeigner;

    /// <summary>
    /// Creates a validated <see cref="Rfc"/> from a 12 or 13-character string.
    /// </summary>
    public static Result<Rfc> Create(string? value) =>
        Create(value.AsSpan());

    /// <summary>
    /// Creates a validated <see cref="Rfc"/> from a character span.
    /// </summary>
    public static Result<Rfc> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();
        if (trimmed.Length is not (12 or 13))
        {
            return Result<Rfc>.Failure(Error.Validation(
                "Rfc.InvalidLength", "The RFC must contain exactly 12 characters (legal entity) or 13 characters (natural person)."));
        }

        Span<char> buffer = stackalloc char[trimmed.Length];
        for (int i = 0; i < trimmed.Length; i++)
        {
            char c = trimmed[i];
            if (!char.IsAsciiLetterOrDigit(c) && c != '&' && c != 'Ñ' && c != 'ñ')
            {
                return Result<Rfc>.Failure(Error.Validation(
                    "Rfc.InvalidCharacters", "The RFC contains characters not permitted by the SAT."));
            }
            buffer[i] = char.ToUpperInvariant(c);
        }

        int letterCount = buffer.Length == 12 ? 3 : 4;

        // Verify initial letters
        for (int i = 0; i < letterCount; i++)
        {
            char c = buffer[i];
            if (!char.IsLetter(c) && c != '&' && c != 'Ñ')
            {
                return Result<Rfc>.Failure(Error.Validation(
                    "Rfc.InvalidInitialLetters", $"The first {letterCount.ToString(CultureInfo.InvariantCulture)} characters of the RFC must be letters."));
            }
        }

        // Verify 6 numeric date digits
        for (int i = letterCount; i < letterCount + 6; i++)
        {
            if (!char.IsDigit(buffer[i]))
            {
                return Result<Rfc>.Failure(Error.Validation(
                    "Rfc.InvalidDateDigits", "The date portion of the RFC (positions 4-9 or 5-10) must contain 6 numeric digits (YYMMDD)."));
            }
        }

        return Result<Rfc>.Success(new Rfc(buffer.ToString()));
    }

    /// <inheritdoc/>
    public override string ToString() => _value;

    /// <inheritdoc/>
    public int CompareTo(Rfc other) => string.Compare(_value, other._value, StringComparison.Ordinal);

        /// <summary>
    /// Determines whether the left <see cref="Rfc"/> is less than the right <see cref="Rfc"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Rfc"/> to compare.</param>
    /// <param name="right">The second <see cref="Rfc"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(Rfc left, Rfc right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="Rfc"/> is less than or equal to the right <see cref="Rfc"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Rfc"/> to compare.</param>
    /// <param name="right">The second <see cref="Rfc"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(Rfc left, Rfc right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="Rfc"/> is greater than the right <see cref="Rfc"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Rfc"/> to compare.</param>
    /// <param name="right">The second <see cref="Rfc"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(Rfc left, Rfc right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="Rfc"/> is greater than or equal to the right <see cref="Rfc"/>.
    /// </summary>
    /// <param name="left">The first <see cref="Rfc"/> to compare.</param>
    /// <param name="right">The second <see cref="Rfc"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(Rfc left, Rfc right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public static Rfc Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid RFC: '{s}'.");

    /// <inheritdoc/>
    public static Rfc Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid RFC: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Rfc result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out Rfc result) =>
        TryParse(s.AsSpan(), provider, out result);

    /// <inheritdoc/>
    public static Rfc Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider = null) =>
        TryParse(utf8Text, provider, out var res) ? res : throw new FormatException("Invalid UTF-8 RFC representation.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out Rfc result)
    {
        Span<char> chars = stackalloc char[utf8Text.Length];
        Encoding.UTF8.TryGetChars(utf8Text, chars, out int written);
        return TryParse(chars[..written], provider, out result);
    }
}





