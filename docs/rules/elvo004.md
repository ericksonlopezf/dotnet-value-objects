# ELVO004: Value Objects Must Not Be Initialized with Default

| Property | Value |
|---|---|
| **Rule ID** | `ELVO004` |
| **Category** | `Architecture.Domain` |
| **Severity** | `Error` |
| **Enabled by Default** | `true` |
| **Applies to** | Types implementing `IValueObject`, `SingleValueObject`, `StringValueObject`, or struct-based value objects |

---

## 🎯 Rule Description

In Domain-Driven Design (DDD), **Value Objects must always be valid by construction**. When Value Objects are implemented as value types (`readonly record struct` or `readonly struct`), C# allows parameterless instantiation via `default(T)`, `default`, or `new T()`.

Initializing a Value Object with its default value bypasses all domain validation rules and factory methods, producing uninitialized or corrupt domain primitives (e.g., empty currencies, unvalidated tax IDs, or zeroed financial amounts).

`ELVO004` detects and prevents:
- Explicit `default(T)` expressions for Value Object types.
- Default literal expressions (`default`) targeting Value Object variables or parameters.
- Parameterless object creation expressions (`new T()`) for struct-based Value Objects.

---

## ❌ Violation Example

```csharp
// Violation: Using default creates unvalidated Money with null/empty currency
Money emptyPrice = default(Money);

// Violation: Default literal bypasses validation
Money zeroAmount = default;

// Violation: Parameterless struct constructor creates uninitialized instance
Money uninitialized = new Money();
```

---

## ✅ Compliant Example

```csharp
// Compliant: Explicitly construct via the domain factory method
Result<Money> priceResult = Money.Create(0.00m, CurrencyCode.USD);

if (priceResult.IsSuccess)
{
    Money price = priceResult.Value;
    // Guaranteed to satisfy all domain invariants
}
```

---

## 🛠️ Remediation & Guidance

Do not use `default` or parameterless instantiation for Value Objects. Always instantiate Value Objects via their public static factory methods (e.g., `Create` or `TryCreate`) to ensure domain invariants are verified and errors are properly encapsulated within `Result` types.
