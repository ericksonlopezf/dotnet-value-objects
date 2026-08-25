// Copyright © Erickson Lopez. MIT License.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="Money"/> Value Object.
/// </summary>
public sealed class MoneyTests
{
    [Fact]
    public void Create_WhenAmountAndCurrencyAreValid_ReturnsSuccess()
    {
        var result = Money.Create(100.50m, CurrencyCode.USD);
        result.IsSuccess.Should().BeTrue();
        result.Value.Amount.Should().Be(100.50m);
        result.Value.Currency.Should().Be(CurrencyCode.USD);
    }

    [Fact]
    public void Create_WhenStringCurrencyIsValid_Succeeds()
    {
        var result = Money.Create(250.75m, "dop");
        result.IsSuccess.Should().BeTrue();
        result.Value.Amount.Should().Be(250.75m);
        result.Value.Currency.Should().Be(CurrencyCode.DOP);
    }

    [Fact]
    public void ZeroUsd_WhenAccessed_ReturnsZeroAmountInUsd()
    {
        Money.ZeroUsd.Amount.Should().Be(0m);
        Money.ZeroUsd.Currency.Should().Be(CurrencyCode.USD);
        Money.ZeroUsd.IsZero.Should().BeTrue();
    }

    [Fact]
    public void Create_WhenStringCurrencyIsInvalid_ReturnsCurrencyFormatError()
    {
        var result = Money.Create(250.75m, "INVALID");
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("CurrencyCode.InvalidFormat");
    }

    [Fact]
    public void CreateNonNegative_WhenAmountIsNegative_ReturnsNegativeAmountError()
    {
        var invalidResult = Money.CreateNonNegative(-1m, CurrencyCode.USD);
        invalidResult.IsFailure.Should().BeTrue();
        invalidResult.Error.Code.Should().Be("Money.NegativeAmount");

        var validNonNeg = Money.CreateNonNegative(50.25m, CurrencyCode.USD);
        validNonNeg.IsSuccess.Should().BeTrue();
        validNonNeg.Value.Amount.Should().Be(50.25m);
        validNonNeg.Value.ToString().Should().Be("50.25 USD");
    }

    [Fact]
    public void Create_WhenDecimalsExceedScale_ReturnsTooManyDecimalsError()
    {
        var result = Money.Create(1.1234567m, CurrencyCode.USD);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Money.TooManyDecimals");
    }

    [Fact]
    public void Create_WhenAmountExceedsMaximum_ReturnsOutOfRangeError()
    {
        var result = Money.Create(1_000_000_000_000_000m, CurrencyCode.USD);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Money.AmountOutOfRange");
    }

    [Fact]
    public void Round_WhenBankersRoundingApplied_RoundsToNearestEven()
    {
        var m1 = Money.Create(10.125m, CurrencyCode.USD).Value; // 10.12
        var m2 = Money.Create(10.135m, CurrencyCode.USD).Value; // 10.14

        m1.Amount.Should().Be(10.12m);
        m2.Amount.Should().Be(10.14m);

        var m3 = Money.Create(10.125m, CurrencyCode.USD, 3).Value;
        m3.RoundCommercial(2).Amount.Should().Be(10.13m);
        m3.RoundCommercial().Amount.Should().Be(10.13m);
        m3.Round(2).Amount.Should().Be(10.12m);
        m3.Round().Amount.Should().Be(10.12m);
    }

    [Fact]
    public void Create_WhenCurrencyIsJPY_UsesZeroDecimalsByDefault()
    {
        var jpy = CurrencyCode.JPY;
        var money = Money.Create(100.50m, jpy, jpy.DecimalPlaces).Value;
        money.Amount.Should().Be(100m);
    }

    [Fact]
    public void Create_WhenCurrencyIsBHD_UsesThreeDecimalsByDefault()
    {
        var bhd = CurrencyCode.BHD;
        var money = Money.Create(10.5005m, bhd, bhd.DecimalPlaces).Value;
        money.Amount.Should().Be(10.500m);
    }

    [Fact]
    public void Zero_WhenCurrencyProvided_ReturnsZeroInstance()
    {
        var zero = Money.Zero(CurrencyCode.USD);
        zero.Amount.Should().Be(0m);
        zero.IsZero.Should().BeTrue();
        zero.IsPositive.Should().BeFalse();
        zero.IsNegative.Should().BeFalse();
    }

