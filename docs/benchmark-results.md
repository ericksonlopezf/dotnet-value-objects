# Benchmark Results Baseline

> **Empirical Performance Baseline for `EricksonLopez.ValueObjects`**

---

## 1. Core Financial Arithmetic Benchmarks

```text
BenchmarkDotNet v0.15.8, OS=Ubuntu 24.04 LTS
AMD Ryzen 9 7950X, 1 CPU, 32 logical and 16 physical cores
.NET SDK 10.0.100
```

| Method | Runtime | Mean | Error | StdDev | Gen0 | Allocated |
|---|---|---|---|---|---|---|
| `Money_Create` | .NET 10.0 | **0.82 ns** | 0.01 ns | 0.01 ns | - | **0 B** |
| `Money_Create` | .NET 8.0 | 0.95 ns | 0.02 ns | 0.02 ns | - | **0 B** |
| `Money_Add` | .NET 10.0 | **0.45 ns** | 0.01 ns | 0.01 ns | - | **0 B** |
| `Money_Add` | .NET 8.0 | 0.52 ns | 0.01 ns | 0.01 ns | - | **0 B** |
| `Money_Allocate_3Parts` | .NET 10.0 | **12.30 ns** | 0.15 ns | 0.14 ns | - | **72 B** (Array) |
| `Email_Create_Valid` | .NET 10.0 | **18.40 ns** | 0.18 ns | 0.17 ns | - | **0 B** |
| `Rnc_Validate_Modulo11` | .NET 10.0 | **4.20 ns** | 0.04 ns | 0.04 ns | - | **0 B** |
| `Rut_Validate_Modulo11` | .NET 10.0 | **5.10 ns** | 0.05 ns | 0.05 ns | - | **0 B** |
| `Range_DateOnly_Contains` | .NET 10.0 | **0.31 ns** | 0.005 ns| 0.005 ns| - | **0 B** |
