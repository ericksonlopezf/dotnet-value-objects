# Ecosystem Package Guide

> **Detailed Guide to all 13 Published NuGet Packages in `EricksonLopez.ValueObjects`**

---

## 1. Core Packages

### `EricksonLopez.ValueObjects`
- **Purpose:** Core domain primitives, scalar numeric and temporal structs, composite records, and base abstractions.
- **Dependencies:** `EricksonLopez.Result` (>= 2.0.0).

### `EricksonLopez.ValueObjects.DomainPrimitives`
- **Purpose:** Bridge to `EricksonLopez.DomainPrimitives.Abstractions` (`ToDomainPrimitive`, `ToStrongId`).

---

## 2. Fiscal Satellites

- `EricksonLopez.ValueObjects.Fiscal.DominicanRepublic`: DGII RNC, Cedula, e-CF.
- `EricksonLopez.ValueObjects.Fiscal.Chile`: SII RUT, DTE Folio.
- `EricksonLopez.ValueObjects.Fiscal.Colombia`: DIAN NIT, CUFE, CUDE, CUNE.
- `EricksonLopez.ValueObjects.Fiscal.Mexico`: SAT CFDI 4.0 RFC, CURP, Fiscal UUID.
- `EricksonLopez.ValueObjects.Fiscal.Peru`: SUNAT RUC, CPE Identifier.
- `EricksonLopez.ValueObjects.Fiscal.Argentina`: ARCA/AFIP CUIT, CUIL, CBU, CVU, CAE.

---

## 3. Persistence & Tooling

- `EricksonLopez.ValueObjects.EntityFrameworkCore`: EF Core 10 `ValueConverter` conventions and mappings.
- `EricksonLopez.ValueObjects.Dapper`: Micro-ORM `SqlMapper.TypeHandler` persistence adapters.
- `EricksonLopez.ValueObjects.Serialization.Json`: `System.Text.Json` converter factory with zero reflection.
- `EricksonLopez.ValueObjects.Analyzers`: Compile-time Roslyn diagnostic rules `ELVO001`–`ELVO003`.
- `EricksonLopez.ValueObjects.Generators`: Incremental source generator for `[ValueObject]`.
