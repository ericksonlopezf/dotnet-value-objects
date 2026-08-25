# Migration Guide: From NodaMoney to EricksonLopez.ValueObjects

> **Target:** Seamless migration of monetary arithmetic, currency codes, and financial distribution.  
> **Key Benefits:** Zero heap allocations on arithmetic, NativeAOT trimming safety, Roslyn analyzer enforcement, and built-in Martin Fowler lossless allocation.

---

## 1. Concept Mapping Table

| NodaMoney Concept | EricksonLopez.ValueObjects Equivalent | Architectural Advantage |
|---|---|---|
| `NodaMoney.Money` (struct) | `EricksonLopez.ValueObjects.Money` (`readonly record struct`) | 0 B allocations, Railway-Oriented `Result` validation |
| `NodaMoney.Currency` | `EricksonLopez.ValueObjects.CurrencyCode` | Stack-allocated ISO 4217 type, zero runtime table lookups |
| `money.Split(parts)` | `money.Allocate(ratios)` / `money.Distribute(parts)` | Fowler's exact remainder cent distribution algorithm |
| Direct constructor `new Money(100, "USD")` | `Money.Create(100m, CurrencyCode.USD)` | Result-pattern validation preventing unhandled invalid currencies |
| `money.Amount` / `money.Currency` | `money.Amount` / `money.Currency` | 100% property naming compatibility |

---

## 2. Code Comparison & Refactoring

### Before (NodaMoney)

```csharp
using NodaMoney;

// Creation
var price = new Money(199.99m, "USD");
var discount = new Money(20.00m, "USD");

// Arithmetic
var finalPrice = price - discount;

// Allocation
var shares = finalPrice.Split(3); // Returns IEnumerable<Money>
```

### After (EricksonLopez.ValueObjects)

```csharp
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;

// Creation with explicit validation
Result<Money> priceResult = Money.Create(199.99m, CurrencyCode.USD);
Result<Money> discountResult = Money.Create(20.00m, "USD");

if (priceResult.IsSuccess && discountResult.IsSuccess)
{
    // Direct struct operator arithmetic (0 B heap allocation)
    Money finalPrice = priceResult.Value - discountResult.Value;

    // Lossless remainder-preserving distribution
    Money[] shares = finalPrice.Distribute(3);
}
```

---

## 3. Database Persistence Migration

NodaMoney typically requires custom value converters. Replace them with:

```csharp
// Entity Framework Core 10
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.ConfigureDomainValueObjects();
}

// Dapper
DapperValueObjectRegistry.RegisterAll();
```
