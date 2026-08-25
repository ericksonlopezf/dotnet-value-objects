// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;

namespace EricksonLopez.ValueObjects.Samples.Levels;

/// <summary>
/// Level 02: Configuration, Normalization Pipelines, Range Value Objects, BusinessDate, and TimeRange.
/// Demonstrates StringPipeline behaviors, uppercase/lowercase enforcement, mathematical/temporal ranges,
/// BusinessDate domain dates, and advanced TimeRange operations.
/// </summary>
public static class Level02_ConfigurationAndPipelines
{
    /// <summary>
    /// Executes the configuration and pipelines demonstration.
    /// </summary>
    public static void Run()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n===============================================================================");
        Console.WriteLine(" [LEVEL 2] CONFIGURATION, NORMALIZATION PIPELINES, RANGES, AND DATES");
        Console.WriteLine("===============================================================================");
        Console.ResetColor();

        // 1. StringPipeline: Normalization and Trimming
        Console.WriteLine("[1. String Pipelines & Sanitization]");
        var country = Country.Create("  do   ").Value;
        var postalCode = PostalCode.Create("   10101   ").Value;
        var webUrl = WebsiteUrl.Create("https://ericksonlopez.dev/docs/value-objects").Value;
        var tz = TimeZoneCode.Create("America/Santo_Domingo").Value;
        var code = Code.Create("  prod-alpha-99  ").Value;
        var lang = LanguageCode.Create("  es  ").Value;

        Console.WriteLine($"  - Country (Normalized to ISO uppercase)   : '{country.Value}'");
        Console.WriteLine($"  - PostalCode (Sanitized)                  : '{postalCode.Value}'");
        Console.WriteLine($"  - WebsiteUrl                              : '{webUrl.Value}'");
        Console.WriteLine($"  - TimeZoneCode                            : '{tz.Value}'");
        Console.WriteLine($"  - Code (Normalized to uppercase)          : '{code.Value}'");
        Console.WriteLine($"  - LanguageCode (Normalized to lowercase)  : '{lang.Value}'");

        // 2. Mathematical Range<T> & RangeExtensions
        Console.WriteLine("\n[2. Mathematical Range<T> & Interval Operations]");
        var q1 = Range<DateOnly>.Create(new DateOnly(2026, 1, 1), new DateOnly(2026, 3, 31)).Value;
        var february = Range<DateOnly>.Create(new DateOnly(2026, 2, 1), new DateOnly(2026, 2, 28)).Value;
        var april = Range<DateOnly>.Create(new DateOnly(2026, 4, 1), new DateOnly(2026, 4, 30)).Value;
        var march = Range<DateOnly>.Create(new DateOnly(2026, 3, 1), new DateOnly(2026, 3, 31)).Value;

        Console.WriteLine($"  - Range Q1 2026         : [{q1.Start} .. {q1.End}]");
        Console.WriteLine($"  - Duration in Days      : {q1.Days()} days");

        // Contains(Range<T>) — Q1 contains all of February
        Console.WriteLine($"  - Q1 contains Feb       : {q1.Contains(february)}");

        // Contains(T value) — Q1 contains a specific date
        var midFebruary = new DateOnly(2026, 2, 15);
        Console.WriteLine($"  - Q1 contains 15-Feb    : {q1.Contains(midFebruary)}");

        // Overlaps
        Console.WriteLine($"  - Q1 overlaps Apr       : {q1.Overlaps(april)}");
        Console.WriteLine($"  - Q1 overlaps Mar       : {q1.Overlaps(march)}");

        // Intersects — returns shared interval
        if (q1.Intersects(march, out Range<DateOnly> intersection))
        {
            Console.WriteLine($"  - Intersection Q1∩Mar   : [{intersection.Start} .. {intersection.End}]");
        }

