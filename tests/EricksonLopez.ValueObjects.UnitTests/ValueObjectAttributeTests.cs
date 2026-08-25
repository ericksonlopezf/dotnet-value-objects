// Copyright © Erickson Lopez. MIT License.
using AwesomeAssertions;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

public sealed class ValueObjectAttributeTests
{
    [Fact]
    public void Properties_WhenDefaultAndModified_ShouldHaveExpectedValues()
    {
        var attr = new ValueObjectAttribute();

        attr.GenerateConversionOperators.Should().BeFalse();
        attr.GeneratePersistenceHooks.Should().BeTrue();

        attr.GenerateConversionOperators = true;
        attr.GeneratePersistenceHooks = false;

        attr.GenerateConversionOperators.Should().BeTrue();
        attr.GeneratePersistenceHooks.Should().BeFalse();
    }
}



