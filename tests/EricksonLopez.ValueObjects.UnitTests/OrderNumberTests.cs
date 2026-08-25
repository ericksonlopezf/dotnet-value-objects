// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="OrderNumber"/> Domain Primitive.
/// </summary>
public sealed class OrderNumberTests
{
    [Theory]
    [InlineData("ord-2026-001", "ORD-2026-001")]
    [InlineData("  po_99182  ", "PO_99182")]
    public void OrderNumber_WhenValid_ShouldNormalizeToUpper(string input, string expected)
    {
        var result = OrderNumber.Create(input);
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(expected);
        result.Value.ToString().Should().Be(expected);
    }

    [Fact]
    public void OrderNumber_WhenInvalid_ShouldFail()
    {
        OrderNumber.Create(new string('a', 81)).Error.Code.Should().Be("OrderNumber.TooLong");
        var invalid = OrderNumber.Create("ORD#999");
        invalid.Error.Code.Should().Be("OrderNumber.InvalidFormat");
        invalid.Error.Description.Should().Be("Order number must contain uppercase alphanumeric characters and standard separators.");
    }
}




