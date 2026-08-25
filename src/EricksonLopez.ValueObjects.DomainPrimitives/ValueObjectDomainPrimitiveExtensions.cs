// Copyright © Erickson Lopez. MIT License.
using System;
using System.Diagnostics.CodeAnalysis;
using EricksonLopez.DomainPrimitives;
using EricksonLopez.DomainPrimitives.Validation;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.DomainPrimitives;

/// <summary>
/// Provides extension methods for converting <see cref="SingleValueObject{TSelf, TValue}"/> instances to <see cref="IDomainPrimitive{TSelf, TValue}"/>.
/// </summary>
public static class ValueObjectDomainPrimitiveExtensions
{
    /// <summary>
    /// Attempts to instantiate a domain primitive from the raw value of a single-value value object.
    /// </summary>
    /// <typeparam name="TSelf">The concrete value object type.</typeparam>
    /// <typeparam name="TValue">The underlying value type.</typeparam>
    /// <typeparam name="TPrimitive">The target domain primitive type.</typeparam>
    /// <param name="valueObject">The source value object to convert.</param>
    /// <returns>A <see cref="Result{TPrimitive}"/> containing the created primitive or a validation error.</returns>
    public static Result<TPrimitive> ToDomainPrimitive<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TSelf, TValue, TPrimitive>(
        this SingleValueObject<TSelf, TValue> valueObject)
        where TSelf : SingleValueObject<TSelf, TValue>
        where TValue : notnull
        where TPrimitive : IDomainPrimitive<TPrimitive, TValue>
    {
        ArgumentNullException.ThrowIfNull(valueObject);

        if (TPrimitive.TryCreate(valueObject.Value, out var primitive, out var validationError))
        {
            return Result<TPrimitive>.Success(primitive);
        }

        var error = validationError.ToError() ?? Error.Validation("DomainPrimitive.Invalid", "Invalid domain primitive value.");
        return Result<TPrimitive>.Failure(error);
    }

    /// <summary>
    /// Converts a strongly-typed ID value object to an <see cref="IStrongId{TSelf, TValue}"/>.
    /// </summary>
    /// <typeparam name="TSelf">The concrete value object type.</typeparam>
    /// <typeparam name="TValue">The underlying ID value type.</typeparam>
    /// <typeparam name="TStrongId">The target strong ID type.</typeparam>
    /// <param name="valueObject">The source value object to convert.</param>
    /// <returns>A <see cref="Result{TStrongId}"/> containing the created strong ID or a validation error.</returns>
    public static Result<TStrongId> ToStrongId<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TSelf, TValue, TStrongId>(
        this SingleValueObject<TSelf, TValue> valueObject)
        where TSelf : SingleValueObject<TSelf, TValue>
        where TValue : notnull
        where TStrongId : IStrongId<TStrongId, TValue>
    {
        ArgumentNullException.ThrowIfNull(valueObject);

        if (TStrongId.TryCreate(valueObject.Value, out var strongId, out var validationError))
        {
            return Result<TStrongId>.Success(strongId);
        }

        var error = validationError.ToError() ?? Error.Validation("StrongId.Invalid", "Invalid strong ID value.");
        return Result<TStrongId>.Failure(error);
    }
}
