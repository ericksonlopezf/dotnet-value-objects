// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using System.Text;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Argentina;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Argentina.UnitTests;

public sealed class CuitTests
{
    [Theory]
    [InlineData("20-12345678-6", 20, "12345678", 6, true, false)]
    [InlineData("27-23456789-1", 27, "23456789", 1, true, false)]
    [InlineData("30-65432109-0", 30, "65432109", 0, false, true)]
    [InlineData("20123456786", 20, "12345678", 6, true, false)]
    [InlineData("23-12345678-5", 23, "12345678", 5, true, false)]
    [InlineData("24-12345678-1", 24, "12345678", 1, true, false)]
    [InlineData("33-65432109-9", 33, "65432109", 9, false, true)]
    [InlineData("34-65432109-6", 34, "65432109", 6, false, true)]
    [InlineData("20-00000001-9", 20, "00000001", 9, true, false)] // Remainder 1 -> DV 9
    public void Create_ValidCuit_ExtractsAllProperties(
        string input,
        int expectedPrefix,
        string expectedDoc,
        int expectedDv,
        bool isIndividual,
        bool isCompany)
    {
        var result = Cuit.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.TypePrefix.Should().Be(expectedPrefix);
        result.Value.DocumentNumber.Should().Be(expectedDoc);
        result.Value.VerificationDigit.Should().Be(expectedDv);
        result.Value.IsIndividual.Should().Be(isIndividual);
        result.Value.IsCompany.Should().Be(isCompany);
        result.Value.Formatted.Should().Be($"{expectedPrefix}-{expectedDoc}-{expectedDv}");
        result.Value.ToString().Should().Be($"{expectedPrefix}-{expectedDoc}-{expectedDv}");
    }

    [Fact]
    public void CalculateVerificationDigit_DefaultState_RemainderCases()
    {
        // Remainder 0 -> 0
        Cuit.CalculateVerificationDigit("3065432109".AsSpan()).Should().Be(0);

        // Remainder 1 -> 9
        Cuit.CalculateVerificationDigit("2000000001".AsSpan()).Should().Be(9);
    }

    [Theory]
    [InlineData("20-12345678-0")] // Wrong DV
    [InlineData("20-12345678-5")] // Wrong DV
    public void Create_InvalidVerificationDigit_ReturnsError(string input)
    {
        var result = Cuit.Create(input);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Cuit.InvalidVerificationDigit");
    }

    [Theory]
    [InlineData("99-12345678-1")] // Invalid prefix 99
    [InlineData("21-12345678-1")] // Invalid prefix 21
    public void Create_InvalidPrefix_ReturnsError(string input)
    {
        var result = Cuit.Create(input);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Cuit.InvalidPrefix");
    }

    [Theory]
    [InlineData("20-12345-6")]      // Too short
    [InlineData("2012345678")]      // Exactly 10 digits (hits count != 11 after loop)
    [InlineData("201234567890")]    // Exactly 12 digits (hits count >= 11 inside loop)
    [InlineData("20-1234567890-6")] // Too long
    [InlineData("")]
    [InlineData("   ")]
    public void Create_InvalidLength_ReturnsError(string input)
    {
        var result = Cuit.Create(input);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Cuit.InvalidLength");
    }

    [Theory]
    [InlineData("20-1234567A-6")]
    [InlineData("20-12345678#6")]
    [InlineData("20 12345678 6")]
    public void Create_InvalidCharacters_ReturnsError(string input)
    {
        var result = Cuit.Create(input);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Cuit.InvalidCharacters");
    }

    [Fact]
    public void Cuit_ParseAndTryParse_StringAndSpan()
    {
        var validStr = "20-12345678-6";
        var parsed1 = Cuit.Parse(validStr, CultureInfo.InvariantCulture);
        parsed1.Value.Should().Be("20123456786");

        var parsed2 = Cuit.Parse(validStr.AsSpan(), CultureInfo.InvariantCulture);
        parsed2.Value.Should().Be("20123456786");

        Cuit.TryParse(validStr, null, out var tryRes1).Should().BeTrue();
        tryRes1.Value.Should().Be("20123456786");

        Cuit.TryParse(validStr.AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Value.Should().Be("20123456786");

        Action invalidParseStr = () => Cuit.Parse("invalid", CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid CUIT: 'invalid'.");

        Action invalidParseSpan = () => Cuit.Parse("invalid".AsSpan(), CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid CUIT: 'invalid'.");

        Cuit.TryParse("invalid", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(Cuit));

        Cuit.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(Cuit));

        Cuit.TryParse("invalid".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(Cuit));
    }

    [Fact]
    public void Cuit_ParseAndTryParse_Utf8()
    {
        byte[] validUtf8 = Encoding.UTF8.GetBytes("20-12345678-6");
        var parsed = Cuit.Parse(validUtf8, CultureInfo.InvariantCulture);
        parsed.Value.Should().Be("20123456786");

        Cuit.TryParse(validUtf8, null, out var tryRes).Should().BeTrue();
        tryRes.Value.Should().Be("20123456786");

        byte[] invalidUtf8 = Encoding.UTF8.GetBytes("invalid");
        Action invalidParseUtf8 = () => Cuit.Parse(invalidUtf8, CultureInfo.InvariantCulture);
        invalidParseUtf8.Should().Throw<FormatException>().WithMessage("Invalid UTF-8 CUIT representation.");

        Cuit.TryParse(invalidUtf8, null, out var tryFail).Should().BeFalse();
        tryFail.Should().Be(default(Cuit));

        byte[] brokenUtf8 = [0xFF, 0xFE, 0xFD];
        Cuit.TryParse(brokenUtf8, null, out var tryBroken).Should().BeFalse();
        tryBroken.Should().Be(default(Cuit));
    }

    [Fact]
    public void Cuit_ComparisonsAndOperators_Exhaustive()
    {
        var a = Cuit.Create("20-12345678-6").Value;
        var aCopy = Cuit.Create("20-12345678-6").Value;
        var b = Cuit.Create("27-23456789-1").Value;

        a.ShouldSatisfyEqualityContract(aCopy, b, (x, y) => x == y, (x, y) => x != y);
        a.ShouldSatisfyComparisonContract(aCopy, b,
            (x, y) => x < y,
            (x, y) => x <= y,
            (x, y) => x > y,
            (x, y) => x >= y);
    }
}




