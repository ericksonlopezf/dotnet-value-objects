// Copyright © Erickson Lopez. MIT License.
using System;
using System.Collections.Generic;
using System.Globalization;
using BenchmarkDotNet.Attributes;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;

namespace EricksonLopez.ValueObjects.Benchmarks;

[MemoryDiagnoser]
public class ValueObjectBenchmarks
{
    private const string RawEmail = "benchmark.user@example.com";
    private readonly ClassEmail _classEmail1 = new(RawEmail);
    private readonly ClassEmail _classEmail2 = new(RawEmail);
    private readonly RecordClassEmail _recClassEmail1 = new(RawEmail);
    private readonly RecordClassEmail _recClassEmail2 = new(RawEmail);
    private readonly StructEmail _structEmail1 = new(RawEmail);
    private readonly StructEmail _structEmail2 = new(RawEmail);
    private readonly RecordStructEmail _recStructEmail1 = new(RawEmail);
    private readonly RecordStructEmail _recStructEmail2 = new(RawEmail);

    private readonly Money _money1 = Money.Create(100.50m, CurrencyCode.USD).Value;
    private readonly Money _money2 = Money.Create(50.25m, CurrencyCode.USD).Value;
    private readonly Percentage _percentage = Percentage.Create(18.5m).Value;
    private readonly BusinessDate _businessDate = BusinessDate.Create(new DateOnly(2026, 8, 16)).Value;

    private readonly Dictionary<ClassEmail, int> _classDict = [];
    private readonly Dictionary<RecordClassEmail, int> _recClassDict = [];
    private readonly Dictionary<StructEmail, int> _structDict = [];
    private readonly Dictionary<RecordStructEmail, int> _recStructDict = [];

    [GlobalSetup]
    public void Setup()
    {
        _classDict[_classEmail1] = 1;
        _recClassDict[_recClassEmail1] = 1;
        _structDict[_structEmail1] = 1;
        _recStructDict[_recStructEmail1] = 1;
    }

    [Benchmark]
    public ClassEmail Create_Class() => new(RawEmail);

    [Benchmark]
    public RecordClassEmail Create_RecordClass() => new(RawEmail);

    [Benchmark]
    public StructEmail Create_Struct() => new(RawEmail);

    [Benchmark]
    public RecordStructEmail Create_RecordStruct() => new(RawEmail);

    [Benchmark]
    public Result<Email> Domain_Create_Email() => Email.Create(RawEmail);

    [Benchmark]
    public Result<Money> Domain_Create_Money() => Money.Create(100.50m, CurrencyCode.USD);

    [Benchmark]
    public Result<Percentage> Domain_Create_Percentage() => Percentage.Create(18.5m);

    [Benchmark]
    public bool Equals_Class() => _classEmail1.Equals(_classEmail2);

    [Benchmark]
    public bool Equals_RecordClass() => _recClassEmail1.Equals(_recClassEmail2);

    [Benchmark]
    public bool Equals_Struct() => _structEmail1.Equals(_structEmail2);

    [Benchmark]
    public bool Equals_RecordStruct() => _recStructEmail1.Equals(_recStructEmail2);

    [Benchmark]
    public bool Domain_Equals_Money() => _money1 == _money2;

    [Benchmark]
    public int HashCode_Class() => _classEmail1.GetHashCode();

    [Benchmark]
    public int HashCode_RecordClass() => _recClassEmail1.GetHashCode();

    [Benchmark]
    public int HashCode_Struct() => _structEmail1.GetHashCode();

    [Benchmark]
    public int HashCode_RecordStruct() => _recStructEmail1.GetHashCode();

    [Benchmark]
    public int DictionaryLookup_RecordClass() => _recClassDict[_recClassEmail1];

    [Benchmark]
    public int DictionaryLookup_RecordStruct() => _recStructDict[_recStructEmail1];

    [Benchmark]
    public Result<Money> Domain_Money_Add() => _money1.Add(_money2);

    [Benchmark]
    public Money[] Domain_Money_Allocate() => _money1.Allocate(1, 2, 3);

    [Benchmark]
    public Money Domain_Money_ApplyTax() => _money1.ApplyPercentage(_percentage);

    [Benchmark]
    public BusinessDate Domain_Parse_BusinessDate() => BusinessDate.Parse("2026-08-16", CultureInfo.InvariantCulture);
}

