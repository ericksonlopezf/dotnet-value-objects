// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests validating zero heap memory allocation for performance-critical Value Object operations.
/// </summary>
public sealed class ZeroAllocationTests
{
    [Theory]
    [InlineData("USD")]
    [InlineData("EUR")]
    [InlineData("DOP")]
    [InlineData("GBP")]
    [InlineData("JPY")]
    [InlineData("BHD")]
    [InlineData("KWD")]
    public void CurrencyCode_WhenParsedFromSpan_PerformsZeroAllocations(string input)
    {
        ValueObjectContractExtensions.AssertZeroAllocations(() =>
        {
            var success = CurrencyCode.TryParse(input.AsSpan(), CultureInfo.InvariantCulture, out var result);
            if (!success || result.Value != input)
            {
                throw new InvalidOperationException($"Failed to parse {input}");
            }
        });
    }

    [Fact]
    public void Percentage_WhenCreatedAndParsedFromSpan_PerformsZeroAllocations()
    {
        const string input = "25.50";
        ValueObjectContractExtensions.AssertZeroAllocations(() =>
        {
            var success = Percentage.TryParse(input.AsSpan(), CultureInfo.InvariantCulture, out var result);
            if (!success || result.Value != 25.50m)
            {
                throw new InvalidOperationException("Failed to parse Percentage");
            }
        });
    }

    [Fact]
    public void TaxRate_WhenCreatedAndParsedFromSpan_PerformsZeroAllocations()
    {
        const string input = "18.00";
        ValueObjectContractExtensions.AssertZeroAllocations(() =>
        {
            var success = TaxRate.TryParse(input.AsSpan(), CultureInfo.InvariantCulture, out var result);
            if (!success || result.Value != 18.00m)
            {
                throw new InvalidOperationException("Failed to parse TaxRate");
            }
        });
    }

    [Fact]
    public void DiscountRate_WhenCreatedAndParsedFromSpan_PerformsZeroAllocations()
    {
        const string input = "15.00";
        ValueObjectContractExtensions.AssertZeroAllocations(() =>
        {
            var success = DiscountRate.TryParse(input.AsSpan(), CultureInfo.InvariantCulture, out var result);
            if (!success || result.Value != 15.00m)
            {
                throw new InvalidOperationException("Failed to parse DiscountRate");
            }
        });
    }

    [Fact]
    public void Quantity_WhenCreatedAndParsedFromSpan_PerformsZeroAllocations()
    {
        const string input = "1500";
        ValueObjectContractExtensions.AssertZeroAllocations(() =>
        {
            var success = Quantity.TryParse(input.AsSpan(), CultureInfo.InvariantCulture, out var result);
            if (!success || result.Value != 1500)
            {
                throw new InvalidOperationException("Failed to parse Quantity");
            }
        });
    }

    [Fact]
    public void BusinessDate_WhenCreatedAndParsedFromSpan_PerformsZeroAllocations()
    {
        const string input = "2026-08-20";
        ValueObjectContractExtensions.AssertZeroAllocations(() =>
        {
            var success = BusinessDate.TryParse(input.AsSpan(), CultureInfo.InvariantCulture, out var result);
            if (!success || result.Value != new DateOnly(2026, 8, 20))
            {
                throw new InvalidOperationException("Failed to parse BusinessDate");
            }
        });
    }

    [Fact]
    public void Money_WhenPerformingArithmeticAndRounding_PerformsZeroAllocations()
    {
        var m1 = Money.Create(100.25m, CurrencyCode.USD).Value;
        var m2 = Money.Create(50.75m, CurrencyCode.USD).Value;

        ValueObjectContractExtensions.AssertZeroAllocations(() =>
        {
            var added = m1.Add(m2).Value;
            var subtracted = added.Subtract(m2).Value;
            var multiplied = subtracted.Multiply(2m);
            var rounded = multiplied.Round(2);
            var commercial = rounded.RoundCommercial(2);

            if (commercial.Amount != 200.50m)
            {
                throw new InvalidOperationException("Calculation mismatch");
            }
        });
    }

    [Fact]
    public void Range_WhenEvaluatingMembershipAndOverlaps_PerformsZeroAllocations()
    {
        var r1 = Range<int>.Create(10, 100).Value;
        var r2 = Range<int>.Create(50, 150).Value;

        ValueObjectContractExtensions.AssertZeroAllocations(() =>
        {
            var contains = r1.Contains(50);
            var overlaps = r1.Overlaps(r2);

            if (!contains || !overlaps)
            {
                throw new InvalidOperationException("Range evaluation mismatch");
            }
        });
    }

    [Fact]
    public void TimeRange_WhenEvaluatingMembershipAndOverlaps_PerformsZeroAllocations()
    {
        var t1 = TimeRange.Create(new TimeOnly(9, 0), new TimeOnly(17, 0)).Value;
        var t2 = TimeRange.Create(new TimeOnly(13, 0), new TimeOnly(21, 0)).Value;
        var testPoint = new TimeOnly(14, 0);

        ValueObjectContractExtensions.AssertZeroAllocations(() =>
        {
            var contains = t1.Contains(testPoint);
            var overlaps = t1.Overlaps(t2);

            if (!contains || !overlaps)
            {
                throw new InvalidOperationException("TimeRange evaluation mismatch");
            }
        });
    }
}
