// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using System.Linq;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Generative and property-based unit tests verifying algebraic invariants,
/// conservation of mass/money, symmetry, reflexivity, and round-trip consistency across randomized state spaces.
/// </summary>
public sealed class GenerativePropertyTests
{
    private const int Seed = 42_1337;

    [Fact]
    public void Money_Allocate_ConservesTotalAmountWithoutPennyLoss_AcrossGenerativeStateSpace()
    {
        var random = new Random(Seed);

        // 2,500 randomized iterations across varying amounts and partitions
        for (int iteration = 0; iteration < 2500; iteration++)
        {
            // Amount between 0.01 and 1,000,000.00
            decimal amount = Math.Round((decimal)(random.NextDouble() * 1_000_000.0) + 0.01m, 2);
            var money = Money.Create(amount, CurrencyCode.USD).Value;

            // Generate between 2 and 15 random ratio weights (1..100)
            int ratioCount = random.Next(2, 16);
            int[] ratios = new int[ratioCount];
            for (int r = 0; r < ratioCount; r++)
            {
                ratios[r] = random.Next(1, 101);
            }

            var parts = money.Allocate(ratios);
            parts.Length.Should().Be(ratioCount);

            // Invariant 1: Sum of allocated parts MUST EXACTLY EQUAL the original amount (Conservation of Money)
            decimal sumAllocated = parts.Sum(p => p.Amount);
            sumAllocated.Should().Be(money.Amount, $"failed conservation at iteration {iteration} with amount {amount}");

            // Invariant 2: All parts share the same currency
            parts.All(p => p.Currency == CurrencyCode.USD).Should().BeTrue();

            // Invariant 3: No part is negative if original amount is positive
            parts.All(p => p.Amount >= 0m).Should().BeTrue();
        }
    }

    [Fact]
    public void TimeRange_Overlaps_IsSymmetricAndReflexive_AcrossGenerativeStateSpace()
    {
        var random = new Random(Seed);

        for (int iteration = 0; iteration < 1000; iteration++)
        {
            var startA = new TimeOnly(random.Next(0, 23), random.Next(0, 60));
            var endA = new TimeOnly(random.Next(0, 23), random.Next(0, 60));
            if (startA == endA)
            {
                continue;
            }

            var rangeAResult = TimeRange.Create(startA, endA, allowOvernight: true);
            if (rangeAResult.IsFailure)
            {
                continue;
            }

            var rangeA = rangeAResult.Value;

            // Property 1: Reflexivity — Any range overlaps with itself
            rangeA.Overlaps(rangeA).Should().BeTrue();

            var startB = new TimeOnly(random.Next(0, 23), random.Next(0, 60));
            var endB = new TimeOnly(random.Next(0, 23), random.Next(0, 60));
            if (startB == endB)
            {
                continue;
            }

            var rangeBResult = TimeRange.Create(startB, endB, allowOvernight: true);
            if (rangeBResult.IsFailure)
            {
                continue;
            }

            var rangeB = rangeBResult.Value;

            // Property 2: Symmetry — Overlaps(A, B) == Overlaps(B, A)
            bool aOverlapsB = rangeA.Overlaps(rangeB);
            bool bOverlapsA = rangeB.Overlaps(rangeA);

            aOverlapsB.Should().Be(bOverlapsA, $"failed symmetry at iteration {iteration}");
        }
    }

    [Fact]
    public void Range_OverlapsAndContains_SatisfiesAlgebraicProperties_AcrossGenerativeStateSpace()
    {
        var random = new Random(Seed);

        for (int iteration = 0; iteration < 1000; iteration++)
        {
            int s1 = random.Next(-1000, 1000);
            int e1 = s1 + random.Next(1, 500);
            var rangeA = Range<int>.Create(s1, e1).Value;

            int s2 = random.Next(-1000, 1000);
            int e2 = s2 + random.Next(1, 500);
            var rangeB = Range<int>.Create(s2, e2).Value;

            // Property 1: Reflexivity
            rangeA.Overlaps(rangeA).Should().BeTrue();

            // Property 2: Symmetry
            rangeA.Overlaps(rangeB).Should().Be(rangeB.Overlaps(rangeA));

            // Property 3: If a point x is contained in both rangeA and rangeB, they MUST overlap
            int testPoint = random.Next(-1000, 1500);
            if (rangeA.Contains(testPoint) && rangeB.Contains(testPoint))
            {
                rangeA.Overlaps(rangeB).Should().BeTrue();
            }

            // Property 4: Contains bounds
            rangeA.Contains(rangeA.Start).Should().BeTrue();
            rangeA.Contains(rangeA.End).Should().BeTrue();
            rangeA.Contains(rangeA.Start - 1).Should().BeFalse();
            rangeA.Contains(rangeA.End + 1).Should().BeFalse();
        }
    }

