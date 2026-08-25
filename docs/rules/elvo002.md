# ELVO002: Value Objects Must Provide a Static Create Factory Method

| Property | Value |
|---|---|
| **Rule ID** | `ELVO002` |
| **Category** | `Architecture.Domain` |
| **Severity** | `Error` |
| **Enabled by Default** | `true` |
| **Applies to** | Concrete types implementing `IValueObject`, `SingleValueObject`, `StringValueObject`, or inheriting `ValueObject` |

---

## 🎯 Rule Description

Value Objects must encapsulate invariant validation. To ensure a predictable and uniform API surface across the entire domain model, every concrete Value Object must expose at least one `public static` factory method named `Create`.

The factory method must return a `Result` or `Result<TValueObject>` to enforce explicit, functional error handling without exceptions.

---

## ❌ Violation Example

```csharp
public sealed record class CurrencyCode : IValueObject<CurrencyCode>
{
    private readonly string _code;

    private CurrencyCode(string code) => _code = code;

    // Violation: Missing static Create factory method returning Result<CurrencyCode>
    public static CurrencyCode FromString(string code) => new(code);
}
```

---

## ✅ Compliant Example

```csharp
public sealed record class CurrencyCode : IValueObject<CurrencyCode>
{
    private readonly string _code;

    private CurrencyCode(string code) => _code = code;

    // Compliant: Standardized static Create method returning Result<CurrencyCode>
    public static Result<CurrencyCode> Create(string? code)
    {
        if (string.IsNullOrWhiteSpace(code) || code.Trim().Length != 3)
        {
            return Error.Validation("CurrencyCode.Invalid", "Currency code must be exactly 3 uppercase letters.");
        }

        return new CurrencyCode(code.Trim().ToUpperInvariant());
    }
}
```

---

## 🛠️ Automated Code Fix

The analyzer offers a template code fix generating the canonical `public static Result<T> Create(...)` skeleton.
