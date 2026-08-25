// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="DocumentNumber"/> Value Object.
/// </summary>
public sealed class DocumentNumberTests
{
    [Fact]
    public void DocumentNumber_Valid_NormalizesUppercase()
    {
        var doc = DocumentNumber.Create("fac-2026-001").Value;
        doc.Value.Should().Be("FAC-2026-001");
        doc.ToString().Should().Be("FAC-2026-001");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("INV#123")] // invalid symbol #
    public void DocumentNumber_Invalid_ShouldFail(string? invalid)
    {
        var result = DocumentNumber.Create(invalid);
        result.IsFailure.Should().BeTrue();
        if (string.IsNullOrWhiteSpace(invalid)) result.Error.Code.Should().Be("DocumentNumber.Required");
        else result.Error.Code.Should().Be("DocumentNumber.InvalidFormat");
    }

    [Fact]
    public void DocumentNumber_Invalid_ShouldReturnSpecificErrors()
    {
        var invalid = DocumentNumber.Create("INV#123");
        invalid.Error.Code.Should().Be("DocumentNumber.InvalidFormat");
        invalid.Error.Description.Should().Be("Document number must start with an alphanumeric character and contain only letters, digits, periods, underscores, slashes, or hyphens.");

        DocumentNumber.Create(new string('A', 61)).Error.Code.Should().Be("DocumentNumber.TooLong");
    }

    [Fact]
    public void DocumentNumber_EqualityContract()
    {
        var d1 = DocumentNumber.Create("DOC-001").Value;
        var d1Copy = DocumentNumber.Create("doc-001").Value;
        var d2 = DocumentNumber.Create("DOC-002").Value;

        d1.ShouldSatisfyEqualityContract(d1Copy, d2, (a, b) => a == b, (a, b) => a != b);
    }
}




