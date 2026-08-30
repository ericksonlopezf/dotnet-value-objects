```

BenchmarkDotNet v0.15.8, Linux Ubuntu 24.04.4 LTS (Noble Numbat)
AMD EPYC 7763 2.60GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.400
  [Host] : .NET 10.0.11 (10.0.11, 10.0.1126.37416), X64 RyuJIT x86-64-v3


```
| Method                       | Job       | Runtime   | Mean | Error | Ratio | RatioSD | Alloc Ratio |
|----------------------------- |---------- |---------- |-----:|------:|------:|--------:|------------:|
| Parse_TenantCode_ValidObject | .NET 10.0 | .NET 10.0 |   NA |    NA |     ? |       ? |           ? |
| Parse_TenantCode_ValidObject | .NET 8.0  | .NET 8.0  |   NA |    NA |     ? |       ? |           ? |
| Parse_TenantCode_ValidObject | .NET 9.0  | .NET 9.0  |   NA |    NA |     ? |       ? |           ? |
|                              |           |           |      |       |       |         |             |
| Parse_TenantCode_Null        | .NET 10.0 | .NET 10.0 |   NA |    NA |     ? |       ? |           ? |
| Parse_TenantCode_Null        | .NET 8.0  | .NET 8.0  |   NA |    NA |     ? |       ? |           ? |
| Parse_TenantCode_Null        | .NET 9.0  | .NET 9.0  |   NA |    NA |     ? |       ? |           ? |

Benchmarks with issues:
  DapperTypeHandlerBenchmarks.Parse_TenantCode_ValidObject: .NET 10.0(Runtime=.NET 10.0, Toolchain=net10.0)
  DapperTypeHandlerBenchmarks.Parse_TenantCode_ValidObject: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  DapperTypeHandlerBenchmarks.Parse_TenantCode_ValidObject: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  DapperTypeHandlerBenchmarks.Parse_TenantCode_Null: .NET 10.0(Runtime=.NET 10.0, Toolchain=net10.0)
  DapperTypeHandlerBenchmarks.Parse_TenantCode_Null: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  DapperTypeHandlerBenchmarks.Parse_TenantCode_Null: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
