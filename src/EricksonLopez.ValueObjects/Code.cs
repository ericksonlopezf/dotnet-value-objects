// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a generic entity code — an uppercase alphanumeric identifier with separators.
/// Normalized to uppercase.
///
/// <para><b>Rules:</b> Required, 1–60 characters, uppercase alphanumeric with <c>. _ / -</c> separators
/// (code pattern). Stored uppercase.</para>
/// <para><b>Used by:</b> ERP, Inventory, POS, SaaS, Property Management, Financial</para>
///
/// <para><b>⚠️ Usage guidance (read before using):</b> <c>Code</c> is a deliberately generic VO
/// for domain concepts where the ubiquitous language uses the word "code" without further specificity
/// (e.g., a department code, a category code, a warehouse location code).</para>
///
/// <para>If your domain concept has a more specific name, prefer a purpose-built VO:
/// <list type="bullet">
///   <item><see cref="SKU"/> — product stock-keeping unit</item>
///   <item><see cref="TenantCode"/> — multi-tenant identifier</item>
///   <item><see cref="DocumentNumber"/> — numbered document</item>
///   <item><see cref="ReferenceNumber"/> — internal reference</item>
/// </list>
/// Using <c>Code</c> when a more specific VO exists weakens the Ubiquitous Language.
/// Only use <c>Code</c> when the concept genuinely crosses multiple domains with no specific semantic.</para>
/// </summary>
public sealed record Code : StringValueObject<Code>
{
    private Code(string value) : base(value) { }

    /// <summary>
    /// Creates a new <see cref="Code"/> instance after validating and normalizing to uppercase.
    /// </summary>
    /// <param name="value">The raw entity code string representation.</param>
    /// <returns>A <see cref="Result{Code}"/> containing the created instance or a validation error.</returns>
    public static Result<Code> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(Code), 1, 60,
            static n => new Code(n), StringPipeline.NormalizeTrimUpper,
            StringPipeline.CodePattern);
    }
}



