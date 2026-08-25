// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents the identity of the actor who marked an entity as deleted (soft delete).
/// Normalized to uppercase.
///
/// <para><b>Rules:</b> Required, 1–120 characters, uppercase alphanumeric or human/system identifier characters. Stored uppercase.</para>
/// <para><b>Used by:</b> All bounded contexts (Soft delete audit trail)</para>
/// </summary>
public sealed record DeletedBy : StringValueObject<DeletedBy>
{
    private DeletedBy(string value) : base(value) { }

    /// <summary>
    /// Creates a new <see cref="DeletedBy"/> instance after validating and normalizing the actor identifier.
    /// </summary>
    /// <param name="value">The raw actor identifier string.</param>
    /// <returns>A <see cref="Result{DeletedBy}"/> containing the created instance or a validation error.</returns>
    public static Result<DeletedBy> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(DeletedBy), 1, 120,
            static n => new DeletedBy(n), StringPipeline.NormalizeCode,
            StringPipeline.LooseIdentifierPattern);
    }
}


