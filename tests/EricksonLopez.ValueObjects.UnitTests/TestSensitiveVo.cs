// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;

namespace EricksonLopez.ValueObjects.UnitTests;

[SensitiveData(mask: "[REDACTED-SECRET]")]
public sealed record TestSensitiveVo : SingleValueObject<TestSensitiveVo, string>
{
    protected override bool IsSensitive => true;
    protected override string Mask => "[REDACTED-SECRET]";

    public TestSensitiveVo(string value) : base(value)
    {
    }
}


