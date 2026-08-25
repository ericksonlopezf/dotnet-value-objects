// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using System.Text;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Peru;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Peru.UnitTests;

public sealed class RucTests
{
    [Theory]
    [InlineData("20456789014", 20, false, true)]
    [InlineData("10456789019", 10, true, false)]
    [InlineData("15456789011", 15, true, false)] // Check 11 -> returns 1
    [InlineData("20000000010", 20, false, true)] // Check 10 -> returns 0
    [InlineData("17456789013", 17, true, false)]
    [InlineData("  20456789014  ", 20, false, true)]
    public void Create_ValidRuc_ExtractsPrefixAndEntityFlags(
        string input,
        int expectedPrefix,
        bool isNatural,
        bool isLegal)
    {
        var result = Ruc.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(input.Trim());
        result.Value.Prefix.Should().Be(expectedPrefix);
        result.Value.IsNaturalPerson.Should().Be(isNatural);
        result.Value.IsLegalEntity.Should().Be(isLegal);
        result.Value.ToString().Should().Be(input.Trim());
    }

    [Theory]
    [InlineData("2045678901")]    // 10
    [InlineData("204567890145")]  // 12
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_InvalidLength_ReturnsError(string? input)
    {
        var result = Ruc.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Ruc.InvalidLength");
    }

    [Theory]
    [InlineData("2045678901A")]
    [InlineData("20456 89014")]
    [InlineData("20456-89014")]
    [InlineData("ABCDEFGHIJK")]
    public void Create_InvalidCharacters_ReturnsError(string input)
    {
        var result = Ruc.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Ruc.InvalidCharacters");
    }

    [Theory]
    [InlineData("11456789014")]
    [InlineData("21456789014")]
    [InlineData("99456789014")]
    [InlineData("00456789014")]
    public void Create_InvalidPrefix_ReturnsError(string input)
    {
        var result = Ruc.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Ruc.InvalidPrefix");
    }

    [Theory]
    [InlineData("20456789010")]
    [InlineData("20456789019")]
    [InlineData("10456789014")]
    public void Create_InvalidVerificationDigit_ReturnsError(string input)
    {
        var result = Ruc.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Ruc.InvalidVerificationDigit");
    }

    [Fact]
    public void CalculateVerificationDigit_DefaultState_AllBranches()
    {
        Ruc.CalculateVerificationDigit("2000000001".AsSpan()).Should().Be(0); // 10 -> 0
        Ruc.CalculateVerificationDigit("2000000006".AsSpan()).Should().Be(1); // 11 -> 1
        Ruc.CalculateVerificationDigit("2045678901".AsSpan()).Should().Be(4); // check
    }

    [Fact]
    public void Ruc_DefaultState_ComparisonOperators()
    {
        var r1 = Ruc.Create("10456789019").Value;
        var r2 = Ruc.Create("20456789014").Value;
        var r1Clone = Ruc.Create("10456789019").Value;

        (r1 < r2).Should().BeTrue();
        (r1 <= r2).Should().BeTrue();
        (r2 > r1).Should().BeTrue();
        (r2 >= r1).Should().BeTrue();

        (r1 < r1Clone).Should().BeFalse();
        (r1 > r1Clone).Should().BeFalse();
        (r1 <= r1Clone).Should().BeTrue();
        (r1 >= r1Clone).Should().BeTrue();
        r1.CompareTo(r2).Should().BeNegative();
        r2.CompareTo(r1).Should().BePositive();
        r1.CompareTo(r1Clone).Should().Be(0);
    }

    [Fact]
    public void Ruc_ParseAndTryParse_StringAndSpan()
    {
        var validStr = "20456789014";
        var parsed1 = Ruc.Parse(validStr, CultureInfo.InvariantCulture);
        parsed1.Value.Should().Be(validStr);

        var parsed2 = Ruc.Parse(validStr.AsSpan(), CultureInfo.InvariantCulture);
        parsed2.Value.Should().Be(validStr);

        Ruc.TryParse(validStr, null, out var tryRes1).Should().BeTrue();
        tryRes1.Value.Should().Be(validStr);

        Ruc.TryParse(validStr.AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Value.Should().Be(validStr);

        Action invalidParseStr = () => Ruc.Parse("invalid", CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid RUC: 'invalid'.");

        Action invalidParseSpan = () => Ruc.Parse("invalid".AsSpan(), CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid RUC: 'invalid'.");

        Ruc.TryParse("invalid", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(Ruc));

        Ruc.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(Ruc));

        Ruc.TryParse("invalid".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(Ruc));
    }

    [Fact]
    public void Ruc_ParseAndTryParse_Utf8()
    {
        byte[] validUtf8 = Encoding.UTF8.GetBytes("20456789014");
        var parsed = Ruc.Parse(validUtf8, CultureInfo.InvariantCulture);
        parsed.Value.Should().Be("20456789014");

        Ruc.TryParse(validUtf8, null, out var tryRes).Should().BeTrue();
        tryRes.Value.Should().Be("20456789014");

        byte[] invalidUtf8 = Encoding.UTF8.GetBytes("invalid");
        Action invalidParseUtf8 = () => Ruc.Parse(invalidUtf8, CultureInfo.InvariantCulture);
        invalidParseUtf8.Should().Throw<FormatException>().WithMessage("Invalid UTF-8 RUC representation.");

        Ruc.TryParse(invalidUtf8, null, out var tryFail).Should().BeFalse();
        tryFail.Should().Be(default(Ruc));

        byte[] brokenUtf8 = [0xFF, 0xFE, 0xFD];
        Ruc.TryParse(brokenUtf8, null, out var tryBroken).Should().BeFalse();
        tryBroken.Should().Be(default(Ruc));
    }

    [Fact]
    public void Ruc_ComparisonsAndOperators_Exhaustive()
    {
        var a = Ruc.Create("20100070970").Value;
        var aCopy = Ruc.Create("20100070970").Value;
        var b = Ruc.Create("20456789014").Value;

        a.ShouldSatisfyEqualityContract(aCopy, b, (x, y) => x == y, (x, y) => x != y);
        a.ShouldSatisfyComparisonContract(aCopy, b,
            (x, y) => x < y,
            (x, y) => x <= y,
            (x, y) => x > y,
            (x, y) => x >= y);
    }
}




