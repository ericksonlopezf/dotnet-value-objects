// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents an internal reference number used to <b>correlate</b> related documents
/// or operations within the organization. Unlike <see cref="DocumentNumber"/> which is a document's
/// own identity, a ReferenceNumber points to another entity or process.
/// Normalized to uppercase.
///
/// <para><b>Rules:</b> Required, 1–80 chars, uppercase alphanumeric with
/// <c>. _ / -</c> separators.</para>
/// <para><b>Used by:</b> ERP, POS, E-Invoicing, Inventory, Manufacturing, CRM, Financial</para>
///
/// <para><b>Examples:</b></para>
/// <list type="bullet">
///   <item>A payment's <c>ReferenceNumber</c> = the invoice <see cref="DocumentNumber"/> it pays.</item>
///   <item>A bank transfer reference (e.g., <c>REF-987654</c>).</item>
///   <item>A check number (e.g., <c>CHK-001234</c>).</item>
///   <item>An ACH authorization code used to correlate a batch of payments.</item>
/// </list>
///
/// <para><b>Key distinction:</b>
/// <see cref="DocumentNumber"/> = "I am document X."
/// <c>ReferenceNumber</c> = "I reference document/operation X."
/// <see cref="ExternalReference"/> = "A third party calls me X."</para>
/// </summary>
public sealed record ReferenceNumber : StringValueObject<ReferenceNumber>
{
    private ReferenceNumber(string value) : base(value) { }

    /// <summary>
    /// Creates a new <see cref="ReferenceNumber"/> instance after validating and normalizing the reference number.
    /// </summary>
    /// <param name="value">The raw reference number string.</param>
    /// <returns>A <see cref="Result{ReferenceNumber}"/> containing the created instance or a validation error.</returns>
    public static Result<ReferenceNumber> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(ReferenceNumber), 1, 80,
            static n => new ReferenceNumber(n),
            StringPipeline.NormalizeCode,
            StringPipeline.CodePattern,
            "Reference number must start with an alphanumeric character and contain only " +
            "letters, digits, periods, underscores, slashes, or hyphens.");
    }
}



