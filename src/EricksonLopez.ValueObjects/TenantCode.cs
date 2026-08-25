// Copyright © Erickson Lopez. MIT License.
using System;
using System.Text.RegularExpressions;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a tenant identifier in a multi-tenant SaaS system.
/// DNS-friendly: lowercase letters, digits, and hyphens only.
///
/// <para><b>Architectural note:</b> <c>TenantCode</c> is placed in <c>Shared/</c>
/// (not <c>SaaS/</c>) because it is used by every bounded context (13/13 systems)
/// as the primary multi-tenancy discriminator, making it a universal cross-cutting
/// concern rather than a SaaS-specific concept.</para>
///
/// <para><b>Rules:</b> 3–64 chars, DNS-friendly lowercase slug (letters, digits, hyphens),
/// must start and end with a letter or digit.</para>
/// <para><b>Used by:</b> All bounded contexts — universal tenancy discriminator</para>
/// </summary>
public sealed partial record TenantCode : StringValueObject<TenantCode>
{
    [GeneratedRegex(@"^[a-z0-9][a-z0-9-]{1,62}[a-z0-9]$")]
    private static partial Regex TenantCodePattern();

    private TenantCode(string value) : base(value) { }

    /// <summary>
    /// Creates a new <see cref="TenantCode"/> instance after validating DNS format and normalizing to lowercase.
    /// </summary>
    /// <param name="value">The raw tenant code string.</param>
    /// <returns>A <see cref="Result{TenantCode}"/> containing the created instance or a validation error.</returns>
    public static Result<TenantCode> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(TenantCode), 3, 64,
            static n => new TenantCode(n),
            static raw => raw.Trim().ToLowerInvariant(),
            TenantCodePattern(),
            "Tenant code must be DNS-friendly lowercase text using letters, digits, and hyphens.");
    }
}


