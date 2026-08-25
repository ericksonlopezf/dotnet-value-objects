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
/// Unit tests for the <see cref="BusinessDate"/> Value Object.
/// </summary>
public sealed class BusinessDateTests
{
    [Fact]
    public void BusinessDate_ValidDate_ShouldSucceed()
    {
        var date = new DateOnly(2026, 6, 15);
        var result = BusinessDate.Create(date);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(date);
        result.Value.ToString().Should().Be("2026-06-15");
    }

    [Fact]
    public void BusinessDate_FromDateTimeOffset_DiscardsTime()
    {
        var dto = new DateTimeOffset(2026, 8, 15, 14, 30, 0, TimeSpan.Zero);
        var result = BusinessDate.FromDateTimeOffset(dto);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(new DateOnly(2026, 8, 15));
    }

    [Fact]
    public void BusinessDate_DefaultState_RejectsMinAndMaxValue()
    {
        var minResult = BusinessDate.Create(DateOnly.MinValue);
        minResult.IsFailure.Should().BeTrue();
        minResult.Error.Code.Should().Be("BusinessDate.OutOfRange");
        minResult.Error.Description.Should().Be("Business date cannot be DateOnly.MinValue or DateOnly.MaxValue.");

        var maxResult = BusinessDate.Create(DateOnly.MaxValue);
        maxResult.IsFailure.Should().BeTrue();
        maxResult.Error.Code.Should().Be("BusinessDate.OutOfRange");
    }

    [Fact]
    public void BusinessDate_Equality_SameDate_AreEqual()
    {
        var a = BusinessDate.Create(new DateOnly(2026, 1, 1)).Value;
        var b = BusinessDate.Create(new DateOnly(2026, 1, 1)).Value;
        var c = BusinessDate.Create(new DateOnly(2026, 1, 2)).Value;

        a.ShouldSatisfyEqualityContract(b, c, (x, y) => x == y, (x, y) => x != y);
    }

    [Fact]
    public void BusinessDate_CompareTo_ObjectAndOperators()
    {
        var d1 = BusinessDate.Create(new DateOnly(2026, 1, 1)).Value;
        var d1Copy = BusinessDate.Create(new DateOnly(2026, 1, 1)).Value;
        var d2 = BusinessDate.Create(new DateOnly(2026, 1, 2)).Value;

        d1.ShouldSatisfyComparisonContract(d1Copy, d2,
            (a, b) => a < b,
            (a, b) => a <= b,
            (a, b) => a > b,
            (a, b) => a >= b);

        d1.ToString().Should().Be("2026-01-01");

        ((IComparable)d1).CompareTo((object)d1).Should().Be(0);
        ((IComparable)d1).CompareTo((object)d2).Should().BeNegative();

        Action act = () => d1.CompareTo("not-a-date");
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Object is not a BusinessDate*")
            .WithParameterName("obj");

        Action actNull = () => ((IComparable)d1).CompareTo(null);
        actNull.Should().Throw<ArgumentException>()
            .WithMessage("*Object is not a BusinessDate*")
            .WithParameterName("obj");
    }

    [Fact]
    public void Formatting_WithFormatsAndProviders_BehavesCorrectly()
    {
        var d = BusinessDate.Create(new DateOnly(2026, 8, 15)).Value;

        d.ToString().Should().Be("2026-08-15");
        d.ToString(null, null).Should().Be("2026-08-15");
        d.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture).Should().Be("15/08/2026");

        var frCulture = CultureInfo.GetCultureInfo("fr-FR");
        d.ToString("MMMM", frCulture).Should().Be("août");
        d.ToString("MMMM", CultureInfo.InvariantCulture).Should().Be("August");

        Span<char> buffer = stackalloc char[20];
        d.TryFormat(buffer, out int charsWritten, default, null).Should().BeTrue();
        charsWritten.Should().Be(10);
        buffer[..charsWritten].ToString().Should().Be("2026-08-15");

        d.TryFormat(buffer, out int customWritten, "dd/MM/yyyy".AsSpan(), CultureInfo.InvariantCulture).Should().BeTrue();
        customWritten.Should().Be(10);
        buffer[..customWritten].ToString().Should().Be("15/08/2026");

        d.TryFormat(buffer, out int frWritten, "MMMM".AsSpan(), frCulture).Should().BeTrue();
        frWritten.Should().Be(4);
        buffer[..frWritten].ToString().Should().Be("août");

