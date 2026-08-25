// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.DomainPrimitives.Validation;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.DomainPrimitives;

/// <summary>
/// Provides extension methods for converting between <see cref="PrimitiveError"/> and <see cref="Error"/>.
/// </summary>
public static class DomainPrimitiveErrorExtensions
{
    /// <summary>
    /// Converts a <see cref="PrimitiveError"/> into an <see cref="Error"/>, or returns <see langword="null"/> if <paramref name="primitiveError"/> represents <see cref="PrimitiveError.None"/>.
    /// </summary>
    /// <param name="primitiveError">The primitive error to convert.</param>
    /// <returns>The corresponding <see cref="Error"/>, or <see langword="null"/> if no error occurred.</returns>
    public static Error? ToError(this PrimitiveError primitiveError)
    {
        if (!primitiveError.IsError)
        {
            return null;
        }

        return Error.Validation(
            primitiveError.Code!,
            primitiveError.Message ?? "A domain primitive validation error occurred.");
    }

    /// <summary>
    /// Converts an <see cref="Error"/> into a <see cref="PrimitiveError"/>.
    /// </summary>
    /// <param name="error">The error to convert, or <see langword="null"/> for success.</param>
    /// <returns>The corresponding <see cref="PrimitiveError"/>.</returns>
    public static PrimitiveError ToPrimitiveError(this Error? error)
    {
        if (error is null)
        {
            return PrimitiveError.None;
        }

        return PrimitiveError.Create(error.Code, error.Description);
    }
}
