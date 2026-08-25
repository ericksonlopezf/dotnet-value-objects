# Security Policy & Threat Model

> **Enterprise Security Invariants, PII Protection, and Supply Chain Integrity**

---

## 1. Threat Model & Invariants

### 1.1 Personally Identifiable Information (PII) Protection
- **Vulnerability:** Sensitive customer contact data (emails, telephone numbers, national IDs) leaking into plain-text application logs, telemetry, and unhandled exception traces.
- **Mitigation:** Sensitive types are decorated with `[SensitiveData]`. The default `ToString()` implementation automatically masks all but the first and last characters (e.g. `e***z@enterprise.com`, `+1809***1234`). Raw values are accessible only via explicit property access (`.Value`).

### 1.2 Arithmetic Overflow & Inexact Currency Precision
- **Vulnerability:** Fractional cent truncation causing financial loss and reconciliation discrepancies across distributed ledgers.
- **Mitigation:** `Money` operations use high-precision 128-bit `decimal` representations (28–29 significant digits). Fractional distribution uses Martin Fowler's proportional allocation algorithm to ensure exact penny conservation.

---

## 2. Supply Chain Security

1. **Strong Name Key Signing (`.snk`)**: All assemblies are cryptographically signed with an official strong name key.
2. **Sigstore Keyless Provenance Attestation**: Every `.nupkg` published via GitHub Actions receives a cryptographically signed build provenance attestation via OpenID Connect (OIDC).
3. **NuGet Trusted Publishing**: Publishing to NuGet.org is authorized strictly through short-lived GitHub OIDC tokens without long-lived static API secrets.
4. **Deterministic Builds**: Enabled across all projects (`<Deterministic>true</Deterministic>`) to guarantee reproducible binary output from source.
