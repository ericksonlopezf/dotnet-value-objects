// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="NationalId"/> Value Object.
/// </summary>
public sealed class NationalIdTests
{
    [Fact]
    public void NationalId_ValidId_NormalizesUppercase()
    {
        var result = NationalId.Create("  id-98765432-a  ");

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("ID-98765432-A");
        result.Value.ToString().Should().Be("***"); // PII defense: ToString() returns the configured mask
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("12")] // < 4 chars
    [InlineData("ID#999")] // invalid symbol
    public void NationalId_Invalid_ShouldFail(string? invalid)
    {
        var result = NationalId.Create(invalid);
        result.IsFailure.Should().BeTrue();
        if (invalid == "12") result.Error.Code.Should().Be("NationalId.TooShort");
        if (invalid == "ID#999")
        {
            result.Error.Code.Should().Be("NationalId.InvalidFormat");
            result.Error.Description.Should().Be("National ID must contain alphanumeric characters, spaces, periods, underscores, slashes, or hyphens.");
        }
    }

    [Fact]
    public void NationalId_TooLong_ShouldFail()
    {
        NationalId.Create(new string('a', 41)).Error.Code.Should().Be("NationalId.TooLong");
    }
}




