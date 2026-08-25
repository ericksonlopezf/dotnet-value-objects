// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Mexico;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Mexico.UnitTests;

public sealed class CurpTests
{
    [Theory]
    [InlineData("GODE561231HDFRRN08", 'H', "DF")]
    [InlineData("gode561231hdfrrn08", 'H', "DF")]
    [InlineData("MAMR800101MDFRRN01", 'M', "DF")]
    [InlineData("  GODE561231HDFRRN08  ", 'H', "DF")]
    public void Create_ValidCurp_ExtractsGenderAndStateCode(string input, char expectedGender, string expectedState)
    {
        var result = Curp.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(input.Trim().ToUpperInvariant());
        result.Value.Gender.Should().Be(expectedGender);
        result.Value.StateCode.Should().Be(expectedState);
        result.Value.ToString().Should().Be(input.Trim().ToUpperInvariant());
    }

    [Theory]
    [InlineData("GODE561231HDFRRN0")]   // 17
    [InlineData("GODE561231HDFRRN089")] // 19
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_InvalidLength_ReturnsError(string? input)
    {
        var result = Curp.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Curp.InvalidLength");
    }

    [Theory]
    [InlineData("GODE561231HDFRRN0#")]
    [InlineData("GODE 561231HDFRRN0")]
    [InlineData("GODE561231HDFRRN0!")]
    public void Create_InvalidCharacters_ReturnsError(string input)
    {
        var result = Curp.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Curp.InvalidCharacters");
    }

    [Theory]
    [InlineData("1ODE561231HDFRRN08")]
    [InlineData("G1DE561231HDFRRN08")]
    [InlineData("GO1E561231HDFRRN08")]
    [InlineData("GOD1561231HDFRRN08")]
    public void Create_InvalidInitialLetters_ReturnsError(string input)
    {
        var result = Curp.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Curp.InvalidInitialLetters");
    }

    [Theory]
    [InlineData("GODE5A1231HDFRRN08")]
    [InlineData("GODE561A31HDFRRN08")]
    [InlineData("GODE56123AHDFRRN08")]
    public void Create_InvalidBirthDate_ReturnsError(string input)
    {
        var result = Curp.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Curp.InvalidBirthDate");
    }

    [Fact]
    public void Curp_DefaultState_ComparisonOperators()
    {
        var c1 = Curp.Create("GODE561231HDFRRN08").Value;
        var c2 = Curp.Create("MAMR800101MDFRRN01").Value;
        var c1Clone = Curp.Create("gode561231hdfrrn08").Value;

        (c1 < c2).Should().BeTrue();
        (c1 <= c2).Should().BeTrue();
        (c2 > c1).Should().BeTrue();
        (c2 >= c1).Should().BeTrue();

        (c1 < c1Clone).Should().BeFalse();
        (c1 > c1Clone).Should().BeFalse();
        (c1 <= c1Clone).Should().BeTrue();
        (c1 >= c1Clone).Should().BeTrue();
        c1.CompareTo(c2).Should().BeNegative();
        c2.CompareTo(c1).Should().BePositive();
        c1.CompareTo(c1Clone).Should().Be(0);
    }

    [Fact]
    public void Curp_DefaultState_ParseAndTryParse()
    {
        var validStr = "GODE561231HDFRRN08";
        var parsed1 = Curp.Parse(validStr, CultureInfo.InvariantCulture);
        parsed1.Value.Should().Be(validStr);

        var parsed2 = Curp.Parse(validStr.AsSpan(), CultureInfo.InvariantCulture);
        parsed2.Value.Should().Be(validStr);

        Curp.TryParse(validStr, null, out var tryRes1).Should().BeTrue();
        tryRes1.Value.Should().Be(validStr);

        Curp.TryParse(validStr.AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Value.Should().Be(validStr);

        Action invalidParseStr = () => Curp.Parse("invalid", CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid CURP: 'invalid'.");

        Action invalidParseSpan = () => Curp.Parse("invalid".AsSpan(), CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid CURP: 'invalid'.");

        Curp.TryParse("invalid", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(Curp));

        Curp.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(Curp));

        Curp.TryParse("invalid".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(Curp));
    }
}




