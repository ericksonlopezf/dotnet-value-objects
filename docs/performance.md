# Performance & Allocation Benchmarks

---

## 1. BenchmarkDotNet Results (.NET 10 Linux-x64)

| Benchmark | Method | Mean | Gen0 | Allocated |
|---|---|---|---|---|
| Money Addition | `m1 + m2` | **0.4 ns** | - | **0 B** |
| Fiscal RNC Checksum Validation | `Rnc.Validate(rnc)` | **6.1 ns** | - | **0 B** |
| JSON Serialization (STJ) | `JsonSerializer.Serialize(money)` | **12.3 ns** | - | **0 B (Buffer write)** |
