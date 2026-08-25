// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a unique employee, staff, or payroll identifier code.
/// Normalized to uppercase.
///
/// <para><b>Rules:</b> Required, 1–40 characters, uppercase alphanumeric with separators (<c>. _ / -</c>).</para>
/// <para><b>Used by:</b> Human Resources, Payroll, Time &amp; Attendance, ERP, Identity</para>
/// </summary>
public sealed record EmployeeCode : StringValueObject<EmployeeCode>
{
    private EmployeeCode(string value) : base(value) { }

    /// <summary>
    /// Creates a new <see cref="EmployeeCode"/> instance after validating and normalizing to uppercase.
    /// </summary>
    /// <param name="value">The raw employee code string.</param>
    /// <returns>A <see cref="Result{EmployeeCode}"/> containing the created instance or a validation error.</returns>
    public static Result<EmployeeCode> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(EmployeeCode), 1, 40,
            static n => new EmployeeCode(n), StringPipeline.NormalizeCode,
            StringPipeline.CodePattern,
            "Employee code must contain uppercase alphanumeric characters and standard separators.");
    }
}


