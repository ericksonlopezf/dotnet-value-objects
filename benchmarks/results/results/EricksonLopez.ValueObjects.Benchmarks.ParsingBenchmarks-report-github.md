```

BenchmarkDotNet v0.15.8, Linux Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763 2.98GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]    : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v3
  .NET 10.0 : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v3
  .NET 8.0  : .NET 8.0.31 (8.0.31, 8.0.3126.42015), X64 RyuJIT x86-64-v3
  .NET 9.0  : .NET 9.0.20 (9.0.20, 9.0.2026.41315), X64 RyuJIT x86-64-v3


```
| Method                      | Job       | Runtime   | Mean      | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------------------------- |---------- |---------- |----------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| Parse_Percentage_String     | .NET 10.0 | .NET 10.0 |  58.11 ns | 0.102 ns | 0.091 ns |  0.74 |    0.00 |      - |         - |          NA |
| Parse_Percentage_String     | .NET 8.0  | .NET 8.0  |  78.18 ns | 0.350 ns | 0.273 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_Percentage_String     | .NET 9.0  | .NET 9.0  |  65.77 ns | 0.064 ns | 0.060 ns |  0.84 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_Percentage_Span       | .NET 10.0 | .NET 10.0 |  57.77 ns | 0.091 ns | 0.085 ns |  0.63 |    0.00 |      - |         - |          NA |
| Parse_Percentage_Span       | .NET 8.0  | .NET 8.0  |  91.96 ns | 0.096 ns | 0.080 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_Percentage_Span       | .NET 9.0  | .NET 9.0  |  84.94 ns | 0.140 ns | 0.131 ns |  0.92 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryParse_Percentage_Span    | .NET 10.0 | .NET 10.0 |  51.49 ns | 0.106 ns | 0.094 ns |  0.70 |    0.00 |      - |         - |          NA |
| TryParse_Percentage_Span    | .NET 8.0  | .NET 8.0  |  73.61 ns | 0.092 ns | 0.082 ns |  1.00 |    0.00 |      - |         - |          NA |
| TryParse_Percentage_Span    | .NET 9.0  | .NET 9.0  |  62.49 ns | 0.043 ns | 0.038 ns |  0.85 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_Cuit_String           | .NET 10.0 | .NET 10.0 |  51.30 ns | 0.140 ns | 0.124 ns |  0.86 |    0.01 | 0.0029 |      48 B |        1.00 |
| Parse_Cuit_String           | .NET 8.0  | .NET 8.0  |  59.57 ns | 0.500 ns | 0.444 ns |  1.00 |    0.01 | 0.0029 |      48 B |        1.00 |
| Parse_Cuit_String           | .NET 9.0  | .NET 9.0  |  52.76 ns | 0.233 ns | 0.218 ns |  0.89 |    0.01 | 0.0029 |      48 B |        1.00 |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_Cuit_Span             | .NET 10.0 | .NET 10.0 |  56.92 ns | 0.117 ns | 0.098 ns |  0.96 |    0.00 | 0.0029 |      48 B |        1.00 |
| Parse_Cuit_Span             | .NET 8.0  | .NET 8.0  |  59.56 ns | 0.250 ns | 0.234 ns |  1.00 |    0.01 | 0.0029 |      48 B |        1.00 |
| Parse_Cuit_Span             | .NET 9.0  | .NET 9.0  |  55.22 ns | 0.868 ns | 0.812 ns |  0.93 |    0.01 | 0.0029 |      48 B |        1.00 |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_Cuit_Utf8             | .NET 10.0 | .NET 10.0 |  60.93 ns | 0.125 ns | 0.098 ns |  0.91 |    0.01 | 0.0029 |      48 B |        1.00 |
| Parse_Cuit_Utf8             | .NET 8.0  | .NET 8.0  |  66.99 ns | 0.441 ns | 0.413 ns |  1.00 |    0.01 | 0.0029 |      48 B |        1.00 |
| Parse_Cuit_Utf8             | .NET 9.0  | .NET 9.0  |  71.78 ns | 0.086 ns | 0.076 ns |  1.07 |    0.01 | 0.0029 |      48 B |        1.00 |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryParse_Cuit_Utf8          | .NET 10.0 | .NET 10.0 |  66.44 ns | 0.160 ns | 0.150 ns |  1.03 |    0.00 | 0.0029 |      48 B |        1.00 |
| TryParse_Cuit_Utf8          | .NET 8.0  | .NET 8.0  |  64.44 ns | 0.110 ns | 0.097 ns |  1.00 |    0.00 | 0.0029 |      48 B |        1.00 |
| TryParse_Cuit_Utf8          | .NET 9.0  | .NET 9.0  |  63.17 ns | 0.185 ns | 0.173 ns |  0.98 |    0.00 | 0.0029 |      48 B |        1.00 |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_Cbu_String            | .NET 10.0 | .NET 10.0 |        NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
| Parse_Cbu_String            | .NET 8.0  | .NET 8.0  |        NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
| Parse_Cbu_String            | .NET 9.0  | .NET 9.0  |        NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_Cbu_Span              | .NET 10.0 | .NET 10.0 |        NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
| Parse_Cbu_Span              | .NET 8.0  | .NET 8.0  |        NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
| Parse_Cbu_Span              | .NET 9.0  | .NET 9.0  |        NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_Cbu_Utf8              | .NET 10.0 | .NET 10.0 |        NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
| Parse_Cbu_Utf8              | .NET 8.0  | .NET 8.0  |        NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
| Parse_Cbu_Utf8              | .NET 9.0  | .NET 9.0  |        NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_Money_String          | .NET 10.0 | .NET 10.0 |  93.80 ns | 0.106 ns | 0.089 ns |  0.85 |    0.00 |      - |         - |          NA |
| Parse_Money_String          | .NET 8.0  | .NET 8.0  | 110.10 ns | 0.126 ns | 0.105 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_Money_String          | .NET 9.0  | .NET 9.0  |  97.47 ns | 0.141 ns | 0.117 ns |  0.89 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_Money_Span            | .NET 10.0 | .NET 10.0 |  99.75 ns | 0.061 ns | 0.054 ns |  0.89 |    0.00 |      - |         - |          NA |
| Parse_Money_Span            | .NET 8.0  | .NET 8.0  | 112.02 ns | 0.157 ns | 0.131 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_Money_Span            | .NET 9.0  | .NET 9.0  | 101.21 ns | 0.211 ns | 0.197 ns |  0.90 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryParse_Money_Span         | .NET 10.0 | .NET 10.0 |  86.45 ns | 0.177 ns | 0.148 ns |  0.84 |    0.00 |      - |         - |          NA |
| TryParse_Money_Span         | .NET 8.0  | .NET 8.0  | 103.40 ns | 0.284 ns | 0.252 ns |  1.00 |    0.00 |      - |         - |          NA |
| TryParse_Money_Span         | .NET 9.0  | .NET 9.0  |  92.49 ns | 0.094 ns | 0.088 ns |  0.89 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryFormat_Money_Span        | .NET 10.0 | .NET 10.0 |  63.16 ns | 0.058 ns | 0.048 ns |  1.12 |    0.00 |      - |         - |          NA |
| TryFormat_Money_Span        | .NET 8.0  | .NET 8.0  |  56.57 ns | 0.038 ns | 0.034 ns |  1.00 |    0.00 |      - |         - |          NA |
| TryFormat_Money_Span        | .NET 9.0  | .NET 9.0  |  56.06 ns | 0.096 ns | 0.085 ns |  0.99 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_DateRange_String      | .NET 10.0 | .NET 10.0 | 285.29 ns | 0.515 ns | 0.456 ns |  0.97 |    0.00 |      - |         - |          NA |
| Parse_DateRange_String      | .NET 8.0  | .NET 8.0  | 293.21 ns | 0.207 ns | 0.173 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_DateRange_String      | .NET 9.0  | .NET 9.0  | 275.77 ns | 1.917 ns | 1.793 ns |  0.94 |    0.01 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_DateRange_Span        | .NET 10.0 | .NET 10.0 | 286.82 ns | 0.372 ns | 0.290 ns |  0.98 |    0.00 |      - |         - |          NA |
| Parse_DateRange_Span        | .NET 8.0  | .NET 8.0  | 293.13 ns | 0.242 ns | 0.215 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_DateRange_Span        | .NET 9.0  | .NET 9.0  | 278.33 ns | 2.823 ns | 2.641 ns |  0.95 |    0.01 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryParse_DateRange_Span     | .NET 10.0 | .NET 10.0 | 287.13 ns | 0.202 ns | 0.169 ns |  0.97 |    0.00 |      - |         - |          NA |
| TryParse_DateRange_Span     | .NET 8.0  | .NET 8.0  | 297.03 ns | 0.463 ns | 0.386 ns |  1.00 |    0.00 |      - |         - |          NA |
| TryParse_DateRange_Span     | .NET 9.0  | .NET 9.0  | 277.35 ns | 3.800 ns | 3.555 ns |  0.93 |    0.01 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_TimeRange_String      | .NET 10.0 | .NET 10.0 | 336.38 ns | 0.579 ns | 0.513 ns |  0.98 |    0.00 |      - |         - |          NA |
| Parse_TimeRange_String      | .NET 8.0  | .NET 8.0  | 341.96 ns | 0.307 ns | 0.240 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_TimeRange_String      | .NET 9.0  | .NET 9.0  | 312.82 ns | 5.159 ns | 4.826 ns |  0.91 |    0.01 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_TimeRange_Span        | .NET 10.0 | .NET 10.0 | 335.34 ns | 0.279 ns | 0.218 ns |  0.99 |    0.00 |      - |         - |          NA |
| Parse_TimeRange_Span        | .NET 8.0  | .NET 8.0  | 338.45 ns | 0.203 ns | 0.170 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_TimeRange_Span        | .NET 9.0  | .NET 9.0  | 311.97 ns | 3.525 ns | 3.297 ns |  0.92 |    0.01 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryParse_TimeRange_Span     | .NET 10.0 | .NET 10.0 | 331.75 ns | 0.257 ns | 0.215 ns |  0.98 |    0.00 |      - |         - |          NA |
| TryParse_TimeRange_Span     | .NET 8.0  | .NET 8.0  | 337.73 ns | 0.207 ns | 0.173 ns |  1.00 |    0.00 |      - |         - |          NA |
| TryParse_TimeRange_Span     | .NET 9.0  | .NET 9.0  | 310.32 ns | 5.337 ns | 4.731 ns |  0.92 |    0.01 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_GeoCoordinate_String  | .NET 10.0 | .NET 10.0 | 137.58 ns | 0.139 ns | 0.116 ns |  1.03 |    0.00 |      - |         - |          NA |
| Parse_GeoCoordinate_String  | .NET 8.0  | .NET 8.0  | 133.69 ns | 0.191 ns | 0.169 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_GeoCoordinate_String  | .NET 9.0  | .NET 9.0  | 122.50 ns | 1.738 ns | 1.625 ns |  0.92 |    0.01 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_GeoCoordinate_Span    | .NET 10.0 | .NET 10.0 | 133.39 ns | 0.404 ns | 0.337 ns |  0.97 |    0.00 |      - |         - |          NA |
| Parse_GeoCoordinate_Span    | .NET 8.0  | .NET 8.0  | 136.86 ns | 0.142 ns | 0.133 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_GeoCoordinate_Span    | .NET 9.0  | .NET 9.0  | 128.63 ns | 1.840 ns | 1.721 ns |  0.94 |    0.01 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryParse_GeoCoordinate_Span | .NET 10.0 | .NET 10.0 | 135.87 ns | 0.129 ns | 0.120 ns |  1.03 |    0.00 |      - |         - |          NA |
| TryParse_GeoCoordinate_Span | .NET 8.0  | .NET 8.0  | 132.09 ns | 0.137 ns | 0.114 ns |  1.00 |    0.00 |      - |         - |          NA |
| TryParse_GeoCoordinate_Span | .NET 9.0  | .NET 9.0  | 123.88 ns | 1.652 ns | 1.545 ns |  0.94 |    0.01 |      - |         - |          NA |

Benchmarks with issues:
  ParsingBenchmarks.Parse_Cbu_String: .NET 10.0(Runtime=.NET 10.0, Toolchain=net10.0)
  ParsingBenchmarks.Parse_Cbu_String: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ParsingBenchmarks.Parse_Cbu_String: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  ParsingBenchmarks.Parse_Cbu_Span: .NET 10.0(Runtime=.NET 10.0, Toolchain=net10.0)
  ParsingBenchmarks.Parse_Cbu_Span: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ParsingBenchmarks.Parse_Cbu_Span: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  ParsingBenchmarks.Parse_Cbu_Utf8: .NET 10.0(Runtime=.NET 10.0, Toolchain=net10.0)
  ParsingBenchmarks.Parse_Cbu_Utf8: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ParsingBenchmarks.Parse_Cbu_Utf8: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
