// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;

namespace EricksonLopez.ValueObjects.UnitTests;

public sealed record TestNullableStringVo : SingleValueObject<TestNullableStringVo, string>
{
    public TestNullableStringVo(string value) : base(value)
    {
    }

    public static Result<TestNullableStringVo> Create(string value)
    {
        return Result<TestNullableStringVo>.Success(new TestNullableStringVo(value));
    }
}



