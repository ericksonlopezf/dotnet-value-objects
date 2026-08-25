// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a manufacturing batch or production lot identifier for traceability.
/// Normalized to uppercase.
///
/// <para><b>Rules:</b> Required, 1–80 characters, uppercase alphanumeric with separators (<c>. _ / -</c>).</para>
/// <para><b>Used by:</b> Manufacturing, Quality Control, Inventory, Food &amp; Beverage, Pharmaceuticals</para>
/// </summary>
public sealed record BatchNumber : StringValueObject<BatchNumber>
{
    private BatchNumber(string value) : base(value) { }

    /// <summary>
    /// Creates a new <see cref="BatchNumber"/> instance after validating and normalizing to uppercase.
    /// </summary>
    /// <param name="value">The raw batch/lot identifier string.</param>
    /// <returns>A <see cref="Result{BatchNumber}"/> containing the created instance or a validation error.</returns>
    public static Result<BatchNumber> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(BatchNumber), 1, 80,
            static n => new BatchNumber(n), StringPipeline.NormalizeCode,
            StringPipeline.CodePattern,
            "Batch number must contain uppercase alphanumeric characters and standard separators.");
    }
}


