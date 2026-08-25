// Copyright © Erickson Lopez. MIT License.
using System;
using System.Text;
using System.Text.RegularExpressions;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Centralized validation and normalization pipeline for string-based Value Objects.
/// Ensures consistent behavior across all VOs: control character rejection,
/// whitespace normalization, length validation, and pattern matching.
///
/// <para>This class is internal to prevent consumers from bypassing VO factories.</para>
///
/// <para><b>Numeric validation</b> (decimal scale checks) lives in <see cref="NumericValidation"/>.</para>
/// </summary>
internal static partial class StringPipeline
{
    [GeneratedRegex(@"^[\p{L}\p{M}' .-]+$", RegexOptions.CultureInvariant)]
    private static partial Regex HumanNameRegex();

    [GeneratedRegex(@"^[\p{L}\p{M}\p{N}&.,'()/#@+ -]+$", RegexOptions.CultureInvariant)]
    private static partial Regex BusinessNameRegex();

    [GeneratedRegex(@"^[A-Z0-9][A-Z0-9._/-]*$", RegexOptions.CultureInvariant)]
    private static partial Regex CodeRegex();

    [GeneratedRegex(@"^[A-Z0-9][A-Z0-9 ._/-]*$", RegexOptions.CultureInvariant)]
    private static partial Regex LooseIdentifierRegex();

    /// <summary>Gets the regular expression pattern for human names (letters, marks, apostrophes, spaces, periods, hyphens).</summary>
    public static Regex HumanNamePattern => HumanNameRegex();

    /// <summary>Gets the regular expression pattern for business names (letters, marks, digits, common punctuation).</summary>
    public static Regex BusinessNamePattern => BusinessNameRegex();

    /// <summary>Gets the regular expression pattern for strict code formats (alphanumeric, periods, underscores, slashes, hyphens).</summary>
    public static Regex CodePattern => CodeRegex();

    /// <summary>Gets the regular expression pattern for loose identifiers (alphanumeric, spaces, periods, underscores, slashes, hyphens).</summary>
    public static Regex LooseIdentifierPattern => LooseIdentifierRegex();

    /// <summary>
    /// Validates and constructs a value object through the standard normalization and validation pipeline.
    /// </summary>
    /// <typeparam name="T">The type of value object to construct upon successful validation.</typeparam>
    /// <param name="value">The raw input string to validate.</param>
    /// <param name="fieldName">The name of the field used for error reporting.</param>
    /// <param name="minLength">The minimum required character length.</param>
    /// <param name="maxLength">The maximum permitted character length.</param>
    /// <param name="factory">The factory function to instantiate the target value object.</param>
    /// <param name="normalize">The optional normalization function applied before length and pattern checks.</param>
    /// <param name="pattern">The optional regular expression pattern that the normalized value must match.</param>
    /// <param name="patternMessage">The custom error message to use when pattern matching fails.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the created value object, or a validation failure.</returns>
    public static Result<T> Required<T>(
        string? value,
        string fieldName,
        int minLength,
        int maxLength,
        Func<string, T> factory,
        Func<string, string>? normalize = null,
        Regex? pattern = null,
        string? patternMessage = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result<T>.Failure(Error.Validation(
                $"{fieldName}.Required",
                $"{fieldName} is required."));
        }

        string normalized = normalize is null ? value.Trim() : normalize(value);

        if (ContainsControlCharacters(normalized))
        {
            return Result<T>.Failure(Error.Validation(
                $"{fieldName}.ControlCharacters",
                $"{fieldName} cannot contain control characters."));
        }

        if (normalized.Length < minLength)
        {
            return Result<T>.Failure(Error.Validation(
                $"{fieldName}.TooShort",
                $"{fieldName} must contain at least {minLength} characters."));
        }

        if (normalized.Length > maxLength)
        {
            return Result<T>.Failure(Error.Validation(
                $"{fieldName}.TooLong",
                $"{fieldName} must contain at most {maxLength} characters."));
        }

