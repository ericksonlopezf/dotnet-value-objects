// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="LastName"/> Value Object.
/// </summary>
public sealed class LastNameTests
{
    [Fact]
    public void LastName_WhitespaceCollapse_And_Accents()
    {
        var lastName = LastName.Create("  Pérez   Gómez  ").Value;
        lastName.Value.Should().Be("Pérez Gómez");
        lastName.ToString().Should().Be("Pérez Gómez");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("User123")] // digits in human name
    [InlineData("User<script>")]
    public void LastName_InvalidCharacters_ShouldFail(string? invalid)
    {
        var last = LastName.Create(invalid);
        last.IsFailure.Should().BeTrue();
        if (string.IsNullOrWhiteSpace(invalid))
        {
            last.Error.Code.Should().Be("LastName.Required");
        }
        else
        {
            last.Error.Code.Should().Be("LastName.InvalidFormat");
        }
    }

    [Fact]
    public void LastName_AccentedAndCompoundNames_ShouldSucceed()
    {
        var last = LastName.Create("O'Connor-Nuñez").Value;
        last.Value.Should().Be("O'Connor-Nuñez");
    }
}



