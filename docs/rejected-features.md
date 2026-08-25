# Rejected Features & Architectural Discard Log

> **Formal Record of Evaluated and Rejected Architectural Proposals**

---

## 1. Rejected: Implicit Cast Operators to Primitives

- **Proposal:** Providing `public static implicit operator string(Email email)` or `implicit operator decimal(Money m)`.
- **Reason for Rejection:** Implicit conversions destroy type safety at compile time. Developers could accidentally pass an `Email` to any method accepting `string` or mix up parameter order silently. Explicit `.Value` access or explicit casts are required.
- **Reference:** ADR-011.

---

## 2. Rejected: Unified Single Universal Tax ID

- **Proposal:** Modeling a generic `TaxId` struct with a regex string instead of dedicated country satellites.
- **Reason for Rejection:** Different countries enforce completely different mathematical algorithms (Modulo 11, Modulo 10, prime-weighted factors, SHA-384 electronic signatures). A single regex cannot validate mathematical check digits and fails regulatory audits.
- **Reference:** ADR-005, REJ-001.

---

## 3. Rejected: Raw Decimal Tax Calculations

- **Proposal:** Allowing tax calculation methods to accept raw `decimal` rates instead of strongly typed `TaxRate`.
- **Reason for Rejection:** Prone to scale errors (e.g. passing `18` instead of `0.18`). Strongly typed `TaxRate` enforces valid 0.00%–100.00% bounds.
- **Reference:** REJ-002.

---

## 4. Rejected: Runtime Reflection in JSON & ORM Converters

- **Proposal:** Using `Activator.CreateInstance` and reflection emitters for generic persistence converters.
- **Reason for Rejection:** Reflection breaks NativeAOT compilation, induces trim warnings (`IL2026`, `IL3050`), and degrades high-throughput microservice startup latency.
- **Reference:** ADR-004.
