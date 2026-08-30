
BenchmarkDotNet v0.15.8, Linux Ubuntu 24.04.4 LTS (Noble Numbat)
AMD EPYC 7763 2.60GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.400
  [Host]    : .NET 10.0.11 (10.0.11, 10.0.1126.37416), X64 RyuJIT x86-64-v3
  .NET 10.0 : .NET 10.0.11 (10.0.11, 10.0.1126.37416), X64 RyuJIT x86-64-v3


 Method                        | Job       | Runtime   | Mean        | Error     | StdDev    | Median      | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
------------------------------ |---------- |---------- |------------:|----------:|----------:|------------:|------:|--------:|-------:|----------:|------------:|
 Create_Class                  | .NET 10.0 | .NET 10.0 |  14.6220 ns | 0.3542 ns | 0.8689 ns |  14.6140 ns |     ? |       ? | 0.0014 |      24 B |           ? |
 Create_Class                  | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
 Create_Class                  | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Create_RecordClass            | .NET 10.0 | .NET 10.0 |  15.0651 ns | 0.3644 ns | 0.9915 ns |  15.1702 ns |     ? |       ? | 0.0014 |      24 B |           ? |
 Create_RecordClass            | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
 Create_RecordClass            | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Create_Struct                 | .NET 10.0 | .NET 10.0 |   0.0008 ns | 0.0007 ns | 0.0006 ns |   0.0008 ns |     ? |       ? |      - |         - |           ? |
 Create_Struct                 | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
 Create_Struct                 | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Create_RecordStruct           | .NET 10.0 | .NET 10.0 |   0.0005 ns | 0.0007 ns | 0.0006 ns |   0.0000 ns |     ? |       ? |      - |         - |           ? |
 Create_RecordStruct           | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
 Create_RecordStruct           | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Domain_Create_Email           | .NET 10.0 | .NET 10.0 | 166.8870 ns | 0.5310 ns | 0.4967 ns | 167.0560 ns |     ? |       ? | 0.0091 |     152 B |           ? |
 Domain_Create_Email           | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
 Domain_Create_Email           | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Domain_Create_Money           | .NET 10.0 | .NET 10.0 |  11.1980 ns | 0.0081 ns | 0.0068 ns |  11.1964 ns |     ? |       ? |      - |         - |           ? |
 Domain_Create_Money           | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
 Domain_Create_Money           | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Domain_Create_Percentage      | .NET 10.0 | .NET 10.0 |   7.4336 ns | 0.0038 ns | 0.0035 ns |   7.4331 ns |     ? |       ? |      - |         - |           ? |
 Domain_Create_Percentage      | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
 Domain_Create_Percentage      | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Equals_Class                  | .NET 10.0 | .NET 10.0 |   0.5929 ns | 0.0048 ns | 0.0043 ns |   0.5937 ns |     ? |       ? |      - |         - |           ? |
 Equals_Class                  | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
 Equals_Class                  | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Equals_RecordClass            | .NET 10.0 | .NET 10.0 |   0.5398 ns | 0.0062 ns | 0.0055 ns |   0.5404 ns |     ? |       ? |      - |         - |           ? |
 Equals_RecordClass            | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
 Equals_RecordClass            | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Equals_Struct                 | .NET 10.0 | .NET 10.0 |   0.5194 ns | 0.0024 ns | 0.0022 ns |   0.5192 ns |     ? |       ? |      - |         - |           ? |
 Equals_Struct                 | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
 Equals_Struct                 | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Equals_RecordStruct           | .NET 10.0 | .NET 10.0 |   0.3188 ns | 0.0045 ns | 0.0038 ns |   0.3184 ns |     ? |       ? |      - |         - |           ? |
 Equals_RecordStruct           | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
 Equals_RecordStruct           | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Domain_Equals_Money           | .NET 10.0 | .NET 10.0 |   5.0967 ns | 0.0048 ns | 0.0043 ns |   5.0960 ns |     ? |       ? |      - |         - |           ? |
 Domain_Equals_Money           | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
 Domain_Equals_Money           | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
                               |           |           |             |           |           |             |       |         |        |           |             |
 HashCode_Class                | .NET 10.0 | .NET 10.0 |  18.8870 ns | 0.0110 ns | 0.0098 ns |  18.8873 ns |     ? |       ? |      - |         - |           ? |
 HashCode_Class                | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
 HashCode_Class                | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
                               |           |           |             |           |           |             |       |         |        |           |             |
 HashCode_RecordClass          | .NET 10.0 | .NET 10.0 |  22.3936 ns | 0.1159 ns | 0.1084 ns |  22.4406 ns |     ? |       ? |      - |         - |           ? |
 HashCode_RecordClass          | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
 HashCode_RecordClass          | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
                               |           |           |             |           |           |             |       |         |        |           |             |
 HashCode_Struct               | .NET 10.0 | .NET 10.0 |  18.5396 ns | 0.0095 ns | 0.0084 ns |  18.5370 ns |     ? |       ? |      - |         - |           ? |
 HashCode_Struct               | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
 HashCode_Struct               | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
                               |           |           |             |           |           |             |       |         |        |           |             |
 HashCode_RecordStruct         | .NET 10.0 | .NET 10.0 |  17.7636 ns | 0.0225 ns | 0.0200 ns |  17.7632 ns |     ? |       ? |      - |         - |           ? |
 HashCode_RecordStruct         | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
 HashCode_RecordStruct         | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
                               |           |           |             |           |           |             |       |         |        |           |             |
 DictionaryLookup_RecordClass  | .NET 10.0 | .NET 10.0 |  26.6826 ns | 0.0101 ns | 0.0089 ns |  26.6818 ns |     ? |       ? |      - |         - |           ? |
 DictionaryLookup_RecordClass  | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
 DictionaryLookup_RecordClass  | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
                               |           |           |             |           |           |             |       |         |        |           |             |
 DictionaryLookup_RecordStruct | .NET 10.0 | .NET 10.0 |  24.0312 ns | 0.0070 ns | 0.0065 ns |  24.0307 ns |     ? |       ? |      - |         - |           ? |
 DictionaryLookup_RecordStruct | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
 DictionaryLookup_RecordStruct | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Domain_Money_Add              | .NET 10.0 | .NET 10.0 |   8.7584 ns | 0.0079 ns | 0.0070 ns |   8.7591 ns |     ? |       ? |      - |         - |           ? |
 Domain_Money_Add              | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
 Domain_Money_Add              | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Domain_Money_Allocate         | .NET 10.0 | .NET 10.0 | 372.6612 ns | 0.3925 ns | 0.3277 ns | 372.5613 ns |     ? |       ? | 0.0057 |      96 B |           ? |
 Domain_Money_Allocate         | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
 Domain_Money_Allocate         | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Domain_Money_ApplyTax         | .NET 10.0 | .NET 10.0 |  55.3506 ns | 0.0318 ns | 0.0282 ns |  55.3365 ns |     ? |       ? |      - |         - |           ? |
 Domain_Money_ApplyTax         | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
 Domain_Money_ApplyTax         | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
                               |           |           |             |           |           |             |       |         |        |           |             |
 Domain_Parse_BusinessDate     | .NET 10.0 | .NET 10.0 | 128.7058 ns | 0.1051 ns | 0.0983 ns | 128.6630 ns |     ? |       ? |      - |         - |           ? |
 Domain_Parse_BusinessDate     | .NET 8.0  | .NET 8.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |
 Domain_Parse_BusinessDate     | .NET 9.0  | .NET 9.0  |          NA |        NA |        NA |          NA |     ? |       ? |     NA |        NA |           ? |

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
