// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Mexico;

/// <summary>
/// Represents a Mexican Carta Porte 3.1 Identifier (IdCCP),
/// mandated by the SAT for goods transportation CFDI supplements.
///
/// <para><b>Structure:</b> Exactly 39 characters starting with the fixed prefix <c>"CCC"</c>
/// followed by a valid 36-character uppercase RFC 4122 UUID v4 (<c>CCCXXXXXXXX-XXXX-4XXX-YXXX-XXXXXXXXXXXX</c>).</para>
/// </summary>
[ValueObject]
public readonly record struct IdCcp : ISpanParsable<IdCcp>, IComparable<IdCcp>
{
    /// <summary>
    /// Represents the fixed 3-character prefix ("CCC") for Carta Porte 3.1 identifiers.
    /// </summary>
    public const string Prefix = "CCC";

    private readonly FiscalUuid _uuid;

    private IdCcp(FiscalUuid uuid) => _uuid = uuid;

    /// <summary>
    /// Gets the underlying <see cref="FiscalUuid"/>.
    /// </summary>
    public FiscalUuid Uuid => _uuid;

    /// <summary>
    /// Creates a validated <see cref="IdCcp"/> from a <see cref="FiscalUuid"/>.
    /// </summary>
    public static Result<IdCcp> Create(FiscalUuid uuid) =>
        Result<IdCcp>.Success(new IdCcp(uuid));

    /// <summary>
    /// Creates a validated <see cref="IdCcp"/> from a 39-character string.
    /// </summary>
    public static Result<IdCcp> Create(string? value) =>
        Create(value.AsSpan());

    /// <summary>
    /// Creates a validated <see cref="IdCcp"/> from a character span.
    /// </summary>
    public static Result<IdCcp> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();
        if (trimmed.Length != 39)
        {
            return Result<IdCcp>.Failure(Error.Validation(
                "IdCcp.InvalidLength", "The Carta Porte IdCCP must contain exactly 39 characters (CCC prefix + 36-character UUID)."));
        }

        if (!trimmed.StartsWith(Prefix, StringComparison.OrdinalIgnoreCase))
        {
            return Result<IdCcp>.Failure(Error.Validation(
                "IdCcp.InvalidPrefix", "The IdCCP must start with the 'CCC' prefix."));
        }

        ReadOnlySpan<char> uuidSpan = trimmed[3..];
        var uuidResult = FiscalUuid.Create(uuidSpan);
        if (uuidResult.IsFailure)
        {
            return Result<IdCcp>.Failure(uuidResult.Error);
        }

        return Result<IdCcp>.Success(new IdCcp(uuidResult.Value));
    }

    /// <summary>
    /// Formats the IdCCP in canonical format: <c>CCCXXXXXXXX-XXXX-4XXX-YXXX-XXXXXXXXXXXX</c>.
    /// </summary>
    public string Formatted => $"{Prefix}{_uuid.Formatted}";

    /// <inheritdoc/>
    public override string ToString() => Formatted;

    /// <inheritdoc/>
    public int CompareTo(IdCcp other) => _uuid.CompareTo(other._uuid);

        /// <summary>
    /// Determines whether the left <see cref="IdCcp"/> is less than the right <see cref="IdCcp"/>.
    /// </summary>
    /// <param name="left">The first <see cref="IdCcp"/> to compare.</param>
    /// <param name="right">The second <see cref="IdCcp"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(IdCcp left, IdCcp right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="IdCcp"/> is less than or equal to the right <see cref="IdCcp"/>.
    /// </summary>
    /// <param name="left">The first <see cref="IdCcp"/> to compare.</param>
    /// <param name="right">The second <see cref="IdCcp"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(IdCcp left, IdCcp right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="IdCcp"/> is greater than the right <see cref="IdCcp"/>.
    /// </summary>
    /// <param name="left">The first <see cref="IdCcp"/> to compare.</param>
    /// <param name="right">The second <see cref="IdCcp"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(IdCcp left, IdCcp right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="IdCcp"/> is greater than or equal to the right <see cref="IdCcp"/>.
    /// </summary>
    /// <param name="left">The first <see cref="IdCcp"/> to compare.</param>
    /// <param name="right">The second <see cref="IdCcp"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(IdCcp left, IdCcp right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public static IdCcp Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid IdCcp: '{s}'.");

    /// <inheritdoc/>
    public static IdCcp Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid IdCcp: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out IdCcp result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out IdCcp result) =>
        TryParse(s.AsSpan(), provider, out result);
}



