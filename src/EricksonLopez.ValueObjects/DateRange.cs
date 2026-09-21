// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents an inclusive calendar date range defined by start and end dates.
/// </summary>
/// <remarks>
/// Comparison is performed by <see cref="Start"/> date first; when start dates are equal,
/// comparison is determined by <see cref="End"/> date.
/// </remarks>
public readonly record struct DateRange : IValueObject<DateRange>, IComparable<DateRange>, IComparable, IParsable<DateRange>, ISpanParsable<DateRange>
{
    /// <summary>
    /// Gets the starting date of the range.
    /// </summary>
    public DateOnly Start { get; }

    /// <summary>
    /// Gets the ending date of the range.
    /// </summary>
    public DateOnly End { get; }

    /// <summary>
    /// Gets the total number of calendar days spanned by the range (inclusive).
    /// </summary>
    public int DurationInDays => End.DayNumber - Start.DayNumber + 1;

    /// <summary>
    /// Gets the inclusive number of calendar days spanned by the range (equivalent to <see cref="DurationInDays"/>).
    /// </summary>
    public int InclusiveDays => DurationInDays;

    /// <summary>
    /// Gets the difference in calendar days between start and end (exclusive of boundary point).
    /// </summary>
    public int DaysDifference => End.DayNumber - Start.DayNumber;

    /// <summary>
    /// Gets a value indicating whether this instance has been explicitly initialized and does not represent the default struct state.
    /// </summary>
    public bool IsInitialized => Start != DateOnly.MinValue;

    private DateRange(DateOnly start, DateOnly end)
    {
        Start = start;
        End = end;
    }

    /// <summary>
    /// Creates a validated <see cref="DateRange"/> instance with specified start and end dates.
    /// </summary>
    /// <param name="start">The start date of the range.</param>
    /// <param name="end">The end date of the range.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the validated date range, or a validation failure.</returns>
    public static Result<DateRange> Create(DateOnly start, DateOnly end)
    {
        if (start == DateOnly.MinValue || end == DateOnly.MaxValue)
        {
            return Result<DateRange>.Failure(Error.Validation(
                "DateRange.OutOfRange",
                "Date range cannot use DateOnly.MinValue as start or DateOnly.MaxValue as end."));
        }

        if (start > end)
        {
            return Result<DateRange>.Failure(Error.Validation(
                "DateRange.StartAfterEnd",
                "Date range start must be before or equal to end."));
        }

        return Result<DateRange>.Success(new DateRange(start, end));
    }

    /// <summary>
    /// Determines whether the specified date is contained within this date range.
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <returns><see langword="true"/> if the date is within the inclusive range; otherwise, <see langword="false"/>.</returns>
    public bool Contains(DateOnly date) => date >= Start && date <= End;

    /// <summary>
    /// Determines whether this date range overlaps with another date range.
    /// </summary>
    /// <param name="other">The other date range to check for overlap.</param>
    /// <returns><see langword="true"/> if the ranges overlap; otherwise, <see langword="false"/>.</returns>
    public bool Overlaps(DateRange other) => Start <= other.End && other.Start <= End;

    /// <summary>
    /// Compares this date range with another date range.
    /// </summary>
    /// <remarks>
    /// Comparison is first by <see cref="Start"/>; when start dates are equal, comparison falls back to <see cref="End"/>.
    /// </remarks>
    /// <param name="other">The other date range to compare against.</param>
    /// <returns>A value indicating the relative order of the date ranges being compared.</returns>
    public int CompareTo(DateRange other)
    {
        int startComparison = Start.CompareTo(other.Start);
        return startComparison != 0 ? startComparison : End.CompareTo(other.End);
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException"><paramref name="obj"/> is not of type <see cref="DateRange"/></exception>
    public int CompareTo(object? obj) =>
        obj is DateRange other ? CompareTo(other) : throw new ArgumentException("Object is not a DateRange", nameof(obj));

    /// <summary>
    /// Determines whether the left date range is earlier than the right date range.
    /// </summary>
    /// <param name="left">The first date range to compare.</param>
    /// <param name="right">The second date range to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is earlier than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(DateRange left, DateRange right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left date range is earlier than or equal to the right date range.
    /// </summary>
    /// <param name="left">The first date range to compare.</param>
    /// <param name="right">The second date range to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is earlier than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(DateRange left, DateRange right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left date range is later than the right date range.
    /// </summary>
    /// <param name="left">The first date range to compare.</param>
    /// <param name="right">The second date range to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is later than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(DateRange left, DateRange right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left date range is later than or equal to the right date range.
    /// </summary>
    /// <param name="left">The first date range to compare.</param>
    /// <param name="right">The second date range to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is later than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(DateRange left, DateRange right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public override string ToString() => $"[{Start:yyyy-MM-dd} .. {End:yyyy-MM-dd}]";

    /// <summary>
    /// Parses a string into a <see cref="DateRange"/>.
    /// </summary>
    /// <param name="s">The string to parse (e.g. <c>"[2026-01-01 .. 2026-12-31]"</c> or <c>"2026-01-01..2026-12-31"</c>).</param>
    /// <param name="provider">An optional format provider.</param>
    /// <returns>The parsed <see cref="DateRange"/>.</returns>
    /// <exception cref="FormatException"><paramref name="s"/> is not in a valid format</exception>
    public static DateRange Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid DateRange: '{s}'.");

    /// <summary>
    /// Parses a span of characters into a <see cref="DateRange"/>.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <returns>The parsed <see cref="DateRange"/>.</returns>
    public static DateRange Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid DateRange: '{s.ToString()}'.");

    /// <summary>
    /// Attempts to parse a string into a <see cref="DateRange"/>.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <param name="result">When this method returns, contains the parsed range if successful; otherwise, default.</param>
    /// <returns><see langword="true"/> if parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(string? s, IFormatProvider? provider, out DateRange result) =>
        TryParse(s.AsSpan(), provider, out result);

    /// <summary>
    /// Attempts to parse a span of characters into a <see cref="DateRange"/>.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="provider">An optional format provider.</param>
    /// <param name="result">When this method returns, contains the parsed range if successful; otherwise, default.</param>
    /// <returns><see langword="true"/> if parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out DateRange result)
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

        if (!DateOnly.TryParse(startSpan, provider, DateTimeStyles.None, out var start) ||
            !DateOnly.TryParse(endSpan, provider, DateTimeStyles.None, out var end))
        {
            result = default;
            return false;
        }

        var res = Create(start, end);
        if (res.IsSuccess)
        {
            result = res.Value;
            return true;
        }

        result = default;
        return false;
    }
}


