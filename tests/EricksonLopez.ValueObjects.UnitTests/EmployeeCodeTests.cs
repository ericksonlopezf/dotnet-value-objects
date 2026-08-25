// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="EmployeeCode"/> Domain Primitive.
/// </summary>
public sealed class EmployeeCodeTests
{
    [Theory]
    [InlineData("emp-0012", "EMP-0012")]
    [InlineData("staff_99", "STAFF_99")]
    public void EmployeeCode_WhenValid_ShouldNormalizeToUpper(string input, string expected)
    {
        var result = EmployeeCode.Create(input);
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(expected);
        result.Value.ToString().Should().Be(expected);
    }

    [Fact]
    public void EmployeeCode_WhenInvalid_ShouldFail()
    {
        EmployeeCode.Create(new string('a', 61)).Error.Code.Should().Be("EmployeeCode.TooLong");
        var invalid = EmployeeCode.Create("EMP#999");
        invalid.Error.Code.Should().Be("EmployeeCode.InvalidFormat");
        invalid.Error.Description.Should().Be("Employee code must contain uppercase alphanumeric characters and standard separators.");
    }
}




