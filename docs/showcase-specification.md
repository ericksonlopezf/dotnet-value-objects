# EricksonLopez.ValueObjects — Showcase Specification and Architectural Reference

> **Official Reference Implementation and Executable Living Documentation**  
> Target: **.NET 10 / C# 13** · Architecture: **Pure Domain-Driven Design (DDD)** · Memory Model: **Zero-Allocation / Native AOT**  
> Status: **100% Synchronized with Public API & Runtime Verified** | Last sync: 2026-08-24

---

## 1. Discovery and Repository Classification

The following table provides the structural classification of all projects within the `EricksonLopez.ValueObjects.slnx` solution:

| Project Path | Category | Architectural Responsibility | Target Framework |
|---|---|---|---|
| `src/EricksonLopez.ValueObjects` | **Core Library** | Central domain kernel: base abstractions (`IValueObject`, `SingleValueObject`, `StringValueObject`, `Range<T>`), scalar numeric/temporal types (`Money`, `TaxRate`, `Percentage`, `Range`, `TimeRange`), and 45+ universal Value Objects. | `net10.0` |
| `src/EricksonLopez.ValueObjects.DomainPrimitives` | **Core Library** | Bridge and adaptation layer to `EricksonLopez.DomainPrimitives.Abstractions` (`ToDomainPrimitive`, `ToStrongId`). | `net10.0` |
| `src/EricksonLopez.ValueObjects.Fiscal.Argentina` | **Core Library** | Argentina fiscal domain (ARCA/AFIP): CUIT, CUIL, CBU, CVU, CAE, CAEA, CAI, CDI, Point of Sale, Voucher Types, and VAT Rates with Modulo 10 and 11 algorithms. | `net10.0` |
| `src/EricksonLopez.ValueObjects.Fiscal.Chile` | **Core Library** | Chile fiscal domain (SII): RUT (Modulo 11 with check digit 'K'), Fiscal DTE Folio, DTE Types (33, 34, 39, 41, 52, 61), VAT Rates, and Withholding (Law 21.133). | `net10.0` |
| `src/EricksonLopez.ValueObjects.Fiscal.Colombia` | **Core Library** | Colombia fiscal domain (DIAN): NIT (Modulo 11), CUFE, CUDE, CUNE (SHA-384), DANE municipality codes, CIIU, and Tax Types. | `net10.0` |
| `src/EricksonLopez.ValueObjects.Fiscal.DominicanRepublic` | **Core Library** | Dominican Republic fiscal domain (DGII): RNC (Modulo 11), Cedula (Modulo 10 / Luhn), Traditional NCF, Electronic e-NCF (e-CF), Fiscal Period, and Security Code. | `net10.0` |
| `src/EricksonLopez.ValueObjects.Fiscal.Mexico` | **Core Library** | Mexico fiscal domain (SAT CFDI 4.0): RFC (Natural/Moral persons with homoclave), CURP, Fiscal UUID, IdCCP (Bill of Lading), Customs Pedimento Number, CFDI Usage, and Tax Regime. | `net10.0` |
| `src/EricksonLopez.ValueObjects.Fiscal.Peru` | **Core Library** | Peru fiscal domain (SUNAT): RUC (Modulo 11), CPE Identifier (Type-Series-Correlative), Detraction Accounts, Ubigeo Code, Affectation Types, and Tax Period. | `net10.0` |
| `src/EricksonLopez.ValueObjects.Serialization.Json` | **Infrastructure** | High-performance Native AOT-compatible `System.Text.Json` converters for scalar, string-based, and generic Value Objects (`Range<T>`). | `net10.0` |
| `src/EricksonLopez.ValueObjects.EntityFrameworkCore` | **Infrastructure** | Relational persistence adapters: Individual `ValueConverter` mappings and centralized `ConfigureDomainValueObjects` extensions for EF Core 10. | `net10.0` |
| `src/EricksonLopez.ValueObjects.Dapper` | **Infrastructure** | Micro-ORM adapters: Generic and structured `SqlMapper.TypeHandler` for high-throughput zero-allocation Dapper mapping. | `net10.0` |
| `src/EricksonLopez.ValueObjects.Generators` | **Internal / Tooling** | Roslyn Incremental Source Generator synthesizing `IParsable<TSelf>` and `ISpanParsable<TSelf>` contracts for types decorated with `[ValueObject]`. | `netstandard2.0` |
| `src/EricksonLopez.ValueObjects.Analyzers` | **Internal / Tooling** | Roslyn Diagnostic Analyzers (`ELVO001`, `ELVO002`, `ELVO003`) enforcing compile-time invariants (private constructors, factory methods, immutability). | `netstandard2.0` |
| `samples/EricksonLopez.ValueObjects.Samples` | **Samples / Showcase** | **Official Executable Showcase**: Progressive reference implementation (Level 00 to Level 10) validating 100% of the public API at runtime. | `net10.0` |
| `benchmarks/EricksonLopez.ValueObjects.Benchmarks` | **Benchmarks** | Micro-benchmarks using BenchmarkDotNet measuring heap allocation, span parsing throughput, and struct vs record performance. | `net10.0` |
| `tests/*` (14 test projects) | **Tests** | Comprehensive test suite containing 1,516+ test cases (100% line, branch, and mutation coverage). | `net10.0` |
| `docs/*` | **Documentation** | Architectural blueprints, DDD design specifications, fiscal regulations, and Architecture Decision Records (ADRs). | N/A |

---

## 2. Public API Inventory

### 2.1 Core Abstractions and Domain Kernel (`EricksonLopez.ValueObjects`)

