// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.ValueObjects;

namespace EricksonLopez.ValueObjects.UnitTests;

public sealed record TestGenericOnlyComparableVo : SingleValueObject<TestGenericOnlyComparableVo, TestGenericOnlyComparablePayload>
{
    public TestGenericOnlyComparableVo(TestGenericOnlyComparablePayload value) : base(value)
    {
    }
}
