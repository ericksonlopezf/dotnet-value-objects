// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;

/// <summary>
/// Represents a fiscal year-month period (e.g., 2024-01 = January 2024).
/// Supports sequential navigation and comparison.
///
/// <para><b>Rules:</b> Year in [2000, 2100], Month in [1, 12].</para>
/// <para><b>Used by:</b> Fiscal, Accounting, Billing</para>
///
/// <para><b>Navigation:</b> <see cref="Next"/> and <see cref="Previous"/> return
/// <c>Result&lt;FiscalPeriod&gt;</c> to prevent silent creation of out-of-range periods
/// at the year boundaries.</para>
///
/// <para><b>Boundary note:</b> This VO lives in the Fiscal bounded context because
/// fiscal periods are a financial/accounting concept. It assumes calendar-year fiscal
/// periods (January–December). For jurisdictions with non-calendar fiscal years
/// (e.g., UK April–March, Japan April–March), the consuming BC must implement its own
/// fiscal year mapping on top of this VO.</para>
/// </summary>
public readonly record struct FiscalPeriod : IValueObject<FiscalPeriod>, IComparable<FiscalPeriod>, IComparable
{
    /// <summary>Gets the calendar year.</summary>
    public int Year { get; }
    /// <summary>Gets the month (1-12).</summary>
    public int Month { get; }

    /// <summary>Gets the first date of the fiscal period.</summary>
    public DateOnly Start => new(Year, Month, 1);
    /// <summary>Gets the last date of the fiscal period.</summary>
    public DateOnly End => new(Year, Month, DateTime.DaysInMonth(Year, Month));

    /// <summary>
    /// Gets the statutory DGII filing deadline for this period (the 20th day of the following month).
    /// </summary>
    public DateOnly FilingDeadline
    {
        get
        {
            int nextMonth = Month == 12 ? 1 : Month + 1;
            int nextYear = Month == 12 ? Year + 1 : Year;
            return new DateOnly(nextYear, nextMonth, 20);
        }
    }

    private FiscalPeriod(int year, int month)
    {
        Year = year;
        Month = month;
    }

    /// <summary>
    /// Creates a <see cref="FiscalPeriod"/> from year and month integers.
    /// </summary>
    /// <param name="year">The calendar year (2000–2100 inclusive).</param>
    /// <param name="month">The calendar month (1–12 inclusive).</param>
    /// <returns>A <see cref="Result{FiscalPeriod}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<FiscalPeriod> Create(int year, int month)
    {
        if (year is < 2000 or > 2100)
        {
            return Result<FiscalPeriod>.Failure(Error.Validation(
                "FiscalPeriod.InvalidYear", $"Year {year} is out of valid range (2000-2100)."));
        }

        if (month is < 1 or > 12)
        {
            return Result<FiscalPeriod>.Failure(Error.Validation(
                "FiscalPeriod.InvalidMonth", $"Month {month} must be between 1 and 12."));
        }

        return Result<FiscalPeriod>.Success(new FiscalPeriod(year, month));
    }

    /// <summary>
    /// Parses a string representation in either <c>YYYYMM</c> (e.g. <c>202608</c>) or <c>YYYY-MM</c> (e.g. <c>2026-08</c>).
    /// </summary>
    /// <param name="value">A string in <c>YYYYMM</c> or <c>YYYY-MM</c> format.</param>
    /// <returns>A <see cref="Result{FiscalPeriod}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<FiscalPeriod> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result<FiscalPeriod>.Failure(Error.Validation(
                "FiscalPeriod.Required", "Fiscal period is required."));
        }

        string trimmed = value.Trim().Replace("-", "");
        if (trimmed.Length != 6 || !int.TryParse(trimmed, NumberStyles.None, CultureInfo.InvariantCulture, out int number))
        {
            return Result<FiscalPeriod>.Failure(Error.Validation(
                "FiscalPeriod.InvalidFormat",
                $"Fiscal period '{value}' must be in 'YYYYMM' or 'YYYY-MM' format."));
        }

        int year = number / 100;
        int month = number % 100;

        return Create(year, month);
    }

    /// <summary>
    /// Creates a <see cref="FiscalPeriod"/> containing the specified date.
    /// </summary>
    public static FiscalPeriod FromDate(DateOnly date) => new(date.Year, date.Month);

    /// <summary>
    /// Checks whether the filing deadline has elapsed relative to the provided reference date.
    /// </summary>
    public bool IsDue(DateOnly currentDate) => currentDate > FilingDeadline;

    /// <summary>Gets the next sequential fiscal period.</summary>
    public FiscalPeriod Next() => Month == 12 ? new FiscalPeriod(Year + 1, 1) : new FiscalPeriod(Year, Month + 1);

    /// <summary>Gets the previous sequential fiscal period.</summary>
    public FiscalPeriod Previous() => Month == 1 ? new FiscalPeriod(Year - 1, 12) : new FiscalPeriod(Year, Month - 1);

    /// <inheritdoc/>
    public int CompareTo(FiscalPeriod other)
    {
        int yearComparison = Year.CompareTo(other.Year);
        return yearComparison != 0 ? yearComparison : Month.CompareTo(other.Month);
    }

    /// <inheritdoc/>
    public int CompareTo(object? obj) =>
        obj is FiscalPeriod other ? CompareTo(other) : throw new ArgumentException("Object is not a FiscalPeriod", nameof(obj));

        /// <summary>
    /// Determines whether the left <see cref="FiscalPeriod"/> is less than the right <see cref="FiscalPeriod"/>.
    /// </summary>
    /// <param name="left">The first <see cref="FiscalPeriod"/> to compare.</param>
    /// <param name="right">The second <see cref="FiscalPeriod"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(FiscalPeriod left, FiscalPeriod right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="FiscalPeriod"/> is less than or equal to the right <see cref="FiscalPeriod"/>.
    /// </summary>
    /// <param name="left">The first <see cref="FiscalPeriod"/> to compare.</param>
    /// <param name="right">The second <see cref="FiscalPeriod"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(FiscalPeriod left, FiscalPeriod right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="FiscalPeriod"/> is greater than the right <see cref="FiscalPeriod"/>.
    /// </summary>
    /// <param name="left">The first <see cref="FiscalPeriod"/> to compare.</param>
    /// <param name="right">The second <see cref="FiscalPeriod"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(FiscalPeriod left, FiscalPeriod right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="FiscalPeriod"/> is greater than or equal to the right <see cref="FiscalPeriod"/>.
    /// </summary>
    /// <param name="left">The first <see cref="FiscalPeriod"/> to compare.</param>
    /// <param name="right">The second <see cref="FiscalPeriod"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(FiscalPeriod left, FiscalPeriod right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public override string ToString() => $"{Year}{Month:D2}";
}


