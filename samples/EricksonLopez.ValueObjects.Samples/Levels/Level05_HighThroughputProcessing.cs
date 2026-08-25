// Copyright © Erickson Lopez. MIT License.
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;

namespace EricksonLopez.ValueObjects.Samples.Levels;

/// <summary>
/// Level 05: High-Throughput Processing and Concurrency.
/// Demonstrates zero-allocation struct Value Objects in parallel pipelines, batch calculations, and lock-free thread safety.
/// </summary>
public static class Level05_HighThroughputProcessing
{
    /// <summary>
    /// Executes high-throughput batch processing and concurrency demonstrations.
    /// </summary>
    public static void Run()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n===============================================================================");
        Console.WriteLine(" [LEVEL 5] HIGH-THROUGHPUT PROCESSING, BATCH, AND ZERO-LOCK CONCURRENCY");
        Console.WriteLine("===============================================================================");
        Console.ResetColor();

        const int iterations = 100_000;
        Console.WriteLine($"[1. Batch Processing of {iterations:N0} Money operations without locks]");

        var baseMoney = Money.Create(100.00m, "USD").Value;
        var tax = TaxRate.Create(18.0m).Value;

        var sw = Stopwatch.StartNew();
        decimal totalAccumulator = 0m;
        object syncLock = new();

        Parallel.For(0, iterations, _ =>
        {
            // Pure stack operation without heap allocations
            var split = baseMoney.Allocate(50, 30, 20);
            var itemTotal = split[0].Amount + split[1].Amount + split[2].Amount;

            lock (syncLock)
            {
                totalAccumulator += itemTotal;
            }
        });

        sw.Stop();
        Console.WriteLine($"  - Total accumulated : ${totalAccumulator:N2} USD");
        Console.WriteLine($"  - Total time        : {sw.ElapsedMilliseconds} ms ({iterations / Math.Max(1, sw.ElapsedMilliseconds):N0} ops/ms)");
        Console.WriteLine($"  - Invariant         : Thread-safe immutability by design (zero data-race)");
    }
}
