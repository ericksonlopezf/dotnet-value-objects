// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents the identity of the actor who created an entity or record.
/// Normalized to uppercase.
///
/// <para><b>Rules:</b> Required, 1–120 characters, uppercase alphanumeric or human/system identifier characters. Stored uppercase.</para>
/// <para><b>Used by:</b> All bounded contexts (Audit trail, Multi-tenant compliance, Event Sourcing)</para>
/// </summary>
public sealed record CreatedBy : StringValueObject<CreatedBy>
{
    private CreatedBy(string value) : base(value) { }

    /// <summary>
    /// Creates a new <see cref="CreatedBy"/> instance after validating and normalizing the actor identifier.
    /// </summary>
    /// <param name="value">The raw actor identifier string.</param>
    /// <returns>A <see cref="Result{CreatedBy}"/> containing the created instance or a validation error.</returns>
    public static Result<CreatedBy> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(CreatedBy), 1, 120,
            static n => new CreatedBy(n), StringPipeline.NormalizeCode,
            StringPipeline.LooseIdentifierPattern);
    }
}


