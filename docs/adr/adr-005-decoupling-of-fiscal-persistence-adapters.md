# ADR-005: Decoupling of Fiscal Persistence Adapters

- **Status:** Accepted
- **Date:** 2026-08-16
- **Context:** Entity Framework Core & Dapper Persistence

## Context and Problem Statement

Directly coupling `EricksonLopez.ValueObjects.EntityFrameworkCore` to all 6 fiscal satellite packages forces consumers who only need universal value objects (like `Email` or `Money`) to transitively pull unnecessary fiscal assemblies.

## Decision

1. **Generic Persistence Converters:** `EricksonLopez.ValueObjects.EntityFrameworkCore` and `EricksonLopez.ValueObjects.Dapper` provide open generic adapters (`StringValueObjectValueConverter<TVO>`, `SingleValueObjectTypeHandler<TVO, TRaw>`).
2. **Decoupled Consumer Registration:** Consumers register specific fiscal types via open generic converters without creating monolithic cross-package references in EF Core libraries.

## Consequences

- **Positive:** Modular package footprint and clean dependency trees.
- **Negative:** Applications using fiscal packages configure their EF Core mappings explicitly in `ConfigureConventions`.
