// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.ValueObjects;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Randomized Property-based tests to ensure mathematical invariants of Money.
/// Evaluates thousands of randomized inputs per test to prove domain safety.
/// </summary>
public class MoneyPropertyTests
{
    private const int Iterations = 10000;

    [Fact]
    public void Addition_IsCommutative()
    {
        for (int i = 0; i < Iterations; i++)
        {
            var a = GenerateRandomMoney();
            var b = GenerateRandomMoney(a.Currency);

            if (IsOverflow(a.Amount, b.Amount, true)) continue;

            Assert.Equal(a + b, b + a);
        }
    }

    [Fact]
    public void Addition_And_Subtraction_Are_Inverses()
    {
        for (int i = 0; i < Iterations; i++)
        {
            var a = GenerateRandomMoney();
            var b = GenerateRandomMoney(a.Currency);

            if (IsOverflow(a.Amount, b.Amount, true)) continue;

            var sum = a + b;

            if (IsOverflow(sum.Amount, b.Amount, false)) continue;

            Assert.Equal(a, sum - b);
        }
    }

    [Fact]
    public void Multiplication_By_One_Is_Identity()
    {
        for (int i = 0; i < Iterations; i++)
        {
            var a = GenerateRandomMoney();
            var result = a.TryMultiply(1m);

            Assert.True(result.IsSuccess);
            Assert.Equal(a, result.Value);
        }
    }

    [Fact]
    public void Multiplication_By_Zero_Is_Zero()
    {
        for (int i = 0; i < Iterations; i++)
        {
            var a = GenerateRandomMoney();
            var result = a.TryMultiply(0m);

            Assert.True(result.IsSuccess);
            Assert.Equal(0m, result.Value.Amount);
        }
    }

    [Fact]
    public void Allocation_Conserves_TotalAmount()
    {
        var random = new Random();
        for (int i = 0; i < Iterations; i++)
        {
            var total = GenerateRandomMoney();

            int[] ratios = new int[random.Next(1, 100)];
            long sum = 0;
            for (int j = 0; j < ratios.Length; j++)
            {
                ratios[j] = random.Next(1, 1000);
                sum += ratios[j];
            }

            if (sum > int.MaxValue) continue;

            var allocated = total.Allocate(ratios);

            decimal allocatedSum = 0;
            foreach (var m in allocated)
            {
                allocatedSum += m.Amount;
            }

            Assert.Equal(total.Amount, allocatedSum);
        }
    }

    private static bool IsOverflow(decimal a, decimal b, bool addition)
    {
        try
        {
            if (addition) _ = a + b;
            else _ = a - b;
            return false;
        }
        catch (OverflowException)
        {
            return true;
        }
    }

    private static Money GenerateRandomMoney(CurrencyCode? forceCurrency = null)
    {
        var random = new Random(Guid.NewGuid().GetHashCode());

        CurrencyCode[] currencies = { CurrencyCode.USD, CurrencyCode.EUR, CurrencyCode.GBP, CurrencyCode.DOP };
        CurrencyCode currency = forceCurrency ?? currencies[random.Next(currencies.Length)];

        // Generate random decimal in range [-1,000,000, 1,000,000]
        decimal amount = (decimal)(random.NextDouble() * 2_000_000 - 1_000_000);

        // Round to currency scale
        amount = Math.Round(amount, currency.DecimalPlaces);

        return Money.Create(amount, currency).Value;
    }
}
