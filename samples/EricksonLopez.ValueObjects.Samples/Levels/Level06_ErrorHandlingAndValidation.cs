// Copyright © Erickson Lopez. MIT License.
using System;
using System.Collections.Generic;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;

namespace EricksonLopez.ValueObjects.Samples.Levels;

/// <summary>
/// Level 06: Error Handling, Invariant Enforcement, Multi-Field Validation, and Domain Contracts.
/// Demonstrates the Result pattern, domain error codes, composite DTO validation, DomainException.ThrowIf(),
/// Percentage full API (FromFraction, ApplyTo, Zero/Hundred/Full, IsZero, Parse/TryParse),
/// comparison operators on struct VOs, and explicit cast operator.
/// </summary>
public static class Level06_ErrorHandlingAndValidation
{
    /// <summary>
    /// Executes error handling and validation demonstrations.
    /// </summary>
    public static void Run()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n===============================================================================");
        Console.WriteLine(" [LEVEL 6] FUNCTIONAL ERROR HANDLING, INVARIANTS, AND DOMAIN CONTRACTS");
        Console.WriteLine("===============================================================================");
        Console.ResetColor();

        // ─── 1. Composite DTO validation ──────────────────────────────────────────────
        Console.WriteLine("[1. Composite Customer Registration DTO Validation]");

        string rawEmail       = "invalid-email";
        string rawPhone       = "123";
        decimal rawPercentage = 150.0m; // Invalid (> 100%)
        string rawCountry     = "";

        List<Error> validationErrors = [];

        var emailRes   = Email.Create(rawEmail);
        if (emailRes.IsFailure) validationErrors.Add(emailRes.Error);

        var phoneRes = PhoneNumber.Create(rawPhone);
        if (phoneRes.IsFailure) validationErrors.Add(phoneRes.Error);

        var pctRes = Percentage.Create(rawPercentage);
        if (pctRes.IsFailure) validationErrors.Add(pctRes.Error);

        var countryRes = Country.Create(rawCountry);
        if (countryRes.IsFailure) validationErrors.Add(countryRes.Error);

        Console.WriteLine($"  - Total captured validation errors: {validationErrors.Count}");
        foreach (var err in validationErrors)
        {
            Console.WriteLine($"    ✖ [{err.Code}] {err.Description}");
        }

        // ─── 2. Result.Match — Safe Fallback Recovery ─────────────────────────────────
        Console.WriteLine("\n[2. Safe Fallback Recovery with Result.Match]");
        var discount = pctRes.Match(
            onSuccess: validPct => validPct,
            onFailure: _ => Percentage.Zero
        );
        Console.WriteLine($"  - Applied discount (Fallback): {discount.Value}%");

        // ─── 3. Percentage — Full API ──────────────────────────────────────────────────
        Console.WriteLine("\n[3. Percentage — Full API]");

        // Static constants
        Console.WriteLine($"  - Percentage.Zero    : {Percentage.Zero}");
        Console.WriteLine($"  - Percentage.Hundred : {Percentage.Hundred}");
        Console.WriteLine($"  - Percentage.Full    : {Percentage.Full}");

        // Create and IsZero
        var pct18  = Percentage.Create(18.0m).Value;
        var pct0   = Percentage.Zero;
        Console.WriteLine($"  - 18%.IsZero         : {pct18.IsZero}");
        Console.WriteLine($"  - 0%.IsZero          : {pct0.IsZero}");
        Console.WriteLine($"  - Fraction of 18%    : {pct18.Fraction}");
        Console.WriteLine($"  - AsFraction of 18%  : {pct18.AsFraction}");

        // FromFraction — construct from decimal fraction
        var fromFrac = Percentage.FromFraction(0.25m).Value;
        Console.WriteLine($"  - FromFraction(0.25) : {fromFrac}");

        // ValidatePercentage — shared static validation method
        var valResult = Percentage.ValidatePercentage(101.0m, "TestField");
        Console.WriteLine($"  - ValidatePercentage(101): IsFailure={valResult.IsFailure}, Error=[{valResult.Error.Code}]");

