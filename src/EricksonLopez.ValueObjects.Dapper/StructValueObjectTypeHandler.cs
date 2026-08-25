// Copyright © Erickson Lopez. MIT License.
using System;
using System.Data;
using System.Globalization;
using Dapper;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Dapper;

/// <summary>
/// Provides a Dapper type handler for struct-based Value Object types.
/// </summary>
/// <typeparam name="TVO">The Value Object struct type.</typeparam>
/// <typeparam name="TPrimitive">The underlying database primitive type.</typeparam>
public sealed class StructValueObjectTypeHandler<TVO, TPrimitive> : SqlMapper.TypeHandler<TVO>
    where TVO : struct, IValueObject
    where TPrimitive : notnull
{
    private readonly Func<TPrimitive, Result<TVO>> _factory;
    private readonly Func<TVO, TPrimitive> _valueSelector;

    /// <summary>
    /// Initializes a new instance of the <see cref="StructValueObjectTypeHandler{TVO, TPrimitive}"/> class.
    /// </summary>
    /// <param name="factory">The factory function that creates a Value Object struct from a primitive value.</param>
    /// <param name="valueSelector">The function that extracts the underlying primitive value from the Value Object struct.</param>
    public StructValueObjectTypeHandler(Func<TPrimitive, Result<TVO>> factory, Func<TVO, TPrimitive> valueSelector)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _valueSelector = valueSelector ?? throw new ArgumentNullException(nameof(valueSelector));
    }

    /// <inheritdoc/>
    public override void SetValue(IDbDataParameter parameter, TVO value)
    {
        ArgumentNullException.ThrowIfNull(parameter, nameof(parameter));
        parameter.Value = _valueSelector(value);
    }

    /// <inheritdoc/>
    public override TVO Parse(object value)
    {
        if (value is null || value is DBNull)
        {
            throw new DataException($"Cannot parse null database value into struct '{typeof(TVO).Name}'.");
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
