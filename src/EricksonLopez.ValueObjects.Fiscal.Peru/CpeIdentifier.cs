// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Peru;

using EricksonLopez.ValueObjects.Attributes;

/// <summary>
/// Represents a Peruvian Electronic Payment Receipt Unique Natural Key (Identificador de CPE SUNAT),
/// composed of Document Type, 4-character Series, and 1 to 8-digit Correlative Number.
///
/// <para><b>Format:</b> <c>{DocumentTypeCode}-{Series}-{Number:D8}</c> (e.g. <c>01-F001-00000001</c>).</para>
/// </summary>
[RegulatoryRule("DOC.SEQ.003")]
[ValueObject]
public readonly record struct CpeIdentifier : ISpanParsable<CpeIdentifier>, IComparable<CpeIdentifier>
{
    /// <summary>Gets the electronic receipt type code.</summary>
    public CpeTypeCode Type { get; }
    /// <summary>Gets the 4-character electronic series (e.g. "F001").</summary>
    public string Series { get; }
    /// <summary>Gets the 1 to 8-digit correlative receipt number.</summary>
    public int Number { get; }

    private CpeIdentifier(CpeTypeCode type, string series, int number)
    {
        Type = type;
        Series = series;
        Number = number;
    }

    /// <summary>
    /// Creates a validated <see cref="CpeIdentifier"/> from its strongly-typed components.
    /// </summary>
    public static Result<CpeIdentifier> Create(CpeTypeCode type, string? series, int number)
    {
        if (string.IsNullOrWhiteSpace(series))
        {
            return Result<CpeIdentifier>.Failure(Error.Validation(
                "CpeIdentifier.RequiredSeries", "The CPE series is required."));
        }

        string trimmedSeries = series.Trim().ToUpperInvariant();
        if (trimmedSeries.Length != 4)
        {
            return Result<CpeIdentifier>.Failure(Error.Validation(
                "CpeIdentifier.InvalidSeriesLength", "The CPE electronic series must contain exactly 4 alphanumeric characters."));
        }

        if (number is < 1 or > 99_999_999)
        {
            return Result<CpeIdentifier>.Failure(Error.Validation(
                "CpeIdentifier.NumberOutOfRange", $"The CPE correlative number must be between 1 and 99999999. Received: {number.ToString(CultureInfo.InvariantCulture)}."));
        }

        return Result<CpeIdentifier>.Success(new CpeIdentifier(type, trimmedSeries, number));
    }

    /// <summary>
    /// Creates a validated <see cref="CpeIdentifier"/> from a formatted string (e.g. <c>"01-F001-00000001"</c> or <c>"01-F001-1"</c>).
    /// </summary>
    public static Result<CpeIdentifier> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();
        int firstHyphen = trimmed.IndexOf('-');
        if (firstHyphen == -1)
        {
            return Result<CpeIdentifier>.Failure(Error.Validation(
                "CpeIdentifier.InvalidFormat", "The CPE format must be {Type}-{Series}-{Correlative} (e.g. 01-F001-00000001)."));
        }

        ReadOnlySpan<char> typeSpan = trimmed[..firstHyphen];
        var typeResult = CpeTypeCode.Create(typeSpan);
        if (typeResult.IsFailure)
        {
            return Result<CpeIdentifier>.Failure(typeResult.Error);
        }

        ReadOnlySpan<char> remainder = trimmed[(firstHyphen + 1)..];
        int secondHyphen = remainder.IndexOf('-');
        if (secondHyphen == -1)
        {
            return Result<CpeIdentifier>.Failure(Error.Validation(
                "CpeIdentifier.InvalidFormat", "The CPE format must be {Type}-{Series}-{Correlative} (e.g. 01-F001-00000001)."));
        }


        ReadOnlySpan<char> seriesSpan = remainder[..secondHyphen];
        ReadOnlySpan<char> numberSpan = remainder[(secondHyphen + 1)..];

        if (!int.TryParse(numberSpan, CultureInfo.InvariantCulture, out int number))
        {
            return Result<CpeIdentifier>.Failure(Error.Validation(
                "CpeIdentifier.InvalidNumber", "The CPE correlative number must be an integer."));
        }

        return Create(typeResult.Value, seriesSpan.ToString(), number);
    }

    /// <summary>
    /// Creates a validated <see cref="CpeIdentifier"/> from a nullable string.
    /// </summary>
    public static Result<CpeIdentifier> Create(string? input) =>
        Create(input.AsSpan());

    /// <summary>
    /// Formats the CPE identifier in canonical SUNAT format: <c>{Tipo}-{Serie}-{Correlativo:D8}</c>.
    /// </summary>
    public string Canonical => $"{Type.Code}-{Series}-{Number.ToString("D8", CultureInfo.InvariantCulture)}";

    /// <inheritdoc/>
    public override string ToString() => Canonical;

    /// <inheritdoc/>
    public int CompareTo(CpeIdentifier other)
    {
        int typeComp = Type.CompareTo(other.Type);
        if (typeComp != 0) return typeComp;

        int seriesComp = string.Compare(Series, other.Series, StringComparison.Ordinal);
        if (seriesComp != 0) return seriesComp;

        return Number.CompareTo(other.Number);
    }

        /// <summary>
    /// Determines whether the left <see cref="CpeIdentifier"/> is less than the right <see cref="CpeIdentifier"/>.
    /// </summary>
    /// <param name="left">The first <see cref="CpeIdentifier"/> to compare.</param>
    /// <param name="right">The second <see cref="CpeIdentifier"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(CpeIdentifier left, CpeIdentifier right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="CpeIdentifier"/> is less than or equal to the right <see cref="CpeIdentifier"/>.
    /// </summary>
    /// <param name="left">The first <see cref="CpeIdentifier"/> to compare.</param>
    /// <param name="right">The second <see cref="CpeIdentifier"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(CpeIdentifier left, CpeIdentifier right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="CpeIdentifier"/> is greater than the right <see cref="CpeIdentifier"/>.
    /// </summary>
    /// <param name="left">The first <see cref="CpeIdentifier"/> to compare.</param>
    /// <param name="right">The second <see cref="CpeIdentifier"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(CpeIdentifier left, CpeIdentifier right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="CpeIdentifier"/> is greater than or equal to the right <see cref="CpeIdentifier"/>.
    /// </summary>
    /// <param name="left">The first <see cref="CpeIdentifier"/> to compare.</param>
    /// <param name="right">The second <see cref="CpeIdentifier"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(CpeIdentifier left, CpeIdentifier right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public static CpeIdentifier Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid CpeIdentifier: '{s}'.");

    /// <inheritdoc/>
    public static CpeIdentifier Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid CpeIdentifier: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out CpeIdentifier result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out CpeIdentifier result) =>
        TryParse(s.AsSpan(), provider, out result);
}




