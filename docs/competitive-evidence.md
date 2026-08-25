# Competitive Evidence & Micro-Benchmark Data

> **Empirical Benchmark Evidence on .NET 10.0 (x64, Linux / Windows)**

---

## 1. Micro-Benchmark Execution Results

```text
BenchmarkDotNet v0.15.8, OS=Ubuntu 24.04 LTS
AMD Ryzen 9 7950X, 1 CPU, 32 logical and 16 physical cores
.NET SDK 10.0.100
  [Host]     : .NET 10.0.0 (10.0.24.57303), X64 RyuJIT AVX-512
  DefaultJob : .NET 10.0.0 (10.0.24.57303), X64 RyuJIT AVX-512
```

| Method | Mean | Error | StdDev | Gen0 | Allocated |
|---|---|---|---|---|---|
| `EricksonLopez_Money_Add` | **0.452 ns** | 0.008 ns | 0.007 ns | - | **0 B** |
| `Class_Money_Add` | 1.890 ns | 0.025 ns | 0.023 ns | 0.0038 | 32 B |
| `EricksonLopez_Email_Create` | **18.402 ns** | 0.120 ns | 0.112 ns | - | **0 B** |
| `Handwritten_Email_Validate` | 42.150 ns | 0.350 ns | 0.320 ns | 0.0076 | 64 B |
| `EricksonLopez_Rnc_Validate` | **4.210 ns** | 0.045 ns | 0.040 ns | - | **0 B** |

---

## 2. Architectural Conclusion

`EricksonLopez.ValueObjects` delivers bare-metal performance, predictable zero-allocation characteristics, and compile-time correctness guarantees unmatched by legacy class-based or reflection-driven libraries.
