// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using System.Text;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Colombia;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Colombia.UnitTests;

public sealed class NitTests
{
    [Theory]
    [InlineData(830099999L, (byte)9)]
    [InlineData(800197268L, (byte)4)]
    [InlineData(900254330L, (byte)0)] // Remainder 0 -> DV 0
    [InlineData(900254334L, (byte)1)] // Remainder 1 -> DV 1
    [InlineData(1000000L, (byte)4)]
    [InlineData(999999999999999L, (byte)2)]
    public void Create_FromValidLong_ComputesCorrectDv(long baseNumber, byte expectedDv)
    {
        var result = Nit.Create(baseNumber);

        result.IsSuccess.Should().BeTrue();
        result.Value.BaseNumber.Should().Be(baseNumber);
        result.Value.VerificationDigit.Should().Be(expectedDv);
        result.Value.ToCanonicalString().Should().Be($"{baseNumber.ToString(CultureInfo.InvariantCulture)}-{expectedDv.ToString(CultureInfo.InvariantCulture)}");
        result.Value.ToString().Should().Be(result.Value.ToCanonicalString());
    }

    [Theory]
    [InlineData(0L)]
    [InlineData(-1L)]
    [InlineData(999_999L)]
    [InlineData(1_000_000_000_000_000L)]
    [InlineData(-1000000L)]
    public void Create_FromLongOutOfRange_ReturnsError(long baseNumber)
    {
        var result = Nit.Create(baseNumber);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Nit.OutOfRange");
    }

