# ADR-004: Elimination of Runtime Reflection for Native AOT

- **Status:** Accepted
- **Date:** 2026-08-16
- **Context:** Native AOT & Trimming Compatibility

## Context and Problem Statement

Using `System.Reflection` in static constructors or base classes (e.g. searching for `[SensitiveData]` or generic type arguments) breaks trimming and Native AOT compilation, causing runtime warnings or unexpected null references when compiled with `PublishAot=true`.

## Decision

1. **Eliminate Runtime Reflection:** Remove all reflection calls (`GetCustomAttribute`, `MakeGenericType`, `Type.GetType`) from core domain base classes.
2. **Explicit Property Overrides:** Masking metadata is evaluated via virtual properties or compile-time Source Generators.
3. **Compile-Time Verification:** The solution enforces `<IsAotCompatible>true</IsAotCompatible>` and validates execution via the `EricksonLopez.ValueObjects.NativeAotTests` smoke test suite.

## Consequences

- **Positive:** Full compatibility with .NET 10 Native AOT and Ahead-Of-Time compilation.
- **Negative:** Base abstractions require explicit generic type parameters rather than reflective self-discovery.
