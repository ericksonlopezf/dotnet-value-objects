# Migration Guide: Eliminating Primitive Obsession

> **Target:** Step-by-step refactoring from raw BCL scalar types (`string`, `decimal`, `Guid`) to strongly typed, zero-allocation Value Objects.

---

## 1. The Perils of Primitive Obsession

Raw primitive parameters permit silent architectural defects:
- Passing a destination ID into a source ID parameter: `Transfer(targetId, sourceId, amount)`
- Allowing invalid emails or corrupted tax IDs into database tables.
- Adding distinct currencies blindly (`100 USD + 100 EUR = 200 ???`).

---

## 2. Refactoring Steps

### Step 1: Replace Method Signatures

```diff
- public Task<Invoice> IssueInvoice(string customerEmail, decimal total, string currency, string taxId);
+ public Task<Result<Invoice>> IssueInvoice(Email customerEmail, Money total, Rnc taxId);
```

### Step 2: Validate at Application Boundaries

Move defensive checking from service layers to API endpoints/controllers:

```csharp
// API Controller / Minimal API Endpoint
var emailResult = Email.Create(request.Email);
var moneyResult = Money.Create(request.Amount, request.Currency);
var rncResult = Rnc.Create(request.TaxId);

if (Result.Combine(emailResult, moneyResult, rncResult) is { IsFailure: true } error)
{
    return TypedResults.BadRequest(error.Error);
}

// Inner domain service executes with guaranteed valid models
await invoiceService.IssueInvoice(emailResult.Value, moneyResult.Value, rncResult.Value);
```

### Step 3: Replace Entity Properties

```diff
public class CustomerEntity
{
    public Guid Id { get; set; }
-   public string Email { get; set; }
-   public string PhoneNumber { get; set; }
+   public Email Email { get; set; }
+   public PhoneNumber PhoneNumber { get; set; }
}
```
