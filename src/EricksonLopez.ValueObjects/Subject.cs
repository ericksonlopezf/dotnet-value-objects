// Copyright © Erickson Lopez. MIT License.
using System;
using System.Threading;
using System.Threading.Tasks;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a single-line subject, title, or topic for messages, tickets, and notifications.
/// Normalized by collapsing excessive whitespace.
///
/// <para><b>Rules:</b> Required, 1–250 characters, single-line text without control characters.</para>
/// <para><b>Used by:</b> CRM, Ticketing, Notifications, Email, Task Management</para>
/// </summary>
public sealed record Subject : StringValueObject<Subject>
{
    private Subject(string value) : base(value) { }

    /// <summary>
    /// Creates a new <see cref="Subject"/> instance after validating and collapsing whitespace.
    /// </summary>
    /// <param name="value">The raw subject line string.</param>
    /// <returns>A <see cref="Result{Subject}"/> containing the created instance or a validation error.</returns>
    public static Result<Subject> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(Subject), 1, 250,
            static n => new Subject(n), StringPipeline.CollapseWhitespace);
    }
}





