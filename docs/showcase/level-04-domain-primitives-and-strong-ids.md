# Level 04: Domain Primitives Bridge & Strong IDs

> **Module:** Interoperability with `EricksonLopez.DomainPrimitives.Abstractions`  
> **Key Package:** `EricksonLopez.ValueObjects.DomainPrimitives`

---

## 1. The Separation of Concerns

- **Domain Primitives (`EricksonLopez.DomainPrimitives`)**: Scalar value wrappers, generated source structs, and Roslyn-analyzed single-value contracts.
- **Value Objects (`EricksonLopez.ValueObjects`)**: Rich, multi-attribute, enterprise composite objects (`Money`, `Address`, `Range<T>`) and pre-packaged fiscal tax satellites.

---

## 2. Bridging Value Objects to Domain Primitive Contracts

The `EricksonLopez.ValueObjects.DomainPrimitives` package provides bidirectional conversion adapters and strongly-typed identifier bridges:

```csharp
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.DomainPrimitives;

// Converting domain value objects to standardized primitive contracts
Email email = Email.Create("user@domain.com").Value;
var primitiveBridge = email.ToDomainPrimitive();

// Strongly typed ID representation
var strongId = email.ToStrongId();
```
