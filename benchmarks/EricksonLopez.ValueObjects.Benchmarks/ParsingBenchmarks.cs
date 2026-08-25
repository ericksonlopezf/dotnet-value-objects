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
}


