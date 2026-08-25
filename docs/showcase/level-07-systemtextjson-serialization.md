# Level 07: System.Text.Json Serialization

> **Module:** High-Throughput, Zero-Reflection JSON Serialization  
> **Key Package:** `EricksonLopez.ValueObjects.Serialization.Json`

---

## 1. Zero-Allocation JSON Converters

The `ValueObjectJsonConverterFactory` converts Value Objects directly to primitive JSON tokens:
- `Email`, `PhoneNumber`, `Country`, `PostalCode`, `Rnc`, `Rut` serialize directly as string tokens (`"value"`).
- `Money` serializes as a structured object: `{"amount": 150.00, "currency": "USD"}`.
- `Range<T>` serializes as: `{"start": ..., "end": ...}`.

---

## 2. Configuration & Usage

```csharp
using System.Text.Json;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.Serialization.Json;

var serializerOptions = new JsonSerializerOptions();
serializerOptions.Converters.Add(new ValueObjectJsonConverterFactory());

var order = new
{
    Id = Guid.NewGuid(),
    CustomerEmail = Email.Create("customer@domain.com").Value,
    Total = Money.Create(199.99m, CurrencyCode.USD).Value
};

string json = JsonSerializer.Serialize(order, serializerOptions);
// Output: {"Id":"...","CustomerEmail":"customer@domain.com","Total":{"amount":199.99,"currency":"USD"}}
```
