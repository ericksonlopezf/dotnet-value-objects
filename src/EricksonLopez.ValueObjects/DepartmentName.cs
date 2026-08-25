// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents the name of a business department, division, or organizational unit.
/// Normalized by collapsing excessive whitespace.
///
/// <para><b>Rules:</b> Required, 2–120 characters, business name characters.</para>
/// <para><b>Used by:</b> Human Resources, Payroll, Organization Structure, ERP, Budgeting</para>
/// </summary>
public sealed record DepartmentName : StringValueObject<DepartmentName>
{
    private DepartmentName(string value) : base(value) { }

    /// <summary>
    /// Creates a new <see cref="DepartmentName"/> instance after validating and collapsing whitespace.
    /// </summary>
    /// <param name="value">The raw department name string.</param>
    /// <returns>A <see cref="Result{DepartmentName}"/> containing the created instance or a validation error.</returns>
    public static Result<DepartmentName> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(DepartmentName), 2, 120,
            static n => new DepartmentName(n), StringPipeline.NormalizeBusinessName,
            StringPipeline.BusinessNamePattern,
            "Department name must contain valid business name characters.");
    }
}


