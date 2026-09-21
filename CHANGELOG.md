# Changelog

All notable changes to the `EricksonLopez.ValueObjects` ecosystem will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).


## [Unreleased]

## [2.0.0] - 2026-09-20

### Breaking Changes

- **BC-001 (Compile-time & Binary Breaking): Converted `TimeRange` from record class to `readonly record struct`.**
  - *Previous Behavior:* `TimeRange` was a reference type (`public sealed record TimeRange : ValueObject`), nullable by default reference semantics, allocating on the managed heap.
  - *Current Behavior:* `TimeRange` is a zero-allocation value type (`public readonly record struct TimeRange : IValueObject<TimeRange>, IComparable<TimeRange>, IComparable, IParsable<TimeRange>, ISpanParsable<TimeRange>`).
  - *Affected Consumers:* Callers checking `timeRange == null`, storing `TimeRange?` expecting reference nullability, or relying on `ValueObject` polymorphism. Binaries compiled against v1.0.0 will throw `TypeLoadException` without recompilation.
  - *Migration Guidance:* Replace null checks (`timeRange == null`) with `!timeRange.IsInitialized` or `timeRange.IsEmpty`. Recompile consuming assemblies against v2.0.0. Update polymorphic signatures accepting `ValueObject` to accept generic `IValueObject<TimeRange>` or explicit `TimeRange`.

- **BC-002 (Runtime Breaking): Enforced hard 20,000 character limit on `StringValueObject` base constructor.**
  - *Previous Behavior:* `StringValueObject` accepted strings of arbitrary length, relying exclusively on derived factory methods to enforce length limits.
  - *Current Behavior:* `StringValueObject` protected constructor immediately throws `ArgumentException` if the input string exceeds 20,000 characters to prevent Large Object Heap (LOH) exhaustion and memory exhaustion DoS.
  - *Affected Consumers:* Custom value objects or test harnesses initializing `StringValueObject` with payloads exceeding 20,000 characters.
  - *Migration Guidance:* Validate string lengths before constructor invocation using domain-specific `Create(...)` factory methods, which return `Result<T>` instead of throwing.

- **BC-003 (Behavioral & Data Breaking): Enforced mandatory Unicode Normalization FormC across all `StringValueObject` and `StringPipeline` instances.**
  - *Previous Behavior:* Input strings preserved their original Unicode normalization form (e.g. FormD decomposed combining characters or ligatures).
  - *Current Behavior:* `StringValueObject` and `StringPipeline.Execute` automatically convert strings to canonical Unicode Normalization FormC (`.Normalize(NormalizationForm.FormC)`).
  - *Affected Consumers:* Consumers performing raw byte comparisons, hash computations, or database lookups against external systems using decomposed Unicode (FormD) representations.
  - *Migration Guidance:* Ensure external systems, databases, and checksum verifiers normalize input text to Unicode FormC before computing signatures or comparing raw string representations.

- **BC-004 (Behavioral & Validation Breaking): Classified Unicode Format (`Cf`) and Unassigned (`Cn`) characters as invalid control characters in `StringPipeline`.**
  - *Previous Behavior:* `StringPipeline.ContainsControlCharacters` only checked `char.IsControl()`, permitting invisible formatting characters (e.g. Zero-Width Spaces U+200B, Soft Hyphens U+00AD, RTL marks U+200E/U+200F).
  - *Current Behavior:* Characters in `UnicodeCategory.Format` or `UnicodeCategory.OtherNotAssigned` are identified as control characters, causing `StringPipeline.Execute` to return validation failure (`{fieldName}.ControlCharacters`).
  - *Affected Consumers:* Workflows ingesting rich text or copy containing zero-width spaces, soft hyphens, or bidirectional formatting marks into string value objects.
  - *Migration Guidance:* Call `StringPipeline.StripFormatCharacters(rawText)` prior to passing text into value object factories, or sanitize input data upstream to remove invisible format markers.

- **BC-005 (Runtime Breaking): `Money.Zero(CurrencyCode)` throws `DomainException` on uninitialized currency.**
  - *Previous Behavior:* `Money.Zero(default(CurrencyCode))` permitted creating a zero monetary amount with an empty/uninitialized currency.
  - *Current Behavior:* Throws `DomainException("Cannot create Money with uninitialized currency.")` if `!currency.IsInitialized`.
  - *Affected Consumers:* Callers passing default structs or uninitialized `CurrencyCode` instances into `Money.Zero(...)`.
  - *Migration Guidance:* Ensure `CurrencyCode` is created through `CurrencyCode.Create("USD")` or valid constants (e.g. `CurrencyCode.USD`) before passing to `Money.Zero()`. Use `Money.ZeroUsd` for default zero amounts.

