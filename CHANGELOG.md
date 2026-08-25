# Changelog

All notable changes to the `EricksonLopez.ValueObjects` ecosystem will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [Unreleased]

---

## [1.0.0] - 2026-08-24

### Added
- **Core Domain (`EricksonLopez.ValueObjects`)**:
  - Pure domain Value Object framework targeting .NET 10 (C# 13).
  - Base abstractions: `IValueObject`, `IValueObject<T>`, `ValueObject` (record class inheriting from `EricksonLopez.DomainPrimitives.ValueObject`), `SingleValueObject<TSelf, TValue>`, and `StringValueObject<TSelf>`.
  - Zero-allocation continuous interval type `Range<T>` as `readonly record struct` with intersection, overlap, and boundary evaluation logic.
  - Multi-currency monetary arithmetic `Money` (`readonly record struct`) supporting Martin Fowler's proportional allocation algorithm and currency match validation.
  - Centralized text sanitation and normalization engine `StringPipeline` (whitespace collapsing, uppercase/lowercase, custom regex validation).
  - 60+ universal corporate value objects:
    - *Identity & Personal*: `FirstName`, `MiddleName`, `LastName`, `FullName`, `DisplayName`, `NationalId`, `PassportNumber`, `CreatedBy`, `ModifiedBy`, `DeletedBy`.
    - *Contact & Communication*: `Email`, `PhoneNumber`, `WebsiteUrl`, `Subject`, `MessageBody`, `Comment`, `Note`.
    - *Organizational & Operational*: `CompanyName`, `DepartmentName`, `PositionTitle`, `TenantCode`, `EmployeeCode`, `CustomerCode`, `SupplierCode`, `WarehouseCode`, `SalesChannelCode`.
    - *Geography & Localization*: `Country`, `PostalCode`, `Address`, `LanguageCode`, `LocaleCode`, `TimeZoneCode`, `CurrencyCode`.
    - *Document & Inventory*: `DocumentNumber`, `ReferenceNumber`, `OrderNumber`, `ReceiptNumber`, `BatchNumber`, `SerialNumber`, `Barcode`, `SKU`, `LicenseKey`, `FileName`, `ContentType`, `Description`, `Code`, `ExternalReference`.
    - *Finance, Quantitative & Temporal*: `Percentage`, `TaxRate`, `DiscountRate`, `Quantity`, `ExchangeRate`, `BusinessDate`, `DateRange`, `TimeRange`.
    - *Security & Protection*: `PasswordHash`, `[SensitiveData]` attribute, virtual `IsSensitive` / `Mask` properties for Native AOT zero-reflection masking, and `[DebuggerDisplay]` redaction.
- **Roslyn Analyzers (`EricksonLopez.ValueObjects.Analyzers`)**:
  - `ELVO001`: Enforces private or protected constructors on Value Objects to mandate static factories.
  - `ELVO002`: Enforces static `Create(...)` factory method returning `Result<T>`.
  - `ELVO003`: Enforces absolute immutability on Value Objects (disallowing mutable properties/fields).
- **Roslyn Incremental Source Generator (`EricksonLopez.ValueObjects.Generators`)**:
  - `ValueObjectIncrementalGenerator`: Generates `IParsable<TSelf>` and `ISpanParsable<TSelf>` implementations for types decorated with `[ValueObject]`.
- **Domain Primitives Bridge (`EricksonLopez.ValueObjects.DomainPrimitives`)**:
  - Extension methods `ToDomainPrimitive` and `ToStrongId` providing seamless bridging to `EricksonLopez.DomainPrimitives.Abstractions`.
- **JSON Serialization (`EricksonLopez.ValueObjects.Serialization.Json`)**:
  - Native AOT System.Text.Json converters for `SingleValueObject<TSelf, TValue>`, `StringValueObject<TSelf>`, and `Range<T>`.
- **Dapper Integration (`EricksonLopez.ValueObjects.Dapper`)**:
  - High-performance Dapper `SqlMapper.TypeHandler` implementations for scalar and struct value objects (`SingleValueObjectTypeHandler`, `StructValueObjectTypeHandler`, `ValueObjectTypeHandler`).
- **Entity Framework Core Integration (`EricksonLopez.ValueObjects.EntityFrameworkCore`)**:
  - ModelBuilder configuration extensions `ConfigureDomainValueObjects` and NativeAOT-ready `ValueConverter` implementations with full trimming metadata annotations.
- **Fiscal Satellites**:
  - **Dominican Republic (`.Fiscal.DominicanRepublic`)**: `Rnc` (Modulo 11), `Cedula` (Modulo 10), `Ncf` (Serie B), `ElectronicNcf` (e-CF Serie E / Law 32-23), `FiscalPeriod`, `SecurityCode`.
  - **Mexico (`.Fiscal.Mexico`)**: `Rfc` (SAT with homoclave checksum for person/entity), `Curp` (RENAPO), `FiscalUuid` (CFDI 4.0), `IdCcp` (Carta Porte 3.1), `PedimentoNumber` (Anexo 22), `TaxRegimeCode`, `PaymentFormCode`, `CfdiUsageCode`.
  - **Argentina (`.Fiscal.Argentina`)**: `Cuit` / `Cuil` (ARCA/AFIP Modulo 11), `Cbu` / `Cvu` (BCRA Modulo 10), `Cdi`, `Cae`, `Caea`, `Cai`, `PointOfSale`, `VoucherNumber`, `VoucherType`, `VoucherLetter`, `VatRate`, `JurisdictionCode`.
  - **Chile (`.Fiscal.Chile`)**: `Rut` (SII Modulo 11 with check digit 'K'), `FiscalFolio` (CAF), `DteTypeCode` (DTE 33, 34, 39, 41, 52, 61), `DocumentReference`, `TaxRateVat`, `WithholdingRate` (Law 21.133).
  - **Colombia (`.Fiscal.Colombia`)**: `Nit` (DIAN Modulo 11), `Cufe`, `Cude`, `Cune` (SHA-384), `DaneMunicipalityCode`, `CiiuCode`, `AuthorizationRange`, `RejectionReasonCode`, `TaxTypeCode`.
  - **Peru (`.Fiscal.Peru`)**: `Ruc` (SUNAT Modulo 11 with prefixes 10, 15, 17, 20), `CpeIdentifier` (Type-Series-Correlative), `CpeTypeCode`, `DetractionAccount` (SPOT Banco de la Nacion / National Bank), `SunatProductCode`, `TaxPeriod` (SIRE YYYYMM), `UbigeoCode` (INEI).
- **Test Suite & Tooling**:
  - 15 test projects with 1,507+ automated tests achieving 100% line, branch, and method coverage.
  - Dedicated NativeAOT smoke test suite (`EricksonLopez.ValueObjects.NativeAotTests`).
  - Stryker.NET mutation testing configuration with 100% mutation score threshold.
- **Documentation & Governance**:
  - Comprehensive English technical documentation across all modules, architecture blueprints (`docs/architecture.md`), fiscal specifications (`docs/fiscal-architecture-specification.md`), and ADR catalog (`docs/adr/`).
  - Standard OSS repository health files (`CONTRIBUTING.md`, `CODE_OF_CONDUCT.md`, `SECURITY.md`, `SUPPORT.md`, `LICENSE`, GitHub issue & pull request templates).
  - Centralized NuGet package documentation (`docs/nuget-packages.md`) and CI/CD operations guide (`docs/ci-cd.md`).

