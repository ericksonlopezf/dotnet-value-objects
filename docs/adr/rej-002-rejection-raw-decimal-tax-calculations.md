# REJ-002: Rejection of Raw Decimal Math for Tax Calculations

- **Status:** Rejected
- **Date:** 2026-08-16
- **Original Proposal:** Expose static utility methods like `public static decimal CalculateTax(decimal baseAmount, decimal rate)`.

## Rationale for Rejection

1. **Loss of Currency Semantics:** Raw `decimal` arithmetic drops currency information (`CurrencyCode`), creating risks of accidental multi-currency operations.
2. **Ignorance of Local Rounding Rules:** Tax authorities mandate specific rounding strategies (e.g. Chile integer rounding for CLP, Colombia Banker's Rounding).
3. **Alternative Decision:** Encapsulate monetary calculations in `Money` and domain rates (`TaxRate.Calculate(Money baseAmount)` / `Money.ApplyPercentage(TaxRate rate)`).
