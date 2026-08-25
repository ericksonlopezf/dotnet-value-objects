# Anti-Patterns & Architectural Remedies

> **Guide:** Avoiding common anti-patterns in enterprise Domain-Driven Design when modeling Value Objects.

---

## 1. Primitive Obsession

### The Anti-Pattern
Using raw scalar BCL types (`string`, `decimal`, `Guid`) to represent complex domain concepts across services, repositories, and controllers.

```csharp
// Anti-pattern: Easy to mix parameter order, no validation enforcement
public void Transfer(Guid sourceAccountId, Guid targetAccountId, decimal amount, string currency) { }
```

### The Remedy
Encapsulate concepts into strongly typed, stack-allocated structs:

```csharp
// Remedy: Type-safe, invariant-enforced, zero-allocation
public void Transfer(AccountId source, AccountId target, Money amount) { }
```

---

## 2. Inexact Division & Cent Loss

### The Anti-Pattern
Dividing `decimal` values using standard arithmetic and rounding each share independently.

```csharp
// Anti-pattern: $100.00 / 3 = $33.33 * 3 = $99.99 (Penny lost!)
decimal share = Math.Round(total / 3, 2);
```

### The Remedy
Use Fowler's `Allocate` method which distributes remainder pennies deterministically:

```csharp
// Remedy: Returns [$33.34, $33.33, $33.33] -> Sum is exactly $100.00
Money[] shares = total.Allocate(1, 1, 1);
```

---

## 3. Mutable Value Objects

### The Anti-Pattern
Allowing property setters on Value Objects:

```csharp
// Anti-pattern: Mutates state, breaks dictionary hashing and thread-safety
public record Money { public decimal Amount { get; set; } }
```

### The Remedy
Use `readonly record struct` with read-only properties (enforced by Roslyn analyzer `ELVO003`):

```csharp
public readonly record struct Money(decimal Amount, CurrencyCode Currency);
```

---

## 4. Exception-Driven Validation

### The Anti-Pattern
Throwing `ArgumentException` during routine user input validation.

### The Remedy
Return `Result<T>` from static factory methods for Railway-Oriented error handling.
