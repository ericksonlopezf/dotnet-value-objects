// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents an individual's composite full name consisting of first, optional middle, and last name.
/// </summary>
public sealed record FullName : ValueObject
{
    /// <summary>
    /// Gets the first name component.
    /// </summary>
    public FirstName FirstName { get; }

    /// <summary>
    /// Gets the optional middle name component.
    /// </summary>
    public MiddleName? MiddleName { get; }

    /// <summary>
    /// Gets the last name component.
    /// </summary>
    public LastName LastName { get; }

    /// <summary>
    /// Gets the complete formatted full name string.
    /// </summary>
    public string Value { get; }

    private FullName(FirstName firstName, MiddleName? middleName, LastName lastName)
    {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        Value = middleName is null
            ? $"{firstName.Value} {lastName.Value}"
            : $"{firstName.Value} {middleName.Value} {lastName.Value}";
    }

    /// <summary>
    /// Creates a <see cref="FullName"/> instance from strongly typed name components.
    /// </summary>
    /// <param name="firstName">The validated first name.</param>
    /// <param name="lastName">The validated last name.</param>
    /// <param name="middleName">The optional validated middle name.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the composite full name, or a validation failure.</returns>
    public static Result<FullName> Create(FirstName? firstName, LastName? lastName, MiddleName? middleName = null)
    {
        if (firstName is null)
        {
            return Result<FullName>.Failure(Error.Validation("FullName.FirstNameRequired", "First name is required."));
        }

        if (lastName is null)
        {
            return Result<FullName>.Failure(Error.Validation("FullName.LastNameRequired", "Last name is required."));
        }

        return Result<FullName>.Success(new FullName(firstName, middleName, lastName));
    }

    /// <summary>
    /// Creates a <see cref="FullName"/> instance from raw name strings, validating each component.
    /// </summary>
    /// <param name="firstName">The raw first name string.</param>
    /// <param name="lastName">The raw last name string.</param>
    /// <param name="middleName">The optional raw middle name string.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the composite full name, or a validation failure.</returns>
    public static Result<FullName> Create(string? firstName, string? lastName, string? middleName = null)
    {
        Result<FirstName> firstNameResult = FirstName.Create(firstName);
        if (firstNameResult.IsFailure)
        {
            return Result<FullName>.Failure(firstNameResult.Error);
        }

        Result<LastName> lastNameResult = LastName.Create(lastName);
        if (lastNameResult.IsFailure)
        {
            return Result<FullName>.Failure(lastNameResult.Error);
        }

        Result<MiddleName?> middleNameResult = MiddleName.CreateOptional(middleName);
        if (middleNameResult.IsFailure)
        {
            return Result<FullName>.Failure(middleNameResult.Error);
        }

        return Create(firstNameResult.Value, lastNameResult.Value, middleNameResult.Value);
    }

    /// <inheritdoc/>
    public override string ToString() => Value;
}


