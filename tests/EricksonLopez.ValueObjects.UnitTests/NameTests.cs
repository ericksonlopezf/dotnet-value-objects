// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="Name"/> Domain Primitive.
/// </summary>
public sealed class NameTests
{
    [Fact]
    public void Name_Valid_CollapsesWhitespace()
    {
        var name = Name.Create("  General   Category  ").Value;
        name.Value.Should().Be("General Category");
        name.ToString().Should().Be("General Category");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Name_Required_ShouldFail(string? invalid)
    {
        var result = Name.Create(invalid);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Name.Required");
    }

    [Fact]
    public void Name_Boundaries_ShouldFail()
    {
        Name.Create(new string('a', 201)).Error.Code.Should().Be("Name.TooLong");

        var invalidChar = Name.Create("Category <1>");
        invalidChar.Error.Code.Should().Be("Name.InvalidFormat");
        invalidChar.Error.Description.Should().Be("Name can contain letters, digits, spaces, and common business punctuation.");
    }

    [Fact]
    public void Name_EqualityContract()
    {
        var n1 = Name.Create("Category A").Value;
        var n1Copy = Name.Create("Category A").Value;
        var n2 = Name.Create("Category B").Value;

        n1.ShouldSatisfyEqualityContract(n1Copy, n2, (a, b) => a == b, (a, b) => a != b);
    }
}




