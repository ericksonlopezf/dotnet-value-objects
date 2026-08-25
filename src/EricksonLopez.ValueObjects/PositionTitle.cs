// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a job title, job role, or organizational position.
/// Normalized by collapsing excessive whitespace.
///
/// <para><b>Rules:</b> Required, 2–120 characters, human and business title characters.</para>
/// <para><b>Used by:</b> Human Resources, Payroll, Organization Chart, ERP, CRM</para>
/// </summary>
public sealed record PositionTitle : StringValueObject<PositionTitle>
{
    private PositionTitle(string value) : base(value) { }

    /// <summary>
    /// Creates a new <see cref="PositionTitle"/> instance after validating and collapsing whitespace.
    /// </summary>
    /// <param name="value">The raw position or job title string.</param>
    /// <returns>A <see cref="Result{PositionTitle}"/> containing the created instance or a validation error.</returns>
    public static Result<PositionTitle> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(PositionTitle), 2, 120,
            static n => new PositionTitle(n), StringPipeline.NormalizeHumanName,
            StringPipeline.HumanNamePattern,
            "Position title must contain valid letters, spaces, and punctuation.");
    }
}


