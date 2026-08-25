# Public API Surface & Compatibility Policy

> **Public API Contracts, Versioning Guarantees & Breaking Change Protocol**

---

## 1. Semantic Versioning Policy (SemVer 2.0.0)

- **MAJOR (X.0.0)**: Breaking changes in public API signatures or invariant semantics.
- **MINOR (1.X.0)**: Backward-compatible additions (new Value Objects, new fiscal types, new helper methods).
- **PATCH (1.0.X)**: Backward-compatible bug fixes and performance optimizations.

---

## 2. API Surface Verification

The repository utilizes `Microsoft.CodeAnalysis.PublicApiAnalyzers` to track all exported public types and members. No public member may be altered or removed without triggering build diagnostics and requiring a formal RFC.
