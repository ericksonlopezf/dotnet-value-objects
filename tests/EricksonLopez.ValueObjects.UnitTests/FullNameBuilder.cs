// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Builder for constructing valid or invalid instances of <see cref="FullName"/> for testing.
/// </summary>
public sealed class FullNameBuilder
{
    private string? _firstName = "Juan";
    private string? _lastName = "Pérez";
    private string? _middleName = "Carlos";

    public FullNameBuilder WithFirstName(string? firstName)
    {
        _firstName = firstName;
        return this;
    }

    public FullNameBuilder WithLastName(string? lastName)
    {
        _lastName = lastName;
        return this;
    }

    public FullNameBuilder WithMiddleName(string? middleName)
    {
        _middleName = middleName;
        return this;
    }

    public FullNameBuilder WithoutMiddleName()
    {
        _middleName = null;
        return this;
    }

    public FullName Build()
    {
        var result = BuildResult();
        if (result.IsFailure)
        {
            throw new InvalidOperationException($"Cannot build FullName. Validation failed: {result.Error.Code} - {result.Error.Description}");
        }

        return result.Value;
    }

    public Result<FullName> BuildResult()
    {
        return FullName.Create(_firstName, _lastName, _middleName);
    }
}
