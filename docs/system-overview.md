# System Overview & Architecture Topology

> **High-Level Ecosystem Overview of `EricksonLopez.ValueObjects`**

---

## 1. High-Level Overview

`EricksonLopez.ValueObjects` provides foundational Domain-Driven Design building blocks for enterprise .NET systems. It is structured into three main layers:

1. **Domain Kernel Layer**: Universal value objects, numeric/financial types (`Money`, `CurrencyCode`, `Range<T>`), and base abstractions.
2. **Fiscal Satellite Layer**: Autonomous country-specific regulatory tax and electronic invoice packages.
3. **Infrastructure & Tooling Layer**: High-performance persistence adapters (EF Core 10, Dapper), System.Text.Json converters, Roslyn source generators, and architectural code analyzers.
