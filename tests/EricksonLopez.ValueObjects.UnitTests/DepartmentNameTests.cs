// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="DepartmentName"/> Domain Primitive.
/// </summary>
public sealed class DepartmentNameTests
{
    [Fact]
    public void DepartmentName_WhenValid_ShouldCollapseWhitespace()
    {
        var result = DepartmentName.Create("  Tecnología  e  Innovación  ");
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("Tecnología e Innovación");
        result.Value.ToString().Should().Be("Tecnología e Innovación");
    }

    [Fact]
    public void DepartmentName_WhenInvalid_ShouldFail()
    {
        DepartmentName.Create(new string('a', 121)).Error.Code.Should().Be("DepartmentName.TooLong");
        var invalid = DepartmentName.Create("Department<script>");
        invalid.Error.Code.Should().Be("DepartmentName.InvalidFormat");
        invalid.Error.Description.Should().Be("Department name must contain valid business name characters.");
    }
}




