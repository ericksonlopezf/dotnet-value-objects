// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Peru;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Peru.UnitTests;

public sealed class SunatProductCodeTests
{
    [Theory]
    [InlineData("10101501")]
    [InlineData("50202306")]
    [InlineData("84111506")]
    public void Create_Valid8DigitCode_Succeeds(string code)
    {
        var result = SunatProductCode.Create(code);

        result.IsSuccess.Should().BeTrue();
        result.Value.Code.Should().Be(code);
        result.Value.ToString().Should().Be(code);
    }

    [Theory]
    [InlineData("1010150")] // 7 digits
    [InlineData("101015011")] // 9 digits
    [InlineData("")]
    public void Create_InvalidLength_Before2027_PermissiveFallback(string code)
    {
        var dateBefore2027 = new DateOnly(2026, 12, 31);
        var result = SunatProductCode.Create(code, dateBefore2027);

        result.IsSuccess.Should().BeTrue();
        result.Value.Code.Should().Be(code.Trim());
    }

    [Theory]
    [InlineData("1010150")] // 7 digits
    [InlineData("101015011")] // 9 digits
    [InlineData("")]
    public void Create_InvalidLength_From2027_ReturnsFailure(string code)
    {
        var dateFrom2027 = new DateOnly(2027, 1, 1);
        var result = SunatProductCode.Create(code, dateFrom2027);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SunatProductCode.InvalidLength");
    }

    [Theory]
    [InlineData("1010150A")]
    [InlineData("ABCD5678")]
    public void Create_InvalidCharacters_ReturnsFailure(string code)
    {
        var result = SunatProductCode.Create(code);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SunatProductCode.InvalidCharacters");
    }

    [Fact]
    public void ComparisonOperators_DefaultState_WorkCorrectly()
    {
        var c1 = SunatProductCode.Create("10101501").Value;
        var c2 = SunatProductCode.Create("50202306").Value;
        var c1Clone = SunatProductCode.Create("10101501").Value;

        (c1 < c2).Should().BeTrue();
        (c1 <= c2).Should().BeTrue();
        (c2 > c1).Should().BeTrue();
        (c2 >= c1).Should().BeTrue();

        (c2 < c1).Should().BeFalse();
        (c1 > c2).Should().BeFalse();
        (c1 < c1Clone).Should().BeFalse();
        (c1 > c1Clone).Should().BeFalse();

        (c1 == c1Clone).Should().BeTrue();
        (c1 <= c1Clone).Should().BeTrue();
        (c1 >= c1Clone).Should().BeTrue();

        c1.CompareTo(c2).Should().BeNegative();
        c2.CompareTo(c1).Should().BePositive();
        c1.CompareTo(c1Clone).Should().Be(0);

        default(SunatProductCode).ToString().Should().BeEmpty();
    }

    [Fact]
    public void ParseAndTryParse_DefaultState_WorkCorrectly()
    {
        var parsed1 = SunatProductCode.Parse("10101501", CultureInfo.InvariantCulture);
        parsed1.Code.Should().Be("10101501");

        var parsed2 = SunatProductCode.Parse("10101501".AsSpan(), CultureInfo.InvariantCulture);
        parsed2.Code.Should().Be("10101501");

        SunatProductCode.TryParse("10101501", null, out var tryRes1).Should().BeTrue();
        tryRes1.Code.Should().Be("10101501");

        SunatProductCode.TryParse("10101501".AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Code.Should().Be("10101501");

        SunatProductCode.TryParse("invalid", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(SunatProductCode));

        SunatProductCode.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(SunatProductCode));

        SunatProductCode.TryParse("invalid".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(SunatProductCode));

        Action invalidParseStr = () => SunatProductCode.Parse("invalid", CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid SunatProductCode: 'invalid'.");

        Action invalidParseSpan = () => SunatProductCode.Parse("invalid".AsSpan(), CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid SunatProductCode: 'invalid'.");
    }
}




