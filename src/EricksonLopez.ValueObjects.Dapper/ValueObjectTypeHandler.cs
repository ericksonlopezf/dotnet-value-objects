// Copyright © Erickson Lopez. MIT License.
using System;
using Dapper;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Dapper;

/// <summary>
/// Provides methods for registering Dapper type handlers for Value Objects.
/// </summary>
public static class ValueObjectTypeHandler
{
    /// <summary>
    /// Registers a Dapper type handler for a <see cref="SingleValueObject{TSelf, TValue}"/> type.
    /// </summary>
    /// <typeparam name="TVO">The Value Object type.</typeparam>
    /// <typeparam name="TPrimitive">The underlying primitive type.</typeparam>
    /// <param name="factory">The factory method to create instances of the Value Object.</param>
    public static void Register<TVO, TPrimitive>(Func<TPrimitive, Result<TVO>> factory)
        where TVO : SingleValueObject<TVO, TPrimitive>
        where TPrimitive : notnull
    {
        SqlMapper.AddTypeHandler(new SingleValueObjectTypeHandler<TVO, TPrimitive>(factory));
    }

    /// <summary>
    /// Registers a Dapper type handler for a struct-based Value Object type.
    /// </summary>
    /// <typeparam name="TVO">The struct Value Object type.</typeparam>
    /// <typeparam name="TPrimitive">The underlying primitive type.</typeparam>
    /// <param name="factory">The factory method to create instances of the Value Object.</param>
    /// <param name="valueSelector">The delegate extracting the primitive value from the Value Object struct.</param>
    public static void RegisterStruct<TVO, TPrimitive>(Func<TPrimitive, Result<TVO>> factory, Func<TVO, TPrimitive> valueSelector)
        where TVO : struct, IValueObject
        where TPrimitive : notnull
    {
        SqlMapper.AddTypeHandler(new StructValueObjectTypeHandler<TVO, TPrimitive>(factory, valueSelector));
    }
}

