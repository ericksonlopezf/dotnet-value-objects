# RFC-0001: Standardizing Result Pattern Over Exceptions for Value Object Factories

> **Status:** Approved  
> **Authors:** Erickson Lopez (<ericksonlopezf@gmail.com>)  
> **Created:** 2026-08-20  
> **Target Release:** v1.0.0  

---

## 1. Summary

This RFC establishes that all factory methods (`Create`) across `EricksonLopez.ValueObjects` must return `Result<TValueObject>` from `EricksonLopez.Result` instead of throwing domain or validation exceptions during expected validation failures.

---

## 2. Motivation

Throwing exceptions (`ArgumentException`, `FormatException`) for routine user input validation carries severe performance penalties in high-throughput .NET microservices due to stack trace generation. Furthermore, exceptions obscure business failure flows. The Railway-Oriented Programming (ROP) pattern makes failure states explicit, typed, and composable without GC overhead.

---

## 3. Specification

1. Every Value Object factory is typed as `public static Result<TSelf> Create(...)`.
2. Validation failures produce structured `Error.Validation` instances with unique error codes (e.g. `Email.Empty`, `Money.InvalidAmount`).
3. `DomainException` is reserved strictly for catastrophic programming invariants (e.g., unreachable states).

```csharp
public static Result<Money> Create(decimal amount, CurrencyCode currency)
{
    if (amount < 0 && currency.IsNonNegativeOnly)
    {
        return Error.Validation("Money.NegativeNotAllowed", "Negative money is prohibited for this currency.");
    }
    return new Money(amount, currency);
}
```

---

## 4. Decision

Approved by Core Committee. Integrated across 100% of public factory methods.