| Type | Namespace | Responsibility | Dependencies | Use Cases | Complexity | Showcase Sample |
|---|---|---|---|---|---|:---:|
| `IValueObject` | `EricksonLopez.ValueObjects` | Marker interface for all value objects in the ecosystem. | None | Generic identification, AOT-safe reflection, serialization. | Basic | Yes (Level 00, 08) |
| `IValueObject<TSelf>` | `EricksonLopez.ValueObjects` | Strongly typed contract for value objects enforcing equatable semantics (`IEquatable<TSelf>`). | `IValueObject` | Semantic equality and strict domain typing. | Basic | Yes (Level 01, 08) |
| `ValueObject` | `EricksonLopez.ValueObjects` | Abstract base record for composite value objects with multiple components. | `DomainPrimitives.ValueObject` | Physical addresses (`Address`), work shifts (`TimeRange`), coordinates. | Intermediate | Yes (Level 03, 08) |
| `SingleValueObject<TSelf, TValue>` | `EricksonLopez.ValueObjects` | Abstract base record for single-value wrappers (`TValue`). Provides comparison operators, `IComparable`, and explicit cast `(TValue)vo`. | `IValueObject<TSelf>`, `IComparable` | Custom scalar VOs, strongly typed identifiers. | Intermediate | Yes (Level 06, 08) |
| `StringValueObject<TSelf>` | `EricksonLopez.ValueObjects` | Specialized single-value record for strings with sanitization and validation pipelines. | `SingleValueObject<TSelf, string>` | Cleaned text, catalog codes, identifiers. | Intermediate | Yes (Level 02, 08) |
| `Range<T>` | `EricksonLopez.ValueObjects` | Generic `readonly record struct` modeling inclusive intervals `[Start .. End]`. | `IComparable<T>`, `IEquatable<T>` | Fiscal periods, numerical brackets, contract validities. | Advanced | Yes (Level 02, 09) |
| `RangeExtensions` | `EricksonLopez.ValueObjects` | Extension methods: `Duration()` for `Range<DateTimeOffset>`, `Days()` for `Range<DateOnly>`. | `Range<DateTimeOffset>`, `Range<DateOnly>` | Business temporal metrics. | Intermediate | Yes (Level 02) |
| `TimeRange` | `EricksonLopez.ValueObjects` | Composite value object for time intervals with overnight shift support. | `ValueObject`, `TimeOnly` | Shift scheduling, opening hours. Supports `Contains(TimeOnly)`, `Overlaps(TimeRange)`, `Duration`. | Intermediate | Yes (Level 02) |
| `Address` | `EricksonLopez.ValueObjects` | Composite value object representing a normalized physical postal address. | `ValueObject`, `Country`, `PostalCode` | Billing, logistics shipping, resident profiles. | Intermediate | Yes (Level 03) |
| `FullName` | `EricksonLopez.ValueObjects` | Composite value object for structured personal names. Overloads: `Create(string, string, string?)` and `Create(FirstName, LastName, MiddleName?)`. | `ValueObject`, `FirstName`, `LastName` | HR, CRM, identity management. | Basic | Yes (Level 03) |
| `SensitiveDataAttribute` | `EricksonLopez.ValueObjects` | Attribute masking PII and sensitive data in `ToString()` and log streams. | `Attribute` | Data privacy compliance (GDPR, HIPAA, PCI-DSS). | Basic | Yes (Level 01, 03, 08, 10) |
| `ValueObjectAttribute` | `EricksonLopez.ValueObjects` | Marker attribute activating incremental source generators for parsing. | `Attribute` | Automatic `IParsable<TSelf>` synthesis. | Advanced | Yes (Level 08) |
| `RegulatoryRuleAttribute` | `EricksonLopez.ValueObjects` | Metadata documenting statutory articles and regulatory mandates. | `Attribute` | Compliance audits and legal traceability. | Basic | Yes (Level 04, 08) |
| `DomainException` | `EricksonLopez.ValueObjects` | Unchecked exception reserved strictly for critical domain invariant violations. `ThrowIf(condition, message)` guard pattern. | `Exception` | Unrecoverable programmatic errors. | Basic | Yes (Level 06) |

---

### 2.2 Scalar Numeric and Financial Value Objects (`readonly record struct`)

| Type | Structure | Invariants and Rules | Operators | Key Methods | Complexity | Sample |
|---|---|---|---|---|:---:|:---:|
| `Money` | `readonly record struct` | High-precision decimal (up to 4 places) + ISO 4217 `CurrencyCode`. | `+`, `-`, `*`, `operator -()`, `<`, `>`, `<=`, `>=` | `Create`, `CreateNonNegative`, `Zero`, `ZeroUsd`, `Allocate` (Fowler), `Distribute`, `ApplyPercentage`, `Negate`, `Abs`, `Round`, `RoundCommercial`, `Add(Result)`, `Subtract(Result)`, `IsZero/IsPositive/IsNegative`, `IsGreaterThan/IsLessThan/IsGreaterThanOrEqual/IsLessThanOrEqual`, `ToString(format, provider)`, `TryFormat` | Advanced | Yes (Level 01, 03, 05, 07) |
| `CurrencyCode` | `readonly record struct` | ISO 4217 uppercase 3-letter currency code (e.g., "USD", "EUR", "DOP"). | `<`, `>`, `<=`, `>=` | `Create`, `TryParse`, `Parse` | Basic | Yes (Level 01, 02, 09, 10) |
| `Percentage` | `readonly record struct` | Fractional value between 0.00% and 100.00%. | `<`, `>`, `<=`, `>=` | `Create`, `FromFraction`, `ValidatePercentage`, `Fraction`, `AsFraction`, `ApplyTo`, `IsZero`, `Zero`, `Hundred`, `Full`, `TryParse`, `Parse` | Basic | Yes (Level 01, 06, 07, 09) |
| `TaxRate` | `readonly record struct` | Statutory tax rate (0.00% to 100.00%). | `<`, `>`, `<=`, `>=` | `Create`, `Fraction`, `AsFraction`, `CalculateTax(decimal)`, `CalculateTax(Money)`, `IsExempt`, `Exempt`, `TryParse`, `Parse` | Basic | Yes (Level 02, 03, 05, 07, 09, 10) |
| `DiscountRate` | `readonly record struct` | Commercial discount rate (0.00% to 100.00%). | `<`, `>`, `<=`, `>=` | `Create`, `Fraction`, `AsFraction`, `CalculateDiscount`, `ApplyTo(decimal)`, `ApplyTo(Money)`, `IsZero`, `None`, `TryParse`, `Parse` | Basic | Yes (Level 02, 03, 07, 10) |
| `ExchangeRate` | `readonly record struct` | Foreign exchange rate pair (`FromCurrency`, `ToCurrency`, `Rate`). | None | `Create`, `Convert(Money)`, `Inverse()` | Intermediate | Yes (Level 02, 03) |
| `Quantity` | `readonly record struct` | Non-negative integer quantity (>= 0). | `+`, `-`, `<`, `>`, `<=`, `>=` | `Create`, `Add`, `Subtract`, `Zero`, `IsZero`, `TryParse`, `Parse` | Basic | Yes (Level 03, 05, 07, 10) |
| `BusinessDate` | `readonly record struct` | Immutable commercial/accounting date based on `DateOnly`. | `<`, `>`, `<=`, `>=` | `Create`, `FromDateTimeOffset`, `Parse`, `TryParse(string)`, `TryParse(Span)` | Intermediate | Yes (Level 02, 07, 10) |
| `DateRange` | `readonly record struct` | Specialized temporal interval for `DateOnly` with overlap calculations. | `<`, `>`, `<=`, `>=` | `Create`, `DurationInDays`, `Contains(DateOnly)`, `Overlaps(DateRange)` | Intermediate | Yes (Level 02, 10) |
| `Email` | `readonly record struct` | Validated, lowercase normalized email address. Protected via `[SensitiveData]`. | `<`, `>`, `<=`, `>=` | `Create`, `LocalPart`, `Domain`, `Masked()`, `TryParse`, `Parse` | Basic | Yes (Level 01, 03, 06, 09) |
| `PhoneNumber` | `readonly record struct` | International E.164 formatted telephone number (`+` prefix). | `<`, `>`, `<=`, `>=` | `Create`, `TryParse`, `Parse` | Basic | Yes (Level 01, 06, 09) |

---

### 2.3 Textual and Business Value Objects (`StringValueObject<TSelf>`)

