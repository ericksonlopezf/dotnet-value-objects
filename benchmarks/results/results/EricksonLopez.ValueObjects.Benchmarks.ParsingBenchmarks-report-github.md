```

BenchmarkDotNet v0.15.8, Linux Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763 3.24GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]    : .NET 10.0.13 (10.0.13, 10.0.1326.50504), X64 RyuJIT x86-64-v3
  .NET 10.0 : .NET 10.0.13 (10.0.13, 10.0.1326.50504), X64 RyuJIT x86-64-v3
  .NET 8.0  : .NET 8.0.32 (8.0.32, 8.0.3226.50405), X64 RyuJIT x86-64-v3
  .NET 9.0  : .NET 9.0.21 (9.0.21, 9.0.2126.50404), X64 RyuJIT x86-64-v3


```
| Method                      | Job       | Runtime   | Mean      | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------------------------- |---------- |---------- |----------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| Parse_Percentage_String     | .NET 10.0 | .NET 10.0 |  55.34 ns | 0.054 ns | 0.045 ns |  0.77 |    0.00 |      - |         - |          NA |
| Parse_Percentage_String     | .NET 8.0  | .NET 8.0  |  71.83 ns | 0.046 ns | 0.036 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_Percentage_String     | .NET 9.0  | .NET 9.0  |  65.75 ns | 0.069 ns | 0.058 ns |  0.92 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_Percentage_Span       | .NET 10.0 | .NET 10.0 |  69.83 ns | 0.170 ns | 0.151 ns |  0.76 |    0.00 |      - |         - |          NA |
| Parse_Percentage_Span       | .NET 8.0  | .NET 8.0  |  91.57 ns | 0.150 ns | 0.125 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_Percentage_Span       | .NET 9.0  | .NET 9.0  |  85.57 ns | 0.037 ns | 0.033 ns |  0.93 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryParse_Percentage_Span    | .NET 10.0 | .NET 10.0 |  48.83 ns | 0.109 ns | 0.085 ns |  0.66 |    0.00 |      - |         - |          NA |
| TryParse_Percentage_Span    | .NET 8.0  | .NET 8.0  |  74.41 ns | 0.233 ns | 0.206 ns |  1.00 |    0.00 |      - |         - |          NA |
| TryParse_Percentage_Span    | .NET 9.0  | .NET 9.0  |  63.39 ns | 0.094 ns | 0.083 ns |  0.85 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_Cuit_String           | .NET 10.0 | .NET 10.0 |  52.74 ns | 0.945 ns | 0.838 ns |  0.90 |    0.01 | 0.0029 |      48 B |        1.00 |
| Parse_Cuit_String           | .NET 8.0  | .NET 8.0  |  58.40 ns | 0.269 ns | 0.225 ns |  1.00 |    0.01 | 0.0029 |      48 B |        1.00 |
| Parse_Cuit_String           | .NET 9.0  | .NET 9.0  |  54.85 ns | 0.661 ns | 0.586 ns |  0.94 |    0.01 | 0.0029 |      48 B |        1.00 |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_Cuit_Span             | .NET 10.0 | .NET 10.0 |  59.34 ns | 0.351 ns | 0.311 ns |  1.03 |    0.01 | 0.0029 |      48 B |        1.00 |
| Parse_Cuit_Span             | .NET 8.0  | .NET 8.0  |  57.61 ns | 0.817 ns | 0.764 ns |  1.00 |    0.02 | 0.0029 |      48 B |        1.00 |
| Parse_Cuit_Span             | .NET 9.0  | .NET 9.0  |  55.38 ns | 0.457 ns | 0.427 ns |  0.96 |    0.01 | 0.0029 |      48 B |        1.00 |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_Cuit_Utf8             | .NET 10.0 | .NET 10.0 |  61.59 ns | 0.242 ns | 0.226 ns |  0.92 |    0.01 | 0.0029 |      48 B |        1.00 |
| Parse_Cuit_Utf8             | .NET 8.0  | .NET 8.0  |  67.10 ns | 0.617 ns | 0.577 ns |  1.00 |    0.01 | 0.0029 |      48 B |        1.00 |
| Parse_Cuit_Utf8             | .NET 9.0  | .NET 9.0  |  66.25 ns | 0.129 ns | 0.100 ns |  0.99 |    0.01 | 0.0029 |      48 B |        1.00 |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryParse_Cuit_Utf8          | .NET 10.0 | .NET 10.0 |  59.98 ns | 0.534 ns | 0.499 ns |  0.93 |    0.01 | 0.0029 |      48 B |        1.00 |
| TryParse_Cuit_Utf8          | .NET 8.0  | .NET 8.0  |  64.36 ns | 0.453 ns | 0.424 ns |  1.00 |    0.01 | 0.0029 |      48 B |        1.00 |
| TryParse_Cuit_Utf8          | .NET 9.0  | .NET 9.0  |  71.50 ns | 0.262 ns | 0.245 ns |  1.11 |    0.01 | 0.0029 |      48 B |        1.00 |
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
| Parse_Money_String          | .NET 10.0 | .NET 10.0 |  95.39 ns | 0.303 ns | 0.269 ns |  0.89 |    0.00 |      - |         - |          NA |
| Parse_Money_String          | .NET 8.0  | .NET 8.0  | 107.34 ns | 0.151 ns | 0.126 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_Money_String          | .NET 9.0  | .NET 9.0  |  98.76 ns | 0.273 ns | 0.242 ns |  0.92 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_Money_Span            | .NET 10.0 | .NET 10.0 | 100.35 ns | 0.196 ns | 0.163 ns |  0.90 |    0.00 |      - |         - |          NA |
| Parse_Money_Span            | .NET 8.0  | .NET 8.0  | 111.95 ns | 0.092 ns | 0.072 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_Money_Span            | .NET 9.0  | .NET 9.0  |  99.72 ns | 0.083 ns | 0.074 ns |  0.89 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryParse_Money_Span         | .NET 10.0 | .NET 10.0 |  92.44 ns | 0.220 ns | 0.183 ns |  0.90 |    0.00 |      - |         - |          NA |
| TryParse_Money_Span         | .NET 8.0  | .NET 8.0  | 103.06 ns | 0.073 ns | 0.065 ns |  1.00 |    0.00 |      - |         - |          NA |
| TryParse_Money_Span         | .NET 9.0  | .NET 9.0  |  95.92 ns | 0.214 ns | 0.200 ns |  0.93 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryFormat_Money_Span        | .NET 10.0 | .NET 10.0 |  62.23 ns | 0.096 ns | 0.085 ns |  1.08 |    0.00 |      - |         - |          NA |
| TryFormat_Money_Span        | .NET 8.0  | .NET 8.0  |  57.42 ns | 0.032 ns | 0.028 ns |  1.00 |    0.00 |      - |         - |          NA |
| TryFormat_Money_Span        | .NET 9.0  | .NET 9.0  |  54.45 ns | 0.021 ns | 0.018 ns |  0.95 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_DateRange_String      | .NET 10.0 | .NET 10.0 | 287.69 ns | 0.313 ns | 0.277 ns |  0.98 |    0.00 |      - |         - |          NA |
| Parse_DateRange_String      | .NET 8.0  | .NET 8.0  | 294.87 ns | 0.278 ns | 0.247 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_DateRange_String      | .NET 9.0  | .NET 9.0  | 280.75 ns | 0.531 ns | 0.471 ns |  0.95 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_DateRange_Span        | .NET 10.0 | .NET 10.0 | 291.19 ns | 1.412 ns | 1.320 ns |  0.99 |    0.00 |      - |         - |          NA |
| Parse_DateRange_Span        | .NET 8.0  | .NET 8.0  | 294.36 ns | 0.297 ns | 0.263 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_DateRange_Span        | .NET 9.0  | .NET 9.0  | 281.86 ns | 0.695 ns | 0.616 ns |  0.96 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryParse_DateRange_Span     | .NET 10.0 | .NET 10.0 | 284.65 ns | 0.282 ns | 0.235 ns |  0.96 |    0.00 |      - |         - |          NA |
| TryParse_DateRange_Span     | .NET 8.0  | .NET 8.0  | 296.19 ns | 0.464 ns | 0.388 ns |  1.00 |    0.00 |      - |         - |          NA |
| TryParse_DateRange_Span     | .NET 9.0  | .NET 9.0  | 308.39 ns | 0.507 ns | 0.474 ns |  1.04 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_TimeRange_String      | .NET 10.0 | .NET 10.0 | 335.15 ns | 0.719 ns | 0.600 ns |  0.99 |    0.00 |      - |         - |          NA |
| Parse_TimeRange_String      | .NET 8.0  | .NET 8.0  | 337.47 ns | 0.156 ns | 0.130 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_TimeRange_String      | .NET 9.0  | .NET 9.0  | 318.85 ns | 0.203 ns | 0.170 ns |  0.94 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_TimeRange_Span        | .NET 10.0 | .NET 10.0 | 335.40 ns | 0.481 ns | 0.426 ns |  0.98 |    0.00 |      - |         - |          NA |
| Parse_TimeRange_Span        | .NET 8.0  | .NET 8.0  | 341.14 ns | 0.200 ns | 0.188 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_TimeRange_Span        | .NET 9.0  | .NET 9.0  | 319.31 ns | 0.383 ns | 0.358 ns |  0.94 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryParse_TimeRange_Span     | .NET 10.0 | .NET 10.0 | 334.14 ns | 0.239 ns | 0.224 ns |  1.01 |    0.00 |      - |         - |          NA |
| TryParse_TimeRange_Span     | .NET 8.0  | .NET 8.0  | 332.02 ns | 0.230 ns | 0.204 ns |  1.00 |    0.00 |      - |         - |          NA |
| TryParse_TimeRange_Span     | .NET 9.0  | .NET 9.0  | 317.80 ns | 0.451 ns | 0.400 ns |  0.96 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_GeoCoordinate_String  | .NET 10.0 | .NET 10.0 | 138.58 ns | 0.136 ns | 0.114 ns |  1.01 |    0.00 |      - |         - |          NA |
| Parse_GeoCoordinate_String  | .NET 8.0  | .NET 8.0  | 136.97 ns | 0.156 ns | 0.130 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_GeoCoordinate_String  | .NET 9.0  | .NET 9.0  | 126.16 ns | 0.110 ns | 0.086 ns |  0.92 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_GeoCoordinate_Span    | .NET 10.0 | .NET 10.0 | 140.23 ns | 0.445 ns | 0.394 ns |  1.05 |    0.00 |      - |         - |          NA |
| Parse_GeoCoordinate_Span    | .NET 8.0  | .NET 8.0  | 133.53 ns | 0.168 ns | 0.140 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_GeoCoordinate_Span    | .NET 9.0  | .NET 9.0  | 127.35 ns | 0.162 ns | 0.135 ns |  0.95 |    0.00 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryParse_GeoCoordinate_Span | .NET 10.0 | .NET 10.0 | 140.50 ns | 0.264 ns | 0.247 ns |  1.03 |    0.00 |      - |         - |          NA |
| TryParse_GeoCoordinate_Span | .NET 8.0  | .NET 8.0  | 137.07 ns | 0.159 ns | 0.141 ns |  1.00 |    0.00 |      - |         - |          NA |
| TryParse_GeoCoordinate_Span | .NET 9.0  | .NET 9.0  | 124.79 ns | 0.236 ns | 0.197 ns |  0.91 |    0.00 |      - |         - |          NA |

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
