# Migration Guide

This guide outlines breaking changes and migration procedures across versions of `EricksonLopez.ValueObjects`.

---

## 1.0.0 (Initial Release)

### 1. Result Pattern Adoption (ADR-001)

Direct constructor invocation is prohibited and enforced at compile time by Roslyn Analyzer `ELVO001`.

```csharp
// Functional factory returning Result<T>
var result = Email.Create("user@example.com");
if (result.IsSuccess)
{
    Email email = result.Value;
}
```

### 2. Zero-Allocation `Range<T>` (ADR-010)

`Range<T>` is implemented as a `readonly record struct`. Use `Range<T>?` when nullable semantics are required.

```csharp
Range<int>? range = null; // Explicitly nullable struct
```

### 3. Base Hierarchy & DomainPrimitives Integration (ADR-007 / ADR-008)

`ValueObject` base record inherits from `EricksonLopez.DomainPrimitives.ValueObject`. For bidirectional bridging:

```csharp
using EricksonLopez.ValueObjects.DomainPrimitives;

var emailPrimitive = email.ToDomainPrimitive();
var strongId = tenantId.ToStrongId();
```

### 4. Native AOT Sensitive Data Redaction (ADR-004 / ADR-013)

Value Objects containing sensitive data utilize virtual property overrides to achieve 100% Native AOT trimming compliance without runtime reflection:

```csharp
[SensitiveData(mask: "****")]
public sealed record NationalId : StringValueObject<NationalId>
{
    protected override bool IsSensitive => true;
    protected override string Mask => "****";
}
```

### 5. Entity Framework Core Configuration

Use the `ConfigureDomainValueObjects()` extension in `OnModelCreating` conventions:

```csharp
protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
{
    configurationBuilder.ConfigureDomainValueObjects();
}
```

---

## Capabilities & Architectural Specifications Summary

| Component | Architecture / Pattern | Invariants |
|---|---|---|
| Core Factories | Static `Create(...)` returning `Result<T>` | Private constructors mandated by Roslyn Analyzer `ELVO001` |
| Continuous Intervals | `Range<T>` (`readonly record struct`) | Non-nullable value type, zero heap allocation |
| Multi-Currency Arithmetic | `Money` (`readonly record struct`) | Martin Fowler proportional allocation, strict currency match |
| EF Core Integration | `ValueConverter<TVO, TRaw>` | Full trimming annotations for Native AOT execution |
| Dapper Persisters | `SqlMapper.TypeHandler` | High-throughput struct and scalar type handlers |
| Domain Primitives Bridge | `ToDomainPrimitive()`, `ToStrongId()` | Zero-overhead bidirectional translation |

