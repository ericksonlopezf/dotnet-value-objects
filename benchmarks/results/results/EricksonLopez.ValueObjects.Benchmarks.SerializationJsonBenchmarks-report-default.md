
BenchmarkDotNet v0.15.8, Linux Ubuntu 24.04.4 LTS (Noble Numbat)
AMD EPYC 7763 2.60GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.400
  [Host] : .NET 10.0.11 (10.0.11, 10.0.1126.37416), X64 RyuJIT x86-64-v3


 Method                  | Job       | Runtime   | Mean | Error | Ratio | RatioSD | Alloc Ratio |
------------------------ |---------- |---------- |-----:|------:|------:|--------:|------------:|
 Serialize_TenantCode    | .NET 10.0 | .NET 10.0 |   NA |    NA |     ? |       ? |           ? |
 Serialize_TenantCode    | .NET 8.0  | .NET 8.0  |   NA |    NA |     ? |       ? |           ? |
 Serialize_TenantCode    | .NET 9.0  | .NET 9.0  |   NA |    NA |     ? |       ? |           ? |
                         |           |           |      |       |       |         |             |
 Deserialize_TenantCode  | .NET 10.0 | .NET 10.0 |   NA |    NA |     ? |       ? |           ? |
 Deserialize_TenantCode  | .NET 8.0  | .NET 8.0  |   NA |    NA |     ? |       ? |           ? |
 Deserialize_TenantCode  | .NET 9.0  | .NET 9.0  |   NA |    NA |     ? |       ? |           ? |
                         |           |           |      |       |       |         |             |
 Serialize_CompanyName   | .NET 10.0 | .NET 10.0 |   NA |    NA |     ? |       ? |           ? |
 Serialize_CompanyName   | .NET 8.0  | .NET 8.0  |   NA |    NA |     ? |       ? |           ? |
 Serialize_CompanyName   | .NET 9.0  | .NET 9.0  |   NA |    NA |     ? |       ? |           ? |
                         |           |           |      |       |       |         |             |
 Deserialize_CompanyName | .NET 10.0 | .NET 10.0 |   NA |    NA |     ? |       ? |           ? |
 Deserialize_CompanyName | .NET 8.0  | .NET 8.0  |   NA |    NA |     ? |       ? |           ? |
 Deserialize_CompanyName | .NET 9.0  | .NET 9.0  |   NA |    NA |     ? |       ? |           ? |
                         |           |           |      |       |       |         |             |
 Serialize_Range         | .NET 10.0 | .NET 10.0 |   NA |    NA |     ? |       ? |           ? |
 Serialize_Range         | .NET 8.0  | .NET 8.0  |   NA |    NA |     ? |       ? |           ? |
 Serialize_Range         | .NET 9.0  | .NET 9.0  |   NA |    NA |     ? |       ? |           ? |
                         |           |           |      |       |       |         |             |
 Deserialize_Range       | .NET 10.0 | .NET 10.0 |   NA |    NA |     ? |       ? |           ? |
 Deserialize_Range       | .NET 8.0  | .NET 8.0  |   NA |    NA |     ? |       ? |           ? |
 Deserialize_Range       | .NET 9.0  | .NET 9.0  |   NA |    NA |     ? |       ? |           ? |

Benchmarks with issues:
  SerializationJsonBenchmarks.Serialize_TenantCode: .NET 10.0(Runtime=.NET 10.0, Toolchain=net10.0)
  SerializationJsonBenchmarks.Serialize_TenantCode: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  SerializationJsonBenchmarks.Serialize_TenantCode: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  SerializationJsonBenchmarks.Deserialize_TenantCode: .NET 10.0(Runtime=.NET 10.0, Toolchain=net10.0)
  SerializationJsonBenchmarks.Deserialize_TenantCode: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  SerializationJsonBenchmarks.Deserialize_TenantCode: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  SerializationJsonBenchmarks.Serialize_CompanyName: .NET 10.0(Runtime=.NET 10.0, Toolchain=net10.0)
  SerializationJsonBenchmarks.Serialize_CompanyName: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  SerializationJsonBenchmarks.Serialize_CompanyName: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  SerializationJsonBenchmarks.Deserialize_CompanyName: .NET 10.0(Runtime=.NET 10.0, Toolchain=net10.0)
  SerializationJsonBenchmarks.Deserialize_CompanyName: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  SerializationJsonBenchmarks.Deserialize_CompanyName: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  SerializationJsonBenchmarks.Serialize_Range: .NET 10.0(Runtime=.NET 10.0, Toolchain=net10.0)
  SerializationJsonBenchmarks.Serialize_Range: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  SerializationJsonBenchmarks.Serialize_Range: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  SerializationJsonBenchmarks.Deserialize_Range: .NET 10.0(Runtime=.NET 10.0, Toolchain=net10.0)
  SerializationJsonBenchmarks.Deserialize_Range: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  SerializationJsonBenchmarks.Deserialize_Range: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
