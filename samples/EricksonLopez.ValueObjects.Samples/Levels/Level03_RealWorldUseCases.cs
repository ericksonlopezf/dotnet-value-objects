// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;

namespace EricksonLopez.ValueObjects.Samples.Levels;

/// <summary>
/// Level 03: Real-World Enterprise Business Scenarios.
/// Demonstrates the complete Money API (Allocate, Distribute, Negate, Abs, Round, ApplyPercentage, arithmetic operators,
/// comparisons, IsZero/IsPositive/IsNegative), DiscountRate/TaxRate full computation API, ExchangeRate.Convert/Inverse,
/// Email.LocalPart/Domain/Masked(), FullName overloads, and Quantity arithmetic.
/// </summary>
public static class Level03_RealWorldUseCases
{
    /// <summary>
    /// Executes real-world business scenarios.
    /// </summary>
    public static void Run()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n===============================================================================");
        Console.WriteLine(" [LEVEL 3] REAL-WORLD USE CASES: MONEY, RATES, CONVERSION, AND SUPPLY CHAIN");
        Console.WriteLine("===============================================================================");
        Console.ResetColor();

        // ─── 1. Money — Full API ─────────────────────────────────────────────────────
        Console.WriteLine("[1. Money — Full API: Creation, Arithmetic, Properties, and Invariants]");

        // Create overloads
        var usd100 = Money.Create(100.00m, "USD").Value;
        var usd50  = Money.Create(50.00m, CurrencyCode.USD).Value;
        var zero   = Money.Zero(CurrencyCode.USD);

        Console.WriteLine($"  - usd100   : {usd100}");
        Console.WriteLine($"  - usd50    : {usd50}");
        Console.WriteLine($"  - ZeroUsd  : {Money.ZeroUsd}");
        Console.WriteLine($"  - Zero(USD): {zero}");

        // CreateNonNegative — succeeds
        var nonNeg = Money.CreateNonNegative(75.00m, CurrencyCode.USD).Value;
        Console.WriteLine($"  - NonNeg   : {nonNeg}");

        // CreateNonNegative — fails
        var negResult = Money.CreateNonNegative(-10.00m, CurrencyCode.USD);
        Console.WriteLine($"  - -$10 NonNeg error: [{negResult.Error.Code}] {negResult.Error.Description}");

        // Boolean properties
        var negMoney = Money.Create(-25.00m, "USD").Value;
        Console.WriteLine($"  - $100 IsZero    : {usd100.IsZero}");
        Console.WriteLine($"  - $100 IsPositive: {usd100.IsPositive}");
        Console.WriteLine($"  - $0   IsZero    : {zero.IsZero}");
        Console.WriteLine($"  - -$25 IsNegative: {negMoney.IsNegative}");

        // Arithmetic operators
        var sum  = usd100 + usd50;
        var diff = usd100 - usd50;
        var mult = usd100 * 1.5m;
        var neg  = -usd100;
        Console.WriteLine($"  - $100 + $50     : {sum}");
        Console.WriteLine($"  - $100 - $50     : {diff}");
        Console.WriteLine($"  - $100 × 1.5     : {mult}");
        Console.WriteLine($"  - -$100          : {neg}");

        // Result-returning Add/Subtract (safe, for different currencies)
        var eurResult = Money.Create(100.00m, "EUR");
        if (eurResult.IsSuccess)
        {
            var crossCurrencyAdd = usd100.Add(eurResult.Value);
            Console.WriteLine($"  - USD + EUR (error): [{crossCurrencyAdd.Error.Code}]");
        }

        // Negate / Abs
        var absVal = negMoney.Abs();
        var negVal = usd100.Negate();
        Console.WriteLine($"  - Abs(-$25)      : {absVal}");
        Console.WriteLine($"  - Negate($100)   : {negVal}");

