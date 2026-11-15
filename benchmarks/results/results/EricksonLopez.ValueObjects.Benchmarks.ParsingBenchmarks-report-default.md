
BenchmarkDotNet v0.15.8, Linux Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 9V74 3.69GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]    : .NET 10.0.15 (10.0.15, 10.0.1526.53008), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.15 (10.0.15, 10.0.1526.53008), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.34 (8.0.34, 8.0.3426.52217), X64 RyuJIT x86-64-v4
  .NET 9.0  : .NET 9.0.23 (9.0.23, 9.0.2326.52216), X64 RyuJIT x86-64-v4


 Method                      | Job       | Runtime   | Mean      | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
---------------------------- |---------- |---------- |----------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
 Parse_Percentage_String     | .NET 10.0 | .NET 10.0 |  43.43 ns | 0.048 ns | 0.043 ns |  0.70 |    0.00 |      - |         - |          NA |
 Parse_Percentage_String     | .NET 8.0  | .NET 8.0  |  61.86 ns | 0.057 ns | 0.048 ns |  1.00 |    0.00 |      - |         - |          NA |
 Parse_Percentage_String     | .NET 9.0  | .NET 9.0  |  53.45 ns | 0.069 ns | 0.061 ns |  0.86 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_Percentage_Span       | .NET 10.0 | .NET 10.0 |  57.45 ns | 0.405 ns | 0.379 ns |  0.73 |    0.00 |      - |         - |          NA |
 Parse_Percentage_Span       | .NET 8.0  | .NET 8.0  |  79.02 ns | 0.076 ns | 0.068 ns |  1.00 |    0.00 |      - |         - |          NA |
 Parse_Percentage_Span       | .NET 9.0  | .NET 9.0  |  68.17 ns | 0.118 ns | 0.092 ns |  0.86 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 TryParse_Percentage_Span    | .NET 10.0 | .NET 10.0 |  39.64 ns | 0.040 ns | 0.034 ns |  0.67 |    0.00 |      - |         - |          NA |
 TryParse_Percentage_Span    | .NET 8.0  | .NET 8.0  |  59.52 ns | 0.132 ns | 0.117 ns |  1.00 |    0.00 |      - |         - |          NA |
 TryParse_Percentage_Span    | .NET 9.0  | .NET 9.0  |  51.19 ns | 0.063 ns | 0.059 ns |  0.86 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_Cuit_String           | .NET 10.0 | .NET 10.0 |  42.10 ns | 0.181 ns | 0.151 ns |  0.96 |    0.00 | 0.0029 |      48 B |        1.00 |
 Parse_Cuit_String           | .NET 8.0  | .NET 8.0  |  43.88 ns | 0.128 ns | 0.113 ns |  1.00 |    0.00 | 0.0029 |      48 B |        1.00 |
 Parse_Cuit_String           | .NET 9.0  | .NET 9.0  |  39.19 ns | 0.085 ns | 0.079 ns |  0.89 |    0.00 | 0.0029 |      48 B |        1.00 |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_Cuit_Span             | .NET 10.0 | .NET 10.0 |  47.33 ns | 0.150 ns | 0.141 ns |  1.06 |    0.00 | 0.0029 |      48 B |        1.00 |
 Parse_Cuit_Span             | .NET 8.0  | .NET 8.0  |  44.45 ns | 0.178 ns | 0.149 ns |  1.00 |    0.00 | 0.0029 |      48 B |        1.00 |
 Parse_Cuit_Span             | .NET 9.0  | .NET 9.0  |  40.54 ns | 0.095 ns | 0.084 ns |  0.91 |    0.00 | 0.0029 |      48 B |        1.00 |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_Cuit_Utf8             | .NET 10.0 | .NET 10.0 |  49.42 ns | 0.154 ns | 0.128 ns |  0.94 |    0.00 | 0.0029 |      48 B |        1.00 |
 Parse_Cuit_Utf8             | .NET 8.0  | .NET 8.0  |  52.51 ns | 0.233 ns | 0.206 ns |  1.00 |    0.01 | 0.0029 |      48 B |        1.00 |
 Parse_Cuit_Utf8             | .NET 9.0  | .NET 9.0  |  48.39 ns | 0.098 ns | 0.087 ns |  0.92 |    0.00 | 0.0029 |      48 B |        1.00 |
                             |           |           |           |          |          |       |         |        |           |             |
 TryParse_Cuit_Utf8          | .NET 10.0 | .NET 10.0 |  50.55 ns | 0.179 ns | 0.159 ns |  1.02 |    0.00 | 0.0029 |      48 B |        1.00 |
 TryParse_Cuit_Utf8          | .NET 8.0  | .NET 8.0  |  49.52 ns | 0.084 ns | 0.070 ns |  1.00 |    0.00 | 0.0029 |      48 B |        1.00 |
 TryParse_Cuit_Utf8          | .NET 9.0  | .NET 9.0  |  48.25 ns | 0.071 ns | 0.063 ns |  0.97 |    0.00 | 0.0029 |      48 B |        1.00 |
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
 Parse_Money_String          | .NET 10.0 | .NET 10.0 |  77.50 ns | 0.459 ns | 0.407 ns |  0.90 |    0.01 |      - |         - |          NA |
 Parse_Money_String          | .NET 8.0  | .NET 8.0  |  85.97 ns | 0.271 ns | 0.240 ns |  1.00 |    0.00 |      - |         - |          NA |
 Parse_Money_String          | .NET 9.0  | .NET 9.0  |  79.77 ns | 0.116 ns | 0.091 ns |  0.93 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_Money_Span            | .NET 10.0 | .NET 10.0 |  83.58 ns | 0.086 ns | 0.067 ns |  0.98 |    0.00 |      - |         - |          NA |
 Parse_Money_Span            | .NET 8.0  | .NET 8.0  |  84.96 ns | 0.089 ns | 0.083 ns |  1.00 |    0.00 |      - |         - |          NA |
 Parse_Money_Span            | .NET 9.0  | .NET 9.0  |  84.14 ns | 0.100 ns | 0.084 ns |  0.99 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 TryParse_Money_Span         | .NET 10.0 | .NET 10.0 |  71.60 ns | 0.120 ns | 0.093 ns |  0.88 |    0.00 |      - |         - |          NA |
 TryParse_Money_Span         | .NET 8.0  | .NET 8.0  |  81.14 ns | 0.096 ns | 0.080 ns |  1.00 |    0.00 |      - |         - |          NA |
 TryParse_Money_Span         | .NET 9.0  | .NET 9.0  |  75.38 ns | 0.064 ns | 0.057 ns |  0.93 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 TryFormat_Money_Span        | .NET 10.0 | .NET 10.0 |  55.26 ns | 0.159 ns | 0.148 ns |  1.25 |    0.01 |      - |         - |          NA |
 TryFormat_Money_Span        | .NET 8.0  | .NET 8.0  |  44.27 ns | 0.249 ns | 0.233 ns |  1.00 |    0.01 |      - |         - |          NA |
 TryFormat_Money_Span        | .NET 9.0  | .NET 9.0  |  41.84 ns | 0.094 ns | 0.083 ns |  0.95 |    0.01 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_DateRange_String      | .NET 10.0 | .NET 10.0 | 209.42 ns | 0.108 ns | 0.084 ns |  0.93 |    0.00 |      - |         - |          NA |
 Parse_DateRange_String      | .NET 8.0  | .NET 8.0  | 224.23 ns | 0.640 ns | 0.535 ns |  1.00 |    0.00 |      - |         - |          NA |
 Parse_DateRange_String      | .NET 9.0  | .NET 9.0  | 212.40 ns | 0.319 ns | 0.283 ns |  0.95 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_DateRange_Span        | .NET 10.0 | .NET 10.0 | 208.08 ns | 0.367 ns | 0.306 ns |  0.95 |    0.00 |      - |         - |          NA |
 Parse_DateRange_Span        | .NET 8.0  | .NET 8.0  | 217.91 ns | 0.481 ns | 0.376 ns |  1.00 |    0.00 |      - |         - |          NA |
 Parse_DateRange_Span        | .NET 9.0  | .NET 9.0  | 213.72 ns | 0.473 ns | 0.420 ns |  0.98 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 TryParse_DateRange_Span     | .NET 10.0 | .NET 10.0 | 206.99 ns | 0.278 ns | 0.247 ns |  0.95 |    0.00 |      - |         - |          NA |
 TryParse_DateRange_Span     | .NET 8.0  | .NET 8.0  | 217.79 ns | 0.737 ns | 0.654 ns |  1.00 |    0.00 |      - |         - |          NA |
 TryParse_DateRange_Span     | .NET 9.0  | .NET 9.0  | 209.90 ns | 0.431 ns | 0.403 ns |  0.96 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_TimeRange_String      | .NET 10.0 | .NET 10.0 | 257.84 ns | 1.264 ns | 1.121 ns |  0.94 |    0.00 |      - |         - |          NA |
 Parse_TimeRange_String      | .NET 8.0  | .NET 8.0  | 273.93 ns | 0.319 ns | 0.266 ns |  1.00 |    0.00 |      - |         - |          NA |
 Parse_TimeRange_String      | .NET 9.0  | .NET 9.0  | 265.44 ns | 0.440 ns | 0.411 ns |  0.97 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_TimeRange_Span        | .NET 10.0 | .NET 10.0 | 254.42 ns | 0.456 ns | 0.356 ns |  0.88 |    0.00 |      - |         - |          NA |
 Parse_TimeRange_Span        | .NET 8.0  | .NET 8.0  | 290.21 ns | 0.613 ns | 0.479 ns |  1.00 |    0.00 |      - |         - |          NA |
 Parse_TimeRange_Span        | .NET 9.0  | .NET 9.0  | 255.49 ns | 0.712 ns | 0.666 ns |  0.88 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 TryParse_TimeRange_Span     | .NET 10.0 | .NET 10.0 | 251.33 ns | 2.035 ns | 1.904 ns |  0.92 |    0.01 |      - |         - |          NA |
 TryParse_TimeRange_Span     | .NET 8.0  | .NET 8.0  | 272.24 ns | 1.770 ns | 1.655 ns |  1.00 |    0.01 |      - |         - |          NA |
 TryParse_TimeRange_Span     | .NET 9.0  | .NET 9.0  | 255.53 ns | 0.331 ns | 0.309 ns |  0.94 |    0.01 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_GeoCoordinate_String  | .NET 10.0 | .NET 10.0 | 101.14 ns | 0.527 ns | 0.493 ns |  0.97 |    0.01 |      - |         - |          NA |
 Parse_GeoCoordinate_String  | .NET 8.0  | .NET 8.0  | 104.43 ns | 0.730 ns | 0.683 ns |  1.00 |    0.01 |      - |         - |          NA |
 Parse_GeoCoordinate_String  | .NET 9.0  | .NET 9.0  |  89.64 ns | 0.196 ns | 0.184 ns |  0.86 |    0.01 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_GeoCoordinate_Span    | .NET 10.0 | .NET 10.0 | 101.94 ns | 0.491 ns | 0.459 ns |  1.02 |    0.01 |      - |         - |          NA |
 Parse_GeoCoordinate_Span    | .NET 8.0  | .NET 8.0  | 100.37 ns | 0.405 ns | 0.359 ns |  1.00 |    0.00 |      - |         - |          NA |
 Parse_GeoCoordinate_Span    | .NET 9.0  | .NET 9.0  |  89.91 ns | 0.048 ns | 0.037 ns |  0.90 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 TryParse_GeoCoordinate_Span | .NET 10.0 | .NET 10.0 |  98.18 ns | 0.517 ns | 0.459 ns |  0.97 |    0.00 |      - |         - |          NA |
 TryParse_GeoCoordinate_Span | .NET 8.0  | .NET 8.0  | 101.57 ns | 0.127 ns | 0.106 ns |  1.00 |    0.00 |      - |         - |          NA |
 TryParse_GeoCoordinate_Span | .NET 9.0  | .NET 9.0  |  90.07 ns | 0.170 ns | 0.151 ns |  0.89 |    0.00 |      - |         - |          NA |

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
