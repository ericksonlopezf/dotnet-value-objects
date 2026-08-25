// Copyright © Erickson Lopez. MIT License.
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a software license key.
/// Format: grouped uppercase alphanumeric segments separated by hyphens (e.g., <c>AAAA-BBBB-CCCC</c>).
///
/// <para><b>Rules:</b> 2–9 groups, each 4–8 uppercase alphanumeric characters, separated by hyphens.</para>
/// <para><b>Used by:</b> SaaS BC — licensing only. Not a shared kernel VO.</para>
///
/// <para><b>Security note:</b> Use <see cref="Masked"/> when logging or displaying license keys
/// to avoid exposing the full key in logs or UIs.</para>
/// </summary>
[SensitiveData(mask: "XXXX-XXXX-XXXX")]
[DebuggerDisplay("{" + nameof(ToString) + "()}")]
public sealed partial record LicenseKey : StringValueObject<LicenseKey>
{
    [GeneratedRegex(@"^[A-Z0-9]{4,8}(-[A-Z0-9]{4,8}){2,8}$")]
    private static partial Regex LicenseKeyPattern();

    private LicenseKey(string value) : base(value) { }

    /// <summary>
    /// Creates a new <see cref="LicenseKey"/> instance after validating format and normalizing to uppercase.
    /// </summary>
    /// <param name="value">The raw license key string.</param>
    /// <returns>A <see cref="Result{LicenseKey}"/> containing the created instance or a validation error.</returns>
    public static Result<LicenseKey> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(LicenseKey), 14, 80,
            static n => new LicenseKey(n),
            static raw => raw.Trim().ToUpperInvariant(),
            LicenseKeyPattern(),
            "License key must use grouped uppercase letters and digits separated by hyphens.");
    }

    /// <summary>
    /// Returns a masked version of the license key showing only the last segment group.
    /// </summary>
    /// <returns>The masked license key string.</returns>
    public string Masked()
    {
        int lastDash = Value.LastIndexOf('-');
        return string.Concat("XXXX-XXXX-", Value.AsSpan(lastDash + 1));
    }

    /// <inheritdoc/>
    protected override string ToStringCore() => Masked();
}



