# Benchmark Plan & Performance Methodology

> **BenchmarkDotNet Execution Plan across .NET 8, .NET 9, and .NET 10**

---

## 1. Objectives

1. Quantify execution latency and GC heap allocations for all core Value Objects.
2. Verify zero-allocation guarantees on mathematical and parsing hot paths.
3. Track performance regressions across pull requests and framework upgrades.

---

## 2. Benchmark Categories

- **Financial Arithmetic**: `Money.Create`, `Money.Add`, `Money.Subtract`, `Money.Allocate`, `Money.Distribute`.
- **String Primitives & Parsing**: `Email.Create`, `PhoneNumber.Create`, `Address.Create`, `Range<T>.Contains`.
- **Fiscal Tax Satellites**: `Rnc.Create` (DO), `Rut.Create` (CL), `Nit.Create` (CO), `Rfc.Create` (MX), `Ruc.Create` (PE), `Cuit.Create` (AR).
- **Serialization & Persistence**: `System.Text.Json` serialization vs EF Core / Dapper type mapping.

---

## 3. Configuration & Hardware Baseline

- **Hardware**: AMD Ryzen 9 7950X, 64GB DDR5 6000MHz.
- **Runtimes**: .NET 8.0, .NET 9.0, .NET 10.0 (x64 RyuJIT).
- **Diagnoser**: `[MemoryDiagnoser]` enabled on all benchmarks.
