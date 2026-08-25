# ADR-009: Refactoring Domain Primitives via Source Generators

- **Status:** Accepted
- **Date:** 2026-08-16
- **Context:** Compile-Time Boilerplate Reduction

## Context and Problem Statement

Manually implementing `IParsable<TSelf>` and `ISpanParsable<TSelf>` across dozens of Value Objects creates repetitive boilerplate that is prone to human error and formatting drift.

## Decision

1. **Roslyn Incremental Generator:** Implement `EricksonLopez.ValueObjects.Generators` to automatically synthesize `IParsable<TSelf>` and `ISpanParsable<TSelf>` implementations for partial records decorated with `[ValueObject]`.
2. **Deterministic Output:** The generator generates pure C# code directly hooked to the static `Create` method.

## Consequences

- **Positive:** Eliminates thousands of lines of boilerplate; guarantees zero reflection.
- **Negative:** Value Object records utilizing the generator must be declared as `partial`.
