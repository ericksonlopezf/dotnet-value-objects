// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="CustomerCode"/> Domain Primitive.
/// </summary>
public sealed class CustomerCodeTests
{
    [Theory]
    [InlineData("cli-001", "CLI-001")]
    [InlineData("cust_enterprise", "CUST_ENTERPRISE")]
    public void CustomerCode_WhenValid_ShouldNormalizeToUpper(string input, string expected)
    {
        var result = CustomerCode.Create(input);
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(expected);
        result.Value.ToString().Should().Be(expected);
    }

    [Fact]
    public void CustomerCode_WhenInvalid_ShouldFail()
    {
        CustomerCode.Create(new string('a', 61)).Error.Code.Should().Be("CustomerCode.TooLong");
        var invalid = CustomerCode.Create("CLI#999");
        invalid.Error.Code.Should().Be("CustomerCode.InvalidFormat");
        invalid.Error.Description.Should().Be("Customer code must contain uppercase alphanumeric characters and standard separators.");
    }
}




