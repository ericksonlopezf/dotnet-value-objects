// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="Percentage"/> Value Object.
/// </summary>
public sealed class PercentageTests
{
    [Fact]
    public void ComparisonAndEqualityContracts_WhenValidPercentages_SatisfiesContracts()
    {
        var p0 = Percentage.Create(0m).Value;
        var p10 = Percentage.Create(10m).Value;
        var p10Copy = Percentage.Create(10m).Value;
        var p100 = Percentage.Create(100m).Value;

        p0.IsZero.Should().BeTrue();
        p10.IsZero.Should().BeFalse();

        p10.ShouldSatisfyEqualityContract(p10Copy, p0, (a, b) => a == b, (a, b) => a != b);
        p0.ShouldSatisfyComparisonContract(p0, p10,
            (a, b) => a < b,
            (a, b) => a <= b,
            (a, b) => a > b,
            (a, b) => a >= b);

        p10.CompareTo(p10Copy).Should().Be(0);
        p10.CompareTo((object)p10Copy).Should().Be(0);

        p10.ApplyTo(200m).Should().Be(20m);
        p10.Fraction.Should().Be(0.10m);
        p10.AsFraction.Should().Be(0.10m);
        Percentage.Zero.Value.Should().Be(0m);
        Percentage.Hundred.Value.Should().Be(100m);
        Percentage.Full.Value.Should().Be(100m);
        Percentage.FromFraction(0.18m).Value.Value.Should().Be(18m);
        Percentage.Create(18.5m).Value.ToString().Should().Be("18.5%");

        var tooManyDec = Percentage.Create(18.1234567m);
        tooManyDec.IsFailure.Should().BeTrue();
        tooManyDec.Error.Code.Should().Be("Percentage.TooManyDecimals");
        tooManyDec.Error.Description.Should().Be("Percentage supports at most 6 decimal places.");

        var outOfRangeNegative = Percentage.Create(-0.01m);
        outOfRangeNegative.IsFailure.Should().BeTrue();
        outOfRangeNegative.Error.Code.Should().Be("Percentage.OutOfRange");

        var outOfRange = Percentage.Create(100.01m);
        outOfRange.IsFailure.Should().BeTrue();
        outOfRange.Error.Code.Should().Be("Percentage.OutOfRange");
        outOfRange.Error.Description.Should().Be("Percentage must be between 0 and 100.");
    }

    [Fact]
    public void ComparisonAndDefaults_WhenComparedAgainstInvalidType_ThrowsArgumentException()
    {
        var p1 = Percentage.Create(10m).Value;
        var p2 = Percentage.Create(20m).Value;

        p1.CompareTo(p2).Should().BeNegative();
        p1.CompareTo((object)p2).Should().BeNegative();
        p1.IsZero.Should().BeFalse();
        Percentage.Zero.IsZero.Should().BeTrue();
        Percentage.Hundred.Value.Should().Be(100m);
        Percentage.Full.Value.Should().Be(100m);
        p1.AsFraction.Should().Be(0.1m);
        default(Percentage).ToString().Should().Be("0%");

        ((IComparable)p1).CompareTo((object)p1).Should().Be(0);
        ((IComparable)p1).CompareTo((object)p2).Should().BeNegative();

        Action pInvalid = () => p1.CompareTo("not-a-percentage");
        pInvalid.Should().Throw<ArgumentException>()
            .WithMessage("Object is not a Percentage*")
            .WithParameterName("obj");

        Action pNull = () => ((IComparable)p1).CompareTo(null);
        pNull.Should().Throw<ArgumentException>()
            .WithMessage("Object is not a Percentage*")
            .WithParameterName("obj");
    }

