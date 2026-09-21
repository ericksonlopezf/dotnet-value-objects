// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic.UnitTests;

public sealed class EcfAuthSeedTests
{
    [Fact]
    public void Create_ValidSeed_Succeeds()
    {
        var rnc = Rnc.Create("101000015").Value;
        var now = DateTimeOffset.UtcNow;
        var expires = now.AddMinutes(5);

        var result = EcfAuthSeed.Create("SEED123456789", rnc, now, expires);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("SEED123456789");
        result.Value.TaxpayerRnc.Should().Be(rnc.Value);
        result.Value.IsExpiredAsOf(now.AddMinutes(2)).Should().BeFalse();
        result.Value.IsExpiredAsOf(now.AddMinutes(6)).Should().BeTrue();
    }

    [Fact]
    public void Create_TooShort_ReturnsError()
    {
        var rnc = Rnc.Create("101000015").Value;
        var now = DateTimeOffset.UtcNow;

        var result = EcfAuthSeed.Create("SHORT", rnc, now, now.AddMinutes(5));

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("EcfAuthSeed.TooShort");
    }

    [Fact]
    public void Create_ExpirationBeforeGeneration_ReturnsError()
    {
        var rnc = Rnc.Create("101000015").Value;
        var now = DateTimeOffset.UtcNow;

        var result = EcfAuthSeed.Create("SEED123456789", rnc, now, now.AddMinutes(-1));

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("EcfAuthSeed.InvalidExpiration");
    }

    [Fact]
    public void CreateWithStandardExpiration_Succeeds()
    {
        var rnc = Rnc.Create("101000015").Value;
        var now = DateTimeOffset.UtcNow;

        var result = EcfAuthSeed.CreateWithStandardExpiration("SEED123456789", rnc, now);

        result.IsSuccess.Should().BeTrue();
        result.Value.ExpiresAt.Should().Be(now.AddMinutes(5));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_NullOrEmptySeed_ReturnsRequiredError(string? invalidSeed)
    {
        var now = DateTimeOffset.UtcNow;
        var result = EcfAuthSeed.Create(invalidSeed, "101000015", now, now.AddMinutes(5));
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("EcfAuthSeed.Required");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_NullOrEmptyReceiverRnc_ReturnsRncRequiredError(string? invalidRnc)
    {
        var now = DateTimeOffset.UtcNow;
        var result = EcfAuthSeed.Create("SEED123456789", invalidRnc, now, now.AddMinutes(5));
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("EcfAuthSeed.RncRequired");
    }

    [Fact]
    public void CreateWithExpiration_Succeeds()
    {
        var seed = EcfAuthSeed.CreateWithExpiration("SEED123456789", "101000015", 10);
        seed.Should().NotBeNull();
        seed.SeedValue.Should().Be("SEED123456789");
        seed.ReceiverRnc.Should().Be("101000015");
        seed.RNC.Should().Be("101000015");
        seed.GeneratedAt.Should().BeBefore(seed.ExpiresAt);
    }

    [Fact]
    public void CreateWithExpiration_InvalidParameters_ThrowsArgumentException()
    {
        Action act = () => EcfAuthSeed.CreateWithExpiration("SHORT", "101000015", 5);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void IsExpired_And_RemainingTime_FunctionCorrectly()
    {
        var now = DateTimeOffset.UtcNow;
        var activeSeed = EcfAuthSeed.Reconstitute("SEED123456789", "101000015", now.AddMinutes(-1), now.AddMinutes(4));
        activeSeed.IsExpired.Should().BeFalse();
        activeSeed.RemainingTime.Should().BeGreaterThan(TimeSpan.Zero);
        activeSeed.ValidateExpiration(); // Should not throw

        var expiredSeed = EcfAuthSeed.Reconstitute("SEED123456789", "101000015", now.AddMinutes(-10), now.AddMinutes(-5));
        expiredSeed.IsExpired.Should().BeTrue();
        expiredSeed.RemainingTime.Should().Be(TimeSpan.Zero);
        expiredSeed.IsExpiredAt(now).Should().BeTrue();

        Action act = () => expiredSeed.ValidateExpiration();
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Generate_CreatesValidToken()
    {
        var seed = EcfAuthSeed.Generate("101000015", 15);
        seed.Should().NotBeNull();
        seed.ReceiverRnc.Should().Be("101000015");
        seed.SeedValue.Length.Should().BeGreaterThanOrEqualTo(8);
        seed.IsExpired.Should().BeFalse();
    }

    [Fact]
    public void FromXml_And_ToString_WorkAsExpected()
    {
        var now = DateTimeOffset.UtcNow;
        var seed = EcfAuthSeed.FromXml("SEED123456789", "101000015", now, now.AddMinutes(5));
        seed.Should().NotBeNull();
        seed.ToString().Should().Contain("EcfAuthSeed[");
        seed.ToString().Should().Contain("101000015");

        EcfAuthSeed.AmbientTimeProvider = TimeProvider.System;
        EcfAuthSeed.AmbientTimeProvider.Should().Be(TimeProvider.System);

        var generatedWithProvider = EcfAuthSeed.Generate("101000015", 5, TimeProvider.System);
        generatedWithProvider.Should().NotBeNull();

        var generatedWithoutProvider = EcfAuthSeed.Generate("101000015", 5, null);
        generatedWithoutProvider.Should().NotBeNull();
    }
}