    [Fact]
    public void PercentageAndTaxRate_SpanParsing_PreservesExactValue_AcrossGenerativeStateSpace()
    {
        var random = new Random(Seed);

        for (int iteration = 0; iteration < 1000; iteration++)
        {
            // Decimal in [0.0000, 100.0000]
            decimal val = Math.Round((decimal)(random.NextDouble() * 100.0), 4);

            var pctResult = Percentage.Create(val);
            pctResult.IsSuccess.Should().BeTrue();

            string formatted = val.ToString("G", CultureInfo.InvariantCulture);
            ReadOnlySpan<char> span = formatted.AsSpan();

            bool parsed = Percentage.TryParse(span, CultureInfo.InvariantCulture, out var parsedPct);
            parsed.Should().BeTrue();
            parsedPct.Value.Should().Be(val);

            var taxResult = TaxRate.Create(val);
            taxResult.IsSuccess.Should().BeTrue();

            bool taxParsed = TaxRate.TryParse(span, CultureInfo.InvariantCulture, out var parsedTax);
            taxParsed.Should().BeTrue();
            parsedTax.Value.Should().Be(val);
        }
    }

    [Fact]
    public void DateRange_DaysAndOverlaps_SatisfiesAlgebraicInvariants_AcrossGenerativeStateSpace()
    {
        var random = new Random(Seed);
        var baseDate = new DateOnly(2020, 1, 1);

        for (int iteration = 0; iteration < 1000; iteration++)
        {
            int offset1 = random.Next(0, 3650); // within 10 years
            int duration1 = random.Next(0, 365);
            var start1 = baseDate.AddDays(offset1);
            var end1 = start1.AddDays(duration1);

            var range1 = Range<DateOnly>.Create(start1, end1).Value;

            // Invariant 1: Days calculation exactly matches DayNumber difference
            range1.Days().Should().Be(end1.DayNumber - start1.DayNumber);

            // Invariant 2: Reflexivity of Overlaps
            range1.Overlaps(range1).Should().BeTrue();

            int offset2 = random.Next(0, 3650);
            int duration2 = random.Next(0, 365);
            var start2 = baseDate.AddDays(offset2);
            var end2 = start2.AddDays(duration2);

            var range2 = Range<DateOnly>.Create(start2, end2).Value;

            // Invariant 3: Symmetry of Overlaps
            range1.Overlaps(range2).Should().Be(range2.Overlaps(range1));

            // Invariant 4: Contains bounds
            range1.Contains(range1.Start).Should().BeTrue();
            range1.Contains(range1.End).Should().BeTrue();
            if (range1.Start > DateOnly.MinValue)
            {
                range1.Contains(range1.Start.AddDays(-1)).Should().BeFalse();
            }
            if (range1.End < DateOnly.MaxValue)
            {
                range1.Contains(range1.End.AddDays(1)).Should().BeFalse();
            }
        }
    }

    [Fact]
    public void StringPipeline_NormalizationMethods_AreIdempotent_AcrossGenerativeStateSpace()
    {
        var random = new Random(Seed);
        string[] sampleChars = ["a", "B", " ", "   ", "\t", "é", "ñ", "1", "2", ".", "-", "'", ","];

        for (int iteration = 0; iteration < 1000; iteration++)
        {
            int length = random.Next(1, 25);
            var chars = new string[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = sampleChars[random.Next(sampleChars.Length)];
            }
            string raw = string.Concat(chars);

            // BusinessName Normalization Idempotency: f(f(x)) == f(x)
            string normBiz1 = StringPipeline.NormalizeBusinessName(raw);
            string normBiz2 = StringPipeline.NormalizeBusinessName(normBiz1);
            normBiz2.Should().Be(normBiz1, $"failed BusinessName idempotence at iteration {iteration} for '{raw}'");

            // HumanName Normalization Idempotency: f(f(x)) == f(x)
            string normHuman1 = StringPipeline.NormalizeHumanName(raw);
            string normHuman2 = StringPipeline.NormalizeHumanName(normHuman1);
            normHuman2.Should().Be(normHuman1, $"failed HumanName idempotence at iteration {iteration} for '{raw}'");
        }
    }

