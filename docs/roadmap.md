# Project Roadmap

> **Strategic & Technical Roadmap for `EricksonLopez.ValueObjects`**

---

## 1. Milestone v1.0.0 (Released 2026-08-24)
- [x] Complete core `Money` implementation with Martin Fowler's proportional allocation.
- [x] Core geographical, contact, temporal, and numeric value objects.
- [x] 6 official Latin American fiscal satellites (DO, CL, CO, MX, PE, AR).
- [x] Zero-reflection persistence converters for EF Core 10, Dapper, and System.Text.Json.
- [x] Roslyn Diagnostic Analyzers `ELVO001`, `ELVO002`, `ELVO003`.
- [x] Incremental source generator for `[ValueObject]`.
- [x] 100% NativeAOT trimming validation.
- [x] 100% Stryker mutation testing score.

---

## 2. Milestone v2.0.0 (Released 2026-09-20)
- [x] Zero-allocation `TimeRange` converted from reference record to `readonly record struct`.
- [x] Enforce hard 20,000 character limit on `StringValueObject` base constructor (DoS/LOH defense).
- [x] Mandatory canonical Unicode Normalization FormC across string pipelines.
- [x] Stricter RFC 5321 email validation and ASCII digit normalization for phone numbers.
- [x] Safe uninitialized struct guards on `Money.Zero`, arithmetic, and comparison operators.
- [x] Major ecosystem alignment with `EricksonLopez.DomainPrimitives.Abstractions` v2.0.0.
- [x] Comprehensive zero-tolerance architectural enforcement and Stryker CI/CD optimization.

---

## 3. Milestone v2.1.0 (Planned)
- [ ] Brazil (CNPJ/CPF, NF-e) fiscal satellite.
- [ ] Uruguay (RUT) and Ecuador (RUC) fiscal satellites.
- [ ] OpenAPI 3.1 Swagger schema generator integration package.
