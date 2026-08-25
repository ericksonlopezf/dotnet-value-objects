# Functional Domain Map

> **Hierarchical Map of Domain Responsibilities & Capabilities**

---

## 1. Domain Map

```text
EricksonLopez.ValueObjects
├── Core Abstractions (IValueObject, ValueObject, SingleValueObject, StringValueObject)
├── Financial Domain
│   ├── Money (128-bit decimal, Fowler allocation, rounding, addition/subtraction)
│   ├── CurrencyCode (ISO 4217 uppercase 3-letter codes)
│   ├── Percentage, TaxRate, DiscountRate
│   └── ExchangeRate (Conversion, Inversion)
├── Contact & Geographic Domain
│   ├── Email (RFC validation, PII masking)
│   ├── PhoneNumber (E.164 formatting, PII masking)
│   ├── Country (ISO 3166-1 alpha-2)
│   ├── PostalCode
│   └── Address (Normalized composite record)
├── Temporal & Numeric Domain
│   ├── Range<T> (Inclusive interval [Start .. End])
│   ├── TimeRange (Overnight shift interval)
│   └── BusinessDate (DateOnly accounting wrapper)
└── Multi-Country Fiscal Satellites
    ├── Dominican Republic (DGII: RNC, Cedula, e-CF)
    ├── Chile (SII: RUT, DTE Folio)
    ├── Colombia (DIAN: NIT, CUFE, CUDE, CUNE)
    ├── Mexico (SAT CFDI 4.0: RFC, CURP, Fiscal UUID)
    ├── Peru (SUNAT: RUC, CPE Identifier)
    └── Argentina (ARCA/AFIP: CUIT, CUIL, CBU, CVU, CAE)
```
