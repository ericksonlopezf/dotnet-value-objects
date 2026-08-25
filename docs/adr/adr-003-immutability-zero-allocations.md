# ADR-003: Absolute Immutability and Zero-Allocation Performance

- **Status:** Accepted
- **Date:** 2026-08-16
- **Context:** Core Architecture, Memory Management & GC Pressure

## Context and Problem Statement

High-throughput systems executing millions of financial transactions per second suffer performance bottlenecks if Value Objects generate excessive heap allocations in GC Generation 0.

## Decision

1. **`readonly record struct` for Scalars:** Scalar numbers, percentages, temporal units, and continuous ranges (`Money`, `Quantity`, `Percentage`, `TaxRate`, `DiscountRate`, `DateRange`, `Range<T>`) must be implemented as `readonly record struct`.
2. **`sealed record : StringValueObject<TSelf>` for Text:** Normalized string-backed objects inherit from `StringValueObject<TSelf>` using pre-compiled regex pipelines.
3. **Compile-Time Immutability Check:** Roslyn Analyzer `ELVO003` fails the build if any property or field in a Value Object allows mutation (`set` without `init`).

## Consequences

- **Positive:** Zero heap allocations (Allocated = 0 B) for struct Value Objects; thread-safe by default.
- **Negative:** Value types copied across function calls require pass-by-reference (`in` or `ref readonly`) in extreme hot paths to avoid copy overhead.
