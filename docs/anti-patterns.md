# Anti-Patterns & Common Pitfalls

---

## 1. Prohibited Anti-Patterns in Value Objects

### ❌ Anti-Pattern 1: Mutating Value Object State
```csharp
// BAD: Mutable properties violate value object invariants
public struct Money { public decimal Amount { get; set; } }

// GOOD: Immutable readonly record struct
public readonly record struct Money(decimal Amount, CurrencyCode Currency);
```

### ❌ Anti-Pattern 2: Mixing Currencies Without Conversion
```csharp
// BAD: Adding USD and EUR directly without an explicit exchange rate
var m1 = new Money(100, Currency.USD);
var m2 = new Money(50, Currency.EUR);
// Adding them throws InvalidOperationException to protect financial correctness
```
