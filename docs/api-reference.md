# API Reference: `EricksonLopez.ValueObjects`

> **Comprehensive Type Signatures, Methods, and Invariants**

---

## 1. `Money` Struct Reference

```csharp
namespace EricksonLopez.ValueObjects;

public readonly record struct Money : IValueObject<Money>, IParsable<Money>, ISpanParsable<Money>, ISpanFormattable
{
    public decimal Amount { get; }
    public CurrencyCode Currency { get; }

    public static Result<Money> Create(decimal amount, CurrencyCode currency);
    public static Result<Money> Create(decimal amount, string currencyCode);
    public static Result<Money> CreateNonNegative(decimal amount, CurrencyCode currency);
    public static Money Zero(CurrencyCode currency);
    public static Money ZeroUsd { get; }

    public Money[] Allocate(params int[] ratios);
    public Money[] Allocate(params decimal[] ratios);
    public Money[] Distribute(int n);

    public Money ApplyPercentage(Percentage percentage);
    public Money Negate();
    public Money Abs();
    public Money Round(int decimals, MidpointRounding mode = MidpointRounding.ToEven);

    public Result<Money> Add(Money other);
    public Result<Money> Subtract(Money other);

    public static Money operator +(Money left, Money right);
    public static Money operator -(Money left, Money right);
    public static Money operator *(Money left, decimal factor);
    public static Money operator -(Money money);

    public static bool operator <(Money left, Money right);
    public static bool operator >(Money left, Money right);
    public static bool operator <=(Money left, Money right);
    public static bool operator >=(Money left, Money right);
}
```

---

## 2. `CurrencyCode` Struct Reference

```csharp
namespace EricksonLopez.ValueObjects;

public readonly record struct CurrencyCode : IValueObject<CurrencyCode>, IParsable<CurrencyCode>, ISpanParsable<CurrencyCode>
{
    public string Code { get; }

    public static readonly CurrencyCode USD;
    public static readonly CurrencyCode EUR;
    public static readonly CurrencyCode DOP;
    public static readonly CurrencyCode CLP;
    public static readonly CurrencyCode COP;
    public static readonly CurrencyCode MXN;
    public static readonly CurrencyCode PEN;
    public static readonly CurrencyCode ARS;

    public static Result<CurrencyCode> Create(string? code);
    public static CurrencyCode Parse(string s, IFormatProvider? provider = null);
    public static bool TryParse(string? s, IFormatProvider? provider, out CurrencyCode result);
}
```

---

## 3. `Range<T>` Struct Reference

```csharp
namespace EricksonLopez.ValueObjects;

public readonly record struct Range<T> : IValueObject<Range<T>>, IEquatable<Range<T>>
    where T : struct, IComparable<T>, IEquatable<T>
{
    public T Start { get; }
    public T End { get; }

    public static Result<Range<T>> Create(T start, T end);
    public bool Contains(T value);
    public bool Contains(Range<T> other);
    public bool Overlaps(Range<T> other);
    public Result<Range<T>> Intersect(Range<T> other);
}
```
