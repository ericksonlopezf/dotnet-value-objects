// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a Stock Keeping Unit — the unique product identifier for inventory tracking.
/// Normalized to uppercase.
///
/// <para><b>Rules:</b> Required, 1–64 characters, uppercase alphanumeric with <c>. _ / -</c> separators
/// (code pattern). Stored uppercase.</para>
/// <para><b>Used by:</b> Inventory, POS, ERP, Manufactura, CRM</para>
///
/// <para><b>Design note:</b> SKU is the canonical product-level identifier used by inventory
/// and purchasing systems. It differs from <see cref="Barcode"/>
/// (physical scan code) and <see cref="Code"/> (generic entity code).
/// Use SKU when referencing a product variant across supply chain, warehouse, and sales channels.</para>
///
/// <para><b>⚠️ Usage guidance:</b> <c>Name</c> and <c>Code</c> in the Shared namespace are intentionally
/// generic. When your domain concept is specifically a product SKU, use this VO to make intent explicit
/// and prevent confusion with entity codes from other domains.</para>
/// </summary>
public sealed record SKU : StringValueObject<SKU>
{
    private SKU(string value) : base(value) { }

    /// <summary>
    /// Creates a new <see cref="SKU"/> instance after validating and normalizing to uppercase.
    /// </summary>
    /// <param name="value">The raw stock keeping unit string.</param>
    /// <returns>A <see cref="Result{SKU}"/> containing the created instance or a validation error.</returns>
    public static Result<SKU> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(SKU), 1, 64,
            static n => new SKU(n), StringPipeline.NormalizeTrimUpper,
            StringPipeline.CodePattern);
    }
}


