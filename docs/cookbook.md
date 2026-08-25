# Enterprise Cookbook & Recipes

> **Ready-to-Use Production Recipes for `EricksonLopez.ValueObjects`**

---

## Recipe 1: Multi-Item Shopping Cart Checkout

```csharp
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;

public sealed record CartItem(string Sku, Money UnitPrice, int Quantity);

public sealed class CheckoutService
{
    public Result<Money> CalculateCartTotal(IReadOnlyList<CartItem> items, TaxRate taxRate, DiscountRate? discountRate)
    {
        if (items.Count == 0)
        {
            return Error.Validation("Cart.Empty", "Cannot checkout empty cart.");
        }

        Money subtotal = Money.Zero(items[0].UnitPrice.Currency);

        foreach (var item in items)
        {
            Money itemTotal = item.UnitPrice * item.Quantity;
            var addResult = subtotal.Add(itemTotal);
            if (addResult.IsFailure) return addResult.Error;
            subtotal = addResult.Value;
        }

        if (discountRate is { IsZero: false } discount)
        {
            Money discountAmount = discount.CalculateDiscount(subtotal);
            subtotal = subtotal - discountAmount;
        }

        Money taxAmount = taxRate.CalculateTax(subtotal);
        return subtotal + taxAmount;
    }
}
```

---

## Recipe 2: Multi-Country Tax Identifier Routing

```csharp
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;
using EricksonLopez.ValueObjects.Fiscal.Mexico;
using EricksonLopez.ValueObjects.Fiscal.Chile;

public static class FiscalTaxIdValidator
{
    public static Result<string> ValidateTaxIdForCountry(string countryIso, string rawTaxId)
    {
        return countryIso.ToUpperInvariant() switch
        {
            "DO" => Rnc.Create(rawTaxId).Map(rnc => rnc.Value),
            "MX" => Rfc.Create(rawTaxId).Map(rfc => rfc.Value),
            "CL" => Rut.Create(rawTaxId).Map(rut => rut.Value),
            _ => Error.Validation("Country.Unsupported", $"Tax ID validation not available for country {countryIso}")
        };
    }
}
```

---

## Recipe 3: Masking Sensitive Customer PII in Structured Logging

```csharp
using EricksonLopez.ValueObjects;
using Microsoft.Extensions.Logging;

public sealed class CustomerNotificationService(ILogger<CustomerNotificationService> logger)
{
    public void NotifyCustomer(Email email, PhoneNumber phone)
    {
        // ToString() automatically applies [SensitiveData] masking
        logger.LogInformation("Sending notification to email: {Email} and phone: {Phone}", email, phone);
        // Log output: "Sending notification to email: e***z@enterprise.com and phone: +1809***1234"
    }
}
```
