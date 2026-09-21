// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="LicenseKey"/> Value Object.
/// </summary>
public sealed class LicenseKeyTests
{
    [Fact]
    public void LicenseKey_ValidKey_ShouldSucceedAndNormalizeUpper()
    {
        var res1 = LicenseKey.Create("AAAA-BBBB-CCCC");
        var res2 = LicenseKey.Create("prod-key1-abcd-9876");

        res1.IsSuccess.Should().BeTrue();
        res1.Value.Value.Should().Be("AAAA-BBBB-CCCC");

        res2.IsSuccess.Should().BeTrue();
        res2.Value.Value.Should().Be("PROD-KEY1-ABCD-9876");
    }

    [Fact]
    public void LicenseKey_TwoGroups_ShouldSucceed()
    {
        var res = LicenseKey.Create("AAAA-BBBB");
        res.IsSuccess.Should().BeTrue();
        res.Value.Value.Should().Be("AAAA-BBBB");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("AAA-BBB-CCC")] // group length < 4
    [InlineData("AAAA_BBBB_CCCC")] // invalid separator
    [InlineData("TOOLONGGROUP12345-BBBB-CCCC")] // group > 8
    [InlineData("AAAA")] // only 1 group (< 2 total needed)
    public void LicenseKey_InvalidFormat_ShouldFail(string? invalid)
    {
        var result = LicenseKey.Create(invalid);

        result.IsFailure.Should().BeTrue();
        if (string.IsNullOrWhiteSpace(invalid)) result.Error.Code.Should().Be("LicenseKey.Required");
        else if (invalid == "AAAA") result.Error.Code.Should().Be("LicenseKey.TooShort");
        else if (invalid == "AAAA_BBBB_CCCC")
        {
            result.Error.Code.Should().Be("LicenseKey.InvalidFormat");
            result.Error.Description.Should().Be("License key must use grouped uppercase letters and digits separated by hyphens.");
        }
        else result.Error.Code.Should().Be("LicenseKey.InvalidFormat");
    }

    [Fact]
    public void LicenseKey_TooLong_ShouldFail()
    {
        LicenseKey.Create(new string('A', 81)).Error.Code.Should().Be("LicenseKey.TooLong");
    }

    [Fact]
    public void LicenseKey_MaskedAndToString_HidesFirstGroups()
    {
        var license = LicenseKey.Create("PROD-KEY1-ABCD-9876").Value;

        license.Masked().Should().Be("XXXX-XXXX-9876");
        license.ToString().Should().Be("XXXX-XXXX-9876");
    }

    [Fact]
    public void LicenseKey_Equality_SameKey_AreEqual()
    {
        var k1 = LicenseKey.Create("aaaa-bbbb-cccc").Value;
        var k2 = LicenseKey.Create("AAAA-BBBB-CCCC").Value;
        var k3 = LicenseKey.Create("AAAA-BBBB-DDDD").Value;

        k1.ShouldSatisfyEqualityContract(k2, k3, (a, b) => a == b, (a, b) => a != b);
    }
}




