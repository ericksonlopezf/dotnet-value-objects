// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="PasswordHash"/> Value Object.
/// </summary>
public sealed class PasswordHashTests
{
    [Fact]
    public void PasswordHash_ValidHash_ShouldSucceed()
    {
        // Example Argon2id and BCrypt format hashes
        const string argonHash = "$argon2id$v=19$m=65536,t=3,p=4$c29tZXNhbHQ$RdescudvJCsgTVEvUzTFhg";
        const string bcryptHash = "$2a$12$e8Mc8tkqd.hIPmFcqJ.h6OqVj4M2v5T5LqHq5kUu8oZ7P.m3y4g2W";

        var res1 = PasswordHash.Create(argonHash);
        var res2 = PasswordHash.Create(bcryptHash);

        res1.IsSuccess.Should().BeTrue();
        res1.Value.Value.Should().Be(argonHash);
        res2.IsSuccess.Should().BeTrue();
        res2.Value.Value.Should().Be(bcryptHash);
    }

    [Fact]
    public void PasswordHash_ToString_IsMasked()
    {
        const string hash = "$argon2id$v=19$m=65536,t=3,p=4$c29tZXNhbHQ$RdescudvJCsgTVEvUzTFhg";
        var passwordHash = PasswordHash.Create(hash).Value;

        passwordHash.ToString().Should().Be("***HASHED***");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("too_short_hash_123")] // 18 chars (< 20)
    public void PasswordHash_NullEmptyOrTooShort_ShouldFail(string? invalid)
    {
        var result = PasswordHash.Create(invalid);

        result.IsFailure.Should().BeTrue();
        if (string.IsNullOrWhiteSpace(invalid)) result.Error.Code.Should().Be("PasswordHash.Required");
        else if (invalid == "too_short_hash_123")
        {
            result.Error.Code.Should().Be("PasswordHash.TooShort");
        }
    }

    [Fact]
    public void PasswordHash_TooLong_ShouldFail()
    {
        PasswordHash.Create(new string('a', 513)).Error.Code.Should().Be("PasswordHash.TooLong");
    }

    [Fact]
    public void PasswordHash_ContainingWhitespace_ShouldFail()
    {
        const string hashWithSpace = "$argon2id$v=19$m=65536,t=3,p=4 $c29tZXNhbHQ$RdescudvJCsgTVEvUzTFhg";

        var result = PasswordHash.Create(hashWithSpace);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("PasswordHash.ContainsWhitespace");
        result.Error.Description.Should().Be("Password hash cannot contain whitespace.");
    }

    [Fact]
    public void PasswordHash_Equality_SameHash_AreEqual()
    {
        const string hash1 = "$argon2id$v=19$m=65536,t=3,p=4$c29tZXNhbHQ$RdescudvJCsgTVEvUzTFhg";
        const string hash2 = "$2a$12$e8Mc8tkqd.hIPmFcqJ.h6OqVj4M2v5T5LqHq5kUu8oZ7P.m3y4g2W";
        var h1 = PasswordHash.Create(hash1).Value;
        var h2 = PasswordHash.Create(hash1).Value;
        var h3 = PasswordHash.Create(hash2).Value;

        h1.ShouldSatisfyEqualityContract(h2, h3, (a, b) => a == b, (a, b) => a != b);
    }
}




