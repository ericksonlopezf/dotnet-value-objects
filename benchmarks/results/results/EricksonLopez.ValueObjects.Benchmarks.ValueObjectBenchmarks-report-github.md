```

BenchmarkDotNet v0.15.8, Linux Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763 3.24GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.402
  [Host]    : .NET 10.0.13 (10.0.13, 10.0.1326.50504), X64 RyuJIT x86-64-v3
  .NET 10.0 : .NET 10.0.13 (10.0.13, 10.0.1326.50504), X64 RyuJIT x86-64-v3
  .NET 8.0  : .NET 8.0.32 (8.0.32, 8.0.3226.50405), X64 RyuJIT x86-64-v3
  .NET 9.0  : .NET 9.0.21 (9.0.21, 9.0.2126.50404), X64 RyuJIT x86-64-v3


```
| Method                        | Job       | Runtime   | Mean        | Error     | StdDev    | Median      | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------ |---------- |---------- |------------:|----------:|----------:|------------:|------:|--------:|-------:|----------:|------------:|
| Create_Class                  | .NET 10.0 | .NET 10.0 |   7.2288 ns | 0.0709 ns | 0.0663 ns |   7.2322 ns |  0.46 |    0.02 | 0.0014 |      24 B |        1.00 |
| Create_Class                  | .NET 8.0  | .NET 8.0  |  15.5819 ns | 0.3828 ns | 0.7466 ns |  15.7120 ns |  1.00 |    0.07 | 0.0014 |      24 B |        1.00 |
| Create_Class                  | .NET 9.0  | .NET 9.0  |   8.4242 ns | 0.2341 ns | 0.2504 ns |   8.4583 ns |  0.54 |    0.03 | 0.0014 |      24 B |        1.00 |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Create_RecordClass            | .NET 10.0 | .NET 10.0 |   7.3208 ns | 0.1035 ns | 0.0917 ns |   7.3418 ns |  0.41 |    0.02 | 0.0014 |      24 B |        1.00 |
| Create_RecordClass            | .NET 8.0  | .NET 8.0  |  17.8667 ns | 0.4296 ns | 1.0293 ns |  18.0435 ns |  1.00 |    0.08 | 0.0014 |      24 B |        1.00 |
| Create_RecordClass            | .NET 9.0  | .NET 9.0  |   8.2533 ns | 0.0865 ns | 0.0810 ns |   8.2227 ns |  0.46 |    0.03 | 0.0014 |      24 B |        1.00 |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Create_Struct                 | .NET 10.0 | .NET 10.0 |   0.0004 ns | 0.0009 ns | 0.0007 ns |   0.0000 ns |     ? |       ? |      - |         - |           ? |
| Create_Struct                 | .NET 8.0  | .NET 8.0  |   0.0003 ns | 0.0007 ns | 0.0006 ns |   0.0000 ns |     ? |       ? |      - |         - |           ? |
| Create_Struct                 | .NET 9.0  | .NET 9.0  |   0.0003 ns | 0.0005 ns | 0.0004 ns |   0.0001 ns |     ? |       ? |      - |         - |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Create_RecordStruct           | .NET 10.0 | .NET 10.0 |   0.0001 ns | 0.0003 ns | 0.0002 ns |   0.0000 ns |     ? |       ? |      - |         - |           ? |
| Create_RecordStruct           | .NET 8.0  | .NET 8.0  |   0.0006 ns | 0.0013 ns | 0.0011 ns |   0.0000 ns |     ? |       ? |      - |         - |           ? |
| Create_RecordStruct           | .NET 9.0  | .NET 9.0  |   0.0003 ns | 0.0004 ns | 0.0003 ns |   0.0000 ns |     ? |       ? |      - |         - |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Create_Email           | .NET 10.0 | .NET 10.0 | 219.8438 ns | 1.3341 ns | 1.1827 ns | 219.8424 ns |  0.86 |    0.01 | 0.0186 |     312 B |        1.00 |
| Domain_Create_Email           | .NET 8.0  | .NET 8.0  | 255.1119 ns | 2.9602 ns | 2.7689 ns | 255.5000 ns |  1.00 |    0.01 | 0.0186 |     312 B |        1.00 |
| Domain_Create_Email           | .NET 9.0  | .NET 9.0  | 249.7958 ns | 0.8149 ns | 0.7623 ns | 249.9297 ns |  0.98 |    0.01 | 0.0186 |     312 B |        1.00 |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Create_Money           | .NET 10.0 | .NET 10.0 |  11.7493 ns | 0.0087 ns | 0.0068 ns |  11.7501 ns |  0.50 |    0.00 |      - |         - |          NA |
| Domain_Create_Money           | .NET 8.0  | .NET 8.0  |  23.6260 ns | 0.0129 ns | 0.0101 ns |  23.6265 ns |  1.00 |    0.00 |      - |         - |          NA |
| Domain_Create_Money           | .NET 9.0  | .NET 9.0  |  19.7779 ns | 0.0197 ns | 0.0174 ns |  19.7714 ns |  0.84 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Create_Percentage      | .NET 10.0 | .NET 10.0 |   8.0507 ns | 0.0074 ns | 0.0065 ns |   8.0489 ns |  0.37 |    0.00 |      - |         - |          NA |
| Domain_Create_Percentage      | .NET 8.0  | .NET 8.0  |  21.7267 ns | 0.0232 ns | 0.0206 ns |  21.7255 ns |  1.00 |    0.00 |      - |         - |          NA |
| Domain_Create_Percentage      | .NET 9.0  | .NET 9.0  |  20.3246 ns | 0.0114 ns | 0.0101 ns |  20.3257 ns |  0.94 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Equals_Class                  | .NET 10.0 | .NET 10.0 |   0.5845 ns | 0.0064 ns | 0.0056 ns |   0.5843 ns |  1.18 |    0.02 |      - |         - |          NA |
| Equals_Class                  | .NET 8.0  | .NET 8.0  |   0.4957 ns | 0.0086 ns | 0.0080 ns |   0.4966 ns |  1.00 |    0.02 |      - |         - |          NA |
| Equals_Class                  | .NET 9.0  | .NET 9.0  |   0.5806 ns | 0.0061 ns | 0.0054 ns |   0.5788 ns |  1.17 |    0.02 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Equals_RecordClass            | .NET 10.0 | .NET 10.0 |   0.5500 ns | 0.0053 ns | 0.0047 ns |   0.5496 ns |  0.83 |    0.01 |      - |         - |          NA |
| Equals_RecordClass            | .NET 8.0  | .NET 8.0  |   0.6601 ns | 0.0050 ns | 0.0044 ns |   0.6588 ns |  1.00 |    0.01 |      - |         - |          NA |
| Equals_RecordClass            | .NET 9.0  | .NET 9.0  |   0.6171 ns | 0.0052 ns | 0.0048 ns |   0.6167 ns |  0.93 |    0.01 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Equals_Struct                 | .NET 10.0 | .NET 10.0 |   0.4642 ns | 0.0015 ns | 0.0013 ns |   0.4646 ns |  1.02 |    0.01 |      - |         - |          NA |
| Equals_Struct                 | .NET 8.0  | .NET 8.0  |   0.4548 ns | 0.0033 ns | 0.0031 ns |   0.4536 ns |  1.00 |    0.01 |      - |         - |          NA |
| Equals_Struct                 | .NET 9.0  | .NET 9.0  |   0.3132 ns | 0.0063 ns | 0.0059 ns |   0.3114 ns |  0.69 |    0.01 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Equals_RecordStruct           | .NET 10.0 | .NET 10.0 |   0.3282 ns | 0.0051 ns | 0.0048 ns |   0.3284 ns |  1.43 |    0.03 |      - |         - |          NA |
| Equals_RecordStruct           | .NET 8.0  | .NET 8.0  |   0.2297 ns | 0.0029 ns | 0.0026 ns |   0.2304 ns |  1.00 |    0.02 |      - |         - |          NA |
| Equals_RecordStruct           | .NET 9.0  | .NET 9.0  |   0.3108 ns | 0.0036 ns | 0.0032 ns |   0.3105 ns |  1.35 |    0.02 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Equals_Money           | .NET 10.0 | .NET 10.0 |   5.0892 ns | 0.0111 ns | 0.0098 ns |   5.0859 ns |  0.98 |    0.00 |      - |         - |          NA |
| Domain_Equals_Money           | .NET 8.0  | .NET 8.0  |   5.1730 ns | 0.0079 ns | 0.0066 ns |   5.1708 ns |  1.00 |    0.00 |      - |         - |          NA |
| Domain_Equals_Money           | .NET 9.0  | .NET 9.0  |   4.7735 ns | 0.0092 ns | 0.0086 ns |   4.7704 ns |  0.92 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| HashCode_Class                | .NET 10.0 | .NET 10.0 |  18.8684 ns | 0.0137 ns | 0.0115 ns |  18.8643 ns |  1.00 |    0.00 |      - |         - |          NA |
| HashCode_Class                | .NET 8.0  | .NET 8.0  |  18.8740 ns | 0.0073 ns | 0.0061 ns |  18.8737 ns |  1.00 |    0.00 |      - |         - |          NA |
| HashCode_Class                | .NET 9.0  | .NET 9.0  |  18.8384 ns | 0.0144 ns | 0.0135 ns |  18.8353 ns |  1.00 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| HashCode_RecordClass          | .NET 10.0 | .NET 10.0 |  22.3561 ns | 0.1216 ns | 0.1078 ns |  22.3464 ns |  1.00 |    0.00 |      - |         - |          NA |
| HashCode_RecordClass          | .NET 8.0  | .NET 8.0  |  22.4602 ns | 0.0169 ns | 0.0150 ns |  22.4618 ns |  1.00 |    0.00 |      - |         - |          NA |
| HashCode_RecordClass          | .NET 9.0  | .NET 9.0  |  22.9378 ns | 0.0105 ns | 0.0093 ns |  22.9350 ns |  1.02 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| HashCode_Struct               | .NET 10.0 | .NET 10.0 |  18.5483 ns | 0.0197 ns | 0.0174 ns |  18.5395 ns |  1.01 |    0.00 |      - |         - |          NA |
| HashCode_Struct               | .NET 8.0  | .NET 8.0  |  18.3848 ns | 0.0082 ns | 0.0068 ns |  18.3835 ns |  1.00 |    0.00 |      - |         - |          NA |
| HashCode_Struct               | .NET 9.0  | .NET 9.0  |  18.0203 ns | 0.0174 ns | 0.0155 ns |  18.0192 ns |  0.98 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| HashCode_RecordStruct         | .NET 10.0 | .NET 10.0 |  17.7518 ns | 0.0236 ns | 0.0221 ns |  17.7409 ns |  0.94 |    0.00 |      - |         - |          NA |
| HashCode_RecordStruct         | .NET 8.0  | .NET 8.0  |  18.8676 ns | 0.0202 ns | 0.0189 ns |  18.8592 ns |  1.00 |    0.00 |      - |         - |          NA |
| HashCode_RecordStruct         | .NET 9.0  | .NET 9.0  |  18.6500 ns | 0.0059 ns | 0.0049 ns |  18.6518 ns |  0.99 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| DictionaryLookup_RecordClass  | .NET 10.0 | .NET 10.0 |  26.0880 ns | 0.1366 ns | 0.1140 ns |  26.0941 ns |  0.73 |    0.00 |      - |         - |          NA |
| DictionaryLookup_RecordClass  | .NET 8.0  | .NET 8.0  |  35.5280 ns | 0.0940 ns | 0.0785 ns |  35.4998 ns |  1.00 |    0.00 |      - |         - |          NA |
| DictionaryLookup_RecordClass  | .NET 9.0  | .NET 9.0  |  33.7959 ns | 0.0443 ns | 0.0346 ns |  33.7910 ns |  0.95 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| DictionaryLookup_RecordStruct | .NET 10.0 | .NET 10.0 |  24.0302 ns | 0.0171 ns | 0.0133 ns |  24.0286 ns |  0.98 |    0.00 |      - |         - |          NA |
| DictionaryLookup_RecordStruct | .NET 8.0  | .NET 8.0  |  24.6081 ns | 0.0182 ns | 0.0152 ns |  24.6065 ns |  1.00 |    0.00 |      - |         - |          NA |
| DictionaryLookup_RecordStruct | .NET 9.0  | .NET 9.0  |  24.4413 ns | 0.0092 ns | 0.0081 ns |  24.4404 ns |  0.99 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Money_Add              | .NET 10.0 | .NET 10.0 |  14.5689 ns | 0.0066 ns | 0.0058 ns |  14.5682 ns |  0.77 |    0.00 |      - |         - |          NA |
| Domain_Money_Add              | .NET 8.0  | .NET 8.0  |  18.9168 ns | 0.0205 ns | 0.0182 ns |  18.9141 ns |  1.00 |    0.00 |      - |         - |          NA |
| Domain_Money_Add              | .NET 9.0  | .NET 9.0  |  18.2289 ns | 0.0079 ns | 0.0066 ns |  18.2277 ns |  0.96 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Money_Allocate         | .NET 10.0 | .NET 10.0 | 381.5842 ns | 0.6141 ns | 0.5744 ns | 381.7145 ns |  1.06 |    0.00 | 0.0057 |      96 B |        1.00 |
| Domain_Money_Allocate         | .NET 8.0  | .NET 8.0  | 359.1303 ns | 0.4178 ns | 0.3703 ns | 359.0889 ns |  1.00 |    0.00 | 0.0057 |      96 B |        1.00 |
| Domain_Money_Allocate         | .NET 9.0  | .NET 9.0  | 360.3484 ns | 0.6948 ns | 0.6160 ns | 360.1881 ns |  1.00 |    0.00 | 0.0057 |      96 B |        1.00 |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Money_ApplyTax         | .NET 10.0 | .NET 10.0 |  74.9407 ns | 0.0270 ns | 0.0239 ns |  74.9498 ns |  0.98 |    0.00 |      - |         - |          NA |
| Domain_Money_ApplyTax         | .NET 8.0  | .NET 8.0  |  76.2968 ns | 0.0857 ns | 0.0716 ns |  76.2763 ns |  1.00 |    0.00 |      - |         - |          NA |
| Domain_Money_ApplyTax         | .NET 9.0  | .NET 9.0  |  71.7916 ns | 0.0659 ns | 0.0550 ns |  71.7972 ns |  0.94 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Parse_BusinessDate     | .NET 10.0 | .NET 10.0 | 129.7442 ns | 0.0646 ns | 0.0539 ns | 129.7307 ns |  0.94 |    0.00 |      - |         - |          NA |
| Domain_Parse_BusinessDate     | .NET 8.0  | .NET 8.0  | 137.9379 ns | 0.1460 ns | 0.1366 ns | 137.9027 ns |  1.00 |    0.00 |      - |         - |          NA |
| Domain_Parse_BusinessDate     | .NET 9.0  | .NET 9.0  | 132.5903 ns | 0.1427 ns | 0.1114 ns | 132.5894 ns |  0.96 |    0.00 |      - |         - |          NA |
