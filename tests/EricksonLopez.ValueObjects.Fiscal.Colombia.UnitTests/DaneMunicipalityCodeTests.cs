// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Colombia;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Colombia.UnitTests;

public sealed class DaneMunicipalityCodeTests
{
    [Theory]
    [InlineData("11001", "11", "001")]
    [InlineData("05001", "05", "001")]
    [InlineData("76001", "76", "001")]
    [InlineData("  11001  ", "11", "001")]
    public void Create_Valid5Digits_ExtractsDepartmentAndMunicipality(string input, string expectedDept, string expectedMuni)
    {
        var result = DaneMunicipalityCode.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Code.Should().Be(input.Trim());
        result.Value.DepartmentCode.Should().Be(expectedDept);
        result.Value.MunicipalityCode.Should().Be(expectedMuni);
        result.Value.ToString().Should().Be(input.Trim());
    }

    [Theory]
    [InlineData("1100")]
    [InlineData("110011")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_InvalidLength_ReturnsError(string? input)
    {
        var result = DaneMunicipalityCode.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("DaneMunicipalityCode.InvalidLength");
    }

    [Theory]
    [InlineData("1100A")]
    [InlineData("11 01")]
    [InlineData("11-01")]
    [InlineData("ABCDE")]
    public void Create_InvalidCharacters_ReturnsError(string input)
    {
        var result = DaneMunicipalityCode.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("DaneMunicipalityCode.InvalidCharacters");
    }

    [Fact]
    public void DaneMunicipalityCode_DefaultState_Equality()
    {
        var dane1 = DaneMunicipalityCode.Create("11001").Value;
        var dane2 = DaneMunicipalityCode.Create("11001").Value;
        var daneDiff = DaneMunicipalityCode.Create("05001").Value;

        (dane1 == dane2).Should().BeTrue();
        (dane1 != daneDiff).Should().BeTrue();
        dane1.Equals(dane2).Should().BeTrue();
        dane1.Equals((object)dane2).Should().BeTrue();
        dane1.Equals(daneDiff).Should().BeFalse();
        dane1.GetHashCode().Should().Be(dane2.GetHashCode());
    }

    [Fact]
    public void DaneMunicipalityCode_DefaultState_ParseAndTryParse()
    {
        var parsed1 = DaneMunicipalityCode.Parse("11001", CultureInfo.InvariantCulture);
        parsed1.Code.Should().Be("11001");

        var parsed2 = DaneMunicipalityCode.Parse("11001".AsSpan(), CultureInfo.InvariantCulture);
        parsed2.Code.Should().Be("11001");

        DaneMunicipalityCode.TryParse("11001", null, out var tryRes1).Should().BeTrue();
        tryRes1.Code.Should().Be("11001");

        DaneMunicipalityCode.TryParse("11001".AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Code.Should().Be("11001");

        Action invalidParseStr = () => DaneMunicipalityCode.Parse("invalid", CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid DANE code: 'invalid'.");

        Action invalidParseSpan = () => DaneMunicipalityCode.Parse("invalid".AsSpan(), CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid DANE code: 'invalid'.");

        DaneMunicipalityCode.TryParse("invalid", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(DaneMunicipalityCode));

        DaneMunicipalityCode.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(DaneMunicipalityCode));

        DaneMunicipalityCode.TryParse("invalid".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(DaneMunicipalityCode));
    }
}




