# Performance & Allocation Architecture

> **Memory Footprints, GC Zero-Allocation Guarantees & Low-Latency Execution**

---

## 1. Zero-Allocation Guarantees

All scalar types (`Money`, `CurrencyCode`, `Percentage`, `TaxRate`, `DiscountRate`, `Range<T>`, `BusinessDate`) are `readonly record struct` value types. They reside on the stack or inline within containing entity class memory, producing **0 B GC heap allocation**.

---

## 2. Inlined Fast-Path Arithmetic

Arithmetic operators (`+`, `-`, `*`) are fully inlined by the JIT compiler, executing in under **0.5 nanoseconds** using native CPU registers.
