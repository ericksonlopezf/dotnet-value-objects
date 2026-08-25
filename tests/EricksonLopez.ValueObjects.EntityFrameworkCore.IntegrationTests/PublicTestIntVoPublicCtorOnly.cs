// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.ValueObjects;

namespace EricksonLopez.ValueObjects.EntityFrameworkCore.IntegrationTests;

public sealed record PublicTestIntVoPublicCtorOnly : SingleValueObject<PublicTestIntVoPublicCtorOnly, int>
{
    public PublicTestIntVoPublicCtorOnly(int value) : base(value) { }
}
