```

BenchmarkDotNet v0.15.8, Linux Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763 2.45GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]    : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v3
  .NET 10.0 : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v3
  .NET 8.0  : .NET 8.0.31 (8.0.31, 8.0.3126.42015), X64 RyuJIT x86-64-v3
  .NET 9.0  : .NET 9.0.20 (9.0.20, 9.0.2026.41315), X64 RyuJIT x86-64-v3


```
| Method                        | Job       | Runtime   | Mean        | Error     | StdDev    | Median      | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------ |---------- |---------- |------------:|----------:|----------:|------------:|------:|--------:|-------:|----------:|------------:|
| Create_Class                  | .NET 10.0 | .NET 10.0 |   6.9171 ns | 0.1290 ns | 0.1206 ns |   6.9112 ns |  0.86 |    0.13 | 0.0014 |      24 B |        1.00 |
| Create_Class                  | .NET 8.0  | .NET 8.0  |   8.1806 ns | 0.4089 ns | 1.1992 ns |   8.1378 ns |  1.02 |    0.21 | 0.0014 |      24 B |        1.00 |
| Create_Class                  | .NET 9.0  | .NET 9.0  |   7.7218 ns | 0.1137 ns | 0.1063 ns |   7.6879 ns |  0.96 |    0.14 | 0.0014 |      24 B |        1.00 |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Create_RecordClass            | .NET 10.0 | .NET 10.0 |   6.6697 ns | 0.1456 ns | 0.1362 ns |   6.6539 ns |  0.41 |    0.03 | 0.0014 |      24 B |        1.00 |
| Create_RecordClass            | .NET 8.0  | .NET 8.0  |  16.2708 ns | 0.3965 ns | 1.0788 ns |  16.3517 ns |  1.00 |    0.10 | 0.0014 |      24 B |        1.00 |
| Create_RecordClass            | .NET 9.0  | .NET 9.0  |   7.5617 ns | 0.0509 ns | 0.0476 ns |   7.5737 ns |  0.47 |    0.03 | 0.0014 |      24 B |        1.00 |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Create_Struct                 | .NET 10.0 | .NET 10.0 |   0.0000 ns | 0.0000 ns | 0.0000 ns |   0.0000 ns |     ? |       ? |      - |         - |           ? |
| Create_Struct                 | .NET 8.0  | .NET 8.0  |   0.0007 ns | 0.0013 ns | 0.0011 ns |   0.0000 ns |     ? |       ? |      - |         - |           ? |
| Create_Struct                 | .NET 9.0  | .NET 9.0  |   0.0007 ns | 0.0012 ns | 0.0012 ns |   0.0000 ns |     ? |       ? |      - |         - |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Create_RecordStruct           | .NET 10.0 | .NET 10.0 |   0.0004 ns | 0.0009 ns | 0.0008 ns |   0.0000 ns |     ? |       ? |      - |         - |           ? |
| Create_RecordStruct           | .NET 8.0  | .NET 8.0  |   0.0000 ns | 0.0000 ns | 0.0000 ns |   0.0000 ns |     ? |       ? |      - |         - |           ? |
| Create_RecordStruct           | .NET 9.0  | .NET 9.0  |   0.0008 ns | 0.0015 ns | 0.0013 ns |   0.0000 ns |     ? |       ? |      - |         - |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Create_Email           | .NET 10.0 | .NET 10.0 | 205.6834 ns | 2.4565 ns | 2.2978 ns | 205.1317 ns |  0.80 |    0.01 | 0.0186 |     312 B |        1.00 |
| Domain_Create_Email           | .NET 8.0  | .NET 8.0  | 255.6639 ns | 1.1550 ns | 1.0804 ns | 255.7228 ns |  1.00 |    0.01 | 0.0186 |     312 B |        1.00 |
| Domain_Create_Email           | .NET 9.0  | .NET 9.0  | 243.8351 ns | 2.5443 ns | 2.3799 ns | 244.6116 ns |  0.95 |    0.01 | 0.0186 |     312 B |        1.00 |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Create_Money           | .NET 10.0 | .NET 10.0 |  11.7409 ns | 0.0068 ns | 0.0057 ns |  11.7404 ns |  0.50 |    0.00 |      - |         - |          NA |
| Domain_Create_Money           | .NET 8.0  | .NET 8.0  |  23.6632 ns | 0.0235 ns | 0.0196 ns |  23.6661 ns |  1.00 |    0.00 |      - |         - |          NA |
| Domain_Create_Money           | .NET 9.0  | .NET 9.0  |  22.3428 ns | 0.0199 ns | 0.0166 ns |  22.3408 ns |  0.94 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Create_Percentage      | .NET 10.0 | .NET 10.0 |   8.0535 ns | 0.0096 ns | 0.0090 ns |   8.0536 ns |  0.39 |    0.00 |      - |         - |          NA |
| Domain_Create_Percentage      | .NET 8.0  | .NET 8.0  |  20.6135 ns | 0.0247 ns | 0.0219 ns |  20.6104 ns |  1.00 |    0.00 |      - |         - |          NA |
| Domain_Create_Percentage      | .NET 9.0  | .NET 9.0  |  20.4486 ns | 0.0205 ns | 0.0171 ns |  20.4419 ns |  0.99 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Equals_Class                  | .NET 10.0 | .NET 10.0 |   0.6062 ns | 0.0206 ns | 0.0183 ns |   0.5967 ns |  1.19 |    0.04 |      - |         - |          NA |
| Equals_Class                  | .NET 8.0  | .NET 8.0  |   0.5085 ns | 0.0090 ns | 0.0084 ns |   0.5090 ns |  1.00 |    0.02 |      - |         - |          NA |
| Equals_Class                  | .NET 9.0  | .NET 9.0  |   0.5787 ns | 0.0023 ns | 0.0019 ns |   0.5788 ns |  1.14 |    0.02 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Equals_RecordClass            | .NET 10.0 | .NET 10.0 |   0.5424 ns | 0.0058 ns | 0.0052 ns |   0.5433 ns |  0.82 |    0.01 |      - |         - |          NA |
| Equals_RecordClass            | .NET 8.0  | .NET 8.0  |   0.6590 ns | 0.0036 ns | 0.0033 ns |   0.6587 ns |  1.00 |    0.01 |      - |         - |          NA |
| Equals_RecordClass            | .NET 9.0  | .NET 9.0  |   0.6215 ns | 0.0050 ns | 0.0044 ns |   0.6211 ns |  0.94 |    0.01 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Equals_Struct                 | .NET 10.0 | .NET 10.0 |   0.4601 ns | 0.0030 ns | 0.0025 ns |   0.4600 ns |  1.01 |    0.01 |      - |         - |          NA |
| Equals_Struct                 | .NET 8.0  | .NET 8.0  |   0.4575 ns | 0.0020 ns | 0.0018 ns |   0.4571 ns |  1.00 |    0.01 |      - |         - |          NA |
| Equals_Struct                 | .NET 9.0  | .NET 9.0  |   0.3109 ns | 0.0068 ns | 0.0057 ns |   0.3106 ns |  0.68 |    0.01 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Equals_RecordStruct           | .NET 10.0 | .NET 10.0 |   0.3144 ns | 0.0022 ns | 0.0019 ns |   0.3143 ns |  1.40 |    0.02 |      - |         - |          NA |
| Equals_RecordStruct           | .NET 8.0  | .NET 8.0  |   0.2240 ns | 0.0038 ns | 0.0033 ns |   0.2235 ns |  1.00 |    0.02 |      - |         - |          NA |
| Equals_RecordStruct           | .NET 9.0  | .NET 9.0  |   0.3113 ns | 0.0042 ns | 0.0040 ns |   0.3119 ns |  1.39 |    0.03 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Equals_Money           | .NET 10.0 | .NET 10.0 |   5.1931 ns | 0.0061 ns | 0.0057 ns |   5.1912 ns |  1.06 |    0.00 |      - |         - |          NA |
| Domain_Equals_Money           | .NET 8.0  | .NET 8.0  |   4.8970 ns | 0.0086 ns | 0.0081 ns |   4.8967 ns |  1.00 |    0.00 |      - |         - |          NA |
| Domain_Equals_Money           | .NET 9.0  | .NET 9.0  |   4.7744 ns | 0.0054 ns | 0.0045 ns |   4.7729 ns |  0.97 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| HashCode_Class                | .NET 10.0 | .NET 10.0 |  18.8649 ns | 0.0046 ns | 0.0039 ns |  18.8650 ns |  1.00 |    0.00 |      - |         - |          NA |
| HashCode_Class                | .NET 8.0  | .NET 8.0  |  18.8784 ns | 0.0144 ns | 0.0127 ns |  18.8731 ns |  1.00 |    0.00 |      - |         - |          NA |
| HashCode_Class                | .NET 9.0  | .NET 9.0  |  18.8302 ns | 0.0157 ns | 0.0139 ns |  18.8250 ns |  1.00 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| HashCode_RecordClass          | .NET 10.0 | .NET 10.0 |  22.3737 ns | 0.1257 ns | 0.1115 ns |  22.3816 ns |  0.97 |    0.00 |      - |         - |          NA |
| HashCode_RecordClass          | .NET 8.0  | .NET 8.0  |  23.0627 ns | 0.0064 ns | 0.0057 ns |  23.0613 ns |  1.00 |    0.00 |      - |         - |          NA |
| HashCode_RecordClass          | .NET 9.0  | .NET 9.0  |  22.9409 ns | 0.0142 ns | 0.0119 ns |  22.9373 ns |  0.99 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| HashCode_Struct               | .NET 10.0 | .NET 10.0 |  18.5439 ns | 0.0149 ns | 0.0132 ns |  18.5387 ns |  1.01 |    0.00 |      - |         - |          NA |
| HashCode_Struct               | .NET 8.0  | .NET 8.0  |  18.3804 ns | 0.0065 ns | 0.0051 ns |  18.3787 ns |  1.00 |    0.00 |      - |         - |          NA |
| HashCode_Struct               | .NET 9.0  | .NET 9.0  |  18.1723 ns | 0.0090 ns | 0.0075 ns |  18.1736 ns |  0.99 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| HashCode_RecordStruct         | .NET 10.0 | .NET 10.0 |  17.7406 ns | 0.0114 ns | 0.0089 ns |  17.7376 ns |  0.94 |    0.00 |      - |         - |          NA |
| HashCode_RecordStruct         | .NET 8.0  | .NET 8.0  |  18.8520 ns | 0.0146 ns | 0.0129 ns |  18.8477 ns |  1.00 |    0.00 |      - |         - |          NA |
| HashCode_RecordStruct         | .NET 9.0  | .NET 9.0  |  18.6567 ns | 0.0110 ns | 0.0092 ns |  18.6548 ns |  0.99 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| DictionaryLookup_RecordClass  | .NET 10.0 | .NET 10.0 |  26.1567 ns | 0.1167 ns | 0.1034 ns |  26.1347 ns |  0.74 |    0.00 |      - |         - |          NA |
| DictionaryLookup_RecordClass  | .NET 8.0  | .NET 8.0  |  35.2411 ns | 0.0400 ns | 0.0355 ns |  35.2392 ns |  1.00 |    0.00 |      - |         - |          NA |
| DictionaryLookup_RecordClass  | .NET 9.0  | .NET 9.0  |  33.8308 ns | 0.0163 ns | 0.0145 ns |  33.8362 ns |  0.96 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| DictionaryLookup_RecordStruct | .NET 10.0 | .NET 10.0 |  24.0369 ns | 0.0161 ns | 0.0134 ns |  24.0381 ns |  0.98 |    0.00 |      - |         - |          NA |
| DictionaryLookup_RecordStruct | .NET 8.0  | .NET 8.0  |  24.6244 ns | 0.0190 ns | 0.0168 ns |  24.6201 ns |  1.00 |    0.00 |      - |         - |          NA |
| DictionaryLookup_RecordStruct | .NET 9.0  | .NET 9.0  |  24.4433 ns | 0.0128 ns | 0.0100 ns |  24.4417 ns |  0.99 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Money_Add              | .NET 10.0 | .NET 10.0 |  14.3309 ns | 0.0168 ns | 0.0157 ns |  14.3280 ns |  0.76 |    0.00 |      - |         - |          NA |
| Domain_Money_Add              | .NET 8.0  | .NET 8.0  |  18.9152 ns | 0.0146 ns | 0.0129 ns |  18.9142 ns |  1.00 |    0.00 |      - |         - |          NA |
| Domain_Money_Add              | .NET 9.0  | .NET 9.0  |  18.0875 ns | 0.0119 ns | 0.0106 ns |  18.0873 ns |  0.96 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Money_Allocate         | .NET 10.0 | .NET 10.0 | 380.4181 ns | 0.4905 ns | 0.4348 ns | 380.2955 ns |  1.07 |    0.00 | 0.0057 |      96 B |        1.00 |
| Domain_Money_Allocate         | .NET 8.0  | .NET 8.0  | 356.3898 ns | 0.3575 ns | 0.3169 ns | 356.2788 ns |  1.00 |    0.00 | 0.0057 |      96 B |        1.00 |
| Domain_Money_Allocate         | .NET 9.0  | .NET 9.0  | 359.9062 ns | 0.1418 ns | 0.1184 ns | 359.9045 ns |  1.01 |    0.00 | 0.0057 |      96 B |        1.00 |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Money_ApplyTax         | .NET 10.0 | .NET 10.0 |  74.0735 ns | 0.0186 ns | 0.0145 ns |  74.0727 ns |  0.98 |    0.00 |      - |         - |          NA |
| Domain_Money_ApplyTax         | .NET 8.0  | .NET 8.0  |  75.9618 ns | 0.0754 ns | 0.0668 ns |  75.9477 ns |  1.00 |    0.00 |      - |         - |          NA |
| Domain_Money_ApplyTax         | .NET 9.0  | .NET 9.0  |  71.2810 ns | 0.0822 ns | 0.0729 ns |  71.2673 ns |  0.94 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Parse_BusinessDate     | .NET 10.0 | .NET 10.0 | 130.5549 ns | 0.5751 ns | 0.4803 ns | 130.7339 ns |  0.96 |    0.00 |      - |         - |          NA |
| Domain_Parse_BusinessDate     | .NET 8.0  | .NET 8.0  | 135.6436 ns | 0.1360 ns | 0.1135 ns | 135.6505 ns |  1.00 |    0.00 |      - |         - |          NA |
| Domain_Parse_BusinessDate     | .NET 9.0  | .NET 9.0  | 129.9785 ns | 0.0980 ns | 0.0818 ns | 130.0077 ns |  0.96 |    0.00 |      - |         - |          NA |
