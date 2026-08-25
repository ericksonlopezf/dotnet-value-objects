// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents an individual's middle name.
/// </summary>
public sealed record MiddleName : StringValueObject<MiddleName>
{
    private MiddleName(string value) : base(value) { }

    /// <summary>
    /// Creates a validated <see cref="MiddleName"/> instance after normalizing whitespace and unicode characters.
    /// </summary>
    /// <param name="value">The raw middle name string.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the validated middle name, or a validation failure.</returns>
    public static Result<MiddleName> Create(string? value)
    {
        return StringPipeline.Required(
            value,
            nameof(MiddleName),
            1,
            80,
            static normalized => new MiddleName(normalized),
            StringPipeline.NormalizeHumanName,
            StringPipeline.HumanNamePattern,
            "Middle name can contain letters, spaces, apostrophes, periods, or hyphens.");
    }

    /// <summary>
    /// Creates an optional <see cref="MiddleName"/> instance, returning <see langword="null"/> if empty or whitespace.
    /// </summary>
    /// <param name="value">The optional raw middle name string.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the optional middle name, or a validation failure if invalid.</returns>
    public static Result<MiddleName?> CreateOptional(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result<MiddleName?>.Success(null);
        }

        Result<MiddleName> result = Create(value);
        return result.IsFailure
            ? Result<MiddleName?>.Failure(result.Error)
            : Result<MiddleName?>.Success(result.Value);
    }
}


