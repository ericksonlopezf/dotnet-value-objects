```

BenchmarkDotNet v0.15.8, Linux Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 9V45 4.31GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]    : .NET 10.0.14 (10.0.14, 10.0.1426.52326), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.14 (10.0.14, 10.0.1426.52326), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.33 (8.0.33, 8.0.3326.52219), X64 RyuJIT x86-64-v4
  .NET 9.0  : .NET 9.0.22 (9.0.22, 9.0.2226.52218), X64 RyuJIT x86-64-v4


```
| Method                      | Job       | Runtime   | Mean      | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------------------------- |---------- |---------- |----------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| Parse_Percentage_String     | .NET 10.0 | .NET 10.0 |  27.37 ns | 0.174 ns | 0.163 ns |  0.70 |    0.01 |      - |         - |          NA |
| Parse_Percentage_String     | .NET 8.0  | .NET 8.0  |  39.07 ns | 0.767 ns | 0.717 ns |  1.00 |    0.03 |      - |         - |          NA |
| Parse_Percentage_String     | .NET 9.0  | .NET 9.0  |  35.52 ns | 0.320 ns | 0.250 ns |  0.91 |    0.02 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_Percentage_Span       | .NET 10.0 | .NET 10.0 |  28.07 ns | 0.529 ns | 0.494 ns |  0.53 |    0.01 |      - |         - |          NA |
| Parse_Percentage_Span       | .NET 8.0  | .NET 8.0  |  52.57 ns | 1.045 ns | 1.161 ns |  1.00 |    0.03 |      - |         - |          NA |
| Parse_Percentage_Span       | .NET 9.0  | .NET 9.0  |  48.35 ns | 0.533 ns | 0.472 ns |  0.92 |    0.02 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryParse_Percentage_Span    | .NET 10.0 | .NET 10.0 |  22.86 ns | 0.300 ns | 0.266 ns |  0.61 |    0.01 |      - |         - |          NA |
| TryParse_Percentage_Span    | .NET 8.0  | .NET 8.0  |  37.31 ns | 0.690 ns | 0.612 ns |  1.00 |    0.02 |      - |         - |          NA |
| TryParse_Percentage_Span    | .NET 9.0  | .NET 9.0  |  32.24 ns | 0.429 ns | 0.380 ns |  0.86 |    0.02 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_Cuit_String           | .NET 10.0 | .NET 10.0 |  28.12 ns | 0.580 ns | 0.690 ns |  0.94 |    0.03 | 0.0029 |      48 B |        1.00 |
| Parse_Cuit_String           | .NET 8.0  | .NET 8.0  |  30.04 ns | 0.620 ns | 0.689 ns |  1.00 |    0.03 | 0.0029 |      48 B |        1.00 |
| Parse_Cuit_String           | .NET 9.0  | .NET 9.0  |  29.68 ns | 0.277 ns | 0.259 ns |  0.99 |    0.02 | 0.0029 |      48 B |        1.00 |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_Cuit_Span             | .NET 10.0 | .NET 10.0 |  30.95 ns | 0.548 ns | 0.486 ns |  1.02 |    0.03 | 0.0029 |      48 B |        1.00 |
| Parse_Cuit_Span             | .NET 8.0  | .NET 8.0  |  30.37 ns | 0.623 ns | 0.788 ns |  1.00 |    0.04 | 0.0029 |      48 B |        1.00 |
| Parse_Cuit_Span             | .NET 9.0  | .NET 9.0  |  29.80 ns | 0.418 ns | 0.349 ns |  0.98 |    0.03 | 0.0029 |      48 B |        1.00 |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_Cuit_Utf8             | .NET 10.0 | .NET 10.0 |  31.87 ns | 0.417 ns | 0.369 ns |  0.88 |    0.02 | 0.0029 |      48 B |        1.00 |
| Parse_Cuit_Utf8             | .NET 8.0  | .NET 8.0  |  36.25 ns | 0.704 ns | 0.659 ns |  1.00 |    0.02 | 0.0029 |      48 B |        1.00 |
| Parse_Cuit_Utf8             | .NET 9.0  | .NET 9.0  |  33.88 ns | 0.684 ns | 0.703 ns |  0.94 |    0.02 | 0.0029 |      48 B |        1.00 |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryParse_Cuit_Utf8          | .NET 10.0 | .NET 10.0 |  31.53 ns | 0.296 ns | 0.277 ns |  0.90 |    0.02 | 0.0029 |      48 B |        1.00 |
| TryParse_Cuit_Utf8          | .NET 8.0  | .NET 8.0  |  34.96 ns | 0.711 ns | 0.818 ns |  1.00 |    0.03 | 0.0029 |      48 B |        1.00 |
| TryParse_Cuit_Utf8          | .NET 9.0  | .NET 9.0  |  33.52 ns | 0.679 ns | 0.698 ns |  0.96 |    0.03 | 0.0029 |      48 B |        1.00 |
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
| Parse_Money_String          | .NET 10.0 | .NET 10.0 |  46.96 ns | 0.299 ns | 0.233 ns |  0.79 |    0.01 |      - |         - |          NA |
| Parse_Money_String          | .NET 8.0  | .NET 8.0  |  59.18 ns | 1.066 ns | 0.997 ns |  1.00 |    0.02 |      - |         - |          NA |
| Parse_Money_String          | .NET 9.0  | .NET 9.0  |  53.93 ns | 1.061 ns | 1.042 ns |  0.91 |    0.02 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_Money_Span            | .NET 10.0 | .NET 10.0 |  51.46 ns | 0.505 ns | 0.422 ns |  0.86 |    0.02 |      - |         - |          NA |
| Parse_Money_Span            | .NET 8.0  | .NET 8.0  |  59.93 ns | 1.131 ns | 1.058 ns |  1.00 |    0.02 |      - |         - |          NA |
| Parse_Money_Span            | .NET 9.0  | .NET 9.0  |  55.22 ns | 0.709 ns | 0.629 ns |  0.92 |    0.02 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryParse_Money_Span         | .NET 10.0 | .NET 10.0 |  39.59 ns | 0.644 ns | 0.602 ns |  0.72 |    0.02 |      - |         - |          NA |
| TryParse_Money_Span         | .NET 8.0  | .NET 8.0  |  54.88 ns | 1.053 ns | 0.985 ns |  1.00 |    0.02 |      - |         - |          NA |
| TryParse_Money_Span         | .NET 9.0  | .NET 9.0  |  51.98 ns | 0.520 ns | 0.461 ns |  0.95 |    0.02 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryFormat_Money_Span        | .NET 10.0 | .NET 10.0 |  33.85 ns | 0.671 ns | 0.824 ns |  1.19 |    0.03 |      - |         - |          NA |
| TryFormat_Money_Span        | .NET 8.0  | .NET 8.0  |  28.36 ns | 0.477 ns | 0.446 ns |  1.00 |    0.02 |      - |         - |          NA |
| TryFormat_Money_Span        | .NET 9.0  | .NET 9.0  |  27.93 ns | 0.235 ns | 0.208 ns |  0.99 |    0.02 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_DateRange_String      | .NET 10.0 | .NET 10.0 | 139.53 ns | 2.534 ns | 2.371 ns |  0.91 |    0.02 |      - |         - |          NA |
| Parse_DateRange_String      | .NET 8.0  | .NET 8.0  | 153.04 ns | 2.980 ns | 3.312 ns |  1.00 |    0.03 |      - |         - |          NA |
| Parse_DateRange_String      | .NET 9.0  | .NET 9.0  | 150.59 ns | 3.025 ns | 3.106 ns |  0.98 |    0.03 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_DateRange_Span        | .NET 10.0 | .NET 10.0 | 138.86 ns | 2.668 ns | 2.854 ns |  0.93 |    0.03 |      - |         - |          NA |
| Parse_DateRange_Span        | .NET 8.0  | .NET 8.0  | 148.75 ns | 2.883 ns | 3.432 ns |  1.00 |    0.03 |      - |         - |          NA |
| Parse_DateRange_Span        | .NET 9.0  | .NET 9.0  | 151.29 ns | 2.656 ns | 2.484 ns |  1.02 |    0.03 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryParse_DateRange_Span     | .NET 10.0 | .NET 10.0 | 134.99 ns | 1.339 ns | 1.118 ns |  0.89 |    0.02 |      - |         - |          NA |
| TryParse_DateRange_Span     | .NET 8.0  | .NET 8.0  | 151.29 ns | 2.999 ns | 3.454 ns |  1.00 |    0.03 |      - |         - |          NA |
| TryParse_DateRange_Span     | .NET 9.0  | .NET 9.0  | 145.48 ns | 2.467 ns | 2.060 ns |  0.96 |    0.03 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_TimeRange_String      | .NET 10.0 | .NET 10.0 | 193.04 ns | 3.127 ns | 2.772 ns |  0.96 |    0.02 |      - |         - |          NA |
| Parse_TimeRange_String      | .NET 8.0  | .NET 8.0  | 202.11 ns | 3.448 ns | 3.225 ns |  1.00 |    0.02 |      - |         - |          NA |
| Parse_TimeRange_String      | .NET 9.0  | .NET 9.0  | 192.46 ns | 3.719 ns | 3.297 ns |  0.95 |    0.02 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_TimeRange_Span        | .NET 10.0 | .NET 10.0 | 194.31 ns | 1.820 ns | 1.520 ns |  0.94 |    0.02 |      - |         - |          NA |
| Parse_TimeRange_Span        | .NET 8.0  | .NET 8.0  | 207.86 ns | 4.099 ns | 4.210 ns |  1.00 |    0.03 |      - |         - |          NA |
| Parse_TimeRange_Span        | .NET 9.0  | .NET 9.0  | 205.72 ns | 3.851 ns | 3.602 ns |  0.99 |    0.03 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryParse_TimeRange_Span     | .NET 10.0 | .NET 10.0 | 188.41 ns | 3.318 ns | 3.104 ns |  0.98 |    0.02 |      - |         - |          NA |
| TryParse_TimeRange_Span     | .NET 8.0  | .NET 8.0  | 193.21 ns | 1.847 ns | 1.542 ns |  1.00 |    0.01 |      - |         - |          NA |
| TryParse_TimeRange_Span     | .NET 9.0  | .NET 9.0  | 188.45 ns | 2.769 ns | 2.454 ns |  0.98 |    0.01 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_GeoCoordinate_String  | .NET 10.0 | .NET 10.0 |  63.98 ns | 1.296 ns | 1.331 ns |  0.96 |    0.02 |      - |         - |          NA |
| Parse_GeoCoordinate_String  | .NET 8.0  | .NET 8.0  |  66.87 ns | 0.839 ns | 0.701 ns |  1.00 |    0.01 |      - |         - |          NA |
| Parse_GeoCoordinate_String  | .NET 9.0  | .NET 9.0  |  60.07 ns | 1.157 ns | 1.026 ns |  0.90 |    0.02 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| Parse_GeoCoordinate_Span    | .NET 10.0 | .NET 10.0 |  63.23 ns | 1.272 ns | 1.361 ns |  0.94 |    0.02 |      - |         - |          NA |
| Parse_GeoCoordinate_Span    | .NET 8.0  | .NET 8.0  |  67.10 ns | 0.999 ns | 0.834 ns |  1.00 |    0.02 |      - |         - |          NA |
| Parse_GeoCoordinate_Span    | .NET 9.0  | .NET 9.0  |  59.18 ns | 0.617 ns | 0.577 ns |  0.88 |    0.01 |      - |         - |          NA |
|                             |           |           |           |          |          |       |         |        |           |             |
| TryParse_GeoCoordinate_Span | .NET 10.0 | .NET 10.0 |  63.66 ns | 1.297 ns | 1.388 ns |  0.98 |    0.03 |      - |         - |          NA |
| TryParse_GeoCoordinate_Span | .NET 8.0  | .NET 8.0  |  64.93 ns | 1.320 ns | 1.467 ns |  1.00 |    0.03 |      - |         - |          NA |
| TryParse_GeoCoordinate_Span | .NET 9.0  | .NET 9.0  |  61.10 ns | 1.191 ns | 1.114 ns |  0.94 |    0.03 |      - |         - |          NA |

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
