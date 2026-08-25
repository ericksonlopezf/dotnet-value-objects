// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.ValueObjects.Attributes;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

using AwesomeAssertions;
using EricksonLopez.ValueObjects.Attributes;
using Xunit;

public sealed class RegulatoryRuleAttributeTests
{
    [Fact]
    public void Constructor_WithValidRuleId_SetsProperty()
    {
        var attr = new RegulatoryRuleAttribute("AR.CUIT.001");

        attr.RuleId.Should().Be("AR.CUIT.001");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithNullOrWhitespaceRuleId_ThrowsArgumentException(string? invalidRuleId)
    {
        Action act = () => _ = new RegulatoryRuleAttribute(invalidRuleId!);

        act.Should().Throw<ArgumentException>()
            .WithParameterName("ruleId")
            .WithMessage("Value cannot be null or whitespace.*");
    }

    [Fact]
    public void AttributeUsage_ShouldTargetStructAndClass_AndAllowMultiple()
    {
        var usage = (AttributeUsageAttribute)Attribute.GetCustomAttribute(
            typeof(RegulatoryRuleAttribute), typeof(AttributeUsageAttribute))!;

        usage.Should().NotBeNull();
        usage.ValidOn.Should().Be(AttributeTargets.Struct | AttributeTargets.Class);
        usage.AllowMultiple.Should().BeTrue();
        usage.Inherited.Should().BeFalse();
    }
}




