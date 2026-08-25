# Testing Strategy & Verification Architecture

> **Comprehensive Testing Methodology across 14 Test Projects**

---

## 1. Test Architecture & Conventions

- **Unit Testing**: Testing domain invariants, factory validation, and mathematical edge cases using `xUnit.v3` and `AwesomeAssertions`.
- **Property-Based Testing**: `FsCheck.Xunit` verifying mathematical identities (e.g. `(a + b) - b == a` on `Money`).
- **Osherove Naming Pattern**: Test methods use `MethodName_StateUnderTest_ExpectedBehavior` (enforced via `.editorconfig` allowance in `tests/`).
- **Integration Testing**: Testing EF Core SQLite / InMemory converters and Dapper type handlers.
- **Mutation Testing**: Stryker.NET mutation coverage enforcing a ≥ 95% break threshold.
