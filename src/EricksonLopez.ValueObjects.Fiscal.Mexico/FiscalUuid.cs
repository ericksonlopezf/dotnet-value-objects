// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Mexico;

using EricksonLopez.ValueObjects.Attributes;

/// <summary>
/// Represents a Mexican SAT CFDI Fiscal UUID (Folio Fiscal Timbrado por PAC/SAT, Anexo 20).
///
/// <para><b>Structure:</b> Exactly 36 uppercase characters representing a standard RFC 4122 UUID v4:
/// <c>XXXXXXXX-XXXX-4XXX-YXXX-XXXXXXXXXXXX</c>.</para>
/// </summary>
[RegulatoryRule("DOC.SEQ.004")]
[ValueObject]
public readonly record struct FiscalUuid : ISpanParsable<FiscalUuid>, IComparable<FiscalUuid>
{
    private readonly Guid _value;

    private FiscalUuid(Guid value) => _value = value;

    /// <summary>
    /// Gets the underlying <see cref="Guid"/> value.
    /// </summary>
    public Guid Value => _value;

    /// <summary>
    /// Creates a validated <see cref="FiscalUuid"/> from an existing <see cref="Guid"/>.
    /// </summary>
    /// <param name="value">The <see cref="Guid"/> representing the fiscal UUID. Must not be <see cref="Guid.Empty"/>.</param>
    /// <returns>A <see cref="Result{FiscalUuid}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<FiscalUuid> Create(Guid value)
    {
        if (value == Guid.Empty)
        {
            return Result<FiscalUuid>.Failure(Error.Validation(
                "FiscalUuid.Empty", "The Fiscal Folio cannot be an empty UUID (Guid.Empty)."));
        }

        return Result<FiscalUuid>.Success(new FiscalUuid(value));
    }

    /// <summary>
    /// Creates a validated <see cref="FiscalUuid"/> from a 36-character UUID string.
    /// </summary>
    /// <param name="value">A string containing the UUID in standard RFC 4122 format.</param>
    /// <returns>A <see cref="Result{FiscalUuid}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<FiscalUuid> Create(string? value) =>
        Create(value.AsSpan());

    /// <summary>
    /// Creates a validated <see cref="FiscalUuid"/> from a character span.
    /// </summary>
    /// <param name="input">A character span containing the UUID in standard RFC 4122 format.</param>
    /// <returns>A <see cref="Result{FiscalUuid}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<FiscalUuid> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();
        if (!Guid.TryParse(trimmed, out Guid parsedGuid))
        {
            return Result<FiscalUuid>.Failure(Error.Validation(
                "FiscalUuid.InvalidFormat", "The Fiscal Folio must be a valid UUID in standard format (e.g. XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX)."));
        }

        return Create(parsedGuid);
    }

    /// <summary>
    /// Formats the Fiscal UUID in canonical uppercase format.
    /// </summary>
    public string Formatted => _value.ToString().ToUpperInvariant();


    /// <inheritdoc/>
    public override string ToString() => Formatted;

    /// <inheritdoc/>
    public int CompareTo(FiscalUuid other) => _value.CompareTo(other._value);

        /// <summary>
    /// Determines whether the left <see cref="FiscalUuid"/> is less than the right <see cref="FiscalUuid"/>.
    /// </summary>
    /// <param name="left">The first <see cref="FiscalUuid"/> to compare.</param>
    /// <param name="right">The second <see cref="FiscalUuid"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(FiscalUuid left, FiscalUuid right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="FiscalUuid"/> is less than or equal to the right <see cref="FiscalUuid"/>.
    /// </summary>
    /// <param name="left">The first <see cref="FiscalUuid"/> to compare.</param>
    /// <param name="right">The second <see cref="FiscalUuid"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(FiscalUuid left, FiscalUuid right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="FiscalUuid"/> is greater than the right <see cref="FiscalUuid"/>.
    /// </summary>
    /// <param name="left">The first <see cref="FiscalUuid"/> to compare.</param>
    /// <param name="right">The second <see cref="FiscalUuid"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(FiscalUuid left, FiscalUuid right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="FiscalUuid"/> is greater than or equal to the right <see cref="FiscalUuid"/>.
    /// </summary>
    /// <param name="left">The first <see cref="FiscalUuid"/> to compare.</param>
    /// <param name="right">The second <see cref="FiscalUuid"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(FiscalUuid left, FiscalUuid right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public static FiscalUuid Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid FiscalUuid: '{s}'.");

    /// <inheritdoc/>
    public static FiscalUuid Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid FiscalUuid: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out FiscalUuid result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out FiscalUuid result) =>
        TryParse(s.AsSpan(), provider, out result);
}



