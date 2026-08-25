# Level 01: Money & Currency Core

> **Module:** High-Precision Financial Arithmetic, Currency Invariants & Lossless Allocation  
> **Key Types:** `Money`, `CurrencyCode`, `Percentage`, `TaxRate`, `DiscountRate`, `ExchangeRate`

---

## 1. Creating and Operating with `Money`

`Money` encapsulates a `decimal` amount (up to 4 decimal places) alongside an ISO 4217 `CurrencyCode`.

```csharp
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;

// Safe factory creation
Result<Money> priceResult = Money.Create(150.75m, CurrencyCode.USD);
Result<Money> feeResult = Money.Create(12.25m, "USD");

if (priceResult.IsSuccess && feeResult.IsSuccess)
{
    Money price = priceResult.Value;
    Money fee = feeResult.Value;

    // Operator arithmetic
    Money total = price + fee; // $163.00 USD
    Money diff = price - fee;  // $138.50 USD

    Console.WriteLine($"Total: {total}");
}
```

---

## 2. Currency Safety and Conversion

Attempting to add or compare differing currencies safely returns a validation failure or throws a strict `CurrencyMismatchException` when operator overloads are used without conversion:

```csharp
Money usd = Money.Create(100m, CurrencyCode.USD).Value;
Money eur = Money.Create(100m, CurrencyCode.EUR).Value;

// Method-based safe addition returns Result.Failure
Result<Money> addResult = usd.Add(eur);
Console.WriteLine(addResult.IsFailure); // true: Cannot add USD and EUR

// Explicit conversion via ExchangeRate
ExchangeRate fx = ExchangeRate.Create(CurrencyCode.EUR, CurrencyCode.USD, 1.085m).Value;
Money convertedEur = fx.Convert(eur); // $108.50 USD
Money convertedTotal = usd + convertedEur; // $208.50 USD
```

---

## 3. Fowler's Lossless Proportional Allocation

When splitting revenue across partners, `Money.Allocate` ensures no pennies are lost to decimal truncation:

```csharp
Money total = Money.Create(100.00m, CurrencyCode.USD).Value;

// Proportions 1 : 1 : 1 (e.g. $100 / 3)
Money[] shares = total.Allocate(1, 1, 1);

// Result: $33.34, $33.33, $33.33 -> Exactly $100.00
Console.WriteLine($"Share 1: {shares[0]}");
Console.WriteLine($"Share 2: {shares[1]}");
Console.WriteLine($"Share 3: {shares[2]}");
```
