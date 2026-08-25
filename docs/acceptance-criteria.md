# Acceptance Criteria & Invariant Verification

> **Ecosystem:** `EricksonLopez.ValueObjects`  
> **Engineering Specification:** v4.0 §10 · Target: .NET 10 / C# 14  
> **Audit Status:** Verified against 1,687 automated test cases

---

## 1. Domain Modeling Acceptance Criteria

### AC-01: Zero Allocation for Scalar Primitives
- **Requirement:** Creation, arithmetic, equality comparison, and span parsing of scalar numeric, financial, and temporal types (`Money`, `CurrencyCode`, `Percentage`, `TaxRate`, `DiscountRate`, `Range<T>`, `BusinessDate`) must allocate **0 bytes** on the managed GC heap.
- **Verification:** Unit tests measuring `GC.GetAllocatedBytesForCurrentThread()` before and after operations assert delta == 0.

### AC-02: Strict Currency Invariants on Financial Arithmetic
- **Requirement:** Adding, subtracting, or comparing `Money` instances across differing ISO 4217 currencies without explicit `ExchangeRate` conversion must fail at the domain boundary.
- **Verification:** Method calls return `Result.Failure` with error code `Money.CurrencyMismatch`. Operator overloads throw `CurrencyMismatchException`.

### AC-03: Martin Fowler Lossless Money Distribution
- **Requirement:** `Money.Allocate(ratios)` and `Money.Distribute(parts)` must conserve 100% of the total amount. Remainder cents resulting from fractional divisions must be assigned to base shares one-by-one to prevent cent loss.
- **Verification:** Sum of returned elements strictly equals original amount across all ratio combinations.

### AC-04: Statutory Accuracy of Fiscal Satellites
- **Requirement:** Country tax identifiers (DGII RNC, Cedula; SII RUT; DIAN NIT; SAT RFC, CURP; SUNAT RUC; ARCA CUIT, CUIL) must strictly implement official government checksum algorithms (Modulo 11, Modulo 10, Luhn, SHA-384).
- **Verification:** 100% test pass rate on official government test vectors.

---

## 2. Infrastructure & Tooling Acceptance Criteria

### AC-05: Roslyn Compile-Time Enforcement
- **Requirement:** Any consumer class declaring a `public` constructor on a Value Object or mutable `set;` property must be rejected by Roslyn analyzers (`ELVO001`, `ELVO002`, `ELVO003`) as a hard compile-time error.
- **Verification:** Roslyn analyzer unit tests with code fix assertions.

### AC-06: 100% NativeAOT & Trimming Safety
- **Requirement:** Zero usage of `System.Reflection`, `MakeGenericType`, or dynamic code emission in production libraries.
- **Verification:** NativeAOT Smoke Test workflow publishes executable binary with `-p:PublishAot=true` and `-p:TreatWarningsAsErrors=true`.
