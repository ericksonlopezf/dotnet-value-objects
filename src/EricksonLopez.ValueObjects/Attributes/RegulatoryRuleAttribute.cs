// Copyright © Erickson Lopez. MIT License.
namespace EricksonLopez.ValueObjects.Attributes;

using System;

/// <summary>
/// Backward-compatible alias for <see cref="EricksonLopez.ValueObjects.RegulatoryRuleAttribute"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public sealed class RegulatoryRuleAttribute : EricksonLopez.ValueObjects.RegulatoryRuleAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RegulatoryRuleAttribute"/> class.
    /// </summary>
    /// <param name="ruleId">The formal identifier of the regulatory rule.</param>
    public RegulatoryRuleAttribute(string ruleId) : base(ruleId)
    {
    }
}
