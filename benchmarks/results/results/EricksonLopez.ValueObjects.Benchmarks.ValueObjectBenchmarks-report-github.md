```

BenchmarkDotNet v0.15.8, Linux Ubuntu 24.04.5 LTS (Noble Numbat)
Intel Xeon 6973P-C 2.60GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]    : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v4
  .NET 8.0  : .NET 8.0.31 (8.0.31, 8.0.3126.42015), X64 RyuJIT x86-64-v4
  .NET 9.0  : .NET 9.0.20 (9.0.20, 9.0.2026.41315), X64 RyuJIT x86-64-v4


```
| Method                        | Job       | Runtime   | Mean        | Error     | StdDev    | Median      | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------ |---------- |---------- |------------:|----------:|----------:|------------:|------:|--------:|-------:|----------:|------------:|
| Create_Class                  | .NET 10.0 | .NET 10.0 |   5.5195 ns | 0.1545 ns | 0.2009 ns |   5.4308 ns |  0.75 |    0.03 | 0.0003 |      24 B |        1.00 |
| Create_Class                  | .NET 8.0  | .NET 8.0  |   7.3620 ns | 0.2007 ns | 0.2231 ns |   7.2947 ns |  1.00 |    0.04 | 0.0003 |      24 B |        1.00 |
| Create_Class                  | .NET 9.0  | .NET 9.0  |   7.6878 ns | 0.1900 ns | 0.1777 ns |   7.7030 ns |  1.05 |    0.04 | 0.0003 |      24 B |        1.00 |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Create_RecordClass            | .NET 10.0 | .NET 10.0 |   7.4852 ns | 0.1954 ns | 0.2540 ns |   7.3799 ns |  1.00 |    0.05 | 0.0003 |      24 B |        1.00 |
| Create_RecordClass            | .NET 8.0  | .NET 8.0  |   7.5275 ns | 0.2035 ns | 0.2646 ns |   7.5168 ns |  1.00 |    0.05 | 0.0003 |      24 B |        1.00 |
| Create_RecordClass            | .NET 9.0  | .NET 9.0  |   6.5374 ns | 0.1404 ns | 0.1245 ns |   6.5709 ns |  0.87 |    0.03 | 0.0003 |      24 B |        1.00 |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Create_Struct                 | .NET 10.0 | .NET 10.0 |   0.0000 ns | 0.0000 ns | 0.0000 ns |   0.0000 ns |     ? |       ? |      - |         - |           ? |
| Create_Struct                 | .NET 8.0  | .NET 8.0  |   0.0073 ns | 0.0167 ns | 0.0139 ns |   0.0000 ns |     ? |       ? |      - |         - |           ? |
| Create_Struct                 | .NET 9.0  | .NET 9.0  |   0.0076 ns | 0.0068 ns | 0.0057 ns |   0.0060 ns |     ? |       ? |      - |         - |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Create_RecordStruct           | .NET 10.0 | .NET 10.0 |   0.0005 ns | 0.0023 ns | 0.0019 ns |   0.0000 ns |     ? |       ? |      - |         - |           ? |
| Create_RecordStruct           | .NET 8.0  | .NET 8.0  |   0.0024 ns | 0.0075 ns | 0.0058 ns |   0.0000 ns |     ? |       ? |      - |         - |           ? |
| Create_RecordStruct           | .NET 9.0  | .NET 9.0  |   0.0105 ns | 0.0180 ns | 0.0160 ns |   0.0025 ns |     ? |       ? |      - |         - |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Create_Email           | .NET 10.0 | .NET 10.0 | 127.4330 ns | 0.6412 ns | 0.5355 ns | 127.4555 ns |  0.80 |    0.00 | 0.0036 |     312 B |        1.00 |
| Domain_Create_Email           | .NET 8.0  | .NET 8.0  | 159.9363 ns | 0.9884 ns | 0.7717 ns | 159.9239 ns |  1.00 |    0.01 | 0.0036 |     312 B |        1.00 |
| Domain_Create_Email           | .NET 9.0  | .NET 9.0  | 151.4957 ns | 0.9508 ns | 0.8428 ns | 151.2082 ns |  0.95 |    0.01 | 0.0036 |     312 B |        1.00 |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Create_Money           | .NET 10.0 | .NET 10.0 |   7.1469 ns | 0.1396 ns | 0.1863 ns |   7.0874 ns |  0.57 |    0.02 |      - |         - |          NA |
| Domain_Create_Money           | .NET 8.0  | .NET 8.0  |  12.5890 ns | 0.2759 ns | 0.4296 ns |  12.3693 ns |  1.00 |    0.05 |      - |         - |          NA |
| Domain_Create_Money           | .NET 9.0  | .NET 9.0  |  11.8522 ns | 0.2250 ns | 0.3154 ns |  11.7152 ns |  0.94 |    0.04 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Create_Percentage      | .NET 10.0 | .NET 10.0 |   4.5144 ns | 0.1052 ns | 0.1253 ns |   4.4621 ns |  0.38 |    0.01 |      - |         - |          NA |
| Domain_Create_Percentage      | .NET 8.0  | .NET 8.0  |  11.8815 ns | 0.0984 ns | 0.0920 ns |  11.8581 ns |  1.00 |    0.01 |      - |         - |          NA |
| Domain_Create_Percentage      | .NET 9.0  | .NET 9.0  |  11.2024 ns | 0.0091 ns | 0.0071 ns |  11.2028 ns |  0.94 |    0.01 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Equals_Class                  | .NET 10.0 | .NET 10.0 |   0.1653 ns | 0.0116 ns | 0.0091 ns |   0.1666 ns |  0.90 |    0.06 |      - |         - |          NA |
| Equals_Class                  | .NET 8.0  | .NET 8.0  |   0.1835 ns | 0.0102 ns | 0.0085 ns |   0.1830 ns |  1.00 |    0.06 |      - |         - |          NA |
| Equals_Class                  | .NET 9.0  | .NET 9.0  |   0.4258 ns | 0.0374 ns | 0.0625 ns |   0.3867 ns |  2.33 |    0.35 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Equals_RecordClass            | .NET 10.0 | .NET 10.0 |   0.1465 ns | 0.0301 ns | 0.0296 ns |   0.1363 ns |  1.50 |    0.36 |      - |         - |          NA |
| Equals_RecordClass            | .NET 8.0  | .NET 8.0  |   0.0999 ns | 0.0203 ns | 0.0170 ns |   0.0965 ns |  1.02 |    0.22 |      - |         - |          NA |
| Equals_RecordClass            | .NET 9.0  | .NET 9.0  |   0.1746 ns | 0.0245 ns | 0.0273 ns |   0.1568 ns |  1.79 |    0.37 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Equals_Struct                 | .NET 10.0 | .NET 10.0 |   0.4177 ns | 0.0189 ns | 0.0167 ns |   0.4196 ns |  1.05 |    0.05 |      - |         - |          NA |
| Equals_Struct                 | .NET 8.0  | .NET 8.0  |   0.3968 ns | 0.0121 ns | 0.0113 ns |   0.3997 ns |  1.00 |    0.04 |      - |         - |          NA |
| Equals_Struct                 | .NET 9.0  | .NET 9.0  |   0.1418 ns | 0.0014 ns | 0.0011 ns |   0.1420 ns |  0.36 |    0.01 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Equals_RecordStruct           | .NET 10.0 | .NET 10.0 |   0.1724 ns | 0.0164 ns | 0.0128 ns |   0.1781 ns |     ? |       ? |      - |         - |           ? |
| Equals_RecordStruct           | .NET 8.0  | .NET 8.0  |   0.0293 ns | 0.0291 ns | 0.0299 ns |   0.0191 ns |     ? |       ? |      - |         - |           ? |
| Equals_RecordStruct           | .NET 9.0  | .NET 9.0  |   0.3919 ns | 0.0012 ns | 0.0010 ns |   0.3917 ns |     ? |       ? |      - |         - |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Equals_Money           | .NET 10.0 | .NET 10.0 |   2.5187 ns | 0.0329 ns | 0.0439 ns |   2.5150 ns |  0.95 |    0.04 |      - |         - |          NA |
| Domain_Equals_Money           | .NET 8.0  | .NET 8.0  |   2.6464 ns | 0.0804 ns | 0.1073 ns |   2.5970 ns |  1.00 |    0.06 |      - |         - |          NA |
| Domain_Equals_Money           | .NET 9.0  | .NET 9.0  |   2.4466 ns | 0.0226 ns | 0.0211 ns |   2.4411 ns |  0.93 |    0.04 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| HashCode_Class                | .NET 10.0 | .NET 10.0 |  14.2843 ns | 0.1167 ns | 0.1035 ns |  14.2942 ns |  0.99 |    0.01 |      - |         - |          NA |
| HashCode_Class                | .NET 8.0  | .NET 8.0  |  14.4301 ns | 0.0669 ns | 0.0558 ns |  14.4291 ns |  1.00 |    0.01 |      - |         - |          NA |
| HashCode_Class                | .NET 9.0  | .NET 9.0  |  14.0802 ns | 0.1413 ns | 0.1322 ns |  14.0031 ns |  0.98 |    0.01 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| HashCode_RecordClass          | .NET 10.0 | .NET 10.0 |  14.9058 ns | 0.1213 ns | 0.1013 ns |  14.9083 ns |  0.98 |    0.01 |      - |         - |          NA |
| HashCode_RecordClass          | .NET 8.0  | .NET 8.0  |  15.1509 ns | 0.0687 ns | 0.0643 ns |  15.1520 ns |  1.00 |    0.01 |      - |         - |          NA |
| HashCode_RecordClass          | .NET 9.0  | .NET 9.0  |  14.9656 ns | 0.2126 ns | 0.2184 ns |  14.8485 ns |  0.99 |    0.01 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| HashCode_Struct               | .NET 10.0 | .NET 10.0 |  13.9221 ns | 0.2258 ns | 0.2319 ns |  13.8498 ns |  1.02 |    0.02 |      - |         - |          NA |
| HashCode_Struct               | .NET 8.0  | .NET 8.0  |  13.6383 ns | 0.0965 ns | 0.0806 ns |  13.6177 ns |  1.00 |    0.01 |      - |         - |          NA |
| HashCode_Struct               | .NET 9.0  | .NET 9.0  |  13.5209 ns | 0.0334 ns | 0.0261 ns |  13.5096 ns |  0.99 |    0.01 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| HashCode_RecordStruct         | .NET 10.0 | .NET 10.0 |  14.3212 ns | 0.3119 ns | 0.6083 ns |  13.9827 ns |  1.06 |    0.05 |      - |         - |          NA |
| HashCode_RecordStruct         | .NET 8.0  | .NET 8.0  |  13.5293 ns | 0.1911 ns | 0.1788 ns |  13.4335 ns |  1.00 |    0.02 |      - |         - |          NA |
| HashCode_RecordStruct         | .NET 9.0  | .NET 9.0  |  13.7653 ns | 0.2982 ns | 0.4643 ns |  13.5263 ns |  1.02 |    0.04 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| DictionaryLookup_RecordClass  | .NET 10.0 | .NET 10.0 |  18.1028 ns | 0.0983 ns | 0.0767 ns |  18.0689 ns |  0.88 |    0.01 |      - |         - |          NA |
| DictionaryLookup_RecordClass  | .NET 8.0  | .NET 8.0  |  20.5342 ns | 0.3865 ns | 0.3228 ns |  20.4880 ns |  1.00 |    0.02 |      - |         - |          NA |
| DictionaryLookup_RecordClass  | .NET 9.0  | .NET 9.0  |  20.4272 ns | 0.3555 ns | 0.3325 ns |  20.3212 ns |  1.00 |    0.02 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| DictionaryLookup_RecordStruct | .NET 10.0 | .NET 10.0 |  16.8023 ns | 0.1003 ns | 0.0938 ns |  16.8251 ns |  0.97 |    0.01 |      - |         - |          NA |
| DictionaryLookup_RecordStruct | .NET 8.0  | .NET 8.0  |  17.3492 ns | 0.1265 ns | 0.0988 ns |  17.3435 ns |  1.00 |    0.01 |      - |         - |          NA |
| DictionaryLookup_RecordStruct | .NET 9.0  | .NET 9.0  |  16.9714 ns | 0.3689 ns | 0.3947 ns |  16.8013 ns |  0.98 |    0.02 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Money_Add              | .NET 10.0 | .NET 10.0 |   8.3082 ns | 0.1945 ns | 0.3406 ns |   8.2748 ns |  0.75 |    0.03 |      - |         - |          NA |
| Domain_Money_Add              | .NET 8.0  | .NET 8.0  |  11.1284 ns | 0.0835 ns | 0.0697 ns |  11.1152 ns |  1.00 |    0.01 |      - |         - |          NA |
| Domain_Money_Add              | .NET 9.0  | .NET 9.0  |   9.7131 ns | 0.1354 ns | 0.1057 ns |   9.6864 ns |  0.87 |    0.01 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Money_Allocate         | .NET 10.0 | .NET 10.0 | 298.1662 ns | 1.5728 ns | 1.4712 ns | 298.5613 ns |  1.10 |    0.01 | 0.0010 |      96 B |        1.00 |
| Domain_Money_Allocate         | .NET 8.0  | .NET 8.0  | 271.7798 ns | 2.5143 ns | 2.0996 ns | 272.5014 ns |  1.00 |    0.01 | 0.0010 |      96 B |        1.00 |
| Domain_Money_Allocate         | .NET 9.0  | .NET 9.0  | 261.7694 ns | 1.2111 ns | 1.4874 ns | 261.2046 ns |  0.96 |    0.01 | 0.0010 |      96 B |        1.00 |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Money_ApplyTax         | .NET 10.0 | .NET 10.0 |  50.5767 ns | 0.5457 ns | 0.5105 ns |  50.5952 ns |  0.93 |    0.01 |      - |         - |          NA |
| Domain_Money_ApplyTax         | .NET 8.0  | .NET 8.0  |  54.1868 ns | 0.2461 ns | 0.2302 ns |  54.1696 ns |  1.00 |    0.01 |      - |         - |          NA |
| Domain_Money_ApplyTax         | .NET 9.0  | .NET 9.0  |  51.0524 ns | 0.2732 ns | 0.2133 ns |  50.9626 ns |  0.94 |    0.01 |      - |         - |          NA |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Parse_BusinessDate     | .NET 10.0 | .NET 10.0 |  69.8970 ns | 0.6377 ns | 0.4979 ns |  69.8174 ns |  0.91 |    0.03 |      - |         - |          NA |
| Domain_Parse_BusinessDate     | .NET 8.0  | .NET 8.0  |  77.0767 ns | 1.5672 ns | 2.6613 ns |  75.5417 ns |  1.00 |    0.05 |      - |         - |          NA |
| Domain_Parse_BusinessDate     | .NET 9.0  | .NET 9.0  |  73.3597 ns | 0.1820 ns | 0.1421 ns |  73.3729 ns |  0.95 |    0.03 |      - |         - |          NA |
