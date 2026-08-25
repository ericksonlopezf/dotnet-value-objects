# Quickstart Guide: 5-Minute Setup

> **Getting Started with `EricksonLopez.ValueObjects` in Modern .NET**

---

## 1. Install Package

```bash
dotnet add package EricksonLopez.ValueObjects
```

---

## 2. Model Financial Transactions

```csharp
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;

// 1. Create type-safe Money instances
Result<Money> price = Money.Create(99.99m, CurrencyCode.USD);
Result<Money> tax = Money.Create(18.00m, "USD");

if (price.IsSuccess && tax.IsSuccess)
{
    // 2. Direct struct arithmetic (0 heap allocations)
    Money total = price.Value + tax.Value;
    Console.WriteLine($"Total: {total}");

    // 3. Proportional revenue split without penny loss
    Money[] partnerShares = total.Allocate(1, 1); // 50/50 split
    Console.WriteLine($"Partner 1: {partnerShares[0]}"); // $59.00 USD
    Console.WriteLine($"Partner 2: {partnerShares[1]}"); // $58.99 USD
}
```

---

## 3. Enable EF Core 10 Persistence

```bash
dotnet add package EricksonLopez.ValueObjects.EntityFrameworkCore
```

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Map all domain Value Objects to primitive columns automatically
    modelBuilder.ConfigureDomainValueObjects();
}
```
