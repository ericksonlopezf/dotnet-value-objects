// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.ValueObjects;

namespace EricksonLopez.ValueObjects.EntityFrameworkCore.IntegrationTests;

public sealed record PublicTestStrVoPublicCtorOnly : StringValueObject<PublicTestStrVoPublicCtorOnly>
{
    public PublicTestStrVoPublicCtorOnly(string value) : base(value) { }
}