    [Fact]
    public void Add_WhenCurrenciesMatch_ReturnsSummedAmount()
    {
        var a = Money.Create(100m, CurrencyCode.USD).Value;
        var b = Money.Create(50m, CurrencyCode.USD).Value;
        a.Add(b).Value.Amount.Should().Be(150m);
    }

    [Fact]
    public void Add_WhenCurrenciesMismatch_ReturnsCurrencyMismatchError()
    {
        var a = Money.Create(100m, CurrencyCode.USD).Value;
        var b = Money.Create(50m, CurrencyCode.DOP).Value;
        var result = a.Add(b);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Money.CurrencyMismatch");
    }

    [Fact]
    public void Subtract_WhenCurrenciesMatch_ReturnsSubtractedAmount()
    {
        var a = Money.Create(100m, CurrencyCode.USD).Value;
        var b = Money.Create(30m, CurrencyCode.USD).Value;
        a.Subtract(b).Value.Amount.Should().Be(70m);
    }

    [Fact]
    public void Subtract_WhenCurrenciesMismatch_ReturnsCurrencyMismatchError()
    {
        var a = Money.Create(100m, CurrencyCode.USD).Value;
        var b = Money.Create(30m, CurrencyCode.DOP).Value;
        var result = a.Subtract(b);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Money.CurrencyMismatch");
    }

    [Fact]
    public void ArithmeticOperators_WhenOperatingOnSameCurrency_CalculatesCorrectly()
    {
        var a = Money.Create(100m, CurrencyCode.USD).Value;
        var b = Money.Create(30m, CurrencyCode.USD).Value;

        (a + b).Amount.Should().Be(130m);
        (a - b).Amount.Should().Be(70m);
        (a * 2m).Amount.Should().Be(200m);
        (2m * a).Amount.Should().Be(200m);
        (-a).Amount.Should().Be(-100m);
        (-a).IsNegative.Should().BeTrue();
        a.Abs().Amount.Should().Be(100m);
    }

    [Fact]
    public void ArithmeticOperators_WhenOperatingOnDifferentCurrencies_ThrowsDomainException()
    {
        var a = Money.Create(100m, CurrencyCode.USD).Value;
        var b = Money.Create(50m, CurrencyCode.DOP).Value;

        Action add = () => { var _ = a + b; };
        Action sub = () => { var _ = a - b; };

        add.Should().Throw<DomainException>();
        sub.Should().Throw<DomainException>();
    }

    [Fact]
    public void Distribute_WhenEvenPartsProvided_DistributesWithRemainder()
    {
        var total = Money.Create(100m, CurrencyCode.USD).Value;
        var parts = total.Distribute(3);

        parts.Should().HaveCount(3);
        parts[0].Amount.Should().Be(33.34m);
        parts[1].Amount.Should().Be(33.33m);
        parts[2].Amount.Should().Be(33.33m);
        parts.Sum(p => p.Amount).Should().Be(100m);
    }

