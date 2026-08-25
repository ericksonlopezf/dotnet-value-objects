// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a unique customer, client, or account master data code.
/// Normalized to uppercase.
///
/// <para><b>Rules:</b> Required, 1–40 characters, uppercase alphanumeric with separators (<c>. _ / -</c>).</para>
/// <para><b>Used by:</b> ERP, CRM, POS, Electronic Invoicing, Subscriptions</para>
/// </summary>
public sealed record CustomerCode : StringValueObject<CustomerCode>
{
    private CustomerCode(string value) : base(value) { }

    /// <summary>
    /// Creates a new <see cref="CustomerCode"/> instance after validating and normalizing to uppercase.
    /// </summary>
    /// <param name="value">The raw customer code string.</param>
    /// <returns>A <see cref="Result{CustomerCode}"/> containing the created instance or a validation error.</returns>
    public static Result<CustomerCode> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(CustomerCode), 1, 40,
            static n => new CustomerCode(n), StringPipeline.NormalizeCode,
            StringPipeline.CodePattern,
            "Customer code must contain uppercase alphanumeric characters and standard separators.");
    }
}


