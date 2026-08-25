// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.DomainPrimitives;
using EricksonLopez.DomainPrimitives.Validation;
using EricksonLopez.Result;
using Xunit;

namespace EricksonLopez.ValueObjects.DomainPrimitives.UnitTests;

public sealed class ValueObjectDomainPrimitiveTests
{
    private sealed record TestCustomerCode(string Value) : SingleValueObject<TestCustomerCode, string>(Value);

    private readonly struct MockPrimitive : IDomainPrimitive<MockPrimitive, string>
    {
        public string Value { get; }
        public bool IsDefault => Value is null;
        public static string PrimitiveName => "MockPrimitive";

        public MockPrimitive(string value) => Value = value;

        public static MockPrimitive Create(string value) =>
            TryCreate(value, out var res, out var err) ? res : throw new InvalidOperationException(err.Message);

        public static bool TryCreate(string value, out MockPrimitive result, out PrimitiveError validationError)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length < 3)
            {
                result = default;
                validationError = PrimitiveError.Create("CODE_INVALID", "Code must be at least 3 characters.");
                return false;
            }

            result = new MockPrimitive(value);
            validationError = PrimitiveError.None;
            return true;
        }
    }

    private readonly struct MockPrimitiveSilentFailure : IDomainPrimitive<MockPrimitiveSilentFailure, string>
    {
        public string Value { get; }
        public bool IsDefault => Value is null;
        public static string PrimitiveName => "MockPrimitiveSilentFailure";

        public MockPrimitiveSilentFailure(string value) => Value = value;

        public static MockPrimitiveSilentFailure Create(string value) =>
            TryCreate(value, out var res, out var err) ? res : throw new InvalidOperationException(err.Message);

        public static bool TryCreate(string value, out MockPrimitiveSilentFailure result, out PrimitiveError validationError)
        {
            result = default;
            validationError = PrimitiveError.None; // Simulates failure returning PrimitiveError.None to test fallback
            return false;
        }
    }

    private readonly struct MockStrongId : IStrongId<MockStrongId, string>
    {
        public string Value { get; }
        public bool IsDefault => Value is null;
        public static string PrimitiveName => "MockStrongId";
        public static MockStrongId Empty => new(string.Empty);

        public MockStrongId(string value) => Value = value;

        public static MockStrongId Create() => new(Guid.NewGuid().ToString("N"));

        public static MockStrongId Create(string value) =>
            TryCreate(value, out var res, out var err) ? res : throw new InvalidOperationException(err.Message);

        public static bool TryCreate(string value, out MockStrongId result, out PrimitiveError validationError)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result = default;
                validationError = PrimitiveError.Create("ID_EMPTY", "ID cannot be empty.");
                return false;
            }

            result = new MockStrongId(value);
            validationError = PrimitiveError.None;
            return true;
        }
    }

    private readonly struct MockStrongIdSilentFailure : IStrongId<MockStrongIdSilentFailure, string>
    {
        public string Value { get; }
        public bool IsDefault => Value is null;
        public static string PrimitiveName => "MockStrongIdSilentFailure";
        public static MockStrongIdSilentFailure Empty => new(string.Empty);

        public MockStrongIdSilentFailure(string value) => Value = value;

        public static MockStrongIdSilentFailure Create() => new(Guid.NewGuid().ToString("N"));

        public static MockStrongIdSilentFailure Create(string value) =>
            TryCreate(value, out var res, out var err) ? res : throw new InvalidOperationException(err.Message);

        public static bool TryCreate(string value, out MockStrongIdSilentFailure result, out PrimitiveError validationError)
        {
            result = default;
            validationError = PrimitiveError.None; // Simulates failure returning PrimitiveError.None to test fallback
            return false;
        }
    }

    [Fact]
    public void PrimitiveError_ToError_WhenValidError_ConvertsCorrectly()
    {
        var primErr = PrimitiveError.Create("ERR_CODE", "Error description");
        var error = primErr.ToError();

        error.Should().NotBeNull();
        error!.Code.Should().Be("ERR_CODE");
        error.Description.Should().Be("Error description");
        error.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public void PrimitiveError_ToError_WhenMessageIsNull_UsesFallbackMessage()
    {
        var primErr = new PrimitiveError("ERR_CODE", null);
        var error = primErr.ToError();

        error.Should().NotBeNull();
        error!.Code.Should().Be("ERR_CODE");
        error.Description.Should().Be("A domain primitive validation error occurred.");
    }

    [Fact]
    public void PrimitiveError_ToError_WhenCodeIsNull_ReturnsNull()
    {
        var primErr = new PrimitiveError(null, "Error description");
        var error = primErr.ToError();

        error.Should().BeNull();
    }

    [Fact]
    public void PrimitiveError_None_ToError_ReturnsNull()
    {
        var primErr = PrimitiveError.None;
        var error = primErr.ToError();

        error.Should().BeNull();
    }

    [Fact]
    public void Error_ToPrimitiveError_WhenValidError_ConvertsCorrectly()
    {
        var error = Error.Validation("ERR_CODE", "Error description");
        var primErr = error.ToPrimitiveError();

        primErr.IsError.Should().BeTrue();
        primErr.Code.Should().Be("ERR_CODE");
        primErr.Message.Should().Be("Error description");
    }

    [Fact]
    public void Error_Null_ToPrimitiveError_ReturnsNone()
    {
        var primErr = ((Error?)null).ToPrimitiveError();

        primErr.IsError.Should().BeFalse();
        primErr.Should().Be(PrimitiveError.None);
    }

    [Fact]
    public void ToDomainPrimitive_WhenValid_ReturnsSuccess()
    {
        var vo = new TestCustomerCode("CUST-123");
        var result = vo.ToDomainPrimitive<TestCustomerCode, string, MockPrimitive>();

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("CUST-123");
    }

    [Fact]
    public void ToDomainPrimitive_WhenInvalid_ReturnsFailure()
    {
        var vo = new TestCustomerCode("AB");
        var result = vo.ToDomainPrimitive<TestCustomerCode, string, MockPrimitive>();

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("CODE_INVALID");
    }

    [Fact]
    public void ToDomainPrimitive_WhenValueObjectIsNull_ThrowsArgumentNullException()
    {
        TestCustomerCode? vo = null;
        var act = () => vo!.ToDomainPrimitive<TestCustomerCode, string, MockPrimitive>();

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToDomainPrimitive_WhenPrimitiveFailsWithNoneError_ReturnsDefaultValidationError()
    {
        var vo = new TestCustomerCode("TEST");
        var result = vo.ToDomainPrimitive<TestCustomerCode, string, MockPrimitiveSilentFailure>();

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("DomainPrimitive.Invalid");
        result.Error.Description.Should().Be("Invalid domain primitive value.");
    }

    [Fact]
    public void ToStrongId_WhenValid_ReturnsSuccess()
    {
        var vo = new TestCustomerCode("ID-999");
        var result = vo.ToStrongId<TestCustomerCode, string, MockStrongId>();

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("ID-999");
    }

    [Fact]
    public void ToStrongId_WhenInvalid_ReturnsFailure()
    {
        var vo = new TestCustomerCode("");
        var result = vo.ToStrongId<TestCustomerCode, string, MockStrongId>();

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("ID_EMPTY");
    }

    [Fact]
    public void ToStrongId_WhenValueObjectIsNull_ThrowsArgumentNullException()
    {
        TestCustomerCode? vo = null;
        var act = () => vo!.ToStrongId<TestCustomerCode, string, MockStrongId>();

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToStrongId_WhenStrongIdFailsWithNoneError_ReturnsDefaultValidationError()
    {
        var vo = new TestCustomerCode("TEST");
        var result = vo.ToStrongId<TestCustomerCode, string, MockStrongIdSilentFailure>();

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("StrongId.Invalid");
        result.Error.Description.Should().Be("Invalid strong ID value.");
    }
}
