# RFC-0000: Template for RFCs

> **Status:** Draft | Under Review | Approved | Rejected | Withdrawn  
> **Authors:** Your Name (<your.email@example.com>)  
> **Created:** YYYY-MM-DD  
> **Target Release:** vX.Y.Z  

---

## 1. Summary

A concise 2–3 paragraph summary of what is being proposed and why.

---

## 2. Problem Statement & Motivation

- What problem does this solve?
- Why is the current architecture insufficient?
- What are the real-world use cases?

---

## 3. Detailed Proposal & Specification

### API Design
```csharp
public readonly record struct ProposedType
{
    // Signature
}
```

### Memory & Performance Impact
- Expected heap allocations (must be 0 B on hot paths).
- Benchmark considerations.

### NativeAOT & Trimming Impact
- Reflection-free guarantees.

---

## 4. Breaking Changes & Migration Path

- Is this a binary, source, or behavioral breaking change?
- What steps must existing consumers take?

---

## 5. Alternatives Considered & Rejected

- What alternative solutions were evaluated and why were they discarded?

---

## 6. Decision & Votes

- **Lead Maintainer:** [+1 | 0 | -1]
- **Core Committee:** [+1 | 0 | -1]
