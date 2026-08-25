// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="CurrencyCode"/> Value Object.
/// </summary>
public sealed class CurrencyCodeTests
{
    [Fact]
    public void CurrencyCode_WhenValid_ShouldHaveCorrectDecimalPlaces()
    {
        var usd = CurrencyCode.Create("USD").Value;
        var jpy = CurrencyCode.Create("JPY").Value;
        var bhd = CurrencyCode.Create("BHD").Value;

        usd.DecimalPlaces.Should().Be(2);
        jpy.DecimalPlaces.Should().Be(0);
        bhd.DecimalPlaces.Should().Be(3);
        CurrencyCode.DOP.DecimalPlaces.Should().Be(2);
    }

    [Theory]
    [InlineData("USD", 2)]
    [InlineData("EUR", 2)]
    [InlineData("DOP", 2)]
    [InlineData("GBP", 2)]
    [InlineData("JPY", 0)]
    [InlineData("KRW", 0)]
    [InlineData("VND", 0)]
    [InlineData("BHD", 3)]
    [InlineData("KWD", 3)]
    [InlineData("OMR", 3)]
    [InlineData("CLF", 4)]
    [InlineData("UYW", 4)]
    [InlineData("AAA", 2)] // Unknown ISO code defaults to 2
    public void CurrencyCode_DecimalPlaces_Lookup(string code, int expectedDecimals)
    {
        var currency = CurrencyCode.Create(code).Value;
        currency.DecimalPlaces.Should().Be(expectedDecimals);
    }

    [Fact]
    public void CurrencyCode_AllKnownDecimalPlaces_AndWellKnownInstances()
    {
        // 0 decimal places
        CurrencyCode.Create("BIF").Value.DecimalPlaces.Should().Be(0);
        CurrencyCode.Create("CLP").Value.DecimalPlaces.Should().Be(0);
        CurrencyCode.Create("DJF").Value.DecimalPlaces.Should().Be(0);
        CurrencyCode.Create("GNF").Value.DecimalPlaces.Should().Be(0);
        CurrencyCode.Create("ISK").Value.DecimalPlaces.Should().Be(0);
        CurrencyCode.Create("JPY").Value.DecimalPlaces.Should().Be(0);
        CurrencyCode.Create("KMF").Value.DecimalPlaces.Should().Be(0);
        CurrencyCode.Create("KRW").Value.DecimalPlaces.Should().Be(0);
        CurrencyCode.Create("MGA").Value.DecimalPlaces.Should().Be(0);
        CurrencyCode.Create("PYG").Value.DecimalPlaces.Should().Be(0);
        CurrencyCode.Create("RWF").Value.DecimalPlaces.Should().Be(0);
        CurrencyCode.Create("UGX").Value.DecimalPlaces.Should().Be(0);
        CurrencyCode.Create("VND").Value.DecimalPlaces.Should().Be(0);
        CurrencyCode.Create("VUV").Value.DecimalPlaces.Should().Be(0);
        CurrencyCode.Create("XAF").Value.DecimalPlaces.Should().Be(0);
        CurrencyCode.Create("XOF").Value.DecimalPlaces.Should().Be(0);
        CurrencyCode.Create("XPF").Value.DecimalPlaces.Should().Be(0);

        // 3 decimal places
        CurrencyCode.Create("BHD").Value.DecimalPlaces.Should().Be(3);
        CurrencyCode.Create("IQD").Value.DecimalPlaces.Should().Be(3);
        CurrencyCode.Create("JOD").Value.DecimalPlaces.Should().Be(3);
        CurrencyCode.Create("KWD").Value.DecimalPlaces.Should().Be(3);
        CurrencyCode.Create("LYD").Value.DecimalPlaces.Should().Be(3);
        CurrencyCode.Create("OMR").Value.DecimalPlaces.Should().Be(3);
        CurrencyCode.Create("TND").Value.DecimalPlaces.Should().Be(3);

        // 4 decimal places
        CurrencyCode.Create("CLF").Value.DecimalPlaces.Should().Be(4);
        CurrencyCode.Create("UYW").Value.DecimalPlaces.Should().Be(4);

        // Default 2 decimals
        CurrencyCode.Create("USD").Value.DecimalPlaces.Should().Be(2);
        CurrencyCode.Create("EUR").Value.DecimalPlaces.Should().Be(2);

        // Well-known instances
        CurrencyCode.DOP.Value.Should().Be("DOP");
        CurrencyCode.USD.Value.Should().Be("USD");
        CurrencyCode.EUR.Value.Should().Be("EUR");
        CurrencyCode.GBP.Value.Should().Be("GBP");
        CurrencyCode.JPY.Value.Should().Be("JPY");
        CurrencyCode.BHD.Value.Should().Be("BHD");
        CurrencyCode.KWD.Value.Should().Be("KWD");

        default(CurrencyCode).DecimalPlaces.Should().Be(2);
        default(CurrencyCode).ToString().Should().Be(string.Empty);
    }

