// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Mexico;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Mexico.UnitTests;

public sealed class PedimentoNumberTests
{
    [Theory]
    [InlineData("244739990001234", "24", "47", "3999", "0001234")]
    [InlineData("24  47  3999  0001234", "24", "47", "3999", "0001234")]
    [InlineData("  244739990001234  ", "24", "47", "3999", "0001234")]
    public void Create_ValidPedimento_ExtractsComponents(
        string input,
        string expectedYear,
        string expectedCustoms,
        string expectedPatent,
        string expectedSequential)
    {
        var result = PedimentoNumber.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Digits.Should().Be("244739990001234");
        result.Value.Year.Should().Be(expectedYear);
        result.Value.CustomsOffice.Should().Be(expectedCustoms);
        result.Value.Patent.Should().Be(expectedPatent);
        result.Value.Sequential.Should().Be(expectedSequential);
        result.Value.Formatted.Should().Be("24  47  3999  0001234");
        result.Value.ToString().Should().Be("24  47  3999  0001234");
    }

    [Theory]
    [InlineData("24473999000123")]    // 14
    [InlineData("2447399900012345")]  // 16
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_InvalidLength_ReturnsError(string? input)
    {
        var result = PedimentoNumber.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("PedimentoNumber.InvalidLength");
    }

    [Theory]
    [InlineData("24473999000123A")]
    [InlineData("24-47-3999-0001234")]
    [InlineData("24#47#3999#0001234")]
    public void Create_InvalidCharacters_ReturnsError(string input)
    {
        var result = PedimentoNumber.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("PedimentoNumber.InvalidCharacters");
    }

    [Fact]
    public void PedimentoNumber_DefaultState_ComparisonOperators()
    {
        var p1 = PedimentoNumber.Create("234739990001234").Value;
        var p2 = PedimentoNumber.Create("244739990001234").Value;
        var p1Clone = PedimentoNumber.Create("23  47  3999  0001234").Value;

        (p1 < p2).Should().BeTrue();
        (p1 <= p2).Should().BeTrue();
        (p2 > p1).Should().BeTrue();
        (p2 >= p1).Should().BeTrue();

        (p1 < p1Clone).Should().BeFalse();
        (p1 > p1Clone).Should().BeFalse();
        (p1 <= p1Clone).Should().BeTrue();
        (p1 >= p1Clone).Should().BeTrue();
        p1.CompareTo(p2).Should().BeNegative();
        p2.CompareTo(p1).Should().BePositive();
        p1.CompareTo(p1Clone).Should().Be(0);
    }

    [Fact]
    public void PedimentoNumber_DefaultState_ParseAndTryParse()
    {
        var validStr = "244739990001234";
        var parsed1 = PedimentoNumber.Parse(validStr, CultureInfo.InvariantCulture);
        parsed1.Digits.Should().Be(validStr);

        var parsed2 = PedimentoNumber.Parse(validStr.AsSpan(), CultureInfo.InvariantCulture);
        parsed2.Digits.Should().Be(validStr);

        PedimentoNumber.TryParse(validStr, null, out var tryRes1).Should().BeTrue();
        tryRes1.Digits.Should().Be(validStr);

        PedimentoNumber.TryParse(validStr.AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Digits.Should().Be(validStr);

        Action invalidParseStr = () => PedimentoNumber.Parse("invalid", CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid PedimentoNumber: 'invalid'.");

        Action invalidParseSpan = () => PedimentoNumber.Parse("invalid".AsSpan(), CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid PedimentoNumber: 'invalid'.");

        PedimentoNumber.TryParse("invalid", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(PedimentoNumber));

        PedimentoNumber.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(PedimentoNumber));

        PedimentoNumber.TryParse("invalid".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(PedimentoNumber));
    }
}




