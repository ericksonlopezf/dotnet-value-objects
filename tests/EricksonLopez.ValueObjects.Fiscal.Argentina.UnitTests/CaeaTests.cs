// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Argentina;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Argentina.UnitTests;

public sealed class CaeaTests
{
    [Fact]
    public void Create_Valid14Digits_Succeeds()
    {
        var exp = new DateOnly(2026, 12, 31);
        var caea = Caea.Create("12345678901234", exp).Value;

        caea.Code.Should().Be("12345678901234");
        caea.ExpirationDate.Should().Be(exp);
        caea.IsExpired(new DateOnly(2026, 6, 1)).Should().BeFalse();
        caea.IsExpired(new DateOnly(2026, 12, 31)).Should().BeFalse();
        caea.IsExpired(new DateOnly(2027, 1, 1)).Should().BeTrue();
        caea.ToString().Should().Be("12345678901234 (Vto: 2026-12-31)");
    }

    [Theory]
    [InlineData("1234567890123")]   // 13 digits
    [InlineData("123456789012345")] // 15 digits
    [InlineData("")]
    [InlineData("   ")]
    public void Create_InvalidLength_ReturnsError(string input)
    {
        var exp = new DateOnly(2026, 12, 31);
        var result = Caea.Create(input, exp);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Caea.InvalidLength");
    }

    [Fact]
    public void Create_InvalidCharacters_ReturnsError()
    {
        var exp = new DateOnly(2026, 12, 31);
        var result = Caea.Create("1234567890123A", exp);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Caea.InvalidCharacters");
    }

    [Fact]
    public void Caea_DefaultState_ParseAndTryParse()
    {
        var validStr = "12345678901234";
        var parsed1 = Caea.Parse(validStr, System.Globalization.CultureInfo.InvariantCulture);
        parsed1.Code.Should().Be("12345678901234");
        parsed1.ExpirationDate.Should().Be(DateOnly.MaxValue);

        var parsed2 = Caea.Parse(validStr.AsSpan(), System.Globalization.CultureInfo.InvariantCulture);
        parsed2.Code.Should().Be("12345678901234");

        Caea.TryParse(validStr, null, out var tryRes1).Should().BeTrue();
        tryRes1.Code.Should().Be("12345678901234");

        Caea.TryParse(validStr.AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Code.Should().Be("12345678901234");

        Action invalidParseStr = () => Caea.Parse("invalid", System.Globalization.CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid CAEA: 'invalid'.");

        Action invalidParseSpan = () => Caea.Parse("invalid".AsSpan(), System.Globalization.CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid CAEA: 'invalid'.");

        Caea.TryParse("invalid", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(Caea));

        Caea.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(Caea));

        Caea.TryParse("invalid".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(Caea));
    }

    [Fact]
    public void Caea_Equality_EvaluatesCorrectly()
    {
        var exp = new DateOnly(2026, 12, 31);
        var a = Caea.Create("12345678901234", exp).Value;
        var aCopy = Caea.Create("12345678901234", exp).Value;
        var b = Caea.Create("99999999999999", exp).Value;

        (a == aCopy).Should().BeTrue();
        (a != b).Should().BeTrue();
    }
}




