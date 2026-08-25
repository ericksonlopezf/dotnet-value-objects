# Performance & Allocation Optimization Guide

The architecture of `EricksonLopez.ValueObjects` is engineered for **zero-allocation execution** and GC Gen0/Gen1 predictability in high-throughput enterprise systems.

---

## 1. Value Type Representation

Value Objects representing scalar numbers, percentages, temporal data, or currency codes are implemented as `readonly record struct` to avoid heap allocations:

| Type Category | Implementation | GC Allocation | Examples |
|---|---|:---:|---|
| **Monetary** | `readonly record struct` | **0 B** | `Money`, `CurrencyCode`, `ExchangeRate` |
| **Percentage & Rate** | `readonly record struct` | **0 B** | `Percentage`, `TaxRate`, `DiscountRate` |
| **Quantity & Scalar** | `readonly record struct` | **0 B** | `Quantity`, `BusinessDate` |
| **Range & Interval** | `readonly record struct Range<T>` | **0 B** | `DateRange`, `Range<int>`, `Range<decimal>` |
| **Text-Based VOs** | `sealed record : StringValueObject<TSelf>` | **1 alloc** (string intern) | `Email`, `FirstName`, `PostalCode` |
| **Composite VOs** | `sealed record : ValueObject` | **1 alloc** | `Address`, `FullName`, `TimeRange` |

---

## 2. String Normalization & Allocation Minimization

Text-based Value Objects (`Email`, `FirstName`, `PostalCode`, `TenantCode`, `SKU`, `DocumentNumber`, etc.) use `StringPipeline` with:

- **Pre-compiled regex** (compiled once at class initialization, reused across calls).
- **`StringBuilder`-based normalization** for whitespace collapsing and character-level transformations, minimizing intermediate string allocations.
- **Single allocation** on the factory success path (the final normalized string value).

---

## 3. Money Allocation Algorithm (Fowler's Method)

`Money.Allocate(params ReadOnlySpan<int> ratios)` implements Martin Fowler's proportional allocation algorithm, which distributes a monetary amount across weighted parties without fractional currency loss:

```csharp
var price = Money.Create(100.00m, CurrencyCode.USD).Value;
Money[] shares = price.Allocate(5, 3, 2);
// shares[0] = $50.00, shares[1] = $30.00, shares[2] = $20.00
// Zero cent lost: 50 + 30 + 20 = 100 ✅
```

The algorithm guarantees that `sum(shares) == original` by distributing any remainder one minimum currency unit at a time. The truncation precision and remainder unit automatically respect the currency's ISO 4217 decimal places via `CurrencyCode.DecimalPlaces`:

- **USD, EUR (2 decimals)**: distributes remainder in `0.01` units
- **JPY, KRW (0 decimals)**: distributes remainder in `1` units  
- **KWD, BHD (3 decimals)**: distributes remainder in `0.001` units

---

## 4. Money Equal-Parts Distribution

`Money.Distribute(int parts)` splits a monetary amount into equal parts, assigning any indivisible remainder to the first part:

```csharp
var price = Money.Create(100.00m, CurrencyCode.USD).Value;
Money[] equal = price.Distribute(3);
// equal[0] = $33.34, equal[1] = $33.33, equal[2] = $33.33
```

Unlike `Allocate`, `Distribute` uses banker's rounding (`MidpointRounding.ToEven`) for the base share calculation.

---

## 5. Money Rounding

`Money.Round()` rounds to the currency's standard decimal places using banker's rounding (`MidpointRounding.ToEven`):

```csharp
var m = Money.Create(12.345m, CurrencyCode.USD).Value;
Money rounded = m.Round(); // $12.34 (banker's rounding)
```

`Money.RoundCommercial()` uses commercial rounding (`MidpointRounding.AwayFromZero`), which rounds 0.005 up to 0.01 (the typical behaviour consumers expect):

```csharp
Money commercial = m.RoundCommercial(); // $12.35
```

---

## 6. Result<T> Zero-Overhead Design

`Result<T>` is a `readonly struct`, meaning:
- No heap allocation on success or failure paths.
- Error metadata (`Error`) is returned by value.
- Compatible with `readonly record struct` value objects on the stack.

---

## 7. Benchmarks

The benchmark suite in `benchmarks/EricksonLopez.ValueObjects.Benchmarks` validates performance characteristics using BenchmarkDotNet:

- Allocation = **0 B** for all `readonly record struct` value object operations and factory successes.
- Sub-microsecond execution on creation, Fowler money allocation, and `Range<T>` boundary checks.

### Running Benchmarks

```bash
dotnet run --project benchmarks/EricksonLopez.ValueObjects.Benchmarks/EricksonLopez.ValueObjects.Benchmarks.csproj --configuration Release
```

> **Note**: Always run benchmarks in `Release` configuration on a dedicated machine. `Debug` builds do not represent production performance characteristics.
