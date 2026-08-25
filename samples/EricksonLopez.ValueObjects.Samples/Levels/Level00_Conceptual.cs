// Copyright © Erickson Lopez. MIT License.
using System;

namespace EricksonLopez.ValueObjects.Samples.Levels;

/// <summary>
/// Level 00: Conceptual Foundation of the EricksonLopez.ValueObjects Ecosystem.
/// Explains the architectural principles, benefits, trade-offs, and comparison with alternatives.
/// </summary>
public static class Level00_Conceptual
{
    /// <summary>
    /// Executes the conceptual demonstration and prints architectural rationale.
    /// </summary>
    public static void Run()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n===============================================================================");
        Console.WriteLine(" [LEVEL 0] CONCEPTUAL: ARCHITECTURAL PRINCIPLES AND DESIGN PHILOSOPHY");
        Console.WriteLine("===============================================================================");
        Console.ResetColor();

        Console.WriteLine(@"
1. WHAT IS ERICKSONLOPEZ.VALUEOBJECTS?
   It is a modular enterprise Value Objects ecosystem for .NET 10 (C# 13)
   designed under strict principles of Domain-Driven Design (DDD), absolute
   immutability, zero-allocation performance, and native Ahead-of-Time (AOT) support.

2. WHAT PROBLEM DOES IT SOLVE?
   - Eliminates the 'Primitive Obsession' anti-pattern.
   - Prevents accidental semantic type mixing (e.g. passing a TaxRate instead of DiscountRate).
   - Enforces domain invariants upon instantiation (impossible to construct an invalid VO).
   - Prevents sensitive data (PII) leaks in logs via the [SensitiveData] attribute.
   - Supports multi-country fiscal regulations (Argentina, Chile, Colombia, Dominican Republic, Mexico, Peru).

3. ARCHITECTURAL PILLARS:
   a) Absolute Immutability: readonly record struct and sealed record with private constructors.
   b) Zero-Allocation Abstractions: Result<T> on the stack without unnecessary heap allocations.
   c) Domain Purity: Core library without dependencies on web or persistence frameworks.
   d) Clean Infrastructure Adapters: EF Core 10 ValueConverters, Dapper TypeHandlers, STJ Converters.

4. COMPARISON:
   ┌──────────────────────────────┬──────────────────────────────┬──────────────────────────────┐
   │ Feature                      │ Standard Primitives (string) │ EricksonLopez.ValueObjects   │
   ├──────────────────────────────┼──────────────────────────────┼──────────────────────────────┤
   │ Invariant Validation         │ Manual and scattered         │ Centralized and Immutable    │
   │ Type Safety                  │ Weak (string == string)      │ Strong (Email != SKU)        │
   │ PII Protection in Logs       │ Vulnerable to leaks          │ Automatic [SensitiveData]    │
   │ Multi-Country Fiscal Support │ None                         │ 6 Supported Jurisdictions    │
   │ EF Core / Dapper Integration │ N/A                          │ Out-of-the-box Extensions    │
   └──────────────────────────────┴──────────────────────────────┴──────────────────────────────┘
");
    }
}
