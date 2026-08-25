// Copyright © Erickson Lopez. MIT License.
using System;
using System.Data;
using System.Globalization;
using Dapper;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Dapper;

/// <summary>
/// Provides a Dapper type handler for <see cref="SingleValueObject{TSelf, TValue}"/> types.
/// </summary>
/// <typeparam name="TVO">The Value Object type.</typeparam>
/// <typeparam name="TPrimitive">The underlying database primitive type.</typeparam>
public sealed class SingleValueObjectTypeHandler<TVO, TPrimitive> : SqlMapper.TypeHandler<TVO>
    where TVO : SingleValueObject<TVO, TPrimitive>
    where TPrimitive : notnull
{
    private readonly Func<TPrimitive, Result<TVO>> _factory;

    /// <summary>
    /// Initializes a new instance of the <see cref="SingleValueObjectTypeHandler{TVO, TPrimitive}"/> class.
    /// </summary>
    /// <param name="factory">The factory function that creates a Value Object from a primitive value.</param>
    public SingleValueObjectTypeHandler(Func<TPrimitive, Result<TVO>> factory)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    /// <inheritdoc/>
    public override void SetValue(IDbDataParameter parameter, TVO? value)
    {
        ArgumentNullException.ThrowIfNull(parameter, nameof(parameter));

        if (value is null)
        {
            parameter.Value = DBNull.Value;
        }
        else
        {
            parameter.Value = value.Value;
        }
    }

    /// <inheritdoc/>
    public override TVO? Parse(object value)
    {
        if (value is null || value is DBNull)
        {
            return null;
        }

        if (value is TPrimitive primitive)
        {
            var result = _factory(primitive);
            if (result.IsFailure)
            {
                throw new DataException($"Failed to map database value '{value}' to '{typeof(TVO).Name}': {result.Error.Description}");
            }
            return result.Value;
        }

        var converted = (TPrimitive)Convert.ChangeType(value, typeof(TPrimitive), CultureInfo.InvariantCulture);
        var res = _factory(converted);
        if (res.IsFailure)
        {
            throw new DataException($"Failed to map database value '{value}' to '{typeof(TVO).Name}': {res.Error.Description}");
        }

        return res.Value;
    }
}

