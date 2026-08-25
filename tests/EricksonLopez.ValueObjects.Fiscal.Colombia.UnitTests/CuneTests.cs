// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Colombia;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Colombia.UnitTests;

public sealed class CuneTests
{
    private const string ValidCune = "69a23075253818e69d7bdf91e12720d206f3fb8d2077e68cf15c2cf0731427509cf6e9e46a782ebfa790d56ee25d0c75";

    [Fact]
    public void Create_Valid96Hex_Succeeds()
    {
        var result = Cune.Create(ValidCune);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(ValidCune);
        result.Value.ToString().Should().Be(ValidCune);
    }

    [Fact]
    public void Create_Uppercase96Hex_ConvertsToLowercase()
    {
        var upper = ValidCune.ToUpperInvariant();
        var result = Cune.Create(upper);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(ValidCune);
    }

    [Theory]
    [InlineData("69a23075253818e69d7bdf91e12720d206f3fb8d2077e68cf15c2cf0731427509cf6e9e46a782ebfa790d56ee25d0c7")] // 95
    [InlineData("69a23075253818e69d7bdf91e12720d206f3fb8d2077e68cf15c2cf0731427509cf6e9e46a782ebfa790d56ee25d0c75a")] // 97
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_InvalidLength_ReturnsError(string? input)
    {
        var result = Cune.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Cune.InvalidLength");
    }

    [Theory]
    [InlineData("69a23075253818e69d7bdf91e12720d206f3fb8d2077e68cf15c2cf0731427509cf6e9e46a782ebfa790d56ee25d0c7Z")]
    [InlineData("69a23075253818e69d7bdf91e12720d206f3fb8d2077e68cf15c2cf0731427509cf6e9e46a782ebfa790d56ee25d0c7#")]
    [InlineData("69a23075253818e69d7bdf91e12720d206f3fb8d2077e68cf15c2cf0731427509cf6e9e46a782ebfa790d56ee25d0c G")]
    public void Create_InvalidCharacters_ReturnsError(string input)
    {
        var result = Cune.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Cune.InvalidCharacters");
    }

    [Fact]
    public void Cune_DefaultState_Equality()
    {
        var cune1 = Cune.Create(ValidCune).Value;
        var cune2 = Cune.Create(ValidCune).Value;
        var cuneDiff = Cune.Create("000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000").Value;

        (cune1 == cune2).Should().BeTrue();
        (cune1 != cuneDiff).Should().BeTrue();
        cune1.Equals(cune2).Should().BeTrue();
        cune1.Equals((object)cune2).Should().BeTrue();
        cune1.Equals(cuneDiff).Should().BeFalse();
        cune1.GetHashCode().Should().Be(cune2.GetHashCode());
    }

    [Fact]
    public void Cune_DefaultState_ParseAndTryParse()
    {
        var parsed1 = Cune.Parse(ValidCune, CultureInfo.InvariantCulture);
        parsed1.Value.Should().Be(ValidCune);

        var parsed2 = Cune.Parse(ValidCune.AsSpan(), CultureInfo.InvariantCulture);
        parsed2.Value.Should().Be(ValidCune);

        Cune.TryParse(ValidCune, null, out var tryRes1).Should().BeTrue();
        tryRes1.Value.Should().Be(ValidCune);

        Cune.TryParse(ValidCune.AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Value.Should().Be(ValidCune);

        Action invalidParseStr = () => Cune.Parse("invalid", CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid CUNE: 'invalid'.");

        Action invalidParseSpan = () => Cune.Parse("invalid".AsSpan(), CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid CUNE: 'invalid'.");

        Cune.TryParse("invalid", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(Cune));

        Cune.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(Cune));

        Cune.TryParse("invalid".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(Cune));
    }
}