        // Rounding
        var rough = Money.Create(100.005m, "USD").Value;
        var bankers    = rough.Round();       // Banker's rounding (ToEven)
        var commercial = rough.RoundCommercial(); // AwayFromZero
        Console.WriteLine($"  - Round (banker)    : {bankers}");
        Console.WriteLine($"  - Round (commercial): {commercial}");

        // ApplyPercentage
        var invoice100 = Money.Create(100.00m, "USD").Value;
        var vatPct     = Percentage.Create(18.0m).Value;
        var vatAmount  = invoice100.ApplyPercentage(vatPct);
        Console.WriteLine($"  - $100 × 18% VAT : {vatAmount}");

        // Allocate — Fowler anti-penny-loss
        var shares = invoice100.Allocate(1, 1, 1);
        Console.WriteLine($"  - $100 ÷ [1:1:1] → {shares[0]}, {shares[1]}, {shares[2]} (Total: {shares[0].Amount + shares[1].Amount + shares[2].Amount} USD)");

        // Distribute — equal parts
        var parts = Money.Create(100.00m, "USD").Value.Distribute(3);
        Console.WriteLine($"  - $100 Distribute(3) → {parts[0]}, {parts[1]}, {parts[2]}");

        // Comparison operators & methods
        Console.WriteLine($"  - $100 > $50     : {usd100.IsGreaterThan(usd50)}");
        Console.WriteLine($"  - $50  < $100    : {usd50.IsLessThan(usd100)}");
        Console.WriteLine($"  - $100 >= $100   : {usd100.IsGreaterThanOrEqual(usd100)}");
        Console.WriteLine($"  - $50  <= $100   : {usd50.IsLessThanOrEqual(usd100)}");
        Console.WriteLine($"  - op $100 > $50  : {usd100 > usd50}");
        Console.WriteLine($"  - op $50  < $100 : {usd50 < usd100}");

        // ToString with format
        Console.WriteLine($"  - Format N4      : {usd100.ToString("N4", null)}");
        Console.WriteLine($"  - Format default : {usd100}");

        // TryFormat
        Span<char> buf = stackalloc char[32];
        if (usd100.TryFormat(buf, out int written, "N2".AsSpan(), null))
        {
            Console.WriteLine($"  - TryFormat N2   : {new string(buf[..written])}");
        }

        // ─── 2. TaxRate — Tax Computation ─────────────────────────────────────────────
        Console.WriteLine("\n[2. TaxRate — Sales Tax Computation]");
        var itbis = TaxRate.Create(18.0m).Value;
        var exempt = TaxRate.Exempt;
        var baseAmount = 850.00m;
        var baseMoney = Money.Create(baseAmount, "DOP").Value;

        Console.WriteLine($"  - ITBIS 18%        : {itbis} (IsExempt: {itbis.IsExempt})");
        Console.WriteLine($"  - Exempt           : {exempt} (IsExempt: {exempt.IsExempt})");
        Console.WriteLine($"  - Tax(decimal)     : {itbis.CalculateTax(baseAmount)} DOP");
        Console.WriteLine($"  - Tax(Money)       : {itbis.CalculateTax(baseMoney)}");

        // ─── 3. DiscountRate — Commercial Discounts ───────────────────────────────────
        Console.WriteLine("\n[3. DiscountRate — Commercial Discount Application]");
        var discount10 = DiscountRate.Create(10.0m).Value;
        var noDiscount = DiscountRate.None;
        var priceBase  = 500.00m;
        var priceMoney = Money.Create(priceBase, "USD").Value;

        Console.WriteLine($"  - Discount 10%          : {discount10} (IsZero: {discount10.IsZero})");
        Console.WriteLine($"  - No Discount           : {noDiscount} (IsZero: {noDiscount.IsZero})");
        Console.WriteLine($"  - Discount(decimal)     : {discount10.CalculateDiscount(priceBase)}");
        Console.WriteLine($"  - ApplyTo(decimal)      : {discount10.ApplyTo(priceBase)} (net price)");
        Console.WriteLine($"  - ApplyTo(Money)        : {discount10.ApplyTo(priceMoney)} (net price)");

