// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;

namespace EricksonLopez.ValueObjects.Samples.Levels;

/// <summary>
/// Level 01: Quick Start Guide.
/// Demonstrates minimal setup, instantiation via Result pattern, and basic usage of foundational Value Objects.
/// </summary>
public static class Level01_QuickStart
{
    /// <summary>
    /// Executes the quick start demonstration.
    /// </summary>
    public static void Run()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n===============================================================================");
        Console.WriteLine(" [LEVEL 1] QUICK START: INSTANTIATION, RESULT PATTERN, AND BASIC INVARIANTS");
        Console.WriteLine("===============================================================================");
        Console.ResetColor();

        // 1. Instantiation of Email with automatic normalization and PII masking
        Result<Email> emailResult = Email.Create("   john.doe@enterprise.com   ");
        if (emailResult.IsSuccess)
        {
            Email email = emailResult.Value;
            Console.WriteLine($"[Valid Email]");
            Console.WriteLine($"  - Real Value : {email.Value}");
            Console.WriteLine($"  - ToString() : {email} (Masked by [SensitiveData])");
        }

        // 2. Error handling without exceptions (Zero-Allocation Error Handling)
        Result<Email> invalidEmailResult = Email.Create("invalid-email-without-at-sign");
        Console.WriteLine($"\n[Invalid Email]");
        Console.WriteLine($"  - IsFailure : {invalidEmailResult.IsFailure}");
        Console.WriteLine($"  - Code      : {invalidEmailResult.Error.Code}");
        Console.WriteLine($"  - Message   : {invalidEmailResult.Error.Description}");

        // 3. Fowler Money Pattern & CurrencyCode
        Result<Money> moneyResult = Money.Create(1500.75m, "USD");
        if (moneyResult.IsSuccess)
        {
            Money money = moneyResult.Value;
            Console.WriteLine($"\n[Money]");
            Console.WriteLine($"  - Amount   : {money.Amount}");
            Console.WriteLine($"  - Currency : {money.Currency}");
            Console.WriteLine($"  - Format   : {money}");
        }

        // 4. Percentage & Fraction Invariants
        Result<Percentage> percentageResult = Percentage.Create(18.0m);
        if (percentageResult.IsSuccess)
        {
            Percentage vat = percentageResult.Value;
            Console.WriteLine($"\n[Percentage]");
            Console.WriteLine($"  - Percentage : {vat.Value}%");
            Console.WriteLine($"  - Fraction   : {vat.Fraction} (For direct mathematical calculations)");
        }

        // 5. International PhoneNumber
        Result<PhoneNumber> phoneResult = PhoneNumber.Create("+1-809-555-0199");
        if (phoneResult.IsSuccess)
        {
            PhoneNumber phone = phoneResult.Value;
            Console.WriteLine($"\n[PhoneNumber]");
            Console.WriteLine($"  - Value    : {phone.Value}");
            Console.WriteLine($"  - Redact   : {phone}");
        }
    }
}
