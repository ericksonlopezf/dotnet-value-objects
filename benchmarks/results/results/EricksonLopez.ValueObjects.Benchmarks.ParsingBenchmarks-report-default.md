
BenchmarkDotNet v0.15.8, Linux Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763 3.24GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]    : .NET 10.0.13 (10.0.13, 10.0.1326.50504), X64 RyuJIT x86-64-v3
  .NET 10.0 : .NET 10.0.13 (10.0.13, 10.0.1326.50504), X64 RyuJIT x86-64-v3
  .NET 8.0  : .NET 8.0.32 (8.0.32, 8.0.3226.50405), X64 RyuJIT x86-64-v3
  .NET 9.0  : .NET 9.0.21 (9.0.21, 9.0.2126.50404), X64 RyuJIT x86-64-v3


 Method                      | Job       | Runtime   | Mean      | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
---------------------------- |---------- |---------- |----------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
 Parse_Percentage_String     | .NET 10.0 | .NET 10.0 |  56.19 ns | 0.052 ns | 0.044 ns |  0.77 |    0.00 |      - |         - |          NA |
 Parse_Percentage_String     | .NET 8.0  | .NET 8.0  |  73.43 ns | 0.102 ns | 0.080 ns |  1.00 |    0.00 |      - |         - |          NA |
 Parse_Percentage_String     | .NET 9.0  | .NET 9.0  |  65.99 ns | 0.053 ns | 0.041 ns |  0.90 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_Percentage_Span       | .NET 10.0 | .NET 10.0 |  56.87 ns | 0.089 ns | 0.075 ns |  0.62 |    0.00 |      - |         - |          NA |
 Parse_Percentage_Span       | .NET 8.0  | .NET 8.0  |  92.18 ns | 0.135 ns | 0.113 ns |  1.00 |    0.00 |      - |         - |          NA |
 Parse_Percentage_Span       | .NET 9.0  | .NET 9.0  |  84.99 ns | 0.045 ns | 0.038 ns |  0.92 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 TryParse_Percentage_Span    | .NET 10.0 | .NET 10.0 |  50.42 ns | 0.341 ns | 0.285 ns |  0.70 |    0.00 |      - |         - |          NA |
 TryParse_Percentage_Span    | .NET 8.0  | .NET 8.0  |  71.67 ns | 0.149 ns | 0.140 ns |  1.00 |    0.00 |      - |         - |          NA |
 TryParse_Percentage_Span    | .NET 9.0  | .NET 9.0  |  62.53 ns | 0.103 ns | 0.086 ns |  0.87 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_Cuit_String           | .NET 10.0 | .NET 10.0 |  53.71 ns | 0.101 ns | 0.095 ns |  0.91 |    0.00 | 0.0029 |      48 B |        1.00 |
 Parse_Cuit_String           | .NET 8.0  | .NET 8.0  |  58.73 ns | 0.295 ns | 0.276 ns |  1.00 |    0.01 | 0.0029 |      48 B |        1.00 |
 Parse_Cuit_String           | .NET 9.0  | .NET 9.0  |  55.66 ns | 0.329 ns | 0.308 ns |  0.95 |    0.01 | 0.0029 |      48 B |        1.00 |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_Cuit_Span             | .NET 10.0 | .NET 10.0 |  59.29 ns | 0.102 ns | 0.090 ns |  1.01 |    0.01 | 0.0029 |      48 B |        1.00 |
 Parse_Cuit_Span             | .NET 8.0  | .NET 8.0  |  58.58 ns | 0.540 ns | 0.505 ns |  1.00 |    0.01 | 0.0029 |      48 B |        1.00 |
 Parse_Cuit_Span             | .NET 9.0  | .NET 9.0  |  55.82 ns | 0.168 ns | 0.157 ns |  0.95 |    0.01 | 0.0029 |      48 B |        1.00 |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_Cuit_Utf8             | .NET 10.0 | .NET 10.0 |  62.27 ns | 0.305 ns | 0.255 ns |  0.92 |    0.00 | 0.0029 |      48 B |        1.00 |
 Parse_Cuit_Utf8             | .NET 8.0  | .NET 8.0  |  67.65 ns | 0.279 ns | 0.261 ns |  1.00 |    0.01 | 0.0029 |      48 B |        1.00 |
 Parse_Cuit_Utf8             | .NET 9.0  | .NET 9.0  |  63.57 ns | 0.178 ns | 0.158 ns |  0.94 |    0.00 | 0.0029 |      48 B |        1.00 |
                             |           |           |           |          |          |       |         |        |           |             |
 TryParse_Cuit_Utf8          | .NET 10.0 | .NET 10.0 |  61.54 ns | 0.261 ns | 0.218 ns |  0.92 |    0.00 | 0.0029 |      48 B |        1.00 |
 TryParse_Cuit_Utf8          | .NET 8.0  | .NET 8.0  |  66.73 ns | 0.198 ns | 0.176 ns |  1.00 |    0.00 | 0.0029 |      48 B |        1.00 |
 TryParse_Cuit_Utf8          | .NET 9.0  | .NET 9.0  |  63.05 ns | 0.308 ns | 0.273 ns |  0.94 |    0.00 | 0.0029 |      48 B |        1.00 |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_Cbu_String            | .NET 10.0 | .NET 10.0 |        NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
 Parse_Cbu_String            | .NET 8.0  | .NET 8.0  |        NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
 Parse_Cbu_String            | .NET 9.0  | .NET 9.0  |        NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_Cbu_Span              | .NET 10.0 | .NET 10.0 |        NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
 Parse_Cbu_Span              | .NET 8.0  | .NET 8.0  |        NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
 Parse_Cbu_Span              | .NET 9.0  | .NET 9.0  |        NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_Cbu_Utf8              | .NET 10.0 | .NET 10.0 |        NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
 Parse_Cbu_Utf8              | .NET 8.0  | .NET 8.0  |        NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
 Parse_Cbu_Utf8              | .NET 9.0  | .NET 9.0  |        NA |       NA |       NA |     ? |       ? |     NA |        NA |           ? |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_Money_String          | .NET 10.0 | .NET 10.0 |  94.83 ns | 0.110 ns | 0.092 ns |  0.89 |    0.00 |      - |         - |          NA |
 Parse_Money_String          | .NET 8.0  | .NET 8.0  | 106.83 ns | 0.075 ns | 0.058 ns |  1.00 |    0.00 |      - |         - |          NA |
 Parse_Money_String          | .NET 9.0  | .NET 9.0  |  97.25 ns | 0.178 ns | 0.157 ns |  0.91 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_Money_Span            | .NET 10.0 | .NET 10.0 | 102.32 ns | 0.263 ns | 0.233 ns |  0.91 |    0.00 |      - |         - |          NA |
 Parse_Money_Span            | .NET 8.0  | .NET 8.0  | 112.76 ns | 0.230 ns | 0.192 ns |  1.00 |    0.00 |      - |         - |          NA |
 Parse_Money_Span            | .NET 9.0  | .NET 9.0  | 102.69 ns | 0.358 ns | 0.317 ns |  0.91 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 TryParse_Money_Span         | .NET 10.0 | .NET 10.0 |  83.42 ns | 0.280 ns | 0.219 ns |  0.82 |    0.00 |      - |         - |          NA |
 TryParse_Money_Span         | .NET 8.0  | .NET 8.0  | 101.78 ns | 0.121 ns | 0.107 ns |  1.00 |    0.00 |      - |         - |          NA |
 TryParse_Money_Span         | .NET 9.0  | .NET 9.0  |  99.72 ns | 0.181 ns | 0.142 ns |  0.98 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 TryFormat_Money_Span        | .NET 10.0 | .NET 10.0 |  63.27 ns | 0.126 ns | 0.106 ns |  0.96 |    0.00 |      - |         - |          NA |
 TryFormat_Money_Span        | .NET 8.0  | .NET 8.0  |  65.72 ns | 0.050 ns | 0.046 ns |  1.00 |    0.00 |      - |         - |          NA |
 TryFormat_Money_Span        | .NET 9.0  | .NET 9.0  |  54.91 ns | 0.127 ns | 0.112 ns |  0.84 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_DateRange_String      | .NET 10.0 | .NET 10.0 | 286.11 ns | 0.264 ns | 0.234 ns |  0.97 |    0.00 |      - |         - |          NA |
 Parse_DateRange_String      | .NET 8.0  | .NET 8.0  | 294.42 ns | 0.463 ns | 0.410 ns |  1.00 |    0.00 |      - |         - |          NA |
 Parse_DateRange_String      | .NET 9.0  | .NET 9.0  | 282.86 ns | 0.730 ns | 0.609 ns |  0.96 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_DateRange_Span        | .NET 10.0 | .NET 10.0 | 290.95 ns | 0.491 ns | 0.435 ns |  0.96 |    0.00 |      - |         - |          NA |
 Parse_DateRange_Span        | .NET 8.0  | .NET 8.0  | 301.53 ns | 0.294 ns | 0.260 ns |  1.00 |    0.00 |      - |         - |          NA |
 Parse_DateRange_Span        | .NET 9.0  | .NET 9.0  | 282.39 ns | 0.362 ns | 0.321 ns |  0.94 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 TryParse_DateRange_Span     | .NET 10.0 | .NET 10.0 | 291.50 ns | 0.359 ns | 0.280 ns |  0.99 |    0.00 |      - |         - |          NA |
 TryParse_DateRange_Span     | .NET 8.0  | .NET 8.0  | 294.08 ns | 0.227 ns | 0.189 ns |  1.00 |    0.00 |      - |         - |          NA |
 TryParse_DateRange_Span     | .NET 9.0  | .NET 9.0  | 277.43 ns | 0.442 ns | 0.369 ns |  0.94 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_TimeRange_String      | .NET 10.0 | .NET 10.0 | 336.61 ns | 0.236 ns | 0.184 ns |  0.98 |    0.00 |      - |         - |          NA |
 Parse_TimeRange_String      | .NET 8.0  | .NET 8.0  | 342.00 ns | 0.407 ns | 0.340 ns |  1.00 |    0.00 |      - |         - |          NA |
 Parse_TimeRange_String      | .NET 9.0  | .NET 9.0  | 319.18 ns | 0.233 ns | 0.207 ns |  0.93 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_TimeRange_Span        | .NET 10.0 | .NET 10.0 | 331.95 ns | 0.348 ns | 0.271 ns |  0.96 |    0.00 |      - |         - |          NA |
 Parse_TimeRange_Span        | .NET 8.0  | .NET 8.0  | 345.00 ns | 1.478 ns | 1.383 ns |  1.00 |    0.01 |      - |         - |          NA |
 Parse_TimeRange_Span        | .NET 9.0  | .NET 9.0  | 327.16 ns | 0.277 ns | 0.216 ns |  0.95 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 TryParse_TimeRange_Span     | .NET 10.0 | .NET 10.0 | 330.80 ns | 0.315 ns | 0.263 ns |  0.99 |    0.00 |      - |         - |          NA |
 TryParse_TimeRange_Span     | .NET 8.0  | .NET 8.0  | 334.23 ns | 0.527 ns | 0.467 ns |  1.00 |    0.00 |      - |         - |          NA |
 TryParse_TimeRange_Span     | .NET 9.0  | .NET 9.0  | 318.01 ns | 0.124 ns | 0.116 ns |  0.95 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_GeoCoordinate_String  | .NET 10.0 | .NET 10.0 | 141.35 ns | 0.516 ns | 0.482 ns |  1.04 |    0.00 |      - |         - |          NA |
 Parse_GeoCoordinate_String  | .NET 8.0  | .NET 8.0  | 136.37 ns | 0.076 ns | 0.064 ns |  1.00 |    0.00 |      - |         - |          NA |
 Parse_GeoCoordinate_String  | .NET 9.0  | .NET 9.0  | 134.53 ns | 0.620 ns | 0.580 ns |  0.99 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_GeoCoordinate_Span    | .NET 10.0 | .NET 10.0 | 134.86 ns | 0.610 ns | 0.541 ns |  1.01 |    0.00 |      - |         - |          NA |
 Parse_GeoCoordinate_Span    | .NET 8.0  | .NET 8.0  | 133.91 ns | 0.290 ns | 0.272 ns |  1.00 |    0.00 |      - |         - |          NA |
 Parse_GeoCoordinate_Span    | .NET 9.0  | .NET 9.0  | 126.82 ns | 0.149 ns | 0.140 ns |  0.95 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 TryParse_GeoCoordinate_Span | .NET 10.0 | .NET 10.0 | 141.06 ns | 0.395 ns | 0.350 ns |  1.08 |    0.01 |      - |         - |          NA |
 TryParse_GeoCoordinate_Span | .NET 8.0  | .NET 8.0  | 130.46 ns | 0.822 ns | 0.729 ns |  1.00 |    0.01 |      - |         - |          NA |
 TryParse_GeoCoordinate_Span | .NET 9.0  | .NET 9.0  | 139.51 ns | 0.306 ns | 0.256 ns |  1.07 |    0.01 |      - |         - |          NA |

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
