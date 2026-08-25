# ELVO003: Value Objects Must Be Immutable

| Property | Value |
|---|---|
| **Rule ID** | `ELVO003` |
| **Category** | `Architecture.Domain` |
| **Severity** | `Error` |
| **Enabled by Default** | `true` |
| **Applies to** | Types implementing `IValueObject`, `SingleValueObject`, `StringValueObject`, or inheriting `ValueObject` |

---

## 🎯 Rule Description

In Domain-Driven Design, **Value Objects are strictly immutable**. Once instantiated, their properties and internal state must never change. Mutability breaks structural equality, hash code consistency, and thread safety.

`ELVO003` verifies that:
- All properties on a Value Object have `init`-only or `get`-only accessors (no mutable `set;`).
- All fields are marked `readonly`.
- Value types are declared as `readonly record struct` or `readonly struct`.

---

## ❌ Violation Example

```csharp
public sealed record class Money : IValueObject<Money>
{
    // Violation: Mutable property setter permits state alteration after creation
    public decimal Amount { get; set; }
    public string Currency { get; set; }
}
```

---

## ✅ Compliant Example

```csharp
public readonly record struct Money : IValueObject<Money>
{
    // Compliant: Immutable properties with get-only accessors
    public decimal Amount { get; }
    public CurrencyCode Currency { get; }

    private Money(decimal amount, CurrencyCode currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Result<Money> Create(decimal amount, CurrencyCode currency)
    {
        return new Money(amount, currency);
    }
}
```

---

## 🛠️ Automated Code Fix

The analyzer provides a code fix converting mutable `set;` accessors to `init;` or removing them entirely in favor of read-only properties.
