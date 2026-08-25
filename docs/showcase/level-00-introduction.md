# Level 00: Architecture & Philosophy

> **Module:** Foundations of Immutable Value Objects & Memory Architecture  
> **Prerequisites:** Understanding Domain-Driven Design (DDD) Value Objects vs Entities.

---

## 1. The Value Object Paradigm

In Domain-Driven Design, a **Value Object** is an entity whose identity is determined entirely by its attributes rather than an explicit identifier key (such as an ID).

### Core Properties of Value Objects
1. **Value Equality**: Two instances with matching internal attributes are considered identical.
2. **Immutability**: Once created, internal state cannot be modified. Any transformation returns a new instance.
3. **Self-Validation**: Instances cannot be created in an invalid state.
4. **Side-Effect Free Functions**: Methods and operations compute results without mutating inputs.

---

## 2. Memory Architecture: `readonly record struct`

Traditional C# Value Objects implemented as `class` reference types cause high GC heap allocation under heavy traffic.

`EricksonLopez.ValueObjects` builds scalar types as stack-allocated `readonly record struct` value types:

| Metric | Class-Based VO | EricksonLopez Struct VO |
|---|---|---|
| Heap Allocation | 24–48 bytes per object | **0 bytes** (Stack / Register allocated) |
| Garbage Collector Churn | High (Gen0 / Gen1 pressure) | **Zero GC pressure** |
| Memory Layout | Heap pointer indirection | Direct inline storage within Entity structs |
| Comparison Cost | Virtual dispatch & reference check | Inline bitwise / field comparison |

---

## 3. The Result-Pattern Invariant Gate

Constructors are marked `private`. Construction is routed through `public static Result<TSelf> Create(...)` factory methods:

```csharp
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;

Result<CurrencyCode> usd = CurrencyCode.Create("USD");
if (usd.IsSuccess)
{
    Console.WriteLine($"Valid currency: {usd.Value}");
}
```
