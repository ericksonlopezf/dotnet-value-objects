// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Peru;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Peru.UnitTests;

public sealed class DetractionAccountTests
{
    [Theory]
    [InlineData("00051123456")]
    [InlineData("00123456789")]
    [InlineData("  00051123456  ")]
    public void Create_Valid11DigitsStartingWith00_Succeeds(string input)
    {
        var result = DetractionAccount.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.AccountNumber.Should().Be(input.Trim());
        result.Value.ToString().Should().Be(input.Trim());
    }

    [Theory]
    [InlineData("0005112345")]   // 10
    [InlineData("000511234567")] // 12
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_InvalidLength_ReturnsError(string? input)
    {
        var result = DetractionAccount.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("DetractionAccount.InvalidLength");
    }

    [Theory]
    [InlineData("01051123456")]
    [InlineData("10051123456")]
    [InlineData("20051123456")]
    public void Create_InvalidPrefix_ReturnsError(string input)
    {
        var result = DetractionAccount.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("DetractionAccount.InvalidPrefix");
    }

    [Theory]
    [InlineData("0005112345A")]
    [InlineData("00 51123456")]
    [InlineData("00-05112345")]
    public void Create_InvalidCharacters_ReturnsError(string input)
    {
        var result = DetractionAccount.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("DetractionAccount.InvalidCharacters");
    }

    [Fact]
    public void DetractionAccount_DefaultState_ComparisonOperators()
    {
        var a1 = DetractionAccount.Create("00051123456").Value;
        var a2 = DetractionAccount.Create("00051123457").Value;
        var a1Clone = DetractionAccount.Create("00051123456").Value;

        (a1 < a2).Should().BeTrue();
        (a1 <= a2).Should().BeTrue();
        (a2 > a1).Should().BeTrue();
        (a2 >= a1).Should().BeTrue();

        (a1 < a1Clone).Should().BeFalse();
        (a1 > a1Clone).Should().BeFalse();
        (a1 <= a1Clone).Should().BeTrue();
        (a1 >= a1Clone).Should().BeTrue();
        a1.CompareTo(a2).Should().BeNegative();
        a2.CompareTo(a1).Should().BePositive();
        a1.CompareTo(a1Clone).Should().Be(0);
    }

    [Fact]
    public void DetractionAccount_DefaultState_ParseAndTryParse()
    {
        var validStr = "00051123456";
        var parsed1 = DetractionAccount.Parse(validStr, CultureInfo.InvariantCulture);
        parsed1.AccountNumber.Should().Be(validStr);

        var parsed2 = DetractionAccount.Parse(validStr.AsSpan(), CultureInfo.InvariantCulture);
        parsed2.AccountNumber.Should().Be(validStr);

        DetractionAccount.TryParse(validStr, null, out var tryRes1).Should().BeTrue();
        tryRes1.AccountNumber.Should().Be(validStr);

        DetractionAccount.TryParse(validStr.AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.AccountNumber.Should().Be(validStr);

        Action invalidParseStr = () => DetractionAccount.Parse("invalid", CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid DetractionAccount: 'invalid'.");

        Action invalidParseSpan = () => DetractionAccount.Parse("invalid".AsSpan(), CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid DetractionAccount: 'invalid'.");

        DetractionAccount.TryParse("invalid", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(DetractionAccount));

        DetractionAccount.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(DetractionAccount));

        DetractionAccount.TryParse("invalid".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(DetractionAccount));
    }
}




