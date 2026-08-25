// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic.UnitTests;

public sealed class NcfExpirationDateTests
{
    [Fact]
    public void FromAuthorizationYear_DefaultState_SetsDecember31stOfFollowingYear()
    {
        var expiration = NcfExpirationDate.FromAuthorizationYear(2025);

        expiration.Value.Should().Be(new DateOnly(2026, 12, 31));
    }

    [Fact]
    public void IsExpired_DefaultState_EvaluatesAccurately()
    {
        var expiration = NcfExpirationDate.FromAuthorizationYear(2025); // Exp: 2026-12-31

        expiration.IsExpired(new DateOnly(2026, 12, 30)).Should().BeFalse();
        expiration.IsExpired(new DateOnly(2026, 12, 31)).Should().BeFalse();
        expiration.IsExpired(new DateOnly(2027, 1, 1)).Should().BeTrue();
    }


    [Fact]
    public void Create_ValidDate_Succeeds()
    {
        var date = new DateOnly(2026, 12, 31);
        var result = NcfExpirationDate.Create(date);
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(date);
        result.Value.ToString().Should().Be("2026-12-31");
    }

    [Fact]
    public void Create_MinOrMaxDate_ReturnsOutOfRangeError()
    {
        NcfExpirationDate.Create(DateOnly.MinValue).Error.Code.Should().Be("NcfExpirationDate.OutOfRange");
        NcfExpirationDate.Create(DateOnly.MaxValue).Error.Code.Should().Be("NcfExpirationDate.OutOfRange");
    }

    [Fact]
    public void NcfExpirationDate_ComparisonsAndOperators_Exhaustive()
    {
        var a = NcfExpirationDate.Create(new DateOnly(2025, 12, 31)).Value;
        var aCopy = NcfExpirationDate.Create(new DateOnly(2025, 12, 31)).Value;
        var b = NcfExpirationDate.Create(new DateOnly(2026, 12, 31)).Value;

        a.ShouldSatisfyEqualityContract(aCopy, b, (x, y) => x == y, (x, y) => x != y);
        a.ShouldSatisfyComparisonContract(aCopy, b,
            (x, y) => x < y,
            (x, y) => x <= y,
            (x, y) => x > y,
            (x, y) => x >= y);

        a.CompareTo((object)aCopy).Should().Be(0);

        Action invalidObj = () => a.CompareTo("not-an-expirationdate");
        invalidObj.Should().Throw<ArgumentException>()
            .WithMessage("*Object is not an NcfExpirationDate*");
    }
}