| Type | Domain Category | Format and Validation Rules | Normalization | Sample |
|---|---|---|---|:---:|
| `SKU` | Supply Chain | 3–50 alphanumeric and hyphen characters. | Uppercase Trim | Yes (Level 03, 10) |
| `Barcode` | Inventory | Standard barcode format (EAN-8, EAN-13, UPC-A, Code128). | Trimmed | Yes (Level 03) |
| `WarehouseCode` | Logistics | Warehouse identifier (2–30 characters). | Uppercase Trim | Yes (Level 03) |
| `BatchNumber` | Traceability | Production batch identifier (2–50 characters). | Uppercase Trim | Yes (Level 03) |
| `SerialNumber` | Inventory | Asset serial number (2–60 characters). | Uppercase Trim | Yes (Level 03) |
| `OrderNumber` | Sales | Order correlative number (2–50 characters). | Uppercase Trim | Yes (Level 03, 10) |
| `CustomerCode` | CRM / Sales | Customer code identifier (2–30 characters). | Uppercase Trim | Yes (Level 03, 10) |
| `SalesChannelCode`| Omnichannel | Sales channel identifier (e.g., "ECOMMERCE", "POS"). | Uppercase Trim | Yes (Level 03) |
| `DocumentNumber` | Invoicing | Commercial document / invoice number (2–50 characters). | Uppercase Trim | Yes (Level 03) |
| `ReceiptNumber` | Treasury | Cash receipt or collection number (2–50 characters). | Uppercase Trim | Yes (Level 03) |
| `ReferenceNumber`| Reconciliation | Bank/gateway payment reference (2–100 characters). | Uppercase Trim | Yes (Level 03) |
| `SupplierCode` | Procurement | Vendor/supplier code (2–30 characters). | Uppercase Trim | Yes (Level 03) |
| `Country` | Geographic | ISO 3166-1 alpha-2 exactly 2 uppercase letters (e.g., "DO", "US", "ES"). | Uppercase Trim | Yes (Level 02, 03, 06) |
| `PostalCode` | Geographic | Alphanumeric postal code (2–20 characters). | Trimmed | Yes (Level 02, 03, 09) |
| `TimeZoneCode` | Localization | IANA time zone identifier (e.g., "America/Santo_Domingo"). | Trimmed | Yes (Level 02, 10) |
| `LanguageCode` | Localization | ISO 639-1 language code (2 lowercase letters, e.g., "es", "en"). | Lowercase Trim | Yes (Level 02, 10) |
| `LocaleCode` | Localization | Culture/locale tag (e.g., "es-DO", "en-US"). | Trimmed | Yes (Level 10) |
| `WebsiteUrl` | Communication | Valid absolute HTTP/HTTPS URI. | Lowercase/Trim | Yes (Level 02, 10) |
| `NationalId` | Identity PII | Generic national identity document (masked). | Uppercase Trim | Yes (Level 03) |
| `PassportNumber` | Identity PII | International passport number (5–20 characters, masked). | Uppercase Trim | Yes (Level 03) |
| `CompanyName` | Corporate | Registered business name (2–150 characters). | Trimmed | Yes (Level 03) |
| `DepartmentName` | Organizational | Corporate department / division name. | Trimmed | Yes (Level 03) |
| `PositionTitle` | HR | Job position / title (2–100 characters). | Trimmed | Yes (Level 03) |
| `FirstName` | Personal | Given first name (1–50 characters). | Trimmed | Yes (Level 03) |
| `MiddleName` | Personal | Middle name (optional / empty permitted). | Trimmed | Yes (Level 03) |
| `LastName` | Personal | Surname / family name (1–50 characters). | Trimmed | Yes (Level 03) |
| `DisplayName` | Personal | User public name / UI alias. | Trimmed | Yes (Level 03) |
| `TenantCode` | Multi-Tenancy | Secure tenant/organization slug for SaaS partitions. | Lowercase Trim | Yes (Level 09, 10) |
| `LicenseKey` | Licensing | Software license key formatted in alphanumeric chunks (masked). | Uppercase Trim | Yes (Level 10) |
| `PasswordHash` | Security | Cryptographic password hash (masked). | Untouched | Yes (Level 01) |
| `CreatedBy` | Audit | User identifier who created the record. | Uppercase Trim | Yes (Level 10) |
| `ModifiedBy` | Audit | User identifier who last modified the record. | Uppercase Trim | Yes (Level 10) |
| `DeletedBy` | Audit | User identifier who performed soft deletion. | Uppercase Trim | Yes (Level 10) |
| `Subject` | Messaging | Notification / message subject (1–200 characters). | Trimmed | Yes (Level 10) |
| `Comment` | Collaboration | User comment or discussion note (1–2000 characters). | Trimmed | Yes (Level 10) |
| `Note` | Collaboration | Short textual note (1–500 characters). | Trimmed | Yes (Level 10) |
| `MessageBody` | Messaging | Body of notification / email (up to 10,000 characters). | Trimmed | Yes (Level 10) |
| `ContentType` | Files | Standard MIME type (e.g., "application/pdf", "image/png"). | Lowercase Trim | Yes (Level 10) |
| `FileName` | Files | Sanitized file name with valid extension. | Trimmed | Yes (Level 10) |
| `Code` | Catalog | Generic alphanumeric catalog code (2–50 characters). | Uppercase Trim | Yes (Level 02) |
| `ExternalReference`| Integration | External third-party system reference identifier. | Trimmed | Yes (Level 03) |

---

### 2.4 Multi-Country Fiscal Satellites

#### Dominican Republic (`EricksonLopez.ValueObjects.Fiscal.DominicanRepublic`)
- `Rnc`: National Taxpayer Registry (9 or 11 digits, DGII Modulo 11 validation).
- `Cedula`: National Identity and Voter ID (11 digits, DGII Modulo 10 / Luhn validation).
- `Ncf`: Traditional Fiscal Receipt Number (Series 'B', type, and sequence).
- `ElectronicNcf`: Electronic Fiscal Receipt (e-CF, Series 'E', types 31, 32, 33, 34, 41, 43, 44, 45, 46, 47).
- `FiscalPeriod`: Accounting tax period YYYYMM.
- `SecurityCode`: 6-character alphanumeric verification code for e-CF web validation.
- `TaxpayerId`: Composite value object encapsulating either RNC or Cedula.
- `TaxpayerIdType`: Enum distinguishing between Corporate (RNC) and Individual (Cedula).

#### Mexico (`EricksonLopez.ValueObjects.Fiscal.Mexico`)
- `Rfc`: Federal Taxpayer Registry with SAT homoclave validation (12 characters for entities, 13 for individuals).
- `Curp`: Unique Population Registry Code (18 characters with verification algorithm).
- `FiscalUuid`: Fiscal Digital Stamp Universal Unique Identifier (UUID v4 timbrado SAT).
- `IdCcp`: Waybill Identifier (Carta Porte) for federal freight transportation.
- `PaymentFormCode`: Official SAT payment method catalog keys (01, 02, 03, 04, 28, 99, etc.).
- `TaxRegimeCode`: Official SAT tax regime catalog keys (601, 603, 605, 606, 612, 626, etc.).
- `CfdiUsageCode`: Official SAT CFDI usage catalog keys (G01, G02, G03, I01, D01, P01, CP01, CN01, etc.).
- `PedimentoNumber`: Customs import clearance identifier (15 digits formatted per SAT rules).

