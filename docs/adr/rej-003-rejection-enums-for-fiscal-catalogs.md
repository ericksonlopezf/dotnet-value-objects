# REJ-003: Rejection of Closed Enums for Government Catalogs

- **Status:** Rejected
- **Date:** 2026-08-16
- **Original Proposal:** Declare government catalogs (SAT CFDI products, SUNAT catalogs, DIAN tax codes) as static C# `enum` types.

## Rationale for Rejection

1. **Temporal Validity Dimension:** Government catalogs have dynamic effective dates (`ValidFrom`, `ValidTo`) and frequent administrative additions.
2. **Open-Closed Principle (OCP) Violation:** Adding a single code would require publishing a new package version and forcing client redeployments.
3. **Alternative Decision:** Model morphological Value Objects for pattern/length validation and verify code validity dynamically via application services.
