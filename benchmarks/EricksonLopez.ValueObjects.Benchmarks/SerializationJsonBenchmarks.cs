// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Benchmarks;

using System.Text.Json;
using BenchmarkDotNet.Attributes;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.Serialization.Json;

/// <summary>
/// Benchmarks measuring serialization and deserialization throughput with System.Text.Json.
/// </summary>
[MemoryDiagnoser]
public class SerializationJsonBenchmarks
{
    private sealed class TenantCodeJsonConverter : StringValueObjectJsonConverter<TenantCode>
    {
        protected override Result<TenantCode> CreateInstance(string value) => TenantCode.Create(value);
    }

    private sealed class CompanyNameJsonConverter : StringValueObjectJsonConverter<CompanyName>
    {
        protected override Result<CompanyName> CreateInstance(string value) => CompanyName.Create(value);
    }

    private readonly JsonSerializerOptions _options = new();
    private readonly TenantCode _tenantCode = TenantCode.Create("TENANT_CORP_001").Value;
    private readonly CompanyName _companyName = CompanyName.Create("Acme Corporation").Value;
    private readonly Range<int> _intRange = Range<int>.Create(1, 100).Value;

    private string _tenantCodeJson = string.Empty;
    private string _companyNameJson = string.Empty;
    private string _rangeJson = string.Empty;

    [GlobalSetup]
    public void Setup()
    {
        _options.Converters.Add(new TenantCodeJsonConverter());
        _options.Converters.Add(new CompanyNameJsonConverter());
        _options.Converters.Add(new RangeJsonConverter<int>());

        _tenantCodeJson = JsonSerializer.Serialize(_tenantCode, _options);
        _companyNameJson = JsonSerializer.Serialize(_companyName, _options);
        _rangeJson = JsonSerializer.Serialize(_intRange, _options);
    }

    [Benchmark]
    public string Serialize_TenantCode() => JsonSerializer.Serialize(_tenantCode, _options);

    [Benchmark]
    public TenantCode? Deserialize_TenantCode() => JsonSerializer.Deserialize<TenantCode>(_tenantCodeJson, _options);

    [Benchmark]
    public string Serialize_CompanyName() => JsonSerializer.Serialize(_companyName, _options);

    [Benchmark]
    public CompanyName? Deserialize_CompanyName() => JsonSerializer.Deserialize<CompanyName>(_companyNameJson, _options);

    [Benchmark]
    public string Serialize_Range() => JsonSerializer.Serialize(_intRange, _options);

    [Benchmark]
    public Range<int>? Deserialize_Range() => JsonSerializer.Deserialize<Range<int>>(_rangeJson, _options);
}


