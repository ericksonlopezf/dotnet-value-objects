// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="SupplierCode"/> Domain Primitive.
/// </summary>
public sealed class SupplierCodeTests
{
    [Theory]
    [InlineData("prov-992", "PROV-992")]
    [InlineData("supp_acme", "SUPP_ACME")]
    public void SupplierCode_WhenValid_ShouldNormalizeToUpper(string input, string expected)
    {
        var result = SupplierCode.Create(input);
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(expected);
        result.Value.ToString().Should().Be(expected);
    }

    [Fact]
    public void SupplierCode_WhenInvalid_ShouldFail()
    {
        SupplierCode.Create(new string('a', 61)).Error.Code.Should().Be("SupplierCode.TooLong");
        var invalid = SupplierCode.Create("PROV#999");
        invalid.Error.Code.Should().Be("SupplierCode.InvalidFormat");
        invalid.Error.Description.Should().Be("Supplier code must contain uppercase alphanumeric characters and standard separators.");
    }
}




