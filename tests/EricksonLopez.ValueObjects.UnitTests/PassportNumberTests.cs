// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="PassportNumber"/> Value Object.
/// </summary>
public sealed class PassportNumberTests
{
    [Fact]
    public void PassportNumber_WhenValid_ShouldNormalizeAndMaskSensitiveData()
    {
        var result = PassportNumber.Create("  rd9918274  ");
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("RD9918274");
        result.Value.ToString().Should().Be("XXXXXXXXX"); // Masked by SensitiveDataAttribute
    }

    [Fact]
    public void PassportNumber_WhenInvalid_ShouldFail()
    {
        PassportNumber.Create(new string('A', 21)).Error.Code.Should().Be("PassportNumber.TooLong");
        PassportNumber.Create("ABCD").Error.Code.Should().Be("PassportNumber.TooShort");
        var invalid = PassportNumber.Create("PASS#999");
        invalid.Error.Code.Should().Be("PassportNumber.InvalidFormat");
        invalid.Error.Description.Should().Be("Passport number must contain between 5 and 20 alphanumeric characters.");
    }
}




