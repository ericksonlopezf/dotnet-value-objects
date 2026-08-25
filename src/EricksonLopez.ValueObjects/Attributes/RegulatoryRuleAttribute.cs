// Copyright © Erickson Lopez. MIT License.
namespace EricksonLopez.ValueObjects.Attributes;

using System;

/// <summary>
/// Specifies the regulatory rule identifier that governs the invariants and algorithms of a value object.
/// </summary>
/// <remarks>
/// Evaluated by compliance verification gates to ensure domain rules trace back to official legal statutes or technical resolutions.
/// </remarks>
[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public sealed class RegulatoryRuleAttribute : Attribute
{
    /// <summary>
    /// Gets the formal identifier of the regulatory rule.
    /// </summary>
    public string RuleId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RegulatoryRuleAttribute"/> class with the specified rule identifier.
    /// </summary>
    /// <param name="ruleId">The formal identifier of the regulatory rule.</param>
    /// <exception cref="ArgumentException"><paramref name="ruleId"/> is <see langword="null"/> or whitespace</exception>
    public RegulatoryRuleAttribute(string ruleId)
    {
        if (string.IsNullOrWhiteSpace(ruleId))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(ruleId));
        }

        RuleId = ruleId;
    }
}
