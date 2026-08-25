// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="FirstName"/> Value Object.
/// </summary>
public sealed class FirstNameTests
{
    [Fact]
    public void FirstName_WhitespaceCollapse_And_Accents()
    {
        var firstName = FirstName.Create("  José   María  ").Value;
        firstName.Value.Should().Be("José María");
        firstName.ToString().Should().Be("José María");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("User123")] // digits in human name
    [InlineData("User<script>")]
    public void FirstName_InvalidCharacters_ShouldFail(string? invalid)
    {
        var first = FirstName.Create(invalid);
        first.IsFailure.Should().BeTrue();
        if (string.IsNullOrWhiteSpace(invalid))
        {
            first.Error.Code.Should().Be("FirstName.Required");
        }
        else
        {
            first.Error.Code.Should().Be("FirstName.InvalidFormat");
        }
    }

    [Fact]
    public void FirstName_AccentedAndCompoundNames_ShouldSucceed()
    {
        var first = FirstName.Create("Jean-Pierre").Value;
        first.Value.Should().Be("Jean-Pierre");
    }

    [Theory]
    [InlineData("First\0Name")]
    [InlineData("First\u0007Name")]
    [InlineData("First\u001FName")]
    public void FirstName_ControlCharacters_ShouldFail(string invalidWithControl)
    {
        var res = FirstName.Create(invalidWithControl);
        res.IsFailure.Should().BeTrue();
        res.Error.Code.Should().Be("FirstName.ControlCharacters");
    }
}



