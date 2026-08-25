// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="ExchangeRate"/> Value Object.
/// </summary>
public sealed class ExchangeRateTests
{
    [Fact]
    public void ConvertAndInverse_WhenValidExchangeRate_ConvertsAndInvertsAccurately()
    {
        var rate = ExchangeRate.Create(CurrencyCode.USD, CurrencyCode.DOP, 60m).Value;
        var usd = Money.Create(100m, CurrencyCode.USD).Value;

        var dop = rate.Convert(usd).Value;
        dop.Amount.Should().Be(6000m);
        dop.Currency.Should().Be(CurrencyCode.DOP);

        var mismatch = rate.Convert(Money.Create(100m, CurrencyCode.EUR).Value);
        mismatch.IsFailure.Should().BeTrue();
        mismatch.Error.Code.Should().Be("ExchangeRate.CurrencyMismatch");

        var inverse = rate.Inverse().Value;
        inverse.FromCurrency.Should().Be(CurrencyCode.DOP);
        inverse.ToCurrency.Should().Be(CurrencyCode.USD);
        inverse.Rate.Should().Be(Math.Round(1m / 60m, 12));
    }

    [Fact]
    public void Create_WhenFromAndToCurrenciesAreEqual_ReturnsSameCurrencyError()
    {
        var result = ExchangeRate.Create(CurrencyCode.USD, CurrencyCode.USD, 1m);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("ExchangeRate.SameCurrency");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_WhenRateIsZeroOrNegative_ReturnsNonPositiveError(decimal invalidRate)
    {
        var result = ExchangeRate.Create(CurrencyCode.USD, CurrencyCode.DOP, invalidRate);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("ExchangeRate.NonPositive");
    }

    [Fact]
    public void Convert_WhenSourceCurrencyMismatchesFromCurrency_ReturnsCurrencyMismatchError()
    {
        var rate = ExchangeRate.Create(CurrencyCode.USD, CurrencyCode.DOP, 60m).Value;
        var eur = Money.Create(100m, CurrencyCode.EUR).Value;

        var result = rate.Convert(eur);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("ExchangeRate.CurrencyMismatch");
    }

    [Fact]
    public void Create_WhenRateExceedsDecimalScale_ReturnsTooManyDecimalsError()
    {
        var result = ExchangeRate.Create(CurrencyCode.USD, CurrencyCode.DOP, 1.123456789012345m);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("ExchangeRate.TooManyDecimals");

        var rate = ExchangeRate.Create(CurrencyCode.USD, CurrencyCode.DOP, 60.5m).Value;
        rate.ToString().Should().Be("USD/DOP = 60.5");
    }

    [Fact]
    public void EqualityContract_WhenValidRates_SatisfiesEquality()
    {
        var rate1 = ExchangeRate.Create(CurrencyCode.USD, CurrencyCode.DOP, 60m).Value;
        var rate1Copy = ExchangeRate.Create(CurrencyCode.USD, CurrencyCode.DOP, 60m).Value;
        var rate2 = ExchangeRate.Create(CurrencyCode.USD, CurrencyCode.DOP, 61m).Value;

        rate1.ShouldSatisfyEqualityContract(rate1Copy, rate2, (a, b) => a == b, (a, b) => a != b);
    }
}




