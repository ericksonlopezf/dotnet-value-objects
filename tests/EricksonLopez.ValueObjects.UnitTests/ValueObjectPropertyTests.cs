// Copyright © Erickson Lopez. MIT License.
using System;
using System.Linq;
using AwesomeAssertions;
using FsCheck;
using FsCheck.Fluent;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Property-based tests verifying algebraic laws, invariants, and mathematical guarantees across Value Objects.
/// </summary>
public sealed class ValueObjectPropertyTests
{
    private static readonly CurrencyCode Usd = CurrencyCode.USD;

    [Fact]
    public void Money_Addition_IsCommutative()
    {
        // For all a, b where sum does not overflow MaxAbsoluteAmount: a + b == b + a
        var prop = Prop.ForAll(
            ArbMap.Default.GeneratorFor<decimal>()
                .Where(x => Math.Abs(x) < 400_000_000_000_000m && NumericValidation.IsScaleAtMost(x, 2))
                .ToArbitrary(),
            ArbMap.Default.GeneratorFor<decimal>()
                .Where(y => Math.Abs(y) < 400_000_000_000_000m && NumericValidation.IsScaleAtMost(y, 2))
                .ToArbitrary(),
            (a, b) =>
            {
                var m1 = Money.Create(a, Usd).Value;
                var m2 = Money.Create(b, Usd).Value;
                return (m1 + m2) == (m2 + m1);
            });

        Check.QuickThrowOnFailure(prop);
    }

    [Fact]
    public void Money_AdditiveIdentity_PreservesAmount()
    {
        // a + 0 == a and a - 0 == a
        var zero = Money.Zero(Usd);
        var prop = Prop.ForAll(
            ArbMap.Default.GeneratorFor<decimal>()
                .Where(x => Math.Abs(x) < 900_000_000_000_000m && NumericValidation.IsScaleAtMost(x, 2))
                .ToArbitrary(),
            a =>
            {
                var m = Money.Create(a, Usd).Value;
                return (m + zero) == m && (m - zero) == m;
            });

        Check.QuickThrowOnFailure(prop);
    }

    [Fact]
    public void Money_AdditiveInverse_YieldsZero()
    {
        // a + (-a) == 0
        var zero = Money.Zero(Usd);
        var prop = Prop.ForAll(
            ArbMap.Default.GeneratorFor<decimal>()
                .Where(x => Math.Abs(x) < 900_000_000_000_000m && NumericValidation.IsScaleAtMost(x, 2))
                .ToArbitrary(),
            a =>
            {
                var m = Money.Create(a, Usd).Value;
                var neg = m.Negate();
                return (m + neg) == zero;
            });

        Check.QuickThrowOnFailure(prop);
    }

    [Fact]
    public void Money_ProportionalAllocation_ConservesTotalSum()
    {
        // Sum of shares == original total amount (zero penny loss)
        var prop = Prop.ForAll(
            ArbMap.Default.GeneratorFor<decimal>()
                .Where(x => x > 0.01m && x < 100_000m && NumericValidation.IsScaleAtMost(x, 2))
                .ToArbitrary(),
            ArbMap.Default.GeneratorFor<int[]>()
                .Where(r => r is not null && r.Length >= 1 && r.Length <= 10 && r.All(x => x > 0 && x <= 100))
                .ToArbitrary(),
            (amount, ratios) =>
            {
                var m = Money.Create(amount, Usd).Value;
                var shares = m.Allocate(ratios);
                decimal sumShares = shares.Sum(s => s.Amount);
                return sumShares == m.Amount;
            });

        Check.QuickThrowOnFailure(prop);
    }

    [Fact]
    public void Percentage_RoundTrip_PreservesValue()
    {
        // For all valid percentages, ToString -> Parse produces identical percentage
        var prop = Prop.ForAll(
            ArbMap.Default.GeneratorFor<decimal>()
                .Where(p => p >= 0m && p <= 100m && NumericValidation.IsScaleAtMost(p, 4))
                .ToArbitrary(),
            p =>
            {
                var original = Percentage.Create(p).Value;
                var parsed = Percentage.Parse(original.ToString());
                return original == parsed && original.Fraction == (p / 100m);
            });

        Check.QuickThrowOnFailure(prop);
    }

    [Fact]
    public void GeoCoordinate_Distance_IsSymmetricAndZeroForSelf()
    {
        // Distance(A, B) == Distance(B, A) and Distance(A, A) == 0
        var coordGen = ArbMap.Default.GeneratorFor<Tuple<double, double>>()
            .Where(t => !double.IsNaN(t.Item1) && !double.IsNaN(t.Item2) &&
                        t.Item1 >= -85.0 && t.Item1 <= 85.0 &&
                        t.Item2 >= -175.0 && t.Item2 <= 175.0)
            .ToArbitrary();

        var prop = Prop.ForAll(coordGen, coordGen, (t1, t2) =>
        {
            var c1 = GeoCoordinate.Create(Math.Round(t1.Item1, 5), Math.Round(t1.Item2, 5)).Value;
            var c2 = GeoCoordinate.Create(Math.Round(t2.Item1, 5), Math.Round(t2.Item2, 5)).Value;

            double d1 = c1.DistanceToKilometers(c2);
            double d2 = c2.DistanceToKilometers(c1);
            double selfDist = c1.DistanceToKilometers(c1);

            return Math.Abs(d1 - d2) < 0.0001 && selfDist == 0.0;
        });

        Check.QuickThrowOnFailure(prop);
    }

    [Fact]
    public void StringPipeline_UnicodeFormC_IsIdempotent()
    {
        // f(f(s)) == f(s)
        var stringGen = ArbMap.Default.GeneratorFor<string>()
            .Where(s => !string.IsNullOrWhiteSpace(s) && s.Length <= 40 && !s.Any(char.IsControl))
            .ToArbitrary();

        var prop = Prop.ForAll(stringGen, s =>
        {
            var r1 = StringPipeline.RequiredString(s, "TestField", 1, 100);
            if (r1.IsFailure) return true;

            var r2 = StringPipeline.RequiredString(r1.Value, "TestField", 1, 100);
            return r2.IsSuccess && r1.Value == r2.Value;
        });

        Check.QuickThrowOnFailure(prop);
    }
}