#### Argentina (`EricksonLopez.ValueObjects.Fiscal.Argentina`)
- `Cuit`: Unique Tax Identification Key with AFIP Modulo 11 algorithm (prefixes 20, 23, 24, 27, 30, 33, 34).
- `Cuil`: Unique Labor Identification Key for individuals.
- `Cbu`: Single Banking Code (22 digits with dual-block verification for Bank/Branch and Account).
- `Cvu`: Virtual Banking Code for Payment Service Providers (PSP / Fintech).
- `Cae`: Electronic Authorization Code issued by AFIP (14 digits).
- `Caea`: Anticipated Electronic Authorization Code (14 digits).
- `PointOfSale`: AFIP POS number (1 to 99999).
- `VoucherNumber`: Correlative tax voucher number (1 to 99999999).
- `VoucherLetter`: Fiscal invoice classification letter ('A', 'B', 'C', 'E', 'M', 'T').
- `VoucherType`: Official AFIP voucher classification codes (Invoice A=1, Invoice B=6, Debit Note=2, etc.).
- `VatRate`: Current statutory VAT rates in Argentina (0%, 2.5%, 5%, 10.5%, 21%, 27%).

#### Chile (`EricksonLopez.ValueObjects.Fiscal.Chile`)
- `Rut`: Single Tax Role with SII Modulo 11 algorithm (numeric check digit or 'K').
- `FiscalFolio`: Correlative DTE fiscal sequence number.
- `DteTypeCode`: Official SII electronic document types (33=Invoice, 34=Exempt Invoice, 39=Receipt, 52=Dispatch Guide, 61=Credit Note).
- `TaxRateVat`: Standard VAT rate in Chile (19.0%).
- `WithholdingRate`: Fee withholding tax rates under Law 21.133.
- `DocumentReference`: Reference to preceding tax documents.

#### Colombia (`EricksonLopez.ValueObjects.Fiscal.Colombia`)
- `Nit`: Tax Identification Number with DIAN Modulo 11 check digit verification.
- `Cufe`: Unique Electronic Invoice Code (SHA-384 96-character hexadecimal hash).
- `Cude`: Unique Electronic Document Code (Support Document / Credit / Debit Notes).
- `Cune`: Unique Electronic Payroll Code.
- `DaneMunicipalityCode`: 5-digit DANE geopolitical municipality division code.
- `CiiuCode`: 4-digit International Standard Industrial Classification code.
- `TaxTypeCode`: Official DIAN tax type catalog (01=IVA, 02=IC, 03=ICA, 04=INC, 05=ReteIVA).

#### Peru (`EricksonLopez.ValueObjects.Fiscal.Peru`)
- `Ruc`: Single Taxpayers Registry (11 digits, SUNAT Modulo 11, prefixes 10, 15, 17, 20).
- `CpeIdentifier`: Complete Electronic Payment Voucher Identifier (Type-Series-Correlative).
- `CpeTypeCode`: SUNAT voucher classification catalog (01=Invoice, 03=Receipt, 07=Credit Note, 08=Debit Note).
- `AffectationTypeCode`: SUNAT IGV tax affectation catalog (10=Taxed, 20=Exempt, 30=Unaffected).
- `DetractionAccount`: Bank of the Nation SPOT tax withholding checking account.
- `TaxPeriod`: Accounting tax period (YYYYMM).
- `UbigeoCode`: 6-digit INEI geographical location code.

---

### 2.5 Infrastructure and Persistence Adapters

#### Entity Framework Core (`EricksonLopez.ValueObjects.EntityFrameworkCore`)
- `ValueObjectModelConfigurationExtensions.ConfigureDomainValueObjects`: Automatically registers conversion pipelines for all domain value objects on the `ModelConfigurationBuilder`.
- `StringValueObjectValueConverter<TVO>`: Open generic value converter for any `StringValueObject`.
- `SingleValueObjectValueConverter<TVO, TValue>`: Generic value converter for scalar value objects.
- `EmailValueConverter`, `PhoneNumberValueConverter`, `PostalCodeValueConverter`, `CurrencyCodeValueConverter`, `PercentageValueConverter`, `TaxRateValueConverter`, `QuantityValueConverter`.

#### Dapper (`EricksonLopez.ValueObjects.Dapper`)
- `ValueObjectTypeHandler.Register<TVO, TPrimitive>(Func<TPrimitive, Result<TVO>> factory)`: Registers a `SqlMapper.TypeHandler` for any `SingleValueObject`.
- `ValueObjectTypeHandler.RegisterStruct<TVO, TPrimitive>(factory, valueSelector)`: Registers a `SqlMapper.TypeHandler` for `readonly record struct` types.
- `SingleValueObjectTypeHandler<TVO, TPrimitive>` and `StructValueObjectTypeHandler<TVO, TPrimitive>`.

#### System.Text.Json (`EricksonLopez.ValueObjects.Serialization.Json`)
- `RangeJsonConverter<T>`: Native AOT-compatible JSON converter for `Range<T>` intervals.
- `StringValueObjectJsonConverter<TSelf>`: Direct string serialization without reflection.
- `SingleValueObjectJsonConverter<TSelf, TValue>`: Generic serializer for primitive wrappers.

#### Domain Primitives Bridge (`EricksonLopez.ValueObjects.DomainPrimitives`)
- `ValueObjectDomainPrimitiveExtensions.ToDomainPrimitive<TSelf, TValue, TPrimitive>`: Functional mapping to `IDomainPrimitive`.
- `ValueObjectDomainPrimitiveExtensions.ToStrongId<TSelf, TValue, TStrongId>`: Functional mapping to `IStrongId`.
- `DomainPrimitiveErrorExtensions.ToError` and `ToPrimitiveError`: Error translation between `PrimitiveError` and `Result.Error`.

---

## 3. Functional Architecture and Processing Flow

The following diagram illustrates data flow and boundary isolation across the ecosystem:

```
[ Ingestion Layer / DTOs / API ]
               │
               ▼  (Invocation of static Create factories)
[ StringPipeline & Immutable Invariants ]
    ├─ Trimming & Normalization (Upper/Lower)
    ├─ Length, Regex & Format Checks
    └─ Statutory Checksums (Mod-10, Mod-11, Luhn)
               │
               ├─────────────────────────┬─────────────────────────┐
               ▼ (IsSuccess == true)     ▼ (IsFailure == true)     │
    [ Instantiated Value Object ]    [ Result<T>.Failure ]         │
    - readonly record struct         - Zero Exception Overhead     │
    - sealed record                  - Typed Domain Error Code     │
    - [SensitiveData] active         - Return to Client Pipeline   │
               │                                                   │
               ▼                                                   │
[ Pure Domain Logic & Operations ]                                 │
    ├─ Fowler Money Arithmetic & Allocate                          │
    ├─ Range<T> & TimeRange Intervals                              │
    └─ Aggregate Root Composition (DDD)                            │
               │                                                   │
               ├─────────────────────────────────┐                 │
               ▼                                 ▼                 ▼
[ Persistence Adapters ]               [ Serialization Adapters ]
    ├─ EF Core 10 ValueConverters          ├─ System.Text.Json Converters
    └─ Dapper SqlMapper.TypeHandlers       └─ Native AOT JSON Contexts
```

### Architectural Guarantees:
1. **Entry to Domain**: Raw unvalidated primitives never enter domain aggregates. Invocations of `Create(...)` guarantee invalid state cannot exist in memory.
2. **Domain Purity**: Core value objects have zero dependencies on databases, ORMs, or web frameworks. Math operations (`Money + Money`, `Range.Overlaps`) execute on the stack without side effects.
3. **Domain to Infrastructure**: During persistence or serialization, adapters (`ValueConverter`, `TypeHandler`, `JsonConverter`) unwrap the primitive representation safely without breaking encapsulation.

---

