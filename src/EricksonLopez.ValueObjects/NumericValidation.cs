// Copyright © Erickson Lopez. MIT License.
namespace EricksonLopez.ValueObjects;

/// <summary>
/// Provides validation routines for numeric value objects.
/// </summary>
internal static class NumericValidation
{
    /// <summary>
    /// Determines whether the specified decimal value has at most the permitted decimal scale.
    /// </summary>
    /// <param name="value">The decimal value to evaluate.</param>
    /// <param name="maxScale">The maximum allowable number of decimal places.</param>
    /// <returns><see langword="true"/> if the scale of <paramref name="value"/> is less than or equal to <paramref name="maxScale"/>; otherwise, <see langword="false"/>.</returns>
    public static bool IsScaleAtMost(decimal value, int maxScale) =>
        value.Scale <= maxScale;
}
