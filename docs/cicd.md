# CI/CD Workflows Summary

> **Summary Index of Continuous Integration & Deployment Workflows**

---

## 1. Workflows Summary

- [`ci.yml`](https://github.com/ericksonlopezf/dotnet-value-objects/blob/main/.github/workflows/ci.yml): Main CI pipeline.
- [`dotnet-build-test.yml`](https://github.com/ericksonlopezf/dotnet-value-objects/blob/main/.github/workflows/dotnet-build-test.yml): Build, test, coverage & Sonar scanner.
- [`aot-smoke-test.yml`](https://github.com/ericksonlopezf/dotnet-value-objects/blob/main/.github/workflows/aot-smoke-test.yml): NativeAOT validation.
- [`benchmarks.yml`](https://github.com/ericksonlopezf/dotnet-value-objects/blob/main/.github/workflows/benchmarks.yml): Benchmark baseline capture.
- [`weekly-benchmarks.yml`](https://github.com/ericksonlopezf/dotnet-value-objects/blob/main/.github/workflows/weekly-benchmarks.yml): Deep weekly performance review.
- [`mutation-testing.yml`](https://github.com/ericksonlopezf/dotnet-value-objects/blob/main/.github/workflows/mutation-testing.yml): Stryker mutation gate.
- [`publish.yml`](https://github.com/ericksonlopezf/dotnet-value-objects/blob/main/.github/workflows/publish.yml): Sigstore provenance and NuGet OIDC publishing.
- [`release-please.yml`](https://github.com/ericksonlopezf/dotnet-value-objects/blob/main/.github/workflows/release-please.yml): Conventional commit automation.
- [`repo-compliance.yml`](https://github.com/ericksonlopezf/dotnet-value-objects/blob/main/.github/workflows/repo-compliance.yml): Architecture & rules compliance check.
