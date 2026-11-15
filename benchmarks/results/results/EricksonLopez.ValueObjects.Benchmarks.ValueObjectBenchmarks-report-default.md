
BenchmarkDotNet v0.15.8, Linux Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 9V74 3.69GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]    : .NET 10.0.15 (10.0.15, 10.0.1526.53008), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.15 (10.0.15, 10.0.1526.53008), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.34 (8.0.34, 8.0.3426.52217), X64 RyuJIT x86-64-v4
  .NET 9.0  : .NET 9.0.23 (9.0.23, 9.0.2326.52216), X64 RyuJIT x86-64-v4


 Method                        | Job       | Runtime   | Mean        | Error     | StdDev    | Median      | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
------------------------------ |---------- |---------- |------------:|----------:|----------:|------------:|------:|--------:|-------:|----------:|------------:|
 Create_Class                  | .NET 10.0 | .NET 10.0 |   5.0409 ns | 0.1515 ns | 0.2221 ns |   4.9268 ns |  0.98 |    0.06 | 0.0014 |      24 B |        1.00 |
 Create_Class                  | .NET 8.0  | .NET 8.0  |   5.1387 ns | 0.1592 ns | 0.1955 ns |   5.1458 ns |  1.00 |    0.05 | 0.0014 |      24 B |        1.00 |
 Create_Class                  | .NET 9.0  | .NET 9.0  |   5.1317 ns | 0.1020 ns | 0.0954 ns |   5.1643 ns |  1.00 |    0.04 | 0.0014 |      24 B |        1.00 |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Create_RecordClass            | .NET 10.0 | .NET 10.0 |   5.3195 ns | 0.1558 ns | 0.1457 ns |   5.2985 ns |  0.95 |    0.03 | 0.0014 |      24 B |        1.00 |
 Create_RecordClass            | .NET 8.0  | .NET 8.0  |   5.5788 ns | 0.1327 ns | 0.1242 ns |   5.5427 ns |  1.00 |    0.03 | 0.0014 |      24 B |        1.00 |
 Create_RecordClass            | .NET 9.0  | .NET 9.0  |   5.9119 ns | 0.1470 ns | 0.1375 ns |   5.8481 ns |  1.06 |    0.03 | 0.0014 |      24 B |        1.00 |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Create_Struct                 | .NET 10.0 | .NET 10.0 |   0.0003 ns | 0.0004 ns | 0.0004 ns |   0.0000 ns |     ? |       ? |      - |         - |           ? |
 Create_Struct                 | .NET 8.0  | .NET 8.0  |   0.0001 ns | 0.0002 ns | 0.0001 ns |   0.0000 ns |     ? |       ? |      - |         - |           ? |
 Create_Struct                 | .NET 9.0  | .NET 9.0  |   0.0014 ns | 0.0014 ns | 0.0013 ns |   0.0016 ns |     ? |       ? |      - |         - |           ? |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Create_RecordStruct           | .NET 10.0 | .NET 10.0 |   0.0010 ns | 0.0009 ns | 0.0009 ns |   0.0011 ns |     ? |       ? |      - |         - |           ? |
 Create_RecordStruct           | .NET 8.0  | .NET 8.0  |   0.0007 ns | 0.0006 ns | 0.0006 ns |   0.0008 ns |     ? |       ? |      - |         - |           ? |
 Create_RecordStruct           | .NET 9.0  | .NET 9.0  |   0.0007 ns | 0.0011 ns | 0.0009 ns |   0.0002 ns |     ? |       ? |      - |         - |           ? |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Domain_Create_Email           | .NET 10.0 | .NET 10.0 | 134.1540 ns | 0.4611 ns | 0.4314 ns | 134.3349 ns |  0.74 |    0.00 | 0.0186 |     312 B |        1.00 |
 Domain_Create_Email           | .NET 8.0  | .NET 8.0  | 180.8761 ns | 0.9040 ns | 0.8456 ns | 180.6053 ns |  1.00 |    0.01 | 0.0186 |     312 B |        1.00 |
 Domain_Create_Email           | .NET 9.0  | .NET 9.0  | 172.6629 ns | 0.6040 ns | 0.5649 ns | 172.6870 ns |  0.95 |    0.01 | 0.0186 |     312 B |        1.00 |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Domain_Create_Money           | .NET 10.0 | .NET 10.0 |   9.2775 ns | 0.0199 ns | 0.0187 ns |   9.2691 ns |  0.60 |    0.00 |      - |         - |          NA |
 Domain_Create_Money           | .NET 8.0  | .NET 8.0  |  15.4095 ns | 0.0212 ns | 0.0165 ns |  15.4084 ns |  1.00 |    0.00 |      - |         - |          NA |
 Domain_Create_Money           | .NET 9.0  | .NET 9.0  |  17.5272 ns | 0.0267 ns | 0.0250 ns |  17.5250 ns |  1.14 |    0.00 |      - |         - |          NA |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Domain_Create_Percentage      | .NET 10.0 | .NET 10.0 |   6.2559 ns | 0.0270 ns | 0.0252 ns |   6.2578 ns |  0.42 |    0.00 |      - |         - |          NA |
 Domain_Create_Percentage      | .NET 8.0  | .NET 8.0  |  15.0543 ns | 0.0384 ns | 0.0359 ns |  15.0507 ns |  1.00 |    0.00 |      - |         - |          NA |
 Domain_Create_Percentage      | .NET 9.0  | .NET 9.0  |  14.9973 ns | 0.0067 ns | 0.0056 ns |  14.9982 ns |  1.00 |    0.00 |      - |         - |          NA |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Equals_Class                  | .NET 10.0 | .NET 10.0 |   0.1664 ns | 0.0022 ns | 0.0021 ns |   0.1663 ns |  0.64 |    0.01 |      - |         - |          NA |
 Equals_Class                  | .NET 8.0  | .NET 8.0  |   0.2602 ns | 0.0040 ns | 0.0034 ns |   0.2590 ns |  1.00 |    0.02 |      - |         - |          NA |
 Equals_Class                  | .NET 9.0  | .NET 9.0  |   0.2784 ns | 0.0022 ns | 0.0020 ns |   0.2790 ns |  1.07 |    0.02 |      - |         - |          NA |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Equals_RecordClass            | .NET 10.0 | .NET 10.0 |   0.1600 ns | 0.0027 ns | 0.0022 ns |   0.1597 ns |  0.29 |    0.00 |      - |         - |          NA |
 Equals_RecordClass            | .NET 8.0  | .NET 8.0  |   0.5597 ns | 0.0024 ns | 0.0020 ns |   0.5594 ns |  1.00 |    0.00 |      - |         - |          NA |
 Equals_RecordClass            | .NET 9.0  | .NET 9.0  |   0.1555 ns | 0.0035 ns | 0.0031 ns |   0.1553 ns |  0.28 |    0.01 |      - |         - |          NA |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Equals_Struct                 | .NET 10.0 | .NET 10.0 |   0.2927 ns | 0.0035 ns | 0.0031 ns |   0.2910 ns |     ? |       ? |      - |         - |           ? |
 Equals_Struct                 | .NET 8.0  | .NET 8.0  |   0.0000 ns | 0.0000 ns | 0.0000 ns |   0.0000 ns |     ? |       ? |      - |         - |           ? |
 Equals_Struct                 | .NET 9.0  | .NET 9.0  |   0.0042 ns | 0.0026 ns | 0.0024 ns |   0.0055 ns |     ? |       ? |      - |         - |           ? |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Equals_RecordStruct           | .NET 10.0 | .NET 10.0 |   0.0198 ns | 0.0020 ns | 0.0019 ns |   0.0193 ns |  0.08 |    0.01 |      - |         - |          NA |
 Equals_RecordStruct           | .NET 8.0  | .NET 8.0  |   0.2573 ns | 0.0015 ns | 0.0013 ns |   0.2571 ns |  1.00 |    0.01 |      - |         - |          NA |
 Equals_RecordStruct           | .NET 9.0  | .NET 9.0  |   0.2734 ns | 0.0019 ns | 0.0017 ns |   0.2727 ns |  1.06 |    0.01 |      - |         - |          NA |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Domain_Equals_Money           | .NET 10.0 | .NET 10.0 |   4.7645 ns | 0.0200 ns | 0.0187 ns |   4.7641 ns |  1.00 |    0.01 |      - |         - |          NA |
 Domain_Equals_Money           | .NET 8.0  | .NET 8.0  |   4.7543 ns | 0.0180 ns | 0.0169 ns |   4.7524 ns |  1.00 |    0.00 |      - |         - |          NA |
 Domain_Equals_Money           | .NET 9.0  | .NET 9.0  |   4.5551 ns | 0.0119 ns | 0.0106 ns |   4.5543 ns |  0.96 |    0.00 |      - |         - |          NA |
                               |           |           |             |           |           |             |       |         |        |           |             |
 HashCode_Class                | .NET 10.0 | .NET 10.0 |  16.8215 ns | 0.0218 ns | 0.0194 ns |  16.8125 ns |  1.00 |    0.00 |      - |         - |          NA |
 HashCode_Class                | .NET 8.0  | .NET 8.0  |  16.8389 ns | 0.0306 ns | 0.0286 ns |  16.8247 ns |  1.00 |    0.00 |      - |         - |          NA |
 HashCode_Class                | .NET 9.0  | .NET 9.0  |  16.8280 ns | 0.0256 ns | 0.0239 ns |  16.8309 ns |  1.00 |    0.00 |      - |         - |          NA |
                               |           |           |             |           |           |             |       |         |        |           |             |
 HashCode_RecordClass          | .NET 10.0 | .NET 10.0 |  18.1918 ns | 0.0103 ns | 0.0096 ns |  18.1869 ns |  1.00 |    0.00 |      - |         - |          NA |
 HashCode_RecordClass          | .NET 8.0  | .NET 8.0  |  18.1672 ns | 0.0295 ns | 0.0276 ns |  18.1577 ns |  1.00 |    0.00 |      - |         - |          NA |
 HashCode_RecordClass          | .NET 9.0  | .NET 9.0  |  17.9297 ns | 0.0327 ns | 0.0306 ns |  17.9317 ns |  0.99 |    0.00 |      - |         - |          NA |
                               |           |           |             |           |           |             |       |         |        |           |             |
 HashCode_Struct               | .NET 10.0 | .NET 10.0 |  16.2153 ns | 0.0294 ns | 0.0260 ns |  16.2116 ns |  0.99 |    0.00 |      - |         - |          NA |
 HashCode_Struct               | .NET 8.0  | .NET 8.0  |  16.3617 ns | 0.0090 ns | 0.0070 ns |  16.3605 ns |  1.00 |    0.00 |      - |         - |          NA |
 HashCode_Struct               | .NET 9.0  | .NET 9.0  |  16.1369 ns | 0.0278 ns | 0.0232 ns |  16.1306 ns |  0.99 |    0.00 |      - |         - |          NA |
                               |           |           |             |           |           |             |       |         |        |           |             |
 HashCode_RecordStruct         | .NET 10.0 | .NET 10.0 |  15.9425 ns | 0.0130 ns | 0.0115 ns |  15.9451 ns |  0.96 |    0.00 |      - |         - |          NA |
 HashCode_RecordStruct         | .NET 8.0  | .NET 8.0  |  16.6683 ns | 0.0085 ns | 0.0075 ns |  16.6665 ns |  1.00 |    0.00 |      - |         - |          NA |
 HashCode_RecordStruct         | .NET 9.0  | .NET 9.0  |  16.8042 ns | 0.0057 ns | 0.0047 ns |  16.8035 ns |  1.01 |    0.00 |      - |         - |          NA |
                               |           |           |             |           |           |             |       |         |        |           |             |
 DictionaryLookup_RecordClass  | .NET 10.0 | .NET 10.0 |  22.2158 ns | 0.0161 ns | 0.0143 ns |  22.2116 ns |  0.78 |    0.00 |      - |         - |          NA |
 DictionaryLookup_RecordClass  | .NET 8.0  | .NET 8.0  |  28.5712 ns | 0.0886 ns | 0.0786 ns |  28.5421 ns |  1.00 |    0.00 |      - |         - |          NA |
 DictionaryLookup_RecordClass  | .NET 9.0  | .NET 9.0  |  28.0354 ns | 0.0332 ns | 0.0310 ns |  28.0454 ns |  0.98 |    0.00 |      - |         - |          NA |
                               |           |           |             |           |           |             |       |         |        |           |             |
 DictionaryLookup_RecordStruct | .NET 10.0 | .NET 10.0 |  19.8073 ns | 0.0321 ns | 0.0285 ns |  19.7947 ns |  0.94 |    0.00 |      - |         - |          NA |
 DictionaryLookup_RecordStruct | .NET 8.0  | .NET 8.0  |  20.9944 ns | 0.0301 ns | 0.0281 ns |  21.0040 ns |  1.00 |    0.00 |      - |         - |          NA |
 DictionaryLookup_RecordStruct | .NET 9.0  | .NET 9.0  |  20.9464 ns | 0.0081 ns | 0.0068 ns |  20.9443 ns |  1.00 |    0.00 |      - |         - |          NA |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Domain_Money_Add              | .NET 10.0 | .NET 10.0 |  11.8367 ns | 0.0233 ns | 0.0207 ns |  11.8435 ns |  0.76 |    0.00 |      - |         - |          NA |
 Domain_Money_Add              | .NET 8.0  | .NET 8.0  |  15.4762 ns | 0.0327 ns | 0.0306 ns |  15.4665 ns |  1.00 |    0.00 |      - |         - |          NA |
 Domain_Money_Add              | .NET 9.0  | .NET 9.0  |  15.5913 ns | 0.0328 ns | 0.0306 ns |  15.5923 ns |  1.01 |    0.00 |      - |         - |          NA |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Domain_Money_Allocate         | .NET 10.0 | .NET 10.0 | 332.4199 ns | 0.3424 ns | 0.3036 ns | 332.4461 ns |  1.11 |    0.00 | 0.0057 |      96 B |        1.00 |
 Domain_Money_Allocate         | .NET 8.0  | .NET 8.0  | 300.0379 ns | 0.3937 ns | 0.3490 ns | 300.0129 ns |  1.00 |    0.00 | 0.0057 |      96 B |        1.00 |
 Domain_Money_Allocate         | .NET 9.0  | .NET 9.0  | 301.2992 ns | 0.3898 ns | 0.3646 ns | 301.1228 ns |  1.00 |    0.00 | 0.0057 |      96 B |        1.00 |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Domain_Money_ApplyTax         | .NET 10.0 | .NET 10.0 |  66.5688 ns | 0.0529 ns | 0.0413 ns |  66.5627 ns |  0.97 |    0.00 |      - |         - |          NA |
 Domain_Money_ApplyTax         | .NET 8.0  | .NET 8.0  |  68.8549 ns | 0.1464 ns | 0.1369 ns |  68.9126 ns |  1.00 |    0.00 |      - |         - |          NA |
 Domain_Money_ApplyTax         | .NET 9.0  | .NET 9.0  |  62.2673 ns | 0.0707 ns | 0.0627 ns |  62.2581 ns |  0.90 |    0.00 |      - |         - |          NA |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Domain_Parse_BusinessDate     | .NET 10.0 | .NET 10.0 |  91.9891 ns | 0.1596 ns | 0.1493 ns |  91.9390 ns |  0.83 |    0.00 |      - |         - |          NA |
 Domain_Parse_BusinessDate     | .NET 8.0  | .NET 8.0  | 110.4876 ns | 0.1953 ns | 0.1827 ns | 110.4750 ns |  1.00 |    0.00 |      - |         - |          NA |
 Domain_Parse_BusinessDate     | .NET 9.0  | .NET 9.0  |  98.6114 ns | 0.1871 ns | 0.1751 ns |  98.6645 ns |  0.89 |    0.00 |      - |         - |          NA |
