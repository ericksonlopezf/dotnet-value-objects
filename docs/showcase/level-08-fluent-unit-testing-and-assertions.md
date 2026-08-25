# Level 08 — Fluent Testing & Assertions

In Level 08, we write unit test assertions for value object operations.

---

## 1. Value Object Assertions

```csharp
using EricksonLopez.ValueObjects;
using Xunit;

public class MoneyTests
{
    [Fact]
    public void Addition_SameCurrency_ShouldAddAmounts()
    {
        var m1 = new Money(10m, Currency.USD);
        var m2 = new Money(20m, Currency.USD);

        var result = m1 + m2;

        Assert.Equal(30m, result.Amount);
        Assert.Equal(Currency.USD, result.Currency);
    }
}
```
