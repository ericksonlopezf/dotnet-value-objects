# Cookbook & Recipes

---

## 1. Money Currency Conversion Recipe

```csharp
using EricksonLopez.ValueObjects;

public static Money ConvertCurrency(Money source, decimal exchangeRate, CurrencyCode targetCurrency)
{
    decimal convertedAmount = Math.Round(source.Amount * exchangeRate, 2, MidpointRounding.AwayFromZero);
    return new Money(convertedAmount, targetCurrency);
}
```

---

## 2. Fiscal Identifier Validation Recipe

```csharp
using EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;

public static bool IsValidTaxpayer(string rawRnc)
{
    return Rnc.TryCreate(rawRnc, out _, out _);
}
```
