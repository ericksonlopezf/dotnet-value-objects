// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="WebsiteUrl"/> Value Object.
/// </summary>
public sealed class WebsiteUrlTests
{
    [Fact]
    public void WebsiteUrl_Valid_ExtractsHost()
    {
        var url = WebsiteUrl.Create("https://example.com/api/v1").Value;
        url.Value.Should().Be("https://example.com/api/v1");
        url.Host.Should().Be("example.com");
    }

    [Fact]
    public void WebsiteUrl_ValidHttps_CachesHost()
    {
        var result = WebsiteUrl.Create("  https://ericksonlopez.dev/docs  ");

        result.IsSuccess.Should().BeTrue();
        result.Value.Host.Should().Be("ericksonlopez.dev");
        result.Value.Value.Should().Be("https://ericksonlopez.dev/docs");
        result.Value.ToString().Should().Be("https://ericksonlopez.dev/docs");
    }

    [Fact]
    public void WebsiteUrl_Invalid_ShouldFail()
    {
        WebsiteUrl.Create(null).Error.Code.Should().Be("WebsiteUrl.Required");
        WebsiteUrl.Create("http://").Error.Code.Should().Be("WebsiteUrl.TooShort");
        WebsiteUrl.Create(new string('a', 2049)).Error.Code.Should().Be("WebsiteUrl.TooLong");

        var nonHttp = WebsiteUrl.Create("ftp://example.com");
        nonHttp.Error.Code.Should().Be("WebsiteUrl.InvalidFormat");
        nonHttp.Error.Description.Should().Be("Website URL must be an absolute HTTP or HTTPS URL.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("ftp://files.example.com")] // non-http/https
    [InlineData("/relative/path")]
    [InlineData("not_a_url")]
    public void WebsiteUrl_InvalidInputs_ShouldFail(string? invalid)
    {
        var result = WebsiteUrl.Create(invalid);
        result.IsFailure.Should().BeTrue();
        if (string.IsNullOrWhiteSpace(invalid)) result.Error.Code.Should().Be("WebsiteUrl.Required");
        else result.Error.Code.Should().Be("WebsiteUrl.InvalidFormat");
    }

    [Fact]
    public void WebsiteUrl_HttpScheme_Succeeds()
    {
        var http = WebsiteUrl.Create("http://example.org/path").Value;
        http.Host.Should().Be("example.org");
        http.Value.Should().Be("http://example.org/path");
    }

    [Fact]
    public void WebsiteUrl_EqualityContract()
    {
        var u1 = WebsiteUrl.Create("https://example.com/a").Value;
        var u1Copy = WebsiteUrl.Create("https://example.com/a").Value;
        var u2 = WebsiteUrl.Create("https://example.com/b").Value;

        u1.ShouldSatisfyEqualityContract(u1Copy, u2, (a, b) => a == b, (a, b) => a != b);
    }
}