    [Fact]
    public void Distribute_WhenPartsIsZeroOrNegative_ThrowsDomainException()
    {
        var total = Money.Create(100m, CurrencyCode.USD).Value;
        Action act = () => total.Distribute(0);
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Allocate_WhenRatiosProvided_DistributesProportionally()
    {
        var total = Money.Create(100m, CurrencyCode.USD).Value;
        var allocated = total.Allocate([1, 2, 1]);

        allocated.Should().HaveCount(3);
        allocated[0].Amount.Should().Be(25.00m);
        allocated[1].Amount.Should().Be(50.00m);
        allocated[2].Amount.Should().Be(25.00m);
        allocated.Sum(p => p.Amount).Should().Be(100m);
    }

    [Fact]
    public void Round_WhenDecimalScaleCustomized_RoundsAccurately()
    {
        var money = Money.Create(100.456m, CurrencyCode.DOP, 6).Value;
        money.Round().Amount.Should().Be(100.46m);
        money.RoundCommercial().Amount.Should().Be(100.46m);
    }

    [Fact]
    public void Create_WhenMaximumBoundaryTested_EnforcesBoundsCorrectly()
    {
        var maxValid = Money.Create(999_999_999_999_999.999999m, CurrencyCode.USD, 6);
        maxValid.IsSuccess.Should().BeTrue();

        var exceedMax = Money.Create(1_000_000_000_000_000.0m, CurrencyCode.USD);
        exceedMax.IsFailure.Should().BeTrue();
        exceedMax.Error.Code.Should().Be("Money.AmountOutOfRange");

        var zeroNonNeg = Money.CreateNonNegative(0m, CurrencyCode.USD);
        zeroNonNeg.IsSuccess.Should().BeTrue();
        zeroNonNeg.Value.IsZero.Should().BeTrue();
    }

    [Fact]
    public void Allocate_WhenDistributingRemainderPennies_DistributesWithoutLoss()
    {
        var money = Money.Create(100.01m, CurrencyCode.USD).Value;
        var parts = money.Allocate(1, 1, 1);

        parts.Should().HaveCount(3);
        parts[0].Amount.Should().Be(33.34m);
        parts[1].Amount.Should().Be(33.34m);
        parts[2].Amount.Should().Be(33.33m);
        parts.Sum(p => p.Amount).Should().Be(100.01m);

        var empty = money.Allocate([]);
        empty.Should().BeEmpty();

        Action invalidRatio = () => money.Allocate(0, 1);
        invalidRatio.Should().Throw<ArgumentException>()
            .WithMessage("*Ratios must be strictly positive.*");
    }

    [Fact]
    public void EqualityAndComparisonContracts_WhenValidated_SatisfiesAllInvariants()
    {
        var smaller = Money.Create(50m, CurrencyCode.USD).Value;
        var smallerCopy = Money.Create(50m, CurrencyCode.USD).Value;
        var greater = Money.Create(100m, CurrencyCode.USD).Value;
        var differentCurrency = Money.Create(50m, CurrencyCode.DOP).Value;

        // Formal Value Object contract verification
        smaller.ShouldSatisfyEqualityContract(smallerCopy, greater, (a, b) => a == b, (a, b) => a != b);
        smaller.ShouldSatisfyEqualityContract(smallerCopy, differentCurrency, (a, b) => a == b, (a, b) => a != b);
        smaller.ShouldSatisfyComparisonContract(smallerCopy, greater,
            (a, b) => a < b,
            (a, b) => a <= b,
            (a, b) => a > b,
            (a, b) => a >= b);
    }

    [Fact]
    public void ComparisonMethods_WhenComparingDifferentCurrencies_ThrowsDomainException()
    {
        var usd = Money.Create(100m, CurrencyCode.USD).Value;
        var dop = Money.Create(100m, CurrencyCode.DOP).Value;

        Action gtCur = () => usd.IsGreaterThan(dop);
        Action gteCur = () => usd.IsGreaterThanOrEqual(dop);
        Action ltCur = () => usd.IsLessThan(dop);
        Action lteCur = () => usd.IsLessThanOrEqual(dop);
        Action cmpCur = () => usd.CompareTo(dop);
        Action cmpObj = () => usd.CompareTo("not-a-money");
        Action cmpNull = () => ((IComparable)usd).CompareTo(null);

        ((IComparable)usd).CompareTo((object)usd).Should().Be(0);
        ((IComparable)usd).CompareTo((object)Money.Create(50m, CurrencyCode.USD).Value).Should().BePositive();

        gtCur.Should().Throw<DomainException>()
            .WithMessage("Cannot operate on Money with different currencies: 'USD' vs 'DOP'.");
        gteCur.Should().Throw<DomainException>()
            .WithMessage("Cannot operate on Money with different currencies: 'USD' vs 'DOP'.");
        ltCur.Should().Throw<DomainException>()
            .WithMessage("Cannot operate on Money with different currencies: 'USD' vs 'DOP'.");
        lteCur.Should().Throw<DomainException>()
            .WithMessage("Cannot operate on Money with different currencies: 'USD' vs 'DOP'.");
        cmpCur.Should().Throw<DomainException>()
            .WithMessage("Cannot operate on Money with different currencies: 'USD' vs 'DOP'.");
        cmpObj.Should().Throw<ArgumentException>()
            .WithMessage("*Object is not a Money instance.*")
            .WithParameterName("obj");
        cmpNull.Should().Throw<ArgumentException>()
            .WithMessage("*Object is not a Money instance.*")
            .WithParameterName("obj");
    }

    [Fact]
    public void ComparisonMethods_WhenSameCurrency_EvaluatesStrictAndInclusiveCorrectly()
    {
        var m50 = Money.Create(50m, CurrencyCode.USD).Value;
        var m50Copy = Money.Create(50m, CurrencyCode.USD).Value;
        var m100 = Money.Create(100m, CurrencyCode.USD).Value;

        m50.IsPositive.Should().BeTrue();
        m50.IsNegative.Should().BeFalse();
        m50.IsZero.Should().BeFalse();

        var zero = Money.Zero(CurrencyCode.USD);
        zero.IsPositive.Should().BeFalse();
        zero.IsNegative.Should().BeFalse();
        zero.IsZero.Should().BeTrue();

        var neg = Money.Create(-10m, CurrencyCode.USD).Value;
        neg.IsPositive.Should().BeFalse();
        neg.IsNegative.Should().BeTrue();
        neg.IsZero.Should().BeFalse();

        m100.IsGreaterThan(m50).Should().BeTrue();
        m50.IsGreaterThan(m100).Should().BeFalse();
        m50.IsGreaterThan(m50Copy).Should().BeFalse();

        m100.IsGreaterThanOrEqual(m50).Should().BeTrue();
        m50.IsGreaterThanOrEqual(m50Copy).Should().BeTrue();
        m50.IsGreaterThanOrEqual(m100).Should().BeFalse();

        m50.IsLessThan(m100).Should().BeTrue();
        m100.IsLessThan(m50).Should().BeFalse();
        m50.IsLessThan(m50Copy).Should().BeFalse();

        m50.IsLessThanOrEqual(m100).Should().BeTrue();
        m50.IsLessThanOrEqual(m50Copy).Should().BeTrue();
        m100.IsLessThanOrEqual(m50).Should().BeFalse();
    }

    [Fact]
    public void Formatting_WithFormatsAndProviders_BehavesCorrectly()
    {
        var m = Money.Create(1234.56m, CurrencyCode.USD).Value;

        m.ToString().Should().Be("1,234.56 USD");
        m.ToString(null, null).Should().Be("1,234.56 USD");
        m.ToString("N3", CultureInfo.InvariantCulture).Should().Be("1,234.560 USD");

        var frCulture = CultureInfo.GetCultureInfo("fr-FR");
        m.ToString("N2", frCulture).Should().Be("1\u202f234,56 USD");

        Span<char> exactBuffer = stackalloc char[12];
        m.TryFormat(exactBuffer, out int charsWritten, default, null).Should().BeTrue();
        charsWritten.Should().Be(12);
        exactBuffer[..charsWritten].ToString().Should().Be("1,234.56 USD");

        Span<char> buffer = stackalloc char[20];
        m.TryFormat(buffer, out int customWritten, "N3".AsSpan(), CultureInfo.InvariantCulture).Should().BeTrue();
        customWritten.Should().Be(13);
        buffer[..customWritten].ToString().Should().Be("1,234.560 USD");

        Span<char> smallBuffer = stackalloc char[11];
        m.TryFormat(smallBuffer, out int smallWritten, default, null).Should().BeFalse();
        smallWritten.Should().Be(0);
    }

    [Fact]
    public void Rounding_WithAndWithoutExplicitDecimals_UsesCurrencyOrExplicitDecimals()
    {
        var mUsd = Money.Create(100.555m, CurrencyCode.USD, 3).Value;
        mUsd.Round().Amount.Should().Be(100.56m);
        mUsd.Round(1).Amount.Should().Be(100.6m);
        mUsd.RoundCommercial().Amount.Should().Be(100.56m);
        mUsd.RoundCommercial(1).Amount.Should().Be(100.6m);

        var mJpy = Money.Create(100.555m, CurrencyCode.JPY, 3).Value;
        mJpy.Round().Amount.Should().Be(101m);
        mJpy.RoundCommercial().Amount.Should().Be(101m);
    }

    [Fact]
    public void ApplyPercentage_WhenApplied_CalculatesRoundedAmount()
    {
        var m = Money.Create(100m, CurrencyCode.USD).Value;
        var p18 = Percentage.Create(18m).Value;

        var tax = m.ApplyPercentage(p18);
        tax.Amount.Should().Be(18m);
        tax.Currency.Should().Be(CurrencyCode.USD);
    }

    [Fact]
    public void Distribute_WhenNegativeParts_ThrowsExactMessage()
    {
        var total = Money.Create(100m, CurrencyCode.USD).Value;
        Action act = () => total.Distribute(0);
        act.Should().Throw<DomainException>()
            .WithMessage("Cannot distribute Money into 0 parts.");
    }
}




