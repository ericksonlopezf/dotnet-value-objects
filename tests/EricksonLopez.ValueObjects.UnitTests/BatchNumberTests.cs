// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="BatchNumber"/> Domain Primitive.
/// </summary>
public sealed class BatchNumberTests
{
    [Theory]
    [InlineData("lot-2026-a", "LOT-2026-A")]
    [InlineData("batch_0199", "BATCH_0199")]
    public void BatchNumber_WhenValid_ShouldNormalizeToUpper(string input, string expected)
    {
        var result = BatchNumber.Create(input);
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(expected);
        result.Value.ToString().Should().Be(expected);
    }

    [Fact]
    public void BatchNumber_WhenInvalid_ShouldFail()
    {
        BatchNumber.Create(new string('a', 81)).Error.Code.Should().Be("BatchNumber.TooLong");
        var invalid = BatchNumber.Create("BATCH#999");
        invalid.Error.Code.Should().Be("BatchNumber.InvalidFormat");
        invalid.Error.Description.Should().Be("Batch number must contain uppercase alphanumeric characters and standard separators.");
    }
}




