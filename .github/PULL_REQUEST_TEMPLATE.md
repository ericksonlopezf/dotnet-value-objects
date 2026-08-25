## Description
Briefly describe the changes introduced by this PR.

## Packages Affected
Please check the packages that are affected by this change:
- [ ] `EricksonLopez.ValueObjects`
- [ ] `EricksonLopez.ValueObjects.Fiscal.DominicanRepublic`
- [ ] `EricksonLopez.ValueObjects.Fiscal.Chile`
- [ ] `EricksonLopez.ValueObjects.Fiscal.Colombia`
- [ ] `EricksonLopez.ValueObjects.Fiscal.Mexico`
- [ ] `EricksonLopez.ValueObjects.Fiscal.Peru`
- [ ] `EricksonLopez.ValueObjects.Fiscal.Argentina`
- [ ] `EricksonLopez.ValueObjects.EntityFrameworkCore`
- [ ] `EricksonLopez.ValueObjects.Dapper`
- [ ] `EricksonLopez.ValueObjects.Serialization.Json`
- [ ] `EricksonLopez.ValueObjects.DomainPrimitives`
- [ ] `EricksonLopez.ValueObjects.Analyzers`
- [ ] `EricksonLopez.ValueObjects.Generators`

## Type of Change
- [ ] 🐛 Bug fix (non-breaking change which fixes an issue)
- [ ] ✨ New feature (non-breaking change which adds functionality)
- [ ] 💥 Breaking change (fix or feature that would cause existing functionality to not work as expected)
- [ ] 📖 Documentation update
- [ ] ⚡ Performance improvement
- [ ] 🎨 Code style / Refactoring

## Performance & Allocation
- [ ] This PR does not introduce any new heap allocations on the hot paths (`TryCreate`, `TryParse`, arithmetic, formatting).
- [ ] If changing core numeric, financial, or temporal types, I have verified benchmark results using `BenchmarkDotNet` and MemoryDiagnoser.

## Quality Gates Checklist
- [ ] My code follows the code style of this project (`WarningLevel 5`, `TreatWarningsAsErrors`).
- [ ] I have updated the documentation accordingly (`README.md`, `/docs/*.md`, XML comments).
- [ ] I have added unit / integration tests to cover my changes.
- [ ] All new and existing tests passed (`dotnet test --collect:"XPlat Code Coverage"`).
- [ ] Codecov and SonarQube analyses pass without dropping thresholds.
- [ ] Stryker mutation testing passes (break threshold: 95%).
- [ ] Native AOT compatibility verified (no IL3050/IL2026 warnings).
- [ ] I have updated `CHANGELOG.md` under `[Unreleased]`.
- [ ] My commit messages follow [Conventional Commits](https://www.conventionalcommits.org/).

## Related Issues
Fixes # (issue)
