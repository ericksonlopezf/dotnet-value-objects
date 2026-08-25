# REJ-001: Rejection of Unified GlobalTaxId Abstraction

- **Status:** Rejected
- **Date:** 2026-08-16
- **Original Proposal:** Create an `ITaxId` interface or abstract base class `GlobalTaxId(string Value, CountryCode Country)` to unify all global tax identification numbers.

## Rationale for Rejection

1. **Avoid Over-Abstraction:** A Chilean `Rut` (Modulo 11 with check digit 'K') shares no formatting or mathematical behavior with a Mexican `Rfc` (12-13 alphanumeric characters with homoclave) or an Argentine `Cuit`.
2. **Leaky Abstractions:** A single global base class would force optional properties and meaningless generic methods.
3. **Alternative Decision:** Each country models its own strongly typed Value Object (`Rut`, `Rfc`, `Cuit`, `Nit`, `Rnc`, `Ruc`). Multi-country integrations use explicit composition.
