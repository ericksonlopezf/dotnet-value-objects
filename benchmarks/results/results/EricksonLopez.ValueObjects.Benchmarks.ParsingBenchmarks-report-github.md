```

BenchmarkDotNet v0.15.8, Linux Ubuntu 24.04.5 LTS (Noble Numbat)
Intel Xeon 6973P-C 2.60GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]    : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.31 (8.0.31, 8.0.3126.42015), X64 RyuJIT x86-64-v4
  .NET 9.0  : .NET 9.0.20 (9.0.20, 9.0.2026.41315), X64 RyuJIT x86-64-v4


```
| Method                      | Job       | Runtime   | Mean      | Error    | StdDev   | Median    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------------------------- |---------- |---------- |----------:|---------:|---------:|----------:|------:|--------:|-------:|----------:|------------:|
| Parse_Percentage_String     | .NET 10.0 | .NET 10.0 |  33.89 ns | 0.680 ns | 0.698 ns |  33.99 ns |  0.73 |    0.02 |      - |         - |          NA |
| Parse_Percentage_String     | .NET 8.0  | .NET 8.0  |  46.61 ns | 0.810 ns | 1.261 ns |  46.27 ns |  1.00 |    0.04 |      - |         - |          NA |
| Parse_Percentage_String     | .NET 9.0  | .NET 9.0  |  38.82 ns | 0.290 ns | 0.242 ns |  38.74 ns |  0.83 |    0.02 |      - |         - |          NA |
|                             |           |           |           |          |          |           |       |         |        |           |             |
| Parse_Percentage_Span       | .NET 10.0 | .NET 10.0 |  33.99 ns | 0.241 ns | 0.214 ns |  34.04 ns |  0.58 |    0.02 |      - |         - |          NA |
| Parse_Percentage_Span       | .NET 8.0  | .NET 8.0  |  58.66 ns | 1.181 ns | 2.069 ns |  58.58 ns |  1.00 |    0.05 |      - |         - |          NA |
| Parse_Percentage_Span       | .NET 9.0  | .NET 9.0  |  53.83 ns | 0.398 ns | 0.333 ns |  53.73 ns |  0.92 |    0.03 |      - |         - |          NA |
|                             |           |           |           |          |          |           |       |         |        |           |             |
| TryParse_Percentage_Span    | .NET 10.0 | .NET 10.0 |  31.65 ns | 0.500 ns | 0.443 ns |  31.60 ns |  0.61 |    0.02 |      - |         - |          NA |
| TryParse_Percentage_Span    | .NET 8.0  | .NET 8.0  |  51.51 ns | 1.016 ns | 1.129 ns |  51.38 ns |  1.00 |    0.03 |      - |         - |          NA |
| TryParse_Percentage_Span    | .NET 9.0  | .NET 9.0  |  37.38 ns | 0.760 ns | 1.269 ns |  36.89 ns |  0.73 |    0.03 |      - |         - |          NA |
|                             |           |           |           |          |          |           |       |         |        |           |             |
| Parse_Cuit_String           | .NET 10.0 | .NET 10.0 |  33.16 ns | 0.444 ns | 0.393 ns |  33.30 ns |  0.74 |    0.02 | 0.0005 |      48 B |        1.00 |
| Parse_Cuit_String           | .NET 8.0  | .NET 8.0  |  44.59 ns | 0.907 ns | 1.179 ns |  44.62 ns |  1.00 |    0.04 | 0.0005 |      48 B |        1.00 |
| Parse_Cuit_String           | .NET 9.0  | .NET 9.0  |  32.12 ns | 0.437 ns | 0.341 ns |  31.99 ns |  0.72 |    0.02 | 0.0005 |      48 B |        1.00 |
|                             |           |           |           |          |          |           |       |         |        |           |             |
| Parse_Cuit_Span             | .NET 10.0 | .NET 10.0 |  37.43 ns | 0.621 ns | 0.581 ns |  37.28 ns |  0.96 |    0.02 | 0.0005 |      48 B |        1.00 |
| Parse_Cuit_Span             | .NET 8.0  | .NET 8.0  |  39.07 ns | 0.331 ns | 0.276 ns |  39.07 ns |  1.00 |    0.01 | 0.0005 |      48 B |        1.00 |
| Parse_Cuit_Span             | .NET 9.0  | .NET 9.0  |  32.99 ns | 0.358 ns | 0.318 ns |  33.00 ns |  0.84 |    0.01 | 0.0005 |      48 B |        1.00 |
|                             |           |           |           |          |          |           |       |         |        |           |             |
| Parse_Cuit_Utf8             | .NET 10.0 | .NET 10.0 |  39.27 ns | 0.819 ns | 0.876 ns |  39.33 ns |  0.93 |    0.03 | 0.0005 |      48 B |        1.00 |
| Parse_Cuit_Utf8             | .NET 8.0  | .NET 8.0  |  42.34 ns | 0.857 ns | 0.986 ns |  42.18 ns |  1.00 |    0.03 | 0.0005 |      48 B |        1.00 |
| Parse_Cuit_Utf8             | .NET 9.0  | .NET 9.0  |  39.85 ns | 0.740 ns | 1.216 ns |  39.54 ns |  0.94 |    0.04 | 0.0005 |      48 B |        1.00 |
|                             |           |           |           |          |          |           |       |         |        |           |             |
| TryParse_Cuit_Utf8          | .NET 10.0 | .NET 10.0 |  39.54 ns | 0.790 ns | 1.134 ns |  39.66 ns |  0.92 |    0.03 | 0.0005 |      48 B |        1.00 |
| TryParse_Cuit_Utf8          | .NET 8.0  | .NET 8.0  |  43.00 ns | 0.861 ns | 0.846 ns |  42.92 ns |  1.00 |    0.03 | 0.0005 |      48 B |        1.00 |
| TryParse_Cuit_Utf8          | .NET 9.0  | .NET 9.0  |  38.33 ns | 0.784 ns | 1.150 ns |  37.66 ns |  0.89 |    0.03 | 0.0005 |      48 B |        1.00 |
|                             |           |           |           |          |          |           |       |         |        |           |             |
| Parse_Cbu_String            | .NET 10.0 | .NET 10.0 |        NA |       NA |       NA |        NA |     ? |       ? |     NA |        NA |           ? |
| Parse_Cbu_String            | .NET 8.0  | .NET 8.0  |        NA |       NA |       NA |        NA |     ? |       ? |     NA |        NA |           ? |
| Parse_Cbu_String            | .NET 9.0  | .NET 9.0  |        NA |       NA |       NA |        NA |     ? |       ? |     NA |        NA |           ? |
|                             |           |           |           |          |          |           |       |         |        |           |             |
| Parse_Cbu_Span              | .NET 10.0 | .NET 10.0 |        NA |       NA |       NA |        NA |     ? |       ? |     NA |        NA |           ? |
| Parse_Cbu_Span              | .NET 8.0  | .NET 8.0  |        NA |       NA |       NA |        NA |     ? |       ? |     NA |        NA |           ? |
| Parse_Cbu_Span              | .NET 9.0  | .NET 9.0  |        NA |       NA |       NA |        NA |     ? |       ? |     NA |        NA |           ? |
|                             |           |           |           |          |          |           |       |         |        |           |             |
| Parse_Cbu_Utf8              | .NET 10.0 | .NET 10.0 |        NA |       NA |       NA |        NA |     ? |       ? |     NA |        NA |           ? |
| Parse_Cbu_Utf8              | .NET 8.0  | .NET 8.0  |        NA |       NA |       NA |        NA |     ? |       ? |     NA |        NA |           ? |
| Parse_Cbu_Utf8              | .NET 9.0  | .NET 9.0  |        NA |       NA |       NA |        NA |     ? |       ? |     NA |        NA |           ? |
|                             |           |           |           |          |          |           |       |         |        |           |             |
| Parse_Money_String          | .NET 10.0 | .NET 10.0 |  60.52 ns | 1.238 ns | 1.814 ns |  60.57 ns |  0.85 |    0.03 |      - |         - |          NA |
| Parse_Money_String          | .NET 8.0  | .NET 8.0  |  71.39 ns | 1.182 ns | 1.213 ns |  71.40 ns |  1.00 |    0.02 |      - |         - |          NA |
| Parse_Money_String          | .NET 9.0  | .NET 9.0  |  60.79 ns | 1.171 ns | 1.151 ns |  60.62 ns |  0.85 |    0.02 |      - |         - |          NA |
|                             |           |           |           |          |          |           |       |         |        |           |             |
| Parse_Money_Span            | .NET 10.0 | .NET 10.0 |  65.81 ns | 1.117 ns | 0.990 ns |  65.60 ns |  0.92 |    0.03 |      - |         - |          NA |
| Parse_Money_Span            | .NET 8.0  | .NET 8.0  |  71.63 ns | 1.448 ns | 2.122 ns |  71.59 ns |  1.00 |    0.04 |      - |         - |          NA |
| Parse_Money_Span            | .NET 9.0  | .NET 9.0  |  60.68 ns | 0.987 ns | 0.875 ns |  60.52 ns |  0.85 |    0.03 |      - |         - |          NA |
|                             |           |           |           |          |          |           |       |         |        |           |             |
| TryParse_Money_Span         | .NET 10.0 | .NET 10.0 |  53.16 ns | 0.833 ns | 0.738 ns |  52.96 ns |  0.82 |    0.02 |      - |         - |          NA |
| TryParse_Money_Span         | .NET 8.0  | .NET 8.0  |  65.16 ns | 1.306 ns | 1.698 ns |  64.81 ns |  1.00 |    0.04 |      - |         - |          NA |
| TryParse_Money_Span         | .NET 9.0  | .NET 9.0  |  56.69 ns | 0.475 ns | 0.397 ns |  56.72 ns |  0.87 |    0.02 |      - |         - |          NA |
|                             |           |           |           |          |          |           |       |         |        |           |             |
| TryFormat_Money_Span        | .NET 10.0 | .NET 10.0 |  39.74 ns | 0.820 ns | 1.122 ns |  39.41 ns |  1.18 |    0.04 |      - |         - |          NA |
| TryFormat_Money_Span        | .NET 8.0  | .NET 8.0  |  33.55 ns | 0.631 ns | 0.560 ns |  33.62 ns |  1.00 |    0.02 |      - |         - |          NA |
| TryFormat_Money_Span        | .NET 9.0  | .NET 9.0  |  32.23 ns | 0.380 ns | 0.480 ns |  32.13 ns |  0.96 |    0.02 |      - |         - |          NA |
|                             |           |           |           |          |          |           |       |         |        |           |             |
| Parse_DateRange_String      | .NET 10.0 | .NET 10.0 | 161.55 ns | 2.852 ns | 2.528 ns | 161.66 ns |  0.96 |    0.04 |      - |         - |          NA |
| Parse_DateRange_String      | .NET 8.0  | .NET 8.0  | 167.76 ns | 3.345 ns | 5.945 ns | 165.83 ns |  1.00 |    0.05 |      - |         - |          NA |
| Parse_DateRange_String      | .NET 9.0  | .NET 9.0  | 162.08 ns | 3.269 ns | 5.279 ns | 160.25 ns |  0.97 |    0.05 |      - |         - |          NA |
|                             |           |           |           |          |          |           |       |         |        |           |             |
| Parse_DateRange_Span        | .NET 10.0 | .NET 10.0 | 167.67 ns | 2.678 ns | 2.505 ns | 167.70 ns |  1.03 |    0.02 |      - |         - |          NA |
| Parse_DateRange_Span        | .NET 8.0  | .NET 8.0  | 162.89 ns | 0.526 ns | 0.411 ns | 162.90 ns |  1.00 |    0.00 |      - |         - |          NA |
| Parse_DateRange_Span        | .NET 9.0  | .NET 9.0  | 164.35 ns | 3.297 ns | 5.134 ns | 161.18 ns |  1.01 |    0.03 |      - |         - |          NA |
|                             |           |           |           |          |          |           |       |         |        |           |             |
| TryParse_DateRange_Span     | .NET 10.0 | .NET 10.0 | 155.94 ns | 1.801 ns | 1.597 ns | 155.81 ns |  0.95 |    0.03 |      - |         - |          NA |
| TryParse_DateRange_Span     | .NET 8.0  | .NET 8.0  | 164.47 ns | 3.225 ns | 5.207 ns | 162.77 ns |  1.00 |    0.04 |      - |         - |          NA |
| TryParse_DateRange_Span     | .NET 9.0  | .NET 9.0  | 159.07 ns | 0.976 ns | 0.815 ns | 159.06 ns |  0.97 |    0.03 |      - |         - |          NA |
|                             |           |           |           |          |          |           |       |         |        |           |             |
| Parse_TimeRange_String      | .NET 10.0 | .NET 10.0 | 202.41 ns | 3.983 ns | 5.317 ns | 203.07 ns |  1.02 |    0.03 |      - |         - |          NA |
| Parse_TimeRange_String      | .NET 8.0  | .NET 8.0  | 199.16 ns | 3.997 ns | 4.105 ns | 200.02 ns |  1.00 |    0.03 |      - |         - |          NA |
| Parse_TimeRange_String      | .NET 9.0  | .NET 9.0  | 195.16 ns | 1.412 ns | 1.252 ns | 195.15 ns |  0.98 |    0.02 |      - |         - |          NA |
|                             |           |           |           |          |          |           |       |         |        |           |             |
| Parse_TimeRange_Span        | .NET 10.0 | .NET 10.0 | 203.60 ns | 2.536 ns | 2.118 ns | 204.12 ns |  1.04 |    0.02 |      - |         - |          NA |
| Parse_TimeRange_Span        | .NET 8.0  | .NET 8.0  | 196.29 ns | 3.868 ns | 4.300 ns | 195.21 ns |  1.00 |    0.03 |      - |         - |          NA |
| Parse_TimeRange_Span        | .NET 9.0  | .NET 9.0  | 197.02 ns | 1.104 ns | 0.978 ns | 196.75 ns |  1.00 |    0.02 |      - |         - |          NA |
|                             |           |           |           |          |          |           |       |         |        |           |             |
| TryParse_TimeRange_Span     | .NET 10.0 | .NET 10.0 | 204.62 ns | 3.990 ns | 5.723 ns | 203.95 ns |  1.06 |    0.04 |      - |         - |          NA |
| TryParse_TimeRange_Span     | .NET 8.0  | .NET 8.0  | 193.89 ns | 3.847 ns | 5.003 ns | 191.03 ns |  1.00 |    0.04 |      - |         - |          NA |
| TryParse_TimeRange_Span     | .NET 9.0  | .NET 9.0  | 190.92 ns | 3.708 ns | 3.642 ns | 189.57 ns |  0.99 |    0.03 |      - |         - |          NA |
|                             |           |           |           |          |          |           |       |         |        |           |             |
| Parse_GeoCoordinate_String  | .NET 10.0 | .NET 10.0 |  77.60 ns | 1.014 ns | 0.899 ns |  77.49 ns |  0.78 |    0.02 |      - |         - |          NA |
| Parse_GeoCoordinate_String  | .NET 8.0  | .NET 8.0  | 100.04 ns | 2.002 ns | 3.057 ns |  99.32 ns |  1.00 |    0.04 |      - |         - |          NA |
| Parse_GeoCoordinate_String  | .NET 9.0  | .NET 9.0  |  88.09 ns | 0.462 ns | 0.361 ns |  88.15 ns |  0.88 |    0.03 |      - |         - |          NA |
|                             |           |           |           |          |          |           |       |         |        |           |             |
| Parse_GeoCoordinate_Span    | .NET 10.0 | .NET 10.0 |  77.59 ns | 1.399 ns | 1.240 ns |  77.95 ns |  0.77 |    0.01 |      - |         - |          NA |
| Parse_GeoCoordinate_Span    | .NET 8.0  | .NET 8.0  | 100.49 ns | 0.722 ns | 0.564 ns | 100.33 ns |  1.00 |    0.01 |      - |         - |          NA |
| Parse_GeoCoordinate_Span    | .NET 9.0  | .NET 9.0  |  81.14 ns | 0.789 ns | 0.699 ns |  81.01 ns |  0.81 |    0.01 |      - |         - |          NA |
|                             |           |           |           |          |          |           |       |         |        |           |             |
| TryParse_GeoCoordinate_Span | .NET 10.0 | .NET 10.0 |  77.12 ns | 1.214 ns | 1.014 ns |  76.84 ns |  0.79 |    0.01 |      - |         - |          NA |
| TryParse_GeoCoordinate_Span | .NET 8.0  | .NET 8.0  |  97.84 ns | 0.852 ns | 0.665 ns |  97.72 ns |  1.00 |    0.01 |      - |         - |          NA |
| TryParse_GeoCoordinate_Span | .NET 9.0  | .NET 9.0  |  81.56 ns | 0.953 ns | 0.892 ns |  81.59 ns |  0.83 |    0.01 |      - |         - |          NA |

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