- **BC-006 (Runtime Breaking): `Money` arithmetic and comparison operators throw `DomainException` on uninitialized currency.**
  - *Previous Behavior:* Operating on two `default(Money)` instances evaluated `Currency != other.Currency` as false (both were empty strings) and allowed addition, subtraction, or comparisons to proceed.
  - *Current Behavior:* `EnsureSameCurrency` throws `DomainException("Cannot operate on Money with uninitialized currency.")` if either operand has an uninitialized currency.
  - *Affected Consumers:* Code relying on default struct state for accumulator initialization (e.g. `Money total = default; total += item;`).
  - *Migration Guidance:* Initialize accumulators explicitly using `Money.Zero(currency)` rather than relying on `default(Money)`. Check `money.IsInitialized` before executing arithmetic operations.

- **BC-007 (Behavioral & Formatting Breaking): `Money.ToString()` adopts currency-aware decimal place formatting.**
  - *Previous Behavior:* Default formatting string was hardcoded to `"N2"`, formatting all currencies with 2 decimal places (e.g. `"100.00 JPY"`).
  - *Current Behavior:* Formats amounts using `Currency.DecimalPlaces` (e.g. `"100 JPY"` for Japanese Yen, `"100.500 KWD"` for Kuwaiti Dinar).
  - *Affected Consumers:* Consumers parsing or asserting on the exact string representation of `Money.ToString()`.
  - *Migration Guidance:* Pass explicit format strings (e.g. `money.ToString("N2", CultureInfo.InvariantCulture)`) if fixed 2-decimal formatting is required regardless of currency specification.

- **BC-008 (Behavioral Breaking): `Money.Create` and `Money.Add` reject uninitialized currency.**
  - *Previous Behavior:* `Money.Create(amount, default)` created a `Money` instance without error. `Money.Add` permitted additions without verifying currency initialization.
  - *Current Behavior:* `Money.Create` returns `Result.Failure("Money.InvalidCurrency")`. `Money.Add` returns `Result.Failure("Money.UninitializedCurrency")`.
  - *Affected Consumers:* Calling code passing uninitialized `CurrencyCode` or `Money` structs to factories or domain arithmetic.
  - *Migration Guidance:* Verify `currency.IsInitialized` prior to creating `Money`, and check the returned `Result<Money>` for success.

- **BC-009 (Behavioral & Validation Breaking): Stricter email validation in `Email.Create`.**
  - *Previous Behavior:* Permitted display-name formats (e.g. `"User <user@example.com>"`) and single-label domains (e.g. `"user@localhost"`) via standard `MailAddress.TryCreate`.
  - *Current Behavior:* Rejects carriage returns, newlines, angle brackets (`<`, `>`), local parts > 64 chars, domain parts > 255 chars, and domains without at least one period (`.`).
  - *Affected Consumers:* Systems using intranet or local test email addresses without top-level domains, or passing formatted name-and-address strings.
  - *Migration Guidance:* Extract pure RFC 5321 mailbox addresses (e.g. `user@example.com`) without display names. Ensure test environments use fully qualified domain names (e.g. `user@localhost.localdomain`).

- **BC-010 (Behavioral Breaking): Restricting `PhoneNumber` digits to ASCII digits (`char.IsAsciiDigit`).**
  - *Previous Behavior:* Evaluated `char.IsDigit`, accepting international Unicode decimal digits (such as Arabic-Indic or full-width digits).
  - *Current Behavior:* Requires strict ASCII digits (`'0'` through `'9'`) following the leading `+` prefix.
  - *Affected Consumers:* Applications ingesting internationalized phone numbers represented with non-ASCII Unicode numerals.
  - *Migration Guidance:* Canonicalize non-ASCII numerals to standard ASCII digits `0-9` before creating `PhoneNumber` instances.

- **BC-011 (Runtime Breaking): `ValueObjectIncrementalGenerator` explicit cast operator throws `InvalidCastException` instead of `InvalidOperationException`.**
  - *Previous Behavior:* Casting an invalid primitive to a generated Value Object executed `Create(value).Value`, which threw `InvalidOperationException` when accessing `.Value` on a failed `Result<T>`.
  - *Current Behavior:* Generator emits an explicit check throwing `InvalidCastException($"Cannot cast '{value}' to {typeName}: {result.Error.Description}")`.
  - *Affected Consumers:* Callers catching `InvalidOperationException` when executing explicit casts (e.g. `(Email)rawString`).
  - *Migration Guidance:* Update catch blocks to catch `InvalidCastException`, or preferably replace explicit casting with safe `Create(...)` or `TryParse(...)` methods.

