```

BenchmarkDotNet v0.15.8, Linux Ubuntu 24.04.4 LTS (Noble Numbat)
INTEL XEON PLATINUM 8573C 3.35GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.400
  [Host]    : .NET 10.0.11 (10.0.11, 10.0.1126.37416), X64 RyuJIT x86-64-v4
  .NET 10.0 : .NET 10.0.11 (10.0.11, 10.0.1126.37416), X64 RyuJIT x86-64-v4


```
| Method                        | Job       | Runtime   | Mean        | Error     | StdDev    | Median      | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------ |---------- |---------- |------------:|----------:|----------:|------------:|------:|--------:|-------:|----------:|------------:|
| Create_Class                  | .NET 10.0 | .NET 10.0 |   5.7690 ns | 0.1779 ns | 0.1664 ns |   5.7741 ns |     ? |       ? | 0.0003 |      24 B |           ? |
| Create_Class                  | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| Create_Class                  | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Create_RecordClass            | .NET 10.0 | .NET 10.0 |   4.9926 ns | 0.0965 ns | 0.0806 ns |   4.9655 ns |     ? |       ? | 0.0003 |      24 B |           ? |
| Create_RecordClass            | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| Create_RecordClass            | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Create_Struct                 | .NET 10.0 | .NET 10.0 |   0.0002 ns | 0.0010 ns | 0.0009 ns |   0.0000 ns |     ? |       ? |      - |         - |           ? |
| Create_Struct                 | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| Create_Struct                 | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Create_RecordStruct           | .NET 10.0 | .NET 10.0 |   0.0003 ns | 0.0012 ns | 0.0010 ns |   0.0000 ns |     ? |       ? |      - |         - |           ? |
| Create_RecordStruct           | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| Create_RecordStruct           | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Create_Email           | .NET 10.0 | .NET 10.0 | 129.0971 ns | 1.0269 ns | 0.9605 ns | 129.3473 ns |     ? |       ? | 0.0017 |     152 B |           ? |
| Domain_Create_Email           | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| Domain_Create_Email           | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Create_Money           | .NET 10.0 | .NET 10.0 |   9.1559 ns | 0.1298 ns | 0.1151 ns |   9.1737 ns |     ? |       ? |      - |         - |           ? |
| Domain_Create_Money           | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| Domain_Create_Money           | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Create_Percentage      | .NET 10.0 | .NET 10.0 |   4.8673 ns | 0.0311 ns | 0.0275 ns |   4.8618 ns |     ? |       ? |      - |         - |           ? |
| Domain_Create_Percentage      | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| Domain_Create_Percentage      | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Equals_Class                  | .NET 10.0 | .NET 10.0 |   0.5515 ns | 0.0064 ns | 0.0057 ns |   0.5518 ns |     ? |       ? |      - |         - |           ? |
| Equals_Class                  | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| Equals_Class                  | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Equals_RecordClass            | .NET 10.0 | .NET 10.0 |   0.5840 ns | 0.0081 ns | 0.0072 ns |   0.5855 ns |     ? |       ? |      - |         - |           ? |
| Equals_RecordClass            | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| Equals_RecordClass            | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Equals_Struct                 | .NET 10.0 | .NET 10.0 |   0.2690 ns | 0.0067 ns | 0.0063 ns |   0.2711 ns |     ? |       ? |      - |         - |           ? |
| Equals_Struct                 | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| Equals_Struct                 | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Equals_RecordStruct           | .NET 10.0 | .NET 10.0 |   0.5882 ns | 0.0312 ns | 0.0292 ns |   0.5911 ns |     ? |       ? |      - |         - |           ? |
| Equals_RecordStruct           | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| Equals_RecordStruct           | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Equals_Money           | .NET 10.0 | .NET 10.0 |   2.0023 ns | 0.0159 ns | 0.0149 ns |   2.0006 ns |     ? |       ? |      - |         - |           ? |
| Domain_Equals_Money           | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| Domain_Equals_Money           | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| HashCode_Class                | .NET 10.0 | .NET 10.0 |  16.0014 ns | 0.0827 ns | 0.0773 ns |  16.0040 ns |     ? |       ? |      - |         - |           ? |
| HashCode_Class                | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| HashCode_Class                | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| HashCode_RecordClass          | .NET 10.0 | .NET 10.0 |  16.3980 ns | 0.0881 ns | 0.0781 ns |  16.3918 ns |     ? |       ? |      - |         - |           ? |
| HashCode_RecordClass          | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| HashCode_RecordClass          | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| HashCode_Struct               | .NET 10.0 | .NET 10.0 |  15.2149 ns | 0.0896 ns | 0.0839 ns |  15.2171 ns |     ? |       ? |      - |         - |           ? |
| HashCode_Struct               | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| HashCode_Struct               | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| HashCode_RecordStruct         | .NET 10.0 | .NET 10.0 |  15.2619 ns | 0.0516 ns | 0.0482 ns |  15.2543 ns |     ? |       ? |      - |         - |           ? |
| HashCode_RecordStruct         | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| HashCode_RecordStruct         | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| DictionaryLookup_RecordClass  | .NET 10.0 | .NET 10.0 |  19.9253 ns | 0.1139 ns | 0.0889 ns |  19.9095 ns |     ? |       ? |      - |         - |           ? |
| DictionaryLookup_RecordClass  | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| DictionaryLookup_RecordClass  | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| DictionaryLookup_RecordStruct | .NET 10.0 | .NET 10.0 |  18.5583 ns | 0.0488 ns | 0.0432 ns |  18.5546 ns |     ? |       ? |      - |         - |           ? |
| DictionaryLookup_RecordStruct | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| DictionaryLookup_RecordStruct | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Money_Add              | .NET 10.0 | .NET 10.0 |   6.3323 ns | 0.0300 ns | 0.0280 ns |   6.3330 ns |     ? |       ? |      - |         - |           ? |
| Domain_Money_Add              | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| Domain_Money_Add              | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Money_Allocate         | .NET 10.0 | .NET 10.0 | 291.5490 ns | 1.5281 ns | 1.4293 ns | 291.4719 ns |     ? |       ? | 0.0010 |      96 B |           ? |
| Domain_Money_Allocate         | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| Domain_Money_Allocate         | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Money_ApplyTax         | .NET 10.0 | .NET 10.0 |  45.8323 ns | 0.1697 ns | 0.1587 ns |  45.8408 ns |     ? |       ? |      - |         - |           ? |
| Domain_Money_ApplyTax         | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| Domain_Money_ApplyTax         | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Parse_BusinessDate     | .NET 10.0 | .NET 10.0 |  74.1420 ns | 0.3468 ns | 0.3244 ns |  74.1589 ns |     ? |       ? |      - |         - |           ? |
| Domain_Parse_BusinessDate     | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| Domain_Parse_BusinessDate     | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |

