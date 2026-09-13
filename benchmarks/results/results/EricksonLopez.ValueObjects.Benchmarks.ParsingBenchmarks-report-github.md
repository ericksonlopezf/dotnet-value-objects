```

BenchmarkDotNet v0.15.8, Linux Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763 2.45GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]    : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v3
  .NET 10.0 : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v3


```
| Method                   | Job       | Runtime   | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |---------- |---------- |---------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| Parse_Percentage_String  | .NET 10.0 | .NET 10.0 | 62.53 ns | 0.099 ns | 0.088 ns |     ? |       ? |      - |         - |           ? |
| Parse_Percentage_String  | .NET 8.0  | .NET 8.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
| Parse_Percentage_String  | .NET 9.0  | .NET 9.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
|                          |           |           |          |          |          |       |         |        |           |             |
| Parse_Percentage_Span    | .NET 10.0 | .NET 10.0 | 59.04 ns | 0.155 ns | 0.137 ns |     ? |       ? |      - |         - |           ? |
| Parse_Percentage_Span    | .NET 8.0  | .NET 8.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
| Parse_Percentage_Span    | .NET 9.0  | .NET 9.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
|                          |           |           |          |          |          |       |         |        |           |             |
| TryParse_Percentage_Span | .NET 10.0 | .NET 10.0 | 53.30 ns | 0.109 ns | 0.102 ns |     ? |       ? |      - |         - |           ? |
| TryParse_Percentage_Span | .NET 8.0  | .NET 8.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
| TryParse_Percentage_Span | .NET 9.0  | .NET 9.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
|                          |           |           |          |          |          |       |         |        |           |             |
| Parse_Cuit_String        | .NET 10.0 | .NET 10.0 | 52.63 ns | 0.106 ns | 0.099 ns |     ? |       ? | 0.0029 |      48 B |           ? |
| Parse_Cuit_String        | .NET 8.0  | .NET 8.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
| Parse_Cuit_String        | .NET 9.0  | .NET 9.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
|                          |           |           |          |          |          |       |         |        |           |             |
| Parse_Cuit_Span          | .NET 10.0 | .NET 10.0 | 57.76 ns | 0.234 ns | 0.196 ns |     ? |       ? | 0.0029 |      48 B |           ? |
| Parse_Cuit_Span          | .NET 8.0  | .NET 8.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
| Parse_Cuit_Span          | .NET 9.0  | .NET 9.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
|                          |           |           |          |          |          |       |         |        |           |             |
| Parse_Cuit_Utf8          | .NET 10.0 | .NET 10.0 | 62.63 ns | 0.135 ns | 0.113 ns |     ? |       ? | 0.0029 |      48 B |           ? |
| Parse_Cuit_Utf8          | .NET 8.0  | .NET 8.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
| Parse_Cuit_Utf8          | .NET 9.0  | .NET 9.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
|                          |           |           |          |          |          |       |         |        |           |             |
| TryParse_Cuit_Utf8       | .NET 10.0 | .NET 10.0 | 62.07 ns | 0.154 ns | 0.129 ns |     ? |       ? | 0.0029 |      48 B |           ? |
| TryParse_Cuit_Utf8       | .NET 8.0  | .NET 8.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
| TryParse_Cuit_Utf8       | .NET 9.0  | .NET 9.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
|                          |           |           |          |          |          |       |         |        |           |             |
| Parse_Cbu_String         | .NET 10.0 | .NET 10.0 |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
| Parse_Cbu_String         | .NET 8.0  | .NET 8.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
| Parse_Cbu_String         | .NET 9.0  | .NET 9.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
|                          |           |           |          |          |          |       |         |        |           |             |
| Parse_Cbu_Span           | .NET 10.0 | .NET 10.0 |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
| Parse_Cbu_Span           | .NET 8.0  | .NET 8.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
| Parse_Cbu_Span           | .NET 9.0  | .NET 9.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
|                          |           |           |          |          |          |       |         |        |           |             |
| Parse_Cbu_Utf8           | .NET 10.0 | .NET 10.0 |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
| Parse_Cbu_Utf8           | .NET 8.0  | .NET 8.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
| Parse_Cbu_Utf8           | .NET 9.0  | .NET 9.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |

Benchmarks with issues:
  ParsingBenchmarks.Parse_Percentage_String: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ParsingBenchmarks.Parse_Percentage_String: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  ParsingBenchmarks.Parse_Percentage_Span: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ParsingBenchmarks.Parse_Percentage_Span: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  ParsingBenchmarks.TryParse_Percentage_Span: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ParsingBenchmarks.TryParse_Percentage_Span: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  ParsingBenchmarks.Parse_Cuit_String: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ParsingBenchmarks.Parse_Cuit_String: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  ParsingBenchmarks.Parse_Cuit_Span: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ParsingBenchmarks.Parse_Cuit_Span: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  ParsingBenchmarks.Parse_Cuit_Utf8: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ParsingBenchmarks.Parse_Cuit_Utf8: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  ParsingBenchmarks.TryParse_Cuit_Utf8: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ParsingBenchmarks.TryParse_Cuit_Utf8: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  ParsingBenchmarks.Parse_Cbu_String: .NET 10.0(Runtime=.NET 10.0, Toolchain=net10.0)
  ParsingBenchmarks.Parse_Cbu_String: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ParsingBenchmarks.Parse_Cbu_String: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  ParsingBenchmarks.Parse_Cbu_Span: .NET 10.0(Runtime=.NET 10.0, Toolchain=net10.0)
  ParsingBenchmarks.Parse_Cbu_Span: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ParsingBenchmarks.Parse_Cbu_Span: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  ParsingBenchmarks.Parse_Cbu_Utf8: .NET 10.0(Runtime=.NET 10.0, Toolchain=net10.0)
  ParsingBenchmarks.Parse_Cbu_Utf8: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ParsingBenchmarks.Parse_Cbu_Utf8: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