        Span<char> smallBuffer = stackalloc char[5];
        d.TryFormat(smallBuffer, out int smallWritten, default, null).Should().BeFalse();
        smallWritten.Should().Be(0);
    }

    [Fact]
    public void Parsing_StringAndSpan_ParsesOrThrows()
    {
        var d = BusinessDate.Parse("2026-08-15", CultureInfo.InvariantCulture);
        d.Value.Should().Be(new DateOnly(2026, 8, 15));

        BusinessDate.Parse("2026-08-15".AsSpan(), CultureInfo.InvariantCulture).Value.Should().Be(new DateOnly(2026, 8, 15));

        var esCulture = CultureInfo.GetCultureInfo("es-DO");
        BusinessDate.Parse("15/08/2026", esCulture).Value.Should().Be(new DateOnly(2026, 8, 15));
        BusinessDate.Parse("15/08/2026".AsSpan(), esCulture).Value.Should().Be(new DateOnly(2026, 8, 15));

        Action nullAct = () => BusinessDate.Parse((string)null!, CultureInfo.InvariantCulture);
        nullAct.Should().Throw<ArgumentNullException>();

        Action minDate = () => BusinessDate.Parse("0001-01-01", CultureInfo.InvariantCulture);
        minDate.Should().Throw<FormatException>()
            .WithMessage("Business date cannot be DateOnly.MinValue or DateOnly.MaxValue.");

        Action maxDate = () => BusinessDate.Parse("9999-12-31", CultureInfo.InvariantCulture);
        maxDate.Should().Throw<FormatException>()
            .WithMessage("Business date cannot be DateOnly.MinValue or DateOnly.MaxValue.");

        Action invalidFormat = () => BusinessDate.Parse("not-a-date", CultureInfo.InvariantCulture);
        invalidFormat.Should().Throw<FormatException>()
            .WithMessage("Cannot parse 'not-a-date' as BusinessDate.");

        Action invalidSpanMin = () => BusinessDate.Parse("0001-01-01".AsSpan(), CultureInfo.InvariantCulture);
        invalidSpanMin.Should().Throw<FormatException>()
            .WithMessage("Business date cannot be DateOnly.MinValue or DateOnly.MaxValue.");

        Action invalidSpanMax = () => BusinessDate.Parse("9999-12-31".AsSpan(), CultureInfo.InvariantCulture);
        invalidSpanMax.Should().Throw<FormatException>()
            .WithMessage("Business date cannot be DateOnly.MinValue or DateOnly.MaxValue.");

        Action invalidSpan = () => BusinessDate.Parse("invalid".AsSpan(), CultureInfo.InvariantCulture);
        invalidSpan.Should().Throw<FormatException>()
            .WithMessage("Cannot parse 'invalid' as BusinessDate.");
    }

    [Fact]
    public void TryParse_StringAndSpan_ReturnsSuccessOrFailure()
    {
        BusinessDate.TryParse("2026-08-15", CultureInfo.InvariantCulture, out var r1).Should().BeTrue();
        r1.Value.Should().Be(new DateOnly(2026, 8, 15));

        BusinessDate.TryParse("2026-08-15", null, out var rDefault).Should().BeTrue();
        rDefault.Value.Should().Be(new DateOnly(2026, 8, 15));

        var esCulture = CultureInfo.GetCultureInfo("es-DO");
        BusinessDate.TryParse("15/08/2026", esCulture, out var rEs).Should().BeTrue();
        rEs.Value.Should().Be(new DateOnly(2026, 8, 15));

        BusinessDate.TryParse(null, null, out var rNull).Should().BeFalse();
        rNull.Should().Be(default);

        BusinessDate.TryParse("0001-01-01", null, out var rMin).Should().BeFalse();
        BusinessDate.TryParse("9999-12-31", null, out var rMax).Should().BeFalse();
        BusinessDate.TryParse("not-a-date", null, out var rInvalid).Should().BeFalse();

        BusinessDate.TryParse("2026-08-15".AsSpan(), CultureInfo.InvariantCulture, out var rSpan).Should().BeTrue();
        rSpan.Value.Should().Be(new DateOnly(2026, 8, 15));

        BusinessDate.TryParse("2026-08-15".AsSpan(), null, out var rSpanDef).Should().BeTrue();
        rSpanDef.Value.Should().Be(new DateOnly(2026, 8, 15));

        BusinessDate.TryParse("15/08/2026".AsSpan(), esCulture, out var rEsSpan).Should().BeTrue();
        rEsSpan.Value.Should().Be(new DateOnly(2026, 8, 15));

        BusinessDate.TryParse("0001-01-01".AsSpan(), null, out var rSpanMin).Should().BeFalse();
        BusinessDate.TryParse("9999-12-31".AsSpan(), null, out var rSpanMax).Should().BeFalse();
        BusinessDate.TryParse("invalid".AsSpan(), null, out var rSpanInvalid).Should().BeFalse();
    }
}




