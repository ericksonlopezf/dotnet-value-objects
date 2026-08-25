// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;

namespace EricksonLopez.ValueObjects.Serialization.Json.IntegrationTests;

public sealed record TestIntScalarVo : SingleValueObject<TestIntScalarVo, int>
{
    private TestIntScalarVo(int value) : base(value)
    {
    }

    public static Result<TestIntScalarVo> Create(int value)
    {
        if (value < 0)
        {
            return Result<TestIntScalarVo>.Failure(Error.Validation("TestIntScalarVo.Invalid", "Must be non-negative"));
        }

        return Result<TestIntScalarVo>.Success(new TestIntScalarVo(value));
    }
}


