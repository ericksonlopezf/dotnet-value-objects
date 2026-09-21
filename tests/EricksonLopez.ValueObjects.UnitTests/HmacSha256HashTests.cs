// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="HmacSha256Hash"/> Value Object.
/// </summary>
public sealed class HmacSha256HashTests
{
    private const string ValidHex64 = "a3f5c9e2b1d40876543210abcdef9876543210fedcba0123456789abcdef0123";
    private const string UpperHex64 = "A3F5C9E2B1D40876543210ABCDEF9876543210FEDCBA0123456789ABCDEF0123";

    [Fact]
    public void HmacSha256Hash_ValidHash_ShouldSucceedAndNormalizeToLowercase()
    {
        var result = HmacSha256Hash.Create(UpperHex64);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(ValidHex64);
    }

    [Fact]
    public void HmacSha256Hash_ToString_IsMasked()
    {
        var hash = HmacSha256Hash.Create(ValidHex64).Value;

        hash.ToString().Should().Be("****************************************************************");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void HmacSha256Hash_NullOrWhitespace_ShouldFail(string? invalid)
    {
        var result = HmacSha256Hash.Create(invalid);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("HmacSha256Hash.Required");
    }

    [Theory]
    [InlineData("a3f5c9e2b1d4")] // too short
    [InlineData("a3f5c9e2b1d40876543210abcdef9876543210fedcba0123456789abcdef012345")] // too long
    public void HmacSha256Hash_InvalidLength_ShouldFail(string invalidLength)
    {
        var result = HmacSha256Hash.Create(invalidLength);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().BeOneOf("HmacSha256Hash.TooShort", "HmacSha256Hash.TooLong");
    }

    [Fact]
    public void HmacSha256Hash_InvalidHexCharacters_ShouldFail()
    {
        // 64 chars but contains 'g' and 'z'
        var invalidHex = "g3f5c9e2b1d40876543210abcdef9876543210fedcba0123456789abcdef012z";

        var result = HmacSha256Hash.Create(invalidHex);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("HmacSha256Hash.InvalidFormat");
    }

    [Fact]
    public void HmacSha256Hash_Hydrate_CreatesWithoutValidation()
    {
        var hydrated = HmacSha256Hash.Hydrate(ValidHex64);

        hydrated.Value.Should().Be(ValidHex64);
    }

    [Fact]
    public void HmacSha256Hash_Equality_SameHash_AreEqual()
    {
        const string otherHex = "0000000000000000000000000000000000000000000000000000000000000000";
        var h1 = HmacSha256Hash.Create(ValidHex64).Value;
        var h2 = HmacSha256Hash.Create(UpperHex64).Value;
        var h3 = HmacSha256Hash.Create(otherHex).Value;

        h1.ShouldSatisfyEqualityContract(h2, h3, (a, b) => a == b, (a, b) => a != b);
    }
}
