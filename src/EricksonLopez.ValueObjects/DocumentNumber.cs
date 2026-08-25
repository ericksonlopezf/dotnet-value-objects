// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a structured document number — the <b>primary identity</b> of a business document.
/// This is the number printed on the document itself (e.g., the invoice number, purchase order number).
/// Normalized to uppercase.
///
/// <para><b>Rules:</b> Required, 1–60 chars, uppercase alphanumeric with
/// <c>. _ / -</c> separators.</para>
/// <para><b>Used by:</b> E-Invoicing, ERP, Inventory, Manufacturing, Financial, Audit</para>
///
/// <para><b>When to use DocumentNumber vs ReferenceNumber vs ExternalReference:</b></para>
/// <list type="bullet">
///   <item><b>DocumentNumber</b>: The document's OWN identifier. E.g., invoice <c>FAC-2026-0001</c>,
///   purchase order <c>OC-2026-0042</c>, NCF sequence <c>B0100000001</c>.</item>
///   <item><b>ReferenceNumber</b>: A POINTER to another document or operation within the organization.
///   E.g., "this payment references invoice <c>FAC-2026-0001</c>", bank transfer <c>REF-987654</c>,
///   check number <c>CHK-001234</c>.</item>
///   <item><b>ExternalReference</b>: An identifier from a THIRD PARTY that the organization does not control.
///   E.g., supplier invoice number, DGII authorization code, bank transaction ID.</item>
/// </list>
/// </summary>
public sealed record DocumentNumber : StringValueObject<DocumentNumber>
{
    private DocumentNumber(string value) : base(value) { }

    /// <summary>
    /// Creates a new <see cref="DocumentNumber"/> instance after validating and normalizing the document number.
    /// </summary>
    /// <param name="value">The raw document number string.</param>
    /// <returns>A <see cref="Result{DocumentNumber}"/> containing the created instance or a validation error.</returns>
    public static Result<DocumentNumber> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(DocumentNumber), 1, 60,
            static n => new DocumentNumber(n),
            StringPipeline.NormalizeCode,
            StringPipeline.CodePattern,
            "Document number must start with an alphanumeric character and contain only " +
            "letters, digits, periods, underscores, slashes, or hyphens.");
    }
}



