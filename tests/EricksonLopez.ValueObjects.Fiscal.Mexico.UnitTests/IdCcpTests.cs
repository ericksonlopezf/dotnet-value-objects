// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Mexico;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Mexico.UnitTests;

public sealed class IdCcpTests
{
    private const string ValidUuidStr = "a3bb189e-8bf9-4888-9912-ace4e6543002";
    private const string ValidIdCcpStr = "CCCa3bb189e-8bf9-4888-9912-ace4e6543002";
    private static readonly Guid ValidGuid = Guid.Parse(ValidUuidStr);

    [Fact]
    public void Create_FromFiscalUuid_Succeeds()
    {
        var uuid = FiscalUuid.Create(ValidGuid).Value;
        var result = IdCcp.Create(uuid);

        result.IsSuccess.Should().BeTrue();
        result.Value.Uuid.Should().Be(uuid);
        result.Value.Formatted.Should().Be($"CCC{ValidUuidStr.ToUpperInvariant()}");
        result.Value.ToString().Should().Be($"CCC{ValidUuidStr.ToUpperInvariant()}");
    }

    [Theory]
    [InlineData("CCCa3bb189e-8bf9-4888-9912-ace4e6543002")]
    [InlineData("ccca3bb189e-8bf9-4888-9912-ace4e6543002")]
    [InlineData("  CCCa3bb189e-8bf9-4888-9912-ace4e6543002  ")]
    public void Create_FromValidString_Succeeds(string input)
    {
        var result = IdCcp.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Formatted.Should().Be($"CCC{ValidUuidStr.ToUpperInvariant()}");
    }

    [Theory]
    [InlineData("CCCa3bb189e-8bf9-4888-9912-ace4e654300")]   // 38
    [InlineData("CCCa3bb189e-8bf9-4888-9912-ace4e65430022")] // 40
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_InvalidLength_ReturnsError(string? input)
    {
        var result = IdCcp.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("IdCcp.InvalidLength");
    }

    [Theory]
    [InlineData("ABCa3bb189e-8bf9-4888-9912-ace4e6543002")]
    [InlineData("123a3bb189e-8bf9-4888-9912-ace4e6543002")]
    public void Create_InvalidPrefix_ReturnsError(string input)
    {
        var result = IdCcp.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("IdCcp.InvalidPrefix");
    }

    [Fact]
    public void Create_InvalidUuidComponent_ReturnsError()
    {
        var resultEmpty = IdCcp.Create("CCC00000000-0000-0000-0000-000000000000");
        resultEmpty.IsFailure.Should().BeTrue();
        resultEmpty.Error.Code.Should().Be("FiscalUuid.Empty");

        var resultInvalid = IdCcp.Create("CCCzzzzzzzz-zzzz-zzzz-zzzz-zzzzzzzzzzzz");
        resultInvalid.IsFailure.Should().BeTrue();
        resultInvalid.Error.Code.Should().Be("FiscalUuid.InvalidFormat");
    }

    [Fact]
    public void IdCcp_DefaultState_ComparisonOperators()
    {
        var id1 = IdCcp.Create("CCC00000000-0000-0000-0000-000000000001").Value;
        var id2 = IdCcp.Create("CCC00000000-0000-0000-0000-000000000002").Value;
        var id1Clone = IdCcp.Create("ccc00000000-0000-0000-0000-000000000001").Value;

        (id1 < id2).Should().BeTrue();
        (id1 <= id2).Should().BeTrue();
        (id2 > id1).Should().BeTrue();
        (id2 >= id1).Should().BeTrue();

        (id1 < id1Clone).Should().BeFalse();
        (id1 > id1Clone).Should().BeFalse();
        (id1 <= id1Clone).Should().BeTrue();
        (id1 >= id1Clone).Should().BeTrue();
        id1.CompareTo(id2).Should().BeNegative();
        id2.CompareTo(id1).Should().BePositive();
        id1.CompareTo(id1Clone).Should().Be(0);
    }

    [Fact]
    public void IdCcp_DefaultState_ParseAndTryParse()
    {
        var parsed1 = IdCcp.Parse(ValidIdCcpStr, CultureInfo.InvariantCulture);
        parsed1.Formatted.Should().Be($"CCC{ValidUuidStr.ToUpperInvariant()}");

        var parsed2 = IdCcp.Parse(ValidIdCcpStr.AsSpan(), CultureInfo.InvariantCulture);
        parsed2.Formatted.Should().Be($"CCC{ValidUuidStr.ToUpperInvariant()}");

        IdCcp.TryParse(ValidIdCcpStr, null, out var tryRes1).Should().BeTrue();
        tryRes1.Formatted.Should().Be($"CCC{ValidUuidStr.ToUpperInvariant()}");

        IdCcp.TryParse(ValidIdCcpStr.AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Formatted.Should().Be($"CCC{ValidUuidStr.ToUpperInvariant()}");

        Action invalidParseStr = () => IdCcp.Parse("invalid", CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid IdCcp: 'invalid'.");

        Action invalidParseSpan = () => IdCcp.Parse("invalid".AsSpan(), CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid IdCcp: 'invalid'.");

        IdCcp.TryParse("invalid", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(IdCcp));

        IdCcp.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(IdCcp));

        IdCcp.TryParse("invalid".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(IdCcp));
    }
}