        // ApplyTo — calculate percentage of a base amount
        decimal baseAmt = 1000.00m;
        decimal applied = pct18.ApplyTo(baseAmt);
        Console.WriteLine($"  - 18% ApplyTo(1000)  : {applied}");

        // Comparison operators
        var pct25 = Percentage.Create(25.0m).Value;
        Console.WriteLine($"  - 18% < 25%          : {pct18 < pct25}");
        Console.WriteLine($"  - 25% > 18%          : {pct25 > pct18}");
        Console.WriteLine($"  - 18% <= 18%         : {pct18 <= Percentage.Create(18.0m).Value}");
        Console.WriteLine($"  - 25% >= 18%         : {pct25 >= pct18}");

        // Parse and TryParse
        var parsedPct = Percentage.Parse("33.33%");
        Console.WriteLine($"  - Parse('33.33%')    : {parsedPct}");

        if (Percentage.TryParse("67.5", null, out var tryParsedPct))
        {
            Console.WriteLine($"  - TryParse('67.5')   : {tryParsedPct}");
        }

        // ─── 4. Comparison operators on struct Value Objects ───────────────────────────
        Console.WriteLine("\n[4. Comparison Operators on Struct VOs]");

        // TaxRate operators
        var taxA = TaxRate.Create(18.0m).Value;
        var taxB = TaxRate.Create(21.0m).Value;
        Console.WriteLine($"  - TaxRate 18% < 21%  : {taxA < taxB}");
        Console.WriteLine($"  - TaxRate 21% > 18%  : {taxB > taxA}");
        Console.WriteLine($"  - TaxRate 18% <= 18% : {taxA <= TaxRate.Create(18.0m).Value}");
        Console.WriteLine($"  - TaxRate 21% >= 18% : {taxB >= taxA}");

        // Quantity operators
        var qty5  = Quantity.Create(5).Value;
        var qty10 = Quantity.Create(10).Value;
        Console.WriteLine($"  - Qty 5 < 10         : {qty5 < qty10}");
        Console.WriteLine($"  - Qty 10 > 5         : {qty10 > qty5}");
        Console.WriteLine($"  - Qty 5 <= 5         : {qty5 <= Quantity.Create(5).Value}");
        Console.WriteLine($"  - Qty 10 >= 5        : {qty10 >= qty5}");

        // ─── 5. Explicit Cast Operator (TSelf → TValue) ────────────────────────────────
        Console.WriteLine("\n[5. Explicit Cast Operator — (TValue)valueObject]");

        // SingleValueObject<TSelf,TValue> defines: public static explicit operator TValue(...)
        // Only available on record-based SingleValueObject (reference types), not structs.
        var tenantCode  = TenantCode.Create("my-tenant").Value;
        string rawTenantStr = (string)tenantCode;   // explicit cast to underlying string
        Console.WriteLine($"  - TenantCode explicit cast: '{rawTenantStr}'");

        var companyName = CompanyName.Create("Acme Corp").Value;
        string rawCompany = (string)companyName;
        Console.WriteLine($"  - CompanyName explicit cast: '{rawCompany}'");

        // ─── 6. DomainException.ThrowIf — Guard clauses ────────────────────────────────
        Console.WriteLine("\n[6. DomainException.ThrowIf — Guard for Programmer Invariant Violations]");

        try
        {
            int parts = 0;
            // This guard protects against programmer errors (dividing money into 0 parts).
            // DomainException is reserved for domain-boundary invariant violations, NOT business validation.
            DomainException.ThrowIf(parts <= 0, $"Cannot distribute Money into {parts} parts.");
        }
        catch (DomainException ex)
        {
            Console.WriteLine($"  - DomainException caught: {ex.Message}");
        }

        // Correct usage — no exception
        try
        {
            DomainException.ThrowIf(false, "This should not throw.");
            Console.WriteLine("  - DomainException.ThrowIf(false) → no exception ✔");
        }
        catch (DomainException)
        {
            Console.WriteLine("  ✖ ERROR: This should NOT have thrown.");
        }
    }
}
