// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="Code"/> Domain Primitive.
/// </summary>
public sealed class CodeTests
{
    [Fact]
    public void Code_Valid_NormalizesUppercase()
    {
        var code = Code.Create("dept-01").Value;
        code.Value.Should().Be("DEPT-01");
        code.ToString().Should().Be("DEPT-01");
    }

    [Fact]
    public void Code_TooLong_ShouldFail()
    {
        Code.Create(new string('a', 61)).Error.Code.Should().Be("Code.TooLong");
    }
}



