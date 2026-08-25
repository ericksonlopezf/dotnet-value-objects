# ELVO001: Value Objects Must Have Private or Protected Constructors

| Property | Value |
|---|---|
| **Rule ID** | `ELVO001` |
| **Category** | `Architecture.Domain` |
| **Severity** | `Error` |
| **Enabled by Default** | `true` |
| **Applies to** | Types implementing `IValueObject`, `SingleValueObject`, `StringValueObject`, or inheriting `ValueObject` |

---

## 🎯 Rule Description

Value Objects in Domain-Driven Design (DDD) must be **valid by construction**. Permitting public constructors allows consumers to instantiate types bypassing domain invariants and business rules.

`ELVO001` enforces that all constructors on Value Object types are declared as `private` (or `protected` for abstract base types). Creation must be encapsulated exclusively within static factory methods (such as `Create` or `TryCreate`) returning a `Result`.

---

## ❌ Violation Example

```csharp
public sealed record class Email : StringValueObject<Email>
{
    // Violation: Public constructor allows bypassing validation rules
    public Email(string value) : base(value)
    {
    }
}
```

---

## ✅ Compliant Example

```csharp
public sealed record class Email : StringValueObject<Email>
{
    // Compliant: Private constructor prevents illegal instantiation
    private Email(string value) : base(value)
    {
    }

    public static Result<Email> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Error.Validation("Email.Empty", "Email address cannot be empty.");
        }

        if (!value.Contains('@') || !value.Contains('.'))
        {
            return Error.Validation("Email.InvalidFormat", "Email address format is invalid.");
        }

        return new Email(value.Trim().ToLowerInvariant());
    }
}
```

---

## 🛠️ Automated Code Fix

The analyzer provides a code fix that automatically changes constructor accessibility from `public` or `internal` to `private`.