    [Fact]
    public void Money_Distribute_ConservesTotalAmountAndExactCents_AcrossGenerativeStateSpace()
    {
        var random = new Random(Seed);

        for (int iteration = 0; iteration < 2000; iteration++)
        {
            decimal amount = Math.Round((decimal)(random.NextDouble() * 500_000.0) + 0.05m, 2);
            var money = Money.Create(amount, CurrencyCode.USD).Value;
            int partsCount = random.Next(2, 25);

            var parts = money.Distribute(partsCount);

            // Property 1: Exact count of parts
            parts.Length.Should().Be(partsCount);

            // Property 2: Conservation of Total Amount (zero loss)
            decimal sum = parts.Sum(p => p.Amount);
            sum.Should().Be(money.Amount, $"failed Distribute conservation at iteration {iteration}");

            // Property 3: Invariant currency and non-negativity
            parts.All(p => p.Currency == CurrencyCode.USD).Should().BeTrue();
            parts.All(p => p.Amount >= 0m).Should().BeTrue();
        }
    }

    [Fact]
    public void BusinessDate_OrderingAndEquality_IsTransitiveAndConsistent_AcrossGenerativeStateSpace()
    {
        var random = new Random(Seed);
        var baseDate = new DateOnly(2025, 1, 1);

        for (int iteration = 0; iteration < 1000; iteration++)
        {
            int d1 = random.Next(1, 3000);
            int d2 = d1 + random.Next(1, 100);
            int d3 = d2 + random.Next(1, 100);

            var dateA = BusinessDate.Create(baseDate.AddDays(d1)).Value;
            var dateB = BusinessDate.Create(baseDate.AddDays(d2)).Value;
            var dateC = BusinessDate.Create(baseDate.AddDays(d3)).Value;
            var dateACopy = BusinessDate.Create(baseDate.AddDays(d1)).Value;

            // Property 1: Transitivity (A < B and B < C => A < C)
            (dateA < dateB).Should().BeTrue();
            (dateB < dateC).Should().BeTrue();
            (dateA < dateC).Should().BeTrue();

            // Property 2: Equality consistency
            (dateA == dateACopy).Should().BeTrue();
            (dateA.CompareTo(dateACopy)).Should().Be(0);
            (dateA.GetHashCode()).Should().Be(dateACopy.GetHashCode());

            // Property 3: Non-equality
            (dateA != dateB).Should().BeTrue();
            (dateA > dateB).Should().BeFalse();
        }
    }

    [Fact]
    public void Range_Intersects_IsCommutativeAndIdempotent_AcrossGenerativeStateSpace()
    {
        var random = new Random(Seed);

        for (int iteration = 0; iteration < 1000; iteration++)
        {
            int s1 = random.Next(-1000, 1000);
            int e1 = s1 + random.Next(1, 300);
            var rangeA = Range<int>.Create(s1, e1).Value;

            int s2 = random.Next(-1000, 1000);
            int e2 = s2 + random.Next(1, 300);
            var rangeB = Range<int>.Create(s2, e2).Value;

            // Property 1: Idempotence of Intersection (A intersects A == A)
            bool selfIntersect = rangeA.Intersects(rangeA, out var selfResult);
            selfIntersect.Should().BeTrue();
            selfResult.Should().Be(rangeA);

            // Property 2: Commutativity of Intersection (A intersects B <=> B intersects A, and results are equal)
            bool aIntersectsB = rangeA.Intersects(rangeB, out var resAB);
            bool bIntersectsA = rangeB.Intersects(rangeA, out var resBA);

            aIntersectsB.Should().Be(bIntersectsA, $"failed commutativity existence at iteration {iteration}");
            if (aIntersectsB)
            {
                resAB.Should().Be(resBA, $"failed commutativity result at iteration {iteration}");

                // Property 3: Intersection subset property
                rangeA.Contains(resAB).Should().BeTrue();
                rangeB.Contains(resAB).Should().BeTrue();
            }
            else
            {
                // Property 4: Disjoint ranges do not overlap
                rangeA.Overlaps(rangeB).Should().BeFalse();
            }
        }
    }
}
