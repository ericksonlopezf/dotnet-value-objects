// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.EntityFrameworkCore;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using EricksonLopez.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Provides an Entity Framework Core <see cref="ValueConverter{TModel, TProvider}"/> for <see cref="SingleValueObject{TSelf, TValue}"/> types.
/// </summary>
/// <typeparam name="TVO">The concrete SingleValueObject type.</typeparam>
/// <typeparam name="TValue">The underlying database primitive type.</typeparam>
public class SingleValueObjectValueConverter<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicConstructors)] TVO, TValue> : ValueConverter<TVO, TValue>
    where TVO : SingleValueObject<TVO, TValue>
    where TValue : notnull
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SingleValueObjectValueConverter{TVO, TValue}"/> class using default factory resolution.
    /// </summary>
    public SingleValueObjectValueConverter()
        : this(CreateDefaultFactory())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SingleValueObjectValueConverter{TVO, TValue}"/> class with a custom factory delegate.
    /// </summary>
    /// <param name="factory">The delegate used to instantiate the value object from its underlying primitive value.</param>
    /// <exception cref="ArgumentNullException"><paramref name="factory"/> is <see langword="null"/></exception>
    public SingleValueObjectValueConverter(Func<TValue, TVO> factory)
        : base(
            v => v.Value,
            p => factory(p))
    {
        ArgumentNullException.ThrowIfNull(factory);
    }

    private static Func<TValue, TVO> CreateDefaultFactory()
    {
        var method = typeof(TVO).GetMethod("Create", BindingFlags.Public | BindingFlags.Static, [typeof(TValue)]);
        if (method != null)
        {
            return val =>
            {
                var res = method.Invoke(null, [val]);
                if (res is Result<TVO> r)
                {
                    if (r.IsSuccess) return r.Value;
                    throw new InvalidOperationException($"Cannot convert '{val}' to '{typeof(TVO).Name}': {r.Error.Description}");
                }
                if (res is TVO direct) return direct;
                throw new InvalidOperationException($"Unexpected result from factory method 'Create' on '{typeof(TVO).Name}'.");
            };
        }

        var ctor = typeof(TVO).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, [typeof(TValue)], null);
        if (ctor != null)
        {
            return val => (TVO)ctor.Invoke([val]);
        }

        throw new InvalidOperationException($"No 'Create({typeof(TValue).Name})' method or constructor found on '{typeof(TVO).FullName}'.");
    }
}



