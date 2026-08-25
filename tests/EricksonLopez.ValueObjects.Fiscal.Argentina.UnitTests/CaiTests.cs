// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Argentina;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Argentina.UnitTests;

public sealed class CaiTests
{
    [Fact]
    public void Create_Valid14Digits_Succeeds()
    {
        var exp = new DateOnly(2026, 12, 31);
        var cai = Cai.Create("12345678901234", exp).Value;

        cai.Code.Should().Be("12345678901234");
        cai.ExpirationDate.Should().Be(exp);
        cai.IsExpired(new DateOnly(2026, 6, 1)).Should().BeFalse();
        cai.IsExpired(new DateOnly(2026, 12, 31)).Should().BeFalse();
        cai.IsExpired(new DateOnly(2027, 1, 1)).Should().BeTrue();
        cai.ToString().Should().Be("12345678901234 (Vto: 2026-12-31)");
    }

    [Theory]
    [InlineData("1234567890123")]   // 13 digits
    [InlineData("123456789012345")] // 15 digits
    [InlineData("")]
    [InlineData("   ")]
    public void Create_InvalidLength_ReturnsError(string input)
    {
        var exp = new DateOnly(2026, 12, 31);
        var result = Cai.Create(input, exp);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Cai.InvalidLength");
    }

    [Fact]
    public void Create_InvalidCharacters_ReturnsError()
    {
        var exp = new DateOnly(2026, 12, 31);
        var result = Cai.Create("1234567890123A", exp);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Cai.InvalidCharacters");
    }

    [Fact]
    public void Cai_DefaultState_ParseAndTryParse()
    {
        var validStr = "12345678901234";
        var parsed1 = Cai.Parse(validStr, System.Globalization.CultureInfo.InvariantCulture);
        parsed1.Code.Should().Be("12345678901234");
        parsed1.ExpirationDate.Should().Be(DateOnly.MaxValue);

        var parsed2 = Cai.Parse(validStr.AsSpan(), System.Globalization.CultureInfo.InvariantCulture);
        parsed2.Code.Should().Be("12345678901234");

        Cai.TryParse(validStr, null, out var tryRes1).Should().BeTrue();
        tryRes1.Code.Should().Be("12345678901234");

        Cai.TryParse(validStr.AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Code.Should().Be("12345678901234");

        Action invalidParseStr = () => Cai.Parse("invalid", System.Globalization.CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid CAI: 'invalid'.");

        Action invalidParseSpan = () => Cai.Parse("invalid".AsSpan(), System.Globalization.CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid CAI: 'invalid'.");

        Cai.TryParse("invalid", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(Cai));

        Cai.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(Cai));

        Cai.TryParse("invalid".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(Cai));
    }

    [Fact]
    public void Cai_Equality_EvaluatesCorrectly()
    {
        var exp = new DateOnly(2026, 12, 31);
        var a = Cai.Create("12345678901234", exp).Value;
        var aCopy = Cai.Create("12345678901234", exp).Value;
        var b = Cai.Create("99999999999999", exp).Value;

        (a == aCopy).Should().BeTrue();
        (a != b).Should().BeTrue();
    }
}




