# ADR-014: Standardized Testing Conventions with Osherove Pattern

- **Status:** Accepted
- **Date:** 2026-08-16
- **Context:** Test Architecture, Naming & Diagnostic Clarity

## Context and Problem Statement

Default C# naming rules (`IDE1006` and `CA1707`) mandate strict PascalCase for method names. In complex unit test suites describing units of work, preconditions, and expected business behaviors, PascalCase produces illegible method identifiers in Test Explorers and CI logs (e.g. `CreateWhenVerificationDigitIsInvalidReturnsError` vs. `Create_WhenVerificationDigitIsInvalid_ReturnsError`).

## Decision

1. **Osherove Tripartite Pattern:** Mandate `[UnitOfWork]_[StateUnderTest]_[ExpectedBehavior]` for all test methods in `tests/`.
2. **Targeted Analyzer Suppression:** Suppress `IDE1006` and `CA1707` specifically for `[tests/**/*.cs]` in `.editorconfig`.
3. **100% Mutation Score Requirement:** Tests must assert exact error codes and edge values to achieve 100% mutation score with Stryker.NET without artificial suppression directives.

## Consequences

- **Positive:** Maximum legibility in CI test reports and failure diagnostics.
- **Negative:** Suppressions must remain strictly confined to `tests/`.
