// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="SKU"/> Value Object.
/// </summary>
public sealed class SkuTests
{
    [Fact]
    public void SKU_Valid_NormalizesUppercase()
    {
        var sku = SKU.Create("sku-100-blue").Value;
        sku.Value.Should().Be("SKU-100-BLUE");
        sku.ToString().Should().Be("SKU-100-BLUE");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("SKU#123")] // invalid char
    public void SKU_Invalid_ShouldFail(string? invalid)
    {
        var result = SKU.Create(invalid);
        result.IsFailure.Should().BeTrue();
        if (string.IsNullOrWhiteSpace(invalid)) result.Error.Code.Should().Be("SKU.Required");
        else if (invalid == "SKU#123")
        {
            result.Error.Code.Should().Be("SKU.InvalidFormat");
            result.Error.Description.Should().Be("SKU has an invalid format.");
        }
    }

    [Fact]
    public void SKU_Boundaries_ShouldFail()
    {
        SKU.Create(new string('a', 65)).Error.Code.Should().Be("SKU.TooLong");
    }
}




