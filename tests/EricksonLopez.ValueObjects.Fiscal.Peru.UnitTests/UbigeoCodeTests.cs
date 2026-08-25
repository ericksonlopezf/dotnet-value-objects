// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Peru;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Peru.UnitTests;

public sealed class UbigeoCodeTests
{
    [Theory]
    [InlineData("150101", "15", "01", "01")]
    [InlineData("040101", "04", "01", "01")]
    [InlineData("  150101  ", "15", "01", "01")]
    public void Create_Valid6Digits_ExtractsDepartmentProvinceDistrict(
        string input,
        string expectedDept,
        string expectedProv,
        string expectedDist)
    {
        var result = UbigeoCode.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Code.Should().Be(input.Trim());
        result.Value.DepartmentCode.Should().Be(expectedDept);
        result.Value.ProvinceCode.Should().Be(expectedProv);
        result.Value.DistrictCode.Should().Be(expectedDist);
        result.Value.ToString().Should().Be(input.Trim());
    }

    [Theory]
    [InlineData("15010")]   // 5
    [InlineData("1501011")] // 7
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_InvalidLength_ReturnsError(string? input)
    {
        var result = UbigeoCode.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("UbigeoCode.InvalidLength");
    }

    [Theory]
    [InlineData("15010A")]
    [InlineData("15 101")]
    [InlineData("15-101")]
    [InlineData("ABCDEF")]
    public void Create_InvalidCharacters_ReturnsError(string input)
    {
        var result = UbigeoCode.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("UbigeoCode.InvalidCharacters");
    }

    [Fact]
    public void UbigeoCode_DefaultState_ComparisonOperators()
    {
        var u1 = UbigeoCode.Create("040101").Value;
        var u2 = UbigeoCode.Create("150101").Value;
        var u1Clone = UbigeoCode.Create("040101").Value;

        (u1 < u2).Should().BeTrue();
        (u1 <= u2).Should().BeTrue();
        (u2 > u1).Should().BeTrue();
        (u2 >= u1).Should().BeTrue();

        (u1 < u1Clone).Should().BeFalse();
        (u1 > u1Clone).Should().BeFalse();
        (u1 <= u1Clone).Should().BeTrue();
        (u1 >= u1Clone).Should().BeTrue();
        u1.CompareTo(u2).Should().BeNegative();
        u2.CompareTo(u1).Should().BePositive();
        u1.CompareTo(u1Clone).Should().Be(0);
    }

    [Fact]
    public void UbigeoCode_DefaultState_ParseAndTryParse()
    {
        var validStr = "150101";
        var parsed1 = UbigeoCode.Parse(validStr, CultureInfo.InvariantCulture);
        parsed1.Code.Should().Be(validStr);

        var parsed2 = UbigeoCode.Parse(validStr.AsSpan(), CultureInfo.InvariantCulture);
        parsed2.Code.Should().Be(validStr);

        UbigeoCode.TryParse(validStr, null, out var tryRes1).Should().BeTrue();
        tryRes1.Code.Should().Be(validStr);

        UbigeoCode.TryParse(validStr.AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Code.Should().Be(validStr);

        Action invalidParseStr = () => UbigeoCode.Parse("invalid", CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid UbigeoCode: 'invalid'.");

        Action invalidParseSpan = () => UbigeoCode.Parse("invalid".AsSpan(), CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid UbigeoCode: 'invalid'.");

        UbigeoCode.TryParse("invalid", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(UbigeoCode));

        UbigeoCode.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(UbigeoCode));

        UbigeoCode.TryParse("invalid".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(UbigeoCode));
    }
}




