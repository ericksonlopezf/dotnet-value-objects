# Contributing to EricksonLopez.ValueObjects

Thank you for your interest in contributing to `EricksonLopez.ValueObjects`! This document details the guidelines, build commands, testing procedures, and architectural constraints required for all contributions.

---

## 1. Code of Conduct

All contributors are expected to adhere to our [Code of Conduct](CODE_OF_CONDUCT.md). Please read it before participating.

---

## 2. Architectural Principles & Invariants (Non-Negotiable)

1. **Absolute Immutability**: Every Value Object must be immutable (`readonly record struct` for numeric/scalar structs, `sealed record : StringValueObject<TSelf>` / `SingleValueObject<TSelf, TValue>` for classes). Public setters or mutable fields are strictly prohibited (enforced by Roslyn Analyzer `ELVO003`).
2. **Result-over-Exceptions**: Instantiation must occur exclusively through static factory methods `Create(...)` returning `Result<T>`. Throwing exceptions for standard business control flow is forbidden.
3. **Domain Purity**: Core domain assemblies (`EricksonLopez.ValueObjects` and `EricksonLopez.ValueObjects.Fiscal.*`) must remain pure domain libraries with zero dependencies on persistence (Dapper, EF Core), web frameworks (ASP.NET Core), or serialization engines.
4. **Single Type per File**: Each file must contain exactly one public type matching the file name.
5. **XML Documentation**: All public types, properties, and methods must have comprehensive XML documentation (`<summary>`, `<param>`, `<returns>`, `<example>`, `<exception>`).
6. **Zero Warnings & 100% Quality Gates**: All builds must compile cleanly with 0 warnings (`TreatWarningsAsErrors=true`), 100% test coverage, and 100% mutation score.

---

## 3. Development Prerequisites

- **.NET 10 SDK** (Version `10.0.x` or later)
- **IDE**: Visual Studio 2026 / Visual Studio 2022 (v17.13+), JetBrains Rider 2024.3+, or Visual Studio Code with the C# Dev Kit extension.
- **Git**

---

## 4. Local Build & Test Commands

### 4.1 Verify Formatting

```bash
# Verify code style against .editorconfig (enforced in CI)
dotnet format --verify-no-changes --verbosity normal
```

### 4.2 Restore and Build

```bash
# Restore solution dependencies
dotnet restore EricksonLopez.ValueObjects.slnx

# Build Release with zero warnings enforcement
dotnet build EricksonLopez.ValueObjects.slnx --configuration Release --no-restore /warnaserror /p:ContinuousIntegrationBuild=true
```

### 4.3 Running Tests

```bash
# Run entire test suite (with coverage collection)
dotnet test EricksonLopez.ValueObjects.slnx --configuration Release --no-build --nologo --collect:"XPlat Code Coverage"

# Run with test orchestration script (PowerShell)
./test.ps1

# Run with test orchestration script (Bash)
./test.sh
```

### 4.4 NativeAOT Smoke Tests

```bash
dotnet run --project tests/EricksonLopez.ValueObjects.NativeAotTests/EricksonLopez.ValueObjects.NativeAotTests.csproj --configuration Release --no-build
```

### 4.5 Mutation Testing with Stryker

```bash
# Install Stryker tool globally if needed
dotnet tool install -g dotnet-stryker

# Execute Stryker mutation testing
dotnet stryker --config-file stryker-config.json
```

### 4.6 Running Benchmarks

```bash
dotnet run --project benchmarks/EricksonLopez.ValueObjects.Benchmarks/EricksonLopez.ValueObjects.Benchmarks.csproj --configuration Release
```

---

## 5. Testing Conventions

Test methods must strictly follow the **Osherove Tripartite Pattern** (deliberately permitted via `.editorconfig` rule suppression for `IDE1006` and `CA1707` in `tests/`):

```csharp
[Fact]
public void MethodName_Scenario_ExpectedBehavior()
{
    // Arrange
    var rawInput = "user@example.com";

    // Act
    var result = Email.Create(rawInput);

    // Assert
    result.IsSuccess.Should().BeTrue();
    result.Value.Value.Should().Be("user@example.com");
}
```

---

## 6. Branching Strategy & Git Workflow

- **`main`**: Production branch. Contains stable, releasable code.
- **`develop`**: Integration branch for upcoming features and active stabilization.
- **Topic Branches**: Create feature or bugfix branches off `develop` using the following naming conventions:
  - `feature/<short-description>`
  - `fix/<short-description>`
  - `refactor/<short-description>`
  - `docs/<short-description>`

---

## 7. Pull Request Checklist

Before submitting a Pull Request, verify the following:

- [ ] Solution compiles with zero warnings (`dotnet build ... /warnaserror`).
- [ ] All 1,507+ unit and integration tests pass (`dotnet test`).
- [ ] NativeAOT smoke tests pass cleanly.
- [ ] Stryker mutation testing maintains the 100% threshold.
- [ ] All new types have complete XML documentation.
- [ ] PR follows the structure in `.github/PULL_REQUEST_TEMPLATE.md`.
