# Corporate Value Objects Design & Invariant Catalog

> **Ecosystem**: `EricksonLopez.ValueObjects`  
> **Runtime**: .NET 10 (C# 13 / C# 14)  
> **Paradigms**: Domain-Driven Design (DDD), Zero-Allocation, Native AOT-First, Result Pattern

---

## 1. Architectural Motivation & Problem Statement

Enterprise systems (ERP, POS, Billing, Inventory, Payroll, HR, CRM, SaaS multi-tenancy, Financial Management) consistently process the same fundamental primitives: person names, corporate designations, codes, reference numbers, monetary amounts, percentages, tax rates, business dates, time intervals, national identifiers, and audit metadata.

The most severe operational risk in enterprise applications is not storing text (`string`) or decimals (`decimal`), but rather that disparate microservices or modules define lengths, formatting, whitespace normalization, and validation rules inconsistently.

### Common Risks Mitigated by Corporate Value Objects:

1. **Format Fragmentation**: Module A accepts 10-digit phone numbers with hyphens, while Module B requires E.164 formatting with leading plus signs.
2. **Rounding Drift & Penny Leaks**: Performing naive decimal divisions on monetary values introduces rounding errors across distributed ledgers.
3. **Data Leaks (PII)**: Logging raw strings leaks credentials, tax IDs, and sensitive customer data into observability tools.
4. **Invalid Domain State**: Entities holding `null` or empty strings that bypass business validation due to primitive obsession.

---

## 2. Invariant & Validation Rules by Corporate Category

### 2.1 Identity & Personal Domain

| Value Object | C# Representation | Minimum Length | Maximum Length | Invariants & Normalization |
|---|---|:---:|:---:|---|
| `FirstName` | `sealed record : StringValueObject<FirstName>` | 1 | 80 | Collapses inner whitespace, trims. Allows letters, spaces, apostrophes, hyphens. |
| `MiddleName` | `sealed record : StringValueObject<MiddleName>` | 1 | 50 | Optional in builders, trimmed, collapses whitespace. |
| `LastName` | `sealed record : StringValueObject<LastName>` | 1 | 50 | Collapses inner whitespace, trims. |
| `FullName` | `sealed record : ValueObject` | — | — | Composed of `FirstName`, optional `MiddleName`, `LastName`. |
| `DisplayName` | `sealed record : StringValueObject<DisplayName>` | 1 | 100 | Trimmed, non-empty. |
| `NationalId` | `sealed record : StringValueObject<NationalId>` | 4 | 40 | Uppercase alphanumeric with spaces/periods/underscores/slashes/hyphens. Decorated with `[SensitiveData]` — `ToString()` returns `***`. |
| `PassportNumber` | `sealed record : StringValueObject<PassportNumber>` | 5 | 20 | Alphanumeric, uppercase, decorated with `[SensitiveData]`. |
| `CreatedBy` | `sealed record : StringValueObject<CreatedBy>` | 1 | 100 | Immutable audit trail user/system identifier. |
| `ModifiedBy` | `sealed record : StringValueObject<ModifiedBy>` | 1 | 100 | Immutable audit trail modifier identifier. |
| `DeletedBy` | `sealed record : StringValueObject<DeletedBy>` | 1 | 100 | Soft-deletion audit identifier. |

### 2.2 Contact & Communication Domain

| Value Object | C# Representation | Format / Regex | Invariants & Normalization |
|---|---|---|---|
| `Email` | `readonly record struct` | RFC 5322 standard | Lowercased, trimmed, validates domain and username. |
| `PhoneNumber` | `readonly record struct` | E.164 international format | Requires `+` country code prefix, 8 to 15 digits after `+`. |
| `WebsiteUrl` | `sealed record : StringValueObject<WebsiteUrl>` | URI RFC 3986 | Validates `http://` or `https://` absolute URI scheme. |
| `Subject` | `sealed record : StringValueObject<Subject>` | 1 to 200 chars | Collapses inner whitespace, trimmed. |
| `MessageBody` | `sealed record : StringValueObject<MessageBody>` | 1 to 10,000 chars | Preserves newlines, trims edge whitespace. |

### 2.3 Organizational & Tenant Codes

| Value Object | C# Representation | Length | Invariants & Normalization |
|---|---|:---:|---|
| `CompanyName` | `sealed record : StringValueObject<CompanyName>` | 1 to 150 | Collapses whitespace, trimmed. |
| `DepartmentName` | `sealed record : StringValueObject<DepartmentName>` | 1 to 100 | Collapses whitespace, trimmed. |
| `PositionTitle` | `sealed record : StringValueObject<PositionTitle>` | 1 to 100 | Collapses whitespace, trimmed. |
| `TenantCode` | `sealed record : StringValueObject<TenantCode>` | 2 to 30 | Uppercase alphanumeric + hyphens (`^[A-Z0-9_-]+$`). |
| `EmployeeCode` | `sealed record : StringValueObject<EmployeeCode>` | 1 to 30 | Uppercase alphanumeric. |
| `CustomerCode` | `sealed record : StringValueObject<CustomerCode>` | 1 to 30 | Uppercase alphanumeric. |
| `SupplierCode` | `sealed record : StringValueObject<SupplierCode>` | 1 to 30 | Uppercase alphanumeric. |
| `WarehouseCode` | `sealed record : StringValueObject<WarehouseCode>` | 1 to 20 | Uppercase alphanumeric. |
| `SalesChannelCode` | `sealed record : StringValueObject<SalesChannelCode>` | 1 to 20 | Uppercase alphanumeric. |

### 2.4 Document & Inventory

| Value Object | C# Representation | Invariants & Validation Rules |
|---|---|---|
| `DocumentNumber` | `sealed record : StringValueObject<DocumentNumber>` | Non-empty alphanumeric document identifier (1 to 50 chars). |
| `ReferenceNumber` | `sealed record : StringValueObject<ReferenceNumber>` | External transaction tracking reference (1 to 50 chars). |
| `OrderNumber` | `sealed record : StringValueObject<OrderNumber>` | Commercial purchase or sales order number (1 to 50 chars). |
| `ReceiptNumber` | `sealed record : StringValueObject<ReceiptNumber>` | Proof of payment/receipt number (1 to 50 chars). |
| `BatchNumber` | `sealed record : StringValueObject<BatchNumber>` | Manufacturing batch / lot tracking identifier (1 to 50 chars). |
| `SerialNumber` | `sealed record : StringValueObject<SerialNumber>` | Hardware/item unique serial identifier (1 to 100 chars). |
| `Barcode` | `sealed record : StringValueObject<Barcode>` | EAN-8, EAN-13, UPC-A, UPC-E, Code 128 barcode (3 to 50 chars). |
| `SKU` | `sealed record : StringValueObject<SKU>` | Stock Keeping Unit code (1 to 30 chars, uppercase alphanumeric). |
| `LicenseKey` | `sealed record : StringValueObject<LicenseKey>` | Software license key or activation code (`[SensitiveData]`). |
| `FileName` | `sealed record : StringValueObject<FileName>` | Validates illegal file path characters (`Path.GetInvalidFileNameChars()`). |

### 2.5 Finance, Quantitative & Temporal Arithmetic

#### `Money` & Martin Fowler's Proportional Allocation Algorithm

The `Money` type is an immutable `readonly record struct` representing a `decimal Amount` and a `CurrencyCode Currency`.

```csharp
var price = Money.Create(100.00m, CurrencyCode.USD).Value;

// Proportional allocation across 3 entities in ratios 5 : 3 : 2
Money[] shares = price.Allocate(5, 3, 2);
// shares[0] == $50.00 USD
// shares[1] == $30.00 USD
// shares[2] == $20.00 USD

// Odd allocation: $100 allocated 1 : 1 : 1 without losing the remaining cent:
Money[] oddShares = price.Allocate(1, 1, 1);
// oddShares[0] == $33.34 USD (remainder assigned to first share)
// oddShares[1] == $33.33 USD
// oddShares[2] == $33.33 USD
// Sum of shares == $100.00 USD exactly!
```

#### `Range<T>` Continuous Interval Logic

`Range<T>` is an immutable `readonly record struct` modeling mathematical or temporal intervals `[Start .. End]`:

```csharp
var rangeA = Range<int>.Create(10, 30).Value;
var rangeB = Range<int>.Create(20, 50).Value;

bool overlaps = rangeA.Overlaps(rangeB); // true
bool contains = rangeA.Contains(15);    // true

if (rangeA.Intersects(rangeB, out var intersection))
{
    Console.WriteLine(intersection); // "[20 .. 30]"
}
```

---

## 3. Extensibility & Authoring New Value Objects

To implement a new Value Object in compliance with the architecture:

1. **For Scalar Strings**: Inherit from `StringValueObject<TSelf>` and use the static `StringPipeline.Required` API:
   ```csharp
   public sealed record TrackingNumber : StringValueObject<TrackingNumber>
   {
       private TrackingNumber(string value) : base(value) { }

       /// <summary>
       /// Creates a validated <see cref="TrackingNumber"/> after normalizing to uppercase
       /// and validating the format.
       /// </summary>
       public static Result<TrackingNumber> Create(string? value)
       {
           return StringPipeline.Required(
               value,
               nameof(TrackingNumber),
               minLength: 8,
               maxLength: 24,
               factory: static normalized => new TrackingNumber(normalized),
               normalize: StringPipeline.NormalizeTrimUpper,
               pattern: StringPipeline.CodePattern,
               patternErrorMessage: "Tracking number must be 8–24 uppercase alphanumeric characters.");
       }
   }
   ```
   > **Note**: `StringPipeline` is an `internal` utility class — its static helper methods are the correct
   > extension points, not a fluent builder instance. Do **not** instantiate `StringPipeline` directly.
2. **For Struct Scalars**: Implement as `readonly record struct`:
   ```csharp
   public readonly record struct Weight
   {
       public decimal Kilograms { get; }

       private Weight(decimal kilograms) => Kilograms = kilograms;

       public static Result<Weight> Create(decimal kilograms)
       {
           if (kilograms <= 0)
               return Result<Weight>.Failure(Error.Validation("Weight.MustBePositive", "Weight must be greater than zero."));

           return Result<Weight>.Success(new Weight(kilograms));
       }
   }
   ```
