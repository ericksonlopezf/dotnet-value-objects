# Level 04 — Domain Primitives & Strongly-Typed Identifiers

In Level 04, we integrate Value Objects with `EricksonLopez.DomainPrimitives` using `EricksonLopez.ValueObjects.DomainPrimitives`.

---

## 1. Domain Primitive Synergies

Fiscal identifiers automatically implement `IDomainPrimitive<TSelf, string>` with zero runtime overhead:

```csharp
using EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;

if (Rnc.TryCreate("101000001", out var rnc, out var error))
{
    Console.WriteLine($"Valid RNC: {rnc.Value}");
}
```
