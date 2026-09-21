# ADR-001: Adoption of Result Pattern over Exceptions

## Status
Accepted

## Date
2026-08-16

- **Status:** Accepted
- **Date:** 2026-08-16
- **Context:** Core Domain, Fiscal Parsing & High-Throughput Pipelines

## Context and Problem Statement

Parsing and instantiating Value Objects (`Rut`, `Cuit`, `Ruc`, `Nit`, `Rnc`, `Email`, `Money`, etc.) occurs intensively during batch data ingestion (ETL), payroll calculation, and electronic invoice stamping.

Using exceptions (`throw new FormatException(...)` or `ArgumentException`) as standard validation mechanisms severely degrades CPU performance due to stack unwinding, generates unnecessary Gen0/Gen1 GC allocations, and compromises **Native AOT** predictability in .NET 10.

## Decision

1. **Private Constructors:** All Value Objects must declare strictly private or protected constructors (enforced by Roslyn Analyzer `ELVO001`).
2. **Static Factory Methods:** Instantiation is performed exclusively via static factory methods `Create(...)` returning a monadic `Result<T>`.
3. **Zero Control-Flow Exceptions:** Throwing exceptions for invalid input or corrupted domain data is prohibited. Returns `Result<T>.Failure(Error.Validation(...))`.
4. **Framework Exception Interop:** Framework interface implementations (e.g. `ISpanParsable<T>.Parse`) throw `FormatException` solely to satisfy Microsoft's BCL contract, redirecting internally from `TryParse`.
5. **Arithmetic Operators vs. Monadic Methods Duality:** For arithmetic Value Objects (such as `Money`), instance methods (`.Add()`, `.Subtract()`) return `Result<T>` for defensive, railway-oriented pipelines. Operator overloads (`+`, `-`, `*`) are provided for natural mathematical expressions where invariants are assumed to hold; currency mismatches are treated as unrecoverable domain/programmer errors throwing `DomainException`.

## Consequences

- **Positive:** Zero allocations on validation failure paths, explicit control flow, 100% Native AOT compatibility, and predictable sub-microsecond execution.
- **Negative:** Callers must inspect `result.IsSuccess` before accessing `result.Value`.