## 4. Progressive Showcase Suite (Levels 00 to 10)

The `samples/EricksonLopez.ValueObjects.Samples` project contains the executable 11-level showcase suite:

| Level | File Name | Dimension Covered | Concepts Demonstrated |
|---|---|---|---|
| **00** | `Level00_Conceptual.cs` | DDD Fundamentals & Philosophy | Primitive Obsession, immutability, Result-over-exceptions, comparative analysis. |
| **01** | `Level01_QuickStart.cs` | Quick Start | Functional instantiation with `Result<T>`, Email with `[SensitiveData]`, basic Money, Percentage, E.164 PhoneNumber. |
| **02** | `Level02_ConfigurationAndPipelines.cs` | Pipelines, Ranges, Dates | StringPipeline normalization, `Range<DateOnly>.Contains(T)`, `Range<T>.Intersects()`, `RangeExtensions.Duration()`, `BusinessDate` (Create/FromDateTimeOffset/Parse/TryParse), overnight `TimeRange` (Contains/Overlaps/Duration), `TaxRate`, `ExchangeRate`. |
| **03** | `Level03_RealWorldUseCases.cs` | Production Use Cases | Full `Money` API (Negate/Abs/Round/RoundCommercial/Distribute/ApplyPercentage/CreateNonNegative/Zero/ZeroUsd/TryFormat), `TaxRate.CalculateTax(Money)`, `DiscountRate.ApplyTo(Money)/None`, `ExchangeRate.Convert/Inverse`, `Email.LocalPart/Domain/Masked`, `FullName(FirstName,LastName)`, `Quantity.Add/Subtract/Zero`, supply chain VOs. |
| **04** | `Level04_MultiCountryFiscalDomains.cs` | Multi-Country Compliance | Statutory tax validation across 6 jurisdictions: DGII (DO), SAT (MX), DIAN (CO), AFIP (AR), SII (CL), SUNAT (PE). |
| **05** | `Level05_HighThroughputProcessing.cs` | Concurrency & Performance | 100,000 parallel `Money.Allocate` operations with `Parallel.For`, zero locks, zero data races. |
| **06** | `Level06_ErrorHandlingAndValidation.cs` | Functional Error Handling | `Percentage` full API (FromFraction/ValidatePercentage/ApplyTo/Zero/Hundred/Full/IsZero/Parse), composite DTO validation, `DomainException.ThrowIf()`, comparison operators (`</>/<=/>=`) on `TaxRate`/`Quantity`, explicit cast `(string)vo`. |
| **07** | `Level07_ZeroAllocationAot.cs` | Zero-Allocation & Native AOT | `TaxRate.Parse(Span)`, `Percentage.Parse/TryParse(Span)`, `Quantity.Parse(string/Span)`, `BusinessDate.Parse/TryParse(Span)`, `DiscountRate.Parse/TryParse`, `Money.TryFormat(Span)`, heap allocation verification. |
| **08** | `Level08_CustomValueObjects.cs` | Domain Extensibility | `IValueObject`/`IValueObject<TSelf>` contracts, `[SensitiveData]` on custom VO, `[RegulatoryRule]`, `[ValueObject]`, custom `StringValueObject` (`ProjectCode`), composite `ValueObject` (`GeoCoordinate`), `StringValueObjectJsonConverter<TSelf>` extension. |
| **09** | `Level09_PersistenceAndSerialization.cs` | Persistence & Serialization | `RangeJsonConverter<T>`, `ValueObjectTypeHandler.Register<>`, `ValueObjectTypeHandler.RegisterStruct<>`, `SingleValueObjectValueConverter<TVO,TValue>(factory)`, `StringValueObjectValueConverter<TVO>(factory)`, EF Core 10 SQLite in-memory. |
| **10** | `Level10_EnterpriseDddPatterns.cs` | Enterprise DDD Patterns | `DateRange` (delivery windows/fiscal quarters), `BusinessDate` (invoice/contract dates), `DiscountRate.ApplyTo(Money) + TaxRate.CalculateTax(Money)` pipeline, DomainPrimitives bridge docs, full `EnterpriseOrder` aggregate root. |

---

## 5. Enterprise Cookbook

### Recipe 1: Penny-Loss-Free Fund Distribution (Fowler Money Allocation)
- **Problem**: Dividing a monetary amount across parties in odd ratios (e.g., $100.00 / 3) using standard division causes rounding discrepancy ($33.33 * 3 = $99.99).
- **Solution**: Use `Money.Allocate(ratios...)` to distribute indivisible remainder cents sequentially to leading shares.
- **Code**:
```csharp
var invoice = Money.Create(100.00m, "USD").Value;
Money[] parts = invoice.Allocate(1, 1, 1);
// parts[0] = $33.34 USD
// parts[1] = $33.33 USD
// parts[2] = $33.33 USD
// Total sum: $100.00 USD exact
```
- **Best Practice**: Use `Allocate` for commissions and invoice splits. Never use `Distribute` for unequal proportions.
- **Common Error**: Using integer division `amount / 3` instead — loses the residual cent.

---

### Recipe 2: Equal-Parts Distribution Without Penny Loss
- **Problem**: Splitting a bill into N strictly equal parts.
- **Solution**: Use `Money.Distribute(int parts)` — distributes evenly, assigning the extra cent to the first slice.
- **Code**:
```csharp
var bill = Money.Create(100.00m, "USD").Value;
Money[] thirds = bill.Distribute(3);
// thirds[0] = $33.34 USD
// thirds[1] = $33.33 USD
// thirds[2] = $33.33 USD
```
- **Best Practice**: `Distribute` is preferred for equal splits; `Allocate` for weighted splits.

---

### Recipe 3: Overnight Time Intervals and Shift Scheduling
- **Problem**: Modeling work shifts that span past midnight without requiring full `DateTime` calendar dates.
- **Solution**: Use `TimeRange.Create(start, end, allowOvernight: true)`.
- **Code**:
```csharp
var nightShift = TimeRange.Create(new TimeOnly(22, 0), new TimeOnly(6, 0), allowOvernight: true).Value;
Console.WriteLine($"Crosses midnight: {nightShift.CrossesMidnight}"); // True
Console.WriteLine($"Duration: {nightShift.Duration.TotalHours}h");    // 8h

bool atMidnight = nightShift.Contains(new TimeOnly(0, 30)); // True
```
- **Best Practice**: Always validate with `IsSuccess` before accessing `.Value`.

---

### Recipe 4: Centralized Persistence Configuration in EF Core 10
- **Problem**: Manually mapping value converters across dozens of entities results in boiler-plate.
- **Solution**: Call `ConfigureDomainValueObjects` inside `ConfigureConventions`.
- **Code**:
```csharp
public sealed class ApplicationDbContext : DbContext
{
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.ConfigureDomainValueObjects();
        base.ConfigureConventions(configurationBuilder);
    }
}
```
- **Best Practice**: Register fiscal and custom VOs using `StringValueObjectValueConverter<TVO>` for non-standard types.
- **Common Error**: Calling `ConfigureDomainValueObjects()` inside `OnModelCreating` instead of `ConfigureConventions` — converters won't apply globally.

---

