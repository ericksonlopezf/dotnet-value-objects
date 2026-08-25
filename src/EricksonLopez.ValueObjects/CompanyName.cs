// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents the legal or commercial name of a company, organization, or institution.
///
/// <para><b>Rules:</b> Required, 2–180 chars, business name characters
/// (letters, digits, spaces, <c>&amp; . , ' ( ) / # @ + -</c>), whitespace collapsed.</para>
/// <para><b>Used by:</b> ERP, E-Invoicing, CRM, SaaS, Payroll, Property Management, Financial, Donations</para>
/// </summary>
public sealed record CompanyName : StringValueObject<CompanyName>
{
    private CompanyName(string value) : base(value) { }

    /// <summary>
    /// Creates a new <see cref="CompanyName"/> instance after validating and normalizing whitespace and punctuation.
    /// </summary>
    /// <param name="value">The raw company name string.</param>
    /// <returns>A <see cref="Result{CompanyName}"/> containing the created instance or a validation error.</returns>
    public static Result<CompanyName> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(CompanyName), 2, 180,
            static n => new CompanyName(n),
            StringPipeline.NormalizeBusinessName,
            StringPipeline.BusinessNamePattern,
            "Company name can contain letters, digits, spaces, and common business punctuation.");
    }
}


