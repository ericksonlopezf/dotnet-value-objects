# Planning & Architectural Risk Register

> **Risk Assessment & Mitigation Matrix for `EricksonLopez.ValueObjects`**

---

## 1. Risk Evaluation Matrix

| Risk ID | Description | Impact | Probability | Mitigation Strategy |
|---|---|:---:|:---:|---|
| **RSK-01** | Statutory Tax Changes in LATAM Jurisdictions | High | High | Country rules isolated in dedicated satellites; independent SemVer releases per country. |
| **RSK-02** | Breaking Changes during Trimming & NativeAOT | High | Low | Automated `aot-smoke-test.yml` running on every push/PR with `TreatWarningsAsErrors`. |
| **RSK-03** | Currency Invariant Bypass via Raw Struct `default` | Medium | Low | Roslyn analyzer `ELVO001` flags uninitialized creation; struct methods check for uninitialized states. |
| **RSK-04** | Mutation Regression in Fiscal Checksum Algorithms | High | Low | Stryker.NET mutation testing gate enforced on `main` (100% mutation score threshold). |
