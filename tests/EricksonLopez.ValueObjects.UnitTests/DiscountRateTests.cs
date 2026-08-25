// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="DiscountRate"/> Value Object.
/// </summary>
public sealed class DiscountRateTests
{
    [Fact]
    public void ComparisonAndEqualityContracts_WhenValidDiscountRates_SatisfiesContracts()
    {
        var d0 = DiscountRate.Create(0m).Value;
        var d10 = DiscountRate.Create(10m).Value;
        var d10Copy = DiscountRate.Create(10m).Value;

        d10.ShouldSatisfyEqualityContract(d10Copy, d0, (a, b) => a == b, (a, b) => a != b);
        d0.ShouldSatisfyComparisonContract(d0, d10,
            (a, b) => a < b,
            (a, b) => a <= b,
            (a, b) => a > b,
            (a, b) => a >= b);

        d10.CompareTo(d10Copy).Should().Be(0);
        d10.CompareTo((object)d10Copy).Should().Be(0);

        Action invalidObj = () => d10.CompareTo("not-a-discountrate");
        invalidObj.Should().Throw<ArgumentException>()
            .WithMessage("Object is not a DiscountRate*")
            .WithParameterName("obj");

        // Calculation methods
        d10.Fraction.Should().Be(0.10m);
        d10.AsFraction.Should().Be(0.10m);
        d10.CalculateDiscount(100m).Should().Be(10m);
        d10.ApplyTo(100m).Should().Be(90m);
        var discountedMoney = d10.ApplyTo(Money.Create(100m, CurrencyCode.USD).Value);
        discountedMoney.Amount.Should().Be(90m);
        discountedMoney.Currency.Should().Be(CurrencyCode.USD);
        d10.ToString().Should().Be("10%");
        DiscountRate.None.IsZero.Should().BeTrue();
    }

    [Fact]
    public void ComparisonAndDefaults_WhenComparedAgainstInvalidType_ThrowsArgumentException()
    {
        var d1 = DiscountRate.Create(10m).Value;
        var d2 = DiscountRate.Create(20m).Value;

        d1.CompareTo(d2).Should().BeNegative();
        ((IComparable)d1).CompareTo((object)d1).Should().Be(0);
        ((IComparable)d1).CompareTo((object)d2).Should().BeNegative();
        d1.IsZero.Should().BeFalse();
        DiscountRate.None.IsZero.Should().BeTrue();
        d1.AsFraction.Should().Be(0.1m);
        default(DiscountRate).ToString().Should().Be("0%");

        Action dNull = () => ((IComparable)d1).CompareTo(null);
        dNull.Should().Throw<ArgumentException>()
            .WithMessage("Object is not a DiscountRate*")
            .WithParameterName("obj");

        var failRate = DiscountRate.Create(-5m);
        failRate.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Formatting_WithFormatsAndProviders_BehavesCorrectly()
    {
        var d = DiscountRate.Create(15.5m).Value;
        var dInt = DiscountRate.Create(15m).Value;

        d.ToString().Should().Be("15.5%");
        dInt.ToString().Should().Be("15%");
        d.ToString(null, null).Should().Be("15.5%");
        d.ToString("0.00", CultureInfo.InvariantCulture).Should().Be("15.50%");

        var frCulture = CultureInfo.GetCultureInfo("fr-FR");
        d.ToString("0.0", frCulture).Should().Be("15,5%");

        Span<char> exactBuffer = stackalloc char[5];
        d.TryFormat(exactBuffer, out int charsWritten, default, null).Should().BeTrue();
        charsWritten.Should().Be(5);
        exactBuffer[..charsWritten].ToString().Should().Be("15.5%");

        Span<char> buffer = stackalloc char[10];
        d.TryFormat(buffer, out int customWritten, "0.00".AsSpan(), CultureInfo.InvariantCulture).Should().BeTrue();
        customWritten.Should().Be(6);
        buffer[..customWritten].ToString().Should().Be("15.50%");

        Span<char> smallBuffer = stackalloc char[4];
        d.TryFormat(smallBuffer, out int smallWritten, default, null).Should().BeFalse();
        smallWritten.Should().Be(0);
    }

    [Fact]
    public void Parsing_StringAndSpan_ParsesOrThrows()
    {
        var d = DiscountRate.Parse("15.5%", CultureInfo.InvariantCulture);
        d.Value.Should().Be(15.5m);

        DiscountRate.Parse("  20 %  ", CultureInfo.InvariantCulture).Value.Should().Be(20m);

        var frCulture = CultureInfo.GetCultureInfo("fr-FR");
        DiscountRate.Parse("15,5%", frCulture).Value.Should().Be(15.5m);
        DiscountRate.Parse("15,5%".AsSpan(), frCulture).Value.Should().Be(15.5m);

        DiscountRate.Parse("15".AsSpan(), CultureInfo.InvariantCulture).Value.Should().Be(15m);

        Action nullAct = () => DiscountRate.Parse((string)null!, CultureInfo.InvariantCulture);
        nullAct.Should().Throw<ArgumentNullException>();

        Action invalidRange = () => DiscountRate.Parse("150%", CultureInfo.InvariantCulture);
        invalidRange.Should().Throw<FormatException>()
            .WithMessage("DiscountRate must be between 0 and 100.");

        Action invalidSpanRange = () => DiscountRate.Parse("-10%".AsSpan(), CultureInfo.InvariantCulture);
        invalidSpanRange.Should().Throw<FormatException>()
            .WithMessage("DiscountRate must be between 0 and 100.");

        Action invalidFormat = () => DiscountRate.Parse("abc%", CultureInfo.InvariantCulture);
        invalidFormat.Should().Throw<FormatException>()
            .WithMessage("Cannot parse 'abc%' as DiscountRate.");

        Action invalidSpanFormat = () => DiscountRate.Parse("invalid".AsSpan(), CultureInfo.InvariantCulture);
        invalidSpanFormat.Should().Throw<FormatException>()
            .WithMessage("Cannot parse 'invalid' as DiscountRate.");
    }

    [Fact]
    public void TryParse_StringAndSpan_ReturnsSuccessOrFailure()
    {
        DiscountRate.TryParse("15.5%", CultureInfo.InvariantCulture, out var r1).Should().BeTrue();
        r1.Value.Should().Be(15.5m);

        var frCulture = CultureInfo.GetCultureInfo("fr-FR");
        DiscountRate.TryParse("15,5%", frCulture, out var rFr).Should().BeTrue();
        rFr.Value.Should().Be(15.5m);

        DiscountRate.TryParse("15,5%".AsSpan(), frCulture, out var rSpanFr).Should().BeTrue();
        rSpanFr.Value.Should().Be(15.5m);

        DiscountRate.TryParse(null, null, out var rNull).Should().BeFalse();
        rNull.Should().Be(default);

        DiscountRate.TryParse("   ", null, out var rWhite).Should().BeFalse();
        DiscountRate.TryParse("150%", null, out var rOut).Should().BeFalse();
        DiscountRate.TryParse("-10%".AsSpan(), CultureInfo.InvariantCulture, out _).Should().BeFalse();
        DiscountRate.TryParse("abc%", null, out var rInvalid).Should().BeFalse();
        DiscountRate.TryParse("invalid".AsSpan(), null, out _).Should().BeFalse();

        DiscountRate.TryParse("15%".AsSpan(), CultureInfo.InvariantCulture, out var rSpan).Should().BeTrue();
        rSpan.Value.Should().Be(15m);

        DiscountRate.TryParse("invalid".AsSpan(), null, out var rSpanInvalid).Should().BeFalse();
    }
}



