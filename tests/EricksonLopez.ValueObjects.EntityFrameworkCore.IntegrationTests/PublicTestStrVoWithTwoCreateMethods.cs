// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;

namespace EricksonLopez.ValueObjects.EntityFrameworkCore.IntegrationTests;

public sealed record PublicTestStrVoWithTwoCreateMethods : StringValueObject<PublicTestStrVoWithTwoCreateMethods>
{
    private PublicTestStrVoWithTwoCreateMethods(string value) : base(value) { }

    public static Result<PublicTestStrVoWithTwoCreateMethods> Create() =>
        Result<PublicTestStrVoWithTwoCreateMethods>.Success(new PublicTestStrVoWithTwoCreateMethods("default"));

    public static Result<PublicTestStrVoWithTwoCreateMethods> Create(string value) =>
        Result<PublicTestStrVoWithTwoCreateMethods>.Success(new PublicTestStrVoWithTwoCreateMethods(value));
}
