// Copyright © Erickson Lopez. MIT License.
using System;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Provides extension methods for <see cref="Range{T}"/> instances.
/// </summary>
public static class RangeExtensions
{
    /// <summary>
    /// Calculates the total duration spanned by the date and time range.
    /// </summary>
    /// <param name="range">The date and time range to calculate duration for.</param>
    /// <returns>The <see cref="TimeSpan"/> duration between start and end timestamps.</returns>
    public static TimeSpan Duration(this Range<DateTimeOffset> range) =>
        range.End - range.Start;

    /// <summary>
    /// Calculates the total number of days spanned by the date range.
    /// </summary>
    /// <param name="range">The date range to evaluate.</param>
    /// <returns>The total number of days elapsed between start and end dates.</returns>
    public static int Days(this Range<DateOnly> range) =>
        range.End.DayNumber - range.Start.DayNumber;
}