### Recipe 5: Enterprise Invoice Tax Calculation Chain
- **Problem**: Apply a commercial discount, then compute tax on the reduced price — all in a type-safe, auditable pipeline.
- **Solution**: Chain `DiscountRate.ApplyTo(Money)` → `TaxRate.CalculateTax(Money)`.
- **Code**:
```csharp
var price    = Money.Create(1000.00m, "USD").Value;
var discount = DiscountRate.Create(10.0m).Value;
var itbis    = TaxRate.Create(18.0m).Value;

Money netPrice    = discount.ApplyTo(price);     // $900.00 USD
Money taxAmount   = itbis.CalculateTax(netPrice); // $162.00 USD
Money totalPrice  = netPrice + taxAmount;          // $1,062.00 USD
```
- **Best Practice**: Never apply tax before discount. Always store gross, net, and tax as separate Money VOs.
- **Common Error**: Using `decimal * fraction` directly — bypasses currency validation and rounding invariants.

---

### Recipe 6: Multi-Currency Conversion with Type Safety
- **Problem**: Converting a monetary value from one currency to another without losing type safety.
- **Solution**: Use `ExchangeRate.Create` + `ExchangeRate.Convert(Money)` + `ExchangeRate.Inverse()`.
- **Code**:
```csharp
var usdToDop = ExchangeRate.Create(CurrencyCode.USD, CurrencyCode.DOP, 60.25m).Value;
var amount   = Money.Create(100.00m, CurrencyCode.USD).Value;

var converted = usdToDop.Convert(amount).Value; // RD$6,025.00
var inverse   = usdToDop.Inverse().Value;        // DOP→USD rate
var backToUsd = inverse.Convert(converted).Value; // ~$100.00 USD
```
- **Best Practice**: Always use `Convert(Money)` — it validates currency pair compatibility.
- **Common Error**: Multiplying `Amount * Rate` directly — skips currency mismatch validation.

---

### Recipe 7: Delivery Window Validation with DateRange
- **Problem**: Validate that a requested delivery date falls within the agreed delivery window.
- **Solution**: Use `DateRange.Create` + `DateRange.Contains(DateOnly)`.
- **Code**:
```csharp
var window    = DateRange.Create(new DateOnly(2026, 9, 1), new DateOnly(2026, 9, 15)).Value;
var requested = new DateOnly(2026, 9, 7);

bool valid = window.Contains(requested); // True
Console.WriteLine($"Window: {window}, Duration: {window.DurationInDays} days");
```
- **Best Practice**: Use `BusinessDate` for milestone dates (invoice, signature), `DateRange` for intervals (delivery windows, validity periods).

---

### Recipe 8: PII Protection on Custom Value Objects
- **Problem**: Sensitive values (API keys, account numbers) appearing in log output.
- **Solution**: Decorate custom VO with `[SensitiveData]` and override `IsSensitive`/`Mask`.
- **Code**:
```csharp
[SensitiveData(mask: "sk-***")]
public sealed record ApiKey : StringValueObject<ApiKey>
{
    protected override bool IsSensitive => true;
    protected override string Mask => "sk-***";
    // ...
}

var key = ApiKey.Create("sk-live-secret-token").Value;
Console.WriteLine(key.Value);   // sk-live-secret-token  (real value for domain logic)
Console.WriteLine(key.ToString()); // sk-***              (redacted for logs)
```
- **Best Practice**: Always use `.Value` for domain operations, rely on `ToString()` for logging/tracing.

---

### Recipe 9: Guard Clauses for Programmer Errors
- **Problem**: Defensive programming for preconditions that should never happen in a correct system (division by zero, null aggregates, etc.).
- **Solution**: Use `DomainException.ThrowIf(condition, message)` — reserved for invariant violations, never for business validation.
- **Code**:
```csharp
void Distribute(Money total, int parts)
{
    DomainException.ThrowIf(parts <= 0, $"Cannot distribute into {parts} parts.");
    // ...
}
```
- **Best Practice**: `DomainException` is for programmer errors. Use `Result<T>.Failure(Error.Validation(...))` for business validation.
- **Common Error**: Throwing `DomainException` for user input errors — use `Result<T>` instead.

---

### Recipe 10: Struct VO Registration in Dapper
- **Problem**: Mapping `readonly record struct` Value Objects in Dapper queries (struct VOs cannot use the standard `TypeHandler<TVO>`).
- **Solution**: Use `ValueObjectTypeHandler.RegisterStruct<TVO,TPrimitive>(factory, valueSelector)`.
- **Code**:
```csharp
ValueObjectTypeHandler.RegisterStruct<TaxRate, decimal>(
    factory:       TaxRate.Create,
    valueSelector: vo => vo.Value);

ValueObjectTypeHandler.RegisterStruct<Quantity, int>(
    factory:       val => Quantity.Create(val),
    valueSelector: vo => vo.Value);
```
- **Best Practice**: Register struct handlers in application startup before any Dapper queries execute.
- **Common Error**: Using `Register<>` (for records) on struct types — will fail at runtime with a reflection error.

---

## 6. API Reference (Microsoft Learn Style)

### `Money.Allocate(params int[] ratios)`

#### Signature:
```csharp
public Money[] Allocate(params int[] ratios)
```

#### Parameters:
- `ratios` (`int[]`): An array of positive integer ratios (e.g., `1, 1, 1` for equal parts, or `50, 30, 20` for percentage proportions).

#### Return Value:
- `Money[]`: An array of `Money` instances whose sum strictly equals the original `Amount`.

#### Exceptions:
- `ArgumentException`: Thrown if `ratios` is empty or if the sum of all ratios is less than or equal to zero.

#### Remarks:
Implements Martin Fowler's canonical money allocation algorithm. The remainder is distributed one cent at a time to the leading slices in descending order of fractional remainder, guaranteeing exact accounting balancing.

#### Example (Basic):
```csharp
var invoice = Money.Create(100.00m, "USD").Value;
Money[] shares = invoice.Allocate(1, 1, 1);
// $33.34, $33.33, $33.33
```

#### When to Use: Always for weighted or equal monetary splits requiring accounting accuracy.
#### When NOT to Use: Do not use for single-value scaling — use the `*` operator instead.

---

### `Money.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)`

#### Signature:
```csharp
public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
```

#### Parameters:
- `destination`: Stack-allocated `Span<char>` buffer to write formatted output into.
- `charsWritten`: Number of characters written on success.
- `format`: Standard numeric format specifier (e.g., "N2", "N4").
- `provider`: Optional format provider (pass `null` for invariant culture).

#### Return Value:
- `true` if formatted successfully; `false` if the destination buffer is too small.

#### Remarks:
Implements `ISpanFormattable` for zero-heap-allocation string formatting. Ideal for logging, network protocols, and high-throughput serialization paths.

#### Example:
```csharp
Span<char> buf = stackalloc char[32];
var amount = Money.Create(1234.56m, "USD").Value;
if (amount.TryFormat(buf, out int written, "N2", null))
    Console.WriteLine(new string(buf[..written])); // "1,234.56 USD"
```

#### Performance: Zero heap allocations. Use for hot paths and logging interceptors.

---

### `TaxRate.CalculateTax(Money baseAmount)`

#### Signature:
```csharp
public Money CalculateTax(Money baseAmount)
```

#### Parameters:
- `baseAmount`: The net monetary base on which to apply the tax rate.

#### Return Value:
- `Money`: The calculated tax amount in the same currency as `baseAmount`.

