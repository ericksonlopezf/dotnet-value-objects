```

BenchmarkDotNet v0.15.8, Linux Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 9V74 3.66GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]    : .NET 10.0.15 (10.0.15, 10.0.1526.53008), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.15 (10.0.15, 10.0.1526.53008), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.34 (8.0.34, 8.0.3426.52217), X64 RyuJIT x86-64-v4
  .NET 9.0  : .NET 9.0.23 (9.0.23, 9.0.2326.52216), X64 RyuJIT x86-64-v4


```
| Method                      | Job       | Runtime   | Mean      | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------------------------- |---------- |---------- |----------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| Parse_Percentage_String     | .NET 10.0 | .NET 10.0 |  47.13 ns | 0.333 ns | 0.312 ns |  0.76 |    0.01 |      - |         - |          NA |
| Parse_Percentage_String     | .NET 8.0  | .NET 8.0  |  61.80 ns | 0.477 ns | 0.423 ns |  1.00 |    0.01 |      - |         - |          NA |
| Parse_Percentage_String     | .NET 9.0  | .NET 9.0  |  54.59 ns | 0.300 ns | 0.266 ns |  0.88 |    0.01 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_Percentage_Span       | .NET 10.0 | .NET 10.0 |  48.92 ns | 0.282 ns | 0.264 ns |  0.63 |    0.01 |      - |         - |          NA |
| Parse_Percentage_Span       | .NET 8.0  | .NET 8.0  |  77.36 ns | 0.596 ns | 0.557 ns |  1.00 |    0.01 |      - |         - |          NA |
| Parse_Percentage_Span       | .NET 9.0  | .NET 9.0  |  68.64 ns | 0.053 ns | 0.044 ns |  0.89 |    0.01 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryParse_Percentage_Span    | .NET 10.0 | .NET 10.0 |  40.33 ns | 0.045 ns | 0.040 ns |  0.67 |    0.00 |      - |         - |          NA |
| TryParse_Percentage_Span    | .NET 8.0  | .NET 8.0  |  60.37 ns | 0.426 ns | 0.378 ns |  1.00 |    0.01 |      - |         - |          NA |
| TryParse_Percentage_Span    | .NET 9.0  | .NET 9.0  |  51.34 ns | 0.054 ns | 0.045 ns |  0.85 |    0.01 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_Cuit_String           | .NET 10.0 | .NET 10.0 |  41.09 ns | 0.071 ns | 0.063 ns |  0.94 |    0.00 | 0.0029 |      48 B |        1.00 |
| Parse_Cuit_String           | .NET 8.0  | .NET 8.0  |  43.72 ns | 0.062 ns | 0.055 ns |  1.00 |    0.00 | 0.0029 |      48 B |        1.00 |
| Parse_Cuit_String           | .NET 9.0  | .NET 9.0  |  40.63 ns | 0.074 ns | 0.069 ns |  0.93 |    0.00 | 0.0029 |      48 B |        1.00 |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_Cuit_Span             | .NET 10.0 | .NET 10.0 |  47.43 ns | 0.117 ns | 0.104 ns |  1.07 |    0.01 | 0.0029 |      48 B |        1.00 |
| Parse_Cuit_Span             | .NET 8.0  | .NET 8.0  |  44.30 ns | 0.515 ns | 0.481 ns |  1.00 |    0.01 | 0.0029 |      48 B |        1.00 |
| Parse_Cuit_Span             | .NET 9.0  | .NET 9.0  |  41.16 ns | 0.062 ns | 0.052 ns |  0.93 |    0.01 | 0.0029 |      48 B |        1.00 |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_Cuit_Utf8             | .NET 10.0 | .NET 10.0 |  49.23 ns | 0.140 ns | 0.124 ns |  0.93 |    0.01 | 0.0029 |      48 B |        1.00 |
| Parse_Cuit_Utf8             | .NET 8.0  | .NET 8.0  |  53.04 ns | 0.438 ns | 0.410 ns |  1.00 |    0.01 | 0.0029 |      48 B |        1.00 |
| Parse_Cuit_Utf8             | .NET 9.0  | .NET 9.0  |  48.70 ns | 0.083 ns | 0.074 ns |  0.92 |    0.01 | 0.0029 |      48 B |        1.00 |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryParse_Cuit_Utf8          | .NET 10.0 | .NET 10.0 |  48.77 ns | 0.060 ns | 0.053 ns |  0.98 |    0.00 | 0.0029 |      48 B |        1.00 |
| TryParse_Cuit_Utf8          | .NET 8.0  | .NET 8.0  |  49.82 ns | 0.112 ns | 0.099 ns |  1.00 |    0.00 | 0.0029 |      48 B |        1.00 |
| TryParse_Cuit_Utf8          | .NET 9.0  | .NET 9.0  |  48.43 ns | 0.121 ns | 0.113 ns |  0.97 |    0.00 | 0.0029 |      48 B |        1.00 |
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
| Parse_Money_String          | .NET 10.0 | .NET 10.0 |  75.85 ns | 0.103 ns | 0.091 ns |  0.89 |    0.00 |      - |         - |          NA |
| Parse_Money_String          | .NET 8.0  | .NET 8.0  |  85.39 ns | 0.103 ns | 0.081 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_Money_String          | .NET 9.0  | .NET 9.0  |  80.52 ns | 0.057 ns | 0.053 ns |  0.94 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_Money_Span            | .NET 10.0 | .NET 10.0 |  81.80 ns | 0.322 ns | 0.301 ns |  0.96 |    0.01 |      - |         - |          NA |
| Parse_Money_Span            | .NET 8.0  | .NET 8.0  |  85.50 ns | 0.436 ns | 0.387 ns |  1.00 |    0.01 |      - |         - |          NA |
| Parse_Money_Span            | .NET 9.0  | .NET 9.0  |  82.34 ns | 0.049 ns | 0.038 ns |  0.96 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryParse_Money_Span         | .NET 10.0 | .NET 10.0 |  70.19 ns | 0.498 ns | 0.441 ns |  0.86 |    0.01 |      - |         - |          NA |
| TryParse_Money_Span         | .NET 8.0  | .NET 8.0  |  81.70 ns | 0.090 ns | 0.070 ns |  1.00 |    0.00 |      - |         - |          NA |
| TryParse_Money_Span         | .NET 9.0  | .NET 9.0  |  75.89 ns | 0.103 ns | 0.092 ns |  0.93 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryFormat_Money_Span        | .NET 10.0 | .NET 10.0 |  49.41 ns | 0.030 ns | 0.025 ns |  1.10 |    0.00 |      - |         - |          NA |
| TryFormat_Money_Span        | .NET 8.0  | .NET 8.0  |  44.74 ns | 0.037 ns | 0.029 ns |  1.00 |    0.00 |      - |         - |          NA |
| TryFormat_Money_Span        | .NET 9.0  | .NET 9.0  |  42.08 ns | 0.126 ns | 0.098 ns |  0.94 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_DateRange_String      | .NET 10.0 | .NET 10.0 | 210.99 ns | 1.264 ns | 1.120 ns |  0.95 |    0.01 |      - |         - |          NA |
| Parse_DateRange_String      | .NET 8.0  | .NET 8.0  | 222.36 ns | 1.222 ns | 1.084 ns |  1.00 |    0.01 |      - |         - |          NA |
| Parse_DateRange_String      | .NET 9.0  | .NET 9.0  | 212.80 ns | 0.367 ns | 0.326 ns |  0.96 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_DateRange_Span        | .NET 10.0 | .NET 10.0 | 206.70 ns | 0.183 ns | 0.153 ns |  0.95 |    0.00 |      - |         - |          NA |
| Parse_DateRange_Span        | .NET 8.0  | .NET 8.0  | 217.18 ns | 0.385 ns | 0.321 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_DateRange_Span        | .NET 9.0  | .NET 9.0  | 213.43 ns | 0.471 ns | 0.441 ns |  0.98 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryParse_DateRange_Span     | .NET 10.0 | .NET 10.0 | 206.65 ns | 0.165 ns | 0.128 ns |  0.95 |    0.00 |      - |         - |          NA |
| TryParse_DateRange_Span     | .NET 8.0  | .NET 8.0  | 216.74 ns | 0.228 ns | 0.191 ns |  1.00 |    0.00 |      - |         - |          NA |
| TryParse_DateRange_Span     | .NET 9.0  | .NET 9.0  | 217.47 ns | 0.157 ns | 0.131 ns |  1.00 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_TimeRange_String      | .NET 10.0 | .NET 10.0 | 257.52 ns | 0.346 ns | 0.289 ns |  0.93 |    0.00 |      - |         - |          NA |
| Parse_TimeRange_String      | .NET 8.0  | .NET 8.0  | 276.88 ns | 0.458 ns | 0.382 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_TimeRange_String      | .NET 9.0  | .NET 9.0  | 261.15 ns | 0.321 ns | 0.284 ns |  0.94 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_TimeRange_Span        | .NET 10.0 | .NET 10.0 | 253.97 ns | 0.421 ns | 0.394 ns |  0.88 |    0.00 |      - |         - |          NA |
| Parse_TimeRange_Span        | .NET 8.0  | .NET 8.0  | 288.32 ns | 1.733 ns | 1.621 ns |  1.00 |    0.01 |      - |         - |          NA |
| Parse_TimeRange_Span        | .NET 9.0  | .NET 9.0  | 255.78 ns | 0.577 ns | 0.540 ns |  0.89 |    0.01 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryParse_TimeRange_Span     | .NET 10.0 | .NET 10.0 | 252.25 ns | 1.830 ns | 1.712 ns |  0.93 |    0.01 |      - |         - |          NA |
| TryParse_TimeRange_Span     | .NET 8.0  | .NET 8.0  | 271.21 ns | 1.812 ns | 1.513 ns |  1.00 |    0.01 |      - |         - |          NA |
| TryParse_TimeRange_Span     | .NET 9.0  | .NET 9.0  | 252.97 ns | 0.514 ns | 0.455 ns |  0.93 |    0.01 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_GeoCoordinate_String  | .NET 10.0 | .NET 10.0 | 101.00 ns | 0.642 ns | 0.601 ns |  1.00 |    0.01 |      - |         - |          NA |
| Parse_GeoCoordinate_String  | .NET 8.0  | .NET 8.0  | 100.70 ns | 0.217 ns | 0.192 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_GeoCoordinate_String  | .NET 9.0  | .NET 9.0  |  92.99 ns | 0.167 ns | 0.148 ns |  0.92 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_GeoCoordinate_Span    | .NET 10.0 | .NET 10.0 | 101.49 ns | 0.083 ns | 0.070 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_GeoCoordinate_Span    | .NET 8.0  | .NET 8.0  | 101.55 ns | 0.485 ns | 0.430 ns |  1.00 |    0.01 |      - |         - |          NA |
| Parse_GeoCoordinate_Span    | .NET 9.0  | .NET 9.0  |  90.13 ns | 0.081 ns | 0.076 ns |  0.89 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryParse_GeoCoordinate_Span | .NET 10.0 | .NET 10.0 |  98.87 ns | 0.540 ns | 0.478 ns |  0.99 |    0.01 |      - |         - |          NA |
| TryParse_GeoCoordinate_Span | .NET 8.0  | .NET 8.0  |  99.42 ns | 0.855 ns | 0.758 ns |  1.00 |    0.01 |      - |         - |          NA |
| TryParse_GeoCoordinate_Span | .NET 9.0  | .NET 9.0  |  90.21 ns | 0.104 ns | 0.081 ns |  0.91 |    0.01 |      - |         - |          NA |

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
