// Copyright © Erickson Lopez. MIT License.
using System;
using System.Data;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Dapper;
using EricksonLopez.ValueObjects.UnitTests;
using NSubstitute;
using Xunit;

namespace EricksonLopez.ValueObjects.Dapper.IntegrationTests;

public sealed class SingleValueObjectTypeHandlerTests
{
    [Fact]
    public void Constructor_WhenFactoryIsNull_ThrowsArgumentNullException()
    {
        Action act = () => _ = new SingleValueObjectTypeHandler<TestIntScalarVo, int>(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void SetValue_WhenValueIsNull_SetsDBNull()
    {
        var handler = new SingleValueObjectTypeHandler<TestIntScalarVo, int>(TestIntScalarVo.Create);
        var parameter = Substitute.For<IDbDataParameter>();

        handler.SetValue(parameter, null);

        parameter.Value.Should().Be(DBNull.Value);
    }

    [Fact]
    public void SetValue_WhenValueIsNotNull_SetsPrimitiveValue()
    {
        var handler = new SingleValueObjectTypeHandler<TestIntScalarVo, int>(TestIntScalarVo.Create);
        var parameter = Substitute.For<IDbDataParameter>();
        var vo = TestIntScalarVo.Create(42).Value;

        handler.SetValue(parameter, vo);

        parameter.Value.Should().Be(42);
    }

    [Fact]
    public void SetValue_WhenParameterIsNull_ThrowsArgumentNullException()
    {
        var handler = new SingleValueObjectTypeHandler<TestIntScalarVo, int>(TestIntScalarVo.Create);
        var vo = TestIntScalarVo.Create(42).Value;

        Action act = () => handler.SetValue(null!, vo);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Parse_WhenValueIsNull_ReturnsNull()
    {
        var handler = new SingleValueObjectTypeHandler<TestIntScalarVo, int>(TestIntScalarVo.Create);

        var result = handler.Parse(null!);

        result.Should().BeNull();
    }

    [Fact]
    public void Parse_WhenValueIsDBNull_ReturnsNull()
    {
        var handler = new SingleValueObjectTypeHandler<TestIntScalarVo, int>(TestIntScalarVo.Create);

        var result = handler.Parse(DBNull.Value);

        result.Should().BeNull();
    }

    [Fact]
    public void Parse_WhenValueIsExactPrimitiveType_ReturnsInstance()
    {
        var handler = new SingleValueObjectTypeHandler<TestIntScalarVo, int>(TestIntScalarVo.Create);

        var result = handler.Parse(100);

        result.Should().NotBeNull();
        result!.Value.Should().Be(100);
    }

    [Fact]
    public void Parse_WhenExactPrimitiveFailsValidation_ThrowsDataException()
    {
        var handler = new SingleValueObjectTypeHandler<TestIntScalarVo, int>(TestIntScalarVo.Create);

        Action act = () => handler.Parse(-10);

        act.Should().Throw<DataException>()
            .WithMessage("*Failed to map database value '-10' to 'TestIntScalarVo'*");
    }

    [Fact]
    public void Parse_WhenValueIsConvertibleType_ConvertsAndReturnsInstance()
    {
        var handler = new SingleValueObjectTypeHandler<TestIntScalarVo, int>(TestIntScalarVo.Create);

        var resultFromLong = handler.Parse(250L);
        var resultFromString = handler.Parse("350");

        resultFromLong.Should().NotBeNull();
        resultFromLong!.Value.Should().Be(250);
        resultFromString.Should().NotBeNull();
        resultFromString!.Value.Should().Be(350);
    }

    [Fact]
    public void Parse_WhenConvertedValueFailsValidation_ThrowsDataException()
    {
        var handler = new SingleValueObjectTypeHandler<TestIntScalarVo, int>(TestIntScalarVo.Create);

        Action act = () => handler.Parse("-999");

        act.Should().Throw<DataException>()
            .WithMessage("*Failed to map database value '-999' to 'TestIntScalarVo'*");
    }
}




