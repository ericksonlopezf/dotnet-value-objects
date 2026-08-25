// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="PostalCode"/> Value Object.
/// </summary>
public sealed class PostalCodeTests
{
    [Fact]
    public void PostalCode_ValidInternational_Succeeds()
    {
        var usZip = PostalCode.Create("10001-1234").Value;
        var doZip = PostalCode.Create("10101").Value;
        var ukZip = PostalCode.Create("sw1a 1aa").Value;

        usZip.Value.Should().Be("10001-1234");
        doZip.Value.Should().Be("10101");
        ukZip.Value.Should().Be("SW1A 1AA");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("12")] // < 3 chars
    [InlineData("123456789012345678901")] // > 20 chars
    [InlineData("10101@#$")] // invalid symbols
    public void PostalCode_Invalid_ShouldFail(string? invalid)
    {
        var result = PostalCode.Create(invalid);
        result.IsFailure.Should().BeTrue();
        if (string.IsNullOrWhiteSpace(invalid)) result.Error.Code.Should().Be("PostalCode.Required");
        else if (invalid == "12") result.Error.Code.Should().Be("PostalCode.TooShort");
        else if (invalid == "123456789012345678901") result.Error.Code.Should().Be("PostalCode.TooLong");
        else if (invalid == "10101@#$") result.Error.Code.Should().Be("PostalCode.InvalidFormat");
    }
}




