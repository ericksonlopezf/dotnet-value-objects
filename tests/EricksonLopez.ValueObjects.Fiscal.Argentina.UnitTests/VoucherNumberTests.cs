// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Argentina;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Argentina.UnitTests;

public sealed class VoucherNumberTests
{
    [Theory]
    [InlineData(1, "00000001")]
    [InlineData(42, "00000042")]
    [InlineData(99999999, "99999999")]
    public void Create_ValidInt_FormatsCorrectly(int value, string expectedFormatted)
    {
        var vn = VoucherNumber.Create(value).Value;

        vn.Value.Should().Be(value);
        vn.Formatted.Should().Be(expectedFormatted);
        vn.ToString().Should().Be(expectedFormatted);
    }

    [Theory]
    [InlineData("1", 1)]
    [InlineData("00000042", 42)]
    [InlineData("99999999", 99999999)]
    public void Create_ValidString_Succeeds(string input, int expectedValue)
    {
        var result = VoucherNumber.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(expectedValue);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(100_000_000)]
    public void Create_OutOfRange_ReturnsOutOfRangeError(int invalid)
    {
        var result = VoucherNumber.Create(invalid);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("VoucherNumber.OutOfRange");
    }

    [Theory]
    [InlineData("ABC")]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_InvalidFormat_ReturnsInvalidFormatError(string invalid)
    {
        var result = VoucherNumber.Create(invalid);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("VoucherNumber.InvalidFormat");
    }

    [Fact]
    public void VoucherNumber_DefaultState_ParseAndTryParse()
    {
        var parsed1 = VoucherNumber.Parse("00000001", System.Globalization.CultureInfo.InvariantCulture);
        parsed1.Value.Should().Be(1);

        var parsed2 = VoucherNumber.Parse("00000001".AsSpan(), System.Globalization.CultureInfo.InvariantCulture);
        parsed2.Value.Should().Be(1);

        VoucherNumber.TryParse("00000001", null, out var tryRes1).Should().BeTrue();
        tryRes1.Value.Should().Be(1);

        VoucherNumber.TryParse("00000001".AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Value.Should().Be(1);

        Action invalidParseStr = () => VoucherNumber.Parse("invalid", System.Globalization.CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid VoucherNumber: 'invalid'.");

        Action invalidParseSpan = () => VoucherNumber.Parse("invalid".AsSpan(), System.Globalization.CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid VoucherNumber: 'invalid'.");

        VoucherNumber.TryParse("invalid", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(VoucherNumber));

        VoucherNumber.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(VoucherNumber));

        VoucherNumber.TryParse("invalid".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(VoucherNumber));

        VoucherNumber.Create((string?)null).IsFailure.Should().BeTrue();
    }

    [Fact]
    public void VoucherNumber_ComparisonsAndOperators_Exhaustive()
    {
        var a = VoucherNumber.Create(1).Value;
        var aCopy = VoucherNumber.Create(1).Value;
        var b = VoucherNumber.Create(2).Value;

        a.ShouldSatisfyEqualityContract(aCopy, b, (x, y) => x == y, (x, y) => x != y);
        a.ShouldSatisfyComparisonContract(aCopy, b,
            (x, y) => x < y,
            (x, y) => x <= y,
            (x, y) => x > y,
            (x, y) => x >= y);
    }
}




