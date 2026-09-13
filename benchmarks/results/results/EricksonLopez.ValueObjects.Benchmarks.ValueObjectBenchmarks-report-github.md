```

BenchmarkDotNet v0.15.8, Linux Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763 2.45GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]    : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v3
  .NET 10.0 : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v3


```
| Method                        | Job       | Runtime   | Mean        | Error     | StdDev    | Median      | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------ |---------- |---------- |------------:|----------:|----------:|------------:|------:|--------:|-------:|----------:|------------:|
| Create_Class                  | .NET 10.0 | .NET 10.0 |   6.1026 ns | 0.0645 ns | 0.0603 ns |   6.1009 ns |     ? |       ? | 0.0014 |      24 B |           ? |
| Create_Class                  | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| Create_Class                  | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Create_RecordClass            | .NET 10.0 | .NET 10.0 |   6.4942 ns | 0.1785 ns | 0.1670 ns |   6.4646 ns |     ? |       ? | 0.0014 |      24 B |           ? |
| Create_RecordClass            | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| Create_RecordClass            | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Create_Struct                 | .NET 10.0 | .NET 10.0 |   0.0014 ns | 0.0014 ns | 0.0011 ns |   0.0015 ns |     ? |       ? |      - |         - |           ? |
| Create_Struct                 | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| Create_Struct                 | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Create_RecordStruct           | .NET 10.0 | .NET 10.0 |   0.0007 ns | 0.0019 ns | 0.0015 ns |   0.0000 ns |     ? |       ? |      - |         - |           ? |
| Create_RecordStruct           | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| Create_RecordStruct           | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Create_Email           | .NET 10.0 | .NET 10.0 | 166.9218 ns | 0.2979 ns | 0.2487 ns | 166.9475 ns |     ? |       ? | 0.0091 |     152 B |           ? |
| Domain_Create_Email           | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| Domain_Create_Email           | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Create_Money           | .NET 10.0 | .NET 10.0 |  11.1190 ns | 0.0106 ns | 0.0088 ns |  11.1174 ns |     ? |       ? |      - |         - |           ? |
| Domain_Create_Money           | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| Domain_Create_Money           | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Create_Percentage      | .NET 10.0 | .NET 10.0 |   7.4341 ns | 0.0137 ns | 0.0114 ns |   7.4317 ns |     ? |       ? |      - |         - |           ? |
| Domain_Create_Percentage      | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| Domain_Create_Percentage      | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Equals_Class                  | .NET 10.0 | .NET 10.0 |   0.5660 ns | 0.0042 ns | 0.0040 ns |   0.5660 ns |     ? |       ? |      - |         - |           ? |
| Equals_Class                  | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| Equals_Class                  | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Equals_RecordClass            | .NET 10.0 | .NET 10.0 |   0.7552 ns | 0.0078 ns | 0.0069 ns |   0.7558 ns |     ? |       ? |      - |         - |           ? |
| Equals_RecordClass            | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| Equals_RecordClass            | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Equals_Struct                 | .NET 10.0 | .NET 10.0 |   0.4637 ns | 0.0019 ns | 0.0016 ns |   0.4633 ns |     ? |       ? |      - |         - |           ? |
| Equals_Struct                 | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| Equals_Struct                 | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Equals_RecordStruct           | .NET 10.0 | .NET 10.0 |   0.3280 ns | 0.0091 ns | 0.0081 ns |   0.3255 ns |     ? |       ? |      - |         - |           ? |
| Equals_RecordStruct           | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| Equals_RecordStruct           | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Equals_Money           | .NET 10.0 | .NET 10.0 |   5.1174 ns | 0.0039 ns | 0.0033 ns |   5.1175 ns |     ? |       ? |      - |         - |           ? |
| Domain_Equals_Money           | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| Domain_Equals_Money           | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| HashCode_Class                | .NET 10.0 | .NET 10.0 |  18.8705 ns | 0.0061 ns | 0.0051 ns |  18.8703 ns |     ? |       ? |      - |         - |           ? |
| HashCode_Class                | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| HashCode_Class                | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| HashCode_RecordClass          | .NET 10.0 | .NET 10.0 |  22.3978 ns | 0.0971 ns | 0.0861 ns |  22.4411 ns |     ? |       ? |      - |         - |           ? |
| HashCode_RecordClass          | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| HashCode_RecordClass          | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| HashCode_Struct               | .NET 10.0 | .NET 10.0 |  18.5336 ns | 0.0069 ns | 0.0061 ns |  18.5334 ns |     ? |       ? |      - |         - |           ? |
| HashCode_Struct               | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| HashCode_Struct               | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| HashCode_RecordStruct         | .NET 10.0 | .NET 10.0 |  17.7530 ns | 0.0237 ns | 0.0210 ns |  17.7439 ns |     ? |       ? |      - |         - |           ? |
| HashCode_RecordStruct         | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| HashCode_RecordStruct         | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| DictionaryLookup_RecordClass  | .NET 10.0 | .NET 10.0 |  26.0812 ns | 0.0999 ns | 0.0834 ns |  26.0744 ns |     ? |       ? |      - |         - |           ? |
| DictionaryLookup_RecordClass  | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| DictionaryLookup_RecordClass  | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| DictionaryLookup_RecordStruct | .NET 10.0 | .NET 10.0 |  24.0436 ns | 0.0241 ns | 0.0213 ns |  24.0328 ns |     ? |       ? |      - |         - |           ? |
| DictionaryLookup_RecordStruct | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| DictionaryLookup_RecordStruct | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Money_Add              | .NET 10.0 | .NET 10.0 |   8.7502 ns | 0.0052 ns | 0.0041 ns |   8.7505 ns |     ? |       ? |      - |         - |           ? |
| Domain_Money_Add              | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| Domain_Money_Add              | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Money_Allocate         | .NET 10.0 | .NET 10.0 | 369.9921 ns | 0.3734 ns | 0.3310 ns | 370.0024 ns |     ? |       ? | 0.0057 |      96 B |           ? |
| Domain_Money_Allocate         | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| Domain_Money_Allocate         | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Money_ApplyTax         | .NET 10.0 | .NET 10.0 |  55.3658 ns | 0.0247 ns | 0.0219 ns |  55.3622 ns |     ? |       ? |      - |         - |           ? |
| Domain_Money_ApplyTax         | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
| Domain_Money_ApplyTax         | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
|                               |           |           |             |           |           |             |       |         |        |           |             |
| Domain_Parse_BusinessDate     | .NET 10.0 | .NET 10.0 | 127.1179 ns | 0.2004 ns | 0.1776 ns | 127.0445 ns |     ? |       ? |      - |         - |           ? |
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
