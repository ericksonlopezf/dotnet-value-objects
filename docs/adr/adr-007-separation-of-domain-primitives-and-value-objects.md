# ADR-007: Conceptual Separation between Domain Primitives and Value Objects

- **Status:** Accepted
- **Date:** 2026-08-16
- **Context:** Architectural Governance (AO-VO-001)

## Context and Problem Statement

`EricksonLopez.DomainPrimitives` and `EricksonLopez.ValueObjects` address related aspects of DDD modeling. Without formal boundaries, duplicate code generators and parsing infrastructure could emerge across both repositories.

## Decision

1. **Capability Boundary:**
   - **`EricksonLopez.DomainPrimitives`**: Owns code generation infrastructure, strongly typed primitive attributes (`[StringPrimitive]`), and BCL parsing interfaces.
   - **`EricksonLopez.ValueObjects`**: Owns rich domain implementations, complex regulatory business rules (e.g. Fowler Money allocation, tax algorithms), and cross-value validation.
2. **Bridge Package:** The bridge package `EricksonLopez.ValueObjects.DomainPrimitives` provides `ToDomainPrimitive` and `ToStrongId` adapters.

## Consequences

- **Positive:** Zero duplication of generators or infrastructure code.
- **Negative:** Requires understanding the distinction between simple primitive wrappers and rich Value Objects.
