// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="WarehouseCode"/> Domain Primitive.
/// </summary>
public sealed class WarehouseCodeTests
{
    [Theory]
    [InlineData("wh-main-01", "WH-MAIN-01")]
    [InlineData("bodega_norte", "BODEGA_NORTE")]
    public void WarehouseCode_WhenValid_ShouldNormalizeToUpper(string input, string expected)
    {
        var result = WarehouseCode.Create(input);
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(expected);
        result.Value.ToString().Should().Be(expected);
    }

    [Fact]
    public void WarehouseCode_WhenInvalid_ShouldFail()
    {
        WarehouseCode.Create(new string('a', 61)).Error.Code.Should().Be("WarehouseCode.TooLong");
        var invalid = WarehouseCode.Create("WH#999");
        invalid.Error.Code.Should().Be("WarehouseCode.InvalidFormat");
        invalid.Error.Description.Should().Be("Warehouse code must contain uppercase alphanumeric characters and standard separators.");
    }
}




