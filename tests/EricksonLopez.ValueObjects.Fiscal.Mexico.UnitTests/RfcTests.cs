// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using System.Text;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Mexico;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Mexico.UnitTests;

public sealed class RfcTests
{
    [Theory]
    [InlineData("ABC680524P76", true, false, false, false)]
    [InlineData("Ñ&A680524P76", true, false, false, false)]
    [InlineData("abc680524p76", true, false, false, false)]
    [InlineData("GODE561231GR8", false, true, false, false)]
    [InlineData("XAXX010101000", false, true, true, false)]
    [InlineData("XEXX010101000", false, true, false, true)]
    [InlineData("  GODE561231GR8  ", false, true, false, false)]
    public void Create_ValidRfc_ExtractsAllProperties(
        string input,
        bool isCompany,
        bool isIndividual,
        bool isGenericNational,
        bool isGenericForeigner)
    {
        var result = Rfc.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(input.Trim().ToUpperInvariant());
        result.Value.IsCompany.Should().Be(isCompany);
        result.Value.IsIndividual.Should().Be(isIndividual);
        result.Value.IsGenericNational.Should().Be(isGenericNational);
        result.Value.IsGenericForeigner.Should().Be(isGenericForeigner);
        result.Value.ToString().Should().Be(input.Trim().ToUpperInvariant());
    }

    [Theory]
    [InlineData("ABC680524P7")]    // 11
    [InlineData("GODE561231GR89")]  // 14
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_InvalidLength_ReturnsError(string? input)
    {
        var result = Rfc.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Rfc.InvalidLength");
    }

    [Theory]
    [InlineData("ABC680524P#6")]
    [InlineData("ABC 680524P76")]
    [InlineData("GODE561231GR!")]
    public void Create_InvalidCharacters_ReturnsError(string input)
    {
        var result = Rfc.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Rfc.InvalidCharacters");
    }

    [Theory]
    [InlineData("1BC680524P76")]
    [InlineData("A1C680524P76")]
    [InlineData("AB1680524P76")]
    [InlineData("1234567890123")]
    [InlineData("G1DE561231GR8")]
    public void Create_InvalidInitialLetters_ReturnsError(string input)
    {
        var result = Rfc.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Rfc.InvalidInitialLetters");
    }

    [Theory]
    [InlineData("ABC680A24P76")]
    [InlineData("ABC68052AP76")]
    [InlineData("GODE561B31GR8")]
    [InlineData("GODEA61231GR8")]
    public void Create_InvalidDateDigits_ReturnsError(string input)
    {
        var result = Rfc.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Rfc.InvalidDateDigits");
    }

    [Fact]
    public void Rfc_DefaultState_ComparisonOperators()
    {
        var rfc1 = Rfc.Create("ABC680524P76").Value;
        var rfc2 = Rfc.Create("GODE561231GR8").Value;
        var rfc1Clone = Rfc.Create("abc680524p76").Value;

        (rfc1 < rfc2).Should().BeTrue();
        (rfc1 <= rfc2).Should().BeTrue();
        (rfc2 > rfc1).Should().BeTrue();
        (rfc2 >= rfc1).Should().BeTrue();

        (rfc1 < rfc1Clone).Should().BeFalse();
        (rfc1 > rfc1Clone).Should().BeFalse();
        (rfc1 <= rfc1Clone).Should().BeTrue();
        (rfc1 >= rfc1Clone).Should().BeTrue();
        rfc1.CompareTo(rfc2).Should().BeNegative();
        rfc2.CompareTo(rfc1).Should().BePositive();
        rfc1.CompareTo(rfc1Clone).Should().Be(0);
    }

    [Fact]
    public void Rfc_ParseAndTryParse_StringAndSpan()
    {
        var validStr = "ABC680524P76";
        var parsed1 = Rfc.Parse(validStr, CultureInfo.InvariantCulture);
        parsed1.Value.Should().Be(validStr);

        var parsed2 = Rfc.Parse(validStr.AsSpan(), CultureInfo.InvariantCulture);
        parsed2.Value.Should().Be(validStr);

        Rfc.TryParse(validStr, null, out var tryRes1).Should().BeTrue();
        tryRes1.Value.Should().Be(validStr);

        Rfc.TryParse(validStr.AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Value.Should().Be(validStr);

        Action invalidParseStr = () => Rfc.Parse("invalid", CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid RFC: 'invalid'.");

        Action invalidParseSpan = () => Rfc.Parse("invalid".AsSpan(), CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid RFC: 'invalid'.");

        Rfc.TryParse("invalid", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(Rfc));

        Rfc.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(Rfc));

        Rfc.TryParse("invalid".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(Rfc));
    }

    [Fact]
    public void Rfc_ParseAndTryParse_Utf8()
    {
        byte[] validUtf8 = Encoding.UTF8.GetBytes("ABC680524P76");
        var parsed = Rfc.Parse(validUtf8, CultureInfo.InvariantCulture);
        parsed.Value.Should().Be("ABC680524P76");

        Rfc.TryParse(validUtf8, null, out var tryRes).Should().BeTrue();
        tryRes.Value.Should().Be("ABC680524P76");

        byte[] invalidUtf8 = Encoding.UTF8.GetBytes("invalid");
        Action invalidParseUtf8 = () => Rfc.Parse(invalidUtf8, CultureInfo.InvariantCulture);
        invalidParseUtf8.Should().Throw<FormatException>().WithMessage("Invalid UTF-8 RFC representation.");

        Rfc.TryParse(invalidUtf8, null, out var tryFail).Should().BeFalse();
        tryFail.Should().Be(default(Rfc));

        byte[] brokenUtf8 = [0xFF, 0xFE, 0xFD];
        Rfc.TryParse(brokenUtf8, null, out var tryBroken).Should().BeFalse();
        tryBroken.Should().Be(default(Rfc));
    }

    [Fact]
    public void Rfc_ComparisonsAndOperators_Exhaustive()
    {
        var a = Rfc.Create("ABC680524P76").Value;
        var aCopy = Rfc.Create("ABC680524P76").Value;
        var b = Rfc.Create("XAXX010101000").Value;

        a.ShouldSatisfyEqualityContract(aCopy, b, (x, y) => x == y, (x, y) => x != y);
        a.ShouldSatisfyComparisonContract(aCopy, b,
            (x, y) => x < y,
            (x, y) => x <= y,
            (x, y) => x > y,
            (x, y) => x >= y);
    }
}




