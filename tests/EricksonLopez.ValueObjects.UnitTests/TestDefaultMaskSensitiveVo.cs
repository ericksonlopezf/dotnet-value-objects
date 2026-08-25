// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;

namespace EricksonLopez.ValueObjects.UnitTests;

[SensitiveData]
public sealed record TestDefaultMaskSensitiveVo : SingleValueObject<TestDefaultMaskSensitiveVo, string>
{
    protected override bool IsSensitive => true;

    public TestDefaultMaskSensitiveVo(string value) : base(value)
    {
    }
}


