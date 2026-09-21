// Copyright © Erickson Lopez. MIT License.
using System;

namespace EricksonLopez.ValueObjects.Benchmarks;

using System.Globalization;
using System.Text;
using BenchmarkDotNet.Attributes;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.Fiscal.Argentina;

/// <summary>
/// Benchmarks measuring throughput and allocations of string, span, and UTF-8 parsing.
/// </summary>
[MemoryDiagnoser]
public class ParsingBenchmarks
{
    private const string RawPercentage = "18.5";
    private const string RawCuit = "20-12345678-6";
    private const string RawCbu = "0110599520000001234567";
    private const string RawMoney = "100.50 USD";
    private const string RawDateRange = "[2026-01-01 .. 2026-12-31]";
    private const string RawTimeRange = "[08:00:00 .. 17:00:00]";
    private const string RawGeoCoordinate = "40.7128, -74.0060";

    private readonly Money _sampleMoney = Money.Create(100.50m, CurrencyCode.USD).Value;
    private readonly char[] _formatBuffer = new char[64];

    private byte[] _utf8Cuit = [];
    private byte[] _utf8Cbu = [];

    [GlobalSetup]
    public void Setup()
    {
        _utf8Cuit = Encoding.UTF8.GetBytes(RawCuit);
        _utf8Cbu = Encoding.UTF8.GetBytes(RawCbu);
    }

    [Benchmark]
    public Percentage Parse_Percentage_String() => Percentage.Parse(RawPercentage, CultureInfo.InvariantCulture);

    [Benchmark]
    public Percentage Parse_Percentage_Span() => Percentage.Parse(RawPercentage.AsSpan(), CultureInfo.InvariantCulture);

    [Benchmark]
    public bool TryParse_Percentage_Span() => Percentage.TryParse(RawPercentage.AsSpan(), CultureInfo.InvariantCulture, out _);

    [Benchmark]
    public Cuit Parse_Cuit_String() => Cuit.Parse(RawCuit, CultureInfo.InvariantCulture);

    [Benchmark]
    public Cuit Parse_Cuit_Span() => Cuit.Parse(RawCuit.AsSpan(), CultureInfo.InvariantCulture);

    [Benchmark]
    public Cuit Parse_Cuit_Utf8() => Cuit.Parse(_utf8Cuit.AsSpan(), CultureInfo.InvariantCulture);

    [Benchmark]
    public bool TryParse_Cuit_Utf8() => Cuit.TryParse(_utf8Cuit.AsSpan(), CultureInfo.InvariantCulture, out _);

    [Benchmark]
    public Cbu Parse_Cbu_String() => Cbu.Parse(RawCbu, CultureInfo.InvariantCulture);

    [Benchmark]
    public Cbu Parse_Cbu_Span() => Cbu.Parse(RawCbu.AsSpan(), CultureInfo.InvariantCulture);

    [Benchmark]
    public Cbu Parse_Cbu_Utf8() => Cbu.Parse(_utf8Cbu.AsSpan(), CultureInfo.InvariantCulture);

    [Benchmark]
    public Money Parse_Money_String() => Money.Parse(RawMoney, CultureInfo.InvariantCulture);

    [Benchmark]
    public Money Parse_Money_Span() => Money.Parse(RawMoney.AsSpan(), CultureInfo.InvariantCulture);

    [Benchmark]
    public bool TryParse_Money_Span() => Money.TryParse(RawMoney.AsSpan(), CultureInfo.InvariantCulture, out _);

    [Benchmark]
    public bool TryFormat_Money_Span() => _sampleMoney.TryFormat(_formatBuffer.AsSpan(), out _, default, CultureInfo.InvariantCulture);

    [Benchmark]
    public DateRange Parse_DateRange_String() => DateRange.Parse(RawDateRange, CultureInfo.InvariantCulture);

    [Benchmark]
    public DateRange Parse_DateRange_Span() => DateRange.Parse(RawDateRange.AsSpan(), CultureInfo.InvariantCulture);

    [Benchmark]
    public bool TryParse_DateRange_Span() => DateRange.TryParse(RawDateRange.AsSpan(), CultureInfo.InvariantCulture, out _);

    [Benchmark]
    public TimeRange Parse_TimeRange_String() => TimeRange.Parse(RawTimeRange, CultureInfo.InvariantCulture);

    [Benchmark]
    public TimeRange Parse_TimeRange_Span() => TimeRange.Parse(RawTimeRange.AsSpan(), CultureInfo.InvariantCulture);

    [Benchmark]
    public bool TryParse_TimeRange_Span() => TimeRange.TryParse(RawTimeRange.AsSpan(), CultureInfo.InvariantCulture, out _);

    [Benchmark]
    public GeoCoordinate Parse_GeoCoordinate_String() => GeoCoordinate.Parse(RawGeoCoordinate, CultureInfo.InvariantCulture);

    [Benchmark]
    public GeoCoordinate Parse_GeoCoordinate_Span() => GeoCoordinate.Parse(RawGeoCoordinate.AsSpan(), CultureInfo.InvariantCulture);

    [Benchmark]
    public bool TryParse_GeoCoordinate_Span() => GeoCoordinate.TryParse(RawGeoCoordinate.AsSpan(), CultureInfo.InvariantCulture, out _);
}


