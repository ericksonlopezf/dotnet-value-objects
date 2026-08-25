// Copyright © Erickson Lopez. MIT License.
using System;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AwesomeAssertions;
using Dapper;
using EricksonLopez.ValueObjects.Dapper;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Dapper.IntegrationTests;

public sealed class ValueObjectTypeHandlerTests
{
    [Fact]
    public void Register_WhenInvoked_RegistersHandlerWithSqlMapperAndMapsCommandParameters()
    {
        var fields = typeof(SqlMapper).GetFields(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
        foreach (var f in fields)
        {
            if (f.GetValue(null) is IDictionary dict)
            {
                dict.Remove(typeof(TestRegistrationVo));
            }
        }

        ValueObjectTypeHandler.Register<TestRegistrationVo, int>(TestRegistrationVo.Create);

        var foundAny = false;
        foreach (var f in fields)
        {
            if (f.GetValue(null) is IDictionary dict && dict.Contains(typeof(TestRegistrationVo)))
            {
                foundAny = true;
                dict[typeof(TestRegistrationVo)].Should().BeOfType<SingleValueObjectTypeHandler<TestRegistrationVo, int>>();
                break;
            }
        }

        foundAny.Should().BeTrue("A dictionary in Dapper.SqlMapper should contain the newly registered type handler");
    }

    [Fact]
    public void RegisterStruct_WhenInvoked_RegistersStructHandlerWithSqlMapper()
    {
        var fields = typeof(SqlMapper).GetFields(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
        foreach (var f in fields)
        {
            if (f.GetValue(null) is IDictionary dict)
            {
                dict.Remove(typeof(Quantity));
            }
        }

        ValueObjectTypeHandler.RegisterStruct<Quantity, int>(Quantity.Create, q => q.Value);

        var foundAny = false;
        foreach (var f in fields)
        {
            if (f.GetValue(null) is IDictionary dict && dict.Contains(typeof(Quantity)))
            {
                foundAny = true;
                dict[typeof(Quantity)].Should().BeOfType<StructValueObjectTypeHandler<Quantity, int>>();
                break;
            }
        }

        foundAny.Should().BeTrue("A dictionary in Dapper.SqlMapper should contain the newly registered struct type handler");
    }

    [Fact]
    public void Register_WhenConcurrentInvocationsOccur_ExecutesThreadSafely()
    {
        Action act = () => Parallel.For(0, 50, _ =>
        {
            ValueObjectTypeHandler.Register<TestRegistrationVo, int>(TestRegistrationVo.Create);
            ValueObjectTypeHandler.Register<TestIntScalarVo, int>(TestIntScalarVo.Create);
            ValueObjectTypeHandler.RegisterStruct<Quantity, int>(Quantity.Create, q => q.Value);
        });

        act.Should().NotThrow("Concurrent registration of type handlers in Dapper must be thread-safe");
    }
}





