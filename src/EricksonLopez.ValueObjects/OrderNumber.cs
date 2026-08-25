// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a commercial order identifier (sales order, purchase order, service order).
/// Normalized to uppercase.
///
/// <para><b>Rules:</b> Required, 1–60 characters, uppercase alphanumeric with separators (<c>. _ / -</c>).</para>
/// <para><b>Used by:</b> ERP, POS, E-commerce, Procurement, Electronic Invoicing</para>
/// </summary>
public sealed record OrderNumber : StringValueObject<OrderNumber>
{
    private OrderNumber(string value) : base(value) { }

    /// <summary>
    /// Creates a new <see cref="OrderNumber"/> instance after validating and normalizing to uppercase.
    /// </summary>
    /// <param name="value">The raw order number string.</param>
    /// <returns>A <see cref="Result{OrderNumber}"/> containing the created instance or a validation error.</returns>
    public static Result<OrderNumber> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(OrderNumber), 1, 60,
            static n => new OrderNumber(n), StringPipeline.NormalizeCode,
            StringPipeline.CodePattern,
            "Order number must contain uppercase alphanumeric characters and standard separators.");
    }
}


