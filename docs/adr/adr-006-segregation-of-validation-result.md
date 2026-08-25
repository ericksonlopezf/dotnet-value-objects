# ADR-006: Segregation of ValidationResult to Foundation Layer

- **Status:** Accepted
- **Date:** 2026-08-16
- **Context:** Core Domain Abstractions & Result Pattern Alignment

## Context and Problem Statement

Having a local or redundant `ValidationResult` in the Value Objects core creates cognitive friction and duplicate error-handling abstractions against `EricksonLopez.Result` (L0 Foundation).

## Decision

1. **Standardize on `EricksonLopez.Result`:** All factories and validation logic return standard `Result<T>` or `Result` types containing structured `Error` objects (`Error.Validation(...)`).
2. **Remove Local Redundancies:** Prohibit bespoke validation container classes in the domain core.

## Consequences

- **Positive:** Uniform functional error handling across all repositories in the ecosystem.
- **Negative:** Hard dependency on `EricksonLopez.Result`.
