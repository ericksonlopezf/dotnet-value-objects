# Level 01 — Core Value Objects: Money & Currency

In Level 01, we model financial calculations with zero-allocation `Money` and ISO 4217 `CurrencyCode`.

---

## 1. Money Operations

```csharp
using EricksonLopez.ValueObjects;

var price = new Money(100.50m, Currency.USD);
var tax = new Money(18.09m, Currency.USD);

Money total = price + tax; // 118.59 USD

// Throws InvalidOperationException on mismatched currencies
var euro = new Money(50.00m, Currency.EUR);
// var invalid = price + euro; 
```
