// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Colombia;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Colombia.UnitTests;

public sealed class CiiuCodeTests
{
    [Theory]
    [InlineData("6201")]
    [InlineData("0111")]
    [InlineData("9900")]
    [InlineData("  6201  ")]
    public void Create_Valid4Digits_Succeeds(string input)
    {
        var result = CiiuCode.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Code.Should().Be("6201".Trim() == input.Trim() ? input.Trim() : input.Trim());
        result.Value.ToString().Should().Be(input.Trim());
    }

    [Theory]
    [InlineData("620")]
    [InlineData("62011")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_InvalidLength_ReturnsError(string? input)
    {
        var result = CiiuCode.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("CiiuCode.InvalidLength");
    }

    [Theory]
    [InlineData("620A")]
    [InlineData("62 1")]
    [InlineData("ABCD")]
    [InlineData("62-1")]
    public void Create_InvalidCharacters_ReturnsError(string input)
    {
        var result = CiiuCode.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("CiiuCode.InvalidCharacters");
    }

    [Fact]
    public void CiiuCode_DefaultState_Equality()
    {
        var ciiu1 = CiiuCode.Create("6201").Value;
        var ciiu2 = CiiuCode.Create("6201").Value;
        var ciiuDiff = CiiuCode.Create("0111").Value;

        (ciiu1 == ciiu2).Should().BeTrue();
        (ciiu1 != ciiuDiff).Should().BeTrue();
        ciiu1.Equals(ciiu2).Should().BeTrue();
        ciiu1.Equals((object)ciiu2).Should().BeTrue();
        ciiu1.Equals(ciiuDiff).Should().BeFalse();
        ciiu1.GetHashCode().Should().Be(ciiu2.GetHashCode());
    }

    [Fact]
    public void CiiuCode_DefaultState_ParseAndTryParse()
    {
        var parsed1 = CiiuCode.Parse("6201", CultureInfo.InvariantCulture);
        parsed1.Code.Should().Be("6201");

        var parsed2 = CiiuCode.Parse("6201".AsSpan(), CultureInfo.InvariantCulture);
        parsed2.Code.Should().Be("6201");

        CiiuCode.TryParse("6201", null, out var tryRes1).Should().BeTrue();
        tryRes1.Code.Should().Be("6201");

        CiiuCode.TryParse("6201".AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Code.Should().Be("6201");

        Action invalidParseStr = () => CiiuCode.Parse("invalid", CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid CIIU code: 'invalid'.");

        Action invalidParseSpan = () => CiiuCode.Parse("invalid".AsSpan(), CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid CIIU code: 'invalid'.");

        CiiuCode.TryParse("invalid", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(CiiuCode));

        CiiuCode.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(CiiuCode));

        CiiuCode.TryParse("invalid".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(CiiuCode));
    }
}