- **BC-012 (Compile-time Breaking): `RegulatoryRuleAttribute` primary namespace relocated to `EricksonLopez.ValueObjects`.**
  - *Previous Behavior:* Declared solely in `namespace EricksonLopez.ValueObjects.Attributes`.
  - *Current Behavior:* Primary definition moved to `namespace EricksonLopez.ValueObjects`. A compatibility forwarder alias remains in `EricksonLopez.ValueObjects.Attributes`.
  - *Affected Consumers:* Code files containing both `using EricksonLopez.ValueObjects;` and `using EricksonLopez.ValueObjects.Attributes;` encounter compiler error `CS0104: 'RegulatoryRuleAttribute' is an ambiguous reference`.
  - *Migration Guidance:* Remove `using EricksonLopez.ValueObjects.Attributes;` from consumer files, as `RegulatoryRuleAttribute` is now directly accessible from the root domain namespace.

- **BC-013 (Configuration & Compile-time Breaking): `<ImplicitUsings>` disabled in `Directory.Build.props`.**
  - *Previous Behavior:* `<ImplicitUsings>enable</ImplicitUsings>` automatically imported common BCL namespaces (`System`, `System.Collections.Generic`, `System.Linq`, etc.).
  - *Current Behavior:* `<ImplicitUsings>disable</ImplicitUsings>` requires explicit namespace declarations.
  - *Affected Consumers:* Consumers building projects that inherit repo-level `Directory.Build.props` without explicitly specifying required `using` directives.
  - *Migration Guidance:* Add explicit `using` statements to files, or configure `<ImplicitUsings>enable</ImplicitUsings>` at the individual project level if desired.

- **BC-014 (Integration & Dependency Breaking): Major version bump of `EricksonLopez.DomainPrimitives.Abstractions` from `1.0.0` to `2.0.0`.**
  - *Previous Behavior:* Referenced `EricksonLopez.DomainPrimitives.Abstractions` version `1.0.0`.
  - *Current Behavior:* References `EricksonLopez.DomainPrimitives.Abstractions` version `2.0.0`.
  - *Affected Consumers:* Consuming applications binding to `EricksonLopez.DomainPrimitives.Abstractions` v1.0.0.
  - *Migration Guidance:* Upgrade package reference for `EricksonLopez.DomainPrimitives.Abstractions` to `>= 2.0.0`.

- **BC-015 (Compile-time Breaking with WarningsAsErrors): `ValueObjectConstructorAnalyzer` (`ELVO001`) flags implicitly declared parameterless constructors.**
  - *Previous Behavior:* Implicitly generated default constructors were ignored (`if (constructor.IsImplicitlyDeclared) continue;`).
  - *Current Behavior:* Analyzer reports diagnostic `ELVO001` on reference-type Value Objects lacking an explicit constructor.
  - *Affected Consumers:* Consuming code defining classes decorated with `[ValueObject]` or implementing `IValueObject` without an explicit private/protected constructor in projects with `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>`.
  - *Migration Guidance:* Declare an explicit `private` or `protected` constructor on the Value Object class to enforce creation via static factory methods.

- **BC-016 (Behavioral Breaking): Fiscal Satellites enforce strict input buffer caps (32 chars / 64 bytes).**
  - *Previous Behavior:* `Cedula`, `Rnc`, and `TaxpayerId` dynamically allocated stack memory matching input length (`stackalloc char[value.Length]`). UTF-8 span parsers allocated unbounded stack buffers.
  - *Current Behavior:* Inputs exceeding 32 characters in `Cedula`/`Rnc`/`TaxpayerId` return `Result.Failure("*.InvalidLength")`. UTF-8 span parsers reject inputs exceeding 64 bytes with `false`.
  - *Affected Consumers:* Consumers passing large, padded, or malformed strings expecting full processing or standard truncation.
  - *Migration Guidance:* Sanitize and trim fiscal inputs to valid identifier lengths before invoking validation or parsing methods.

### Added

- **New Universal Value Objects (`EricksonLopez.ValueObjects`)**:
  - `GeoCoordinate` (`readonly record struct`): WGS-84 geographic coordinate encapsulating latitude (-90° to +90°) and longitude (-180° to +180°), with Haversine distance calculations (`DistanceToKilometers`, `DistanceToMeters`), `IParsable`, and `ISpanParsable`.
  - `HierarchyPath` (`record class`): Materialized path value object for hierarchical trees and organizational charts (e.g. `/corp/finance/ap`), with `Depth`, `Parent`, `Append`, `TryAppend`, `IsAncestorOf`, `IsDescendantOf`, and `IsDirectChildOf`.
  - `HmacSha256Hash` (`record class`): Cryptographic HMAC-SHA256 hex-encoded hash (64 lowercase hexadecimal characters) with built-in zero-reflection sensitive data masking.
  - `SwiftBic` (`record class`): ISO 9362 Business Identifier Code (BIC / SWIFT) supporting 8-character (head office) and 11-character (branch) formats, decomposing into `BankCode`, `CountryCode`, `LocationCode`, and `BranchCode`.
  - `VehicleVin` (`record class`): ISO 3779 Vehicle Identification Number (17 alphanumeric characters, excluding I, O, Q) decomposing into `Wmi` (World Manufacturer Identifier), `Vds` (Vehicle Descriptor Section), `Vis` (Vehicle Identifier Section), and `ModelYear`.

