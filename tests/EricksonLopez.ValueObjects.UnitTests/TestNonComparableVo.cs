// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;

namespace EricksonLopez.ValueObjects.UnitTests;

public sealed record TestNonComparableVo : SingleValueObject<TestNonComparableVo, TestNonComparablePayload>
{
    public TestNonComparableVo(TestNonComparablePayload value) : base(value)
    {
    }
}


