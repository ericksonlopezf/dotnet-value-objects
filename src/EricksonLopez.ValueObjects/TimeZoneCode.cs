// Copyright © Erickson Lopez. MIT License.
using System;
using System.Text.RegularExpressions;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents an IANA or Windows time zone identifier.
/// </summary>
public sealed partial record TimeZoneCode : StringValueObject<TimeZoneCode>
{
    [GeneratedRegex(@"^[A-Za-z0-9][A-Za-z0-9/_+ -]{1,119}$")]
    private static partial Regex TimeZonePattern();

    private TimeZoneCode(string value) : base(value) { }

    /// <summary>
    /// Creates a validated <see cref="TimeZoneCode"/> instance after normalizing whitespace.
    /// </summary>
    /// <param name="value">The time zone identifier string.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the validated time zone code, or a validation failure.</returns>
    public static Result<TimeZoneCode> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(TimeZoneCode), 2, 120,
            static n => new TimeZoneCode(n), StringPipeline.CollapseWhitespace,
            TimeZonePattern(),
            "Time zone must be a valid IANA or Windows time zone identifier.");
    }
}


