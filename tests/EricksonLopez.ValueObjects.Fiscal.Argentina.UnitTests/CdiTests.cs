// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Argentina;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Argentina.UnitTests;

public sealed class CdiTests
{
    [Theory]
    [InlineData("99-12345678-1")]
    [InlineData("99123456781")]
    [InlineData("99.12345678.1")]
    public void Create_Valid11Digits_Succeeds(string input)
    {
        var result = Cdi.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("99123456781");
        result.Value.Formatted.Should().Be("99-12345678-1");
        result.Value.ToString().Should().Be("99-12345678-1");
    }

    [Theory]
    [InlineData("99-12345-1")]      // Too short
    [InlineData("9912345678")]      // Exactly 10 digits
    [InlineData("991234567890")]    // Exactly 12 digits
    [InlineData("99-1234567890-1")]  // Too long
    [InlineData("")]
    [InlineData("   ")]
    public void Create_InvalidLength_ReturnsError(string input)
    {
        var result = Cdi.Create(input);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Cdi.InvalidLength");
    }

    [Theory]
    [InlineData("99-1234567A-1")]
    [InlineData("99-12345678#1")]
    public void Create_InvalidCharacters_ReturnsError(string input)
    {
        var result = Cdi.Create(input);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Cdi.InvalidCharacters");
    }

    [Fact]
    public void Cdi_ParseAndTryParse_StringAndSpan()
    {
        var validStr = "99-12345678-1";
        var parsed1 = Cdi.Parse(validStr, System.Globalization.CultureInfo.InvariantCulture);
        parsed1.Value.Should().Be("99123456781");

        var parsed2 = Cdi.Parse(validStr.AsSpan(), System.Globalization.CultureInfo.InvariantCulture);
        parsed2.Value.Should().Be("99123456781");

        Cdi.TryParse(validStr, null, out var tryRes1).Should().BeTrue();
        tryRes1.Value.Should().Be("99123456781");

        Cdi.TryParse(validStr.AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Value.Should().Be("99123456781");

        Action invalidParseStr = () => Cdi.Parse("invalid", System.Globalization.CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid CDI: 'invalid'.");

        Action invalidParseSpan = () => Cdi.Parse("invalid".AsSpan(), System.Globalization.CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid CDI: 'invalid'.");

        Cdi.TryParse("invalid", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(Cdi));

        Cdi.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(Cdi));

        Cdi.TryParse("invalid".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(Cdi));
    }

    [Fact]
    public void Cdi_ComparisonsAndOperators_Exhaustive()
    {
        var a = Cdi.Create("99-12345678-1").Value;
        var aCopy = Cdi.Create("99-12345678-1").Value;
        var b = Cdi.Create("99-87654321-1").Value;

        a.ShouldSatisfyEqualityContract(aCopy, b, (x, y) => x == y, (x, y) => x != y);
        a.ShouldSatisfyComparisonContract(aCopy, b,
            (x, y) => x < y,
            (x, y) => x <= y,
            (x, y) => x > y,
            (x, y) => x >= y);
    }
}




