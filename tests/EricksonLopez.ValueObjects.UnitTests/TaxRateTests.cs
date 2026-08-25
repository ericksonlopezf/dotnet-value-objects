// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="TaxRate"/> Value Object.
/// </summary>
public sealed class TaxRateTests
{
    [Fact]
    public void ComparisonAndEqualityContracts_WhenValidTaxRates_SatisfiesContracts()
    {
        var t0 = TaxRate.Create(0m).Value;
        var t18 = TaxRate.Create(18m).Value;
        var t18Copy = TaxRate.Create(18m).Value;

        t18.ShouldSatisfyEqualityContract(t18Copy, t0, (a, b) => a == b, (a, b) => a != b);
        t0.ShouldSatisfyComparisonContract(t0, t18,
            (a, b) => a < b,
            (a, b) => a <= b,
            (a, b) => a > b,
            (a, b) => a >= b);

        t18.CompareTo(t18Copy).Should().Be(0);
        t18.CompareTo((object)t18Copy).Should().Be(0);

        Action invalidObj = () => t18.CompareTo("not-a-taxrate");
        invalidObj.Should().Throw<ArgumentException>()
            .WithMessage("Object is not a TaxRate*")
            .WithParameterName("obj");

        t18.Fraction.Should().Be(0.18m);
        t18.AsFraction.Should().Be(0.18m);
        t18.CalculateTax(100m).Should().Be(18m);
        var taxMoney = t18.CalculateTax(Money.Create(100m, CurrencyCode.DOP).Value);
        taxMoney.Amount.Should().Be(18m);
        t18.ToString().Should().Be("18%");
        TaxRate.Exempt.IsExempt.Should().BeTrue();
    }

    [Fact]
    public void ComparisonAndDefaults_WhenComparedAgainstInvalidType_ThrowsArgumentException()
    {
        var t1 = TaxRate.Create(10m).Value;
        var t2 = TaxRate.Create(20m).Value;

        t1.CompareTo(t2).Should().BeNegative();
        ((IComparable)t1).CompareTo((object)t1).Should().Be(0);
        ((IComparable)t1).CompareTo((object)t2).Should().BeNegative();
        t1.IsExempt.Should().BeFalse();
        TaxRate.Exempt.IsExempt.Should().BeTrue();
        t1.AsFraction.Should().Be(0.1m);
        default(TaxRate).ToString().Should().Be("0%");

        Action tInvalid = () => t1.CompareTo("not-a-taxrate");
        tInvalid.Should().Throw<ArgumentException>()
            .WithMessage("Object is not a TaxRate*")
            .WithParameterName("obj");

        Action tNull = () => ((IComparable)t1).CompareTo(null);
        tNull.Should().Throw<ArgumentException>()
            .WithMessage("Object is not a TaxRate*")
            .WithParameterName("obj");

        var failRate = TaxRate.Create(-5m);
        failRate.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Formatting_WithFormatsAndProviders_BehavesCorrectly()
    {
        var t = TaxRate.Create(18.5m).Value;
        var tInt = TaxRate.Create(18m).Value;

        t.ToString().Should().Be("18.5%");
        tInt.ToString().Should().Be("18%");
        t.ToString(null, null).Should().Be("18.5%");
        t.ToString("0.00", CultureInfo.InvariantCulture).Should().Be("18.50%");

        var frCulture = CultureInfo.GetCultureInfo("fr-FR");
        t.ToString("0.0", frCulture).Should().Be("18,5%");

        Span<char> exactBuffer = stackalloc char[5];
        t.TryFormat(exactBuffer, out int charsWritten, default, null).Should().BeTrue();
        charsWritten.Should().Be(5);
        exactBuffer[..charsWritten].ToString().Should().Be("18.5%");

        Span<char> buffer = stackalloc char[10];
        t.TryFormat(buffer, out int customWritten, "0.00".AsSpan(), CultureInfo.InvariantCulture).Should().BeTrue();
        customWritten.Should().Be(6);
        buffer[..customWritten].ToString().Should().Be("18.50%");

        Span<char> smallBuffer = stackalloc char[4];
        t.TryFormat(smallBuffer, out int smallWritten, default, null).Should().BeFalse();
        smallWritten.Should().Be(0);
    }

    [Fact]
    public void Parsing_StringAndSpan_ParsesOrThrows()
    {
        var t = TaxRate.Parse("18.5%", CultureInfo.InvariantCulture);
        t.Value.Should().Be(18.5m);

        TaxRate.Parse("  25 %  ", CultureInfo.InvariantCulture).Value.Should().Be(25m);

        var frCulture = CultureInfo.GetCultureInfo("fr-FR");
        TaxRate.Parse("18,5%", frCulture).Value.Should().Be(18.5m);
        TaxRate.Parse("18,5%".AsSpan(), frCulture).Value.Should().Be(18.5m);

        TaxRate.Parse("18".AsSpan(), CultureInfo.InvariantCulture).Value.Should().Be(18m);

        Action nullAct = () => TaxRate.Parse((string)null!, CultureInfo.InvariantCulture);
        nullAct.Should().Throw<ArgumentNullException>();

        Action invalidRange = () => TaxRate.Parse("150%", CultureInfo.InvariantCulture);
        invalidRange.Should().Throw<FormatException>()
            .WithMessage("TaxRate must be between 0 and 100.");

        Action invalidSpanRange = () => TaxRate.Parse("-10%".AsSpan(), CultureInfo.InvariantCulture);
        invalidSpanRange.Should().Throw<FormatException>()
            .WithMessage("TaxRate must be between 0 and 100.");

        Action invalidFormat = () => TaxRate.Parse("abc%", CultureInfo.InvariantCulture);
        invalidFormat.Should().Throw<FormatException>()
            .WithMessage("Cannot parse 'abc%' as TaxRate.");

        Action invalidSpanFormat = () => TaxRate.Parse("invalid".AsSpan(), CultureInfo.InvariantCulture);
        invalidSpanFormat.Should().Throw<FormatException>()
            .WithMessage("Cannot parse 'invalid' as TaxRate.");
    }

    [Fact]
    public void TryParse_StringAndSpan_ReturnsSuccessOrFailure()
    {
        TaxRate.TryParse("18.5%", CultureInfo.InvariantCulture, out var r1).Should().BeTrue();
        r1.Value.Should().Be(18.5m);

        var frCulture = CultureInfo.GetCultureInfo("fr-FR");
        TaxRate.TryParse("18,5%", frCulture, out var rFr).Should().BeTrue();
        rFr.Value.Should().Be(18.5m);

        TaxRate.TryParse("18,5%".AsSpan(), frCulture, out var rSpanFr).Should().BeTrue();
        rSpanFr.Value.Should().Be(18.5m);

        TaxRate.TryParse(null, null, out var rNull).Should().BeFalse();
        rNull.Should().Be(default);

        TaxRate.TryParse("   ", null, out var rWhite).Should().BeFalse();
        TaxRate.TryParse("150%", null, out var rOut).Should().BeFalse();
        TaxRate.TryParse("-10%".AsSpan(), CultureInfo.InvariantCulture, out _).Should().BeFalse();
        TaxRate.TryParse("abc%", null, out var rInvalid).Should().BeFalse();
        TaxRate.TryParse("invalid".AsSpan(), null, out _).Should().BeFalse();

        TaxRate.TryParse("18%".AsSpan(), CultureInfo.InvariantCulture, out var rSpan).Should().BeTrue();
        rSpan.Value.Should().Be(18m);

        TaxRate.TryParse("invalid".AsSpan(), null, out var rSpanInvalid).Should().BeFalse();
    }
}