        // ─── 4. ExchangeRate — Currency Conversion ───────────────────────────────────
        Console.WriteLine("\n[4. ExchangeRate — Convert and Inverse]");
        var usdToDop = ExchangeRate.Create(CurrencyCode.USD, CurrencyCode.DOP, 60.25m).Value;
        var amountUsd = Money.Create(100.00m, CurrencyCode.USD).Value;

        // Convert
        var convertResult = usdToDop.Convert(amountUsd);
        if (convertResult.IsSuccess)
        {
            Console.WriteLine($"  - $100 USD → DOP  : {convertResult.Value}");
        }

        // Inverse
        var inverseResult = usdToDop.Inverse();
        if (inverseResult.IsSuccess)
        {
            Console.WriteLine($"  - Inverse USD/DOP : {inverseResult.Value}");
            var dopAmount = Money.Create(6025.00m, CurrencyCode.DOP).Value;
            var backToUsd = inverseResult.Value.Convert(dopAmount);
            if (backToUsd.IsSuccess)
            {
                Console.WriteLine($"  - RD$6025 → USD   : {backToUsd.Value}");
            }
        }

        // ─── 5. Email — LocalPart, Domain, Masked ─────────────────────────────────────
        Console.WriteLine("\n[5. Email — LocalPart, Domain, and Masked()]");
        var email = Email.Create("john.doe@enterprise.com").Value;
        Console.WriteLine($"  - Value      : {email.Value}");
        Console.WriteLine($"  - LocalPart  : {email.LocalPart}");
        Console.WriteLine($"  - Domain     : {email.Domain}");
        Console.WriteLine($"  - Masked()   : {email.Masked()}");
        Console.WriteLine($"  - ToString() : {email}  (struct, no mask on ToString)");

        // ─── 6. FullName — Overloads and Component Properties ─────────────────────────
        Console.WriteLine("\n[6. FullName — Create Overloads and Component Properties]");

        // Overload 1: strings
        var fullNameStr = FullName.Create("Erickson", "Lopez").Value;
        Console.WriteLine($"  - FullName(str)    : {fullNameStr}");
        Console.WriteLine($"  - Value            : {fullNameStr.Value}");

        // Overload 2: strongly-typed name components
        var firstNameResult  = FirstName.Create("Maria");
        var lastNameResult   = LastName.Create("Gonzalez");
        var middleNameResult = MiddleName.Create("Fernanda");
        if (firstNameResult.IsSuccess && lastNameResult.IsSuccess && middleNameResult.IsSuccess)
        {
            var fullNameTyped = FullName.Create(firstNameResult.Value, lastNameResult.Value, middleNameResult.Value).Value;
            Console.WriteLine($"  - FullName(typed)  : {fullNameTyped}");
            Console.WriteLine($"  - FirstName.Value  : {fullNameTyped.FirstName.Value}");
            Console.WriteLine($"  - MiddleName.Value : {fullNameTyped.MiddleName?.Value}");
            Console.WriteLine($"  - LastName.Value   : {fullNameTyped.LastName.Value}");
        }

        // ─── 7. Quantity — Arithmetic ─────────────────────────────────────────────────
        Console.WriteLine("\n[7. Quantity — Add, Subtract, IsZero, Zero]");
        var q30 = Quantity.Create(30).Value;
        var q10 = Quantity.Create(10).Value;

        var addResult = q30.Add(q10);
        if (addResult.IsSuccess)
            Console.WriteLine($"  - 30 + 10    : {addResult.Value.Value}");

        var subResult = q30.Subtract(q10);
        if (subResult.IsSuccess)
            Console.WriteLine($"  - 30 - 10    : {subResult.Value.Value}");

        var overdrawnResult = q10.Subtract(q30);
        Console.WriteLine($"  - 10 - 30 err: [{overdrawnResult.Error.Code}]");

