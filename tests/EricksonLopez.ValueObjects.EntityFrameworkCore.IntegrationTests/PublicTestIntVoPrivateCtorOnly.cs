// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.ValueObjects;

namespace EricksonLopez.ValueObjects.EntityFrameworkCore.IntegrationTests;

public sealed record PublicTestIntVoPrivateCtorOnly : SingleValueObject<PublicTestIntVoPrivateCtorOnly, int>
{
    private PublicTestIntVoPrivateCtorOnly(int value) : base(value) { }
}
