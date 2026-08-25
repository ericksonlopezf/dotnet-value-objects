# Quality Gates & CI Enforcement

> **Comprehensive Quality Gate Matrix for Pull Requests & Releases**

---

## 1. Quality Gates Matrix

| Gate | Tool | Threshold | When Enforced |
|---|---|---|---|
| **Compilation** | `dotnet build` | 0 warnings (`TreatWarningsAsErrors`) | PR & Push |
| **Unit Tests** | `xUnit.v3` | 100% passing tests | PR & Push |
| **Line Coverage** | Codecov / Coverlet | ≥ 99% line coverage | PR & Push |
| **Code Smells / Quality** | SonarCloud | Quality Gate: Passed | PR & Push |
| **Mutation Score** | Stryker.NET | ≥ 95% break threshold | Weekly & Publish Gate |
| **NativeAOT Smoke Test** | `PublishAot=true` | 0 IL2026/IL3050 warnings | PR & Push |
| **Repo Compliance** | `verify-compliance.ps1` | 0 violations (kebab-case, headers, etc.) | PR & Push |
