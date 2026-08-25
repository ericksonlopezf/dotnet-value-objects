// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Colombia;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Colombia.UnitTests;

public sealed class CudeTests
{
    private const string ValidCude = "69a23075253818e69d7bdf91e12720d206f3fb8d2077e68cf15c2cf0731427509cf6e9e46a782ebfa790d56ee25d0c75";

    [Fact]
    public void Create_Valid96Hex_Succeeds()
    {
        var result = Cude.Create(ValidCude);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(ValidCude);
        result.Value.ToString().Should().Be(ValidCude);
    }

    [Fact]
    public void Create_Uppercase96Hex_ConvertsToLowercase()
    {
        var upper = ValidCude.ToUpperInvariant();
        var result = Cude.Create(upper);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(ValidCude);
    }

    [Theory]
    [InlineData("69a23075253818e69d7bdf91e12720d206f3fb8d2077e68cf15c2cf0731427509cf6e9e46a782ebfa790d56ee25d0c7")] // 95
    [InlineData("69a23075253818e69d7bdf91e12720d206f3fb8d2077e68cf15c2cf0731427509cf6e9e46a782ebfa790d56ee25d0c75a")] // 97
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_InvalidLength_ReturnsError(string? input)
    {
        var result = Cude.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Cude.InvalidLength");
    }

    [Theory]
    [InlineData("69a23075253818e69d7bdf91e12720d206f3fb8d2077e68cf15c2cf0731427509cf6e9e46a782ebfa790d56ee25d0c7Z")]
    [InlineData("69a23075253818e69d7bdf91e12720d206f3fb8d2077e68cf15c2cf0731427509cf6e9e46a782ebfa790d56ee25d0c7#")]
    [InlineData("69a23075253818e69d7bdf91e12720d206f3fb8d2077e68cf15c2cf0731427509cf6e9e46a782ebfa790d56ee25d0c G")]
    public void Create_InvalidCharacters_ReturnsError(string input)
    {
        var result = Cude.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Cude.InvalidCharacters");
    }

    [Fact]
    public void Cude_DefaultState_Equality()
    {
        var cude1 = Cude.Create(ValidCude).Value;
        var cude2 = Cude.Create(ValidCude).Value;
        var cudeDiff = Cude.Create("000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000").Value;

        (cude1 == cude2).Should().BeTrue();
        (cude1 != cudeDiff).Should().BeTrue();
        cude1.Equals(cude2).Should().BeTrue();
        cude1.Equals((object)cude2).Should().BeTrue();
        cude1.Equals(cudeDiff).Should().BeFalse();
        cude1.GetHashCode().Should().Be(cude2.GetHashCode());
    }

    [Fact]
    public void Cude_DefaultState_ParseAndTryParse()
    {
        var parsed1 = Cude.Parse(ValidCude, CultureInfo.InvariantCulture);
        parsed1.Value.Should().Be(ValidCude);

        var parsed2 = Cude.Parse(ValidCude.AsSpan(), CultureInfo.InvariantCulture);
        parsed2.Value.Should().Be(ValidCude);

        Cude.TryParse(ValidCude, null, out var tryRes1).Should().BeTrue();
        tryRes1.Value.Should().Be(ValidCude);

        Cude.TryParse(ValidCude.AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Value.Should().Be(ValidCude);

        Action invalidParseStr = () => Cude.Parse("invalid", CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid CUDE: 'invalid'.");

        Action invalidParseSpan = () => Cude.Parse("invalid".AsSpan(), CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid CUDE: 'invalid'.");

        Cude.TryParse("invalid", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(Cude));

        Cude.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(Cude));

        Cude.TryParse("invalid".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(Cude));
    }
}




