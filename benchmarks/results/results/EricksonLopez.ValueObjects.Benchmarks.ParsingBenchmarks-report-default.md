
BenchmarkDotNet v0.15.8, Linux Ubuntu 24.04.4 LTS (Noble Numbat)
AMD EPYC 7763 2.60GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.400
  [Host]    : .NET 10.0.11 (10.0.11, 10.0.1126.37416), X64 RyuJIT x86-64-v3
  .NET 10.0 : .NET 10.0.11 (10.0.11, 10.0.1126.37416), X64 RyuJIT x86-64-v3


 Method                   | Job       | Runtime   | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
------------------------- |---------- |---------- |---------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
 Parse_Percentage_String  | .NET 10.0 | .NET 10.0 | 57.01 ns | 0.075 ns | 0.066 ns |     ? |       ? |      - |         - |           ? |
 Parse_Percentage_String  | .NET 8.0  | .NET 8.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
 Parse_Percentage_String  | .NET 9.0  | .NET 9.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
                          |           |           |          |          |          |       |         |        |           |             |
 Parse_Percentage_Span    | .NET 10.0 | .NET 10.0 | 57.13 ns | 0.085 ns | 0.076 ns |     ? |       ? |      - |         - |           ? |
 Parse_Percentage_Span    | .NET 8.0  | .NET 8.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
 Parse_Percentage_Span    | .NET 9.0  | .NET 9.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
                          |           |           |          |          |          |       |         |        |           |             |
 TryParse_Percentage_Span | .NET 10.0 | .NET 10.0 | 48.51 ns | 0.045 ns | 0.037 ns |     ? |       ? |      - |         - |           ? |
 TryParse_Percentage_Span | .NET 8.0  | .NET 8.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
 TryParse_Percentage_Span | .NET 9.0  | .NET 9.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
                          |           |           |          |          |          |       |         |        |           |             |
 Parse_Cuit_String        | .NET 10.0 | .NET 10.0 | 53.10 ns | 0.296 ns | 0.277 ns |     ? |       ? | 0.0029 |      48 B |           ? |
 Parse_Cuit_String        | .NET 8.0  | .NET 8.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
 Parse_Cuit_String        | .NET 9.0  | .NET 9.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
                          |           |           |          |          |          |       |         |        |           |             |
 Parse_Cuit_Span          | .NET 10.0 | .NET 10.0 | 57.92 ns | 0.150 ns | 0.133 ns |     ? |       ? | 0.0029 |      48 B |           ? |
 Parse_Cuit_Span          | .NET 8.0  | .NET 8.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
 Parse_Cuit_Span          | .NET 9.0  | .NET 9.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
                          |           |           |          |          |          |       |         |        |           |             |
 Parse_Cuit_Utf8          | .NET 10.0 | .NET 10.0 | 62.64 ns | 0.376 ns | 0.352 ns |     ? |       ? | 0.0029 |      48 B |           ? |
 Parse_Cuit_Utf8          | .NET 8.0  | .NET 8.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
 Parse_Cuit_Utf8          | .NET 9.0  | .NET 9.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
                          |           |           |          |          |          |       |         |        |           |             |
 TryParse_Cuit_Utf8       | .NET 10.0 | .NET 10.0 | 61.89 ns | 0.231 ns | 0.204 ns |     ? |       ? | 0.0029 |      48 B |           ? |
 TryParse_Cuit_Utf8       | .NET 8.0  | .NET 8.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
 TryParse_Cuit_Utf8       | .NET 9.0  | .NET 9.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
                          |           |           |          |          |          |       |         |        |           |             |
 Parse_Cbu_String         | .NET 10.0 | .NET 10.0 |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
 Parse_Cbu_String         | .NET 8.0  | .NET 8.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
 Parse_Cbu_String         | .NET 9.0  | .NET 9.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
                          |           |           |          |          |          |       |         |        |           |             |
 Parse_Cbu_Span           | .NET 10.0 | .NET 10.0 |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
 Parse_Cbu_Span           | .NET 8.0  | .NET 8.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
 Parse_Cbu_Span           | .NET 9.0  | .NET 9.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
                          |           |           |          |          |          |       |         |        |           |             |
 Parse_Cbu_Utf8           | .NET 10.0 | .NET 10.0 |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
 Parse_Cbu_Utf8           | .NET 8.0  | .NET 8.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
 Parse_Cbu_Utf8           | .NET 9.0  | .NET 9.0  |       NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |

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
