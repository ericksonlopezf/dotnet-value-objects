// Copyright © Erickson Lopez. MIT License.
using System;
using System.Data;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Dapper;
using NSubstitute;
using Xunit;

namespace EricksonLopez.ValueObjects.Dapper.IntegrationTests;

public sealed class StructValueObjectTypeHandlerTests
{
    [Fact]
    public void Constructor_WhenFactoryIsNull_ThrowsArgumentNullException()
    {
        Action act = () => _ = new StructValueObjectTypeHandler<Quantity, int>(null!, q => q.Value);

        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("factory");
    }

    [Fact]
    public void Constructor_WhenValueSelectorIsNull_ThrowsArgumentNullException()
    {
        Action act = () => _ = new StructValueObjectTypeHandler<Quantity, int>(Quantity.Create, null!);

        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("valueSelector");
    }

    [Fact]
    public void SetValue_WhenParameterIsNull_ThrowsArgumentNullException()
    {
        var handler = new StructValueObjectTypeHandler<Quantity, int>(Quantity.Create, q => q.Value);
        var vo = Quantity.Create(42).Value;

        Action act = () => handler.SetValue(null!, vo);

        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("parameter");
    }

    [Fact]
    public void SetValue_WhenParameterProvided_SetsValueUsingSelector()
    {
        var handler = new StructValueObjectTypeHandler<Quantity, int>(Quantity.Create, q => q.Value);
        var parameter = Substitute.For<IDbDataParameter>();
        var vo = Quantity.Create(42).Value;

        handler.SetValue(parameter, vo);

        parameter.Value.Should().Be(42);
    }

    [Fact]
    public void Parse_WhenValueIsNull_ThrowsDataException()
    {
        var handler = new StructValueObjectTypeHandler<Quantity, int>(Quantity.Create, q => q.Value);

        Action act = () => handler.Parse(null!);

        act.Should().Throw<DataException>()
            .WithMessage("Cannot parse null database value into struct 'Quantity'.");
    }

    [Fact]
    public void Parse_WhenValueIsDBNull_ThrowsDataException()
    {
        var handler = new StructValueObjectTypeHandler<Quantity, int>(Quantity.Create, q => q.Value);

        Action act = () => handler.Parse(DBNull.Value);

        act.Should().Throw<DataException>()
            .WithMessage("Cannot parse null database value into struct 'Quantity'.");
    }

    [Fact]
    public void Parse_WhenValueIsExactPrimitive_ReturnsStructInstance()
    {
        var handler = new StructValueObjectTypeHandler<Quantity, int>(Quantity.Create, q => q.Value);

        var result = handler.Parse(150);

        result.Value.Should().Be(150);
    }

    [Fact]
    public void Parse_WhenExactPrimitiveFailsValidation_ThrowsDataException()
    {
        var handler = new StructValueObjectTypeHandler<Quantity, int>(Quantity.Create, q => q.Value);

        Action act = () => handler.Parse(-5);

        act.Should().Throw<DataException>()
            .WithMessage("*Failed to map database value '-5' to 'Quantity'*");
    }

    [Fact]
    public void Parse_WhenValueIsConvertibleType_ConvertsAndReturnsStructInstance()
    {
        var handler = new StructValueObjectTypeHandler<Quantity, int>(Quantity.Create, q => q.Value);

        var resultFromLong = handler.Parse(200L);
        var resultFromString = handler.Parse("300");

        resultFromLong.Value.Should().Be(200);
        resultFromString.Value.Should().Be(300);
    }

    [Fact]
    public void Parse_WhenConvertedValueFailsValidation_ThrowsDataException()
    {
        var handler = new StructValueObjectTypeHandler<Quantity, int>(Quantity.Create, q => q.Value);

        Action act = () => handler.Parse("-100");

        act.Should().Throw<DataException>()
            .WithMessage("*Failed to map database value '-100' to 'Quantity'*");
    }
}
