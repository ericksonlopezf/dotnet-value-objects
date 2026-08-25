// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;

namespace EricksonLopez.ValueObjects.EntityFrameworkCore.IntegrationTests;

public sealed record PublicTestIntVoWithTwoCreateMethods : SingleValueObject<PublicTestIntVoWithTwoCreateMethods, int>
{
    private PublicTestIntVoWithTwoCreateMethods(int value) : base(value) { }

    public static Result<PublicTestIntVoWithTwoCreateMethods> Create() =>
        Result<PublicTestIntVoWithTwoCreateMethods>.Success(new PublicTestIntVoWithTwoCreateMethods(0));

    public static Result<PublicTestIntVoWithTwoCreateMethods> Create(int value) =>
        Result<PublicTestIntVoWithTwoCreateMethods>.Success(new PublicTestIntVoWithTwoCreateMethods(value));
}
