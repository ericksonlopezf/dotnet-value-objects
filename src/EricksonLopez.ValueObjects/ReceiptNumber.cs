// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a payment or POS transaction receipt identifier.
/// Normalized to uppercase.
///
/// <para><b>Rules:</b> Required, 1–60 characters, uppercase alphanumeric with separators (<c>. _ / -</c>).</para>
/// <para><b>Used by:</b> POS, Treasury, Cash Register, ERP, Billing</para>
/// </summary>
public sealed record ReceiptNumber : StringValueObject<ReceiptNumber>
{
    private ReceiptNumber(string value) : base(value) { }

    /// <summary>
    /// Creates a new <see cref="ReceiptNumber"/> instance after validating and normalizing to uppercase.
    /// </summary>
    /// <param name="value">The raw receipt identifier string.</param>
    /// <returns>A <see cref="Result{ReceiptNumber}"/> containing the created instance or a validation error.</returns>
    public static Result<ReceiptNumber> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(ReceiptNumber), 1, 60,
            static n => new ReceiptNumber(n), StringPipeline.NormalizeCode,
            StringPipeline.CodePattern,
            "Receipt number must contain uppercase alphanumeric characters and standard separators.");
    }
}


