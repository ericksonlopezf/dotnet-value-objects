# Comprehensive Testing Strategy & Quality Standards

> **Ecosystem:** `EricksonLopez.ValueObjects`
> **Standards:** .NET 10 · C# 13 · xUnit v3 · AwesomeAssertions · Stryker.NET (100% Mutation Score) · Native AOT

---

## 1. Quality Policy & Thresholds

Testing in `EricksonLopez.ValueObjects` follows the **FIRST** principles (Fast, Independent, Repeatable, Self-Validating, Timely):

| Quality Gate | Requirement | Tooling / Implementation |
|---|:---:|---|
| **Line Coverage** | **100.0%** | Coverlet XPlat DataCollector (`coverlet.runsettings`) |
| **Branch Coverage** | **100.0%** | Coverlet XPlat DataCollector |
| **Method Coverage** | **100.0%** | Coverlet XPlat DataCollector |
| **Mutation Score** | **100.0%** | Stryker.NET (`stryker-config.json`, 0 surviving mutants) |
| **Compiler Warnings** | **0** | `TreatWarningsAsErrors=true`, `.NET 10 Analyzers` |
| **Code Style** | **Zero violations** | `dotnet format --verify-no-changes` (`.editorconfig`) |
| **Native AOT** | **100% Pass** | Smoke test runner (`EricksonLopez.ValueObjects.NativeAotTests`) |

---

## 2. Test Suite Composition (15 Projects, 1,507+ Tests)

```
tests/
├── EricksonLopez.ValueObjects.UnitTests                           (529 tests)
├── EricksonLopez.ValueObjects.Analyzers.UnitTests                 (33 tests)
├── EricksonLopez.ValueObjects.Generators.UnitTests                (32 tests)
├── EricksonLopez.ValueObjects.DomainPrimitives.UnitTests          (14 tests)
├── EricksonLopez.ValueObjects.Dapper.IntegrationTests             (28 tests)
├── EricksonLopez.ValueObjects.EntityFrameworkCore.IntegrationTests (39 tests)
├── EricksonLopez.ValueObjects.Serialization.Json.IntegrationTests  (27 tests)
├── EricksonLopez.ValueObjects.Fiscal.Argentina.UnitTests          (225 tests)
├── EricksonLopez.ValueObjects.Fiscal.Chile.UnitTests              (171 tests)
├── EricksonLopez.ValueObjects.Fiscal.Colombia.UnitTests           (149 tests)
├── EricksonLopez.ValueObjects.Fiscal.DominicanRepublic.UnitTests  (147 tests)
├── EricksonLopez.ValueObjects.Fiscal.Mexico.UnitTests             (147 tests)
├── EricksonLopez.ValueObjects.Fiscal.Peru.UnitTests               (142 tests)
├── EricksonLopez.ValueObjects.NativeAotTests                      (31 smoke tests)
└── Common/                                                         (Shared ValueObjectContractExtensions)
```

---

## 3. Test Method Naming (Osherove Pattern)

All unit and integration test methods must strictly follow the **Osherove Tripartite Pattern** (ADR-014):

```
[UnitOfWork]_[StateUnderTest]_[ExpectedBehavior]
```

### Examples:
- `Create_WhenRawStringIsValid_ReturnsSuccess()`
- `Create_WhenEmailContainsWhitespace_NormalizesAndReturnsSuccess()`
- `Create_WhenVerificationDigitIsInvalid_ReturnsValidationError()`
- `Allocate_WhenSummingRatios_LeavesZeroRemainder()`

> **IDE Enforcement**: `IDE1006` (naming style) and `CA1707` (underscores in identifiers) are suppressed for the `tests/` directory via `.editorconfig` to permit the Osherove underscore pattern.

---

## 4. Five-Axis Value Object Verification Standard

Every Value Object added to the repository must have test coverage across 5 distinct axes:

1. **Factory Success**: Valid inputs construct the object, normalize formatting, and assert `IsSuccess == true`.
2. **Factory Failure**: `null`, empty, whitespace-only, out-of-range, and malformed inputs return `Result<T>.Failure` with the expected `Error.Code`.
3. **Value Equality & HashCode**: Identical logical values produce equal instances (`==`, `Equals()`, `GetHashCode()`).
4. **Boundary Invariants**: Precision, minimum/maximum lengths, edge dates, and range limit checks.
5. **Business Operations**: Proportional allocation, arithmetic, interval intersections, and domain rules.

---

## 5. Test Execution Commands

```bash
# Run all tests across the entire solution (with coverage collection)
dotnet test EricksonLopez.ValueObjects.slnx --configuration Release --no-build --nologo --collect:"XPlat Code Coverage"

# Run all tests using PowerShell orchestration script
./test.ps1

# Run all tests using Bash orchestration script
./test.sh

# Run Native AOT Smoke Tests
dotnet run --project tests/EricksonLopez.ValueObjects.NativeAotTests/EricksonLopez.ValueObjects.NativeAotTests.csproj --configuration Release --no-build

# Run Mutation Testing with Stryker (main/nightly gate)
dotnet tool install -g dotnet-stryker
dotnet stryker --config-file stryker-config.json
```

---

## 6. Integration Test Dependencies

Integration test projects (`Dapper`, `EntityFrameworkCore`, `Json`) require their respective frameworks and verify end-to-end persistence roundtrips:

| Test Project | Framework Under Test | Infrastructure Required |
|---|---|---|
| `Dapper.IntegrationTests` | Dapper `SqlMapper.TypeHandler` | SQLite (in-memory via Dapper) |
| `EntityFrameworkCore.IntegrationTests` | EF Core `ValueConverter` | SQLite (in-memory via EF Core) |
| `Serialization.Json.IntegrationTests` | `System.Text.Json` converters | None (in-process) |