        if (pattern is not null && !pattern.IsMatch(normalized))
        {
            return Result<T>.Failure(Error.Validation(
                $"{fieldName}.InvalidFormat",
                patternMessage ?? $"{fieldName} has an invalid format."));
        }

        return Result<T>.Success(factory(normalized));
    }

    /// <summary>
    /// Validates and returns the normalized string using standard pipeline rules.
    /// </summary>
    /// <param name="value">The raw input string to validate.</param>
    /// <param name="fieldName">The name of the field used for error reporting.</param>
    /// <param name="minLength">The minimum required character length.</param>
    /// <param name="maxLength">The maximum permitted character length.</param>
    /// <param name="normalize">The optional normalization function applied before length and pattern checks.</param>
    /// <param name="pattern">The optional regular expression pattern that the normalized value must match.</param>
    /// <param name="patternMessage">The custom error message to use when pattern matching fails.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the normalized string, or a validation failure.</returns>
    public static Result<string> RequiredString(
        string? value,
        string fieldName,
        int minLength,
        int maxLength,
        Func<string, string>? normalize = null,
        Regex? pattern = null,
        string? patternMessage = null)
    {
        return Required(value, fieldName, minLength, maxLength,
            static v => v, normalize, pattern, patternMessage);
    }

    /// <summary>
    /// Trims and collapses multiple whitespace characters into a single space.
    /// </summary>
    /// <param name="value">The raw string to process.</param>
    /// <returns>The string with collapsed whitespace.</returns>
    public static string CollapseWhitespace(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return string.Empty;

        var builder = new StringBuilder(value.Length);
        bool hasPendingSpace = false;

        for (int i = 0; i < value.Length; i++)
        {
            char c = value[i];
            if (char.IsWhiteSpace(c))
            {
                if (builder.Length > 0)
                {
                    hasPendingSpace = true;
                }
            }
            else
            {
                if (hasPendingSpace)
                {
                    builder.Append(' ');
                    hasPendingSpace = false;
                }

                builder.Append(c);
            }
        }

        return builder.ToString();
    }

    /// <summary>
    /// Normalizes a human name by trimming and collapsing multiple spaces into a single space.
    /// </summary>
    /// <param name="value">The raw human name string to normalize.</param>
    /// <returns>The normalized human name string.</returns>
    public static string NormalizeHumanName(string value) => CollapseWhitespace(value);

    /// <summary>
    /// Normalizes a business name by trimming and collapsing multiple spaces into a single space.
    /// </summary>
    /// <param name="value">The raw business name string to normalize.</param>
    /// <returns>The normalized business name string.</returns>
    public static string NormalizeBusinessName(string value) => CollapseWhitespace(value);

    /// <summary>
    /// Trims and converts the string to uppercase invariant.
    /// </summary>
    /// <param name="value">The raw string to normalize.</param>
    /// <returns>The trimmed uppercase string.</returns>
    public static string NormalizeTrimUpper(string value) => value.Trim().ToUpperInvariant();

    /// <summary>
    /// Collapses whitespace and converts the code to uppercase invariant.
    /// </summary>
    /// <param name="value">The raw code string to normalize.</param>
    /// <returns>The normalized uppercase code string.</returns>
    public static string NormalizeCode(string value) => CollapseWhitespace(value).ToUpperInvariant();

    /// <summary>
    /// Trims and converts the string to lowercase invariant.
    /// </summary>
    /// <param name="value">The raw string to normalize.</param>
    /// <returns>The trimmed lowercase string.</returns>
    public static string NormalizeLower(string value) => value.Trim().ToLowerInvariant();

    /// <summary>
    /// Determines whether the string contains any Unicode control characters.
    /// </summary>
    /// <param name="value">The string to inspect.</param>
    /// <returns><see langword="true"/> if the string contains control characters; otherwise, <see langword="false"/>.</returns>
    public static bool ContainsControlCharacters(string value)
    {
        foreach (char character in value)
        {
            if (char.IsControl(character))
            {
                return true;
            }
        }

        return false;
    }
}

