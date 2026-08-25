# Quality Audit & Compiler Diagnostics

> **Strict Compiler Configuration, Analyzers & Zero-Tolerance Quality Standards**

---

## 1. Compiler Configuration

- `<WarningLevel>5</WarningLevel>`: Enforces latest compiler warnings.
- `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>`: Hard build failure on any warning.
- `<AnalysisLevel>latest-recommended</AnalysisLevel>`: Highest Roslyn analyzer inspection level.
- `<EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>`: Enforces `.editorconfig` formatting on build.
- `<Nullable>enable</Nullable>`: Complete null-safety enforcement.

---

## 2. Roslyn Custom Diagnostics
- `ELVO001`: Private/protected constructors.
- `ELVO002`: Static `Create` factory returning `Result`.
- `ELVO003`: Absolute immutability.
