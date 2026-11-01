```

BenchmarkDotNet v0.15.8, Linux Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V74 2.75GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.403
  [Host]    : .NET 10.0.14 (10.0.14, 10.0.1426.52326), X64 RyuJIT x86-64-v3
  .NET 10.0 : .NET 10.0.14 (10.0.14, 10.0.1426.52326), X64 RyuJIT x86-64-v3
  .NET 8.0  : .NET 8.0.33 (8.0.33, 8.0.3326.52219), X64 RyuJIT x86-64-v3
  .NET 9.0  : .NET 9.0.22 (9.0.22, 9.0.2226.52218), X64 RyuJIT x86-64-v3


```
| Method                        | Job       | Runtime   | Mean        | Error     | StdDev    | Median      | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------ |---------- |---------- |------------:|----------:|----------:|------------:|------:|--------:|-------:|----------:|------------:|
| Create_Class                  | .NET 10.0 | .NET 10.0 |   6.8359 ns | 0.1470 ns | 0.1375 ns |   6.7803 ns |  1.04 |    0.03 | 0.0014 |      24 B |        1.00 |
| Create_Class                  | .NET 8.0  | .NET 8.0  |   6.5455 ns | 0.1768 ns | 0.1653 ns |   6.4737 ns |  1.00 |    0.03 | 0.0014 |      24 B |        1.00 |
| Create_Class                  | .NET 9.0  | .NET 9.0  |   7.2900 ns | 0.2195 ns | 0.2440 ns |   7.3827 ns |  1.11 |    0.05 | 0.0014 |      24 B |        1.00 |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Create_RecordClass            | .NET 10.0 | .NET 10.0 |   6.3260 ns | 0.0880 ns | 0.0823 ns |   6.3508 ns |  0.93 |    0.01 | 0.0014 |      24 B |        1.00 |
| Create_RecordClass            | .NET 8.0  | .NET 8.0  |   6.7666 ns | 0.0356 ns | 0.0333 ns |   6.7497 ns |  1.00 |    0.01 | 0.0014 |      24 B |        1.00 |
| Create_RecordClass            | .NET 9.0  | .NET 9.0  |   8.4391 ns | 0.0715 ns | 0.0669 ns |   8.4615 ns |  1.25 |    0.01 | 0.0014 |      24 B |        1.00 |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Create_Struct                 | .NET 10.0 | .NET 10.0 |   0.0005 ns | 0.0007 ns | 0.0006 ns |   0.0004 ns |     ? |       ? |      - |         - |           ? |
| Create_Struct                 | .NET 8.0  | .NET 8.0  |   0.0002 ns | 0.0003 ns | 0.0003 ns |   0.0000 ns |     ? |       ? |      - |         - |           ? |
| Create_Struct                 | .NET 9.0  | .NET 9.0  |   0.0000 ns | 0.0001 ns | 0.0001 ns |   0.0000 ns |     ? |       ? |      - |         - |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Create_RecordStruct           | .NET 10.0 | .NET 10.0 |   0.0001 ns | 0.0002 ns | 0.0001 ns |   0.0000 ns |     ? |       ? |      - |         - |           ? |
| Create_RecordStruct           | .NET 8.0  | .NET 8.0  |   0.0003 ns | 0.0004 ns | 0.0004 ns |   0.0000 ns |     ? |       ? |      - |         - |           ? |
| Create_RecordStruct           | .NET 9.0  | .NET 9.0  |   0.0002 ns | 0.0003 ns | 0.0003 ns |   0.0001 ns |     ? |       ? |      - |         - |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Create_Email           | .NET 10.0 | .NET 10.0 | 167.2893 ns | 0.4473 ns | 0.3735 ns | 167.4061 ns |  0.72 |    0.00 | 0.0186 |     312 B |        1.00 |
| Domain_Create_Email           | .NET 8.0  | .NET 8.0  | 233.1635 ns | 1.2341 ns | 1.1544 ns | 232.9379 ns |  1.00 |    0.01 | 0.0186 |     312 B |        1.00 |
| Domain_Create_Email           | .NET 9.0  | .NET 9.0  | 221.4387 ns | 0.7680 ns | 0.7184 ns | 221.5294 ns |  0.95 |    0.01 | 0.0186 |     312 B |        1.00 |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Create_Money           | .NET 10.0 | .NET 10.0 |  11.9433 ns | 0.0100 ns | 0.0089 ns |  11.9421 ns |  0.59 |    0.00 |      - |         - |          NA |
| Domain_Create_Money           | .NET 8.0  | .NET 8.0  |  20.0898 ns | 0.0184 ns | 0.0163 ns |  20.0855 ns |  1.00 |    0.00 |      - |         - |          NA |
| Domain_Create_Money           | .NET 9.0  | .NET 9.0  |  21.6572 ns | 0.0205 ns | 0.0181 ns |  21.6532 ns |  1.08 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Create_Percentage      | .NET 10.0 | .NET 10.0 |   8.0401 ns | 0.0088 ns | 0.0078 ns |   8.0389 ns |  0.41 |    0.00 |      - |         - |          NA |
| Domain_Create_Percentage      | .NET 8.0  | .NET 8.0  |  19.4236 ns | 0.0169 ns | 0.0141 ns |  19.4201 ns |  1.00 |    0.00 |      - |         - |          NA |
| Domain_Create_Percentage      | .NET 9.0  | .NET 9.0  |  19.2902 ns | 0.0080 ns | 0.0071 ns |  19.2911 ns |  0.99 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Equals_Class                  | .NET 10.0 | .NET 10.0 |   0.3761 ns | 0.0012 ns | 0.0009 ns |   0.3764 ns |  1.52 |    0.03 |      - |         - |          NA |
| Equals_Class                  | .NET 8.0  | .NET 8.0  |   0.2472 ns | 0.0046 ns | 0.0043 ns |   0.2478 ns |  1.00 |    0.02 |      - |         - |          NA |
| Equals_Class                  | .NET 9.0  | .NET 9.0  |   0.3537 ns | 0.0019 ns | 0.0016 ns |   0.3529 ns |  1.43 |    0.03 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Equals_RecordClass            | .NET 10.0 | .NET 10.0 |   0.2078 ns | 0.0023 ns | 0.0021 ns |   0.2083 ns |  0.32 |    0.00 |      - |         - |          NA |
| Equals_RecordClass            | .NET 8.0  | .NET 8.0  |   0.6402 ns | 0.0050 ns | 0.0047 ns |   0.6412 ns |  1.00 |    0.01 |      - |         - |          NA |
| Equals_RecordClass            | .NET 9.0  | .NET 9.0  |   0.3550 ns | 0.0020 ns | 0.0019 ns |   0.3545 ns |  0.55 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Equals_Struct                 | .NET 10.0 | .NET 10.0 |   0.3751 ns | 0.0007 ns | 0.0006 ns |   0.3750 ns | 0.996 |    0.00 |      - |         - |          NA |
| Equals_Struct                 | .NET 8.0  | .NET 8.0  |   0.3765 ns | 0.0019 ns | 0.0016 ns |   0.3767 ns | 1.000 |    0.01 |      - |         - |          NA |
| Equals_Struct                 | .NET 9.0  | .NET 9.0  |   0.0015 ns | 0.0012 ns | 0.0011 ns |   0.0015 ns | 0.004 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Equals_RecordStruct           | .NET 10.0 | .NET 10.0 |   0.3737 ns | 0.0007 ns | 0.0006 ns |   0.3738 ns | 16.09 |    0.38 |      - |         - |          NA |
| Equals_RecordStruct           | .NET 8.0  | .NET 8.0  |   0.0232 ns | 0.0006 ns | 0.0006 ns |   0.0232 ns |  1.00 |    0.03 |      - |         - |          NA |
| Equals_RecordStruct           | .NET 9.0  | .NET 9.0  |   0.0020 ns | 0.0016 ns | 0.0014 ns |   0.0019 ns |  0.09 |    0.06 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Equals_Money           | .NET 10.0 | .NET 10.0 |   6.0784 ns | 0.0229 ns | 0.0214 ns |   6.0771 ns |  1.00 |    0.00 |      - |         - |          NA |
| Domain_Equals_Money           | .NET 8.0  | .NET 8.0  |   6.1065 ns | 0.0179 ns | 0.0168 ns |   6.1075 ns |  1.00 |    0.00 |      - |         - |          NA |
| Domain_Equals_Money           | .NET 9.0  | .NET 9.0  |   5.5484 ns | 0.0136 ns | 0.0121 ns |   5.5484 ns |  0.91 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| HashCode_Class                | .NET 10.0 | .NET 10.0 |  21.6866 ns | 0.0168 ns | 0.0157 ns |  21.6811 ns |  1.00 |    0.00 |      - |         - |          NA |
| HashCode_Class                | .NET 8.0  | .NET 8.0  |  21.6688 ns | 0.0058 ns | 0.0051 ns |  21.6690 ns |  1.00 |    0.00 |      - |         - |          NA |
| HashCode_Class                | .NET 9.0  | .NET 9.0  |  21.6685 ns | 0.0066 ns | 0.0059 ns |  21.6687 ns |  1.00 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| HashCode_RecordClass          | .NET 10.0 | .NET 10.0 |  23.4600 ns | 0.0141 ns | 0.0118 ns |  23.4556 ns |  1.01 |    0.00 |      - |         - |          NA |
| HashCode_RecordClass          | .NET 8.0  | .NET 8.0  |  23.1894 ns | 0.0169 ns | 0.0158 ns |  23.1872 ns |  1.00 |    0.00 |      - |         - |          NA |
| HashCode_RecordClass          | .NET 9.0  | .NET 9.0  |  23.0297 ns | 0.0048 ns | 0.0042 ns |  23.0287 ns |  0.99 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| HashCode_Struct               | .NET 10.0 | .NET 10.0 |  20.9495 ns | 0.0129 ns | 0.0114 ns |  20.9453 ns |  0.99 |    0.00 |      - |         - |          NA |
| HashCode_Struct               | .NET 8.0  | .NET 8.0  |  21.0970 ns | 0.0082 ns | 0.0077 ns |  21.0969 ns |  1.00 |    0.00 |      - |         - |          NA |
| HashCode_Struct               | .NET 9.0  | .NET 9.0  |  20.7932 ns | 0.0088 ns | 0.0068 ns |  20.7922 ns |  0.99 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| HashCode_RecordStruct         | .NET 10.0 | .NET 10.0 |  20.5954 ns | 0.0194 ns | 0.0172 ns |  20.5960 ns |  0.96 |    0.00 |      - |         - |          NA |
| HashCode_RecordStruct         | .NET 8.0  | .NET 8.0  |  21.5221 ns | 0.0079 ns | 0.0070 ns |  21.5233 ns |  1.00 |    0.00 |      - |         - |          NA |
| HashCode_RecordStruct         | .NET 9.0  | .NET 9.0  |  21.6681 ns | 0.0049 ns | 0.0043 ns |  21.6688 ns |  1.01 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| DictionaryLookup_RecordClass  | .NET 10.0 | .NET 10.0 |  28.6973 ns | 0.0235 ns | 0.0220 ns |  28.7030 ns |  0.78 |    0.00 |      - |         - |          NA |
| DictionaryLookup_RecordClass  | .NET 8.0  | .NET 8.0  |  37.0210 ns | 0.0293 ns | 0.0260 ns |  37.0235 ns |  1.00 |    0.00 |      - |         - |          NA |
| DictionaryLookup_RecordClass  | .NET 9.0  | .NET 9.0  |  36.1656 ns | 0.0301 ns | 0.0267 ns |  36.1669 ns |  0.98 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| DictionaryLookup_RecordStruct | .NET 10.0 | .NET 10.0 |  25.5550 ns | 0.0115 ns | 0.0102 ns |  25.5510 ns |  0.95 |    0.00 |      - |         - |          NA |
| DictionaryLookup_RecordStruct | .NET 8.0  | .NET 8.0  |  27.0075 ns | 0.0175 ns | 0.0164 ns |  26.9991 ns |  1.00 |    0.00 |      - |         - |          NA |
| DictionaryLookup_RecordStruct | .NET 9.0  | .NET 9.0  |  26.9495 ns | 0.0144 ns | 0.0128 ns |  26.9452 ns |  1.00 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Money_Add              | .NET 10.0 | .NET 10.0 |  14.5261 ns | 0.0164 ns | 0.0146 ns |  14.5280 ns |  0.75 |    0.00 |      - |         - |          NA |
| Domain_Money_Add              | .NET 8.0  | .NET 8.0  |  19.3802 ns | 0.0791 ns | 0.0740 ns |  19.3797 ns |  1.00 |    0.01 |      - |         - |          NA |
| Domain_Money_Add              | .NET 9.0  | .NET 9.0  |  19.7318 ns | 0.0189 ns | 0.0167 ns |  19.7354 ns |  1.02 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Money_Allocate         | .NET 10.0 | .NET 10.0 | 417.7790 ns | 0.2422 ns | 0.2147 ns | 417.6929 ns |  1.09 |    0.00 | 0.0057 |      96 B |        1.00 |
| Domain_Money_Allocate         | .NET 8.0  | .NET 8.0  | 384.4366 ns | 0.2272 ns | 0.1898 ns | 384.4406 ns |  1.00 |    0.00 | 0.0057 |      96 B |        1.00 |
| Domain_Money_Allocate         | .NET 9.0  | .NET 9.0  | 386.6787 ns | 0.1621 ns | 0.1353 ns | 386.6807 ns |  1.01 |    0.00 | 0.0057 |      96 B |        1.00 |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Money_ApplyTax         | .NET 10.0 | .NET 10.0 |  86.5000 ns | 0.0484 ns | 0.0429 ns |  86.5041 ns |  0.96 |    0.00 |      - |         - |          NA |
| Domain_Money_ApplyTax         | .NET 8.0  | .NET 8.0  |  90.4048 ns | 0.0765 ns | 0.0716 ns |  90.3849 ns |  1.00 |    0.00 |      - |         - |          NA |
| Domain_Money_ApplyTax         | .NET 9.0  | .NET 9.0  |  80.6486 ns | 0.1031 ns | 0.0914 ns |  80.6029 ns |  0.89 |    0.00 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Parse_BusinessDate     | .NET 10.0 | .NET 10.0 | 117.3374 ns | 0.0890 ns | 0.0743 ns | 117.3435 ns |  0.90 |    0.00 |      - |         - |          NA |
| Domain_Parse_BusinessDate     | .NET 8.0  | .NET 8.0  | 130.6030 ns | 0.0682 ns | 0.0605 ns | 130.5984 ns |  1.00 |    0.00 |      - |         - |          NA |
| Domain_Parse_BusinessDate     | .NET 9.0  | .NET 9.0  | 127.2787 ns | 0.2557 ns | 0.2392 ns | 127.2289 ns |  0.97 |    0.00 |      - |         - |          NA |