    [Fact]
    public void Formatting_WithFormatsAndProviders_BehavesCorrectly()
    {
        var p = Percentage.Create(18.5m).Value;
        var pInt = Percentage.Create(18m).Value;

        p.ToString().Should().Be("18.5%");
        pInt.ToString().Should().Be("18%");
        p.ToString(null, null).Should().Be("18.5%");
        p.ToString("0.00", CultureInfo.InvariantCulture).Should().Be("18.50%");

        var frCulture = CultureInfo.GetCultureInfo("fr-FR");
        p.ToString("0.0", frCulture).Should().Be("18,5%");

        Span<char> exactBuffer = stackalloc char[5];
        p.TryFormat(exactBuffer, out int charsWritten, default, null).Should().BeTrue();
        charsWritten.Should().Be(5);
        exactBuffer[..charsWritten].ToString().Should().Be("18.5%");

        Span<char> buffer = stackalloc char[10];
        p.TryFormat(buffer, out int customWritten, "0.00".AsSpan(), CultureInfo.InvariantCulture).Should().BeTrue();
        customWritten.Should().Be(6);
        buffer[..customWritten].ToString().Should().Be("18.50%");

        Span<char> smallBuffer = stackalloc char[4];
        p.TryFormat(smallBuffer, out int smallWritten, default, null).Should().BeFalse();
        smallWritten.Should().Be(0);
    }

    [Fact]
    public void Parsing_StringAndSpan_ParsesOrThrows()
    {
        var p = Percentage.Parse("18.5%", CultureInfo.InvariantCulture);
        p.Value.Should().Be(18.5m);

        Percentage.Parse("  25 %  ", CultureInfo.InvariantCulture).Value.Should().Be(25m);

        var frCulture = CultureInfo.GetCultureInfo("fr-FR");
        Percentage.Parse("18,5%", frCulture).Value.Should().Be(18.5m);
        Percentage.Parse("18,5%".AsSpan(), frCulture).Value.Should().Be(18.5m);

        Percentage.Parse("50".AsSpan(), CultureInfo.InvariantCulture).Value.Should().Be(50m);

        Action nullAct = () => Percentage.Parse((string)null!, CultureInfo.InvariantCulture);
        nullAct.Should().Throw<ArgumentNullException>();

        Action invalidRange = () => Percentage.Parse("150%", CultureInfo.InvariantCulture);
        invalidRange.Should().Throw<FormatException>()
            .WithMessage("Percentage must be between 0 and 100.");

        Action invalidSpanRange = () => Percentage.Parse("-10%".AsSpan(), CultureInfo.InvariantCulture);
        invalidSpanRange.Should().Throw<FormatException>()
            .WithMessage("Percentage must be between 0 and 100.");

        Action invalidFormat = () => Percentage.Parse("abc%", CultureInfo.InvariantCulture);
        invalidFormat.Should().Throw<FormatException>()
            .WithMessage("Cannot parse 'abc%' as Percentage.");

        Action invalidSpanFormat = () => Percentage.Parse("invalid".AsSpan(), CultureInfo.InvariantCulture);
        invalidSpanFormat.Should().Throw<FormatException>()
            .WithMessage("Cannot parse 'invalid' as Percentage.");
    }

    [Fact]
    public void TryParse_StringAndSpan_ReturnsSuccessOrFailure()
    {
        Percentage.TryParse("18.5%", CultureInfo.InvariantCulture, out var r1).Should().BeTrue();
        r1.Value.Should().Be(18.5m);

        var frCulture = CultureInfo.GetCultureInfo("fr-FR");
        Percentage.TryParse("18,5%", frCulture, out var rFr).Should().BeTrue();
        rFr.Value.Should().Be(18.5m);

        Percentage.TryParse("18,5%".AsSpan(), frCulture, out var rSpanFr).Should().BeTrue();
        rSpanFr.Value.Should().Be(18.5m);

        Percentage.TryParse(null, null, out var rNull).Should().BeFalse();
        rNull.Should().Be(default);

        Percentage.TryParse("   ", null, out var rWhite).Should().BeFalse();
        Percentage.TryParse("150%", null, out var rOut).Should().BeFalse();
        Percentage.TryParse("-10%".AsSpan(), CultureInfo.InvariantCulture, out _).Should().BeFalse();
        Percentage.TryParse("abc%", null, out var rInvalid).Should().BeFalse();
        Percentage.TryParse("invalid".AsSpan(), null, out _).Should().BeFalse();

        Percentage.TryParse("25%".AsSpan(), CultureInfo.InvariantCulture, out var rSpan).Should().BeTrue();
        rSpan.Value.Should().Be(25m);

        Percentage.TryParse("invalid".AsSpan(), null, out var rSpanInvalid).Should().BeFalse();
    }
}



