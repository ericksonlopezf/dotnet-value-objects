// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="LocaleCode"/> Value Object.
/// </summary>
public sealed class LocaleCodeTests
{
    [Theory]
    [InlineData("es-do", "es-DO")]
    [InlineData("en_US", "en-US")]
    [InlineData("fr", "fr")]
    [InlineData("pt-br", "pt-BR")]
    public void LocaleCode_WhenValid_ShouldNormalizeProperly(string input, string expected)
    {
        var result = LocaleCode.Create(input);
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(expected);
        result.Value.ToString().Should().Be(expected);
    }

    [Fact]
    public void LocaleCode_WhenInvalid_ShouldFailWithSpecificErrors()
    {
        LocaleCode.Create("a").Error.Code.Should().Be("LocaleCode.TooShort");
        LocaleCode.Create(new string('a', 11)).Error.Code.Should().Be("LocaleCode.TooLong");
        var invalid = LocaleCode.Create("invalid_locale_format");
        invalid.Error.Code.Should().Be("LocaleCode.TooLong");
        var invalidPattern = LocaleCode.Create("es-INVALID");
        invalidPattern.Error.Code.Should().Be("LocaleCode.InvalidFormat");
        invalidPattern.Error.Description.Should().Be("Locale code must be formatted as 'language' or 'language-COUNTRY' (e.g., 'es-DO', 'en-US').");
    }

    [Fact]
    public void LocaleCode_EqualityContract()
    {
        var l1 = LocaleCode.Create("es-do").Value;
        var l1Copy = LocaleCode.Create("es_DO").Value;
        var l2 = LocaleCode.Create("en-us").Value;

        l1.ShouldSatisfyEqualityContract(l1Copy, l2, (a, b) => a == b, (a, b) => a != b);
    }
}




