// Copyright © Erickson Lopez. MIT License.
using System;
using System.Text;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Argentina;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Argentina.UnitTests;

public sealed class CbuTests
{
    [Theory]
    [InlineData("0720000700000001234565", "072", "0000", "0000000123456")]
    [InlineData("0000000000000000000000", "000", "0000", "0000000000000")]
    public void Create_ValidCbu_ExtractsProperties(
        string input,
        string expectedBank,
        string expectedBranch,
        string expectedAccount)
    {
        var result = Cbu.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(input);
        result.Value.BankCode.Should().Be(expectedBank);
        result.Value.BranchCode.Should().Be(expectedBranch);
        result.Value.AccountNumber.Should().Be(expectedAccount);
        result.Value.ToString().Should().Be(input);
    }

    [Fact]
    public void CalculateCheckDigits_RemainderZero_And_NonZero()
    {
        Cbu.CalculateBlock1CheckDigit("0000000".AsSpan()).Should().Be(0);
        Cbu.CalculateBlock1CheckDigit("0720000".AsSpan()).Should().Be(7);

        Cbu.CalculateBlock2CheckDigit("0000000000000".AsSpan()).Should().Be(0);
        Cbu.CalculateBlock2CheckDigit("0000000123456".AsSpan()).Should().Be(5);
    }

    [Theory]
    [InlineData("0720000000000001234565")] // Invalid Check Digit 1
    public void Create_InvalidCheckDigit1_ReturnsError(string input)
    {
        var result = Cbu.Create(input);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Cbu.InvalidCheckDigit1");
    }

    [Theory]
    [InlineData("0720000700000001234569")] // Invalid Check Digit 2
    public void Create_InvalidCheckDigit2_ReturnsError(string input)
    {
        var result = Cbu.Create(input);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Cbu.InvalidCheckDigit2");
    }

    [Theory]
    [InlineData("072000070000000123456")]   // 21 chars (too short)
    [InlineData("07200007000000012345655")] // 23 chars (too long)
    [InlineData("")]
    [InlineData("   ")]
    public void Create_InvalidLength_ReturnsError(string input)
    {
        var result = Cbu.Create(input);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Cbu.InvalidLength");
    }

    [Theory]
    [InlineData("072000070000000123456A")]
    [InlineData("A720000700000001234565")]
    public void Create_InvalidCharacters_ReturnsError(string input)
    {
        var result = Cbu.Create(input);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Cbu.InvalidCharacters");
    }

    [Fact]
    public void Cbu_ParseAndTryParse_StringAndSpan()
    {
        var validStr = "0720000700000001234565";
        var parsed1 = Cbu.Parse(validStr, System.Globalization.CultureInfo.InvariantCulture);
        parsed1.Value.Should().Be(validStr);

        var parsed2 = Cbu.Parse(validStr.AsSpan(), System.Globalization.CultureInfo.InvariantCulture);
        parsed2.Value.Should().Be(validStr);

        Cbu.TryParse(validStr, null, out var tryRes1).Should().BeTrue();
        tryRes1.Value.Should().Be(validStr);

        Cbu.TryParse(validStr.AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Value.Should().Be(validStr);

        Action invalidParseStr = () => Cbu.Parse("invalid", System.Globalization.CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid CBU: 'invalid'.");

        Action invalidParseSpan = () => Cbu.Parse("invalid".AsSpan(), System.Globalization.CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid CBU: 'invalid'.");

        Cbu.TryParse("invalid", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(Cbu));

        Cbu.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(Cbu));

        Cbu.TryParse("invalid".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(Cbu));
    }

    [Fact]
    public void Cbu_ParseAndTryParse_Utf8()
    {
        byte[] validUtf8 = Encoding.UTF8.GetBytes("0720000700000001234565");
        var parsed = Cbu.Parse(validUtf8, System.Globalization.CultureInfo.InvariantCulture);
        parsed.Value.Should().Be("0720000700000001234565");

        Cbu.TryParse(validUtf8, null, out var tryRes).Should().BeTrue();
        tryRes.Value.Should().Be("0720000700000001234565");

        byte[] invalidUtf8 = Encoding.UTF8.GetBytes("invalid");
        Action invalidParseUtf8 = () => Cbu.Parse(invalidUtf8, System.Globalization.CultureInfo.InvariantCulture);
        invalidParseUtf8.Should().Throw<FormatException>().WithMessage("Invalid UTF-8 CBU representation.");

        Cbu.TryParse(invalidUtf8, null, out var tryFail).Should().BeFalse();
        tryFail.Should().Be(default(Cbu));

        byte[] brokenUtf8 = [0xFF, 0xFE, 0xFD];
        Cbu.TryParse(brokenUtf8, null, out var tryBroken).Should().BeFalse();
        tryBroken.Should().Be(default(Cbu));
    }

    [Fact]
    public void Cbu_ComparisonsAndOperators_Exhaustive()
    {
        var a = Cbu.Create("0000000000000000000000").Value;
        var aCopy = Cbu.Create("0000000000000000000000").Value;
        var b = Cbu.Create("0720000700000001234565").Value;

        a.ShouldSatisfyEqualityContract(aCopy, b, (x, y) => x == y, (x, y) => x != y);
        a.ShouldSatisfyComparisonContract(aCopy, b,
            (x, y) => x < y,
            (x, y) => x <= y,
            (x, y) => x > y,
            (x, y) => x >= y);
    }
}




