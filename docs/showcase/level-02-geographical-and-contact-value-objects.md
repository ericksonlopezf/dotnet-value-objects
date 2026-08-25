# Level 02: Geographical, Contact & Temporal Value Objects

> **Module:** Contact Data, Postal Addresses, Continuous Intervals & Business Dates  
> **Key Types:** `Address`, `Country`, `PostalCode`, `Email`, `PhoneNumber`, `Range<T>`, `BusinessDate`, `TimeRange`

---

## 1. Contact Information & PII Protection

`Email` and `PhoneNumber` format and validate inputs according to RFC standards and international E.164 conventions. Both use `[SensitiveData]` to prevent raw PII exposure in logs:

```csharp
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;

Result<Email> email = Email.Create("erickson.lopez@enterprise.com");
Result<PhoneNumber> phone = PhoneNumber.Create("+18095551234");

if (email.IsSuccess)
{
    // Masked in logs and ToString()
    Console.WriteLine(email.Value.ToString()); // "e***z@enterprise.com"
    Console.WriteLine(email.Value.Value);      // "erickson.lopez@enterprise.com"
}
```

---

## 2. Structured Postal Addresses

`Address` models normalized physical locations using ISO 3166 `Country` and validated `PostalCode`:

```csharp
Country country = Country.Create("US").Value;
PostalCode postalCode = PostalCode.Create("10001").Value;

Result<Address> address = Address.Create(
    street: "350 5th Ave",
    line2: "Floor 50",
    city: "New York",
    state: "NY",
    postalCode: postalCode,
    country: country);
```

---

## 3. Generic Intervals & Business Dates

`Range<T>` provides inclusive bounds `[Start .. End]` for numerical, financial, and temporal domain intervals:

```csharp
// Date interval
Range<DateOnly> sprint = Range<DateOnly>.Create(
    new DateOnly(2026, 8, 1),
    new DateOnly(2026, 8, 14)).Value;

bool isActive = sprint.Contains(new DateOnly(2026, 8, 10)); // true

// BusinessDate wrapping DateOnly with business-day helpers
BusinessDate date = BusinessDate.Create(new DateOnly(2026, 8, 25)).Value;
```