#### Exceptions:
- None. Returns zero `Money` if `TaxRate` is `Exempt`.

#### Remarks:
Computes `baseAmount.Amount * (Rate / 100)` rounded using the domain's default precision. Uses the currency of `baseAmount` automatically.

#### Example (Basic):
```csharp
var taxRate = TaxRate.Create(18.0m).Value;
var net     = Money.Create(1000.00m, "DOP").Value;
Money tax   = taxRate.CalculateTax(net); // RD$180.00
```

#### Example (Advanced — Exempt Rate):
```csharp
var exempt = TaxRate.Exempt;
Money tax  = exempt.CalculateTax(net); // RD$0.00 (IsExempt = true)
```

#### When to Use: Computing VAT, GST, or sales tax in invoicing pipelines.
#### When NOT to Use: For discount calculations — use `DiscountRate.CalculateDiscount` instead.

---

### `DiscountRate.ApplyTo(Money price)`

#### Signature:
```csharp
public Money ApplyTo(Money price)
```

#### Parameters:
- `price`: The list/gross price to apply the discount to.

#### Return Value:
- `Money`: The net price after the discount is applied.

#### Remarks:
Computes `price - (price.Amount * Rate / 100)` preserving the original currency. Equivalent to multiplying by `(1 - fraction)`.

#### Example:
```csharp
var price    = Money.Create(1000.00m, "USD").Value;
var discount = DiscountRate.Create(15.0m).Value;
Money net    = discount.ApplyTo(price); // $850.00 USD
```

#### When to Use: Commercial pricing engines, quote generation, promotional pricing.
#### When NOT to Use: Applying fractional prices manually — use `Money.ApplyPercentage(Percentage)` instead.

---

## 7. Architectural Diagrams

### Component Dependency Structure

```mermaid
graph TD
    subgraph CoreDomain ["Core Domain (Domain Purity)"]
        EVO["EricksonLopez.ValueObjects<br/>(Base, Universal VOs, Range, Money)"]
        EVODP["EricksonLopez.ValueObjects.DomainPrimitives<br/>(Abstractions Bridge)"]
    end

    subgraph FiscalSatellites ["Fiscal Satellites (Multi-Country Compliance)"]
        FAR["Fiscal.Argentina (AFIP)"]
        FCL["Fiscal.Chile (SII)"]
        FCO["Fiscal.Colombia (DIAN)"]
        FDO["Fiscal.DominicanRepublic (DGII)"]
        FMX["Fiscal.Mexico (SAT)"]
        FPE["Fiscal.Peru (SUNAT)"]
    end

    subgraph InfrastructureAdapters ["Infrastructure Adapters (Zero Domain Pollution)"]
        EF["ValueObjects.EntityFrameworkCore<br/>(EF Core 10 Converters)"]
        DAP["ValueObjects.Dapper<br/>(SqlMapper TypeHandlers)"]
        JSON["ValueObjects.Serialization.Json<br/>(System.Text.Json Converters)"]
    end

    subgraph ShowcaseProject ["Showcase & Reference Runtime"]
        SHOWCASE["EricksonLopez.ValueObjects.Samples<br/>(Levels 00 to 10)"]
    end

    FAR --> EVO
    FCL --> EVO
    FCO --> EVO
    FDO --> EVO
    FMX --> EVO
    FPE --> EVO
    EVODP --> EVO

    EF --> EVO
    DAP --> EVO
    JSON --> EVO

    SHOWCASE --> EVO
    SHOWCASE --> EVODP
    SHOWCASE --> FAR
    SHOWCASE --> FCL
    SHOWCASE --> FCO
    SHOWCASE --> FDO
    SHOWCASE --> FMX
    SHOWCASE --> FPE
    SHOWCASE --> EF
    SHOWCASE --> DAP
    SHOWCASE --> JSON
```

### Creation and Functional Normalization Pipeline

```mermaid
sequenceDiagram
    autonumber
    actor Client as Consumer / API
    participant Factory as VO.Create(rawString)
    participant Pipeline as StringPipeline.Required
    participant Checksum as Checksum Algorithm (Mod-11)
    participant Result as Result<T>

    Client->>Factory: Create("  001-1234567-3  ")
    Factory->>Pipeline: Normalize (Trim, Upper)
    Pipeline->>Checksum: Validate Invariants & Checksum
    alt Valid Input
        Checksum-->>Pipeline: Valid
        Pipeline->>Result: Success(new VO(normalized))
        Result-->>Client: Result<T>.Success(instance)
    else Invalid Input
        Checksum-->>Pipeline: Checksum Failed
        Pipeline->>Result: Failure(Error.Validation(...))
        Result-->>Client: Result<T>.Failure(error)
    end
```

---

### Value Object Lifecycle States

```mermaid
stateDiagram-v2
    [*] --> Unvalidated : Raw primitive received
    Unvalidated --> Normalized : StringPipeline.Trim/Upper/Lower
    Normalized --> ChecksumPending : Has regulatory checksum
    Normalized --> InvariantsPending : No checksum needed
    ChecksumPending --> Valid : Mod-10/Mod-11 passes
    ChecksumPending --> Invalid : Checksum mismatch
    InvariantsPending --> Valid : Length/Regex/Range passes
    InvariantsPending --> Invalid : Invariant violation
    Valid --> Instantiated : Result&lt;T&gt;.Success(new VO(value))
    Invalid --> ErrorResult : Result&lt;T&gt;.Failure(Error.Validation)
    Instantiated --> Persisted : EF Core ValueConverter / Dapper TypeHandler
    Instantiated --> Serialized : System.Text.Json Converter
    Instantiated --> Logged : ToString() → masked if [SensitiveData]
    Persisted --> Instantiated : Deserialized from DB
    Serialized --> Instantiated : Deserialized from JSON
    ErrorResult --> [*] : Caller handles failure
```

---

### Enterprise Invoice Processing Flow

```mermaid
sequenceDiagram
    autonumber
    actor Client as API Client
    participant Domain as Domain Service
    participant Money as Money VO
    participant Discount as DiscountRate VO
    participant Tax as TaxRate VO
    participant EF as EF Core 10
    participant DB as SQLite / PostgreSQL

    Client->>Domain: PlaceOrder(listPrice, discountPct, taxPct)
    Domain->>Money: Money.Create(listPrice, "USD")
    Money-->>Domain: Result<Money>.Success

    Domain->>Discount: DiscountRate.Create(discountPct)
    Discount-->>Domain: Result<DiscountRate>.Success

    Domain->>Tax: TaxRate.Create(taxPct)
    Tax-->>Domain: Result<TaxRate>.Success

    Domain->>Discount: ApplyTo(listMoney) → netPrice
    Discount-->>Domain: netPrice: Money

    Domain->>Tax: CalculateTax(netPrice) → taxAmount
    Tax-->>Domain: taxAmount: Money

    Domain->>Money: netPrice + taxAmount → totalPrice
    Money-->>Domain: totalPrice: Money

    Domain->>EF: SaveChanges(order with VOs)
    EF->>DB: INSERT with primitive conversions via ValueConverters
    DB-->>EF: Persisted
    EF-->>Domain: Success
    Domain-->>Client: OrderConfirmation(totalPrice)
```

---

### Money Arithmetic Pipeline

