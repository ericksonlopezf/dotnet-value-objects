```

BenchmarkDotNet v0.15.8, Linux Ubuntu 24.04.4 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C 3.35GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.400
  [Host]    : .NET 10.0.11 (10.0.11, 10.0.1126.37416), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.11 (10.0.11, 10.0.1126.37416), X64 RyuJIT x86-64-v4


```
| Method                   | Job       | Runtime   | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |---------- |---------- |---------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| Parse_Percentage_String  | .NET 10.0 | .NET 10.0 | 37.18 ns | 0.211 ns | 0.187 ns |     ? |       ? |      - |         - |           ? |
| Parse_Percentage_String  | .NET 8.0  | .NET 8.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
| Parse_Percentage_String  | .NET 9.0  | .NET 9.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
|                          |           |           |          |          |          |       |         |        |           |             |
| Parse_Percentage_Span    | .NET 10.0 | .NET 10.0 | 36.16 ns | 0.172 ns | 0.161 ns |     ? |       ? |      - |         - |           ? |
| Parse_Percentage_Span    | .NET 8.0  | .NET 8.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
| Parse_Percentage_Span    | .NET 9.0  | .NET 9.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
|                          |           |           |          |          |          |       |         |        |           |             |
| TryParse_Percentage_Span | .NET 10.0 | .NET 10.0 | 34.23 ns | 0.170 ns | 0.133 ns |     ? |       ? |      - |         - |           ? |
| TryParse_Percentage_Span | .NET 8.0  | .NET 8.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
| TryParse_Percentage_Span | .NET 9.0  | .NET 9.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
|                          |           |           |          |          |          |       |         |        |           |             |
| Parse_Cuit_String        | .NET 10.0 | .NET 10.0 | 33.84 ns | 0.511 ns | 0.478 ns |     ? |       ? | 0.0005 |      48 B |           ? |
| Parse_Cuit_String        | .NET 8.0  | .NET 8.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
| Parse_Cuit_String        | .NET 9.0  | .NET 9.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
|                          |           |           |          |          |          |       |         |        |           |             |
| Parse_Cuit_Span          | .NET 10.0 | .NET 10.0 | 38.31 ns | 0.372 ns | 0.348 ns |     ? |       ? | 0.0005 |      48 B |           ? |
| Parse_Cuit_Span          | .NET 8.0  | .NET 8.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
| Parse_Cuit_Span          | .NET 9.0  | .NET 9.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
|                          |           |           |          |          |          |       |         |        |           |             |
| Parse_Cuit_Utf8          | .NET 10.0 | .NET 10.0 | 43.71 ns | 0.274 ns | 0.256 ns |     ? |       ? | 0.0005 |      48 B |           ? |
| Parse_Cuit_Utf8          | .NET 8.0  | .NET 8.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
| Parse_Cuit_Utf8          | .NET 9.0  | .NET 9.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
|                          |           |           |          |          |          |       |         |        |           |             |
| TryParse_Cuit_Utf8       | .NET 10.0 | .NET 10.0 | 42.49 ns | 0.377 ns | 0.352 ns |     ? |       ? | 0.0005 |      48 B |           ? |
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
