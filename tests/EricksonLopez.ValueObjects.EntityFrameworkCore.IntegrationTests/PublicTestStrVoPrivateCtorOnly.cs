// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.ValueObjects;

namespace EricksonLopez.ValueObjects.EntityFrameworkCore.IntegrationTests;

public sealed record PublicTestStrVoPrivateCtorOnly : StringValueObject<PublicTestStrVoPrivateCtorOnly>
{
    private PublicTestStrVoPrivateCtorOnly(string value) : base(value) { }
}
