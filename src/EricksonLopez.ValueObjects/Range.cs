// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents an immutable interval value object defined by inclusive start and end boundaries.
/// </summary>
/// <remarks>
/// Guarantees the invariant that <see cref="Start"/> is less than or equal to <see cref="End"/>.
/// </remarks>
/// <typeparam name="T">The scalar type defining the interval boundaries.</typeparam>
public readonly record struct Range<T> : IValueObject, IComparable<Range<T>>, IComparable
    where T : struct, IComparable<T>, IEquatable<T>
{
    /// <summary>
    /// Gets the lower inclusive boundary of the range.
    /// </summary>
    public T Start { get; }

    /// <summary>
    /// Gets the upper inclusive boundary of the range.
    /// </summary>
    public T End { get; }

    private Range(T start, T end)
    {
        Start = start;
        End = end;
    }

    /// <summary>
    /// Creates a validated <see cref="Range{T}"/> instance with the specified inclusive bounds.
    /// </summary>
    /// <param name="start">The lower boundary of the range.</param>
    /// <param name="end">The upper boundary of the range.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the created range, or a validation error if <paramref name="start"/> is greater than <paramref name="end"/>.</returns>
    public static Result<Range<T>> Create(T start, T end)
    {
        if (start.CompareTo(end) > 0)
        {
            return Result<Range<T>>.Failure(
                Error.Validation("Range.InvalidBounds", $"Start '{start}' cannot be greater than End '{end}'."));
        }

        return Result<Range<T>>.Success(new Range<T>(start, end));
    }

    /// <summary>
    /// Determines whether the specified value is contained within the range.
    /// </summary>
    /// <param name="value">The value to test for containment.</param>
    /// <returns><see langword="true"/> if the value falls within the inclusive range boundaries; otherwise, <see langword="false"/>.</returns>
    public bool Contains(T value) =>
        value.CompareTo(Start) >= 0 && value.CompareTo(End) <= 0;

    /// <summary>
    /// Determines whether another range is entirely contained within this range.
    /// </summary>
    /// <param name="other">The other range to evaluate.</param>
    /// <returns><see langword="true"/> if <paramref name="other"/> is completely contained within this range; otherwise, <see langword="false"/>.</returns>
    public bool Contains(Range<T> other) =>
        other.Start.CompareTo(Start) >= 0 && other.End.CompareTo(End) <= 0;

    /// <summary>
    /// Determines whether this range overlaps with another range.
    /// </summary>
    /// <param name="other">The other range to check for overlap.</param>
    /// <returns><see langword="true"/> if the ranges share at least one common value; otherwise, <see langword="false"/>.</returns>
    public bool Overlaps(Range<T> other) =>
        Start.CompareTo(other.End) <= 0 && End.CompareTo(other.Start) >= 0;

    /// <summary>
    /// Calculates the intersection between this range and another range, if one exists.
    /// </summary>
    /// <param name="other">The other range to intersect with.</param>
    /// <param name="intersection">When this method returns, contains the overlapping range if an intersection exists; otherwise, the default value.</param>
    /// <returns><see langword="true"/> if an intersection exists; otherwise, <see langword="false"/>.</returns>
    public bool Intersects(Range<T> other, out Range<T> intersection)
    {
        if (!Overlaps(other))
        {
            intersection = default;
            return false;
        }

        var maxStart = Start.CompareTo(other.Start) > 0 ? Start : other.Start;
        var minEnd = End.CompareTo(other.End) < 0 ? End : other.End;

        intersection = new Range<T>(maxStart, minEnd);
        return true;
    }

    /// <summary>
    /// Compares this range with another range based on start boundary, followed by end boundary.
    /// </summary>
    /// <param name="other">The other range to compare against.</param>
    /// <returns>A value indicating the relative order of the ranges being compared.</returns>
    public int CompareTo(Range<T> other)
    {
        var startComparison = Start.CompareTo(other.Start);
        return startComparison != 0 ? startComparison : End.CompareTo(other.End);
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException"><paramref name="obj"/> is not of type <see cref="Range{T}"/></exception>
    public int CompareTo(object? obj)
    {
        if (obj is null) return 1;
        if (obj is Range<T> other) return CompareTo(other);
        throw new ArgumentException($"Object must be of type Range<{typeof(T).Name}>", nameof(obj));
    }

    /// <summary>
    /// Determines whether the left range is less than the right range.
    /// </summary>
    /// <param name="left">The first range to compare.</param>
    /// <param name="right">The second range to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(Range<T> left, Range<T> right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left range is less than or equal to the right range.
    /// </summary>
    /// <param name="left">The first range to compare.</param>
    /// <param name="right">The second range to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(Range<T> left, Range<T> right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left range is greater than the right range.
    /// </summary>
    /// <param name="left">The first range to compare.</param>
    /// <param name="right">The second range to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(Range<T> left, Range<T> right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left range is greater than or equal to the right range.
    /// </summary>
    /// <param name="left">The first range to compare.</param>
    /// <param name="right">The second range to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(Range<T> left, Range<T> right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public override string ToString() => $"[{Start} .. {End}]";
}

