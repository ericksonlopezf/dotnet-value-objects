// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a unique supplier, vendor, or contractor master data code.
/// Normalized to uppercase.
///
/// <para><b>Rules:</b> Required, 1–40 characters, uppercase alphanumeric with separators (<c>. _ / -</c>).</para>
/// <para><b>Used by:</b> ERP, Purchasing, Accounts Payable, Inventory, Logistics</para>
/// </summary>
public sealed record SupplierCode : StringValueObject<SupplierCode>
{
    private SupplierCode(string value) : base(value) { }

    /// <summary>
    /// Creates a new <see cref="SupplierCode"/> instance after validating and normalizing to uppercase.
    /// </summary>
    /// <param name="value">The raw supplier code string.</param>
    /// <returns>A <see cref="Result{SupplierCode}"/> containing the created instance or a validation error.</returns>
    public static Result<SupplierCode> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(SupplierCode), 1, 40,
            static n => new SupplierCode(n), StringPipeline.NormalizeCode,
            StringPipeline.CodePattern,
            "Supplier code must contain uppercase alphanumeric characters and standard separators.");
    }
}


