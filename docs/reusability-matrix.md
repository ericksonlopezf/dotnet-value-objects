# Cross-System Reusability Matrix

This matrix maps all Universal Value Objects (`EricksonLopez.ValueObjects`) and Fiscal Value Objects across core enterprise application modules:

| Value Object | C# Type | ERP | POS | E-Invoice | Payroll | Inventory | Manufacturing | CRM | SaaS Multi-tenant | HR | Financials | Audit / Log |
|---|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|
| **Personal & Identity** | | | | | | | | | | | | |
| `FirstName` | `record class` | ✅ | ✅ | ✅ | ✅ | — | — | ✅ | ✅ | ✅ | — | ✅ |
| `MiddleName` | `record class` | ✅ | — | — | ✅ | — | — | ✅ | — | ✅ | — | — |
| `LastName` | `record class` | ✅ | ✅ | ✅ | ✅ | — | — | ✅ | ✅ | ✅ | — | ✅ |
| `FullName` | `record class` | ✅ | ✅ | ✅ | ✅ | — | — | ✅ | ✅ | ✅ | — | ✅ |
| `DisplayName` | `record class` | ✅ | ✅ | — | — | — | — | ✅ | ✅ | ✅ | — | ✅ |
| `NationalId` | `record class` | ✅ | — | ✅ | ✅ | — | — | ✅ | — | ✅ | — | ✅ |
| `PassportNumber` | `record class` | ✅ | — | ✅ | ✅ | — | — | ✅ | — | ✅ | — | ✅ |
| `CreatedBy` | `record class` | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| `ModifiedBy` | `record class` | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| `DeletedBy` | `record class` | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| **Contact & Communication** | | | | | | | | | | | | |
| `Email` | `record struct` | ✅ | ✅ | ✅ | ✅ | — | — | ✅ | ✅ | ✅ | ✅ | ✅ |
| `PhoneNumber` | `record struct` | ✅ | ✅ | ✅ | ✅ | — | — | ✅ | — | ✅ | — | — |
| `WebsiteUrl` | `record class` | ✅ | — | — | — | — | — | ✅ | ✅ | — | — | — |
| `Subject` | `record class` | ✅ | — | — | — | — | — | ✅ | ✅ | — | — | ✅ |
| `MessageBody` | `record class` | ✅ | — | — | — | — | — | ✅ | ✅ | — | — | ✅ |
| **Organizational & Codes** | | | | | | | | | | | | |
| `CompanyName` | `record class` | ✅ | ✅ | ✅ | — | — | — | ✅ | ✅ | — | ✅ | ✅ |
| `DepartmentName` | `record class` | ✅ | — | — | ✅ | — | ✅ | — | — | ✅ | — | — |
| `PositionTitle` | `record class` | ✅ | — | — | ✅ | — | — | — | — | ✅ | — | — |
| `TenantCode` | `record class` | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| `EmployeeCode` | `record class` | ✅ | ✅ | — | ✅ | — | ✅ | — | — | ✅ | — | ✅ |
| `CustomerCode` | `record class` | ✅ | ✅ | ✅ | — | — | — | ✅ | — | — | ✅ | ✅ |
| `SupplierCode` | `record class` | ✅ | — | ✅ | — | ✅ | ✅ | — | — | — | ✅ | ✅ |
| `WarehouseCode` | `record class` | ✅ | ✅ | — | — | ✅ | ✅ | — | — | — | — | — |
| `SalesChannelCode` | `record class` | ✅ | ✅ | ✅ | — | — | — | ✅ | — | — | — | — |
| **Document & Inventory** | | | | | | | | | | | | |
| `DocumentNumber` | `record class` | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | — | ✅ | ✅ | ✅ |
| `ReferenceNumber` | `record class` | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | — | — | ✅ | ✅ |
| `OrderNumber` | `record class` | ✅ | ✅ | ✅ | — | ✅ | ✅ | ✅ | — | — | ✅ | ✅ |
| `ReceiptNumber` | `record class` | ✅ | ✅ | ✅ | — | — | — | — | — | — | ✅ | ✅ |
| `BatchNumber` | `record class` | ✅ | — | — | — | ✅ | ✅ | — | — | — | — | ✅ |
| `SerialNumber` | `record class` | ✅ | ✅ | — | — | ✅ | ✅ | — | — | — | — | ✅ |
| `Barcode` | `record class` | ✅ | ✅ | — | — | ✅ | ✅ | — | — | — | — | — |
| `SKU` | `record class` | ✅ | ✅ | ✅ | — | ✅ | ✅ | — | — | — | — | — |
| `LicenseKey` | `record class` | — | — | — | — | — | — | — | ✅ | — | — | ✅ |
| `FileName` | `record class` | ✅ | — | ✅ | ✅ | — | — | ✅ | ✅ | ✅ | ✅ | ✅ |
| **Finance, Temporal & Math** | | | | | | | | | | | | |
| `Money` | `record struct` | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| `CurrencyCode` | `record struct` | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| `Percentage` | `record struct` | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | — |
| `TaxRate` | `record struct` | ✅ | ✅ | ✅ | — | — | — | — | — | — | ✅ | — |
| `DiscountRate` | `record struct` | ✅ | ✅ | ✅ | — | — | — | ✅ | — | — | ✅ | — |
| `Quantity` | `record struct` | ✅ | ✅ | ✅ | — | ✅ | ✅ | — | — | — | — | — |
| `ExchangeRate` | `record struct` | ✅ | ✅ | ✅ | — | — | — | — | — | — | ✅ | — |
| `BusinessDate` | `record struct` | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| `DateRange` | `record struct` | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| `TimeRange` | `record class` | ✅ | ✅ | — | ✅ | — | ✅ | — | — | ✅ | — | — |
| `Range<T>` | `record struct` | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
