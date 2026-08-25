// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="TenantCode"/> Value Object.
/// </summary>
public sealed class TenantCodeTests
{
    [Fact]
    public void TenantCode_ValidSlug_NormalizesLowercase()
    {
        var result = TenantCode.Create("  ACME-Corp-01  ");

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("acme-corp-01");
        result.Value.ToString().Should().Be("acme-corp-01");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("ab")] // < 3 chars
    [InlineData("-invalid-")]
    [InlineData("invalid_underscore")]
    [InlineData("invalid.dot")]
    public void TenantCode_Invalid_ShouldFail(string? invalid)
    {
        var result = TenantCode.Create(invalid);
        result.IsFailure.Should().BeTrue();
        if (string.IsNullOrWhiteSpace(invalid)) result.Error.Code.Should().Be("TenantCode.Required");
        else if (invalid == "ab") result.Error.Code.Should().Be("TenantCode.TooShort");
        else if (invalid == "-invalid-")
        {
            result.Error.Code.Should().Be("TenantCode.InvalidFormat");
            result.Error.Description.Should().Be("Tenant code must be DNS-friendly lowercase text using letters, digits, and hyphens.");
        }
        else result.Error.Code.Should().Be("TenantCode.InvalidFormat");
    }

    [Fact]
    public void TenantCode_TooLong_Fails()
    {
        TenantCode.Create(new string('a', 65)).Error.Code.Should().Be("TenantCode.TooLong");
    }
}




