// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Colombia;

/// <summary>
/// Represents an immutable numbering authorization range granted by the DIAN for electronic invoicing.
///
/// <para><b>Rules:</b>
/// <list type="bullet">
///   <item><description><see cref="From"/> must be strictly greater than zero.</description></item>
///   <item><description><see cref="To"/> must be greater than or equal to <see cref="From"/>.</description></item>
/// </list>
/// </para>
/// </summary>
[ValueObject]
public readonly record struct AuthorizationRange : IEquatable<AuthorizationRange>
{
    /// <summary>Gets the starting sequence number of the authorized range.</summary>
    public long From { get; }
    /// <summary>Gets the ending sequence number of the authorized range.</summary>
    public long To { get; }

    private AuthorizationRange(long from, long to)
    {
        From = from;
        To = to;
    }

    /// <summary>
    /// Creates a validated <see cref="AuthorizationRange"/> instance.
    /// </summary>
    /// <param name="from">The starting authorized sequence number.</param>
    /// <param name="to">The ending authorized sequence number.</param>
    public static Result<AuthorizationRange> Create(long from, long to)
    {
        if (from <= 0)
        {
            return Result<AuthorizationRange>.Failure(Error.Validation(
                "AuthorizationRange.InvalidFrom", "The initial range sequence number must be greater than zero."));
        }

        if (to < from)
        {
            return Result<AuthorizationRange>.Failure(Error.Validation(
                "AuthorizationRange.InvalidTo", $"The end range number ({to}) cannot be less than the starting number ({from})."));
        }

        return Result<AuthorizationRange>.Success(new AuthorizationRange(from, to));
    }

    /// <summary>
    /// Determines whether a specific invoice sequence number is within this authorized range.
    /// </summary>
    public bool Contains(long sequenceNumber) => sequenceNumber >= From && sequenceNumber <= To;

    /// <summary>
    /// Gets the total capacity of sequences in this range.
    /// </summary>
    public long TotalCount => To - From + 1;

    /// <inheritdoc/>
    public override string ToString() => $"[{From}..{To}]";
}



