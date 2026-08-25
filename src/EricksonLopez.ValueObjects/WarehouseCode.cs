// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a warehouse, storage location, or distribution center identifier.
/// Normalized to uppercase.
///
/// <para><b>Rules:</b> Required, 1–40 characters, uppercase alphanumeric with separators (<c>. _ / -</c>).</para>
/// <para><b>Used by:</b> Inventory, Logistics, Supply Chain, ERP, POS</para>
/// </summary>
public sealed record WarehouseCode : StringValueObject<WarehouseCode>
{
    private WarehouseCode(string value) : base(value) { }

    /// <summary>
    /// Creates a new <see cref="WarehouseCode"/> instance after validating and normalizing to uppercase.
    /// </summary>
    /// <param name="value">The raw warehouse code string.</param>
    /// <returns>A <see cref="Result{WarehouseCode}"/> containing the created instance or a validation error.</returns>
    public static Result<WarehouseCode> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(WarehouseCode), 1, 40,
            static n => new WarehouseCode(n), StringPipeline.NormalizeCode,
            StringPipeline.CodePattern,
            "Warehouse code must contain uppercase alphanumeric characters and standard separators.");
    }
}


