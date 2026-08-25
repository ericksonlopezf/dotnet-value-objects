// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="LanguageCode"/> Value Object.
/// </summary>
public sealed class LanguageCodeTests
{
    [Theory]
    [InlineData("ES", "es")]
    [InlineData("EN", "en")]
    [InlineData("fra", "fra")]
    public void LanguageCode_WhenValid_ShouldNormalizeToLowercase(string input, string expected)
    {
        var result = LanguageCode.Create(input);
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(expected);
        result.Value.ToString().Should().Be(expected);
    }

    [Theory]
    [InlineData("e")]
    [InlineData("espa")]
    [InlineData("12")]
    [InlineData(null)]
    public void LanguageCode_WhenInvalid_ShouldFail(string? input)
    {
        var result = LanguageCode.Create(input);
        result.IsFailure.Should().BeTrue();
        if (input == "e") result.Error.Code.Should().Be("LanguageCode.TooShort");
        if (input == "espa") result.Error.Code.Should().Be("LanguageCode.TooLong");
        if (input == "12")
        {
            result.Error.Code.Should().Be("LanguageCode.InvalidFormat");
            result.Error.Description.Should().Be("Language code must be a 2 or 3 letter ISO 639 identifier.");
        }
    }
}




