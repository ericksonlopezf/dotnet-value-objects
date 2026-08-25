// Copyright © Erickson Lopez. MIT License.
using System;
using System.Text;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Argentina;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Argentina.UnitTests;

public sealed class CvuTests
{
    [Theory]
    [InlineData("0000001700000001234565", "00000017", "00000001234565")]
    [InlineData("0000000000000000000000", "00000000", "00000000000000")]
    public void Create_ValidCvu_ExtractsProperties(
        string input,
        string expectedPsp,
        string expectedAccount)
    {
        var result = Cvu.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(input);
        result.Value.PspCode.Should().Be(expectedPsp);
        result.Value.AccountNumber.Should().Be(expectedAccount);
        result.Value.ToString().Should().Be(input);
    }

    [Theory]
    [InlineData("0720000700000001234565")] // Starts with 072 instead of 000
    [InlineData("1000000000000000000000")]
    public void Create_NonZeroPrefix_ReturnsInvalidPrefixError(string input)
    {
        var result = Cvu.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Cvu.InvalidPrefix");
        result.Error.Description.Should().Contain("PSP code");
    }

    [Fact]
    public void Create_InvalidCbuChecks_PropagatesCbuFailure()
    {
        var result = Cvu.Create("0000001900000001234565"); // Invalid check digit 1 for 0000001 (should be 7)
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Cbu.InvalidCheckDigit1");
    }

    [Fact]
    public void Cvu_ParseAndTryParse_StringAndSpan()
    {
        var validStr = "0000001700000001234565";
        var parsed1 = Cvu.Parse(validStr, System.Globalization.CultureInfo.InvariantCulture);
        parsed1.Value.Should().Be(validStr);

        var parsed2 = Cvu.Parse(validStr.AsSpan(), System.Globalization.CultureInfo.InvariantCulture);
        parsed2.Value.Should().Be(validStr);

        Cvu.TryParse(validStr, null, out var tryRes1).Should().BeTrue();
        tryRes1.Value.Should().Be(validStr);

        Cvu.TryParse(validStr.AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Value.Should().Be(validStr);

        Action invalidParseStr = () => Cvu.Parse("0720000700000001234565", System.Globalization.CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid CVU: '0720000700000001234565'.");

        Action invalidParseSpan = () => Cvu.Parse("0720000700000001234565".AsSpan(), System.Globalization.CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid CVU: '0720000700000001234565'.");

        Cvu.TryParse("invalid", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(Cvu));

        Cvu.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(Cvu));

        Cvu.TryParse("invalid".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(Cvu));
    }

    [Fact]
    public void Cvu_ParseAndTryParse_Utf8()
    {
        byte[] validUtf8 = Encoding.UTF8.GetBytes("0000001700000001234565");
        var parsed = Cvu.Parse(validUtf8, System.Globalization.CultureInfo.InvariantCulture);
        parsed.Value.Should().Be("0000001700000001234565");

        Cvu.TryParse(validUtf8, null, out var tryRes).Should().BeTrue();
        tryRes.Value.Should().Be("0000001700000001234565");

        byte[] invalidUtf8 = Encoding.UTF8.GetBytes("0720000700000001234565");
        Action invalidParseUtf8 = () => Cvu.Parse(invalidUtf8, System.Globalization.CultureInfo.InvariantCulture);
        invalidParseUtf8.Should().Throw<FormatException>().WithMessage("Invalid UTF-8 CVU representation.");

        Cvu.TryParse(invalidUtf8, null, out var tryFail).Should().BeFalse();
        tryFail.Should().Be(default(Cvu));

        byte[] brokenUtf8 = [0xFF, 0xFE, 0xFD];
        Cvu.TryParse(brokenUtf8, null, out var tryBroken).Should().BeFalse();
        tryBroken.Should().Be(default(Cvu));
    }

    [Fact]
    public void Cvu_ComparisonsAndOperators_Exhaustive()
    {
        var a = Cvu.Create("0000000000000000000000").Value;
        var aCopy = Cvu.Create("0000000000000000000000").Value;
        var b = Cvu.Create("0000001700000001234565").Value;

        a.ShouldSatisfyEqualityContract(aCopy, b, (x, y) => x == y, (x, y) => x != y);
        a.ShouldSatisfyComparisonContract(aCopy, b,
            (x, y) => x < y,
            (x, y) => x <= y,
            (x, y) => x > y,
            (x, y) => x >= y);
    }
}




