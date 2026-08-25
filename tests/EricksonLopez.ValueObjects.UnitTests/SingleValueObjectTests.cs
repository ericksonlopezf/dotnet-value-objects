// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

public sealed class SingleValueObjectTests
{
    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenValueIsNull()
    {
        Action act = () => _ = new TestNullableStringVo(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Equals_DefaultState_ShouldEvaluateEqualityCorrectly()
    {
        var vo1 = TestIntScalarVo.Create(10).Value;
        var vo2 = TestIntScalarVo.Create(10).Value;
        var vo3 = TestIntScalarVo.Create(20).Value;

        vo1.Should().NotBeNull();
        vo1!.Equals(vo2).Should().BeTrue();
        vo1!.Equals(vo1).Should().BeTrue();
        vo1!.Equals(null).Should().BeFalse();
        vo1!.Equals(vo3).Should().BeFalse();

        SingleValueObject<TestIntScalarVo, int> baseVo1 = vo1!;
        SingleValueObject<TestIntScalarVo, int> baseVo2 = vo2!;
        baseVo1.Equals(vo2).Should().BeTrue();
        baseVo1.Equals((TestIntScalarVo?)null).Should().BeFalse();
        baseVo1.Equals(vo1).Should().BeTrue();
        baseVo1.Equals(vo3).Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_ShouldBeEqual_ForMatchingValues()
    {
        var vo1 = TestIntScalarVo.Create(100).Value;
        var vo2 = TestIntScalarVo.Create(100).Value;

        vo1.Should().NotBeNull();
        vo2.Should().NotBeNull();
        vo1!.GetHashCode().Should().Be(vo2!.GetHashCode());
    }

    [Fact]
    public void CompareTo_Typed_ShouldOrderCorrectly()
    {
        var vo1 = TestIntScalarVo.Create(10).Value;
        var vo2 = TestIntScalarVo.Create(20).Value;

        vo1.CompareTo(vo2).Should().BeNegative();
        vo2.CompareTo(vo1).Should().BePositive();
        vo1.CompareTo(vo1).Should().Be(0);
        vo1.CompareTo(null).Should().Be(1);
    }

    [Fact]
    public void CompareTo_Typed_ShouldSupportNonGenericIComparable()
    {
        var vo1 = new TestNonGenericComparableVo(new TestNonGenericComparablePayload(10));
        var vo2 = new TestNonGenericComparableVo(new TestNonGenericComparablePayload(20));

        vo1.CompareTo(vo2).Should().BeNegative();
        vo2.CompareTo(vo1).Should().BePositive();
    }

    [Fact]
    public void CompareTo_Typed_ShouldThrowNotSupportedException_WhenTypeIsNotComparable()
    {
        var vo1 = new TestNonComparableVo(new TestNonComparablePayload("a"));
        var vo2 = new TestNonComparableVo(new TestNonComparablePayload("b"));

        Action act = () => vo1.CompareTo(vo2);

        act.Should().Throw<NotSupportedException>()
            .WithMessage("*does not implement IComparable*");
    }

    [Fact]
    public void CompareTo_Typed_ShouldSupportGenericOnlyIComparable()
    {
        var vo1 = new TestGenericOnlyComparableVo(new TestGenericOnlyComparablePayload(10));
        var vo2 = new TestGenericOnlyComparableVo(new TestGenericOnlyComparablePayload(20));

        vo1.CompareTo(vo2).Should().BeNegative();
        vo2.CompareTo(vo1).Should().BePositive();
    }

    [Fact]
    public void CompareTo_Object_ShouldOrderCorrectlyOrThrow()
    {
        var vo1 = TestIntScalarVo.Create(10).Value;
        var vo2 = TestIntScalarVo.Create(20).Value;

        vo1.CompareTo((object?)vo2).Should().BeNegative();
        vo1.CompareTo((object?)null).Should().Be(1);

        Action act = () => vo1.CompareTo("not-a-vo");
        act.Should().Throw<ArgumentException>()
            .WithMessage($"Object must be of type {nameof(TestIntScalarVo)}*");
    }

    [Fact]
    public void ToString_WhenValueToStringReturnsNull_ReturnsEmptyString()
    {
        var vo = new TestNullToStringVo(new TestNullToStringPayload());
        vo.ToString().Should().BeEmpty();
    }

    [Fact]
    public void ComparisonOperators_DefaultState_ShouldEvaluateCorrectly()
    {
        var vo1 = TestIntScalarVo.Create(10).Value;
        var vo1Copy = TestIntScalarVo.Create(10).Value;
        var vo2 = TestIntScalarVo.Create(20).Value;
        TestIntScalarVo? nullVo1 = null;
        TestIntScalarVo? nullVo2 = null;

        (vo1 < vo2).Should().BeTrue();
        (vo2 >= vo1).Should().BeTrue();

        (nullVo1 < vo1).Should().BeTrue();
        (nullVo1 < nullVo2).Should().BeFalse();
        (vo1 < nullVo1).Should().BeFalse();
        (vo1 < vo1Copy).Should().BeFalse();

        (nullVo1 <= vo1).Should().BeTrue();
        (nullVo1 <= nullVo2).Should().BeTrue();
        (vo1 <= nullVo1).Should().BeFalse();
        (vo2 <= vo1).Should().BeFalse();
        (vo1 <= vo1Copy).Should().BeTrue();

        (vo1 > nullVo1).Should().BeTrue();
        (nullVo1 > vo1).Should().BeFalse();
        (nullVo1 > nullVo2).Should().BeFalse();
        (vo1 > vo2).Should().BeFalse();
        (vo1 > vo1Copy).Should().BeFalse();

        (nullVo1 >= nullVo2).Should().BeTrue();
        (nullVo1 >= vo1).Should().BeFalse();
        (vo1 >= nullVo1).Should().BeTrue();
        (vo1 >= vo2).Should().BeFalse();
        (vo1 >= vo1Copy).Should().BeTrue();
    }

    [Fact]
    public void ToString_ShouldReturnStringRepresentation_WhenNotSensitive()
    {
        var vo = TestIntScalarVo.Create(42).Value;

        vo.ToString().Should().Be("42");
    }

    [Fact]
    public void ToString_ShouldMaskValue_WhenDecoratedWithSensitiveDataAttribute()
    {
        var vo = new TestSensitiveVo("super-secret-password");
        vo.ToString().Should().Be("[REDACTED-SECRET]");

        var defaultMaskVo = new TestDefaultMaskSensitiveVo("another-secret");
        defaultMaskVo.ToString().Should().Be("***");
    }

    [Fact]
    public void ExplicitCast_ShouldReturnPrimitiveValue_WhenNotNull()
    {
        var vo = TestIntScalarVo.Create(99).Value;

        var primitive = (int)vo;

        primitive.Should().Be(99);
    }

    [Fact]
    public void ExplicitCast_ShouldThrowArgumentNullException_WhenNull()
    {
        TestIntScalarVo vo = null!;

        Action act = () => _ = (int)vo;

        act.Should().Throw<ArgumentNullException>();
    }
}



