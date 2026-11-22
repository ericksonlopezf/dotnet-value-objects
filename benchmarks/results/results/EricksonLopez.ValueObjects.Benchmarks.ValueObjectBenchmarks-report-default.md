
BenchmarkDotNet v0.15.8, Linux Ubuntu 26.04.1 LTS (Resolute Raccoon)
AMD EPYC 9V74 3.66GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 11.0.100
  [Host]    : .NET 10.0.15 (10.0.15, 10.0.1526.53008), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.15 (10.0.15, 10.0.1526.53008), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.34 (8.0.34, 8.0.3426.52217), X64 RyuJIT x86-64-v4
  .NET 9.0  : .NET 9.0.23 (9.0.23, 9.0.2326.52216), X64 RyuJIT x86-64-v4


 Method                        | Job       | Runtime   | Mean        | Error     | StdDev    | Median      | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
------------------------------ |---------- |---------- |------------:|----------:|----------:|------------:|------:|--------:|-------:|----------:|------------:|
 Create_Class                  | .NET 10.0 | .NET 10.0 |   4.9934 ns | 0.1534 ns | 0.1197 ns |   5.0304 ns |  0.92 |    0.03 | 0.0014 |      24 B |        1.00 |
 Create_Class                  | .NET 8.0  | .NET 8.0  |   5.4461 ns | 0.1354 ns | 0.1267 ns |   5.4930 ns |  1.00 |    0.03 | 0.0014 |      24 B |        1.00 |
 Create_Class                  | .NET 9.0  | .NET 9.0  |   5.7159 ns | 0.1370 ns | 0.1281 ns |   5.6835 ns |  1.05 |    0.03 | 0.0014 |      24 B |        1.00 |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Create_RecordClass            | .NET 10.0 | .NET 10.0 |   5.2816 ns | 0.1573 ns | 0.2153 ns |   5.2746 ns |  0.84 |    0.03 | 0.0014 |      24 B |        1.00 |
 Create_RecordClass            | .NET 8.0  | .NET 8.0  |   6.3184 ns | 0.0611 ns | 0.0542 ns |   6.3112 ns |  1.00 |    0.01 | 0.0014 |      24 B |        1.00 |
 Create_RecordClass            | .NET 9.0  | .NET 9.0  |   5.7173 ns | 0.1466 ns | 0.1371 ns |   5.7804 ns |  0.90 |    0.02 | 0.0014 |      24 B |        1.00 |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Create_Struct                 | .NET 10.0 | .NET 10.0 |   0.0007 ns | 0.0010 ns | 0.0009 ns |   0.0002 ns |     ? |       ? |      - |         - |           ? |
 Create_Struct                 | .NET 8.0  | .NET 8.0  |   0.0006 ns | 0.0010 ns | 0.0009 ns |   0.0000 ns |     ? |       ? |      - |         - |           ? |
 Create_Struct                 | .NET 9.0  | .NET 9.0  |   0.0000 ns | 0.0000 ns | 0.0000 ns |   0.0000 ns |     ? |       ? |      - |         - |           ? |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Create_RecordStruct           | .NET 10.0 | .NET 10.0 |   0.0002 ns | 0.0005 ns | 0.0005 ns |   0.0000 ns |     ? |       ? |      - |         - |           ? |
 Create_RecordStruct           | .NET 8.0  | .NET 8.0  |   0.0002 ns | 0.0004 ns | 0.0003 ns |   0.0000 ns |     ? |       ? |      - |         - |           ? |
 Create_RecordStruct           | .NET 9.0  | .NET 9.0  |   0.0008 ns | 0.0012 ns | 0.0010 ns |   0.0006 ns |     ? |       ? |      - |         - |           ? |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Domain_Create_Email           | .NET 10.0 | .NET 10.0 | 135.2331 ns | 0.6388 ns | 0.5975 ns | 135.0210 ns |  0.75 |    0.00 | 0.0186 |     312 B |        1.00 |
 Domain_Create_Email           | .NET 8.0  | .NET 8.0  | 179.8124 ns | 0.8290 ns | 0.7349 ns | 179.9974 ns |  1.00 |    0.01 | 0.0186 |     312 B |        1.00 |
 Domain_Create_Email           | .NET 9.0  | .NET 9.0  | 171.5491 ns | 0.6359 ns | 0.5637 ns | 171.5510 ns |  0.95 |    0.00 | 0.0186 |     312 B |        1.00 |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Domain_Create_Money           | .NET 10.0 | .NET 10.0 |   9.5869 ns | 0.0107 ns | 0.0095 ns |   9.5855 ns |  0.62 |    0.00 |      - |         - |          NA |
 Domain_Create_Money           | .NET 8.0  | .NET 8.0  |  15.3805 ns | 0.0208 ns | 0.0174 ns |  15.3797 ns |  1.00 |    0.00 |      - |         - |          NA |
 Domain_Create_Money           | .NET 9.0  | .NET 9.0  |  17.5476 ns | 0.0276 ns | 0.0259 ns |  17.5406 ns |  1.14 |    0.00 |      - |         - |          NA |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Domain_Create_Percentage      | .NET 10.0 | .NET 10.0 |   6.2349 ns | 0.0042 ns | 0.0035 ns |   6.2351 ns |  0.42 |    0.00 |      - |         - |          NA |
 Domain_Create_Percentage      | .NET 8.0  | .NET 8.0  |  15.0224 ns | 0.0179 ns | 0.0140 ns |  15.0216 ns |  1.00 |    0.00 |      - |         - |          NA |
 Domain_Create_Percentage      | .NET 9.0  | .NET 9.0  |  15.2817 ns | 0.0203 ns | 0.0190 ns |  15.2724 ns |  1.02 |    0.00 |      - |         - |          NA |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Equals_Class                  | .NET 10.0 | .NET 10.0 |   0.1677 ns | 0.0037 ns | 0.0031 ns |   0.1674 ns |  0.65 |    0.01 |      - |         - |          NA |
 Equals_Class                  | .NET 8.0  | .NET 8.0  |   0.2579 ns | 0.0028 ns | 0.0025 ns |   0.2571 ns |  1.00 |    0.01 |      - |         - |          NA |
 Equals_Class                  | .NET 9.0  | .NET 9.0  |   0.3171 ns | 0.0022 ns | 0.0019 ns |   0.3174 ns |  1.23 |    0.01 |      - |         - |          NA |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Equals_RecordClass            | .NET 10.0 | .NET 10.0 |   0.1654 ns | 0.0041 ns | 0.0036 ns |   0.1651 ns |  0.32 |    0.01 |      - |         - |          NA |
 Equals_RecordClass            | .NET 8.0  | .NET 8.0  |   0.5111 ns | 0.0030 ns | 0.0028 ns |   0.5102 ns |  1.00 |    0.01 |      - |         - |          NA |
 Equals_RecordClass            | .NET 9.0  | .NET 9.0  |   0.1565 ns | 0.0025 ns | 0.0023 ns |   0.1560 ns |  0.31 |    0.00 |      - |         - |          NA |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Equals_Struct                 | .NET 10.0 | .NET 10.0 |   0.2898 ns | 0.0019 ns | 0.0016 ns |   0.2895 ns |     ? |       ? |      - |         - |           ? |
 Equals_Struct                 | .NET 8.0  | .NET 8.0  |   0.0000 ns | 0.0000 ns | 0.0000 ns |   0.0000 ns |     ? |       ? |      - |         - |           ? |
 Equals_Struct                 | .NET 9.0  | .NET 9.0  |   0.0016 ns | 0.0018 ns | 0.0015 ns |   0.0013 ns |     ? |       ? |      - |         - |           ? |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Equals_RecordStruct           | .NET 10.0 | .NET 10.0 |   0.0176 ns | 0.0020 ns | 0.0019 ns |   0.0172 ns |  0.07 |    0.01 |      - |         - |          NA |
 Equals_RecordStruct           | .NET 8.0  | .NET 8.0  |   0.2552 ns | 0.0013 ns | 0.0012 ns |   0.2550 ns |  1.00 |    0.01 |      - |         - |          NA |
 Equals_RecordStruct           | .NET 9.0  | .NET 9.0  |   0.2820 ns | 0.0084 ns | 0.0070 ns |   0.2795 ns |  1.10 |    0.03 |      - |         - |          NA |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Domain_Equals_Money           | .NET 10.0 | .NET 10.0 |   4.5231 ns | 0.0173 ns | 0.0162 ns |   4.5256 ns |  0.95 |    0.01 |      - |         - |          NA |
 Domain_Equals_Money           | .NET 8.0  | .NET 8.0  |   4.7646 ns | 0.0243 ns | 0.0228 ns |   4.7582 ns |  1.00 |    0.01 |      - |         - |          NA |
 Domain_Equals_Money           | .NET 9.0  | .NET 9.0  |   4.5047 ns | 0.0115 ns | 0.0090 ns |   4.5082 ns |  0.95 |    0.00 |      - |         - |          NA |
                               |           |           |             |           |           |             |       |         |        |           |             |
 HashCode_Class                | .NET 10.0 | .NET 10.0 |  16.8266 ns | 0.0212 ns | 0.0188 ns |  16.8247 ns |  1.00 |    0.00 |      - |         - |          NA |
 HashCode_Class                | .NET 8.0  | .NET 8.0  |  16.8366 ns | 0.0220 ns | 0.0206 ns |  16.8265 ns |  1.00 |    0.00 |      - |         - |          NA |
 HashCode_Class                | .NET 9.0  | .NET 9.0  |  16.8076 ns | 0.0166 ns | 0.0139 ns |  16.8056 ns |  1.00 |    0.00 |      - |         - |          NA |
                               |           |           |             |           |           |             |       |         |        |           |             |
 HashCode_RecordClass          | .NET 10.0 | .NET 10.0 |  18.1854 ns | 0.0312 ns | 0.0292 ns |  18.1786 ns |  1.01 |    0.00 |      - |         - |          NA |
 HashCode_RecordClass          | .NET 8.0  | .NET 8.0  |  17.9714 ns | 0.0272 ns | 0.0255 ns |  17.9725 ns |  1.00 |    0.00 |      - |         - |          NA |
 HashCode_RecordClass          | .NET 9.0  | .NET 9.0  |  17.9333 ns | 0.0355 ns | 0.0332 ns |  17.9187 ns |  1.00 |    0.00 |      - |         - |          NA |
                               |           |           |             |           |           |             |       |         |        |           |             |
 HashCode_Struct               | .NET 10.0 | .NET 10.0 |  16.2014 ns | 0.0308 ns | 0.0289 ns |  16.2009 ns |  0.99 |    0.00 |      - |         - |          NA |
 HashCode_Struct               | .NET 8.0  | .NET 8.0  |  16.3826 ns | 0.0233 ns | 0.0207 ns |  16.3900 ns |  1.00 |    0.00 |      - |         - |          NA |
 HashCode_Struct               | .NET 9.0  | .NET 9.0  |  16.1460 ns | 0.0266 ns | 0.0236 ns |  16.1455 ns |  0.99 |    0.00 |      - |         - |          NA |
                               |           |           |             |           |           |             |       |         |        |           |             |
 HashCode_RecordStruct         | .NET 10.0 | .NET 10.0 |  15.9720 ns | 0.0314 ns | 0.0293 ns |  15.9860 ns |  0.96 |    0.00 |      - |         - |          NA |
 HashCode_RecordStruct         | .NET 8.0  | .NET 8.0  |  16.6680 ns | 0.0161 ns | 0.0135 ns |  16.6647 ns |  1.00 |    0.00 |      - |         - |          NA |
 HashCode_RecordStruct         | .NET 9.0  | .NET 9.0  |  16.8173 ns | 0.0209 ns | 0.0196 ns |  16.8091 ns |  1.01 |    0.00 |      - |         - |          NA |
                               |           |           |             |           |           |             |       |         |        |           |             |
 DictionaryLookup_RecordClass  | .NET 10.0 | .NET 10.0 |  22.2569 ns | 0.0330 ns | 0.0309 ns |  22.2608 ns |  0.79 |    0.00 |      - |         - |          NA |
 DictionaryLookup_RecordClass  | .NET 8.0  | .NET 8.0  |  28.1928 ns | 0.0444 ns | 0.0393 ns |  28.1990 ns |  1.00 |    0.00 |      - |         - |          NA |
 DictionaryLookup_RecordClass  | .NET 9.0  | .NET 9.0  |  28.0871 ns | 0.0515 ns | 0.0482 ns |  28.1044 ns |  1.00 |    0.00 |      - |         - |          NA |
                               |           |           |             |           |           |             |       |         |        |           |             |
 DictionaryLookup_RecordStruct | .NET 10.0 | .NET 10.0 |  20.4024 ns | 0.0242 ns | 0.0226 ns |  20.4087 ns |  0.97 |    0.00 |      - |         - |          NA |
 DictionaryLookup_RecordStruct | .NET 8.0  | .NET 8.0  |  21.0063 ns | 0.0613 ns | 0.0543 ns |  20.9943 ns |  1.00 |    0.00 |      - |         - |          NA |
 DictionaryLookup_RecordStruct | .NET 9.0  | .NET 9.0  |  20.9555 ns | 0.0075 ns | 0.0059 ns |  20.9548 ns |  1.00 |    0.00 |      - |         - |          NA |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Domain_Money_Add              | .NET 10.0 | .NET 10.0 |  11.5809 ns | 0.0380 ns | 0.0355 ns |  11.5989 ns |  0.76 |    0.00 |      - |         - |          NA |
 Domain_Money_Add              | .NET 8.0  | .NET 8.0  |  15.3094 ns | 0.0210 ns | 0.0197 ns |  15.3077 ns |  1.00 |    0.00 |      - |         - |          NA |
 Domain_Money_Add              | .NET 9.0  | .NET 9.0  |  15.2285 ns | 0.0269 ns | 0.0252 ns |  15.2283 ns |  0.99 |    0.00 |      - |         - |          NA |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Domain_Money_Allocate         | .NET 10.0 | .NET 10.0 | 329.7843 ns | 0.4195 ns | 0.3924 ns | 329.8080 ns |  1.11 |    0.00 | 0.0057 |      96 B |        1.00 |
 Domain_Money_Allocate         | .NET 8.0  | .NET 8.0  | 297.7712 ns | 0.2443 ns | 0.2166 ns | 297.7447 ns |  1.00 |    0.00 | 0.0057 |      96 B |        1.00 |
 Domain_Money_Allocate         | .NET 9.0  | .NET 9.0  | 304.9122 ns | 0.5991 ns | 0.5604 ns | 304.9504 ns |  1.02 |    0.00 | 0.0057 |      96 B |        1.00 |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Domain_Money_ApplyTax         | .NET 10.0 | .NET 10.0 |  66.0044 ns | 0.0713 ns | 0.0596 ns |  66.0309 ns |  0.94 |    0.00 |      - |         - |          NA |
 Domain_Money_ApplyTax         | .NET 8.0  | .NET 8.0  |  69.8940 ns | 0.1341 ns | 0.1120 ns |  69.8449 ns |  1.00 |    0.00 |      - |         - |          NA |
 Domain_Money_ApplyTax         | .NET 9.0  | .NET 9.0  |  62.1599 ns | 0.0526 ns | 0.0492 ns |  62.1545 ns |  0.89 |    0.00 |      - |         - |          NA |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Domain_Parse_BusinessDate     | .NET 10.0 | .NET 10.0 |  92.3645 ns | 0.2079 ns | 0.1945 ns |  92.4283 ns |  0.91 |    0.00 |      - |         - |          NA |
 Domain_Parse_BusinessDate     | .NET 8.0  | .NET 8.0  | 101.4971 ns | 0.1514 ns | 0.1264 ns | 101.4586 ns |  1.00 |    0.00 |      - |         - |          NA |
 Domain_Parse_BusinessDate     | .NET 9.0  | .NET 9.0  |  98.7790 ns | 0.0650 ns | 0.0543 ns |  98.7830 ns |  0.97 |    0.00 |      - |         - |          NA |
