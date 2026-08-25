# ADR-012: Decoupling EntityFrameworkCore from Fiscal Satellites

- **Status:** Accepted
- **Date:** 2026-08-16
- **Context:** Modular Packaging & EF Core Converters

## Context and Problem Statement

Having `EricksonLopez.ValueObjects.EntityFrameworkCore` reference all 6 fiscal satellite packages causes circular dependency concerns and violates single responsibility.

## Decision

1. **Keep EF Core Package Lean:** `EricksonLopez.ValueObjects.EntityFrameworkCore` references only `EricksonLopez.ValueObjects` (Core).
2. **Open Generic Converters:** Fiscal value objects are mapped using `StringValueObjectValueConverter<TVO>` or `SingleValueObjectValueConverter<TVO, TRaw>` in the consumer's `DbContext`.

## Consequences

- **Positive:** Minimal package dependencies and clean build times.
- **Negative:** Consuming applications explicitly register fiscal converters in `ConfigureConventions`.
