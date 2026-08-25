// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Colombia;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Colombia.UnitTests;

public sealed class CufeTests
{
    private const string ValidCufe = "69a23075253818e69d7bdf91e12720d206f3fb8d2077e68cf15c2cf0731427509cf6e9e46a782ebfa790d56ee25d0c75";

    [Fact]
    public void Create_Valid96Hex_Succeeds()
    {
        var result = Cufe.Create(ValidCufe);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(ValidCufe);
        result.Value.ToString().Should().Be(ValidCufe);
    }

    [Fact]
    public void Create_Uppercase96Hex_ConvertsToLowercase()
    {
        var upper = ValidCufe.ToUpperInvariant();
        var result = Cufe.Create(upper);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(ValidCufe);
    }

    [Theory]
    [InlineData("69a23075253818e69d7bdf91e12720d206f3fb8d2077e68cf15c2cf0731427509cf6e9e46a782ebfa790d56ee25d0c7")] // 95
    [InlineData("69a23075253818e69d7bdf91e12720d206f3fb8d2077e68cf15c2cf0731427509cf6e9e46a782ebfa790d56ee25d0c75a")] // 97
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_InvalidLength_ReturnsError(string? input)
    {
        var result = Cufe.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Cufe.InvalidLength");
    }

    [Theory]
    [InlineData("69a23075253818e69d7bdf91e12720d206f3fb8d2077e68cf15c2cf0731427509cf6e9e46a782ebfa790d56ee25d0c7Z")]
    [InlineData("69a23075253818e69d7bdf91e12720d206f3fb8d2077e68cf15c2cf0731427509cf6e9e46a782ebfa790d56ee25d0c7#")]
    [InlineData("69a23075253818e69d7bdf91e12720d206f3fb8d2077e68cf15c2cf0731427509cf6e9e46a782ebfa790d56ee25d0c G")]
    public void Create_InvalidCharacters_ReturnsError(string input)
    {
        var result = Cufe.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Cufe.InvalidCharacters");
    }

    [Fact]
    public void Cufe_DefaultState_Equality()
    {
        var cufe1 = Cufe.Create(ValidCufe).Value;
        var cufe2 = Cufe.Create(ValidCufe).Value;
        var cufeDiff = Cufe.Create("000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000").Value;

        (cufe1 == cufe2).Should().BeTrue();
        (cufe1 != cufeDiff).Should().BeTrue();
        cufe1.Equals(cufe2).Should().BeTrue();
        cufe1.Equals((object)cufe2).Should().BeTrue();
        cufe1.Equals(cufeDiff).Should().BeFalse();
        cufe1.GetHashCode().Should().Be(cufe2.GetHashCode());
    }

    [Fact]
    public void Cufe_DefaultState_ParseAndTryParse()
    {
        var parsed1 = Cufe.Parse(ValidCufe, CultureInfo.InvariantCulture);
        parsed1.Value.Should().Be(ValidCufe);

        var parsed2 = Cufe.Parse(ValidCufe.AsSpan(), CultureInfo.InvariantCulture);
        parsed2.Value.Should().Be(ValidCufe);

        Cufe.TryParse(ValidCufe, null, out var tryRes1).Should().BeTrue();
        tryRes1.Value.Should().Be(ValidCufe);

        Cufe.TryParse(ValidCufe.AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Value.Should().Be(ValidCufe);

        Action invalidParseStr = () => Cufe.Parse("invalid", CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid CUFE: 'invalid'.");

        Action invalidParseSpan = () => Cufe.Parse("invalid".AsSpan(), CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid CUFE: 'invalid'.");

        Cufe.TryParse("invalid", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(Cufe));

        Cufe.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(Cufe));

        Cufe.TryParse("invalid".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(Cufe));
    }
}




