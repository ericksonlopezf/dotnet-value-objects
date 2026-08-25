// Copyright © Erickson Lopez. MIT License.
namespace EricksonLopez.ValueObjects.Benchmarks;

using BenchmarkDotNet.Attributes;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.Dapper;

/// <summary>
/// Benchmarks measuring Dapper TypeHandler mapping and instantiation overhead.
/// </summary>
[MemoryDiagnoser]
public class DapperTypeHandlerBenchmarks
{
    private readonly SingleValueObjectTypeHandler<TenantCode, string> _handler =
        new(TenantCode.Create);

    private readonly TenantCode _tenantCode = TenantCode.Create("TENANT_001").Value;
    private const string RawTenantCode = "TENANT_001";

    [Benchmark]
    public TenantCode? Parse_TenantCode_ValidObject() => _handler.Parse(RawTenantCode);

    [Benchmark]
    public TenantCode? Parse_TenantCode_Null() => _handler.Parse(null!);
}
