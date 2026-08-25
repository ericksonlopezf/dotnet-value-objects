# Security Policy

## Supported Versions

Security updates and patches are provided for the following versions:

| Version | Supported | Target Runtime | Notes |
|:---|:---:|:---|:---|
| **1.0.x** | :white_check_mark: | .NET 10.0 (`net10.0`) | Current active LTS target |
| **< 1.0.0** | :x: | — | Pre-release versions not supported |

---

## Reporting a Vulnerability

We take the security of `EricksonLopez.ValueObjects` seriously. If you discover a security vulnerability, please do **NOT** open a public GitHub issue.

### Disclosure Process

1. **Email Notification**: Send a detailed description of the vulnerability to [ericksonlopezf@gmail.com](mailto:ericksonlopezf@gmail.com).
2. **Details to Include**:
   - Component / package name and version affected.
   - Proof of Concept (PoC) or reproducible test case.
   - Potential impact and threat vector.
3. **Response Timeline**:
   - **Initial Response**: Within 48 hours acknowledging receipt.
   - **Assessment & Fix**: Security patches are prioritized and developed privately.
   - **Public Advisory**: A CVE or GitHub Security Advisory will be published once the patch is released.

---

## Supply Chain Security & Build Integrity

To ensure maximum supply chain integrity:

1. **Deterministic Builds**: Enabled via `<ContinuousIntegrationBuild>true</ContinuousIntegrationBuild>` in CI/CD.
2. **Source Link & Debug Symbols**: All packages embed untracked sources and include portable symbol packages (`.snupkg`) via SourceLink.
3. **Central Package Management (CPM)**: Dependencies are strictly pinned and managed centrally in `Directory.Packages.props` with `<CentralPackageTransitivePinningEnabled>true</CentralPackageTransitivePinningEnabled>`.
4. **Zero Warnings Enforcement**: Compiled with `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>` and `.NET Analyzers (AnalysisLevel=latest-recommended)`.

---

## Known Security Boundaries & Invariants

1. **Sensitive Data Protection (PII Defense)**:
   - Types containing Personally Identifiable Information (PII) or credentials (such as `PasswordHash`, `NationalId`, `PassportNumber`, `Cedula`, `Rnc`, `Cuit`, `Rut`) are decorated with `[SensitiveData]` and `[DebuggerDisplay]`.
   - `ToString()` automatically redacts the sensitive value (e.g. `******` or masked digits) to prevent accidental leakage into telemetry, log aggregators, and debugger visualizers.
2. **Fail-Closed Domain Validation**:
   - Value Objects enforce private constructors and static `Create` factories returning `Result<T>`. An unvalidated or malformed instance cannot be instantiated, preventing bypass attacks at runtime.
