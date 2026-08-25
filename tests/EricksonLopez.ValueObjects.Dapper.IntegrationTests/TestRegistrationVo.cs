// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;

namespace EricksonLopez.ValueObjects.Dapper.IntegrationTests;

public sealed record TestRegistrationVo : SingleValueObject<TestRegistrationVo, int>
{
    private TestRegistrationVo(int value) : base(value)
    {
    }

    public static Result<TestRegistrationVo> Create(int value)
    {
        return Result<TestRegistrationVo>.Success(new TestRegistrationVo(value));
    }
}


