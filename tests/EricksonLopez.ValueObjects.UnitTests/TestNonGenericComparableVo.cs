// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;

namespace EricksonLopez.ValueObjects.UnitTests;

public sealed record TestNonGenericComparableVo : SingleValueObject<TestNonGenericComparableVo, TestNonGenericComparablePayload>
{
    public TestNonGenericComparableVo(TestNonGenericComparablePayload value) : base(value)
    {
    }
}


