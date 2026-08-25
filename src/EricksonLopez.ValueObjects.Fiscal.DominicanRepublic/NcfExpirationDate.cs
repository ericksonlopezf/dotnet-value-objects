// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;

/// <summary>
/// Represents the statutory expiration date of DGII-authorized NCF Serie B sequences.
/// Under DGII regulations, traditional sequences expire on December 31st of the year following authorization.
/// Immutable, allocation-free readonly struct.
/// </summary>
public readonly record struct NcfExpirationDate : IValueObject<NcfExpirationDate>, IComparable<NcfExpirationDate>, IComparable
{
    /// <summary>Gets the statutory expiration date.</summary>
    public DateOnly Value { get; }

    private NcfExpirationDate(DateOnly value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates an <see cref="NcfExpirationDate"/> from a specific <see cref="DateOnly"/>.
    /// </summary>
    public static Result<NcfExpirationDate> Create(DateOnly value)
    {
        if (value == DateOnly.MinValue || value == DateOnly.MaxValue)
        {
            return Result<NcfExpirationDate>.Failure(Error.Validation(
                "NcfExpirationDate.OutOfRange", "Expiration date cannot be MinValue or MaxValue."));
        }

        return Result<NcfExpirationDate>.Success(new NcfExpirationDate(value));
    }

    /// <summary>
    /// Calculates the standard statutory DGII expiration date from the authorization year
    /// (December 31st of the following calendar year).
    /// </summary>
    public static NcfExpirationDate FromAuthorizationYear(int authorizationYear) =>
        new(new DateOnly(authorizationYear + 1, 12, 31));

    /// <summary>
    /// Checks whether this sequence has expired relative to the given date.
    /// </summary>
    public bool IsExpired(DateOnly currentDate) => currentDate > Value;

    /// <inheritdoc/>
    public int CompareTo(NcfExpirationDate other) => Value.CompareTo(other.Value);

    /// <inheritdoc/>
    public int CompareTo(object? obj) =>
        obj is NcfExpirationDate other ? CompareTo(other) : throw new ArgumentException("Object is not an NcfExpirationDate", nameof(obj));

        /// <summary>
    /// Determines whether the left <see cref="NcfExpirationDate"/> is less than the right <see cref="NcfExpirationDate"/>.
    /// </summary>
    /// <param name="left">The first <see cref="NcfExpirationDate"/> to compare.</param>
    /// <param name="right">The second <see cref="NcfExpirationDate"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(NcfExpirationDate left, NcfExpirationDate right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="NcfExpirationDate"/> is less than or equal to the right <see cref="NcfExpirationDate"/>.
    /// </summary>
    /// <param name="left">The first <see cref="NcfExpirationDate"/> to compare.</param>
    /// <param name="right">The second <see cref="NcfExpirationDate"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(NcfExpirationDate left, NcfExpirationDate right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="NcfExpirationDate"/> is greater than the right <see cref="NcfExpirationDate"/>.
    /// </summary>
    /// <param name="left">The first <see cref="NcfExpirationDate"/> to compare.</param>
    /// <param name="right">The second <see cref="NcfExpirationDate"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(NcfExpirationDate left, NcfExpirationDate right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="NcfExpirationDate"/> is greater than or equal to the right <see cref="NcfExpirationDate"/>.
    /// </summary>
    /// <param name="left">The first <see cref="NcfExpirationDate"/> to compare.</param>
    /// <param name="right">The second <see cref="NcfExpirationDate"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(NcfExpirationDate left, NcfExpirationDate right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public override string ToString() => Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
}

