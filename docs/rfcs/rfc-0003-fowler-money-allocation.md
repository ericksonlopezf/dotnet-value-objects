# RFC-0003: Martin Fowler Lossless Money Allocation Algorithm

> **Status:** Approved  
> **Authors:** Erickson Lopez (<ericksonlopezf@gmail.com>)  
> **Created:** 2026-08-22  
> **Target Release:** v1.0.0  

---

## 1. Summary

This RFC specifies the inclusion of Martin Fowler's proportional distribution and allocation algorithms (`Money.Allocate(ratios)` and `Money.Distribute(n)`) into `Money` to eliminate fractional cent loss in financial calculations.

---

## 2. Motivation

In accounting and e-commerce systems, dividing money across parties or tax brackets using raw division frequently produces recurring decimals (e.g. $100.00 / 3 = $33.3333...). Standard rounding truncates the remaining penny ($33.33 * 3 = $99.99), resulting in cent loss and reconciliation discrepancies.

---

## 3. Specification

The `Allocate` method calculates base shares by multiplying total amount by weight over the sum of weights, computes the remainder difference, and distributes remaining pennies one-by-one to the largest fractional remainders:

$$\sum_{i=1}^n \text{Allocate}(\text{ratios})_i = \text{TotalMoney}$$

```csharp
public Money[] Allocate(params int[] ratios)
{
    // Lossless distribution guaranteed by construction
}
```

---

## 4. Decision

Approved and integrated into `Money` with comprehensive unit tests verifying 100% conservation of total amounts.
