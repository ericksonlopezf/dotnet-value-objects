# Feature Specifications & Taxonomy

> **Comprehensive Feature Taxonomy of `EricksonLopez.ValueObjects`**

---

## 1. Core Financial & Temporal Modeling
- `Money`: 128-bit decimal precision, ISO 4217 CurrencyCode, Fowler allocation, banker's/commercial rounding.
- `CurrencyCode`: ISO 4217 uppercase 3-letter codes with zero table allocation.
- `TaxRate` / `DiscountRate` / `Percentage`: Strongly bounded rate calculations (0% to 100%).
- `Range<T>`: Generic inclusive interval `[Start .. End]` with `Contains`, `Overlaps`, and `Intersect`.
- `BusinessDate`: Accounting date wrapper with format/parse operations.

## 2. Multi-Country Fiscal Satellites
- **Dominican Republic**: RNC, Cedula, NCF, e-CF (E31, E32, etc.).
- **Chile**: RUT with Modulo 11 check digit 'K', DTE Folio.
- **Colombia**: NIT with verification digit, CUFE/CUDE/CUNE SHA-384 signatures.
- **Mexico**: RFC with homoclave, CURP, CFDI 4.0 UUID.
- **Peru**: SUNAT RUC, CPE Identifier.
- **Argentina**: CUIT, CUIL, CBU, CVU, CAE.

## 3. Tooling & Enforcement
- Roslyn Analyzers `ELVO001`, `ELVO002`, `ELVO003`.
- Incremental Source Generators for `[ValueObject]` types.