```mermaid
graph LR
    Raw["Raw: decimal + string"] --> Create["Money.Create()"]
    Create --> Valid{Valid?}
    Valid -- No --> Error["Result.Failure(Error)"]
    Valid -- Yes --> VO["Money struct (Amount + Currency)"]
    VO --> Ops["Arithmetic Operators (+, -, *)"]
    VO --> Pct["ApplyPercentage(Percentage)"]
    VO --> Alloc["Allocate(ratios...)"]
    VO --> Dist["Distribute(n parts)"]
    VO --> Neg["Negate() / Abs()"]
    VO --> Round["Round() / RoundCommercial()"]
    VO --> Cmp["Compare (IsGreaterThan etc.)"]
    VO --> Fmt["TryFormat(Span)"]
    Alloc --> Parts["Money[] (Fowler no penny-loss)"]
    Dist  --> EqParts["Money[] (equal, leading gets extra)"]
    Pct   --> Result2["Money (calculated amount)"]
    Ops   --> Result3["Money (same currency)"]
    Round --> Result4["Money (banker or commercial)"]
    Fmt   --> Buffer["Stack char buffer (zero-alloc)"]
```

---

## 8. Best Practices and FAQ

### 1. Why is `readonly record struct` chosen over `class` for numeric VOs?
**Answer:** In high-throughput enterprise systems processing hundreds of thousands of transactions per second, heap-allocated objects create significant Garbage Collector pressure (Gen0/Gen1 churn). Structs reside on the execution stack or inline within containing objects, delivering **zero heap allocations**.

### 2. How does `[SensitiveData]` protect PII information?
**Answer:** Any value object decorated with `[SensitiveData]` or derived from `StringValueObject` with sensitivity masks its `ToString()` representation (e.g., `PassportNumber` -> `XXXXXXXXX`, `LicenseKey` -> `XXXX-XXXX-6789`). This guarantees structured logging systems (Serilog, OpenTelemetry, Application Insights) never leak sensitive plaintext data.

---

## 9. API vs Showcase Synchronization Matrix

| Public API Component | Showcase Coverage | Level | Status |
|---|:---:|:---:|:---:|
| Base Abstractions (`IValueObject`, `IValueObject<TSelf>`, `SingleValueObject`, `StringValueObject`, `ValueObject`) | 100% | Levels 00, 06, 08 | **Synchronized** |
| Attributes (`SensitiveDataAttribute`, `ValueObjectAttribute`, `RegulatoryRuleAttribute`) | 100% | Levels 04, 08 | **Synchronized** |
| `DomainException.ThrowIf()` | 100% | Level 06 | **Synchronized** |
| `Range<T>` (Create, Contains(T), Contains(Range), Overlaps, Intersects) | 100% | Levels 02, 09 | **Synchronized** |
| `RangeExtensions` (Duration for DateTimeOffset, Days for DateOnly) | 100% | Level 02 | **Synchronized** |
| `TimeRange` (Create, Contains, Overlaps, Duration, CrossesMidnight) | 100% | Level 02 | **Synchronized** |
| `DateRange` (Create, Contains, Overlaps, DurationInDays) | 100% | Levels 02, 10 | **Synchronized** |
| `BusinessDate` (Create, FromDateTimeOffset, Parse, TryParse, operators) | 100% | Levels 02, 07, 10 | **Synchronized** |
| `Money` full API (all 20+ methods and operators) | 100% | Levels 01, 03, 05, 07 | **Synchronized** |
| `CurrencyCode` (Create, Parse, TryParse) | 100% | Levels 01, 02, 09, 10 | **Synchronized** |
| `Percentage` full API (FromFraction, ValidatePercentage, ApplyTo, Zero/Hundred/Full, IsZero, Parse/TryParse) | 100% | Levels 01, 06, 07 | **Synchronized** |
| `TaxRate` full API (CalculateTax(decimal), CalculateTax(Money), IsExempt, Exempt, Parse/TryParse) | 100% | Levels 02, 03, 05, 07, 10 | **Synchronized** |
| `DiscountRate` full API (CalculateDiscount, ApplyTo(decimal), ApplyTo(Money), IsZero, None, Parse/TryParse) | 100% | Levels 02, 03, 07, 10 | **Synchronized** |
| `ExchangeRate` (Create, Convert(Money), Inverse()) | 100% | Levels 02, 03 | **Synchronized** |
| `Quantity` (Create, Add, Subtract, IsZero, Zero, Parse/TryParse) | 100% | Levels 03, 05, 07 | **Synchronized** |
| `Email` (Create, LocalPart, Domain, Masked(), Parse/TryParse) | 100% | Levels 01, 03, 06, 09 | **Synchronized** |
| `PhoneNumber` (Create, Parse/TryParse) | 100% | Levels 01, 06, 09 | **Synchronized** |
| `FullName` (both Create overloads, FirstName/LastName/MiddleName properties) | 100% | Level 03 | **Synchronized** |
| `Address` (Create, composite VO pattern) | 100% | Levels 03, 10 | **Synchronized** |
| `IValueObject`/`IValueObject<TSelf>` interface contracts | 100% | Level 08 | **Synchronized** |
| Explicit cast `(TValue)vo` on `SingleValueObject<TSelf,TValue>` | 100% | Level 06 | **Synchronized** |
| Comparison operators `<`, `<=`, `>`, `>=` on struct VOs | 100% | Levels 02, 06 | **Synchronized** |
| 40+ Universal String VOs (SKU, Barcode, TenantCode, etc.) | 100% | Levels 03, 10 | **Synchronized** |
| Fiscal Satellites (DO, MX, CO, AR, CL, PE) | 100% | Level 04 | **Synchronized** |
| `ValueObjectTypeHandler.Register<TVO,TPrimitive>()` | 100% | Level 09 | **Synchronized** |
| `ValueObjectTypeHandler.RegisterStruct<TVO,TPrimitive>()` | 100% | Level 09 | **Synchronized** |
| `SingleValueObjectValueConverter<TVO,TValue>` (default + factory ctor) | 100% | Level 09 | **Synchronized** |
| `StringValueObjectValueConverter<TVO>` (default + factory ctor) | 100% | Level 09 | **Synchronized** |
| `ConfigureDomainValueObjects()` extension | 100% | Level 09 | **Synchronized** |
| `RangeJsonConverter<T>` | 100% | Level 09 | **Synchronized** |
| `StringValueObjectJsonConverter<TSelf>` (extensible base) | 100% | Level 08 | **Synchronized** |
| `SingleValueObjectJsonConverter<TSelf,TValue>` (extensible base) | 100% | Level 08 | **Synchronized** |
| `ValueObjectDomainPrimitiveExtensions` (ToDomainPrimitive/ToStrongId) | 100% | Level 10 | **Synchronized** |
| `DomainPrimitiveErrorExtensions` (ToError/ToPrimitiveError) | 100% | Level 10 | **Synchronized** |

---

## 10. Quality Criteria and Verification

- **Clean Compilation:** 0 errors, 0 warnings (`TreatWarningsAsErrors=true`).
- **Automated Tests:** 918 tests passing across all packages.
- **Showcase Runtime:** Execution verified with exit code 0 across all 11 levels (0–10).
- **Domain Purity:** 100% compliance with `AGENTS.md` and repository architectural invariants.
- **Last Synchronized:** 2026-08-24 — All public APIs in Core Library and Infrastructure are covered by at least one executable example.
