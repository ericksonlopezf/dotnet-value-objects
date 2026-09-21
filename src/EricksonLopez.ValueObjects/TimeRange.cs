// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents an immutable time-of-day interval supporting standard and overnight intervals.
/// </summary>
public readonly record struct TimeRange : IValueObject<TimeRange>, IComparable<TimeRange>, IComparable, IParsable<TimeRange>, ISpanParsable<TimeRange>
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
    /// Gets a value indicating whether this time range is degenerate (i.e. <see cref="Start"/> equals <see cref="End"/> and does not span across midnight).
    /// </summary>
    public bool IsEmpty => Start == End && !CrossesMidnight;

    /// <summary>
    /// Gets a value indicating whether this instance has been explicitly initialized and does not represent the default struct state.
    /// </summary>
    public bool IsInitialized => !IsEmpty;

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
    public bool Overlaps(TimeRange other)
    {
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

    /// <summary>
    /// Compares this time range with another time range.
    /// </summary>
    /// <remarks>
    /// Comparison is first by <see cref="Start"/>; when start times are equal, comparison falls back to <see cref="End"/>.
    /// </remarks>
    /// <param name="other">The other time range to compare against.</param>
    /// <returns>A value indicating the relative order of the time ranges being compared.</returns>
    public int CompareTo(TimeRange other)
    {
        int startComparison = Start.CompareTo(other.Start);
        return startComparison != 0 ? startComparison : End.CompareTo(other.End);
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException"><paramref name="obj"/> is not of type <see cref="TimeRange"/></exception>
    public int CompareTo(object? obj) =>
        obj is TimeRange other ? CompareTo(other) : throw new ArgumentException("Object is not a TimeRange", nameof(obj));

    /// <summary>
    /// Determines whether the left time range is earlier than the right time range.
    /// </summary>
    public static bool operator <(TimeRange left, TimeRange right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left time range is earlier than or equal to the right time range.
    /// </summary>
    public static bool operator <=(TimeRange left, TimeRange right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left time range is later than the right time range.
    /// </summary>
    public static bool operator >(TimeRange left, TimeRange right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left time range is later than or equal to the right time range.
    /// </summary>
    public static bool operator >=(TimeRange left, TimeRange right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public override string ToString() => $"[{Start:HH:mm:ss} .. {End:HH:mm:ss}]";

    /// <summary>
    /// Parses a string into a <see cref="TimeRange"/>.
    /// </summary>
    /// <param name="s">The string to parse (e.g. <c>"[08:00:00 .. 17:00:00]"</c> or <c>"22:00:00..06:00:00"</c>).</param>
    /// <param name="provider">An optional format provider.</param>
    /// <returns>The parsed <see cref="TimeRange"/>.</returns>
    /// <exception cref="FormatException"><paramref name="s"/> is not in a valid format</exception>
    public static TimeRange Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid TimeRange: '{s}'.");

    /// <summary>
    /// Parses a span of characters into a <see cref="TimeRange"/>.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <returns>The parsed <see cref="TimeRange"/>.</returns>
    public static TimeRange Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid TimeRange: '{s.ToString()}'.");

    /// <summary>
    /// Attempts to parse a string into a <see cref="TimeRange"/>.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <param name="result">When this method returns, contains the parsed range if successful; otherwise, default.</param>
    /// <returns><see langword="true"/> if parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(string? s, IFormatProvider? provider, out TimeRange result) =>
        TryParse(s.AsSpan(), provider, out result);

    /// <summary>
    /// Attempts to parse a span of characters into a <see cref="TimeRange"/>.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <param name="result">When this method returns, contains the parsed range if successful; otherwise, default.</param>
    /// <returns><see langword="true"/> if parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out TimeRange result)
    {
        ReadOnlySpan<char> trimmed = s.Trim();
        if (trimmed.StartsWith("[", StringComparison.Ordinal) && trimmed.EndsWith("]", StringComparison.Ordinal))
        {
            trimmed = trimmed[1..^1].Trim();
        }

        int separatorIndex = trimmed.IndexOf("..", StringComparison.Ordinal);
        int separatorLen = 2;
        if (separatorIndex < 0)
        {
            separatorIndex = trimmed.IndexOf('/');
            separatorLen = 1;
        }

        if (separatorIndex < 0)
        {
            result = default;
            return false;
        }

        ReadOnlySpan<char> startSpan = trimmed[..separatorIndex].Trim();
        ReadOnlySpan<char> endSpan = trimmed[(separatorIndex + separatorLen)..].Trim();

        if (!TimeOnly.TryParse(startSpan, provider, DateTimeStyles.None, out var start) ||
            !TimeOnly.TryParse(endSpan, provider, DateTimeStyles.None, out var end))
        {
            result = default;
            return false;
        }

        var res = Create(start, end, allowOvernight: true);
        if (res.IsSuccess)
        {
            result = res.Value;
            return true;
        }

        result = default;
        return false;
    }
}

