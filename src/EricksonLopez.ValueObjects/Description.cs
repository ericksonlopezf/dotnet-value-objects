// Copyright © Erickson Lopez. MIT License.
using System;
using System.Threading;
using System.Threading.Tasks;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Generic description Value Object for entities across all bounded contexts.
/// Suitable for product descriptions, task descriptions, meeting summaries, etc.
/// Accepts any printable content; whitespace is collapsed.
///
/// <para><b>Rules:</b> Required, 1–1000 chars, whitespace collapsed, no control characters.</para>
/// <para><b>Used by:</b> ERP, POS, E-Invoicing, Payroll, Inventory, Manufacturing, CRM, HR,
/// Property Management, Tasks, Financial, Donations, SaaS</para>
/// </summary>
public sealed record Description : StringValueObject<Description>
{
    private Description(string value) : base(value) { }

    /// <summary>
    /// Creates a new <see cref="Description"/> instance after validating and collapsing whitespace.
    /// </summary>
    /// <param name="value">The raw description string.</param>
    /// <returns>A <see cref="Result{Description}"/> containing the created instance or a validation error.</returns>
    public static Result<Description> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(Description), 1, 1000,
            static n => new Description(n),
            StringPipeline.CollapseWhitespace);
    }
}





