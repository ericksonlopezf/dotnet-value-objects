# ADR-013: Sensitive Data Protection (PII) in Domain and Debugger

- **Status:** Accepted
- **Date:** 2026-08-16
- **Context:** PII Defense & Security Logging

## Context and Problem Statement

Value objects containing Personally Identifiable Information (PII) or credentials (e.g. `PasswordHash`, `NationalId`, `PassportNumber`, `Cedula`, `Rnc`, `Cuit`, `Rut`) risk leaking unmasked plaintext into log sinks, telemetry, and IDE debugger watch windows when `ToString()` is invoked.

## Decision

1. **`[SensitiveData]` Decoration:** Annotate all sensitive types with `[SensitiveData(mask: "...")]`.
2. **Masked `ToString()` & Debugger Display:** `ToString()` and `[DebuggerDisplay]` must automatically redact the sensitive payload by default, unless explicitly requested via raw property access.

## Consequences

- **Positive:** Automatic protection against accidental data leakage into log aggregators.
- **Negative:** Debugging requires viewing the explicit `.Value` property to inspect unmasked values.
