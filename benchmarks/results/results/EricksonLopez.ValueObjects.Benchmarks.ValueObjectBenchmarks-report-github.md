```

BenchmarkDotNet v0.15.8, Linux Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763 2.98GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]    : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v3
  .NET 10.0 : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v3
  .NET 8.0  : .NET 8.0.31 (8.0.31, 8.0.3126.42015), X64 RyuJIT x86-64-v3
  .NET 9.0  : .NET 9.0.20 (9.0.20, 9.0.2026.41315), X64 RyuJIT x86-64-v3


```
| Method                        | Job       | Runtime   | Mean        | Error     | StdDev    | Median      | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------ |---------- |---------- |------------:|----------:|----------:|------------:|------:|--------:|-------:|----------:|------------:|
| Create_Class                  | .NET 10.0 | .NET 10.0 |   6.1526 ns | 0.0313 ns | 0.0293 ns |   6.1474 ns |  0.48 |    0.07 | 0.0014 |      24 B |        1.00 |
| Create_Class                  | .NET 8.0  | .NET 8.0  |  13.0257 ns | 0.6050 ns | 1.7840 ns |  13.3057 ns |  1.02 |    0.20 | 0.0014 |      24 B |        1.00 |
| Create_Class                  | .NET 9.0  | .NET 9.0  |   7.4856 ns | 0.1907 ns | 0.1783 ns |   7.4777 ns |  0.59 |    0.08 | 0.0014 |      24 B |        1.00 |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Create_RecordClass            | .NET 10.0 | .NET 10.0 |   6.0395 ns | 0.0102 ns | 0.0090 ns |   6.0396 ns |  0.39 |    0.04 | 0.0014 |      24 B |        1.00 |
| Create_RecordClass            | .NET 8.0  | .NET 8.0  |  15.4689 ns | 0.4729 ns | 1.3944 ns |  15.4555 ns |  1.01 |    0.13 | 0.0014 |      24 B |        1.00 |
| Create_RecordClass            | .NET 9.0  | .NET 9.0  |   7.2418 ns | 0.0290 ns | 0.0257 ns |   7.2423 ns |  0.47 |    0.04 | 0.0014 |      24 B |        1.00 |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Create_Struct                 | .NET 10.0 | .NET 10.0 |   0.0017 ns | 0.0006 ns | 0.0005 ns |   0.0017 ns |     ? |       ? |      - |         - |           ? |
| Create_Struct                 | .NET 8.0  | .NET 8.0  |   0.0003 ns | 0.0005 ns | 0.0004 ns |   0.0002 ns |     ? |       ? |      - |         - |           ? |
| Create_Struct                 | .NET 9.0  | .NET 9.0  |   0.0009 ns | 0.0015 ns | 0.0013 ns |   0.0004 ns |     ? |       ? |      - |         - |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Create_RecordStruct           | .NET 10.0 | .NET 10.0 |   0.0005 ns | 0.0007 ns | 0.0007 ns |   0.0001 ns |     ? |       ? |      - |         - |           ? |
| Create_RecordStruct           | .NET 8.0  | .NET 8.0  |   0.0001 ns | 0.0003 ns | 0.0003 ns |   0.0000 ns |     ? |       ? |      - |         - |           ? |
| Create_RecordStruct           | .NET 9.0  | .NET 9.0  |   0.0005 ns | 0.0010 ns | 0.0008 ns |   0.0000 ns |     ? |       ? |      - |         - |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Create_Email           | .NET 10.0 | .NET 10.0 | 191.1595 ns | 0.3636 ns | 0.3223 ns | 191.2334 ns |  0.78 |    0.00 | 0.0186 |     312 B |        1.00 |
| Domain_Create_Email           | .NET 8.0  | .NET 8.0  | 244.8640 ns | 0.8803 ns | 0.7804 ns | 244.7773 ns |  1.00 |    0.00 | 0.0186 |     312 B |        1.00 |
| Domain_Create_Email           | .NET 9.0  | .NET 9.0  | 235.6764 ns | 0.6725 ns | 0.6290 ns | 235.7269 ns |  0.96 |    0.00 | 0.0186 |     312 B |        1.00 |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Create_Money           | .NET 10.0 | .NET 10.0 |  11.7398 ns | 0.0126 ns | 0.0105 ns |  11.7388 ns |  0.50 |    0.00 |      - |         - |          NA |
| Domain_Create_Money           | .NET 8.0  | .NET 8.0  |  23.6219 ns | 0.0080 ns | 0.0071 ns |  23.6222 ns |  1.00 |    0.00 |      - |         - |          NA |
| Domain_Create_Money           | .NET 9.0  | .NET 9.0  |  22.3127 ns | 0.0150 ns | 0.0133 ns |  22.3140 ns |  0.94 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Create_Percentage      | .NET 10.0 | .NET 10.0 |   8.0516 ns | 0.0047 ns | 0.0044 ns |   8.0503 ns |  0.39 |    0.00 |      - |         - |          NA |
| Domain_Create_Percentage      | .NET 8.0  | .NET 8.0  |  20.8399 ns | 0.0174 ns | 0.0154 ns |  20.8374 ns |  1.00 |    0.00 |      - |         - |          NA |
| Domain_Create_Percentage      | .NET 9.0  | .NET 9.0  |  20.3294 ns | 0.0126 ns | 0.0105 ns |  20.3250 ns |  0.98 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Equals_Class                  | .NET 10.0 | .NET 10.0 |   0.5795 ns | 0.0042 ns | 0.0039 ns |   0.5786 ns |  1.16 |    0.01 |      - |         - |          NA |
| Equals_Class                  | .NET 8.0  | .NET 8.0  |   0.4987 ns | 0.0062 ns | 0.0055 ns |   0.4988 ns |  1.00 |    0.02 |      - |         - |          NA |
| Equals_Class                  | .NET 9.0  | .NET 9.0  |   0.5848 ns | 0.0055 ns | 0.0051 ns |   0.5856 ns |  1.17 |    0.02 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Equals_RecordClass            | .NET 10.0 | .NET 10.0 |   0.5454 ns | 0.0046 ns | 0.0043 ns |   0.5463 ns |  0.79 |    0.01 |      - |         - |          NA |
| Equals_RecordClass            | .NET 8.0  | .NET 8.0  |   0.6903 ns | 0.0048 ns | 0.0045 ns |   0.6903 ns |  1.00 |    0.01 |      - |         - |          NA |
| Equals_RecordClass            | .NET 9.0  | .NET 9.0  |   0.7005 ns | 0.0084 ns | 0.0079 ns |   0.6995 ns |  1.01 |    0.01 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Equals_Struct                 | .NET 10.0 | .NET 10.0 |   0.4645 ns | 0.0015 ns | 0.0012 ns |   0.4649 ns |  0.98 |    0.01 |      - |         - |          NA |
| Equals_Struct                 | .NET 8.0  | .NET 8.0  |   0.4720 ns | 0.0031 ns | 0.0028 ns |   0.4717 ns |  1.00 |    0.01 |      - |         - |          NA |
| Equals_Struct                 | .NET 9.0  | .NET 9.0  |   0.3149 ns | 0.0031 ns | 0.0026 ns |   0.3151 ns |  0.67 |    0.01 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Equals_RecordStruct           | .NET 10.0 | .NET 10.0 |   0.3294 ns | 0.0036 ns | 0.0033 ns |   0.3299 ns |  1.26 |    0.02 |      - |         - |          NA |
| Equals_RecordStruct           | .NET 8.0  | .NET 8.0  |   0.2621 ns | 0.0044 ns | 0.0041 ns |   0.2620 ns |  1.00 |    0.02 |      - |         - |          NA |
| Equals_RecordStruct           | .NET 9.0  | .NET 9.0  |   0.3170 ns | 0.0045 ns | 0.0042 ns |   0.3182 ns |  1.21 |    0.02 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Equals_Money           | .NET 10.0 | .NET 10.0 |   5.0837 ns | 0.0028 ns | 0.0022 ns |   5.0842 ns |  0.98 |    0.00 |      - |         - |          NA |
| Domain_Equals_Money           | .NET 8.0  | .NET 8.0  |   5.1942 ns | 0.0022 ns | 0.0018 ns |   5.1945 ns |  1.00 |    0.00 |      - |         - |          NA |
| Domain_Equals_Money           | .NET 9.0  | .NET 9.0  |   4.8584 ns | 0.0045 ns | 0.0035 ns |   4.8582 ns |  0.94 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| HashCode_Class                | .NET 10.0 | .NET 10.0 |  18.8793 ns | 0.0144 ns | 0.0128 ns |  18.8806 ns |  1.00 |    0.00 |      - |         - |          NA |
| HashCode_Class                | .NET 8.0  | .NET 8.0  |  18.8695 ns | 0.0069 ns | 0.0058 ns |  18.8671 ns |  1.00 |    0.00 |      - |         - |          NA |
| HashCode_Class                | .NET 9.0  | .NET 9.0  |  18.8254 ns | 0.0119 ns | 0.0105 ns |  18.8231 ns |  1.00 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| HashCode_RecordClass          | .NET 10.0 | .NET 10.0 |  22.3422 ns | 0.0956 ns | 0.0894 ns |  22.3732 ns |  0.99 |    0.00 |      - |         - |          NA |
| HashCode_RecordClass          | .NET 8.0  | .NET 8.0  |  22.4699 ns | 0.0246 ns | 0.0218 ns |  22.4682 ns |  1.00 |    0.00 |      - |         - |          NA |
| HashCode_RecordClass          | .NET 9.0  | .NET 9.0  |  22.4070 ns | 0.0116 ns | 0.0097 ns |  22.4042 ns |  1.00 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| HashCode_Struct               | .NET 10.0 | .NET 10.0 |  18.5419 ns | 0.0135 ns | 0.0126 ns |  18.5362 ns |  1.02 |    0.00 |      - |         - |          NA |
| HashCode_Struct               | .NET 8.0  | .NET 8.0  |  18.2674 ns | 0.0099 ns | 0.0083 ns |  18.2672 ns |  1.00 |    0.00 |      - |         - |          NA |
| HashCode_Struct               | .NET 9.0  | .NET 9.0  |  18.1513 ns | 0.0126 ns | 0.0105 ns |  18.1503 ns |  0.99 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| HashCode_RecordStruct         | .NET 10.0 | .NET 10.0 |  17.7502 ns | 0.0233 ns | 0.0206 ns |  17.7410 ns |  0.94 |    0.00 |      - |         - |          NA |
| HashCode_RecordStruct         | .NET 8.0  | .NET 8.0  |  18.8462 ns | 0.0085 ns | 0.0071 ns |  18.8451 ns |  1.00 |    0.00 |      - |         - |          NA |
| HashCode_RecordStruct         | .NET 9.0  | .NET 9.0  |  18.6510 ns | 0.0064 ns | 0.0053 ns |  18.6511 ns |  0.99 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| DictionaryLookup_RecordClass  | .NET 10.0 | .NET 10.0 |  26.1183 ns | 0.1486 ns | 0.1390 ns |  26.0486 ns |  0.75 |    0.00 |      - |         - |          NA |
| DictionaryLookup_RecordClass  | .NET 8.0  | .NET 8.0  |  34.7493 ns | 0.0406 ns | 0.0360 ns |  34.7494 ns |  1.00 |    0.00 |      - |         - |          NA |
| DictionaryLookup_RecordClass  | .NET 9.0  | .NET 9.0  |  33.8736 ns | 0.0363 ns | 0.0322 ns |  33.8695 ns |  0.97 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| DictionaryLookup_RecordStruct | .NET 10.0 | .NET 10.0 |  23.9428 ns | 0.0063 ns | 0.0049 ns |  23.9428 ns |  0.97 |    0.00 |      - |         - |          NA |
| DictionaryLookup_RecordStruct | .NET 8.0  | .NET 8.0  |  24.6097 ns | 0.0171 ns | 0.0152 ns |  24.6058 ns |  1.00 |    0.00 |      - |         - |          NA |
| DictionaryLookup_RecordStruct | .NET 9.0  | .NET 9.0  |  24.4402 ns | 0.0202 ns | 0.0179 ns |  24.4318 ns |  0.99 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Money_Add              | .NET 10.0 | .NET 10.0 |  14.3182 ns | 0.0159 ns | 0.0141 ns |  14.3132 ns |  0.76 |    0.00 |      - |         - |          NA |
| Domain_Money_Add              | .NET 8.0  | .NET 8.0  |  18.7179 ns | 0.0205 ns | 0.0182 ns |  18.7155 ns |  1.00 |    0.00 |      - |         - |          NA |
| Domain_Money_Add              | .NET 9.0  | .NET 9.0  |  18.1184 ns | 0.0079 ns | 0.0066 ns |  18.1199 ns |  0.97 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Money_Allocate         | .NET 10.0 | .NET 10.0 | 382.3500 ns | 0.4694 ns | 0.4161 ns | 382.1793 ns |  1.08 |    0.00 | 0.0057 |      96 B |        1.00 |
| Domain_Money_Allocate         | .NET 8.0  | .NET 8.0  | 353.3489 ns | 0.5025 ns | 0.4454 ns | 353.1620 ns |  1.00 |    0.00 | 0.0057 |      96 B |        1.00 |
| Domain_Money_Allocate         | .NET 9.0  | .NET 9.0  | 357.0456 ns | 0.3188 ns | 0.2826 ns | 356.9359 ns |  1.01 |    0.00 | 0.0057 |      96 B |        1.00 |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Money_ApplyTax         | .NET 10.0 | .NET 10.0 |  74.4771 ns | 0.0838 ns | 0.0743 ns |  74.4407 ns |  0.97 |    0.00 |      - |         - |          NA |
| Domain_Money_ApplyTax         | .NET 8.0  | .NET 8.0  |  76.9456 ns | 0.1610 ns | 0.1344 ns |  76.8959 ns |  1.00 |    0.00 |      - |         - |          NA |
| Domain_Money_ApplyTax         | .NET 9.0  | .NET 9.0  |  71.1132 ns | 0.0448 ns | 0.0397 ns |  71.1159 ns |  0.92 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Parse_BusinessDate     | .NET 10.0 | .NET 10.0 | 127.8906 ns | 0.2234 ns | 0.1981 ns | 127.8546 ns |  0.96 |    0.00 |      - |         - |          NA |
| Domain_Parse_BusinessDate     | .NET 8.0  | .NET 8.0  | 133.5404 ns | 0.2389 ns | 0.2118 ns | 133.4732 ns |  1.00 |    0.00 |      - |         - |          NA |
| Domain_Parse_BusinessDate     | .NET 9.0  | .NET 9.0  | 131.7957 ns | 0.3570 ns | 0.3340 ns | 131.7305 ns |  0.99 |    0.00 |      - |         - |          NA |