    [Fact]
    public void CurrencyCode_ComparisonsAndOperators_Exhaustive()
    {
        var c1 = CurrencyCode.USD;
        var c1Copy = CurrencyCode.Create("USD").Value;
        var c2 = CurrencyCode.EUR;

        c1.ShouldSatisfyEqualityContract(c1Copy, c2, (x, y) => x == y, (x, y) => x != y);
        c2.ShouldSatisfyComparisonContract(c2, c1,
            (x, y) => x < y,
            (x, y) => x <= y,
            (x, y) => x > y,
            (x, y) => x >= y);

        c1.CompareTo((object)c1Copy).Should().Be(0);

        Action invalidObj = () => c1.CompareTo("not-a-currency");
        invalidObj.Should().Throw<ArgumentException>()
            .WithMessage("*Object is not a CurrencyCode*");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CurrencyCode_WhenEmpty_ShouldFail(string? invalid)
    {
        var result = CurrencyCode.Create(invalid);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("CurrencyCode.Required");
    }

    [Theory]
    [InlineData("US")]
    [InlineData("USDD")]
    [InlineData("123")]
    [InlineData("US1")]
    public void CurrencyCode_WhenInvalidFormat_ShouldFail(string? invalid)
    {
        var result = CurrencyCode.Create(invalid);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("CurrencyCode.InvalidFormat");
    }

    [Fact]
    public void CurrencyCode_Normalization_TrimsAndUpper()
    {
        var result = CurrencyCode.Create("  usd  ");
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("USD");
        result.Value.ToString().Should().Be("USD");
    }

    [Fact]
    public void CurrencyCode_DefaultState_ComparisonAndEquality()
    {
        var c1 = CurrencyCode.Create("USD").Value;
        var c2 = CurrencyCode.USD;
        var c3 = CurrencyCode.EUR;

        c1.ShouldSatisfyEqualityContract(c2, c3, (a, b) => a == b, (a, b) => a != b);
        c3.ShouldSatisfyComparisonContract(c3, c1,
            (a, b) => a < b,
            (a, b) => a <= b,
            (a, b) => a > b,
            (a, b) => a >= b);

        c1.Should().Be(c2);
        (c1 == c2).Should().BeTrue();
        (c1 != c3).Should().BeTrue();

        c1.CompareTo((object)c2).Should().Be(0);
        Action act = () => c1.CompareTo("invalid");
        act.Should().Throw<ArgumentException>();

        CurrencyCode.GBP.Value.Should().Be("GBP");
        CurrencyCode.KWD.Value.Should().Be("KWD");

        var defaultCode = default(CurrencyCode);
        defaultCode.DecimalPlaces.Should().Be(2);
        defaultCode.ToString().Should().BeEmpty();
    }

    [Fact]
    public void Parsing_StringAndSpan_ParsesOrThrows()
    {
        var c = CurrencyCode.Parse("USD", CultureInfo.InvariantCulture);
        c.Value.Should().Be("USD");

        CurrencyCode.Parse("  eur  ", CultureInfo.InvariantCulture).Value.Should().Be("EUR");
        CurrencyCode.Parse("GBP".AsSpan(), CultureInfo.InvariantCulture).Value.Should().Be("GBP");
        CurrencyCode.Parse("USD".AsSpan(), CultureInfo.InvariantCulture).Should().Be(CurrencyCode.USD);
        CurrencyCode.Parse("CAD".AsSpan(), CultureInfo.InvariantCulture).Value.Should().Be("CAD");

        Action nullAct = () => CurrencyCode.Parse((string)null!, CultureInfo.InvariantCulture);
        nullAct.Should().Throw<FormatException>();

        Action invalidFormat = () => CurrencyCode.Parse("INVALID", CultureInfo.InvariantCulture);
        invalidFormat.Should().Throw<FormatException>()
            .WithMessage("Currency code must be exactly 3 uppercase letters (ISO 4217), got 'INVALID'.");

        Action invalidSpan = () => CurrencyCode.Parse("US".AsSpan(), CultureInfo.InvariantCulture);
        invalidSpan.Should().Throw<FormatException>()
            .WithMessage("Cannot parse 'US' as CurrencyCode.");

        Action invalidSpanLong = () => CurrencyCode.Parse("USDD".AsSpan(), CultureInfo.InvariantCulture);
        invalidSpanLong.Should().Throw<FormatException>()
            .WithMessage("Cannot parse 'USDD' as CurrencyCode.");
    }

    [Fact]
    public void TryParse_StringAndSpan_ReturnsSuccessOrFailure()
    {
        CurrencyCode.TryParse("USD", null, out var r1).Should().BeTrue();
        r1.Value.Should().Be("USD");

        CurrencyCode.TryParse(null, null, out var rNull).Should().BeFalse();
        rNull.Should().Be(default);

        CurrencyCode.TryParse("INVALID", null, out var rInvalid).Should().BeFalse();

        CurrencyCode.TryParse("USD".AsSpan(), null, out var rUsd).Should().BeTrue();
        rUsd.Should().Be(CurrencyCode.USD);

        CurrencyCode.TryParse("EUR".AsSpan(), null, out var rEur).Should().BeTrue();
        rEur.Should().Be(CurrencyCode.EUR);

        CurrencyCode.TryParse("DOP".AsSpan(), null, out var rDop).Should().BeTrue();
        rDop.Should().Be(CurrencyCode.DOP);

        CurrencyCode.TryParse("GBP".AsSpan(), null, out var rGbp).Should().BeTrue();
        rGbp.Should().Be(CurrencyCode.GBP);

        CurrencyCode.TryParse("JPY".AsSpan(), null, out var rJpy).Should().BeTrue();
        rJpy.Should().Be(CurrencyCode.JPY);

        CurrencyCode.TryParse("BHD".AsSpan(), null, out var rBhd).Should().BeTrue();
        rBhd.Should().Be(CurrencyCode.BHD);

        CurrencyCode.TryParse("KWD".AsSpan(), null, out var rKwd).Should().BeTrue();
        rKwd.Should().Be(CurrencyCode.KWD);

        CurrencyCode.TryParse("CAD".AsSpan(), null, out var rCad).Should().BeTrue();
        rCad.Value.Should().Be("CAD");

        CurrencyCode.TryParse("US".AsSpan(), null, out var rSpanShort).Should().BeFalse();
        CurrencyCode.TryParse("USDD".AsSpan(), null, out var rSpanLong).Should().BeFalse();
        CurrencyCode.TryParse("123".AsSpan(), null, out var rDigits).Should().BeFalse();
        CurrencyCode.TryParse("1BC".AsSpan(), null, out var rNonLetter1).Should().BeFalse();
        CurrencyCode.TryParse("A1C".AsSpan(), null, out var rNonLetter2).Should().BeFalse();
        CurrencyCode.TryParse("AB1".AsSpan(), null, out var rNonLetter3).Should().BeFalse();
    }
}




