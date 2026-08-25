# AI Agent Rules for EricksonLopez.ValueObjects

> This repository contains the enterprise modular Value Objects ecosystem in .NET 10 (C# 13).

---

## 1. Architecture and Design Principles

1. **Absolute Immutability (NON-NEGOTIABLE)**:
   - Every Value Object must be immutable (`readonly record struct` or `sealed record : StringValueObject<TSelf>` / `SingleValueObject<TValue, TSelf>`).
   - Constructors are private. Instances are exclusively instantiated via static factory methods `Create(...)` returning `Result<T>`.
   - Public setters or state-mutating methods are strictly prohibited.

2. **Zero-Allocation Abstractions**:
   - `Result<T>` and `Result` are `readonly struct`.
   - Throwing exceptions for standard domain business flow is prohibited. Always use `Result<T>.Success(...)` or `Result<T>.Failure(...)`.

3. **Value Object Hierarchy and Structure**:
   - **`readonly record struct`**: Required for VOs based on primitive types/structs (`decimal`, `int`, `DateOnly`, `Guid`). Examples: `Money`, `CurrencyCode`, `Percentage`, `TaxRate`, `DiscountRate`, `Quantity`, `BusinessDate`, `DateRange`.
   - **`sealed record : StringValueObject<TSelf>`**: Required for single-text-string VOs leveraging `StringPipeline` for normalization and validation. Examples: `Country`, `PostalCode`, `CompanyName`, `DisplayName`, `DocumentNumber`, `NationalId`, `SKU`, `TenantCode`.
   - **`sealed record : ValueObject`**: Required for composite VOs with multiple complex properties. Examples: `Address`, `TimeRange`.

4. **Sensitive Data Protection**:
   - Every Value Object containing PII, credentials, or protected identifiers must be decorated with `[SensitiveData(mask: "...")]` to ensure `ToString()` masks sensitive values in logs and distributed traces.

5. **Domain Purity**:
   - The `EricksonLopez.ValueObjects` and `EricksonLopez.ValueObjects.Fiscal.*` projects are pure domain libraries (free of dependencies on Dapper, Entity Framework, or ASP.NET Core).
   - Dapper TypeHandlers reside in `EricksonLopez.ValueObjects.Dapper`.
   - JSON serializers reside in `EricksonLopez.ValueObjects.Serialization.Json`.

6. **One Type Per File (NON-NEGOTIABLE)**:
   - Each file must contain exactly one public type. The filename must match the type name exactly.

7. **Exhaustive XML Documentation**:
   - Every public type and member must include comprehensive XML documentation comments (`/// <summary>`, `<para><b>Rules:</b></para>`, `<param>`, `<returns>`, `<example>`).

8. **Build and Test Verification**:
   - After any modification, execute `dotnet build` and `dotnet test` to ensure clean compilation with 0 warnings and 100% test pass rate.
