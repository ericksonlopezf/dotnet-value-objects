```

BenchmarkDotNet v0.15.8, Linux Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74 2.75GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]    : .NET 10.0.14 (10.0.14, 10.0.1426.52326), X64 RyuJIT x86-64-v3
  .NET 10.0 : .NET 10.0.14 (10.0.14, 10.0.1426.52326), X64 RyuJIT x86-64-v3
  .NET 8.0  : .NET 8.0.33 (8.0.33, 8.0.3326.52219), X64 RyuJIT x86-64-v3
  .NET 9.0  : .NET 9.0.22 (9.0.22, 9.0.2226.52218), X64 RyuJIT x86-64-v3


```
| Method                      | Job       | Runtime   | Mean      | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------------------------- |---------- |---------- |----------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| Parse_Percentage_String     | .NET 10.0 | .NET 10.0 |  63.81 ns | 0.093 ns | 0.082 ns |  0.79 |    0.00 |      - |         - |          NA |
| Parse_Percentage_String     | .NET 8.0  | .NET 8.0  |  80.78 ns | 0.097 ns | 0.081 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_Percentage_String     | .NET 9.0  | .NET 9.0  |  71.08 ns | 0.116 ns | 0.097 ns |  0.88 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_Percentage_Span       | .NET 10.0 | .NET 10.0 |  61.77 ns | 0.141 ns | 0.125 ns |  0.63 |    0.00 |      - |         - |          NA |
| Parse_Percentage_Span       | .NET 8.0  | .NET 8.0  |  98.13 ns | 0.136 ns | 0.121 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_Percentage_Span       | .NET 9.0  | .NET 9.0  |  87.91 ns | 0.066 ns | 0.058 ns |  0.90 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryParse_Percentage_Span    | .NET 10.0 | .NET 10.0 |  52.60 ns | 0.056 ns | 0.050 ns |  0.67 |    0.00 |      - |         - |          NA |
| TryParse_Percentage_Span    | .NET 8.0  | .NET 8.0  |  78.15 ns | 0.110 ns | 0.103 ns |  1.00 |    0.00 |      - |         - |          NA |
| TryParse_Percentage_Span    | .NET 9.0  | .NET 9.0  |  65.01 ns | 0.080 ns | 0.075 ns |  0.83 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_Cuit_String           | .NET 10.0 | .NET 10.0 |  50.28 ns | 0.038 ns | 0.032 ns |  0.90 |    0.00 | 0.0029 |      48 B |        1.00 |
| Parse_Cuit_String           | .NET 8.0  | .NET 8.0  |  56.10 ns | 0.078 ns | 0.073 ns |  1.00 |    0.00 | 0.0029 |      48 B |        1.00 |
| Parse_Cuit_String           | .NET 9.0  | .NET 9.0  |  53.20 ns | 0.090 ns | 0.085 ns |  0.95 |    0.00 | 0.0029 |      48 B |        1.00 |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_Cuit_Span             | .NET 10.0 | .NET 10.0 |  57.37 ns | 0.144 ns | 0.134 ns |  1.00 |    0.00 | 0.0029 |      48 B |        1.00 |
| Parse_Cuit_Span             | .NET 8.0  | .NET 8.0  |  57.63 ns | 0.030 ns | 0.023 ns |  1.00 |    0.00 | 0.0029 |      48 B |        1.00 |
| Parse_Cuit_Span             | .NET 9.0  | .NET 9.0  |  54.76 ns | 0.085 ns | 0.076 ns |  0.95 |    0.00 | 0.0029 |      48 B |        1.00 |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_Cuit_Utf8             | .NET 10.0 | .NET 10.0 |  62.01 ns | 0.080 ns | 0.071 ns |  0.92 |    0.00 | 0.0029 |      48 B |        1.00 |
| Parse_Cuit_Utf8             | .NET 8.0  | .NET 8.0  |  67.25 ns | 0.142 ns | 0.126 ns |  1.00 |    0.00 | 0.0029 |      48 B |        1.00 |
| Parse_Cuit_Utf8             | .NET 9.0  | .NET 9.0  |  63.45 ns | 0.085 ns | 0.071 ns |  0.94 |    0.00 | 0.0029 |      48 B |        1.00 |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryParse_Cuit_Utf8          | .NET 10.0 | .NET 10.0 |  61.78 ns | 0.118 ns | 0.098 ns |  0.97 |    0.00 | 0.0029 |      48 B |        1.00 |
| TryParse_Cuit_Utf8          | .NET 8.0  | .NET 8.0  |  63.46 ns | 0.082 ns | 0.073 ns |  1.00 |    0.00 | 0.0029 |      48 B |        1.00 |
| TryParse_Cuit_Utf8          | .NET 9.0  | .NET 9.0  |  62.51 ns | 0.074 ns | 0.066 ns |  0.99 |    0.00 | 0.0029 |      48 B |        1.00 |
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
| Parse_Money_String          | .NET 10.0 | .NET 10.0 | 100.47 ns | 0.054 ns | 0.048 ns |  0.92 |    0.00 |      - |         - |          NA |
| Parse_Money_String          | .NET 8.0  | .NET 8.0  | 109.07 ns | 0.071 ns | 0.063 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_Money_String          | .NET 9.0  | .NET 9.0  | 104.73 ns | 0.038 ns | 0.030 ns |  0.96 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_Money_Span            | .NET 10.0 | .NET 10.0 | 107.48 ns | 0.096 ns | 0.085 ns |  0.97 |    0.00 |      - |         - |          NA |
| Parse_Money_Span            | .NET 8.0  | .NET 8.0  | 110.35 ns | 0.095 ns | 0.080 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_Money_Span            | .NET 9.0  | .NET 9.0  | 105.57 ns | 0.071 ns | 0.063 ns |  0.96 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryParse_Money_Span         | .NET 10.0 | .NET 10.0 |  89.90 ns | 0.175 ns | 0.146 ns |  0.86 |    0.00 |      - |         - |          NA |
| TryParse_Money_Span         | .NET 8.0  | .NET 8.0  | 104.32 ns | 0.100 ns | 0.084 ns |  1.00 |    0.00 |      - |         - |          NA |
| TryParse_Money_Span         | .NET 9.0  | .NET 9.0  | 100.00 ns | 0.082 ns | 0.073 ns |  0.96 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryFormat_Money_Span        | .NET 10.0 | .NET 10.0 |  62.33 ns | 0.056 ns | 0.049 ns |  1.10 |    0.00 |      - |         - |          NA |
| TryFormat_Money_Span        | .NET 8.0  | .NET 8.0  |  56.64 ns | 0.050 ns | 0.044 ns |  1.00 |    0.00 |      - |         - |          NA |
| TryFormat_Money_Span        | .NET 9.0  | .NET 9.0  |  54.21 ns | 0.046 ns | 0.041 ns |  0.96 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_DateRange_String      | .NET 10.0 | .NET 10.0 | 278.84 ns | 0.093 ns | 0.077 ns |  0.99 |    0.00 |      - |         - |          NA |
| Parse_DateRange_String      | .NET 8.0  | .NET 8.0  | 281.16 ns | 0.126 ns | 0.112 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_DateRange_String      | .NET 9.0  | .NET 9.0  | 280.19 ns | 0.247 ns | 0.219 ns |  1.00 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_DateRange_Span        | .NET 10.0 | .NET 10.0 | 263.43 ns | 0.999 ns | 0.885 ns |  0.90 |    0.00 |      - |         - |          NA |
| Parse_DateRange_Span        | .NET 8.0  | .NET 8.0  | 293.28 ns | 0.192 ns | 0.170 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_DateRange_Span        | .NET 9.0  | .NET 9.0  | 273.95 ns | 0.225 ns | 0.176 ns |  0.93 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryParse_DateRange_Span     | .NET 10.0 | .NET 10.0 | 266.31 ns | 0.285 ns | 0.266 ns |  0.93 |    0.00 |      - |         - |          NA |
| TryParse_DateRange_Span     | .NET 8.0  | .NET 8.0  | 285.53 ns | 0.190 ns | 0.159 ns |  1.00 |    0.00 |      - |         - |          NA |
| TryParse_DateRange_Span     | .NET 9.0  | .NET 9.0  | 271.69 ns | 0.169 ns | 0.132 ns |  0.95 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_TimeRange_String      | .NET 10.0 | .NET 10.0 | 331.13 ns | 0.464 ns | 0.411 ns |  0.98 |    0.00 |      - |         - |          NA |
| Parse_TimeRange_String      | .NET 8.0  | .NET 8.0  | 337.46 ns | 0.372 ns | 0.311 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_TimeRange_String      | .NET 9.0  | .NET 9.0  | 346.38 ns | 0.141 ns | 0.118 ns |  1.03 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_TimeRange_Span        | .NET 10.0 | .NET 10.0 | 328.20 ns | 0.192 ns | 0.179 ns |  0.97 |    0.00 |      - |         - |          NA |
| Parse_TimeRange_Span        | .NET 8.0  | .NET 8.0  | 340.08 ns | 0.400 ns | 0.355 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_TimeRange_Span        | .NET 9.0  | .NET 9.0  | 331.00 ns | 0.363 ns | 0.340 ns |  0.97 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryParse_TimeRange_Span     | .NET 10.0 | .NET 10.0 | 335.65 ns | 0.383 ns | 0.299 ns |  1.01 |    0.00 |      - |         - |          NA |
| TryParse_TimeRange_Span     | .NET 8.0  | .NET 8.0  | 332.84 ns | 0.653 ns | 0.579 ns |  1.00 |    0.00 |      - |         - |          NA |
| TryParse_TimeRange_Span     | .NET 9.0  | .NET 9.0  | 339.72 ns | 0.448 ns | 0.419 ns |  1.02 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_GeoCoordinate_String  | .NET 10.0 | .NET 10.0 | 134.62 ns | 0.083 ns | 0.074 ns |  1.01 |    0.00 |      - |         - |          NA |
| Parse_GeoCoordinate_String  | .NET 8.0  | .NET 8.0  | 133.57 ns | 0.097 ns | 0.081 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_GeoCoordinate_String  | .NET 9.0  | .NET 9.0  | 124.96 ns | 0.070 ns | 0.058 ns |  0.94 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_GeoCoordinate_Span    | .NET 10.0 | .NET 10.0 | 136.04 ns | 0.167 ns | 0.139 ns |  1.02 |    0.00 |      - |         - |          NA |
| Parse_GeoCoordinate_Span    | .NET 8.0  | .NET 8.0  | 132.79 ns | 0.136 ns | 0.121 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_GeoCoordinate_Span    | .NET 9.0  | .NET 9.0  | 123.93 ns | 0.203 ns | 0.180 ns |  0.93 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryParse_GeoCoordinate_Span | .NET 10.0 | .NET 10.0 | 134.58 ns | 0.457 ns | 0.405 ns |  1.03 |    0.00 |      - |         - |          NA |
| TryParse_GeoCoordinate_Span | .NET 8.0  | .NET 8.0  | 130.45 ns | 0.070 ns | 0.062 ns |  1.00 |    0.00 |      - |         - |          NA |
| TryParse_GeoCoordinate_Span | .NET 9.0  | .NET 9.0  | 120.43 ns | 0.144 ns | 0.128 ns |  0.92 |    0.00 |      - |         - |          NA |

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
