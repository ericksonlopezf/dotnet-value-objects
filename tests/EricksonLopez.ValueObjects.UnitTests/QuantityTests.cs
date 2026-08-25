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
/// Unit tests for the <see cref="Quantity"/> Value Object.
/// </summary>
public sealed class QuantityTests
{
    [Fact]
    public void Quantity_Create_AndArithmetic()
    {
        var q0 = Quantity.Create(0).Value;
        var q1 = Quantity.Create(10).Value;
        var q1Copy = Quantity.Create(10).Value;
        var q2 = Quantity.Create(5).Value;

        q0.IsZero.Should().BeTrue();
        q0.Value.Should().Be(0);

        q1.Value.Should().Be(10);
        q1.Add(q2).Value.Value.Should().Be(15);
        q1.Subtract(q2).Value.Value.Should().Be(5);
        q1.Subtract(q1Copy).Value.Value.Should().Be(0);
        var subRes = q2.Subtract(q1);
        subRes.IsFailure.Should().BeTrue();
        subRes.Error.Code.Should().Be("Quantity.InsufficientQuantity");

        Quantity.Zero.IsZero.Should().BeTrue();
        q1.ToString().Should().Be("10");

        q2.ShouldSatisfyEqualityContract(Quantity.Create(5).Value, q1, (x, y) => x == y, (x, y) => x != y);
        q2.ShouldSatisfyComparisonContract(Quantity.Create(5).Value, q1,
            (x, y) => x < y,
            (x, y) => x <= y,
            (x, y) => x > y,
            (x, y) => x >= y);

        Action invalidObj = () => q1.CompareTo("not-a-quantity");
        invalidObj.Should().Throw<ArgumentException>()
            .WithMessage("Object is not a Quantity*")
            .WithParameterName("obj");

        ((IComparable)q1).CompareTo((object)q1).Should().Be(0);
        ((IComparable)q2).CompareTo((object)q1).Should().BeNegative();

        Action nullObj = () => ((IComparable)q1).CompareTo(null);
        nullObj.Should().Throw<ArgumentException>()
            .WithMessage("Object is not a Quantity*")
            .WithParameterName("obj");
    }

    [Fact]
    public void Quantity_Negative_ShouldFail()
    {
        var result = Quantity.Create(-1);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Quantity.Negative");
    }

    [Fact]
    public void Formatting_WithFormatsAndProviders_BehavesCorrectly()
    {
        var q = Quantity.Create(1000).Value;

        q.ToString().Should().Be("1000");
        q.ToString(null, null).Should().Be("1000");
        q.ToString("N0", CultureInfo.InvariantCulture).Should().Be("1,000");

        var frCulture = CultureInfo.GetCultureInfo("fr-FR");
        q.ToString("N0", frCulture).Should().Be("1\u202f000");

        Span<char> exactBuffer = stackalloc char[4];
        q.TryFormat(exactBuffer, out int charsWritten, default, null).Should().BeTrue();
        charsWritten.Should().Be(4);
        exactBuffer[..charsWritten].ToString().Should().Be("1000");

        Span<char> exactFrBuffer = stackalloc char[5];
        q.TryFormat(exactFrBuffer, out int frWritten, "N0".AsSpan(), frCulture).Should().BeTrue();
        frWritten.Should().Be(5);
        exactFrBuffer[..frWritten].ToString().Should().Be("1\u202f000");

        Span<char> buffer = stackalloc char[10];
        q.TryFormat(buffer, out int customWritten, "N0".AsSpan(), CultureInfo.InvariantCulture).Should().BeTrue();
        customWritten.Should().Be(5);
        buffer[..customWritten].ToString().Should().Be("1,000");

        Span<char> smallBuffer = stackalloc char[3];
        q.TryFormat(smallBuffer, out int smallWritten, default, null).Should().BeFalse();
        smallWritten.Should().Be(0);
    }

    [Fact]
    public void Parsing_StringAndSpan_ParsesOrThrows()
    {
        var q = Quantity.Parse("50", CultureInfo.InvariantCulture);
        q.Value.Should().Be(50);

        Quantity.Parse("  100  ", CultureInfo.InvariantCulture).Value.Should().Be(100);
        Quantity.Parse("20".AsSpan(), CultureInfo.InvariantCulture).Value.Should().Be(20);

        var frCulture = CultureInfo.GetCultureInfo("fr-FR");
        Quantity.Parse("1\u202f000", frCulture).Value.Should().Be(1000);
        Quantity.Parse("1\u202f000".AsSpan(), frCulture).Value.Should().Be(1000);

        Action nullAct = () => Quantity.Parse((string)null!, CultureInfo.InvariantCulture);
        nullAct.Should().Throw<ArgumentNullException>();

        Action invalidRange = () => Quantity.Parse("-5", CultureInfo.InvariantCulture);
        invalidRange.Should().Throw<FormatException>()
            .WithMessage("Quantity cannot be negative.");

        Action invalidFormat = () => Quantity.Parse("abc", CultureInfo.InvariantCulture);
        invalidFormat.Should().Throw<FormatException>()
            .WithMessage("Cannot parse 'abc' as Quantity.");

        Action invalidSpanRange = () => Quantity.Parse("-10".AsSpan(), CultureInfo.InvariantCulture);
        invalidSpanRange.Should().Throw<FormatException>()
            .WithMessage("Quantity cannot be negative.");

        Action invalidSpanFormat = () => Quantity.Parse("invalid".AsSpan(), CultureInfo.InvariantCulture);
        invalidSpanFormat.Should().Throw<FormatException>()
            .WithMessage("Cannot parse 'invalid' as Quantity.");
    }

    [Fact]
    public void TryParse_StringAndSpan_ReturnsSuccessOrFailure()
    {
        Quantity.TryParse("50", CultureInfo.InvariantCulture, out var r1).Should().BeTrue();
        r1.Value.Should().Be(50);

        Quantity.TryParse("1000", null, out var rDefault).Should().BeTrue();
        rDefault.Value.Should().Be(1000);

        var frCulture = CultureInfo.GetCultureInfo("fr-FR");
        Quantity.TryParse("1\u202f000", frCulture, out var rFr).Should().BeTrue();
        rFr.Value.Should().Be(1000);

        Quantity.TryParse(null, null, out var rNull).Should().BeFalse();
        rNull.Should().Be(default);

        Quantity.TryParse("   ", null, out var rWhite).Should().BeFalse();
        Quantity.TryParse("-5", null, out var rNeg).Should().BeFalse();
        Quantity.TryParse("abc", null, out var rInvalid).Should().BeFalse();

        Quantity.TryParse("100".AsSpan(), CultureInfo.InvariantCulture, out var rSpan).Should().BeTrue();
        rSpan.Value.Should().Be(100);

        Quantity.TryParse("1\u202f000".AsSpan(), frCulture, out var rFrSpan).Should().BeTrue();
        rFrSpan.Value.Should().Be(1000);

        Quantity.TryParse("-10".AsSpan(), null, out var rSpanNeg).Should().BeFalse();
        Quantity.TryParse("invalid".AsSpan(), null, out var rSpanInvalid).Should().BeFalse();
    }
}




