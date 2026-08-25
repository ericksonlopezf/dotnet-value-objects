// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="PhoneNumber"/> Value Object.
/// </summary>
public sealed class PhoneNumberTests
{
    [Fact]
    public void PhoneNumber_ValidE164_ShouldSucceed()
    {
        var result = PhoneNumber.Create("+1 (809) 555-1234");

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("+18095551234");
        result.Value.DigitsOnly.Should().Be("18095551234");
        result.Value.Formatted.Should().Be("(809) 555-1234");
        result.Value.ToString().Should().Be("+18095551234");

        // 8 digits minimum boundary
        var min8 = PhoneNumber.Create("+12345678");
        min8.IsSuccess.Should().BeTrue();
        min8.Value.DigitsOnly.Should().Be("12345678");

        // 15 digits maximum boundary
        var max15 = PhoneNumber.Create("+123456789012345");
        max15.IsSuccess.Should().BeTrue();
        max15.Value.DigitsOnly.Should().Be("123456789012345");

        // Default struct edge cases
        default(PhoneNumber).DigitsOnly.Should().Be(string.Empty);
        default(PhoneNumber).Formatted.Should().Be(string.Empty);
        default(PhoneNumber).ToString().Should().Be(string.Empty);
    }

    [Fact]
    public void PhoneNumber_NonNanpNumber_FormattedReturnsE164()
    {
        var result = PhoneNumber.Create("+34 91 234 5678");
        result.IsSuccess.Should().BeTrue();
        result.Value.Formatted.Should().Be("+34912345678");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("8095551234")] // Missing plus
    [InlineData("+1234567")]   // 7 digits (too few)
    [InlineData("+1234567890123456")] // 16 digits (too many)
    [InlineData("+180912345a")] // non digit
    public void PhoneNumber_Invalid_ShouldFail(string? invalid)
    {
        var result = PhoneNumber.Create(invalid);
        result.IsFailure.Should().BeTrue();
        if (string.IsNullOrWhiteSpace(invalid)) result.Error.Code.Should().Be("PhoneNumber.Required");
        else if (!invalid.Trim().StartsWith('+')) result.Error.Code.Should().Be("PhoneNumber.MissingCountryCode");
        else result.Error.Code.Should().Be("PhoneNumber.InvalidFormat");
    }

    [Fact]
    public void PhoneNumber_ComparisonsAndOperators_Exhaustive()
    {
        var p1 = PhoneNumber.Create("+18091111111").Value;
        var p1Copy = PhoneNumber.Create("+18091111111").Value;
        var p2 = PhoneNumber.Create("+18092222222").Value;

        p1.ShouldSatisfyEqualityContract(p1Copy, p2, (x, y) => x == y, (x, y) => x != y);
        p1.ShouldSatisfyComparisonContract(p1Copy, p2,
            (x, y) => x < y,
            (x, y) => x <= y,
            (x, y) => x > y,
            (x, y) => x >= y);

        ((IComparable)p1).CompareTo((object)p1).Should().Be(0);
        ((IComparable)p1).CompareTo((object)p2).Should().BeNegative();

        Action invalidObj = () => p1.CompareTo("not-a-phone");
        invalidObj.Should().Throw<ArgumentException>()
            .WithMessage("*Object is not a PhoneNumber*");

        Action nullObj = () => ((IComparable)p1).CompareTo(null);
        nullObj.Should().Throw<ArgumentException>()
            .WithMessage("*Object is not a PhoneNumber*");
    }

    [Fact]
    public void Parsing_StringAndSpan_ParsesOrThrows()
    {
        var p = PhoneNumber.Parse("+18095551234", CultureInfo.InvariantCulture);
        p.Value.Should().Be("+18095551234");

        PhoneNumber.Parse("  +1 (809) 555-1234  ", CultureInfo.InvariantCulture).Value.Should().Be("+18095551234");
        PhoneNumber.Parse("+18095551234".AsSpan(), CultureInfo.InvariantCulture).Value.Should().Be("+18095551234");

        Action nullAct = () => PhoneNumber.Parse((string)null!, CultureInfo.InvariantCulture);
        nullAct.Should().Throw<FormatException>();

        Action invalidFormat = () => PhoneNumber.Parse("8095551234", CultureInfo.InvariantCulture);
        invalidFormat.Should().Throw<FormatException>()
            .WithMessage("Phone number must start with '+' country code (E.164 format).");

        Action invalidSpan = () => PhoneNumber.Parse("123".AsSpan(), CultureInfo.InvariantCulture);
        invalidSpan.Should().Throw<FormatException>();
    }

    [Fact]
    public void TryParse_StringAndSpan_ReturnsSuccessOrFailure()
    {
        PhoneNumber.TryParse("+18095551234", null, out var r1).Should().BeTrue();
        r1.Value.Should().Be("+18095551234");

        PhoneNumber.TryParse(null, null, out var rNull).Should().BeFalse();
        rNull.Should().Be(default);

        PhoneNumber.TryParse("8095551234", null, out var rInvalid).Should().BeFalse();

        PhoneNumber.TryParse("+18095551234".AsSpan(), null, out var rSpan).Should().BeTrue();
        rSpan.Value.Should().Be("+18095551234");

        PhoneNumber.TryParse("123".AsSpan(), null, out var rSpanInvalid).Should().BeFalse();
    }
}