        Console.WriteLine($"  - Zero.IsZero: {Quantity.Zero.IsZero}");
        Console.WriteLine($"  - q30.IsZero : {q30.IsZero}");

        // ─── 8. Supply Chain ──────────────────────────────────────────────────────────
        Console.WriteLine("\n[8. Supply Chain and Traceability]");
        var sku       = SKU.Create("PROD-MACBOOK-M3-PRO").Value;
        var barcode   = Barcode.Create("7501031311309").Value;
        var qty       = Quantity.Create(45).Value;
        var warehouse = WarehouseCode.Create("WH-CENTRAL-01").Value;
        var batch     = BatchNumber.Create("LOT-202608-A").Value;
        var serial    = SerialNumber.Create("SN-A1B2C3D4E5").Value;

        Console.WriteLine($"  - SKU        : {sku}");
        Console.WriteLine($"  - Barcode    : {barcode}");
        Console.WriteLine($"  - Quantity   : {qty.Value} units");
        Console.WriteLine($"  - Warehouse  : {warehouse}");
        Console.WriteLine($"  - Batch      : {batch}");
        Console.WriteLine($"  - Serial     : {serial}");

        // ─── 9. Commercial Operations ─────────────────────────────────────────────────
        Console.WriteLine("\n[9. Commercial Operations, Identity, and Address]");
        var customerCode = CustomerCode.Create("CUST-9921").Value;
        var orderNum     = OrderNumber.Create("ORD-2026-8832").Value;
        var channel      = SalesChannelCode.Create("ECOMMERCE_PORTAL").Value;
        var invoiceNum   = DocumentNumber.Create("FAC-B01-00000042").Value;
        var refNum       = ReferenceNumber.Create("REF-PAYPAL-987654").Value;

        Console.WriteLine($"  - Order      : {orderNum} by {customerCode} via {channel}");
        Console.WriteLine($"  - Invoice    : {invoiceNum} (Ref: {refNum})");

        var totalInvoice = Money.Create(100.00m, "USD").Value;
        var allocShares  = totalInvoice.Allocate(1, 1, 1);
        Console.WriteLine($"  - Allocating $100.00 into 3 parts (1:1:1):");
        Console.WriteLine($"    Part 1 : {allocShares[0]}");
        Console.WriteLine($"    Part 2 : {allocShares[1]}");
        Console.WriteLine($"    Part 3 : {allocShares[2]}");
        Console.WriteLine($"    Total  : {allocShares[0].Amount + allocShares[1].Amount + allocShares[2].Amount} USD");

        // ─── 10. Identity and Address ─────────────────────────────────────────────────
        Console.WriteLine("\n[10. Identity, Employees, and Composite Address]");
        var fullName   = FullName.Create("Erickson", "Lopez").Value;
        var company    = CompanyName.Create("EricksonLopez Software Solutions").Value;
        var position   = PositionTitle.Create("Principal Software Architect").Value;
        var dept       = DepartmentName.Create("Cloud Architecture & Security").Value;
        var nationalId = NationalId.Create("001-1234567-8").Value;
        var passport   = PassportNumber.Create("RD98765432").Value;

        var countryDo  = Country.Create("DO").Value;
        var postal     = PostalCode.Create("10148").Value;
        var address    = Address.Create(
            street:     "Av. Winston Churchill #1099, Torre Empresarial",
            city:       "Santo Domingo",
            province:   "Distrito Nacional",
            country:    countryDo,
            postalCode: postal).Value;

        Console.WriteLine($"  - Employee    : {fullName} ({position} in {dept})");
        Console.WriteLine($"  - Company     : {company}");
        Console.WriteLine($"  - National ID : {nationalId} (Masked in logs)");
        Console.WriteLine($"  - Passport    : {passport} (Masked in logs)");
        Console.WriteLine($"  - Address     : {address}");
    }
}
