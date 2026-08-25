# Features Catalog & Specifications

---

## 1. Package Inventory & Core Types

### 1. `EricksonLopez.ValueObjects`
- `Money`: High-precision financial value object with currency validation.
- `Currency`: ISO 4217 currency codes.
- `Address`: Universal geographical address value object.
- `GeoCoordinate`: Spatial coordinates with Haversine distance calculations.
- `PhoneNumber`: E.164 international phone number format.

### 2. Fiscal Satellites
- `Fiscal.DominicanRepublic`: `Rnc`, `Cedula`, `Ncf`.
- `Fiscal.Chile`: `Rut`.
- `Fiscal.Colombia`: `Nit`.
- `Fiscal.Mexico`: `Rfc`, `Curp`.
- `Fiscal.Peru`: `Ruc`, `Dni`.
- `Fiscal.Argentina`: `Cuit`, `Cuil`.

### 3. Adapters & Generators
- `EntityFrameworkCore`: Complex properties and value converters.
- `Dapper`: Type handlers and parameter mappers.
- `Serialization.Json`: NativeAOT System.Text.Json converters.
- `Generators`: Incremental Roslyn source generator for `[ValueObject]`.
