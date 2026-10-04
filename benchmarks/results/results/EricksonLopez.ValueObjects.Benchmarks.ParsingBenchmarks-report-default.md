
BenchmarkDotNet v0.15.8, Linux Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763 2.45GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]    : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v3
  .NET 10.0 : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v3
  .NET 8.0  : .NET 8.0.31 (8.0.31, 8.0.3126.42015), X64 RyuJIT x86-64-v3
  .NET 9.0  : .NET 9.0.20 (9.0.20, 9.0.2026.41315), X64 RyuJIT x86-64-v3


 Method                      | Job       | Runtime   | Mean      | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
---------------------------- |---------- |---------- |----------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
 Parse_Percentage_String     | .NET 10.0 | .NET 10.0 |  54.86 ns | 0.073 ns | 0.064 ns |  0.71 |    0.00 |      - |         - |          NA |
 Parse_Percentage_String     | .NET 8.0  | .NET 8.0  |  76.76 ns | 0.347 ns | 0.308 ns |  1.00 |    0.01 |      - |         - |          NA |
 Parse_Percentage_String     | .NET 9.0  | .NET 9.0  |  66.25 ns | 0.198 ns | 0.176 ns |  0.86 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_Percentage_Span       | .NET 10.0 | .NET 10.0 |  57.01 ns | 0.103 ns | 0.086 ns |  0.62 |    0.00 |      - |         - |          NA |
 Parse_Percentage_Span       | .NET 8.0  | .NET 8.0  |  91.56 ns | 0.077 ns | 0.065 ns |  1.00 |    0.00 |      - |         - |          NA |
 Parse_Percentage_Span       | .NET 9.0  | .NET 9.0  |  86.12 ns | 0.159 ns | 0.148 ns |  0.94 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 TryParse_Percentage_Span    | .NET 10.0 | .NET 10.0 |  51.45 ns | 0.137 ns | 0.114 ns |  0.69 |    0.00 |      - |         - |          NA |
 TryParse_Percentage_Span    | .NET 8.0  | .NET 8.0  |  74.06 ns | 0.091 ns | 0.076 ns |  1.00 |    0.00 |      - |         - |          NA |
 TryParse_Percentage_Span    | .NET 9.0  | .NET 9.0  |  62.07 ns | 0.191 ns | 0.179 ns |  0.84 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_Cuit_String           | .NET 10.0 | .NET 10.0 |  54.39 ns | 0.223 ns | 0.208 ns |  0.90 |    0.00 | 0.0029 |      48 B |        1.00 |
 Parse_Cuit_String           | .NET 8.0  | .NET 8.0  |  60.21 ns | 0.099 ns | 0.092 ns |  1.00 |    0.00 | 0.0029 |      48 B |        1.00 |
 Parse_Cuit_String           | .NET 9.0  | .NET 9.0  |  54.96 ns | 0.435 ns | 0.386 ns |  0.91 |    0.01 | 0.0029 |      48 B |        1.00 |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_Cuit_Span             | .NET 10.0 | .NET 10.0 |  58.78 ns | 0.332 ns | 0.295 ns |  0.99 |    0.01 | 0.0029 |      48 B |        1.00 |
 Parse_Cuit_Span             | .NET 8.0  | .NET 8.0  |  59.53 ns | 0.119 ns | 0.105 ns |  1.00 |    0.00 | 0.0029 |      48 B |        1.00 |
 Parse_Cuit_Span             | .NET 9.0  | .NET 9.0  |  55.21 ns | 0.190 ns | 0.178 ns |  0.93 |    0.00 | 0.0029 |      48 B |        1.00 |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_Cuit_Utf8             | .NET 10.0 | .NET 10.0 |  62.85 ns | 0.142 ns | 0.125 ns |  0.94 |    0.00 | 0.0029 |      48 B |        1.00 |
 Parse_Cuit_Utf8             | .NET 8.0  | .NET 8.0  |  67.00 ns | 0.072 ns | 0.064 ns |  1.00 |    0.00 | 0.0029 |      48 B |        1.00 |
 Parse_Cuit_Utf8             | .NET 9.0  | .NET 9.0  |  62.10 ns | 0.155 ns | 0.138 ns |  0.93 |    0.00 | 0.0029 |      48 B |        1.00 |
                             |           |           |           |          |          |       |         |        |           |             |
 TryParse_Cuit_Utf8          | .NET 10.0 | .NET 10.0 |  60.99 ns | 0.298 ns | 0.279 ns |  0.92 |    0.00 | 0.0029 |      48 B |        1.00 |
 TryParse_Cuit_Utf8          | .NET 8.0  | .NET 8.0  |  66.58 ns | 0.166 ns | 0.139 ns |  1.00 |    0.00 | 0.0029 |      48 B |        1.00 |
 TryParse_Cuit_Utf8          | .NET 9.0  | .NET 9.0  |  62.63 ns | 0.116 ns | 0.109 ns |  0.94 |    0.00 | 0.0029 |      48 B |        1.00 |
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
 Parse_Money_String          | .NET 10.0 | .NET 10.0 |  97.18 ns | 0.160 ns | 0.125 ns |  0.93 |    0.00 |      - |         - |          NA |
 Parse_Money_String          | .NET 8.0  | .NET 8.0  | 104.21 ns | 0.141 ns | 0.118 ns |  1.00 |    0.00 |      - |         - |          NA |
 Parse_Money_String          | .NET 9.0  | .NET 9.0  |  96.48 ns | 0.101 ns | 0.089 ns |  0.93 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_Money_Span            | .NET 10.0 | .NET 10.0 | 102.36 ns | 0.113 ns | 0.100 ns |  0.91 |    0.00 |      - |         - |          NA |
 Parse_Money_Span            | .NET 8.0  | .NET 8.0  | 113.08 ns | 0.097 ns | 0.081 ns |  1.00 |    0.00 |      - |         - |          NA |
 Parse_Money_Span            | .NET 9.0  | .NET 9.0  | 101.91 ns | 0.181 ns | 0.151 ns |  0.90 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 TryParse_Money_Span         | .NET 10.0 | .NET 10.0 |  88.42 ns | 0.236 ns | 0.209 ns |  0.86 |    0.00 |      - |         - |          NA |
 TryParse_Money_Span         | .NET 8.0  | .NET 8.0  | 102.40 ns | 0.102 ns | 0.080 ns |  1.00 |    0.00 |      - |         - |          NA |
 TryParse_Money_Span         | .NET 9.0  | .NET 9.0  | 101.33 ns | 0.177 ns | 0.165 ns |  0.99 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 TryFormat_Money_Span        | .NET 10.0 | .NET 10.0 |  62.88 ns | 0.134 ns | 0.112 ns |  1.11 |    0.00 |      - |         - |          NA |
 TryFormat_Money_Span        | .NET 8.0  | .NET 8.0  |  56.75 ns | 0.081 ns | 0.072 ns |  1.00 |    0.00 |      - |         - |          NA |
 TryFormat_Money_Span        | .NET 9.0  | .NET 9.0  |  56.41 ns | 0.067 ns | 0.059 ns |  0.99 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_DateRange_String      | .NET 10.0 | .NET 10.0 | 290.49 ns | 0.278 ns | 0.260 ns |  0.99 |    0.00 |      - |         - |          NA |
 Parse_DateRange_String      | .NET 8.0  | .NET 8.0  | 294.35 ns | 0.326 ns | 0.272 ns |  1.00 |    0.00 |      - |         - |          NA |
 Parse_DateRange_String      | .NET 9.0  | .NET 9.0  | 291.08 ns | 0.654 ns | 0.612 ns |  0.99 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_DateRange_Span        | .NET 10.0 | .NET 10.0 | 283.34 ns | 0.502 ns | 0.445 ns |  0.97 |    0.00 |      - |         - |          NA |
 Parse_DateRange_Span        | .NET 8.0  | .NET 8.0  | 291.55 ns | 1.033 ns | 0.966 ns |  1.00 |    0.00 |      - |         - |          NA |
 Parse_DateRange_Span        | .NET 9.0  | .NET 9.0  | 282.16 ns | 0.433 ns | 0.405 ns |  0.97 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 TryParse_DateRange_Span     | .NET 10.0 | .NET 10.0 | 283.18 ns | 0.442 ns | 0.345 ns |  0.96 |    0.00 |      - |         - |          NA |
 TryParse_DateRange_Span     | .NET 8.0  | .NET 8.0  | 294.81 ns | 0.225 ns | 0.199 ns |  1.00 |    0.00 |      - |         - |          NA |
 TryParse_DateRange_Span     | .NET 9.0  | .NET 9.0  | 277.15 ns | 0.378 ns | 0.315 ns |  0.94 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_TimeRange_String      | .NET 10.0 | .NET 10.0 | 334.26 ns | 0.530 ns | 0.469 ns |  0.95 |    0.00 |      - |         - |          NA |
 Parse_TimeRange_String      | .NET 8.0  | .NET 8.0  | 351.12 ns | 0.306 ns | 0.271 ns |  1.00 |    0.00 |      - |         - |          NA |
 Parse_TimeRange_String      | .NET 9.0  | .NET 9.0  | 323.23 ns | 0.277 ns | 0.232 ns |  0.92 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_TimeRange_Span        | .NET 10.0 | .NET 10.0 | 334.79 ns | 0.301 ns | 0.251 ns |  0.97 |    0.00 |      - |         - |          NA |
 Parse_TimeRange_Span        | .NET 8.0  | .NET 8.0  | 346.45 ns | 0.552 ns | 0.489 ns |  1.00 |    0.00 |      - |         - |          NA |
 Parse_TimeRange_Span        | .NET 9.0  | .NET 9.0  | 327.67 ns | 0.279 ns | 0.233 ns |  0.95 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 TryParse_TimeRange_Span     | .NET 10.0 | .NET 10.0 | 332.29 ns | 0.319 ns | 0.249 ns |  1.00 |    0.00 |      - |         - |          NA |
 TryParse_TimeRange_Span     | .NET 8.0  | .NET 8.0  | 333.86 ns | 0.470 ns | 0.416 ns |  1.00 |    0.00 |      - |         - |          NA |
 TryParse_TimeRange_Span     | .NET 9.0  | .NET 9.0  | 318.78 ns | 0.190 ns | 0.149 ns |  0.95 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_GeoCoordinate_String  | .NET 10.0 | .NET 10.0 | 145.66 ns | 0.129 ns | 0.120 ns |  1.06 |    0.00 |      - |         - |          NA |
 Parse_GeoCoordinate_String  | .NET 8.0  | .NET 8.0  | 136.92 ns | 0.177 ns | 0.157 ns |  1.00 |    0.00 |      - |         - |          NA |
 Parse_GeoCoordinate_String  | .NET 9.0  | .NET 9.0  | 126.34 ns | 0.191 ns | 0.159 ns |  0.92 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 Parse_GeoCoordinate_Span    | .NET 10.0 | .NET 10.0 | 146.55 ns | 0.467 ns | 0.437 ns |  1.05 |    0.00 |      - |         - |          NA |
 Parse_GeoCoordinate_Span    | .NET 8.0  | .NET 8.0  | 138.99 ns | 0.064 ns | 0.054 ns |  1.00 |    0.00 |      - |         - |          NA |
 Parse_GeoCoordinate_Span    | .NET 9.0  | .NET 9.0  | 145.18 ns | 0.434 ns | 0.406 ns |  1.04 |    0.00 |      - |         - |          NA |
                             |           |           |           |          |          |       |         |        |           |             |
 TryParse_GeoCoordinate_Span | .NET 10.0 | .NET 10.0 | 137.63 ns | 0.180 ns | 0.150 ns |  1.00 |    0.00 |      - |         - |          NA |
 TryParse_GeoCoordinate_Span | .NET 8.0  | .NET 8.0  | 137.19 ns | 0.186 ns | 0.155 ns |  1.00 |    0.00 |      - |         - |          NA |
 TryParse_GeoCoordinate_Span | .NET 9.0  | .NET 9.0  | 125.70 ns | 0.163 ns | 0.153 ns |  0.92 |    0.00 |      - |         - |          NA |

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
