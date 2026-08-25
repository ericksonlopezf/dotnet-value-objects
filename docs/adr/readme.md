# Architecture Decision Records (ADRs)

This directory contains the formal record of accepted and rejected architectural decisions for the `EricksonLopez.ValueObjects` ecosystem.

---

## 1. Accepted Architectural Decisions (ADR)

- [ADR-001: Adoption of Result Pattern over Exceptions](adr-001-result-pattern-over-exceptions.md)
- [ADR-002: Fiscal Catalogs as Dynamic Value Objects, NOT Enums](adr-002-dynamic-catalogs-vs-enums.md)
- [ADR-003: Absolute Immutability and Zero-Allocation Performance](adr-003-immutability-zero-allocations.md)
- [ADR-004: Elimination of Runtime Reflection for Native AOT](adr-004-elimination-of-runtime-reflection-for-native-aot.md)
- [ADR-005: Decoupling Fiscal Persistence Adapters in EntityFrameworkCore](adr-005-decoupling-of-fiscal-persistence-adapters.md)
- [ADR-006: Segregation of ValidationResult to Foundation Layer](adr-006-segregation-of-validation-result.md)
- [ADR-007: Conceptual Separation between Domain Primitives and Value Objects](adr-007-separation-of-domain-primitives-and-value-objects.md)
- [ADR-008: Separation of Strongly Typed IDs into Dedicated Package](adr-008-strongly-typed-ids-separation.md)
- [ADR-009: Refactoring Domain Primitives via Source Generators](adr-009-refactoring-domain-primitives-via-source-generators.md)
- [ADR-010: Range<T> as Zero-Allocation readonly record struct](adr-010-range-as-readonly-record-struct.md)
- [ADR-011: Rejection of Implicit Conversion Operators](adr-011-rejection-of-implicit-conversion-operators.md)
- [ADR-012: Decoupling EntityFrameworkCore from Fiscal Satellites](adr-012-decoupling-entity-framework-core-from-fiscal-satellites.md)
- [ADR-013: Sensitive Data Protection (PII) in Domain and Debugger](adr-013-sensitive-data-masking-in-domain-and-debugger.md)
- [ADR-014: Standardized Testing Conventions with Osherove Pattern](adr-014-testing-conventions-and-osherove-pattern.md)

---

## 2. Rejected Architectural Decisions (REJ)

- [REJ-001: Rejection of Unified GlobalTaxId Abstraction](rej-001-rejection-global-tax-id.md)
- [REJ-002: Rejection of Raw Decimal Math for Tax Calculations](rej-002-rejection-raw-decimal-tax-calculations.md)
- [REJ-003: Rejection of Closed Enums for Government Catalogs](rej-003-rejection-enums-for-fiscal-catalogs.md)
