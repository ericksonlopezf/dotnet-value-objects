# ADR-011: Rejection of Implicit Conversion Operators

- **Status:** Accepted
- **Date:** 2026-08-16
- **Context:** Type Safety and Domain Integrity

## Context and Problem Statement

Allowing implicit conversion operators (`implicit operator string(Email email)` or `implicit operator Email(string raw)`) allows unvalidated strings to silently enter domain entities and bypass the factory validation pipeline.

## Decision

1. **Prohibit Implicit Conversions:** Implicit conversions from primitives to Value Objects and vice-versa are strictly forbidden across the entire ecosystem.
2. **Explicit Property Access:** Values must be read explicitly via the `.Value` property or `.ToString()`.
3. **Explicit Factory Instantiation:** Value Objects must be created via `.Create(...)`.

## Consequences

- **Positive:** Guarantees compile-time safety and prevents accidental primitive obsession bypasses.
- **Negative:** Slightly more verbose syntax when passing inner values to third-party APIs.
