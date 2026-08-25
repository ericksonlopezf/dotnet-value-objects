# ADR-008: Separation of Strongly Typed IDs into Dedicated Package

- **Status:** Accepted
- **Date:** 2026-08-16
- **Context:** Entity Identity vs. Value Objects

## Context and Problem Statement

Entity Identifiers (`CustomerId`, `OrderId`, `UserId`) represent identity rather than values with domain behavior. Conflating strongly typed IDs with business Value Objects leads to bloated base classes and unnecessary arithmetic dependencies.

## Decision

1. **Strong IDs Reside in DomainPrimitives:** Strongly typed entity identifiers are managed via `EricksonLopez.DomainPrimitives` (`IStrongId<TSelf, TValue>`).
2. **Business Codes Reside in ValueObjects:** Corporate operational codes with domain formatting rules (`TenantCode`, `EmployeeCode`, `SKU`, `CustomerCode`) reside in `EricksonLopez.ValueObjects`.

## Consequences

- **Positive:** Clear separation of concerns between entity identity and business values.
- **Negative:** Teams must distinguish between technical primary key wrappers (`UserId`) and domain operational codes (`EmployeeCode`).
