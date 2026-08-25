// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic.UnitTests;

/// <summary>
/// Generative and property-based unit tests verifying algebraic invariants,
/// bound coherence, transitivity, and continuity across randomized state spaces
/// for Dominican Fiscal Periods.
/// </summary>
public sealed class FiscalPeriodGenerativeTests
{
    private const int Seed = 42_1337;

    [Fact]
    public void FiscalPeriod_NavigationAndBounds_SatisfiesAlgebraicInvariants_AcrossGenerativeStateSpace()
    {
        var random = new Random(Seed);

        for (int iteration = 0; iteration < 2000; iteration++)
        {
            int year = random.Next(2001, 2100); // 2001 to 2099 safely avoids edge case overflows in tests
            int month = random.Next(1, 13);

            var periodResult = FiscalPeriod.Create(year, month);
            periodResult.IsSuccess.Should().BeTrue($"Creation failed for {year}-{month}");
            var period = periodResult.Value;

            // Invariant 1: Involution of Navigation (p.Next().Previous() == p)
            var next = period.Next();
            var prev = period.Previous();

            next.Previous().Should().Be(period, $"failed next->previous involution at iteration {iteration}");
            prev.Next().Should().Be(period, $"failed previous->next involution at iteration {iteration}");

            // Invariant 2: Bounds Coherence
            var firstDay = period.Start;
            var lastDay = period.End;

            firstDay.Year.Should().Be(period.Year);
            firstDay.Month.Should().Be(period.Month);
            firstDay.Day.Should().Be(1);

            lastDay.Year.Should().Be(period.Year);
            lastDay.Month.Should().Be(period.Month);
            (lastDay >= firstDay).Should().BeTrue(); // Ensure range is valid

            // Invariant 3: Continuity of Dates across periods
            var nextFirstDay = next.Start;
            lastDay.AddDays(1).Should().Be(nextFirstDay, $"Continuity failed: last day of {period} + 1 day should be first day of {next}");
        }
    }

    [Fact]
    public void FiscalPeriod_OrderingAndEquality_IsTransitiveAndConsistent_AcrossGenerativeStateSpace()
    {
        var random = new Random(Seed);

        for (int iteration = 0; iteration < 1000; iteration++)
        {
            int yearA = random.Next(2000, 2050);
            int monthA = random.Next(1, 13);

            var periodA = FiscalPeriod.Create(yearA, monthA).Value;
            var periodB = periodA.Next();
            var periodC = periodB.Next();
            var periodACopy = FiscalPeriod.Create(yearA, monthA).Value;

            // Property 1: Transitivity (A < B and B < C => A < C)
            (periodA < periodB).Should().BeTrue();
            (periodB < periodC).Should().BeTrue();
            (periodA < periodC).Should().BeTrue();

            // Property 2: Equality consistency
            (periodA == periodACopy).Should().BeTrue();
            (periodA.CompareTo(periodACopy)).Should().Be(0);
            (periodA.GetHashCode()).Should().Be(periodACopy.GetHashCode());

            // Property 3: Non-equality
            (periodA != periodB).Should().BeTrue();
            (periodA > periodB).Should().BeFalse();
        }
    }
}
