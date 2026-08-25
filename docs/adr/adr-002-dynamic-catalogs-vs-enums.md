# ADR-002: Fiscal Catalogs as Dynamic Value Objects, NOT Enums

- **Status:** Accepted
- **Date:** 2026-08-16
- **Context:** Fiscal Jurisdictions (SAT, SUNAT, DIAN, DGII, SII, ARCA)

## Context and Problem Statement

Government tax authorities frequently publish and update catalogs (e.g., SAT CFDI 4.0 use codes, SUNAT product classification, DANE municipality codes). Modeling these regulatory catalogs as C# `enum` types creates brittle tight coupling: every tax resolution would require a library release and force cascading redeployments across microservices.

## Decision

1. **Structural Morphological Value Objects:** Model government catalog items as structural single-value Value Objects (e.g. `TaxRegimeCode`, `CfdiUsageCode`, `CpeTypeCode`, `DaneMunicipalityCode`) that validate length, regex patterns, and morphological structure at compile/instantiation time.
2. **Dynamic Validation:** Existence and temporal validity intervals (`ValidFrom` / `ValidTo`) are resolved dynamically against cached database lookup services in the Application or Infrastructure layers.

## Consequences

- **Positive:** High extensibility without breaking binary compatibility; aligns with the Open-Closed Principle (OCP).
- **Negative:** Deep semantic validation of whether a code was legally valid on an exact historical date requires external service collaboration.