- **Dominican Republic Fiscal Satellites (`EricksonLopez.ValueObjects.Fiscal.DominicanRepublic`)**:
  - `EcfAuthSeed` (`record class`): e-CF electronic invoicing authentication seed (Law 32-23 / DGII) with expiration tracking (`IsExpired`, `RemainingTime`), ambient `TimeProvider` support, and deterministic XML serialization.
  - `EcfInformationalSubtotal` (`record class`): Informative subtotal block for e-CF invoices with tolerance checking (`DgiiTolerance = 0.50m`) across taxable amounts, ITBIS, ISC, and non-billable items.
  - `Nss` (`record class`) & `NssChecksum`: Social Security Number (Numero de Seguridad Social - TSS/CNSS) validation with Modulo 10 check digit verification.
  - `ProvinceMunicipality` (`record class`): 5-digit ONE/JCE territorial division code with province and municipality resolution (`ProvinceCode`, `MunicipalityCode`, `ProvinceName`).
  - `DgiiClassification` & `DgiiExpenseType`: Official DGII electronic invoice expense type and classification enumerations.

- **JSON Serialization Extensions (`EricksonLopez.ValueObjects.Serialization.Json`)**:
  - `MoneyJsonConverter`: Dedicated Native AOT converter serializing `Money` as structured JSON (`{"amount": 100.50, "currency": "USD"}`) or string representation (`"100.50 USD"`).
  - `ValueObjectJsonConverterFactory`: Universal `JsonConverterFactory` dynamically resolving and instantiating converters for all `SingleValueObject`, `IParsable`, and `Range<T>` types.
  - `JsonSerializerOptionsExtensions.AddValueObjectConverters()`: Fluent extension method for registering Value Object converters on `JsonSerializerOptions`.

- **Roslyn Analyzers (`EricksonLopez.ValueObjects.Analyzers`)**:
  - `ELVO004` (`ValueObjectDefaultInitializationAnalyzer`): Flags uninitialized struct instantiation via `default(T)` or parameterless `new T()` for value-type Value Objects.

- **Core Extensions & Properties**:
  - `Range<T>`: Added `IsDegenerate`, `IsEmpty`, and `ContainsHalfOpen(T value)` for boundary analysis.
  - `DateRange`: Added `InclusiveDays`, `DaysDifference`, `IsInitialized`, `Parse`, and `TryParse`.
  - `TimeRange`: Added `IsEmpty`, `IsInitialized`, `CompareTo`, comparison operators (`<`, `<=`, `>`, `>=`), `Parse`, and `TryParse`.
  - `Money`: Added `IsInitialized`, `TryMultiply`, `TryApplyPercentage`, `Parse`, and `TryParse`.
  - Struct `IsInitialized` property across all fiscal and core structs: `CurrencyCode`, `BusinessDate`, `ExchangeRate`, `Percentage`, `Cuit`, `Cuil`, `Cbu`, `Cvu`, `Cdi`, `Cae`, `Caea`, `Cai`, `Rut`, `Nit`, `FiscalUuid`, `IdCcp`, `Rfc`, `Ruc`.
  - `Email`: Added `ToMaskedString()` local mailbox masking and `ReadOnlySpan<char>` factory overload.
  - `PhoneNumber`: Added `ReadOnlySpan<char>` factory overload.
  - Multi-targeting support for `net8.0`, `net9.0`, and `net10.0` across fiscal satellites and core projects.

### Changed

- **`Money.Allocate`**: Optimized with `Span<int>` and stackalloc for up to 128 parts to eliminate heap allocations.
- **`SingleValueObjectValueConverter`**: Replaced reflection-based `MethodInfo.Invoke` with compiled delegates (`Delegate.CreateDelegate`) for EF Core conversion pipelines.
- **`RangeJsonConverter`**: Added `reader.Skip()` for unrecognized JSON properties to improve deserialization resilience.
- **`LicenseKey`**: Expanded regex pattern to support 2-segment licenses (`^[A-Z0-9]{4,8}(-[A-Z0-9]{4,8}){1,8}$`), lowering minimum length to 9.
- **`DgiiChecksum`**: Extended Modulo 11 check digit verification to accept check digit `0` or `2` when remainder is `0`, accommodating DGII special tax identifiers.

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

[Unreleased]: https://github.com/ericksonlopezf/dotnet-value-objects/compare/v2.0.0...HEAD
[2.0.0]: https://github.com/ericksonlopezf/dotnet-value-objects/compare/v1.0.0...v2.0.0
[1.0.0]: https://github.com/ericksonlopezf/dotnet-value-objects/releases/tag/v1.0.0


