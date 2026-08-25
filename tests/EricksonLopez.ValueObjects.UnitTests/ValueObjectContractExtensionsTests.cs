// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

public sealed class ValueObjectContractExtensionsTests
{
    [Fact]
    public void ShouldSatisfyEqualityContract_WhenValidInstancesProvided_Succeeds()
    {
        var email1 = Email.Create("user@example.com").Value;
        var email2 = Email.Create("user@example.com").Value;
        var email3 = Email.Create("other@example.com").Value;

        email1.ShouldSatisfyEqualityContract(
            equalInstance: email2,
            differentInstance: email3,
            equalityOperator: (a, b) => a == b,
            inequalityOperator: (a, b) => a != b);
    }

    [Fact]
    public void ShouldSatisfyEqualityContract_WhenOperatorsOmitted_Succeeds()
    {
        var tenant1 = TenantCode.Create("tenant-alpha").Value;
        var tenant2 = TenantCode.Create("tenant-alpha").Value;
        var tenant3 = TenantCode.Create("tenant-beta").Value;

        tenant1.ShouldSatisfyEqualityContract(tenant2, tenant3);
    }

    [Fact]
    public void ShouldSatisfyComparisonContract_WhenOrderedInstancesProvided_Succeeds()
    {
        var small = CurrencyCode.Create("BHD").Value;
        var smallEqual = CurrencyCode.Create("BHD").Value;
        var large = CurrencyCode.Create("USD").Value;

        small.ShouldSatisfyComparisonContract(
            equalToSmaller: smallEqual,
            greater: large,
            lessThan: (a, b) => a < b,
            lessThanOrEqual: (a, b) => a <= b,
            greaterThan: (a, b) => a > b,
            greaterThanOrEqual: (a, b) => a >= b);
    }
}



