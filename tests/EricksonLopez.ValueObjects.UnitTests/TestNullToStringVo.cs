// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.ValueObjects;

namespace EricksonLopez.ValueObjects.UnitTests;

public sealed record TestNullToStringVo : SingleValueObject<TestNullToStringVo, TestNullToStringPayload>
{
    public TestNullToStringVo(TestNullToStringPayload value) : base(value)
    {
    }
}