    [Theory]
    [InlineData("830099999-9", 830099999L, (byte)9)]
    [InlineData("800197268-4", 800197268L, (byte)4)]
    [InlineData("900254330-0", 900254330L, (byte)0)]
    [InlineData("900254334-1", 900254334L, (byte)1)]
    [InlineData("1000000-4", 1000000L, (byte)4)]
    [InlineData("999999999999999-2", 999999999999999L, (byte)2)]
    [InlineData("830099999", 830099999L, (byte)9)]
    [InlineData("1000000", 1000000L, (byte)4)]
    [InlineData("999999999999999", 999999999999999L, (byte)2)]
    [InlineData("  830099999-9  ", 830099999L, (byte)9)]
    public void Create_FromValidString_Succeeds(string input, long expectedBase, byte expectedDv)
    {
        var result = Nit.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.BaseNumber.Should().Be(expectedBase);
        result.Value.VerificationDigit.Should().Be(expectedDv);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_NullOrEmpty_ReturnsRequiredError(string? input)
    {
        var result = Nit.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Nit.Required");
    }

    [Theory]
    [InlineData("83009999A-9")]
    [InlineData("830099999A")]
    [InlineData("830 099 999-9")]
    [InlineData("ABCDEFGH-1")]
    [InlineData("-1")]
    [InlineData("-9")]
    public void Create_InvalidCharacters_ReturnsError(string input)
    {
        var result = Nit.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Nit.InvalidCharacters");
    }

    [Theory]
    [InlineData("999999-1")]
    [InlineData("1000000000000000-1")]
    [InlineData("0-0")]
    public void Create_StringOutOfRange_ReturnsError(string input)
    {
        var result = Nit.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Nit.OutOfRange");
    }

    [Theory]
    [InlineData("830099999-0")]
    [InlineData("830099999-99")]
    [InlineData("830099999-")]
    [InlineData("830099999-A")]
    public void Create_InvalidVerificationDigit_ReturnsError(string input)
    {
        var result = Nit.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Nit.InvalidVerificationDigit");
    }

    [Fact]
    public void CalculateVerificationDigit_DefaultState_AllBranches()
    {
        Nit.CalculateVerificationDigit(900254330L).Should().Be(0); // Remainder 0 -> 0
        Nit.CalculateVerificationDigit(900254334L).Should().Be(1); // Remainder 1 -> 1
        Nit.CalculateVerificationDigit(830099999L).Should().Be(9); // Other remainder
    }

    [Fact]
    public void Nit_DefaultState_ComparisonOperators()
    {
        var nit1 = Nit.Create(800197268L).Value;
        var nit2 = Nit.Create(830099999L).Value;
        var nit1Clone = Nit.Create("800197268-4").Value;

        (nit1 < nit2).Should().BeTrue();
        (nit1 <= nit2).Should().BeTrue();
        (nit2 > nit1).Should().BeTrue();
        (nit2 >= nit1).Should().BeTrue();

        (nit1 < nit1Clone).Should().BeFalse();
        (nit1 > nit1Clone).Should().BeFalse();
        (nit1 <= nit1Clone).Should().BeTrue();
        (nit1 >= nit1Clone).Should().BeTrue();
        nit1.CompareTo(nit2).Should().BeNegative();
        nit2.CompareTo(nit1).Should().BePositive();
        nit1.CompareTo(nit1Clone).Should().Be(0);
    }

    [Fact]
    public void Nit_ParseAndTryParse_StringAndSpan()
    {
        var validStr = "830099999-9";
        var parsed1 = Nit.Parse(validStr, CultureInfo.InvariantCulture);
        parsed1.BaseNumber.Should().Be(830099999L);
        parsed1.VerificationDigit.Should().Be(9);

        var parsed2 = Nit.Parse(validStr.AsSpan(), CultureInfo.InvariantCulture);
        parsed2.BaseNumber.Should().Be(830099999L);

        Nit.TryParse(validStr, null, out var tryRes1).Should().BeTrue();
        tryRes1.BaseNumber.Should().Be(830099999L);

        Nit.TryParse(validStr.AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.BaseNumber.Should().Be(830099999L);

        Action invalidParseStr = () => Nit.Parse("invalid", CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid NIT: 'invalid'.");

        Action invalidParseSpan = () => Nit.Parse("invalid".AsSpan(), CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid NIT: 'invalid'.");

        Nit.TryParse("invalid", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(Nit));

        Nit.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(Nit));

        Nit.TryParse("invalid".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(Nit));
    }

    [Fact]
    public void Nit_ParseAndTryParse_Utf8()
    {
        byte[] validUtf8 = Encoding.UTF8.GetBytes("830099999-9");
        var parsed = Nit.Parse(validUtf8, CultureInfo.InvariantCulture);
        parsed.BaseNumber.Should().Be(830099999L);

        Nit.TryParse(validUtf8, null, out var tryRes).Should().BeTrue();
        tryRes.BaseNumber.Should().Be(830099999L);

        byte[] invalidUtf8 = Encoding.UTF8.GetBytes("invalid");
        Action invalidParseUtf8 = () => Nit.Parse(invalidUtf8, CultureInfo.InvariantCulture);
        invalidParseUtf8.Should().Throw<FormatException>().WithMessage("Invalid UTF-8 NIT representation.");

        Nit.TryParse(invalidUtf8, null, out var tryFail).Should().BeFalse();
        tryFail.Should().Be(default(Nit));

        byte[] brokenUtf8 = [0xFF, 0xFE, 0xFD];
        Nit.TryParse(brokenUtf8, null, out var tryBroken).Should().BeFalse();
        tryBroken.Should().Be(default(Nit));
    }

    [Fact]
    public void Nit_ComparisonsAndOperators_Exhaustive()
    {
        var a = Nit.Create("800197268-4").Value;
        var aCopy = Nit.Create("800197268-4").Value;
        var b = Nit.Create("900123456-8").Value;

        a.ShouldSatisfyEqualityContract(aCopy, b, (x, y) => x == y, (x, y) => x != y);
        a.ShouldSatisfyComparisonContract(aCopy, b,
            (x, y) => x < y,
            (x, y) => x <= y,
            (x, y) => x > y,
            (x, y) => x >= y);
    }
}




