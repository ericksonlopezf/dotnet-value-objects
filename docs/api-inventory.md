# Public API Inventory

> **Complete Catalog of Types, Structs, and Interfaces across all 13 NuGet Packages**

---

## 1. Core Abstractions & Primitives (`EricksonLopez.ValueObjects`)

| Type | Kind | Namespace | Description |
|---|---|---|---|
| `IValueObject` | Interface | `EricksonLopez.ValueObjects` | Root marker interface for all value objects |
| `IValueObject<TSelf>` | Interface | `EricksonLopez.ValueObjects` | Strongly typed contract for equatable value objects |
| `ValueObject` | Abstract Record Class | `EricksonLopez.ValueObjects` | Base record for multi-component composite objects |
| `SingleValueObject<TSelf, TValue>` | Abstract Record Class | `EricksonLopez.ValueObjects` | Base record for single-value wrappers with explicit casts |
| `StringValueObject<TSelf>` | Abstract Record Class | `EricksonLopez.ValueObjects` | Base record for sanitized and validated string types |
| `Money` | `readonly record struct` | `EricksonLopez.ValueObjects` | High-precision monetary amount + ISO 4217 CurrencyCode |
| `CurrencyCode` | `readonly record struct` | `EricksonLopez.ValueObjects` | ISO 4217 3-letter currency identifier |
| `Percentage` | `readonly record struct` | `EricksonLopez.ValueObjects` | 0.00% to 100.00% fractional value |
| `TaxRate` | `readonly record struct` | `EricksonLopez.ValueObjects` | Statutory tax rate calculation struct |
| `DiscountRate` | `readonly record struct` | `EricksonLopez.ValueObjects` | Commercial discount rate struct |
| `ExchangeRate` | `readonly record struct` | `EricksonLopez.ValueObjects` | Currency conversion rate pair |
| `Range<T>` | `readonly record struct` | `EricksonLopez.ValueObjects` | Generic inclusive interval `[Start .. End]` |
| `BusinessDate` | `readonly record struct` | `EricksonLopez.ValueObjects` | Commercial accounting date based on `DateOnly` |
| `TimeRange` | Composite Record | `EricksonLopez.ValueObjects` | Time interval supporting overnight shifts |
| `Address` | Composite Record | `EricksonLopez.ValueObjects` | Normalized physical postal address |
| `FullName` | Composite Record | `EricksonLopez.ValueObjects` | Structured person name (First, Last, Middle) |
| `Email` | `readonly record struct` | `EricksonLopez.ValueObjects` | Validated email address with PII masking |
| `PhoneNumber` | `readonly record struct` | `EricksonLopez.ValueObjects` | E.164 international phone number |
| `Country` | `readonly record struct` | `EricksonLopez.ValueObjects` | ISO 3166-1 alpha-2 country code |
| `PostalCode` | `readonly record struct` | `EricksonLopez.ValueObjects` | Alphanumeric postal code |
| `SensitiveDataAttribute` | Attribute | `EricksonLopez.ValueObjects` | PII masking attribute for logs and ToString() |
| `ValueObjectAttribute` | Attribute | `EricksonLopez.ValueObjects` | Marker attribute activating incremental generators |

---

## 2. Fiscal Satellites

### Dominican Republic (`Fiscal.DominicanRepublic`)
- `Rnc`: 9-digit corporate tax ID (Modulo 11).
- `Cedula`: 11-digit personal ID (Modulo 10 / Luhn).
- `Ncf`: Traditional government invoice number.
- `ElectronicNcf`: e-CF electronic invoice identifier (E31, E32, etc.).

### Chile (`Fiscal.Chile`)
- `Rut`: National tax ID with check digit 'K' (Modulo 11).
- `FiscalFolio`: SII electronic document correlative number.
- `DteTypeCode`: Document type code (Factura, Boleta, Guía).

### Colombia (`Fiscal.Colombia`)
- `Nit`: Tax identification number with verification digit.
- `Cufe` / `Cude` / `Cune`: SHA-384 electronic invoice cryptographic signatures.

### Mexico (`Fiscal.Mexico`)
- `Rfc`: Natural and moral person tax ID with homoclave.
- `Curp`: Population registry code.
- `FiscalUuid`: CFDI 4.0 electronic UUID.

### Peru (`Fiscal.Peru`)
- `Ruc`: SUNAT 11-digit tax number.
- `CpeIdentifier`: Electronic payment proof series and correlative.

### Argentina (`Fiscal.Argentina`)
- `Cuit` / `Cuil`: ARCA/AFIP 11-digit tax ID.
- `Cbu` / `Cvu`: 22-digit banking/virtual account keys.
- `Cae`: Electronic authorization code.

---

## 3. Persistence & Tooling Packages
- `EricksonLopez.ValueObjects.EntityFrameworkCore`: `ConfigureDomainValueObjects()`, `ValueObjectConverter<T>`.
- `EricksonLopez.ValueObjects.Dapper`: `DapperValueObjectRegistry.RegisterAll()`.
- `EricksonLopez.ValueObjects.Serialization.Json`: `ValueObjectJsonConverterFactory`.
- `EricksonLopez.ValueObjects.Analyzers`: Rules `ELVO001`, `ELVO002`, `ELVO003`.
- `EricksonLopez.ValueObjects.Generators`: Incremental generator for `[ValueObject]`.
