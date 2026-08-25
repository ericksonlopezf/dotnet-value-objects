// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="FullName"/> Value Object.
/// </summary>
public sealed class FullNameTests
{
    [Fact]
    public void Create_WhenComposedWithMiddleName_FormatsCorrectly()
    {
        var first = FirstName.Create("Erickson").Value;
        var middle = MiddleName.Create("Alexander").Value;
        var last = LastName.Create("Lopez").Value;

        var fullWithMiddle = FullName.Create(first, last, middle).Value;
        var fullWithoutMiddle = FullName.Create(first, last).Value;

        fullWithMiddle.FirstName.Should().Be(first);
        fullWithMiddle.MiddleName.Should().Be(middle);
        fullWithMiddle.LastName.Should().Be(last);
        fullWithoutMiddle.MiddleName.Should().BeNull();

        fullWithMiddle.Value.Should().Be("Erickson Alexander Lopez");
        fullWithoutMiddle.Value.Should().Be("Erickson Lopez");
        fullWithMiddle.ToString().Should().Be("Erickson Alexander Lopez");

        var nullFirst = FullName.Create((FirstName)null!, last);
        nullFirst.IsFailure.Should().BeTrue();
        nullFirst.Error.Code.Should().Be("FullName.FirstNameRequired");

        var nullLast = FullName.Create(first, (LastName)null!);
        nullLast.IsFailure.Should().BeTrue();
        nullLast.Error.Code.Should().Be("FullName.LastNameRequired");

        var fromString = FullName.Create("John", "Doe", "Alexander");
        fromString.IsSuccess.Should().BeTrue();
        fromString.Value.Value.Should().Be("John Alexander Doe");

        var err1 = FullName.Create((string)null!, "Doe");
        err1.IsFailure.Should().BeTrue();
        err1.Error.Code.Should().Be("FirstName.Required");

        var err2 = FullName.Create("John", (string)null!);
        err2.IsFailure.Should().BeTrue();
        err2.Error.Code.Should().Be("LastName.Required");

        var err3 = FullName.Create("John", "Doe", "Alex123");
        err3.IsFailure.Should().BeTrue();
        err3.Error.Code.Should().Be("MiddleName.InvalidFormat");
    }

    [Fact]
    public void Create_WhenUsingStringOverloadsAndNullChecks_EnforcesInvariants()
    {
        var valid = FullName.Create("Erickson", "Lopez", "Alexander");
        valid.IsSuccess.Should().BeTrue();
        valid.Value.Value.Should().Be("Erickson Alexander Lopez");

        var validNoMiddle = FullName.Create("Erickson", "Lopez", null);
        validNoMiddle.IsSuccess.Should().BeTrue();
        validNoMiddle.Value.Value.Should().Be("Erickson Lopez");

        var nullStrFirst = FullName.Create((string?)null, "Lopez");
        nullStrFirst.IsFailure.Should().BeTrue();
        nullStrFirst.Error.Code.Should().Be("FirstName.Required");

        var nullStrLast = FullName.Create("Erickson", (string?)null);
        nullStrLast.IsFailure.Should().BeTrue();
        nullStrLast.Error.Code.Should().Be("LastName.Required");

        var invMid1 = FullName.Create("Erickson", "Lopez", "Invalid123Middle");
        invMid1.IsFailure.Should().BeTrue();
        invMid1.Error.Code.Should().Be("MiddleName.InvalidFormat");

        var invMid2 = FullName.Create("Erickson", "Lopez", "Invalid\0Middle");
        invMid2.IsFailure.Should().BeTrue();
        invMid2.Error.Code.Should().Be("MiddleName.ControlCharacters");

        var nullVoFirst = FullName.Create((FirstName?)null, LastName.Create("Lopez").Value);
        nullVoFirst.IsFailure.Should().BeTrue();
        nullVoFirst.Error.Code.Should().Be("FullName.FirstNameRequired");

        var nullVoLast = FullName.Create(FirstName.Create("Erickson").Value, (LastName?)null);
        nullVoLast.IsFailure.Should().BeTrue();
        nullVoLast.Error.Code.Should().Be("FullName.LastNameRequired");
    }

    [Fact]
    public void EqualityContract_WhenValidFullNames_SatisfiesContract()
    {
        var fn1 = FullName.Create("Erickson", "Lopez", "Alexander").Value;
        var fn1Copy = FullName.Create("Erickson", "Lopez", "Alexander").Value;
        var fn2 = FullName.Create("John", "Doe").Value;

        fn1.ShouldSatisfyEqualityContract(fn1Copy, fn2, (a, b) => a == b, (a, b) => a != b);
    }

    [Fact]
    public void Builder_WhenConfigured_BuildsValidInstance()
    {
        var builder = new FullNameBuilder()
            .WithFirstName("Jane")
            .WithLastName("Doe")
            .WithMiddleName("Marie");

        var fullName = builder.Build();

        fullName.FirstName.Value.Should().Be("Jane");
        fullName.LastName.Value.Should().Be("Doe");
        fullName.MiddleName?.Value.Should().Be("Marie");
        fullName.Value.Should().Be("Jane Marie Doe");
    }

    [Fact]
    public void Builder_WhenMissingRequiredField_ThrowsInvalidOperationException()
    {
        var builder = new FullNameBuilder()
            .WithFirstName(null);

        var act = () => builder.Build();

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*FirstName.Required*");
    }
}



