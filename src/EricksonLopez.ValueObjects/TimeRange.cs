// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a time-of-day interval supporting standard and overnight intervals.
/// </summary>
public sealed record TimeRange : ValueObject
{
    private TimeRange(TimeOnly start, TimeOnly end, bool crossesMidnight)
    {
        Start = start;
        End = end;
        CrossesMidnight = crossesMidnight;
    }

    /// <summary>
    /// Gets the starting time of the interval.
    /// </summary>
    public TimeOnly Start { get; }

    /// <summary>
    /// Gets the ending time of the interval.
    /// </summary>
    public TimeOnly End { get; }

    /// <summary>
    /// Gets a value indicating whether this range spans across midnight.
    /// </summary>
    public bool CrossesMidnight { get; }

    /// <summary>
    /// Gets the total elapsed duration of the time range.
    /// </summary>
    public TimeSpan Duration
    {
        get
        {
            TimeSpan start = Start.ToTimeSpan();
            TimeSpan end = End.ToTimeSpan();
            return CrossesMidnight ? TimeSpan.FromDays(1) - start + end : end - start;
        }
    }

    /// <summary>
    /// Creates a validated <see cref="TimeRange"/> instance with specified start and end times.
    /// </summary>
    /// <param name="start">The starting time of the interval.</param>
    /// <param name="end">The ending time of the interval.</param>
    /// <param name="allowOvernight">A value indicating whether overnight intervals (where start is after end) are permitted.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the validated time range, or a validation failure.</returns>
    public static Result<TimeRange> Create(TimeOnly start, TimeOnly end, bool allowOvernight = false)
    {
        if (start < end)
        {
            return Result<TimeRange>.Success(new TimeRange(start, end, crossesMidnight: false));
        }

        if (start > end)
        {
            if (!allowOvernight)
            {
                return Result<TimeRange>.Failure(Error.Validation(
                    "TimeRange.StartAfterEnd",
                    "Time range start must be before end unless overnight ranges are allowed."));
            }

            return Result<TimeRange>.Success(new TimeRange(start, end, crossesMidnight: true));
        }

        return Result<TimeRange>.Failure(Error.Validation(
            "TimeRange.Empty",
            "Time range start and end cannot be the same."));
    }

    /// <summary>
    /// Determines whether the specified time falls within this range.
    /// </summary>
    /// <param name="time">The time to evaluate.</param>
    /// <returns><see langword="true"/> if the time falls within the range; otherwise, <see langword="false"/>.</returns>
    public bool Contains(TimeOnly time)
    {
        return CrossesMidnight
            ? time >= Start || time < End
            : time >= Start && time < End;
    }

    /// <summary>
    /// Determines whether this time range overlaps with another time range.
    /// </summary>
    /// <param name="other">The other time range to check for overlap.</param>
    /// <returns><see langword="true"/> if the ranges overlap; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/></exception>
    public bool Overlaps(TimeRange other)
    {
        ArgumentNullException.ThrowIfNull(other);

        if (CrossesMidnight && other.CrossesMidnight)
        {
            return true;
        }

        if (CrossesMidnight)
        {
            return other.End > Start || other.Start < End;
        }

        if (other.CrossesMidnight)
        {
            return End > other.Start || Start < other.End;
        }

        return Start < other.End && other.Start < End;
    }

    /// <inheritdoc/>
    public override string ToString() => $"[{Start:HH:mm:ss} .. {End:HH:mm:ss}]";
}

