// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a reference code assigned by an external system (e.g., third-party API ID,
/// payment gateway reference, bank transaction ID, integration correlation key).
///
/// <para><b>Case is preserved as-is.</b> External systems may be case-sensitive; normalizing
/// to uppercase would destroy information and break reconciliation with those systems.</para>
///
/// <para><b>Rules:</b> Required, 1–200 chars, leading/trailing whitespace trimmed, no control
/// characters. No pattern restriction — external references may be UUIDs, tokens,
/// numeric IDs, base64 strings, or any format defined by the third party.</para>
/// <para><b>Used by:</b> ERP, FE, CRM, SaaS, Treasury</para>
/// </summary>
public sealed record ExternalReference : StringValueObject<ExternalReference>
{
    private ExternalReference(string value) : base(value) { }

    /// <summary>
    /// Creates a new <see cref="ExternalReference"/> instance after trimming leading and trailing whitespace.
    /// </summary>
    /// <param name="value">The raw external reference string.</param>
    /// <returns>A <see cref="Result{ExternalReference}"/> containing the created instance or a validation error.</returns>
    public static Result<ExternalReference> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(ExternalReference), 1, 200,
            static n => new ExternalReference(n),
            static raw => raw.Trim());
    }
}


