// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using System.Text;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Chile;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Chile.UnitTests;

public sealed class RutTests
{
    [Theory]
    [InlineData(76192083, '9')]
    [InlineData(300, 'K')]
    [InlineData(6, 'K')]
    [InlineData(11000, '0')]
    [InlineData(12345678, '5')]
    [InlineData(11111111, '1')]
    [InlineData(1, '9')]
    [InlineData(99999999, '9')]
    public void Create_FromValidInt_ComputesCorrectDv(int body, char expectedDv)
    {
        var result = Rut.Create(body);

        result.IsSuccess.Should().BeTrue();
        result.Value.Body.Should().Be(body);
        result.Value.Dv.Should().Be(expectedDv);
        result.Value.ToCanonicalString().Should().Be($"{body.ToString(CultureInfo.InvariantCulture)}-{expectedDv}");
        result.Value.ToString().Should().Be(result.Value.ToCanonicalString());
        result.Value.ToFormattedString().Should().Contain($"-{expectedDv}");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(100_000_000)]
    [InlineData(-99999)]
    public void Create_FromIntOutOfRange_ReturnsError(int body)
    {
        var result = Rut.Create(body);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Rut.OutOfRange");
    }

    [Theory]
    [InlineData("76.192.083-9", 76192083, '9')]
    [InlineData("76192083-9", 76192083, '9')]
    [InlineData("761920839", 76192083, '9')]
    [InlineData("300-k", 300, 'K')]
    [InlineData("300k", 300, 'K')]
    [InlineData("300-K", 300, 'K')]
    [InlineData("12.345.678-5", 12345678, '5')]
    [InlineData("11000-0", 11000, '0')]
    [InlineData("6-k", 6, 'K')]
    [InlineData("  76192083-9  ", 76192083, '9')]
    public void Create_FromValidString_Succeeds(string input, int expectedBody, char expectedDv)
    {
        var result = Rut.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Body.Should().Be(expectedBody);
        result.Value.Dv.Should().Be(expectedDv);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_NullOrEmpty_ReturnsRequiredError(string? input)
    {
        var result = Rut.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Rut.Required");
    }

    [Theory]
    [InlineData("76192083-99")]
    [InlineData("76192083-")]
    [InlineData("76192083-123")]
    public void Create_InvalidDvLength_ReturnsError(string input)
    {
        var result = Rut.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Rut.InvalidDv");
    }

    [Theory]
    [InlineData("123456789-5")]
    [InlineData("1234567890-5")]
    [InlineData("123456789K")]
    [InlineData("0-0")]
    [InlineData("00000000-0")]
    public void Create_BodyExceeds8DigitsOrZero_ReturnsError(string input)
    {
        var result = Rut.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Rut.OutOfRange");
    }

    [Theory]
    [InlineData("76A92083-9")]
    [InlineData("76 192 083-9")]
    [InlineData("76#192#083-9")]
    public void Create_InvalidCharacters_ReturnsError(string input)
    {
        var result = Rut.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Rut.InvalidCharacters");
    }

    [Theory]
    [InlineData("-K")]
    [InlineData(".-K")]
    [InlineData("...-K")]
    [InlineData("K")]
    [InlineData("k")]
    [InlineData(".")]
    public void Create_InvalidBody_ReturnsError(string input)
    {
        var result = Rut.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Rut.InvalidBody");
    }

    [Theory]
    [InlineData("76.192.083-0")]
    [InlineData("300-1")]
    [InlineData("12345678-4")]
    [InlineData("11000-1")]
    [InlineData("6-0")]
    public void Create_InvalidVerificationDigit_ReturnsError(string input)
    {
        var result = Rut.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Rut.InvalidVerificationDigit");
    }

    [Fact]
    public void CalculateVerificationDigit_DefaultState_AllBranches()
    {
        Rut.CalculateVerificationDigit(11000).Should().Be('0'); // Remainder 0 -> 11 -> '0'
        Rut.CalculateVerificationDigit(6).Should().Be('K');     // Remainder 1 -> 10 -> 'K'
        Rut.CalculateVerificationDigit(76192083).Should().Be('9');
    }

    [Fact]
    public void Rut_ToFormattedString_FormattedCorrectly()
    {
        var rut = Rut.Create(12345678).Value;
        rut.ToFormattedString().Should().Be("12.345.678-5");

        var rutShort = Rut.Create(300).Value;
        rutShort.ToFormattedString().Should().Be("300-K");
    }

    [Fact]
    public void Rut_DefaultState_ComparisonOperators()
    {
        var rut1 = Rut.Create(300).Value;
        var rut2 = Rut.Create(76192083).Value;
        var rut1Clone = Rut.Create("300-K").Value;

        (rut1 < rut2).Should().BeTrue();
        (rut1 <= rut2).Should().BeTrue();
        (rut2 > rut1).Should().BeTrue();
        (rut2 >= rut1).Should().BeTrue();

        (rut1 < rut1Clone).Should().BeFalse();
        (rut1 > rut1Clone).Should().BeFalse();
        (rut1 <= rut1Clone).Should().BeTrue();
        (rut1 >= rut1Clone).Should().BeTrue();
        rut1.CompareTo(rut2).Should().BeNegative();
        rut2.CompareTo(rut1).Should().BePositive();
        rut1.CompareTo(rut1Clone).Should().Be(0);
    }

    [Fact]
    public void Rut_ParseAndTryParse_StringAndSpan()
    {
        var validStr = "76192083-9";
        var parsed1 = Rut.Parse(validStr, CultureInfo.InvariantCulture);
        parsed1.Body.Should().Be(76192083);
        parsed1.Dv.Should().Be('9');

        var parsed2 = Rut.Parse(validStr.AsSpan(), CultureInfo.InvariantCulture);
        parsed2.Body.Should().Be(76192083);

        Rut.TryParse(validStr, null, out var tryRes1).Should().BeTrue();
        tryRes1.Body.Should().Be(76192083);

        Rut.TryParse(validStr.AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Body.Should().Be(76192083);

        Action invalidParseStr = () => Rut.Parse("invalid", CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid RUT: 'invalid'.");

        Action invalidParseSpan = () => Rut.Parse("invalid".AsSpan(), CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid RUT: 'invalid'.");

        Rut.TryParse("invalid", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(Rut));

        Rut.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(Rut));

        Rut.TryParse("invalid".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(Rut));
    }

    [Fact]
    public void Rut_ParseAndTryParse_Utf8()
    {
        byte[] validUtf8 = Encoding.UTF8.GetBytes("76192083-9");
        var parsed = Rut.Parse(validUtf8, CultureInfo.InvariantCulture);
        parsed.Body.Should().Be(76192083);

        Rut.TryParse(validUtf8, null, out var tryRes).Should().BeTrue();
        tryRes.Body.Should().Be(76192083);

        byte[] invalidUtf8 = Encoding.UTF8.GetBytes("invalid");
        Action invalidParseUtf8 = () => Rut.Parse(invalidUtf8, CultureInfo.InvariantCulture);
        invalidParseUtf8.Should().Throw<FormatException>().WithMessage("Invalid UTF-8 RUT representation.");

        Rut.TryParse(invalidUtf8, null, out var tryFail).Should().BeFalse();
        tryFail.Should().Be(default(Rut));

        byte[] brokenUtf8 = [0xFF, 0xFE, 0xFD];
        Rut.TryParse(brokenUtf8, null, out var tryBroken).Should().BeFalse();
        tryBroken.Should().Be(default(Rut));
    }

    [Fact]
    public void Rut_ComparisonsAndOperators_Exhaustive()
    {
        var a = Rut.Create(12345678).Value;
        var aCopy = Rut.Create(12345678).Value;
        var b = Rut.Create(76192083).Value;

        a.ShouldSatisfyEqualityContract(aCopy, b, (x, y) => x == y, (x, y) => x != y);
        a.ShouldSatisfyComparisonContract(aCopy, b,
            (x, y) => x < y,
            (x, y) => x <= y,
            (x, y) => x > y,
            (x, y) => x >= y);
    }
}




