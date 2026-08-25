# Testing Strategy & Quality Roadmap

---

## 1. Testing Topology

```mermaid
graph TD
    Unit[Unit Tests - Arithmetic & Validation] --> FiscalTests[Fiscal Checksum Verification]
    FiscalTests --> Arch[Architecture Tests - Boundaries]
    Arch --> AOT[NativeAOT Smoke Compilation]
    AOT --> Mutation[Stryker Mutation Quality Gate]
```

- **Unit Tests**: Arithmetic operator precision, currency safety, and address components.
- **Fiscal Tests**: Official modulo 11 / Luhn checksum validation for all supported countries.
- **AOT Smoke Tests**: Standalone native binary execution with `PublishAot=true`.
