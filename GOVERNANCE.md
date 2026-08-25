# Governance

> **Version:** 2.1  
> **Last Updated:** 2026-08-25  
> **Required by:** Engineering Specification v4.0 §11.3

---

## Core Committee

The Core Committee is responsible for reviewing RFCs, approving breaking changes, and
maintaining the architectural quality and strategic direction of the `EricksonLopez.ValueObjects` library ecosystem.

| Member | Role | Contact |
|--------|------|---------|
| Erickson Lopez | Lead Maintainer | [@ericksonlopezf](https://github.com/ericksonlopezf) ([ericksonlopezf@gmail.com](mailto:ericksonlopezf@gmail.com)) |

> **Quorum requirement:** For a committee of 1, 1 approval is required for all decisions.
> As the committee grows, the voting rules below apply.

---

## Design Principles (Non-Negotiable)

These constraints are enforced by the engineering specification and may not be overridden by RFC:

1. **Zero allocation on scalar numeric, financial, and temporal hot paths.** `Money`, `CurrencyCode`, `Percentage`, `TaxRate`, `DiscountRate`, `Range<T>`, and `BusinessDate` must be `readonly record struct` value types residing entirely on the stack or inline within entity layouts.
2. **Zero reflection in hot paths and persistence converters.** All code in `Create()`, `TryParse()`, EF Core `ValueConverter`, Dapper `SqlMapper.TypeHandler`, and System.Text.Json converters must be reflection-free and NativeAOT-compatible.
3. **Strict statutory accuracy for fiscal tax satellites.** All algorithms in country satellites (Modulo 11, Modulo 10, Luhn, prime-weighted checks, electronic invoice schemes) must strictly adhere to current statutory tax mandates without approximation.
4. **Martin Fowler's lossless allocation for financial operations.** Remainder cents during `Money.Allocate(ratios)` or `Money.Distribute(parts)` must be distributed deterministically to prevent rounding loss.
5. **Decoupled satellite architecture.** Domain kernel types must not depend on database ORMs or country satellites. Country satellites depend on core abstractions only.

---

## RFC Process

All changes that meet the criteria below **MUST** go through the RFC process before implementation begins.

### When an RFC is Required

| Change Type | RFC Required? | Examples |
|-------------|--------------|---------|
| New fiscal country satellite | ✅ Required | Adding `EricksonLopez.ValueObjects.Fiscal.Brazil` |
| New core value object | ✅ Required | Adding `Dimensions`, `GeoPoint`, `Weight` |
| Breaking source change | ✅ Required | Renaming public methods, changing method signatures |
| Breaking binary change | ✅ Required | Removing public members, changing types |
| Behavioral breaking change | ✅ Required | Changing validation defaults, error semantics |
| New generator capability | ✅ Required | Adding new attributes, changing generated code structure |
| Governance changes | ✅ Required | Changes to this document |
| Additive (non-breaking) API | ❌ Optional | Adding optional parameters, new overloads |
| Bugfix | ❌ Not Required | Fixing incorrect behavior that is clearly a bug |
| Documentation | ❌ Not Required | README, ADR, XML doc improvements |
| Tooling / infra | ❌ Not Required | CI scripts, benchmark changes, gitignore |

### Definition of "Trivial" (RFC bypass eligible)

A change is **trivial** if ALL of the following are true:
- Does not touch any public API surface (no changes to generated method signatures or names)
- Does not change validation semantics (no changes to error codes, messages, or conditions)
- Has ≤ 50 lines of net change in source files (excluding tests and docs)
- Has complete test coverage (mutation score ≥ 95%)
- Does not introduce new dependencies

### RFC Process Steps

1. **Open a GitHub Issue** with the label `rfc` and the prefix `[RFC] Title`.
2. **Write the RFC document** in `docs/rfcs/RFC-NNNN-<kebab-title>.md` using the template below.
3. **Request review** by posting in the issue thread.
4. **Wait for the Discussion Period**: minimum 48 hours, no maximum.
5. **Collect votes** (see Voting Rules below).
6. **Implement** only after RFC reaches `Approved` state.
7. **Reference RFC in commit message**: `feat: implement rfc-0005 — GeoPoint value object`.

### RFC Document Template

```markdown
# RFC-NNNN: Short Title

> **Status:** Draft | Under Review | Approved | Rejected | Withdrawn
> **Authors:** Your Name
> **Created:** YYYY-MM-DD

## Problem Statement
## Decision
## Migration Guide
## Breaking Change Classification
## Risks and Mitigations
## Votes
```

---

## Voting Rules

- **Approval** requires at least **1 +1 vote** from a core committee member (current single-maintainer model).
- As committee grows: **3 +1 votes** required, **1 -1 vote constitutes a veto**.
- A **veto** (`-1`) must be accompanied by:
  - Technical justification referencing architectural principles, benchmark evidence, or statutory regulations.
  - Concrete alternative proposal or path to compromise.
- **Abstentions** (`0`) do not count toward quorum.
