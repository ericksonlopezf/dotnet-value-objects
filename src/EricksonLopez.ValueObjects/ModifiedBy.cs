// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents the identity of the actor who last modified an entity or record.
/// Normalized to uppercase.
///
/// <para><b>Rules:</b> Required, 1–120 characters, uppercase alphanumeric or human/system identifier characters. Stored uppercase.</para>
/// <para><b>Used by:</b> All bounded contexts (Audit trail, Multi-tenant compliance)</para>
/// </summary>
public sealed record ModifiedBy : StringValueObject<ModifiedBy>
{
    private ModifiedBy(string value) : base(value) { }

    /// <summary>
    /// Creates a new <see cref="ModifiedBy"/> instance after validating and normalizing the actor identifier.
    /// </summary>
    /// <param name="value">The raw actor identifier string.</param>
    /// <returns>A <see cref="Result{ModifiedBy}"/> containing the created instance or a validation error.</returns>
    public static Result<ModifiedBy> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(ModifiedBy), 1, 120,
            static n => new ModifiedBy(n), StringPipeline.NormalizeCode,
            StringPipeline.LooseIdentifierPattern);
    }
}