Benchmarks with issues:
  ValueObjectBenchmarks.Create_Class: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ValueObjectBenchmarks.Create_Class: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  ValueObjectBenchmarks.Create_RecordClass: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ValueObjectBenchmarks.Create_RecordClass: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  ValueObjectBenchmarks.Create_Struct: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ValueObjectBenchmarks.Create_Struct: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  ValueObjectBenchmarks.Create_RecordStruct: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ValueObjectBenchmarks.Create_RecordStruct: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  ValueObjectBenchmarks.Domain_Create_Email: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ValueObjectBenchmarks.Domain_Create_Email: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  ValueObjectBenchmarks.Domain_Create_Money: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ValueObjectBenchmarks.Domain_Create_Money: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  ValueObjectBenchmarks.Domain_Create_Percentage: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ValueObjectBenchmarks.Domain_Create_Percentage: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  ValueObjectBenchmarks.Equals_Class: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ValueObjectBenchmarks.Equals_Class: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  ValueObjectBenchmarks.Equals_RecordClass: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ValueObjectBenchmarks.Equals_RecordClass: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  ValueObjectBenchmarks.Equals_Struct: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ValueObjectBenchmarks.Equals_Struct: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  ValueObjectBenchmarks.Equals_RecordStruct: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ValueObjectBenchmarks.Equals_RecordStruct: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  ValueObjectBenchmarks.Domain_Equals_Money: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ValueObjectBenchmarks.Domain_Equals_Money: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  ValueObjectBenchmarks.HashCode_Class: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ValueObjectBenchmarks.HashCode_Class: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  ValueObjectBenchmarks.HashCode_RecordClass: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ValueObjectBenchmarks.HashCode_RecordClass: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  ValueObjectBenchmarks.HashCode_Struct: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ValueObjectBenchmarks.HashCode_Struct: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  ValueObjectBenchmarks.HashCode_RecordStruct: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ValueObjectBenchmarks.HashCode_RecordStruct: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  ValueObjectBenchmarks.DictionaryLookup_RecordClass: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ValueObjectBenchmarks.DictionaryLookup_RecordClass: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  ValueObjectBenchmarks.DictionaryLookup_RecordStruct: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ValueObjectBenchmarks.DictionaryLookup_RecordStruct: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  ValueObjectBenchmarks.Domain_Money_Add: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ValueObjectBenchmarks.Domain_Money_Add: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  ValueObjectBenchmarks.Domain_Money_Allocate: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ValueObjectBenchmarks.Domain_Money_Allocate: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  ValueObjectBenchmarks.Domain_Money_ApplyTax: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ValueObjectBenchmarks.Domain_Money_ApplyTax: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
  ValueObjectBenchmarks.Domain_Parse_BusinessDate: .NET 8.0(Runtime=.NET 8.0, Toolchain=net8.0)
  ValueObjectBenchmarks.Domain_Parse_BusinessDate: .NET 9.0(Runtime=.NET 9.0, Toolchain=net9.0)
