// Copyright © Erickson Lopez. MIT License.
using System;
using System.Linq;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

public sealed class ValueObjectTests
{
    [Fact]
    public void Email_WhenValid_ShouldNormalizeAndSucceed()
    {
        var result = Email.Create("  Test.User@Example.COM  ");
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("test.user@example.com");
    }

    [Fact]
    public void Email_WhenInvalid_ShouldReturnValidationFailure()
    {
        var result = Email.Create("invalid-email-address");
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Email.InvalidFormat");
    }

    [Fact]
    public void Email_Properties_ShouldReturnLocalPartAndDomain()
    {
        var email = Email.Create("john.doe@domain.com").Value;
        email.LocalPart.Should().Be("john.doe");
        email.Domain.Should().Be("domain.com");
    }

    [Fact]
    public void Money_Add_SameCurrency_ShouldSucceed()
    {
        var m1 = Money.Create(10.50m, CurrencyCode.USD).Value;
        var m2 = Money.Create(5.25m, CurrencyCode.USD).Value;

        var result = m1.Add(m2);

        result.IsSuccess.Should().BeTrue();
        result.Value.Amount.Should().Be(15.75m);
        result.Value.Currency.Should().Be(CurrencyCode.USD);
    }

    [Fact]
    public void Money_Add_DifferentCurrency_ShouldReturnFailure()
    {
        var usd = Money.Create(10m, CurrencyCode.USD).Value;
        var eur = Money.Create(10m, CurrencyCode.EUR).Value;

        var result = usd.Add(eur);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Money.CurrencyMismatch");
    }

    [Fact]
    public void Money_Allocate_ShouldDistributeExactPenniesWithoutLoss()
    {
        var total = Money.Create(100.00m, CurrencyCode.USD).Value;
        var shares = total.Allocate(1, 1, 1);

        shares.Should().HaveCount(3);
        shares[0].Amount.Should().Be(33.34m);
        shares[1].Amount.Should().Be(33.33m);
        shares[2].Amount.Should().Be(33.33m);
        shares.Sum(s => s.Amount).Should().Be(100.00m);
    }

    [Fact]
    public void Range_WhenStartGreaterThanEnd_ShouldFailValidation()
    {
        var result = Range<int>.Create(10, 5);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Range.InvalidBounds");
    }

    [Fact]
    public void Range_Contains_And_Overlaps_ShouldEvaluateCorrectly()
    {
        var r1 = Range<int>.Create(10, 20).Value;
        var r2 = Range<int>.Create(12, 18).Value;
        var r3 = Range<int>.Create(15, 25).Value;
        var r4 = Range<int>.Create(30, 40).Value;

        r1.Contains(15).Should().BeTrue();
        r1.Contains(5).Should().BeFalse();
        r1.Contains(r2).Should().BeTrue();
        r1.Overlaps(r3).Should().BeTrue();
        r1.Overlaps(r4).Should().BeFalse();
    }

    [Fact]
    public void Percentage_WhenOutsideBounds_ShouldFail()
    {
        var resLow = Percentage.Create(-1m);
        var resHigh = Percentage.Create(100.1m);
        var resValid = Percentage.Create(75.5m);

        resLow.IsFailure.Should().BeTrue();
        resLow.Error.Code.Should().Be("Percentage.OutOfRange");
        resHigh.IsFailure.Should().BeTrue();
        resHigh.Error.Code.Should().Be("Percentage.OutOfRange");
        resValid.IsSuccess.Should().BeTrue();
        resValid.Value.Fraction.Should().Be(0.755m);
    }
}