        // Range<DateTimeOffset> Duration extension
        Console.WriteLine("\n[3. Range<DateTimeOffset> — Duration extension]");
        var sessionStart = new DateTimeOffset(2026, 8, 24, 9, 0, 0, TimeSpan.Zero);
        var sessionEnd   = new DateTimeOffset(2026, 8, 24, 17, 30, 0, TimeSpan.Zero);
        var workday = Range<DateTimeOffset>.Create(sessionStart, sessionEnd).Value;
        TimeSpan workDuration = workday.Duration();
        Console.WriteLine($"  - Workday Schedule      : [{workday.Start:HH:mm} .. {workday.End:HH:mm}]");
        Console.WriteLine($"  - Total Duration        : {workDuration.TotalHours}h {workDuration.Minutes}m");

        // 3. BusinessDate — Domain date without time component
        Console.WriteLine("\n[4. BusinessDate — Domain Date without Time Component]");
        var invoiceDate = BusinessDate.Create(new DateOnly(2026, 8, 24)).Value;
        var dueDate     = BusinessDate.Create(new DateOnly(2026, 9, 24)).Value;
        var fromDto     = BusinessDate.FromDateTimeOffset(DateTimeOffset.UtcNow).Value;

        Console.WriteLine($"  - Issue Date            : {invoiceDate}");
        Console.WriteLine($"  - Due Date              : {dueDate}");
        Console.WriteLine($"  - From DTO (UTC)        : {fromDto}");
        Console.WriteLine($"  - Issue < Due           : {invoiceDate < dueDate}");

        // BusinessDate.Parse / TryParse
        var parsed = BusinessDate.Parse("2026-12-31");
        Console.WriteLine($"  - Parsed from string    : {parsed}");

        if (BusinessDate.TryParse("2026-01-15", null, out BusinessDate tryParsed))
        {
            Console.WriteLine($"  - TryParse success      : {tryParsed}");
        }

        // 4. TimeRange with Overnight Support
        Console.WriteLine("\n[5. TimeRange & Overnight Shifts — Contains, Overlaps, Duration]");
        var dayShift   = TimeRange.Create(new TimeOnly(8, 0), new TimeOnly(17, 0)).Value;
        var nightShift = TimeRange.Create(new TimeOnly(22, 0), new TimeOnly(6, 0), allowOvernight: true).Value;

        Console.WriteLine($"  - Day Shift             : {dayShift} (Overnight: {dayShift.CrossesMidnight})");
        Console.WriteLine($"  - Night Shift           : {nightShift} (Overnight: {nightShift.CrossesMidnight})");

        // TimeRange.Duration
        Console.WriteLine($"  - Day Shift Duration    : {dayShift.Duration.TotalHours}h");
        Console.WriteLine($"  - Night Shift Duration  : {nightShift.Duration.TotalHours}h");

        // TimeRange.Contains(TimeOnly)
        var noonTime   = new TimeOnly(12, 0);
        var midnightTime = new TimeOnly(0, 30);
        Console.WriteLine($"  - Day Shift contains 12:00  : {dayShift.Contains(noonTime)}");
        Console.WriteLine($"  - Night Shift contains 00:30: {nightShift.Contains(midnightTime)}");

        // TimeRange.Overlaps(TimeRange)
        var afternoonShift = TimeRange.Create(new TimeOnly(13, 0), new TimeOnly(21, 0)).Value;
        Console.WriteLine($"  - Day overlaps Afternoon (13-21)  : {dayShift.Overlaps(afternoonShift)}");
        Console.WriteLine($"  - Night overlaps Afternoon (13-21): {nightShift.Overlaps(afternoonShift)}");

        // 5. Specialized Financial Rates
        Console.WriteLine("\n[6. Specialized Financial Rates with Strong Semantics]");
        var taxRate     = TaxRate.Create(18.0m).Value;
        var discountRate = DiscountRate.Create(5.5m).Value;
        var exchangeRate = ExchangeRate.Create(CurrencyCode.USD, CurrencyCode.DOP, 60.25m).Value;

        Console.WriteLine($"  - TaxRate               : {taxRate} (Fraction: {taxRate.Fraction})");
        Console.WriteLine($"  - DiscountRate          : {discountRate} (Fraction: {discountRate.Fraction})");
        Console.WriteLine($"  - ExchangeRate          : {exchangeRate} (1 USD = {exchangeRate.Rate})");
    }
}
