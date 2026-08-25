// Copyright © Erickson Lopez. MIT License.
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;

namespace EricksonLopez.ValueObjects.Samples.Levels;

/// <summary>
/// Level 07: Scalability, Zero-Allocation Memory Model, NativeAOT Compatibility, and Span-based Parsing.
/// Demonstrates ReadOnlySpan&lt;char&gt; parsing on TaxRate, Percentage, Quantity, BusinessDate, and Money,
/// explicit cast operator, and the GC heap footprint of struct VOs.
/// </summary>
public static class Level07_ZeroAllocationAot
{
    /// <summary>
    /// Executes zero-allocation and NativeAOT demonstrations.
    /// </summary>
    public static void Run()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n===============================================================================");
        Console.WriteLine(" [LEVEL 7] SCALABILITY, ZERO-ALLOCATION MEMORY FOOTPRINT, AND NATIVE AOT");
        Console.WriteLine("===============================================================================");
        Console.ResetColor();

        // ─── 1. Zero-Allocation Parsing with ReadOnlySpan<char> — TaxRate ───────────────
        Console.WriteLine("[1. Zero-Allocation Parsing with ReadOnlySpan<char>]");

        ReadOnlySpan<char> rawTaxSpan = "18.50".AsSpan();
        if (TaxRate.TryParse(rawTaxSpan, null, out var parsedTax))
        {
            Console.WriteLine($"  - TaxRate.TryParse(Span)       : {parsedTax} (Zero heap allocation)");
        }

        var taxFromSpan = TaxRate.Parse("21%".AsSpan(), null);
        Console.WriteLine($"  - TaxRate.Parse(Span '21%')    : {taxFromSpan}");

        // ─── 2. Percentage.Parse / TryParse ────────────────────────────────────────────
        Console.WriteLine("\n[2. Percentage — Parse and TryParse from string and Span]");

        var pctFromString = Percentage.Parse("33.33%");
        Console.WriteLine($"  - Percentage.Parse(str '33.33%'): {pctFromString}");

        ReadOnlySpan<char> pctSpan = "75".AsSpan();
        if (Percentage.TryParse(pctSpan, null, out var pctFromSpan))
        {
            Console.WriteLine($"  - Percentage.TryParse(Span '75'): {pctFromSpan}");
        }

        // ─── 3. Quantity.Parse / TryParse ──────────────────────────────────────────────
        Console.WriteLine("\n[3. Quantity — Parse and TryParse from string and Span]");

        var qtyFromString = Quantity.Parse("1,000"); // Supports thousands separator
        Console.WriteLine($"  - Quantity.Parse('1,000')      : {qtyFromString.Value}");

        ReadOnlySpan<char> qtySpan = "250".AsSpan();
        if (Quantity.TryParse(qtySpan, null, out var qtyFromSpan))
        {
            Console.WriteLine($"  - Quantity.TryParse(Span '250'): {qtyFromSpan.Value}");
        }

        if (!Quantity.TryParse("-5", null, out _))
        {
            Console.WriteLine("  - Quantity.TryParse('-5')      : false (invalid negative) ✔");
        }

        // ─── 4. BusinessDate.Parse / TryParse ──────────────────────────────────────────
        Console.WriteLine("\n[4. BusinessDate — Parse and TryParse from string and Span]");

        var bdFromString = BusinessDate.Parse("2026-12-31");
        Console.WriteLine($"  - BusinessDate.Parse('2026-12-31')        : {bdFromString}");

        ReadOnlySpan<char> bdSpan = "2026-01-15".AsSpan();
        if (BusinessDate.TryParse(bdSpan, null, out var bdFromSpan))
        {
            Console.WriteLine($"  - BusinessDate.TryParse(Span '2026-01-15'): {bdFromSpan}");
        }

        if (!BusinessDate.TryParse("not-a-date", null, out _))
        {
            Console.WriteLine("  - BusinessDate.TryParse('not-a-date')     : false ✔");
        }

        // ─── 5. DiscountRate.Parse / TryParse ──────────────────────────────────────────
        Console.WriteLine("\n[5. DiscountRate — Parse and TryParse]");

        var drFromString = DiscountRate.Parse("5.5%");
        Console.WriteLine($"  - DiscountRate.Parse('5.5%')      : {drFromString}");

        if (DiscountRate.TryParse("10", null, out var drFromStr))
        {
            Console.WriteLine($"  - DiscountRate.TryParse('10')     : {drFromStr}");
        }

        // ─── 6. Money.TryFormat (ISpanFormattable) ─────────────────────────────────────
        Console.WriteLine("\n[6. Money.TryFormat — ISpanFormattable (stack-friendly output)]");

        var amount = Money.Create(1_234.567m, "USD").Value;

        Span<char> buf = stackalloc char[64];

        // N2 format
        if (amount.TryFormat(buf, out int written, "N2".AsSpan(), null))
        {
            Console.WriteLine($"  - TryFormat N2 : {new string(buf[..written])}");
        }

        // N4 format
        if (amount.TryFormat(buf, out written, "N4".AsSpan(), null))
        {
            Console.WriteLine($"  - TryFormat N4 : {new string(buf[..written])}");
        }

        // Money.ToString(format, provider)
        Console.WriteLine($"  - ToString N0  : {amount.ToString("N0", null)}");

        // ─── 7. GC Heap Footprint — struct VOs allocated on stack ──────────────────────
        Console.WriteLine("\n[7. GC Heap Footprint — Struct VOs on Stack]");
        long allocatedBefore = GC.GetAllocatedBytesForCurrentThread();

        for (int i = 0; i < 1_000; i++)
        {
            var m1  = Money.Create(100m, "USD").Value;
            var m2  = Money.Create(50m, "USD").Value;
            var sum = m1 + m2;
            _ = sum.Amount;
        }

        long allocatedAfter = GC.GetAllocatedBytesForCurrentThread();
        long diff = allocatedAfter - allocatedBefore;

        Console.WriteLine($"  - Allocations across 1,000 Money ops: {diff} bytes on Heap");
        Console.WriteLine($"  - Native AOT Compatibility: 100% free of dynamic Reflection.");

        // ─── 8. Parallel Batch — Thread-safety demonstration ──────────────────────────
        Console.WriteLine("\n[8. Parallel Batch — Thread-safety of Immutable Structs]");
        const int iterations = 50_000;
        var baseMoney = Money.Create(100.00m, "USD").Value;
        decimal totalAccumulator = 0m;
        object syncLock = new();
        var sw = Stopwatch.StartNew();

        Parallel.For(0, iterations, _ =>
        {
            var split     = baseMoney.Allocate(50, 30, 20);
            var itemTotal = split[0].Amount + split[1].Amount + split[2].Amount;
            lock (syncLock) { totalAccumulator += itemTotal; }
        });

        sw.Stop();
        Console.WriteLine($"  - {iterations:N0} ops in {sw.ElapsedMilliseconds} ms → Total ${totalAccumulator:N2} USD");
    }
}
